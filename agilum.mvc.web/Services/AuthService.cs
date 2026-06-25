using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace agilum.mvc.web.Services
{
    /// <summary>
    /// Serviço de autenticação customizado que substitui o ASP.NET Core Identity.
    /// Autentica diretamente contra a entidade Usuario (tabela ca_usuarios),
    /// usando MD5 (legado Pascal) para validação de senha.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Valida login e senha contra a entidade Usuario.
        /// Retorna o Usuario se válido, null caso contrário.
        /// </summary>
        Task<Usuario> ValidarLogin(string login, string senha);

        /// <summary>
        /// Cria ClaimsPrincipal e faz sign-in via cookie.
        /// Opcionalmente armazena dados da empresa nas claims.
        /// </summary>
        Task SignInAsync(HttpContext httpContext, Usuario usuario, bool isPersistent,
            string idEmpresa = null, string nomeEmpresa = null);

        /// <summary>
        /// Faz sign-out (remove cookie de autenticação).
        /// </summary>
        Task SignOutAsync(HttpContext httpContext);

        /// <summary>
        /// Computa hash MD5 no formato legado (Pascal RetornaMD5).
        /// </summary>
        string ComputeMd5Hash(string input);
    }

    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioService _usuarioService;

        public AuthService(IUsuarioRepository usuarioRepository, IUsuarioService usuarioService)
        {
            _usuarioRepository = usuarioRepository;
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Valida credenciais: busca Usuario pelo campo 'usuario' (login)
        /// e compara senha via MD5 (formato legado Pascal).
        /// </summary>
        public async Task<Usuario> ValidarLogin(string login, string senha)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(senha))
                return null;

            var lista = await _usuarioRepository.ObterTodos();

            // Busca o usuário pelo campo 'usuario' (login) na tabela ca_usuarios
            var usuarios = await _usuarioRepository.Buscar(u => u.usuario == login);
            var usuario = usuarios.FirstOrDefault();

            if (usuario == null)
                return null;

            // Verifica se o usuário está ativo
            if (usuario.ativo == "N" || usuario.ativo == "0")
                return null;

            // Valida senha: MD5 legado (igual ao RetornaMD5 do Pascal)
            if (!ValidarSenhaMd5(usuario.senha, senha))
                return null;

            return usuario;
        }

        /// <summary>
        /// Cria o cookie de autenticação com Claims baseadas na entidade Usuario
        /// e dados da empresa selecionada.
        /// </summary>
        public async Task SignInAsync(HttpContext httpContext, Usuario usuario, bool isPersistent,
            string idEmpresa = null, string nomeEmpresa = null)
        {
            var claims = CriarClaims(usuario, idEmpresa, nomeEmpresa);

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                ExpiresUtc = isPersistent
                    ? DateTimeOffset.UtcNow.AddDays(30)
                    : DateTimeOffset.UtcNow.AddHours(3),
                AllowRefresh = true
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        /// <summary>
        /// Remove o cookie de autenticação.
        /// </summary>
        public async Task SignOutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Computa MD5 no formato legado: 32 caracteres hexadecimais minúsculos.
        /// Igual à função RetornaMD5 do Pascal (md5.pas).
        /// </summary>
        public string ComputeMd5Hash(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using (var md5 = MD5.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = md5.ComputeHash(bytes);

                var sb = new StringBuilder();
                foreach (var b in hash)
                    sb.Append(b.ToString("x2")); // lowercase hex

                return sb.ToString();
            }
        }

        #region Métodos Privados

        /// <summary>
        /// Valida a senha fornecida contra o hash MD5 armazenado.
        /// O hash armazenado está em hex lowercase (32 caracteres).
        /// </summary>
        private bool ValidarSenhaMd5(string hashArmazenado, string senhaFornecida)
        {
            if (string.IsNullOrEmpty(hashArmazenado) || string.IsNullOrEmpty(senhaFornecida))
                return false;

            var hashFornecido = ComputeMd5Hash(senhaFornecida);

            return string.Equals(
                hashArmazenado,
                hashFornecido,
                StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Cria a lista de Claims a partir da entidade Usuario e dados da empresa.
        /// Mantém compatibilidade com ClaimTypes usados pelo sistema.
        /// </summary>
        private List<Claim> CriarClaims(Usuario usuario, string idEmpresa = null, string nomeEmpresa = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,
                    !string.IsNullOrEmpty(usuario.idUserAspNet)
                        ? usuario.idUserAspNet
                        : usuario.Id.ToString()),

                new Claim(ClaimTypes.Name, usuario.usuario ?? string.Empty),
                new Claim(ClaimTypes.GivenName, usuario.nome ?? string.Empty),
                new Claim(ClaimTypes.Email, usuario.email ?? string.Empty),

                new Claim("UsuarioId", usuario.Id.ToString()),
                new Claim("Nome", usuario.nome ?? string.Empty),
                new Claim("Login", usuario.usuario ?? string.Empty),
                new Claim("CPF", usuario.cpf ?? string.Empty),
                new Claim("Ativo", usuario.ativo ?? "S"),
                new Claim("id_perfil", usuario.id_perfil?.ToString() ?? "0"),
                new Claim("idUserAspNet", usuario.idUserAspNet ?? string.Empty)
            };

            // Adiciona claims da empresa selecionada (se fornecidas)
            if (!string.IsNullOrEmpty(idEmpresa))
            {
                claims.Add(new Claim("IDEMPRESA", idEmpresa));
            }
            if (!string.IsNullOrEmpty(nomeEmpresa))
            {
                claims.Add(new Claim("NomeEmpresa", nomeEmpresa));
            }

            return claims;
        }

        #endregion
    }
}

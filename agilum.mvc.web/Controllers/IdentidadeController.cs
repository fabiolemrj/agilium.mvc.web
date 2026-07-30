using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Services;
using agilium_manager_azure_business.Interfaces.IService;
using agilum.mvc.web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    /// <summary>
    /// Controller de autenticação que substitui as páginas Identity UI.
    /// Usa IAuthService para validar login contra a entidade Usuario (ca_usuarios).
    /// </summary>
    [Route("identidade")]
    public class IdentidadeController : MainController
    {
        private readonly IEmpresaService _empresaService;
        private readonly IUsuarioService _usuarioService;

        public IdentidadeController(
            INotificador notificador,
            IConfiguration configuration,
            IUser appUser,
            IUtilDapperRepository utilDapperRepository,
            ILogService logService,
            AutoMapper.IMapper mapper,
            ILicencaService licencaService,
            IAuthService authService,
            IEmpresaService empresaService,
            IUsuarioService usuarioService)
            : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper, licencaService, authService)
        {
            _empresaService = empresaService;
            _usuarioService = usuarioService;
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var model = new LoginViewModel();
            await CarregarTodasEmpresas(model);
            return View(model);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                await CarregarTodasEmpresas(model);
                return View(model);
            }

            // 1. Valida credenciais
            var usuario = await _authService.ValidarLogin(model.Login, model.Senha);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                await CarregarTodasEmpresas(model);
                return View(model);
            }

            // 2. Verifica EmpresaAuth: obtém apenas empresas autorizadas para este usuário
            var empresasAuth = await _usuarioService.ObterEmpresasPorUsuario(usuario.Id);

            if (empresasAuth == null || empresasAuth.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Usuário não possui acesso a nenhuma empresa. Contate o administrador.");
                await CarregarTodasEmpresas(model);
                return View(model);
            }

            // 3. Valida se a empresa selecionada está no EmpresaAuth do usuário
            EmpresaAuth empresaAuthSelecionada = null;

            if (!string.IsNullOrEmpty(model.Empresa) && long.TryParse(model.Empresa, out var idEmpresaParsed))
            {
                empresaAuthSelecionada = empresasAuth.Find(e => e.IDEMPRESA == idEmpresaParsed);
                if (empresaAuthSelecionada == null)
                {
                    ModelState.AddModelError("Empresa", "Usuário não possui acesso à empresa selecionada.");
                    await CarregarTodasEmpresas(model);
                    return View(model);
                }
            }
            else
            {
                // Empresa não selecionada (Required já valida, mas fallback)
                ModelState.AddModelError("Empresa", "Selecione uma empresa.");
                await CarregarTodasEmpresas(model);
                return View(model);
            }

            var idEmpresa = empresaAuthSelecionada.IDEMPRESA.ToString();
            var nomeEmpresa = empresaAuthSelecionada.Empresa?.NMRZSOCIAL ?? idEmpresa;

            // 4. Armazena empresa na sessão
            HttpContext.Session.SetString("_empSelec",
                System.Text.Json.JsonSerializer.Serialize(new
                {
                    IDEMPRESA = idEmpresa,
                    IDUSUARIO = usuario.usuario,
                    NomeEmpresa = nomeEmpresa
                }));

            // 5. Sign in
            await _authService.SignInAsync(HttpContext, usuario, model.LembrarMe, idEmpresa, nomeEmpresa);

            LogInformacao($"Login: {usuario.usuario} | Empresa: {nomeEmpresa} (ID: {idEmpresa})", "Identidade", "Login", null);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutAsync(HttpContext);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Identidade");
        }

        [HttpGet("acesso-negado")]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        #region Métodos Auxiliares

        /// <summary>
        /// Carrega TODAS as empresas no dropdown (para seleção inicial).
        /// A validação de acesso (EmpresaAuth) é feita no POST.
        /// </summary>
        private async Task CarregarTodasEmpresas(LoginViewModel model)
        {
            var empresas = await _empresaService.ObterTodas();
            model.Empresas = empresas?
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.NMRZSOCIAL ?? e.NMFANTASIA ?? e.Id.ToString()
                })
                .OrderBy(e => e.Text)
                .ToList() ?? new List<SelectListItem>();
        }

        #endregion
    }

    public class LoginViewModel
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Usuário é obrigatório")]
        public string Login { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Senha é obrigatória")]
        public string Senha { get; set; }

        public bool LembrarMe { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Selecione uma empresa")]
        public string Empresa { get; set; }

        public List<SelectListItem> Empresas { get; set; } = new List<SelectListItem>();
    }
}

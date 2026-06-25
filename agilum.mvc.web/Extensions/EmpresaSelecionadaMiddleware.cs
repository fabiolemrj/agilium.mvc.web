using agilum.mvc.web.ViewModels.EmpresaUsuario;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace agilum.mvc.web.Extensions
{
    /// <summary>
    /// Middleware que impede o acesso a qualquer página autenticada
    /// sem que o usuário tenha uma empresa selecionada na sessão.
    /// </summary>
    public class EmpresaSelecionadaMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Caminhos que NÃO exigem empresa selecionada.
        /// </summary>
        private static readonly string[] RotasPermitidas = new[]
        {
            "/Identity/Account/Login",
            "/Identity/Account/LoginEmpresa",
            "/Identity/Account/Logout",
            "/Identity/Account/Lockout",
            "/Identity/Account/ForgotPassword",
            "/Identity/Account/ResetPassword",
            "/Identity/Account/Register",
            "/Identity/Account/ConfirmEmail",
            "/Identity/Account/AccessDenied",
            "/Identity/Account/ExternalLogin",
            "/Identity/Account/LoginWith2fa",
            "/empresa/ObterListasEmpresasPorUsuario",
            "/empresa/SelecionarEmpresa",
            "/empresa/ObterEmpresaSelecionada",
            "/Home/Index",
            "/Home/Error",
            "/sistema-indisponivel",
            "/ObterVersaoSistema",
            "/lib/",
            "/css/",
            "/js/",
            "/dist/",
            "/Images/",
            "/local/",
            "/_framework/",
            "/Usuario/ExibirImagemUsuarioJson",
            "/Usuario/ObterEmpresasUsuarioJson",
            "/favicon.ico"
        };

        public EmpresaSelecionadaMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;
            var isAuth = context.User.Identity?.IsAuthenticated ?? false;

            // Só verifica se o usuário está autenticado
            if (isAuth)
            {
                // Verifica se a rota atual está na lista de permissão
                if (!RotaPermitida(path))
                {
                    // Verifica se há empresa na sessão
                    var empSelec = context.Session.GetString("_empSelec");

                    // Fallback: verifica nas Claims (cookie de autenticação)
                    if (string.IsNullOrEmpty(empSelec))
                    {
                        var idEmpresaClaim = context.User.FindFirst("IDEMPRESA")?.Value;
                        if (!string.IsNullOrEmpty(idEmpresaClaim))
                        {
                            // Reconstrói o objeto de sessão a partir das claims
                            var nomeEmpresaClaim = context.User.FindFirst("NomeEmpresa")?.Value ?? "Empresa";
                            var empresaClaim = new EmpresaUsuarioViewModel
                            {
                                IDEMPRESA = idEmpresaClaim,
                                NomeEmpresa = nomeEmpresaClaim,
                                IDUSUARIO = context.User.FindFirst(ClaimTypes.Name)?.Value ?? ""
                            };
                            context.Session.SetString("_empSelec", JsonSerializer.Serialize(empresaClaim));
                            await _next(context);
                            return;
                        }
                    }

                    if (string.IsNullOrEmpty(empSelec))
                    {
                        // Sem empresa selecionada → redireciona para a tela de seleção de empresa
                        context.Response.Redirect("/empresa/ObterListasEmpresasPorUsuario");
                        return;
                    }

                    // Verifica se a empresa na sessão é válida
                    try
                    {
                        var empresa = JsonSerializer.Deserialize<EmpresaUsuarioViewModel>(empSelec);
                        if (empresa == null || string.IsNullOrEmpty(empresa.IDEMPRESA))
                        {
                            context.Session.Remove("_empSelec");
                            context.Response.Redirect("/empresa/ObterListasEmpresasPorUsuario");
                            return;
                        }
                    }
                    catch
                    {
                        context.Session.Remove("_empSelec");
                        context.Response.Redirect("/empresa/ObterListasEmpresasPorUsuario");
                        return;
                    }
                }
            }

            await _next(context);
        }

        private static bool RotaPermitida(string path)
        {
            // Root e Home/Index: o próprio controller trata a seleção de empresa
            if (path == "/" || string.IsNullOrEmpty(path))
                return true;

            foreach (var rota in RotasPermitidas)
            {
                if (path.StartsWith(rota, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}

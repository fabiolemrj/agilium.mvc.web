using agilum.mvc.web.ViewModels.EmpresaUsuario;
using Microsoft.AspNetCore.Http;
using System;
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
            "/Home/Index",
            "/Home/Error",
            "/sistema-indisponivel",
            "/ObterVersaoSistema",
            "/lib/",
            "/css/",
            "/js/",
            "/dist/",
            "/Images/",
            "/favicon.ico"
        };

        public EmpresaSelecionadaMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Só verifica se o usuário está autenticado
            if (context.User.Identity.IsAuthenticated)
            {
                var path = context.Request.Path.Value ?? string.Empty;

                // Verifica se a rota atual está na lista de permissão
                if (!RotaPermitida(path))
                {
                    // Verifica se há empresa na sessão
                    var empSelec = context.Session.GetString("_empSelec");

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
            foreach (var rota in RotasPermitidas)
            {
                if (path.StartsWith(rota, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}

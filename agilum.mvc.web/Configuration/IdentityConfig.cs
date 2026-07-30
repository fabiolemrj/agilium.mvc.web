using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace agilum.mvc.web.Configuration
{
    /// <summary>
    /// Configuração de autenticação via Cookie sem ASP.NET Core Identity.
    /// A validação de usuário/senha é feita pelo AuthService customizado,
    /// que autentica diretamente contra a entidade Usuario (tabela ca_usuarios).
    /// </summary>
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/identidade/login";
                    options.LogoutPath = "/identidade/logout";
                    options.AccessDeniedPath = "/identidade/acesso-negado";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = System.TimeSpan.FromHours(3);
                });

            return services;
        }
    }
}

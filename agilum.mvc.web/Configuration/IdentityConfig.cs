
using agilium.api.business.Models;
using agilum.mvc.web.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace agilum.mvc.web.Configuration
{
    /// <summary>
    /// Configuração de autenticação via Cookie + Identity Core.
    /// Usa AddIdentityCore para registrar UserManager, SignInManager, etc.
    /// sem conflitar com a autenticação via Cookie configurada manualmente.
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

            // Obtém connection string com fallback para variável de ambiente direta
            var identityConnStr = configuration.GetConnectionString("dbIdentityContextConnection");
            if (string.IsNullOrEmpty(identityConnStr))
                identityConnStr = Environment.GetEnvironmentVariable("dbIdentityContextConnection");
            if (string.IsNullOrEmpty(identityConnStr))
                identityConnStr = Environment.GetEnvironmentVariable("ConnectionStrings__dbIdentityContextConnection");

            // Registra o DbContext do Identity
            services.AddDbContext<dbIdentityContext>(options =>
                options.UseMySql(
                    identityConnStr,
                    b => b.MigrationsAssembly("agilium.mvc.web")));

            // Registra os serviços do Identity (UserManager, SignInManager, RoleManager, etc.)
            // sem adicionar autenticação própria (Cookie auth já está configurada manualmente)
            services.AddIdentityCore<CaUsuarioIdentity>(options =>
            {
                // Configurações de senha
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Configurações de usuário
                options.User.RequireUniqueEmail = false;

                // Configurações de lockout
                options.Lockout.DefaultLockoutTimeSpan = System.TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
                .AddRoles<IdentityRole>()
                .AddSignInManager<SignInManager<CaUsuarioIdentity>>()
                .AddEntityFrameworkStores<dbIdentityContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Identity/Account/Login";
                    options.LogoutPath = "/Identity/Account/Logout";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
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

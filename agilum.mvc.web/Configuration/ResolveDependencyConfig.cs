using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Notificacoes;
using agilium.api.business.Services;
using agilium.api.infra.Context;
using agilium.api.infra.Repository.Dapper;
using agilium.api.infra.Repository;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using agilium.api.manager.Extension;
using agilium.api.manager.Services;
using agilum.mvc.web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using agilium.api.manager.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace agilum.mvc.web.Configuration
{
    public static class ResolveDependencyConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("ConnectionDb");

      
            services.AddScoped<INotificador, Notificador>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<UserManager<AppUserAgiliumIdentity>>();
            services.AddScoped<SignInManager<AppUserAgiliumIdentity>>();

            services.AddScoped<AgiliumContext>();
            services.AddScoped<IUtilDapperRepository, UtilDapperRepository>();
            services.AddScoped<DbSession>();
            services.AddScoped<IDapperRepository, DapperRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<ILogDapper, LogDapperRepository>();
            services.AddScoped<ILogService, LogService>();


            #region Controle Acesso / Usuario
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IUser, AspNetUser>();

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<agilium.api.manager.Services.IEmailSender, ServiceEmail>();    
            #endregion
            #region Empresa
            services.AddScoped<IEmpresaRepository, EmpresaRepository>();
            services.AddScoped<IEmpresaService, EmpresaService>();
            services.AddScoped<IEmpresaAuthRepository, EmpresaAuthRepository>();
            #endregion
            #region Unidade
            services.AddScoped<IUnidadeRepository, Unidaderepository>();
            services.AddScoped<IUnidadeService, UnidadeService>();
            #endregion


            return services;
        }
    }
}

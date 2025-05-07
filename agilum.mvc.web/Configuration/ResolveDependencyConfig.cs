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
using agilum.mvc.web.Extensions;
using agilum.mvc.web.Interfaces;
using agilum.mvc.web.Services;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System.Net.Http;
namespace agilum.mvc.web.Configuration
{
    public static class ResolveDependencyConfig
    {
        private static int tempoEsperaCircuitoEmSegundos = 10;
        private static int quantidadeAcionamentoCircuito = 2;
        public static IServiceCollection ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("ConnectionDb");


            services.AddScoped<INotificador, Notificador>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAutenticacaoService, AutenticacaoService>();

            services.AddScoped<UserManager<AppUserAgiliumIdentity>>();
            services.AddScoped<SignInManager<AppUserAgiliumIdentity>>();

            services.AddScoped<AgiliumContext>();
            services.AddScoped<IUtilDapperRepository, UtilDapperRepository>();
            services.AddScoped<DbSession>();
            services.AddScoped<IDapperRepository, DapperRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<ILogDapper, LogDapperRepository>();
            services.AddScoped<ILogService, LogService>();

            //services.AddHttpClient<IAutenticacaoService, AutenticacaoService>()
            //  .AddPolicyHandler(PollyExtensions.EsperarTentar())
            //  .AddTransientHttpErrorPolicy(
            //      p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));
            
            #region Dapper
            services.AddScoped<ICaRepositoryDapper, CaRepositoryDapper>();

            #endregion

            #region Controle Acesso / Usuario
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IUser, AspNetUser>();
            services.AddScoped<ICaService, CaService>();

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<agilium.api.manager.Services.IEmailSender, ServiceEmail>();

            services.AddScoped<ICaPerfilRepository, CaPerfilRepository>();
            services.AddScoped<ICaService, CaService>();
            services.AddScoped<ICaPermissaoItemRepository, CaPermissaoItemRepository>();
            services.AddScoped<ICaModeloRepository, CaModeloRepository>();
            services.AddScoped<ICaPerfilManagerRepository, CaPerfilManagerRepository>();
            services.AddScoped<ICaAreaManagerRepository, CaAreaManagerRepository>();
            services.AddScoped<ICaPermissaoManagerRepository, CaPermissaoManagerRepository>();
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

            #region Contato
            services.AddScoped<IContatoRepository, ContatoRepository>();
            services.AddScoped<IContatoEmpresaRepository, ContatoEmpresaRepository>();
            services.AddScoped<IContatoService, ContatoService>();
            services.AddScoped<IContatoDapperRepository, ContatoDapperRepository>();
            #endregion

            #region Endereco
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
            services.AddScoped<IEnderecoService, EnderecoService>();
            services.AddScoped<ICepRepository, CepRepository>();
            services.AddScoped<IEnderecoDapperRepository, EnderecoDapperRepository>();
            #endregion

            return services;
        }
    }

    #region PollyExtension

    public class PollyExtensions
    {
        public static AsyncRetryPolicy<HttpResponseMessage> EsperarTentar()
        {
            var retry = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
                }, (outcome, timespan, retryCount, context) =>
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Tentando pela {retryCount} vez!");
                    Console.ForegroundColor = ConsoleColor.White;
                });

            return retry;
        }
    }

    #endregion
}

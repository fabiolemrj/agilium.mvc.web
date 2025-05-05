using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.Services;
using agilium.webapp.manager.mvc.Services.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using agilium.webapp.manager.mvc.Extensions;

namespace agilium.webapp.manager.mvc.Configuration
{
    public static class DependencyInjectionConfig
    {
        private static int tempoEsperaCircuitoEmSegundos = 10;
        private static int quantidadeAcionamentoCircuito = 2;
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddSingleton<IValidationAttributeAdapterProvider, MoedaValidationAttributeAdapterProvider>();

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailSender, ServiceEmail>();

            #region HttpServices

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>()
                .AddPolicyHandler(PollyExtensions.EsperarTentar())
                .AddTransientHttpErrorPolicy(
                    p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));
            services.AddHttpClient<IUsuarioService, UsuarioService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(PollyExtensions.EsperarTentar())
                .AddTransientHttpErrorPolicy(
                    p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IEmpresaService, EmpresaService>()
           .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
           .AddPolicyHandler(PollyExtensions.EsperarTentar())
           .AddTransientHttpErrorPolicy(
               p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IEnderecoService, EnderecoService>()
                 .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                 .AddPolicyHandler(PollyExtensions.EsperarTentar())
                 .AddTransientHttpErrorPolicy(
                     p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IContatoService, ContatoService>()
                 .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                 .AddPolicyHandler(PollyExtensions.EsperarTentar())
                 .AddTransientHttpErrorPolicy(
                     p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IUnidadeService, UnidadeService>()
                 .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                 .AddPolicyHandler(PollyExtensions.EsperarTentar())
                 .AddTransientHttpErrorPolicy(
                     p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IConfigServices, ConfigService>()
                 .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                 .AddPolicyHandler(PollyExtensions.EsperarTentar())
                 .AddTransientHttpErrorPolicy(
                     p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));
            services.AddHttpClient<IControleAcessoService, ControleAcessoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(PollyExtensions.EsperarTentar())
                .AddTransientHttpErrorPolicy(
                    p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<ICategoriaFinanceiraService, CategoriaFinanceiraService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(PollyExtensions.EsperarTentar())
                .AddTransientHttpErrorPolicy(
                    p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IProdutoService, ProdutoService>()
              .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
              .AddPolicyHandler(PollyExtensions.EsperarTentar())
              .AddTransientHttpErrorPolicy(
                  p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IDevolucaoService, DevolucaoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(PollyExtensions.EsperarTentar())
                .AddTransientHttpErrorPolicy(
                    p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IEstoqueService, EstoqueService>()
               .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
               .AddPolicyHandler(PollyExtensions.EsperarTentar())
               .AddTransientHttpErrorPolicy(
                   p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IFornecedorService, FornecedorService>()
               .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
               .AddPolicyHandler(PollyExtensions.EsperarTentar())
               .AddTransientHttpErrorPolicy(
                   p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IClienteService, ClienteService>()
              .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
              .AddPolicyHandler(PollyExtensions.EsperarTentar())
              .AddTransientHttpErrorPolicy(
                  p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IFuncionarioService, FuncionarioService>()
          .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
          .AddPolicyHandler(PollyExtensions.EsperarTentar())
          .AddTransientHttpErrorPolicy(
              p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IMoedaService, MoedaService>()
          .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
          .AddPolicyHandler(PollyExtensions.EsperarTentar())
          .AddTransientHttpErrorPolicy(
              p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IPontoVendaService, PontoVendaService>()
         .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
         .AddPolicyHandler(PollyExtensions.EsperarTentar())
         .AddTransientHttpErrorPolicy(
             p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<ITabelaAuxiliarFiscalService, TabelaAuxiliarFiscalService>()
          .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
          .AddPolicyHandler(PollyExtensions.EsperarTentar())
          .AddTransientHttpErrorPolicy(
              p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<ITurnoService, TurnoService>()
             .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
             .AddPolicyHandler(PollyExtensions.EsperarTentar())
             .AddTransientHttpErrorPolicy(
                 p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IPlanoContaService, PlanoContaService>()
              .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
              .AddPolicyHandler(PollyExtensions.EsperarTentar())
              .AddTransientHttpErrorPolicy(
                  p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IContaService, ContaService>()
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AddPolicyHandler(PollyExtensions.EsperarTentar())
            .AddTransientHttpErrorPolicy(
                p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<INotaFiscalService, NotaFiscalService>()
           .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
           .AddPolicyHandler(PollyExtensions.EsperarTentar())
           .AddTransientHttpErrorPolicy(
               p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<ICaixaService, CaixaService>()
              .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
              .AddPolicyHandler(PollyExtensions.EsperarTentar())
              .AddTransientHttpErrorPolicy(
                  p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IValeService, ValeService>()
             .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
             .AddPolicyHandler(PollyExtensions.EsperarTentar())
             .AddTransientHttpErrorPolicy(
                 p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IVendaService, VendaService>()
             .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
             .AddPolicyHandler(PollyExtensions.EsperarTentar())
             .AddTransientHttpErrorPolicy(
                 p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IPerdaService, PerdaService>()
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AddPolicyHandler(PollyExtensions.EsperarTentar())
            .AddTransientHttpErrorPolicy(
                p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<ICompraService, CompraService>()
           .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
           .AddPolicyHandler(PollyExtensions.EsperarTentar())
           .AddTransientHttpErrorPolicy(
               p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IImportarXMLNfe, ImportarXmlNfeService>()
         .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
         .AddPolicyHandler(PollyExtensions.EsperarTentar())
         .AddTransientHttpErrorPolicy(
             p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IInventarioService, InventarioService>()
         .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
         .AddPolicyHandler(PollyExtensions.EsperarTentar())
         .AddTransientHttpErrorPolicy(
             p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<ILogService, LogService>()
             .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
             .AddPolicyHandler(PollyExtensions.EsperarTentar())
             .AddTransientHttpErrorPolicy(
                 p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<ISiteMercadoService, SiteMercadoService>()
           .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
           .AddPolicyHandler(PollyExtensions.EsperarTentar())
           .AddTransientHttpErrorPolicy(
               p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));

            services.AddHttpClient<IFormaPagamentoService, FormaPagamentoService>()
          .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
          .AddPolicyHandler(PollyExtensions.EsperarTentar())
          .AddTransientHttpErrorPolicy(
              p => p.CircuitBreakerAsync(quantidadeAcionamentoCircuito, TimeSpan.FromSeconds(tempoEsperaCircuitoEmSegundos)));


            #endregion
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

}

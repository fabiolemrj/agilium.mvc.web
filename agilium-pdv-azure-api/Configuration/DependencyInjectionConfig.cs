using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using agilium.api.business.Notificacoes;
using agilium.api.business.Services;
using agilium.api.infra.Config;
using agilium.api.infra.Context;
using agilium.api.infra.Mappings;
using agilium.api.infra.Repository.Dapper;
using agilium.api.infra.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using agilium.api.pdv.Extension;
using Microsoft.Extensions.Configuration;

namespace agilium.api.pdv.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
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
            //services.AddScoped<IEnderecoRepository, EnderecoRepository>();

            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IUserClaimsManagerService, UserClaimsManagerService>();
            services.AddScoped<IClaimRepository, ObjetoClaimRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            services.Configure<ServiceEmail>(configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailSender, ServiceEmail>();
            services.AddScoped<IUsuarioFotoEntityService, UsuarioFotoEntityService>();
            services.AddScoped<IUsuarioFotoRepository, UsuarioFotoEntityRepository>();

            services.AddScoped<IEmpresaAuthRepository, EmpresaAuthRepository>();

            services.AddScoped<ICaPerfilRepository, CaPerfilRepository>();
            services.AddScoped<ICaService, CaService>();
            services.AddScoped<ICaPermissaoItemRepository, CaPermissaoItemRepository>();
            services.AddScoped<ICaModeloRepository, CaModeloRepository>();


            services.AddScoped<ICaPerfilRepository, CaPerfilRepository>();
            services.AddScoped<ICaService, CaService>();
            services.AddScoped<ICaPermissaoItemRepository, CaPermissaoItemRepository>();
            services.AddScoped<ICaModeloRepository, CaModeloRepository>();
            services.AddScoped<ICaPerfilManagerRepository, CaPerfilManagerRepository>();
            services.AddScoped<ICaAreaManagerRepository, CaAreaManagerRepository>();
            services.AddScoped<ICaPermissaoManagerRepository, CaPermissaoManagerRepository>();
            #endregion

            #region Controle Acesso / Usuario
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            //services.AddScoped<IEnderecoRepository, EnderecoRepository>();

            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IUserClaimsManagerService, UserClaimsManagerService>();
            services.AddScoped<IClaimRepository, ObjetoClaimRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailSender, ServiceEmail>();
            services.AddScoped<IUsuarioFotoEntityService, UsuarioFotoEntityService>();
            services.AddScoped<IUsuarioFotoRepository, UsuarioFotoEntityRepository>();

            services.AddScoped<IEmpresaAuthRepository, EmpresaAuthRepository>();

            services.AddScoped<ICaPerfilRepository, CaPerfilRepository>();
            services.AddScoped<ICaService, CaService>();
            services.AddScoped<ICaPermissaoItemRepository, CaPermissaoItemRepository>();
            services.AddScoped<ICaModeloRepository, CaModeloRepository>();
            #endregion

            #region Endereco
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
            services.AddScoped<IEnderecoService, EnderecoService>();
            services.AddScoped<ICepRepository, CepRepository>();
            services.AddScoped<IEnderecoDapperRepository, EnderecoDapperRepository>();
            #endregion

            #region Empresa
            services.AddScoped<IEmpresaRepository, EmpresaRepository>();
            services.AddScoped<IEmpresaService, EmpresaService>();
            #endregion

            #region Contato
            services.AddScoped<IContatoRepository, ContatoRepository>();
            services.AddScoped<IContatoEmpresaRepository, ContatoEmpresaRepository>();
            services.AddScoped<IContatoService, ContatoService>();
            services.AddScoped<IContatoDapperRepository, ContatoDapperRepository>();
            #endregion

            #region Caixa
            services.AddScoped<ICaixaRepository, CaixaRepository>();
            services.AddScoped<ICaixaDapperRepository, CaixaDapperRepository>();
            services.AddScoped<ICaixaService, CaixaService>();
            services.AddScoped<ICaixaMoedaRepository, CaixaMoedaRepository>();
            services.AddScoped<ICaixaMovimentoRepository, CaixaMovimentaoRepository>();
            #endregion

            #region Turno
            services.AddScoped<ITurnoRepository, TurnoRepository>();
            services.AddScoped<ITurnoPrecoRepository, TurnoPrecoRepository>();
            services.AddScoped<IPTurnoDapperRepository, TurnoDapperRepository>();
            services.AddScoped<ITurnoService, TurnoService>();

            #endregion

            #region Config
            services.AddScoped<IConfigRepository, ConfigRepository>();
            services.AddScoped<IConfigImagemRepository, ConfigImagemRepository>();
            services.AddScoped<IConfigDapperRepository, ConfigDapperRepository>();
            services.AddScoped<IConfigService, ConfigService>();
            #endregion

            #region PDV
            services.AddScoped<IPontoVendaRepository, PontoVendaRepository>();
            services.AddScoped<IPontoVendaService, PontoVendaService>();
            #endregion

            #region Pedido
            services.AddScoped<IPedidoDapperRepository, PedidoDapperRepository>();
            services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IPedidoItemRepository, PedidoItemRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IPedidoSiteMercadoRepository, PedidoSiteMercadoRepository>();
            services.AddScoped<IPedidoVendaItemRepository, PedidoVendaItemRepository>();
            services.AddScoped<IPedidoVendaRepository, PedidoVendaRepository>();
            services.AddScoped<IPedidoSiteMercadoItemRepository, PedidoItemSiteMercadoRepository>();

            #endregion

            #region Venda
            services.AddScoped<IVendaDapperRepository, VendaDapperRepository>();
            services.AddScoped<IVendaRepository, VendaRepository>();
            services.AddScoped<IVendaMoedaRepository, VendaMoedaRepository>();
            services.AddScoped<IVendaItemRepository, VendaItemRepository>();
            services.AddScoped<IVendaFiscalRepository, VendaFiscalRepository>();
            services.AddScoped<IVendaEspelhoRepository, VendaEspelhoRepository>();
            services.AddScoped<IVendaCanceladaRepository, VendaCanceladaRepository>();
            services.AddScoped<IVendaService, VendaService>();
            #endregion

            #region Config
            services.AddScoped<IConfigRepository, ConfigRepository>();
            services.AddScoped<IConfigImagemRepository, ConfigImagemRepository>();
            services.AddScoped<IConfigDapperRepository, ConfigDapperRepository>();
            services.AddScoped<IConfigService, ConfigService>();
            #endregion

            #region Estoque
            services.AddScoped<IEstoqueRepository, EstoqueRepository>();
            services.AddScoped<IEstoqueService, EstoqueService>();
            services.AddScoped<IEstoqueProdutoRepository, EstoqueProdutoRepository>();
            services.AddScoped<IEstoqueHistoricoRepository, EstoqueHistoricoRepository>();
            services.AddScoped<IEstoqueDapperRepository, EstoqueDapperRepository>();
            #endregion

            #region Cliente
            services.AddScoped<IClienteContatoRepository, ClienteContatoRepository>();
            services.AddScoped<IClientePFRepository, ClientePFRepository>();
            services.AddScoped<IClientePJRepository, ClientePJRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IClientePrecoRepository, ClientePrecoRepository>();
            services.AddScoped<IClienteDapperRepository, ClienteDapperRepository>();
            #endregion

            #region Venda
            services.AddScoped<IVendaDapperRepository, VendaDapperRepository>();
            services.AddScoped<IVendaRepository, VendaRepository>();
            services.AddScoped<IVendaMoedaRepository, VendaMoedaRepository>();
            services.AddScoped<IVendaItemRepository, VendaItemRepository>();
            services.AddScoped<IVendaFiscalRepository, VendaFiscalRepository>();
            services.AddScoped<IVendaEspelhoRepository, VendaEspelhoRepository>();
            services.AddScoped<IVendaCanceladaRepository, VendaCanceladaRepository>();
            services.AddScoped<IVendaService, VendaService>();
            #endregion

            #region Venda Temporaria
            services.AddScoped<IVendaTemporariaRepository, VendaTemporariaRepository>();
            services.AddScoped<IVendaTemporariaItemRepository, VendaTemporariaItemRepository>();
            services.AddScoped<IVendaTemporariaMoedaRepository, VendaTemporariaMoedaRepository>();
            services.AddScoped<IVendaTemporariaEspelhoRepository, VendaTemporariaEspelhoRepository>();
            #endregion

            #region Vale
            services.AddScoped<IValeRepository, ValeRepository>();
            services.AddScoped<IValeDapperRepository, ValeDapperRepository>();
            services.AddScoped<IValeService, ValeService>();
            #endregion
            
            #region Dapper
            services.AddScoped<ICaRepositoryDapper, CaRepositoryDapper>();
            #endregion

            return services;
        }
    }
}

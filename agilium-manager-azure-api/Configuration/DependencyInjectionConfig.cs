using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Notificacoes;
using agilium.api.business.Services;
using agilium.api.infra.Config;
using agilium.api.infra.Context;
using agilium.api.infra.Mappings;
using agilium.api.infra.Repository;
using agilium.api.infra.Repository.Dapper;
using agilium.api.manager.Extension;
using agilium.api.manager.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace agilium.api.manager.Configuration
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

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailSender, ServiceEmail>();
            services.AddScoped<IUsuarioFotoEntityService, UsuarioFotoEntityService>();
            services.AddScoped<IUsuarioFotoRepository, UsuarioFotoEntityRepository>();

            services.AddScoped<IEmpresaAuthRepository, EmpresaAuthRepository>();

            services.AddScoped<ICaPerfilRepository, CaPerfilRepository>();
            services.AddScoped<ICaService, CaService>();
            services.AddScoped<ICaPermissaoItemRepository, CaPermissaoItemRepository>();
            services.AddScoped<ICaModeloRepository, CaModeloRepository>();
            services.AddScoped<ICaPerfilManagerRepository, CaPerfilManagerRepository>();
            services.AddScoped<ICaAreaManagerRepository, CaAreaManagerRepository>();
            services.AddScoped<ICaPermissaoManagerRepository, CaPermissaoManagerRepository>();
            #endregion

            #region MongoDb
            var mongoDbSettings = configuration.GetSection("MongoDBSetting").Get<MongoDbSetting>();

            var connectionFactory = new ConnectionFactory(mongoDbSettings.ConnectionString);

            var usuarioFotoMongoMapping = new UsuarioFotoMongoMapping();
            usuarioFotoMongoMapping.RegisterMapp();

            services.AddScoped<IRepositoryMongo<UsuarioFoto>>(
                p => new UsuarioFotoRepositoryMongo(connectionFactory, mongoDbSettings.DatabaseName, "fotoUsuario"));

            services.AddTransient<IUsuarioFotoService, UsuarioFotoService>();

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

            #region Unidade
            services.AddScoped<IUnidadeRepository, Unidaderepository>();
            services.AddScoped<IUnidadeService, UnidadeService>();
            #endregion

            #region Config
            services.AddScoped<IConfigRepository, ConfigRepository>();
            services.AddScoped<IConfigImagemRepository, ConfigImagemRepository>();
            services.AddScoped<IConfigDapperRepository, ConfigDapperRepository>();
            services.AddScoped<IConfigService, ConfigService>();
            #endregion

            #region Tabela Auxiliar Fiscal
            services.AddScoped<ICestNcmRepository, CestNcmRepository>();
            services.AddScoped<ICsosnRepository, CsosnRepository>();
            services.AddScoped<ICstRepository, CstRepository>();
            services.AddScoped<ICfopRepository, CfopRepository>();
            services.AddScoped<IIbptRepository, IbptRepository>();
            services.AddScoped<INcmRepository, NcmRepository>();
            services.AddScoped<ITabelaAuxiliarFiscalService, TabelaAuxiliarFiscalService>();
            #endregion

            #region financeiro
            services.AddScoped<ICategoriaFinanceiroRepository, CategoriaFinanceiraRepository>();
            services.AddScoped<ICategoriaFinanceiraService, CategoriaFinanceiraService>();
            #endregion

            #region Dapper
            services.AddScoped<ICaRepositoryDapper, CaRepositoryDapper>();
            #endregion

            #region Produto
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IProdutoDepartamentoRepository, ProdutoDepartamentoRepository>();
            services.AddScoped<IProdutoMarcaRepository, ProdutoMarcaRepository>();
            services.AddScoped<IGrupoProdutoRepository, GrupoProdutoRepository>();
            services.AddScoped<ISubGrupoProdutoRepository, SubGrupoProdutoRepository>();
            services.AddScoped<IProdutoRepository, ProdutoReposiotry>();
            services.AddScoped<IProdutoComposicaoRepository, ProdutoComposicaoRepository>();
            services.AddScoped<IProdutoCodigoBarraRepository, ProdutoCodigoBarraRepository>();
            services.AddScoped<IProdutoPrecoRepository, ProdutoPrecoRepository>();
            services.AddScoped<IProdutoFotoRepository, ProdutoFotoRepository>();
            services.AddScoped<IProdutoDapper, ProdutoDapper>();

            #endregion

            #region Devolucao
            services.AddScoped<IDevolucaoDapperRepository, DevolucaoDapperRepository>();
            services.AddScoped<IDevolucaoItemRepository, DevolucaoItemRepository>();
            services.AddScoped<IDevolucaoRepository, DevolucaoRepository>();
            services.AddScoped<IMotivoDevolucaoRepository, MotivoDevolucaoRepository>();
            services.AddScoped<IDevolucaoService, DevolucaoService>();
            #endregion

            #region Estoque
            services.AddScoped<IEstoqueRepository, EstoqueRepository>();
            services.AddScoped<IEstoqueService, EstoqueService>();
            services.AddScoped<IEstoqueProdutoRepository, EstoqueProdutoRepository>();
            services.AddScoped<IEstoqueHistoricoRepository, EstoqueHistoricoRepository>();
            services.AddScoped<IEstoqueDapperRepository, EstoqueDapperRepository>();
            #endregion

            #region Fornecedor
            services.AddScoped<IFornecedorContatoRepsoitory, FornecedorContatoRepository>();
            services.AddScoped<IFornecedorRepsoitory, FornecedorRepository>();
            services.AddScoped<IFornecedorService, FornecedorService>();
            services.AddScoped<IFornecedorDapperRepository, FornecedorDapperRepository>();
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

            #region Funcionario
            services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
            services.AddScoped<IFuncionarioService, FuncionarioService>();
            #endregion

            #region Moeda
            services.AddScoped<IMoedaRepository, MoedaRepository>();
            services.AddScoped<IMoedaService, MoedaService>();
            #endregion

            #region PDV
            services.AddScoped<IPontoVendaRepository, PontoVendaRepository>();
            services.AddScoped<IPontoVendaService, PontoVendaService>();
            #endregion

            #region Turno
            services.AddScoped<ITurnoRepository, TurnoRepository>();
            services.AddScoped<ITurnoPrecoRepository, TurnoPrecoRepository>();
            services.AddScoped<IPTurnoDapperRepository, TurnoDapperRepository>();
            services.AddScoped<ITurnoService, TurnoService>();

            #endregion

            #region Plano Conta
            services.AddScoped<IPlanoContaSaldoRepository, PlanoContaSaldoRepository>();
            services.AddScoped<IPlanoContaRepository, PlanoContaRepository>();
            services.AddScoped<IPlanoContaDapperRepository, PlanoContaDapperRepository>();
            services.AddScoped<IPlanoContaLancamentoRepository, PlanoContaLancamentoRepository>();
            services.AddScoped<IPlanoContaService, PlanoContaService>();
            #endregion

            #region Contas
            services.AddScoped<IPContaPagarDapperRepository, ContaPagarDapperRepository>();
            services.AddScoped<IContaPagarRepository, ContaPagarRepository>();
            services.AddScoped<IContaReceberRepository, ContaReceberRepository>();
            services.AddScoped<IPContaReceberDapperRepository, ContaReceberRepositoryDapper>();
            services.AddScoped<IContaService, ContaService>();
            #endregion

            #region Nota Fiscal
            services.AddScoped<INotaFiscalInutilRepository, NotaFiscalInutilRepository>();
            services.AddScoped<IPNotaFiscalDapperRepository, NotaFiscalDapperRepository>();
            services.AddScoped<INotaFiscalService, NotaFiscalService>();
            #endregion

            #region Caixa
            services.AddScoped<ICaixaRepository, CaixaRepository>();
            services.AddScoped<ICaixaMoedaRepository, CaixaMoedaRepository>();
            services.AddScoped<ICaixaMovimentoRepository, CaixaMovimentaoRepository>();
            services.AddScoped<ICaixaService, CaixaService>();
            services.AddScoped<ICaixaDapperRepository, CaixaDapperRepository>();
            #endregion

            #region Vale
            services.AddScoped<IValeRepository, ValeRepository>();
            services.AddScoped<IValeDapperRepository, ValeDapperRepository>();
            services.AddScoped<IValeService, ValeService>();
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

            #region Perda
            services.AddScoped<IPerdaRepository, PerdaRepository>();
            services.AddScoped<IPerdaService, PerdaService>();
            services.AddScoped<IPerdaDapperRepository, PerdaDapperReposiotry>();
            #endregion

            #region Compra
            services.AddScoped<ICompraRepository, CompraRepository>();
            services.AddScoped<ICompraFiscalRepository, CompraFiscalRepository>();
            services.AddScoped<ICompraItemRepository, CompraItemRepository>();
            services.AddScoped<ICompraService, CompraService>();
            services.AddScoped<ICompraDapperRepository, CompraDapperRepository>();
            #endregion

            #region Inventario
            services.AddScoped<IInventarioRepository, InventarioRepository>();
            services.AddScoped<IInventarioItemRepository, InventarioItemRepository>();
            services.AddScoped<IInventarioDapperRepository, InventarioDapperRepository>();
            services.AddScoped<IInventarioService, InventarioService>();
            #endregion

            #region SiteMercado
            services.AddScoped<ISiteMercadoService, SiteMercadoService>();
            services.AddScoped<IProdutoSiteMercadoRepository, ProdutoSiteMercadoRepository>();
            services.AddScoped<IMoedaSiteMercadoRepository, MoedaSiteMercadoRepository>();
            #endregion

            #region Pedido
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IPedidoItemRepository, PedidoItemRepository>();
            services.AddScoped<IPedidoSiteMercadoRepository, PedidoSiteMercadoRepository>();
            services.AddScoped<IPedidoSiteMercadoItemRepository, PedidoItemSiteMercadoRepository>();
            services.AddScoped<IPedidoPagamentoRepository, PedidoPagamentoRepository>();
            services.AddScoped<IPedidoPagamentoSiteMercadoRepository, PedidoPagamentoSiteMercadoRepository>();
            services.AddScoped<IPedidoVendaRepository, PedidoVendaRepository>();
            services.AddScoped<IPedidoVendaItemRepository, PedidoVendaItemRepository>();
          //  services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<IPedidoDapperRepository, PedidoDapperRepository>();
            #endregion

            #region Forma Pagamento
            services.AddScoped<IFormaPagamentoRepository, FormaPagamentoRepository>();
            services.AddScoped<IFormaPagamentoService, FormaPagamentoService>();
            #endregion

            #region Venda Temporaria
            services.AddScoped<IVendaTemporariaRepository, VendaTemporariaRepository>();
            services.AddScoped<IVendaTemporariaItemRepository, VendaTemporariaItemRepository>();
            services.AddScoped<IVendaTemporariaMoedaRepository, VendaTemporariaMoedaRepository>();
            services.AddScoped<IVendaTemporariaEspelhoRepository, VendaTemporariaEspelhoRepository>();
            #endregion

            return services;
        }
    }
}

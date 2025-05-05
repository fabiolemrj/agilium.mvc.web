using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace agilium.api.infra.Context
{
    public class AgiliumContext: DbContext
    {

        public AgiliumContext(DbContextOptions<AgiliumContext> options): base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ObjetoClaim> ObjetosClaims { get; set; }
        public DbSet<UsuarioFotoEntity> UsuarioFotos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Cep> Ceps { get; set; }
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<ContatoEmpresa> ContatoEmpresas { get; set; }
        public DbSet<Unidade> Unidades { get; set; }
        public DbSet<agilium.api.business.Models.Config> Configs { get; set; }
        public DbSet<EmpresaAuth> EmpresasAuths { get; set; }
        public DbSet<Cfop> Cfops { get; set; }
        public DbSet<CestNcm> CestNcms { get; set; }
        public DbSet<Csosn> Csosns { get; set; }
        public DbSet<Cst> Csts { get; set; }
        public DbSet<Ncm> Ncms { get; set; }
        public DbSet<Ibpt> Ibpts { get; set; }
        public DbSet<ConfigImagem> ConfigImagens { get; set; }
        public DbSet<CaPerfil> Perfis { get; set; }
        public DbSet<CaPermissaoItem> PermissoesItens { get; set; }
        public DbSet<CaModelo> Modelos { get; set; }
        public DbSet<CategoriaFinanceira> CategoriaFinanceiras { get; set; }
        public DbSet<ProdutoDepartamento> ProdutosDepartamentos { get; set; }
        public DbSet<ProdutoMarca> ProdutosMarcas { get; set; }
        public DbSet<MotivoDevolucao> MotivosDevolucao { get; set; }
        public DbSet<GrupoProduto> GrupoProdutos { get; set; }
        public DbSet<SubGrupoProduto> SubGrupoProdutos { get; set; }
        public DbSet<Estoque> Estoques { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<FornecedorContato> FornecedoresContatos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ClienteContato> ClientesContatos { get; set; }
        public DbSet<ClientePF> ClientesPF { get; set; }
        public DbSet<ClientePJ> ClientesPJ { get; set; }
        public DbSet<ClientePreco> ClientesPrecos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Moeda> Moedas { get; set; }
        public DbSet<PontoVenda> PontosVendas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ProdutoCodigoBarra> ProdutoCodigosBarra { get; set; }
        public DbSet<ProdutoPreco> ProdutosPrecos { get; set; }
        public DbSet<EstoqueProduto> EstoqueProdutos { get; set; }
        public DbSet<EstoqueHistorico> EstoqueHistoricos { get; set; }
        public DbSet<ProdutoFoto> ProdutoFotos { get; set; }
        public DbSet<TurnoPreco> TurnosPrecos { get; set; }
        public DbSet<PlanoConta> PlanoContas { get; set; }
        public DbSet<PlanoContaSaldo> PlanoContaSaldo { get; set; }
        public DbSet<PlanoContaLancamento> PlanoContaLancamentos { get; set; }
        public DbSet<ContaPagar> ContasPagar { get; set; }
        public DbSet<ContaReceber> ContasReceber { get; set; }
        public DbSet<NotaFiscalInutil> NotasFiscaisInuteis { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Caixa> Caixa { get; set; }
        public DbSet<CaixaMoeda> CaixaMoedas { get; set; }
        public DbSet<CaixaMovimento> CaixaMovimentos { get; set; }
        public DbSet<Vale> Vales { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<VendaCancelada> VendaCancelada { get; set; }
        public DbSet<VendaEspelho> VendaEspelho { get; set; }
        public DbSet<VendaFiscal> VendaFiscal { get; set; }
        public DbSet<VendaItem> VendaItem { get; set; }
        public DbSet<VendaMoeda> VendaMoeda { get; set; }
        public DbSet<Perda> Perdas { get; set; }
        public DbSet<Devolucao> Devolucao { get; set; }
        public DbSet<DevolucaoItem> DevolucaoItem { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<CompraItem> CompraItem { get; set; }
        public DbSet<CompraFiscal> CompraFiscal { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<InventarioItem> InventarioItens { get; set; }
        public DbSet<LogSistema> LogSistemas { get; set; }
        public DbSet<LogErro> LogErro { get; set; }
        public DbSet<ProdutoSiteMercado> ProdutosSiteMercado { get; set; }
        public DbSet<MoedaSiteMercado> MoedasSiteMercado { get; set; }
        //public DbSet<Pedido> Pedidos { get; set; }
        //public DbSet<PedidoItem> PedidosItems { get; set; }
        //public DbSet<PedidoItemSitemercado> PedidoItemSitemercados { get; set; }
        //public DbSet<PedidoSitemercado> PedidoSitemercados { get; set; }
        //public DbSet<PedidoPagamento> PedidoPagamentos { get; set; }
        //public DbSet<PedidoPagamentoSitemercado> PedidoPagamentoSitemercados { get; set; }
        //public DbSet<PedidoVenda> PedidoVendas { get; set; }
        //public DbSet<PedidoVendaItem> PedidoVendaItem { get; set; }
        //public DbSet<FormaPagamento> FormaPagamentos { get; set; }

        public DbSet<CaPermissaoManager> CaPermissaoManager { get; set; }
        public DbSet<CaPerfiManager> CaPerfilManager { get; set; }
        public DbSet<CaAreaManager> CaAreaManager { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                    .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AgiliumContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging()
                 .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole())); ;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}

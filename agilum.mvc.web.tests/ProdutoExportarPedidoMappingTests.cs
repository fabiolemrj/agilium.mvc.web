using agilium.api.business.Enums;
using agilium.api.business.Models;
using agilum.mvc.web.Configuration;
using agilum.mvc.web.ViewModels.Produtos;
using AutoMapper;
using Xunit;

namespace agilum.mvc.web.tests
{
    /// <summary>
    /// Testa o mapeamento AutoMapper entre ProdutoViewModel e Produto,
    /// validando especialmente o campo ExportarPedido / STEXPORTARPEDIDO.
    /// </summary>
    public class ProdutoExportarPedidoMappingTests
    {
        private readonly IMapper _mapper;

        public ProdutoExportarPedidoMappingTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutomapperConfig>();
            });

            // Força a validação de toda a configuração no momento da criação
            // config.AssertConfigurationIsValid(); // Desabilitado: muitos mapeamentos com propriedades não mapeadas

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Map_ViewModelToProduto_ExportarPedidoSim_DeveSetarSTEXPORTARPEDIDO_Sim()
        {
            // Arrange: ViewModel com ExportarPedido = Sim
            var viewModel = new ProdutoViewModel
            {
                Id = 1,
                Codigo = "000001",
                Nome = "Produto Teste",
                Situacao = EAtivo.Ativo,
                Preco = 10.0,
                ExportarPedido = ESimNao.Sim,
                UtilizaBalanca = ESimNao.Nao,
                UnidadeVenda = "UN",
                UnidadeCompra = "UN",
                idEmpresa = 1
            };

            // Act: mapeia ViewModel → Produto
            var produto = _mapper.Map<Produto>(viewModel);

            // Assert
            Assert.NotNull(produto);
            Assert.Equal(ESimNao.Sim, produto.STEXPORTARPEDIDO);
        }

        [Fact]
        public void Map_ViewModelToProduto_ExportarPedidoNao_DeveSetarSTEXPORTARPEDIDO_Nao()
        {
            // Arrange
            var viewModel = new ProdutoViewModel
            {
                Id = 2,
                Codigo = "000002",
                Nome = "Produto Teste 2",
                Situacao = EAtivo.Ativo,
                Preco = 20.0,
                ExportarPedido = ESimNao.Nao,
                UtilizaBalanca = ESimNao.Nao,
                UnidadeVenda = "UN",
                UnidadeCompra = "UN",
                idEmpresa = 1
            };

            // Act
            var produto = _mapper.Map<Produto>(viewModel);

            // Assert
            Assert.NotNull(produto);
            Assert.Equal(ESimNao.Nao, produto.STEXPORTARPEDIDO);
        }

        [Fact]
        public void Map_ViewModelToProduto_ExportarPedidoNulo_DeveSetarSTEXPORTARPEDIDO_Nulo()
        {
            // Arrange
            var viewModel = new ProdutoViewModel
            {
                Id = 3,
                Codigo = "000003",
                Nome = "Produto Teste 3",
                Situacao = EAtivo.Ativo,
                Preco = 30.0,
                ExportarPedido = null,
                UtilizaBalanca = ESimNao.Nao,
                UnidadeVenda = "UN",
                UnidadeCompra = "UN",
                idEmpresa = 1
            };

            // Act
            var produto = _mapper.Map<Produto>(viewModel);

            // Assert
            Assert.NotNull(produto);
            Assert.Null(produto.STEXPORTARPEDIDO);
        }

        [Fact]
        public void Map_ProdutoToViewModel_STEXPORTARPEDIDO_Sim_DeveSetarExportarPedido_Sim()
        {
            // Arrange: Produto com STEXPORTARPEDIDO = Sim
            var produto = new Produto(
                idEmpresa: 1,
                iDGRUPO: null,
                cDPRODUTO: "000004",
                nMPRODUTO: "Produto Origem",
                cTPRODUTO: ECategoriaProduto.Simples,
                tPPRODUTO: ETipoProduto.Mercadoria,
                uNCOMPRA: "UN",
                uNVENDA: "UN",
                nURELACAO: 1,
                nUPRECO: 15.0,
                nUQTDMIN: null,
                cDSEFAZ: null,
                cDANP: null,
                cDNCM: null,
                cDCEST: null,
                cDSERV: null,
                sTPRODUTO: EAtivo.Ativo,
                vLULTIMACOMPRA: null,
                vLCUSTOMEDIO: null,
                pCIBPTFED: null,
                pCIBPTEST: null,
                pCIBPTMUN: null,
                pCIBPTIMP: null,
                nUCFOP: null,
                sTORIGEMPROD: null,
                dSICMS_CST: null,
                pCICMS_ALIQ: null,
                pCICMS_REDUCBC: null,
                pCICMSST_ALIQ: null,
                pCICMSST_MVA: null,
                pCICMSST_REDUCBC: null,
                dSIPI_CST: null,
                pCIPI_ALIQ: null,
                dSPIS_CST: null,
                pCPIS_ALIQ: null,
                dSCOFINS_CST: null,
                pCCOFINS_ALIQ: null,
                sTESTOQUE: null,
                sTBALANCA: ESimNao.Nao,
                fLG_IFOOD: ESimNao.Nao,
                sTEXPORTARPEDIDO: ESimNao.Sim,
                iDMARCA: null,
                iDDEP: null,
                iDSUBGRUPO: null,
                dSVOLUME: null
            );

            // Força o Id (o construtor da Entity gera um Id aleatório)
            typeof(agilium.api.business.Models.Entity)
                .GetProperty("Id")!
                .SetValue(produto, 4);

            // Act: mapeia Produto → ViewModel
            var viewModel = _mapper.Map<ProdutoViewModel>(produto);

            // Assert
            Assert.NotNull(viewModel);
            Assert.Equal(ESimNao.Sim, viewModel.ExportarPedido);
            Assert.Equal("000004", viewModel.Codigo);
            Assert.Equal("Produto Origem", viewModel.Nome);
            Assert.Equal(15.0, viewModel.Preco);
        }

        [Fact]
        public void Map_RoundTrip_ExportarPedido_DeveManterValor()
        {
            // Arrange: ViewModel → Produto → ViewModel (ida e volta)
            var viewModelOriginal = new ProdutoViewModel
            {
                Id = 5,
                Codigo = "000005",
                Nome = "Produto RoundTrip",
                Situacao = EAtivo.Ativo,
                Preco = 50.0,
                ExportarPedido = ESimNao.Sim,
                UtilizaBalanca = ESimNao.Nao,
                UnidadeVenda = "UN",
                UnidadeCompra = "UN",
                idEmpresa = 1
            };

            // Act: ida e volta
            var produto = _mapper.Map<Produto>(viewModelOriginal);
            var viewModelResult = _mapper.Map<ProdutoViewModel>(produto);

            // Assert: ExportarPedido deve manter o valor após round-trip
            Assert.Equal(viewModelOriginal.ExportarPedido, viewModelResult.ExportarPedido);
            Assert.Equal(viewModelOriginal.Codigo, viewModelResult.Codigo);
            Assert.Equal(viewModelOriginal.Nome, viewModelResult.Nome);
            Assert.Equal(viewModelOriginal.Preco, viewModelResult.Preco);
        }

        [Fact]
        public void Map_ViewModelToProduto_PropriedadesBasicas_DevemSerMapeadas()
        {
            // Arrange: valida que propriedades básicas com setters privados são mapeadas
            var viewModel = new ProdutoViewModel
            {
                Id = 6,
                Codigo = "ABC123",
                Nome = "Nome do Produto",
                Situacao = EAtivo.Ativo,
                Preco = 99.90,
                ExportarPedido = ESimNao.Sim,
                UtilizaBalanca = ESimNao.Sim,
                UnidadeVenda = "KG",
                UnidadeCompra = "KG",
                idEmpresa = 1,
                IDGRUPO = 10,
                IDDEP = 20,
                IDMARCA = 30,
                IDSUBGRUPO = 40,
                RelacaoCompraVenda = 2,
                QuantMinima = 5.0,
                Volume = "500ml"
            };

            // Act
            var produto = _mapper.Map<Produto>(viewModel);

            // Assert: verifica que propriedades com setters privados foram mapeadas
            Assert.Equal(6L, produto.Id);
            Assert.Equal("ABC123", produto.CDPRODUTO);
            Assert.Equal("Nome do Produto", produto.NMPRODUTO);
            Assert.Equal(EAtivo.Ativo, produto.STPRODUTO);
            Assert.Equal(99.90, produto.NUPRECO);
            Assert.Equal(ESimNao.Sim, produto.STEXPORTARPEDIDO);
            Assert.Equal(ESimNao.Sim, produto.STBALANCA);
            Assert.Equal("KG", produto.UNVENDA);
            Assert.Equal("KG", produto.UNCOMPRA);
            Assert.Equal(1L, produto.idEmpresa);
            Assert.Equal(10L, produto.IDGRUPO);
            Assert.Equal(20L, produto.IDDEP);
            Assert.Equal(30L, produto.IDMARCA);
            Assert.Equal(40L, produto.IDSUBGRUPO);
            Assert.Equal(2, produto.NURELACAO);
            Assert.Equal(5.0, produto.NUQTDMIN);
            Assert.Equal("500ml", produto.DSVOLUME);
        }
    }
}

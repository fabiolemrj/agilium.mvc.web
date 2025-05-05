using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace agilium.api.business.Models
{
    public class Produto: Entity
    {
        public Int64? idEmpresa { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public Int64? IDGRUPO { get; private set; }
        public virtual GrupoProduto GrupoProduto { get; private set; }
        public string CDPRODUTO { get; private set; }
        public string NMPRODUTO { get; private set; }
        public string CTPRODUTO { get; private set; }
        public ETipoProduto? TPPRODUTO { get; private set; }
        public string UNCOMPRA { get; private set; }
        public string UNVENDA { get; private set; }
        public int? NURELACAO { get; private set; }
        public double? NUPRECO { get; private set; }
        public double? NUQTDMIN { get; private set; }
        public string CDSEFAZ { get; private set; }
        public string CDANP { get; private set; }
        public string CDNCM { get; private set; }
        public string CDCEST { get; private set; }
        public string CDSERV { get; private set; }
        public EAtivo? STPRODUTO { get; private set; }
        public double? VLULTIMACOMPRA { get; private set; }
        public double? VLCUSTOMEDIO { get; private set; }
        public double? PCIBPTFED { get; private set; }
        public double? PCIBPTEST { get; private set; }
        public double? PCIBPTMUN { get; private set; }
        public double? PCIBPTIMP { get; private set; }
        public int? NUCFOP { get; private set; }
        public EOrigemProduto? STORIGEMPROD { get; private set; }
        public string DSICMS_CST { get; private set; }
        public double? PCICMS_ALIQ { get; private set; }
        public double? PCICMS_REDUCBC { get; private set; }
        public double? PCICMSST_ALIQ { get; private set; }
        public double? PCICMSST_MVA { get; private set; }
        public double? PCICMSST_REDUCBC { get; private set; }
        public string DSIPI_CST { get; private set; }
        public double? PCIPI_ALIQ { get; private set; }
        public string DSPIS_CST { get; private set; }
        public double? PCPIS_ALIQ { get; private set; }
        public string DSCOFINS_CST { get; private set; }
        public double? PCCOFINS_ALIQ { get; private set; }
        public int? STESTOQUE { get; private set; }
        public ESimNao? STBALANCA { get; private set; }
        public ESimNao? FLG_IFOOD { get; private set; }
        public long? IDMARCA { get; private set; }
        public virtual ProdutoMarca ProdutoMarca { get; private set; }
        public long? IDDEP { get; private set; }
        public virtual ProdutoDepartamento ProdutoDepartamento { get; private set; }
        public long? IDSUBGRUPO { get; private set; }
        public virtual SubGrupoProduto SubGrupoProduto { get; private set; }
        public string DSVOLUME { get; private set; }

        #region Listas virtuais
        public virtual List<ProdutoComposicao> ProdutosComposicoes { get; set; } = new List<ProdutoComposicao>();
        public virtual List<ProdutoCodigoBarra> ProdutoCodigoBarras { get; set; } = new List<ProdutoCodigoBarra>();
        public virtual List<ProdutoPreco> ProdutoPrecos { get; set; } = new List<ProdutoPreco>();
        public virtual List<EstoqueProduto> EstoqueProdutos { get; set; } = new List<EstoqueProduto>();
        public virtual List<EstoqueHistorico> EstoqueHistoricos { get; set; } = new List<EstoqueHistorico>();
        public virtual List<ProdutoFoto> ProdutoFotos { get; set; } = new List<ProdutoFoto>();
        public virtual List<ClientePreco> ClientePrecos { get; set; } = new List<ClientePreco>();
        public virtual List<TurnoPreco> TurnoPrecos { get; set; } = new List<TurnoPreco>();
        public virtual List<VendaItem> VendaItem { get; private set; } = new List<VendaItem>();
        public virtual List<Perda> Perdas { get; set; } = new List<Perda>();
        public virtual List<CompraItem> CompraItems { get; set; } = new List<CompraItem>();
        public virtual List<InventarioItem> InventarioItem { get; set; } = new List<InventarioItem>();
        public virtual List<ProdutoSiteMercado> ProdutoSiteMercado { get; set; } = new List<ProdutoSiteMercado>();
        public virtual List<PedidoItem> Pedidoitens { get; set; } = new List<PedidoItem>();
        public virtual List<VendaTemporariaItem> VendaTemporariaItem { get; set; }= new List<VendaTemporariaItem>();
        #endregion

        public Produto()
        {            
        }

        public void AdicionarSubGrupo(SubGrupoProduto subGrupo) => SubGrupoProduto = subGrupo;
        public void AdicionarDepartamento(ProdutoDepartamento departamento) => ProdutoDepartamento = departamento;
        public void AdicionarMarca(ProdutoMarca marca) => ProdutoMarca = marca;
        public void AdicionarGrupo(GrupoProduto grupo) => GrupoProduto = grupo;

        public void AdicionarCategoria(ECategoriaProduto categoriaProduto)
        {
            if (categoriaProduto == ECategoriaProduto.Simples)
                CTPRODUTO = "1";
            else if (categoriaProduto == ECategoriaProduto.Composto)
                CTPRODUTO = "2";
            else if (categoriaProduto == ECategoriaProduto.Combo)
                CTPRODUTO = "3";
            else if (categoriaProduto == ECategoriaProduto.Insumo)
                CTPRODUTO = "4";           
        }
        public Produto(long? idEmpresa, long? iDGRUPO, string cDPRODUTO, string nMPRODUTO, ECategoriaProduto cTPRODUTO, ETipoProduto? tPPRODUTO, string uNCOMPRA, string uNVENDA, int? nURELACAO, double? nUPRECO, double? nUQTDMIN, string cDSEFAZ, string cDANP, string cDNCM, string cDCEST, string cDSERV, EAtivo? sTPRODUTO, double? vLULTIMACOMPRA, double? vLCUSTOMEDIO, double? pCIBPTFED, double? pCIBPTEST, double? pCIBPTMUN, double? pCIBPTIMP, int? nUCFOP, EOrigemProduto? sTORIGEMPROD, string dSICMS_CST, double? pCICMS_ALIQ, double? pCICMS_REDUCBC, double? pCICMSST_ALIQ, double? pCICMSST_MVA, double? pCICMSST_REDUCBC, string dSIPI_CST, double? pCIPI_ALIQ, string dSPIS_CST, double? pCPIS_ALIQ, string dSCOFINS_CST, double? pCCOFINS_ALIQ, int? sTESTOQUE, ESimNao? sTBALANCA, ESimNao? fLG_IFOOD, long? iDMARCA, long? iDDEP, long? iDSUBGRUPO, string dSVOLUME)
        {
            this.idEmpresa = idEmpresa;
            IDGRUPO = iDGRUPO;
            CDPRODUTO = cDPRODUTO;
            NMPRODUTO = nMPRODUTO;
            CTPRODUTO = cTPRODUTO.ToString();
            TPPRODUTO = tPPRODUTO;
            UNCOMPRA = uNCOMPRA;
            UNVENDA = uNVENDA;
            NURELACAO = nURELACAO;
            NUPRECO = nUPRECO;
            NUQTDMIN = nUQTDMIN;
            CDSEFAZ = cDSEFAZ;
            CDANP = cDANP;
            CDNCM = cDNCM;
            CDCEST = cDCEST;
            CDSERV = cDSERV;
            STPRODUTO = sTPRODUTO;
            VLULTIMACOMPRA = vLULTIMACOMPRA;
            VLCUSTOMEDIO = vLCUSTOMEDIO;
            PCIBPTFED = pCIBPTFED;
            PCIBPTEST = pCIBPTEST;
            PCIBPTMUN = pCIBPTMUN;
            PCIBPTIMP = pCIBPTIMP;
            NUCFOP = nUCFOP;
            STORIGEMPROD = sTORIGEMPROD;
            DSICMS_CST = dSICMS_CST;
            PCICMS_ALIQ = pCICMS_ALIQ;
            PCICMS_REDUCBC = pCICMS_REDUCBC;
            PCICMSST_ALIQ = pCICMSST_ALIQ;
            PCICMSST_MVA = pCICMSST_MVA;
            PCICMSST_REDUCBC = pCICMSST_REDUCBC;
            DSIPI_CST = dSIPI_CST;
            PCIPI_ALIQ = pCIPI_ALIQ;
            DSPIS_CST = dSPIS_CST;
            PCPIS_ALIQ = pCPIS_ALIQ;
            DSCOFINS_CST = dSCOFINS_CST;
            PCCOFINS_ALIQ = pCCOFINS_ALIQ;
            STESTOQUE = sTESTOQUE;
            STBALANCA = sTBALANCA;
            FLG_IFOOD = fLG_IFOOD;
            IDMARCA = iDMARCA;
            IDDEP = iDDEP;
            IDSUBGRUPO = iDSUBGRUPO;
            DSVOLUME = dSVOLUME;
        }
    }
}

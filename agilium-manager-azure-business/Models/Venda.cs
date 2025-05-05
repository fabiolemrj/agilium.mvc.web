using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Venda: Entity
    {
        public Int64? IDCAIXA { get; private set; }
        public virtual Caixa Caixa { get; private set; }
        public Int64? IDCLIENTE { get; private set; }
        public virtual Cliente Cliente { get; private set; }
        public int? SQVENDA { get; private set; }
        public DateTime? DTHRVENDA { get; private set; }
        public string NUCPFCNPJ { get; private set; }
        public double? VLVENDA { get; private set; }
        public double? VLDESC { get; private set; }
        public double? VLTOTAL { get; private set; }
        public double? VLACRES { get; private set; }
        public ESituacaoVenda? STVENDA { get; private set; }
        public string DSINFCOMPL { get; private set; }
        public double? VLTOTIBPTFED { get; private set; }
        public double? VLTOTIBPTEST { get; private set; }
        public double? VLTOTIBPTMUN { get; private set; }
        public double? VLTOTIBPTIMP { get; private set; }
        public int? NUNF { get; private set; }
        public string DSSERIE { get; private set; }
        public ETipoDocVenda? TPDOC { get; private set; }
        public ETipoEmissaoVenda? STEMISSAO { get; private set; }
        public string DSCHAVEACESSO { get; private set; }
        public EOrigemVenda? TPORIGEM { get; private set; }
        public virtual List<VendaCancelada> VendaCancelada { get; private set; } = new List<VendaCancelada>();
        public virtual List<VendaEspelho> VendaEspelho { get; private set; }=new List<VendaEspelho>();
        public virtual List<VendaFiscal> VendaFiscal { get; private set; }= new List<VendaFiscal>();
        public virtual List<VendaItem> VendaItem { get; private set; } = new List<VendaItem>();
        public virtual List<VendaMoeda> VendaMoeda { get; private set; } = new List<VendaMoeda>();
        public virtual List<Devolucao> Devolucao { get; set; } = new List<Devolucao>();
        public virtual List<PedidoSitemercado> PedidosSiteMercado { get; set; } = new List<PedidoSitemercado>();
        public virtual List<PedidoVenda> PedidosVendas { get; set; } = new List<PedidoVenda>();
        public Venda()
        {            
        }

        public void AdiciconarVendaEspelho(VendaEspelho vendaEspelho) => VendaEspelho.Add(vendaEspelho);
        public void AdicionarItem(VendaItem item) => VendaItem.Add(item);
        public void AdicionarMoeda(VendaMoeda vendaMoeda) => VendaMoeda.Add(vendaMoeda);
        public void AdicionarVendaCancelada(VendaCancelada vendaCancelada) => VendaCancelada.Add(vendaCancelada);
        public void AddCaixa(Caixa caixa) => Caixa = caixa;
        public void AdicionarInformacaoComplementar(string valor) => DSINFCOMPL = DSINFCOMPL + valor;
        public void AdicionarOrigemVenda(EOrigemVenda valor) => TPORIGEM = valor;
        public void MudarSituacaoAtivo() => STVENDA = ESituacaoVenda.Ativo;
        public void MudarSituacaoInativo() => STVENDA = ESituacaoVenda.Inativo;

        public void AdicionarIbpt(double? vLTOTIBPTFED, double? vLTOTIBPTEST, double? vLTOTIBPTMUN, double? vLTOTIBPTIMP)
        {
            VLTOTIBPTFED = vLTOTIBPTFED;
            VLTOTIBPTEST = vLTOTIBPTEST;
            VLTOTIBPTMUN = vLTOTIBPTMUN;
            VLTOTIBPTIMP = vLTOTIBPTIMP;
        }

        public Venda(long? iDCAIXA, long? iDCLIENTE, int? sQVENDA, DateTime? dTHRVENDA, string nUCPFCNPJ, double? vLVENDA, double? vLDESC, double? vLTOTAL, double? vLACRES, ESituacaoVenda? sTVENDA, string dSINFCOMPL, double? vLTOTIBPTFED, double? vLTOTIBPTEST, double? vLTOTIBPTMUN, double? vLTOTIBPTIMP, int? nUNF, string dSSERIE, ETipoDocVenda? tPDOC, ETipoEmissaoVenda? sTEMISSAO, string dSCHAVEACESSO)
        {
            IDCAIXA = iDCAIXA;
            IDCLIENTE = iDCLIENTE;
            SQVENDA = sQVENDA;
            DTHRVENDA = dTHRVENDA;
            NUCPFCNPJ = nUCPFCNPJ;
            VLVENDA = vLVENDA;
            VLDESC = vLDESC;
            VLTOTAL = vLTOTAL;
            VLACRES = vLACRES;
            STVENDA = sTVENDA;
            DSINFCOMPL = dSINFCOMPL;
            VLTOTIBPTFED = vLTOTIBPTFED;
            VLTOTIBPTEST = vLTOTIBPTEST;
            VLTOTIBPTMUN = vLTOTIBPTMUN;
            VLTOTIBPTIMP = vLTOTIBPTIMP;
            NUNF = nUNF;
            DSSERIE = dSSERIE;
            TPDOC = tPDOC;
            STEMISSAO = sTEMISSAO;
            DSCHAVEACESSO = dSCHAVEACESSO;
        }

        public Venda(long? iDCAIXA, long? iDCLIENTE, int? sQVENDA, DateTime? dTHRVENDA, string nUCPFCNPJ, double? vLVENDA, double? vLDESC, double? vLTOTAL, double? vLACRES, ESituacaoVenda? sTVENDA, string dSINFCOMPL, double? vLTOTIBPTFED, double? vLTOTIBPTEST, double? vLTOTIBPTMUN, double? vLTOTIBPTIMP, int? nUNF, string dSSERIE, ETipoDocVenda? tPDOC, ETipoEmissaoVenda? sTEMISSAO, string dSCHAVEACESSO, EOrigemVenda? tPORIGEM) : this(iDCAIXA, iDCLIENTE, sQVENDA, dTHRVENDA, nUCPFCNPJ, vLVENDA, vLDESC, vLTOTAL, vLACRES, sTVENDA, dSINFCOMPL, vLTOTIBPTFED, vLTOTIBPTEST, vLTOTIBPTMUN, vLTOTIBPTIMP, nUNF, dSSERIE, tPDOC, sTEMISSAO, dSCHAVEACESSO)
        {
            TPORIGEM = tPORIGEM;
        }
    }
}

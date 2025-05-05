using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Models
{
    public class VendaTemporaria: Entity
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
        public int? STVENDA { get; private set; }
        public string DSINFCOMPL { get; private set; }
        public double? VLTOTIBPTFED { get; private set; }
        public double? VLTOTIBPTEST { get; private set; }
        public double? VLTOTIBPTMUN { get; private set; }
        public double? VLTOTIBPTIMP { get; private set; }
        public int? NUNF { get; private set; }
        public string DSSERIE { get; private set; }
        public int? TPDOC { get; private set; }
        public int? STEMISSAO { get; private set; }
        public string DSCHAVEACESSO { get; private set; }
        public EOrigemVenda? TPORIGEM { get; private set; }
        public virtual List<VendaTemporariaEspelho> VendaTemporariaEspelho { get; set; }= new List<VendaTemporariaEspelho>();
        public virtual List<VendaTemporariaItem> VendaTemporariaItem { get; set; }= new List<VendaTemporariaItem>();
        public virtual List<VendaTemporariaMoeda> VendaTemporariaMoeda { get; set; } = new List<VendaTemporariaMoeda>();
        
        public VendaTemporaria()
        {
        }

        public VendaTemporaria(int? sQVENDA, DateTime? dTHRVENDA, string nUCPFCNPJ, double? vLVENDA, double? vLDESC, double? vLTOTAL, double? vLACRES, int? sTVENDA, string dSINFCOMPL, double? vLTOTIBPTFED, double? vLTOTIBPTEST, double? vLTOTIBPTMUN, double? vLTOTIBPTIMP, int? nUNF, string dSSERIE, int? tPDOC, int? sTEMISSAO, string dSCHAVEACESSO, EOrigemVenda? tPORIGEM)
        {

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
            TPORIGEM = tPORIGEM;
        }

        public void AdicionarItem(VendaTemporariaItem vendaItem) => VendaTemporariaItem.Add(vendaItem);
        public void AdicionarEspelho(VendaTemporariaEspelho vendaEspelho) => VendaTemporariaEspelho.Add(vendaEspelho);
        public void AdicionarMoeda(VendaTemporariaMoeda vendaMoeda) => VendaTemporariaMoeda.Add(vendaMoeda);

    }
}

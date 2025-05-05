using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class ContaReceber: Entity
    {
        public Int64? IDCONTAPAI { get; private set; }
        public virtual ContaReceber ContaPai { get; private set; }
        public Int64? IDCLIENTE { get; private set; }
        public virtual Cliente Cliente { get; private set; }
        public Int64? IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public Int64? IDCATEG_FINANC { get; private set; }
        public virtual CategoriaFinanceira CategFinanc { get; private set; }
        public Int64? IDUSUARIO { get; private set; }
        public virtual Usuario Usuario { get; set; }
        public Int64? IDLANC { get; private set; }
        public virtual PlanoContaLancamento PlanoContaLancamento { get; private set; }
        public DateTime? DTVENC { get; private set; }
        public DateTime? DTPAG { get; private set; }
        public double? VLCONTA { get; private set; }
        public string DESCR { get; private set; }
        public double? VLDESC { get; private set; }
        public double? VLACRES { get; private set; }
        public int? PARCINI { get; private set; }
        public Int64? IDCONTA { get; private set; }
        public virtual PlanoConta PlanoConta { get; private set; }
        public int? STCONTA { get; private set; }
        public int? TPCONTA { get; private set; }
        public string OBS { get; private set; }
        public string NUMNF { get; private set; }
        public DateTime? DTNF { get; private set; }
        public DateTime? DTCAD { get; private set; } = DateTime.Now;
        public virtual List<ContaReceber> ContaReceberPai { get; set; } = new List<ContaReceber>();
        public ContaReceber()
        {
            
        }

        public ContaReceber(long? iDCONTAPAI, long? iDCLIENTE, long? iDEMPRESA, long? iDCATEG_FINANC, long? iDUSUARIO, long? iDLANC, DateTime? dTVENC, DateTime? dTPAG, double? vLCONTA, string dESCR, double? vLDESC, double? vLACRES, int? pARCINI, long? iDCONTA, int? sTCONTA, int? tPCONTA, string oBS, string nUMNF, DateTime? dTNF, DateTime? dTCAD)
        {
            IDCONTAPAI = iDCONTAPAI;
            IDCLIENTE = iDCLIENTE;
            IDEMPRESA = iDEMPRESA;
            IDCATEG_FINANC = iDCATEG_FINANC;
            IDUSUARIO = iDUSUARIO;
            IDLANC = iDLANC;
            DTVENC = dTVENC;
            DTPAG = dTPAG;
            VLCONTA = vLCONTA;
            DESCR = dESCR;
            VLDESC = vLDESC;
            VLACRES = vLACRES;
            PARCINI = pARCINI;
            IDCONTA = iDCONTA;
            STCONTA = sTCONTA;
            TPCONTA = tPCONTA;
            OBS = oBS;
            NUMNF = nUMNF;
            DTNF = dTNF;
            DTCAD = dTCAD;
        }
    }
}

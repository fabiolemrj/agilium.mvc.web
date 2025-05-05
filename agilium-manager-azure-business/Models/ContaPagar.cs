using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class ContaPagar: Entity
    {
        public Int64? IDCONTAPAI { get; private set; }
        public virtual ContaPagar ContaPagaPai { get; private set; }
        public Int64? IDCATEG_FINANC { get; private set; }
        public virtual CategoriaFinanceira CategFinanc { get; private set; }
        public Int64? IDUSUARIO { get; private set; }
        public virtual Usuario Usuario { get; private set; }
        public Int64? IDFORNEC { get; private set; }
        public virtual Fornecedor Fornecedor { get; private set; }
        public Int64? IDCONTA { get; private set; }
        public virtual PlanoConta PlanoConta { get; private set; }
        public Int64? IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public Int64? IDLANC { get; private set; }
        public virtual PlanoContaLancamento PlanoContaLancamento { get; private set; }
        public string DESCR { get; private set; }
        public DateTime? DTVENC { get; private set; }
        public DateTime? DTPAG { get; private set; }
        public double? VLCONTA { get; private set; }
        public double? VLDESC { get; private set; }
        public double? VLACRESC { get; private set; }
        public int? PARCINI { get; private set; }
        public int? TPCONTA { get; private set; }
        public int? STCONTA { get; private set; }
        public string OBS { get; private set; }
        public string NUMNF { get; private set; }
        public DateTime? DTNF { get; private set; }
        public DateTime? DTCAD { get; private set; }
        public virtual List<ContaPagar> ContasPagar { get; set; } = new List<ContaPagar>();
        public ContaPagar()
        {
            
        }

        public ContaPagar(long? iDCONTAPAI, long? iDCATEG_FINANC, long? iDUSUARIO, long? iDFORNEC, long? iDCONTA, long? iDEMPRESA, long? iDLANC, string dESCR, DateTime? dTVENC, DateTime? dTPAG, double? vLCONTA, double? vLDESC, double? vLACRESC, int? pARCINI, int? tPCONTA, int? sTCONTA, string oBS, string nUMNF, DateTime? dTNF, DateTime? dTCAD)
        {
            IDCONTAPAI = iDCONTAPAI;
            IDCATEG_FINANC = iDCATEG_FINANC;
            IDUSUARIO = iDUSUARIO;
            IDFORNEC = iDFORNEC;
            IDCONTA = iDCONTA;
            IDEMPRESA = iDEMPRESA;
            IDLANC = iDLANC;
            DESCR = dESCR;
            DTVENC = dTVENC;
            DTPAG = dTPAG;
            VLCONTA = vLCONTA;
            VLDESC = vLDESC;
            VLACRESC = vLACRESC;
            PARCINI = pARCINI;
            TPCONTA = tPCONTA;
            STCONTA = sTCONTA;
            OBS = oBS;
            NUMNF = nUMNF;
            DTNF = dTNF;
            DTCAD = dTCAD;
        }
    }
}

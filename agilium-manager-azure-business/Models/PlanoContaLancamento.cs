using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class PlanoContaLancamento: Entity
    {
        public Int64? IDCONTA { get; private set; }
        public virtual PlanoConta PlanoConta { get; private set; }
        public DateTime? DTCAD { get; private set; }
        public DateTime? DTREF { get; private set; }
        public int? NUANOMESREF { get; private set; }
        public string DSLANC { get; private set; }
        public double? VLLANC { get; private set; }
        public ETipoContaLancacmento? TPLANC { get; private set; }
        public EAtivo? STLANC { get; private set; }
        public virtual List<ContaPagar> ContaPagar { get; set; } = new List<ContaPagar>();
        public virtual List<ContaReceber> ContaReceber { get; set; } = new List<ContaReceber>();
        public virtual List<EstoqueHistorico> EstoquesHistoricos { get; set; } = new List<EstoqueHistorico>();
        public PlanoContaLancamento()
        {
            
        }

        public PlanoContaLancamento(long? iDCONTA, DateTime? dTCAD, DateTime? dTREF, int? nUANOMESREF, string dSLANC, double? vLLANC, ETipoContaLancacmento? tPLANC, EAtivo? sTLANC)
        {
            IDCONTA = iDCONTA;
            DTCAD = dTCAD;
            DTREF = dTREF;
            NUANOMESREF = nUANOMESREF;
            DSLANC = dSLANC;
            VLLANC = vLLANC;
            TPLANC = tPLANC;
            STLANC = sTLANC;
        }
    }
}

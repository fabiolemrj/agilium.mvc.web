using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class PlanoConta: Entity
    {
        public Int64? IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public Int64? IDCONTAPAI { get; private set; }
        public virtual PlanoConta PlanoContaPai { get; private set; }
        public string CDCONTA { get; private set; }
        public string DSCONTA { get; private set; }
        public ETipoContaLancacmento? TPCONTA { get; private set; }
        public EAtivo? STCONTA { get; private set; }

        public virtual List<PlanoConta> PlanoContasFilho { get; set; } = new List<PlanoConta>();
        public virtual List<PlanoContaSaldo> PlanoContaSaldos { get; set; } = new List<PlanoContaSaldo>();
        public virtual List<PlanoContaLancamento> PlanoContaLancamentos { get; set; } = new List<PlanoContaLancamento>();
        public virtual List<ContaPagar> ContaPagar { get; set; } = new List<ContaPagar>();
        public virtual List<ContaReceber> ContaReceber { get; set; } = new List<ContaReceber>();

        public PlanoConta()
        {
            
        }
        public PlanoConta(long? iDEMPRESA, long? iDCONTAPAI, string cDCONTA, string dSCONTA, ETipoContaLancacmento? tPCONTA, EAtivo? sTCONTA)
        {
            IDEMPRESA = iDEMPRESA;
            IDCONTAPAI = iDCONTAPAI;
            CDCONTA = cDCONTA;
            DSCONTA = dSCONTA;
            TPCONTA = tPCONTA;
            STCONTA = sTCONTA;
        }
    }
}

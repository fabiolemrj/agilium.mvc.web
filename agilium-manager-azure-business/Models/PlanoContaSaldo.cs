using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class PlanoContaSaldo: Entity
    {

        public Int64? IDCONTA { get; private set; }
        public virtual PlanoConta PlanoConta { get; private set; }
        public DateTime? DTHRATU { get; private set; }
        public int? NUANOMESREF { get; private set; }
        public double? VLSALDO { get; private set; }
        public PlanoContaSaldo()
        {
            
        }

        public PlanoContaSaldo(long? iDCONTA, DateTime? dTHRATU, int? nUANOMESREF, double? vLSALDO)
        {
            IDCONTA = iDCONTA;
            DTHRATU = dTHRATU;
            NUANOMESREF = nUANOMESREF;
            VLSALDO = vLSALDO;
        }
    }
}

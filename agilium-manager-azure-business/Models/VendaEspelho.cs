using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class VendaEspelho: Entity
    {
        public Int64? IDVENDA { get; private set; }
        public virtual Venda Venda { get; private set; }
        public string DSESPELHO { get; private set; }
        public VendaEspelho()
        {            
        }

        public VendaEspelho(long? iDVENDA, string dSESPELHO)
        {
            IDVENDA = iDVENDA;
            DSESPELHO = dSESPELHO;
        }
    }
}

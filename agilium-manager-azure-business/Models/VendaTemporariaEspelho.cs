using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Models
{
    public class VendaTemporariaEspelho : Entity
    {
        public Int64? IDVENDA { get; private set; }
        public virtual VendaTemporaria Venda { get; private set; }
        public string DSESPELHO { get; private set; }

        public VendaTemporariaEspelho()
        {

        }
        public VendaTemporariaEspelho(string dSESPELHO)
        {
            DSESPELHO = dSESPELHO;
        }
    }
}

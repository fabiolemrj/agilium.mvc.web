using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class ClienteContato: Entity
    {
        public long IDCLIENTE { get; private set; }
        public virtual Cliente Cliente { get; set; }
        public long IDCONTATO { get; private set; }
        public virtual Contato Contato { get; set; }

        public ClienteContato()
        {
            
        }
        public ClienteContato(long iDCLIENTE, long iDCONTATO)
        {
            IDCLIENTE = iDCLIENTE;
            IDCONTATO = iDCONTATO;
        }
    }
}

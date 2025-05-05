using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Models
{
    public class VendaTemporariaMoeda : Entity
    {
        public Int64? IDVENDA { get; private set; }
        public virtual VendaTemporaria Venda { get; private set; }
        public Int64? IDMOEDA { get; private set; }
        public virtual Moeda Moeda { get; private set; }
        public Int64? IDVALE { get; private set; }
        public virtual Vale Vale { get; private set; }
        public double? VLPAGO { get; private set; }
        public double? VLTROCO { get; private set; }
        public int? NUPARCELAS { get; private set; }
        public string NSU { get; private set; }
        public VendaTemporariaMoeda()
        {

        }

        public VendaTemporariaMoeda(long? iDVALE, double? vLPAGO, double? vLTROCO, int? nUPARCELAS, string nSU)
        {
            IDVALE = iDVALE;
            VLPAGO = vLPAGO;
            VLTROCO = vLTROCO;
            NUPARCELAS = nUPARCELAS;
            NSU = nSU;
        }
    }
}

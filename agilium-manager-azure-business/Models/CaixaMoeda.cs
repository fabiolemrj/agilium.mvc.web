using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class CaixaMoeda: Entity
    {
        public Int64? IDCAIXA { get; private set; }
        public virtual Caixa Caixa { get; private set; }
        public Int64? IDMOEDA { get; private set; }
        public virtual Moeda Moeda { get; private set; }
        public double? VLMOEDAORIGINAL { get; private set; }
        public double? VLMOEDACORRECAO { get; private set; }
        public Int64? IDUSUARIOCORRECAO { get; private set; }
        public virtual Usuario UsuarioCorrecao { get; private set; }
        public DateTime? DTHRCORRECAO { get; private set; }
        public CaixaMoeda()
        {            
        }

        public CaixaMoeda(long? iDCAIXA, long? iDMOEDA, double? vLMOEDAORIGINAL, double? vLMOEDACORRECAO, long? iDUSUARIOCORRECAO, DateTime? dTHRCORRECAO)
        {
            IDCAIXA = iDCAIXA;
            IDMOEDA = iDMOEDA;
            VLMOEDAORIGINAL = vLMOEDAORIGINAL;
            VLMOEDACORRECAO = vLMOEDACORRECAO;
            IDUSUARIOCORRECAO = iDUSUARIOCORRECAO;
            DTHRCORRECAO = dTHRCORRECAO;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class MotivoDevolucao : Entity
    {
        public long? IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public string DSMOTDEV { get; private set; }
        public int? STMOTDEV { get; private set; }
        public virtual List<Devolucao> Devolucao { get; private set; } = new List<Devolucao>();
        public MotivoDevolucao()
        {
        }

        public MotivoDevolucao(string dSMOTDEV, int? sTMOTDEV)
        {
       
            DSMOTDEV = dSMOTDEV;
            STMOTDEV = sTMOTDEV;
        }
    }
}

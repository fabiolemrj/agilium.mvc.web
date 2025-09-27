using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Licenca: Entity
    {
        public Int64? IDEMPRESA { get; private set; }
        public string K1 { get; private set; }
        public string K2 { get; private set; }
        public string K3 { get; private set; }
        public string K4 { get; private set; }
        public string K5 { get; private set; }
        public string K6 { get; private set; }
        public string K7 { get; private set; }

        public Licenca(long? iDEMPRESA, string k1, string k2, string k3, string k4, string k5, string k6, string k7)
        {
            IDEMPRESA = iDEMPRESA;
            K1 = k1;
            K2 = k2;
            K3 = k3;
            K4 = k4;
            K5 = k5;
            K6 = k6;
            K7 = k7;
        }
        public Licenca()
        {
            
        }
    }
}

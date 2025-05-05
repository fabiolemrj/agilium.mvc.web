using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Config : Entity
    {
        public string CHAVE { get; private set; }
        public long? IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public string VALOR { get; private set; }

        public Config()
        {

        }

        public Config(string cHAVE, long? iDEMPRESA, string vALOR)
        {
            CHAVE = cHAVE;
            IDEMPRESA = iDEMPRESA;
            VALOR = vALOR;
        }
    }
}

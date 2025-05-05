using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class ConfigImagem : Entity
    {
        public string CHAVE { get; set; }
        public long? IDEMPRESA { get; set; }
        public virtual Empresa Empresa { get; set; }
        public byte[] IMG { get; set; }

        public ConfigImagem(string cHAVE, long? iDEMPRESA, byte[] iMG)
        {
            CHAVE = cHAVE;
            IDEMPRESA = iDEMPRESA;
            IMG = iMG;
        }

        public ConfigImagem()
        {

        }
    }
}

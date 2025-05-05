using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class EmpresaAuth: Entity
    {
        public long IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; set; }
        public long IDUSUARIO { get; private set; }
        public virtual Usuario Usuario { get; set; }

        public EmpresaAuth()
        {

        }

        public EmpresaAuth(long iDEMPRESA, long iDUSUARIO)
        {
            IDEMPRESA = iDEMPRESA;
            IDUSUARIO = iDUSUARIO;
        }
    }
}

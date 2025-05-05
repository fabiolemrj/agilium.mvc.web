using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class ContatoEmpresa: Entity
    {
        public long IDCONTATO { get; private set; }
        public virtual Contato Contato { get; private set; }
        public long IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public ContatoEmpresa()
        {

        }

        public ContatoEmpresa(long iDCONTATO, long iDEMPRESA)
        {
            IDCONTATO = iDCONTATO;
            IDEMPRESA = iDEMPRESA;
        }

        public ContatoEmpresa(Contato contato, Empresa empresa)
        {
            Contato = contato;
            Empresa = empresa;
        }

        public void PopularContato(long idContato) => IDCONTATO = idContato;
        public void PopularEmpresa(long idEmpresa) => IDEMPRESA = idEmpresa;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class ClientePJ: Entity
    {
        public virtual Cliente Cliente { get; private set; }
        public string NMRZSOCIAL { get; private set; }
        public string NUCNPJ { get; private set; }
        public string DSINSCREST { get; private set; }
        public ClientePJ()
        {            
        }

        public ClientePJ(string nMRZSOCIAL, string nUCNPJ, string dSINSCREST)
        {
            NMRZSOCIAL = nMRZSOCIAL;
            NUCNPJ = nUCNPJ;
            DSINSCREST = dSINSCREST;
        }

        public void AdicionarCliente(Cliente cliente) => Cliente = cliente;
    }
}

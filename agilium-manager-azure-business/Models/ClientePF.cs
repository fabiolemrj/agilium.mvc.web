using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class ClientePF: Entity
    {
        public virtual Cliente Cliente { get; private set; }
        public string NUCPF { get; private set; }
        public string NURG { get; private set; }
        public DateTime? DTNASC { get; private set; }

        public ClientePF()
        {
            
        }

        public ClientePF(string nUCPF, string nURG, DateTime? dTNASC)
        {
            NUCPF = nUCPF;
            NURG = nURG;
            DTNASC = dTNASC;
        }

        public void AdicionarCliente (Cliente cliente) => Cliente = cliente;
        public void AdicionarIdCliente(long id)=>Id = id;
       
    }


}

using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class FornecedorContato: Entity
    {
        public long IDCONTATO { get; private set; }
        public virtual Contato Contato { get; private set; }
        public Int64 IDFORN { get; private set; }
        public virtual Fornecedor Fornecedor { get; private set; }
        public FornecedorContato()
        {
            
        }

        public FornecedorContato(long iDCONTATO, long iDFORN)
        {
            IDCONTATO = iDCONTATO;
            IDFORN = iDFORN;
        }

        public FornecedorContato(Contato contato, Fornecedor fornecedor)
        {
            Contato = contato;
            Fornecedor = fornecedor;
        }

        public void AdicionarContato(Contato contato) => Contato = contato;
        public void AdicionarContato(long idContato) => IDCONTATO = idContato;
    }
}

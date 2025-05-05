using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class ProdutoCodigoBarra : Entity
    {
        public virtual Produto Produto { get; set; }
        public Int64? IDPRODUTO { get; private set; }
        public string CDBARRA { get; private set; }
        public ProdutoCodigoBarra()
        {

        }

        public ProdutoCodigoBarra(long? iDPRODUTO, string cDBARRA)
        {
            IDPRODUTO = iDPRODUTO;
            CDBARRA = cDBARRA;
        }

        public void AdicionarCodigoBarra(string cdbarra) => CDBARRA = cdbarra;
    }
}

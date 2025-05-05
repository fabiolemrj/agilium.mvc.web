using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class EstoqueProduto : Entity
    {
        public virtual Produto Produto { get; private set; }
        public Int64? IDPRODUTO { get; private set; }
        public virtual Estoque Estoque { get; private set; }
        public Int64? IDESTOQUE { get; private set; }
        public double? NUQTD { get; private set; }
        public EstoqueProduto()
        {

        }

        public EstoqueProduto(long? iDPRODUTO, long? iDESTOQUE, double? nUQTD)
        {
            IDPRODUTO = iDPRODUTO;
            IDESTOQUE = iDESTOQUE;
            NUQTD = nUQTD;
        }
    }
}

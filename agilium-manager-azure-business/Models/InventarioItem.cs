using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class InventarioItem: Entity
    {
        public Int64? IDINVENT { get; private set; }
        public virtual Inventario Inventario { get; private set; }
        public Int64? IDPRODUTO { get; private set; }
        public virtual Produto Produto { get; private set; }
        public Int64? IDPERDA { get; private set; }
        public virtual Perda Perda { get; private set; }
        public Int64? IDUSUARIOANALISE { get; private set; }
        public virtual Usuario UsuarioAnalise { get; private set; }
        public DateTime? DTHRANALISE { get; private set; }
        public double? NUQTDANALISE { get; private set; }
        public double? NUQTDESTOQUE { get; private set; }
        public double? VLCUSTOMEDIO { get; private set; }
        public InventarioItem()
        {            
        }

        public InventarioItem(long? iDINVENT, long? iDPRODUTO, long? iDPERDA, long? iDUSUARIOANALISE, DateTime? dTHRANALISE, double? nUQTDANALISE, double? nUQTDESTOQUE, double? vLCUSTOMEDIO)
        {
            IDINVENT = iDINVENT;
            IDPRODUTO = iDPRODUTO;
            IDPERDA = iDPERDA;
            IDUSUARIOANALISE = iDUSUARIOANALISE;
            DTHRANALISE = dTHRANALISE;
            NUQTDANALISE = nUQTDANALISE;
            NUQTDESTOQUE = nUQTDESTOQUE;
            VLCUSTOMEDIO = vLCUSTOMEDIO;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class PedidoVendaItem:Entity
    {
        public long IDITEMPEDIDO { get; set; }
        public virtual PedidoItem PedidoItem { get; set; }
        public long IDVENDA_ITEM { get; set; }
        public virtual VendaItem VendaItem { get; set; }
        public PedidoVendaItem()
        {            
        }

        public PedidoVendaItem(long iDITEMPEDIDO, long iDVENDA_ITEM)
        {
            IDITEMPEDIDO = iDITEMPEDIDO;
            IDVENDA_ITEM = iDVENDA_ITEM;
        }
    }
}

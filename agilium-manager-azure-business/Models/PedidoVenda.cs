using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class PedidoVenda: Entity
    {
        public long? IDPedido { get; private set; }
        public long? IDVenda { get; private set; }

        // Relacionamentos
        public virtual Pedido Pedido { get; set; }
        public virtual Venda Venda { get; set; }
        public PedidoVenda()
        {            
        }

        public PedidoVenda(long? iDPedido, long? iDVenda)
        {
            IDPedido = iDPedido;
            IDVenda = iDVenda;
        }
    }
}

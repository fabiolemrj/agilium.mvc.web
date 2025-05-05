using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class PedidoPagamento: Entity
    {
        public long? IDPedido { get; private set; }
        public long? IDFormaPagamento { get; private set; }
        public long? IDMoeda { get; private set; }
        public double? VLPagamento { get; private set; }
        public double? VLTroco { get; private set; }
        public string DSObsPagamento { get; private set; }

        // Relacionamentos
        public virtual Pedido Pedido { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }
        public virtual Moeda Moeda { get; set; }
        public PedidoPagamento()
        {            
        }

        public PedidoPagamento(long? iDPedido, long? iDFormaPagamento, long? iDMoeda, double? vLPagamento, double? vLTroco, string dSObsPagamento)
        {
            IDPedido = iDPedido;
            IDFormaPagamento = iDFormaPagamento;
            IDMoeda = iDMoeda;
            VLPagamento = vLPagamento;
            VLTroco = vLTroco;
            DSObsPagamento = dSObsPagamento;
        }
    }
}

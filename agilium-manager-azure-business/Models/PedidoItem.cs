using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class PedidoItem: Entity
    {     
        public long? IDPedido { get; private set; }
        public virtual Pedido Pedido { get; set; }
        public long? IDProduto { get; private set; }
        public virtual Produto Produto { get; set; }
        public long? IDEstoque { get; private set; }
        public virtual Estoque Estoque { get; set; }
        public long? IDFornecedor { get; private set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public int? SQItemPedido { get; private set; }
        public double? VLUnit { get; private set; }
        public double? NUQtd { get; private set; }
        public double? VLItem { get; private set; }
        public double? VLAcres { get; private set; }
        public double? VLDesc { get; private set; }
        public double? VLOutros { get; private set; }
        public double? VLTotal { get; private set; }
        public double? VLCustoMedio { get; private set; }
        public int? STItemPedido { get; private set; }
        public DateTime? DTPrevEntrega { get; private set; }
        public string DSObsItem { get; private set; }
        public virtual List<PedidoVendaItem> PedidosVendaItems { get; set; } = new List<PedidoVendaItem>();
        public PedidoItem()
        {            
        }

        public PedidoItem(long? iDPedido, long? iDProduto, long? iDEstoque, long? iDFornecedor, int? sQItemPedido, double? vLUnit, double? nUQtd, double? vLItem, double? vLAcres, double? vLDesc, double? vLOutros, double? vLTotal, double? vLCustoMedio, int? sTItemPedido, DateTime? dTPrevEntrega, string dSObsItem)
        {
            IDPedido = iDPedido;
            IDProduto = iDProduto;
            IDEstoque = iDEstoque;
            IDFornecedor = iDFornecedor;
            SQItemPedido = sQItemPedido;
            VLUnit = vLUnit;
            NUQtd = nUQtd;
            VLItem = vLItem;
            VLAcres = vLAcres;
            VLDesc = vLDesc;
            VLOutros = vLOutros;
            VLTotal = vLTotal;
            VLCustoMedio = vLCustoMedio;
            STItemPedido = sTItemPedido;
            DTPrevEntrega = dTPrevEntrega;
            DSObsItem = dSObsItem;
        }
    }
}

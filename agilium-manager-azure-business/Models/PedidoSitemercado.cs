using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class PedidoSitemercado: Entity
    {
        public long? IDEmpresa { get; private set; }
        public long? IDVenda { get; private set; }
        public long? IDCliente { get; private set; }
        public int? IDLoja { get; private set; }
        public string Codigo { get; private set; }
        public string CodigoLoja { get; private set; }
        public DateTime? DTHRPedido { get; private set; }
        public DateTime? DTHRIniAgend { get; private set; }
        public DateTime? DTHRFimAgend { get; private set; }
        public int? Entrega { get; private set; }
        public int? CPFNAnota { get; private set; }
        public string PessoaAutReceb { get; private set; }
        public int? QtdItemUnico { get; private set; }
        public double? VLMercado { get; private set; }
        public double? VLConveniencia { get; private set; }
        public double? VLEntrega { get; private set; }
        public double? VLRetirada { get; private set; }
        public double? VLTroco { get; private set; }
        public double? VLDesconto { get; private set; }
        public double? VLTotal { get; private set; }
        public double? VLCorrigido { get; private set; }
        public string DSJSONPedido { get; private set; }
        public string CDCupom { get; private set; }

        // Relacionamentos
        public virtual Cliente Cliente { get;  set; }
        public virtual Empresa Empresa { get; set; }
        public virtual Venda Venda { get; set; }

        public virtual List<PedidoItemSitemercado> PedidoItemSiteMercado { get; set; } = new List<PedidoItemSitemercado>();
        public virtual List<PedidoPagamentoSitemercado> PedidoPagamentoSitemercados { get; set; } = new List<PedidoPagamentoSitemercado>();
        public PedidoSitemercado()
        {
            
        }

        public PedidoSitemercado(long? iDEmpresa, long? iDVenda, long? iDCliente, int? iDLoja, string codigo, string codigoLoja, DateTime? dTHRPedido, DateTime? dTHRIniAgend, DateTime? dTHRFimAgend, int? entrega, int? cPFNAnota, string pessoaAutReceb, int? qtdItemUnico, double? vLMercado, double? vLConveniencia, double? vLEntrega, double? vLRetirada, double? vLTroco, double? vLDesconto, double? vLTotal, double? vLCorrigido, string dSJSONPedido, string cDCupom)
        {
            IDEmpresa = iDEmpresa;
            IDVenda = iDVenda;
            IDCliente = iDCliente;
            IDLoja = iDLoja;
            Codigo = codigo;
            CodigoLoja = codigoLoja;
            DTHRPedido = dTHRPedido;
            DTHRIniAgend = dTHRIniAgend;
            DTHRFimAgend = dTHRFimAgend;
            Entrega = entrega;
            CPFNAnota = cPFNAnota;
            PessoaAutReceb = pessoaAutReceb;
            QtdItemUnico = qtdItemUnico;
            VLMercado = vLMercado;
            VLConveniencia = vLConveniencia;
            VLEntrega = vLEntrega;
            VLRetirada = vLRetirada;
            VLTroco = vLTroco;
            VLDesconto = vLDesconto;
            VLTotal = vLTotal;
            VLCorrigido = vLCorrigido;
            DSJSONPedido = dSJSONPedido;
            CDCupom = cDCupom;
        }
    }
}

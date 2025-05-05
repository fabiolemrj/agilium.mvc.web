using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Pedido: Entity
    {
        public long IDEmpresa { get; private set; }
        public long IDFuncionario { get; private set; }
        public long IDCliente { get; private set; }
        public long IDEndereco { get; private set; }
        public long IDCaixa { get; private set; }
        public long IDPDV { get; private set; }
        public string CDPedido { get; private set; }
        public DateTime DTPedido { get; private set; }
        public ESituacaoPedido STPedido { get; private set; }
        public double VLPedido { get; private set; }
        public double VLAcres { get; private set; }
        public double VLDesc { get; private set; }
        public double VLOutros { get; private set; }
        public double VLTotal { get; private set; }
        public string DSObs { get; private set; }
        public double NUDistancia { get; private set; }
        public DateTime DTHRConclusao { get; private set; }
        public virtual List<PedidoItem> itens { get; set; } = new List<PedidoItem>();
        public virtual List<PedidoPagamento> PedidoPagamentos { get; set; } = new List<PedidoPagamento>();
        public virtual List<PedidoVenda> PedidosVendas { get; set; } = new List<PedidoVenda>();


        // Relacionamentos
        public virtual Caixa Caixa { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual Endereco Endereco { get; set; }
        public virtual Funcionario Funcionario { get; set; }
        public virtual PontoVenda PDV { get; set; }
        public Pedido()
        {            
        }

        public Pedido(long iDEmpresa, long iDFuncionario, long iDCliente, long iDEndereco, long iDCaixa, long iDPDV, string cDPedido, DateTime dTPedido, ESituacaoPedido sTPedido, double vLPedido, double vLAcres, double vLDesc, double vLOutros, double vLTotal, string dSObs, double nUDistancia, DateTime dTHRConclusao)
        {
            IDEmpresa = iDEmpresa;
            IDFuncionario = iDFuncionario;
            IDCliente = iDCliente;
            IDEndereco = iDEndereco;
            IDCaixa = iDCaixa;
            IDPDV = iDPDV;
            CDPedido = cDPedido;
            DTPedido = dTPedido;
            STPedido = sTPedido;
            VLPedido = vLPedido;
            VLAcres = vLAcres;
            VLDesc = vLDesc;
            VLOutros = vLOutros;
            VLTotal = vLTotal;
            DSObs = dSObs;
            NUDistancia = nUDistancia;
            DTHRConclusao = dTHRConclusao;
        }
    }
}

using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Models.CustomReturn.PedidoViewModel
{
    public class PedidoListaViewModel
    {
        public string Id { get; set; }
        public string IDEmpresa { get; set; }
        public string IDFuncionario { get; set; }
        public string IDCliente { get; set; }
        public string IDEndereco { get; set; }
        public string IDCaixa { get; set; }
        public string IDPDV { get; set; }
        public string CDPedido { get; set; }
        public DateTime DTPedido { get; set; }
        public ESituacaoPedido STPedido { get; set; }
        public double VLPedido { get; set; }
        public double VLAcres { get; set; }
        public double VLDesc { get; set; }
        public double VLOutros { get; set; }
        public double VLTotal { get; set; }
        public string DSObs { get; set; }
        public double NUDistancia { get; set; }
        public DateTime DTHRConclusao { get; set; } 
        public string Contatos { get; set; }
        public string Logradouro { get; set; }
        public string Num { get; set; }
        public string Compl { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string DsPtRef { get; set; }
        public int SqCaixa { get; set; }
        public int SqVenda { get; set; }
        public long IdVenda { get; set; }
        public int StEmissao { get; set; }
        public string NmFunc { get; set; }
        public string NmCliente { get; set; }
        public string Codigo { get; set; }

    }

    public class PedidoItemListaViewModel
    {
        public string Id { get; set; }
        public string IDPedido { get; set; }
        public string IDProduto { get; set; }
        public string IDEstoque { get; set; }
        public string IDFornecedor { get; set; }
        public int? SQItemPedido { get; set; }
        public double? VLUnit { get; set; }
        public double? NUQtd { get; set; }
        public double? VLItem { get; set; }
        public double? VLAcres { get; set; }
        public double? VLDesc { get; set; }
        public double? VLOutros { get; set; }
        public double? VLTotal { get; set; }
        public double? VLCustoMedio { get; set; }
        public int? STItemPedido { get; set; }
        public DateTime? DTPrevEntrega { get; set; }
        public string DSObsItem { get; set; }
        public string DsProduto { get; set; }
    }

    public class PedidoFormaPagamentoListaViewModel
    {
        public string Id { get; set; }
        public string IDPedido { get; set; }
        public string IDFormaPagamento { get; set; }
        public string IDMoeda { get; set; }
        public double? VLPagamento { get; set; }
        public double? VLTroco { get; set; }
        public string DSObsPagamento { get; set; }
        public string DsMoeda { get; set; }
    }

    public class PedidosEstatisticasListaViewModel
    {
        public int PedidosAguardando { get; set; } = 0;
        public int PedidosGeral { get; set; } = 0;
        public int PedidosConcluido { get; set; } = 0;
        public int PedidosCancelado { get; set; } = 0;
    }

    public class PedidoListaSituacaoViewModel
    {
        public ESituacaoPedido StPedido { get; set; }
        public int Total { get; set; }
    }

    public class PedidoSalvarCustomViewModel : PedidoListaViewModel
    {
        public IEnumerable<PedidoItemListaViewModel> Itens { get; set; } = new List<PedidoItemListaViewModel>();
        public IEnumerable<PedidoFormaPagamentoListaViewModel> FormasPagamento { get; set; } = new List<PedidoFormaPagamentoListaViewModel>();
    }
}

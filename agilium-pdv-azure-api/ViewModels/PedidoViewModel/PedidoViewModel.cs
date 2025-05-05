using agilium.api.business.Enums;
using System;
using System.Collections;
using System.Collections.Generic;

namespace agilium.api.pdv.ViewModels.PedidoViewModel
{
    public class PedidoSalvarViewModel: PedidoViewModel
    {
        public IEnumerable<PedidoItemViewModel> Itens { get; set; } = new List<PedidoItemViewModel>();
        public IEnumerable<PedidoFormaPagamentoViewModel> FormasPagamento { get; set; } = new List<PedidoFormaPagamentoViewModel>();
    }
    public class PedidoViewModel
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
        public string IdVenda { get; set; }
        public int StEmissao { get; set; }
        public string NmFunc { get; set; }
        public string NmCliente { get; set; }
        public string Codigo { get; set; }
    }

    public class PedidoFiltroConsultaViewModel
    {
        public DateTime dataInicial { get; set; }
        public DateTime dataFinal { get; set; }
        public string nomeCliente { get; set; }
        public string nomeEntregador { get; set; }
        public string numeroPedido { get; set; }
        public string bairroEntrega { get; set; }
    }

    public class PedidoItemViewModel
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

    public class PedidoFormaPagamentoViewModel
    {
        public string Id { get; set; }
        public string? IDPedido { get; set; }
        public string? IDFormaPagamento { get; set; }
        public string? IDMoeda { get; set; }
        public double? VLPagamento { get; set; }
        public double? VLTroco { get; set; }
        public string DSObsPagamento { get; set; }
        public string DsMoeda { get; set; }
        public ESituacaoTroco? SitucacaoTroco { get; set; }
        public string nsu { get; set; }
        public int parcelas { get; set; }
    }

    public class PedidosEstatisticasViewModel
    {
        public int PedidosAguardando { get; set; } = 0;
        public int PedidosGeral { get; set; } = 0;
        public int PedidosConcluido { get; set; } = 0;
        public int PedidosCancelado { get; set; } = 0;
    }

    public class PedidoDetalhesRetornoViewModel
    {
        public string idPedido { get; set; }
        public IEnumerable<PedidoItemViewModel> Itens { get; set; } =new List<PedidoItemViewModel>();
        public IEnumerable<PedidoFormaPagamentoViewModel> FormasPagamento { get; set; } = new List<PedidoFormaPagamentoViewModel>();
    }

    public class PedidoPorFuncionarioViewModel
    {
        public double TotalPedido { get; set; } = 0;
        public double DistanciaPercorrida { get; set; } = 0;
    }
}

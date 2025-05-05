using agilium.api.business.Enums;
using System;

namespace agilium.api.pdv.ViewModels.VendaViewModel
{
    public class VendaItemViewModel
    {
        public string Id { get; set; }
        public string IDVENDA { get; set; }
        public string VendaNome { get; set; }
        public string IDPRODUTO { get; set; }
        public string ProdutoNome { get; set; }
        public int? Sequencial { get; set; }
        public double? ValorUnitario { get; set; }
        public double? Quantidade { get; set; }
        public double? Valor { get; set; }
        public double? ValorAcrescimo { get; set; }
        public double? ValorDesconto { get; set; }
        public double? ValorTotal { get; set; }
        public double? ValorCustoMedio { get; set; }
        public ESituacaoItemVenda? Situacao { get; set; }
        public double? PCIBPTFED { get; set; }
        public double? PCIBPTEST { get; set; }
        public double? PCIBPTMUN { get; set; }
        public double? PCIBPTIMP { get; set; }
        public string CodigoProduto { get; set; }
        public string SituacaoProduto { get; set; }
        public bool cancelado { get; set; }
    }

    public class VendaMoedaViewModel
    {
        public string IDVENDA { get; set; }
        public string VendaNome { get; set; }
        public string IDMOEDA { get; set; }
        public string MoedaNome { get; set; }
        public string IDVALE { get; set; }
        public double? ValorPago { get; set; }
        public double? ValorTroco { get; set; }
        public int? NumeroParcela { get; set; }
        public string NSU { get; set; }
    }

    public class VendaNovaViewModel
    {
        public string IdCaixa { get; set; }
        public string IdEstoque { get; set; }
        public string IdPdv { get; set; }
        public int SqVenda { get; set; }
        public int SqCaixa { get; set; }
        public int Turno { get; set; }
    }
}

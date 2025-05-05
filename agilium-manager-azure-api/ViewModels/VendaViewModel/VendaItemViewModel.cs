using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;

namespace agilium.api.manager.ViewModels.VendaViewModel
{
    public class VendaItemViewModel
    {
        public long Id { get; set; }
        public Int64? IDVENDA { get; set; }
        public string VendaNome { get; set; }
        public Int64? IDPRODUTO { get; set; }
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
    }
}

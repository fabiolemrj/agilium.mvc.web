using agilium.webapp.manager.mvc.Enums;
using System;
using System.Collections.Generic;

namespace agilium.webapp.manager.mvc.ViewModels.VendaViewModel
{
    public class VendaViewModel
    {
        public long Id { get; set; }
        public Int64? IDCAIXA { get; set; }
        public string CaixaNome { get; set; }
        public Int64? IDCLIENTE { get; set; }
        public string ClienteNome { get; set; }
        public int? Sequencial { get; set; }
        public DateTime? Data { get; set; }
        public string CpfCnpj { get; set; }
        public double? Valor { get; set; }
        public double? ValorDesconto { get; set; }
        public double? ValorTotal { get; set; }
        public double? ValorAcrescimo { get; set; }
        public ESituacaoVenda? Situacao { get; set; }
        public string InformacaoComplementar { get; set; }
        public double? ValorTotalIbptFed { get; set; }
        public double? ValorTotalIbptEst { get; set; }
        public double? ValorTotalIbptMun { get; set; }
        public double? ValorTotalIbptImp { get; set; }
        public int? NumeroNF { get; set; }
        public string SerieNF { get; set; }
        public ETipoDocVenda? TipoDocumento { get; set; }
        public ETipoEmissaoVenda? Emissao { get; set; }
        public string ChaveAcesso { get; set; }
        public string PDVNome { get; set; }
        public string FuncionarioNome { get; set; }
        //public VendaDetalhes VendaDetalhes { get; set; } = new VendaDetalhes();
    }

    public class VendaMoedaViewModel
    {
        public long Id { get; set; }
        public Int64? IDVENDA { get; set; }
        public string VendaNome { get; set; }
        public Int64? IDMOEDA { get; set; }
        public string MoedaNome { get; set; }
        public Int64? IDVALE { get; set; }
        public double? ValorPago { get; set; }
        public double? ValorTroco { get; set; }
        public int? NumeroParcela { get; set; }
        public string NSU { get; set; }
    }

    public class VendaItemViewModel
    {
        public long Id { get; set; }
        public Int64? IDVENDA { get; set; }
        public string VendaNome { get; set; }
        public Int64? IDPRODUTO { get; set; }
        public string CodigoProduto { get; set; }
        public string SituacaoProduto { get; set; }
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
    }

    public class VendaDetalhesViewModel
    {
        public string SequencialVenda { get; set; }
        public long idVenda { get; set; }
        public List<VendaItemViewModel> VendaItens { get; set; }=new List<VendaItemViewModel>();
        public List<VendaMoedaViewModel> VendaMoedas { get; set; } = new List<VendaMoedaViewModel>();
    }
}

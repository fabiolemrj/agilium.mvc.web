using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;

namespace agilium.api.manager.ViewModels.VendaViewModel
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
    }

    public class VendaDetalhes
    {
        public long idVenda { get; set; }
        public string SequencialVenda { get; set; }
        public List<VendaItemViewModel> VendaItens { get; set; } = new List<VendaItemViewModel>();
        public List<VendaMoedaViewModel> VendaMoedas { get; set; } = new List<VendaMoedaViewModel>();
    }

}

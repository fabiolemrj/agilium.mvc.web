using agilium.webapp.manager.mvc.Enums;
using System;
using System.Collections.Generic;

namespace agilium.webapp.manager.mvc.ViewModels.VendaReportViewModel
{
    public class VendaReportViewModel
    {
        public long Id { get; set; }
        public string Sequencial { get; set; }
        public ESituacaoVenda Situacao { get; set; }
        public double Valor { get; set; }
        public double Acrescimo { get; set; }
        public double Desconto { get; set; }
        public double Devolucao { get; set; }
        public double Total { get; set; }
        public string SeqCaixa { get; set; }
        public string Operador { get; set; }
        public string Pdv { get; set; }
        public DateTime DataVenda { get; set; }
    }

    public class VendaItemReportViewModel
    {
        public long Id { get; set; }
        public long IdVenda { get; set; }
        public string Produto { get; set; }
        public double ValorUnitario { get; set; }
        public double Quantidade { get; set; }
        public double Total { get; set; }
        public ESituacaoItemVenda Situacao { get; set; }
    }

    public class VendaMoedaItemReportViewModel
    {
        public string Moeda { get; set; }
        public double Valor { get; set; }
    }

    public class VendaDetalheReportViewModel : VendaReportViewModel
    {
        public List<VendaItemReportViewModel> Itens { get; set; } = new List<VendaItemReportViewModel>();
        public List<VendaMoedaItemReportViewModel> Moedas { get; set; } = new List<VendaMoedaItemReportViewModel>();
        //public List<VendaMoedaItemReportViewModel> TotalMoedas { get; set; } = new List<VendaMoedaItemReportViewModel>();

    }

    public class VendasReportViewModel
    {
        public List<VendaDetalheReportViewModel> Vendas { get; set; } = new List<VendaDetalheReportViewModel>();
        public List<VendaMoedaItemReportViewModel> TotalMoedas { get; set; } = new List<VendaMoedaItemReportViewModel>();
        public double TotalQuantidade { get; set; }
        public double SubTotal { get; set; }
        public double TotalAcrescimo { get; set; }
        public double TotalDevolucao { get; set; }
        public double ValorTotal { get; set; }
        public double TotalDesconto { get; set; }
    }
}

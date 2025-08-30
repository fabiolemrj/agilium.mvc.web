using System.Collections.Generic;
using System;
using agilum.mvc.web.Enums;
using System.ComponentModel.DataAnnotations;

namespace agilum.mvc.web.ViewModels.VendaRepot
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

    public class VendaRankingReport
    {
        public string Resultado { get; set; }
        public double QuantidadeVendida { get; set; }
        public double TotalVendida { get; set; }
        public double CustoTotal { get; set; }
        public double LucroTotal { get; set; }
    }

    public class VendaMoedaReport
    {
        public List<ListaDatasVendaReport> ListaDatasVendaReports { get; set; } = new List<ListaDatasVendaReport>();
        public List<TotalVendaMoedaPorDataReport> TotalizacaoMoeda { get; set; } = new List<TotalVendaMoedaPorDataReport>();
    }

    public class ListaDatasVendaReport
    {
        public DateTime DataVenda { get; set; }
        public List<TotalVendaMoedaPorDataReport> TotalVendaMoedaPorDataReport { get; set; } = new List<TotalVendaMoedaPorDataReport>();

    }

    public class TotalVendaMoedaPorDataReport
    {
        public double Valor { get; set; }
        public string Descricao { get; set; }
    }

    public class VendaFornecedorReportViewModel
    {
        public long IdProduto { get; set; }
        public string Produto { get; set; }
        public double Quantidade { get; set; }
        public double Total { get; set; }
        public long? IdFornecedor { get; set; }
        public string Fornecedor { get; set; }
    }

    public class VendasFornecedorViewModel
    {
        public List<VendaFornecedorReportViewModel> Vendas { get; set; } = new List<VendaFornecedorReportViewModel>();
        public double TotalQuantidade { get; set; }
        public double TotalValor { get; set; }
    }

    public class VendaFiltroRankingViewModel
    {
        [Display(Name = "Data Inicial")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public DateTime dataInicial { get; set; }
        [Display(Name = "Data Final")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public DateTime dataFinal { get; set; }
        [Display(Name = "Resultado por")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public EResultadoFiltroRanking TipoResultado { get; set; }
        [Display(Name = "Ordenação por")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public EOrdenacaoFiltroRanking Ordenacao { get; set; }
        public List<VendaRankingReport> ListaVendas { get; set; } = new List<VendaRankingReport>();
    }

    public class VendaDiferencaCaixaReport
    {
        public long idCaixa { get; set; }
        public double vlabt { get; set; }
        public double VLFECH { get; set; }
        public double Valor { get; set; }
        public int SQCAIXA { get; set; }
        public string NMFUNC { get; set; }
        public DateTime DTHRABT { get; set; }
        public DateTime DTHRFECH { get; set; }
        public int Classificacao { get; set; }
    }
}

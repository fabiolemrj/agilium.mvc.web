using System.Collections.Generic;
using System;

namespace agilium.webapp.manager.mvc.ViewModels.VendaReportViewModel
{
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
}

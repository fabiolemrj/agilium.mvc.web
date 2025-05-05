using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.CustomReturn.ReportViewModel.VendaReportViewModel
{
    public class VendaRankingReport
    {
        public string Resultado { get; set; }
        public double QuantidadeVendida { get; set; }
        public double TotalVendida { get; set; }
        public double CustoTotal { get; set; }
        public double LucroTotal { get; set; }
    }
}

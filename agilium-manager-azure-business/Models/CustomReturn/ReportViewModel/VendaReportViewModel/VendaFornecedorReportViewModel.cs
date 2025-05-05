using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.CustomReturn.ReportViewModel.VendaReportViewModel
{
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
}

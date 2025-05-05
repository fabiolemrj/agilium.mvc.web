using agilium.api.business.Enums;
using agilium.api.business.Models.CustomReturn.ReportViewModel.VendaReportViewModel;
using System;
using System.Collections.Generic;

namespace agilium.api.manager.ViewModels.VendaViewModel
{
    public class VendaFiltroRankingViewModel
    {
        public DateTime dataInicial { get; set; }
        public DateTime dataFinal { get; set; }
        public EResultadoFiltroRanking TipoResultado { get; set; }
        public EOrdenacaoFiltroRanking Ordenacao { get; set; }
        public List<VendaRankingReport> ListaVendas { get; set; } = new List<VendaRankingReport>();
    }
}

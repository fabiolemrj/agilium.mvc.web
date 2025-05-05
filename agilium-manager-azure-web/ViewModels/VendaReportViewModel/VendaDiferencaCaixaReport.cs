using System;

namespace agilium.webapp.manager.mvc.ViewModels.VendaReportViewModel
{
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

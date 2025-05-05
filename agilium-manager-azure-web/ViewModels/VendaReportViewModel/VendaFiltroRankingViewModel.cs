using agilium.webapp.manager.mvc.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.ViewModels.VendaReportViewModel
{
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
}

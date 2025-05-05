using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace agilium.webapp.manager.mvc.ViewModels.PlanoContaViewModel
{
    public class PlanoContaSaldoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Plano de conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64 IDCONTA { get; set; }
        [Display(Name = "Data/Hora")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public DateTime DataHora { get; set; } = DateTime.Now;
        [Display(Name = "Ano/Mês Referência")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int? AnoMesReferencia { get; set; }
        [Display(Name = "Valor Saldo")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public double ValorSaldo { get; set; } = 0;
    }
}

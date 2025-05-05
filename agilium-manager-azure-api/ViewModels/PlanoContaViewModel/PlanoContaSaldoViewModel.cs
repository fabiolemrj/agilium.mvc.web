using agilium.api.business.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.api.manager.ViewModels.PlanoContaViewModel
{
    public class PlanoContaSaldoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Plano de conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64 IDCONTA { get; set; }
        [Display(Name = "Data/Hora")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public DateTime? DataHora { get; set; }
        [Display(Name = "Ano/Mês Referência")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int? AnoMesReferencia { get; set; }
        [Display(Name = "Valor Saldo")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public double ValorSaldo { get; set; } = 0;
    }
}

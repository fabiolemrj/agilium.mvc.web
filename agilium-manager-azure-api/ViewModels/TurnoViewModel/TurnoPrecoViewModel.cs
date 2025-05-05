using agilium.api.business.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.api.manager.ViewModels.TurnoViewModel
{
    public class TurnoPrecoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Produto")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64 IDPRODUTO { get; set; }
        [Display(Name = "Numero do Turno")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int NumeroTurno { get; set; }
        [Display(Name = "Diferença ")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int Diferenca { get; set; }
        [Display(Name = "Tipo de Valor")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int TipoValor { get; set; }
        [Display(Name = "Valor")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double Valor { get; set; }
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }
        [Display(Name = "Data/Hora")]
        public DateTime DataHora { get; set; } 
    }
}

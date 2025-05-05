using agilium.api.business.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.api.manager.ViewModels.ClienteViewModel
{
    public class ClientePrecoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long IDCLIENTE { get; set; }
        [Display(Name = "Produto")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long IDPRODUTO { get; set; }
        [Display(Name = "Diferença")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int Diferenca { get; set; }
        [Display(Name = "Valor")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double Valor { get; set; }
        [Display(Name = "Data Hora")]
        public DateTime Datahora { get; set; }
        [Display(Name = "Tipo de Valor")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int TipoValor { get; set; }
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }
    }
}

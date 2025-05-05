using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.ViewModels.EstoqueViewModel
{
    public class EstoqueHistoricoViewModel
    {
        [Display(Name = "Estoque")]
        public Int64? IDESTOQUE { get; set; }
        [Display(Name = "Produto")]
        public Int64? IDPRODUTO { get; set; }
        public Int64? IDITEM { get; set; }
        public Int64? IDLANC { get; set; }
        [Display(Name = "Data/Hora")]
        public DateTime? DataHora { get; set; }
        [Display(Name = "Usuario")]
        [StringLength(50, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string NomeUsuario { get; set; }
        [Display(Name = "Tipo")]
        public int? TipoHistorico { get; set; }
        [Display(Name = "Descricao")]
        [StringLength(250, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Descricao { get; set; }
        [Display(Name = "Quantidade")]
        [Range(0, double.MaxValue, ErrorMessage = "O valor de {0} deve ser maior que {1}")]
        public double? Quantidade { get; set; }
    }
}

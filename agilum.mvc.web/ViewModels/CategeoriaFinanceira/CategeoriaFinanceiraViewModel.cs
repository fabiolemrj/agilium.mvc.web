
using System.ComponentModel.DataAnnotations;

namespace agilum.mvc.web.ViewModels.CategeoriaFinanceira
{
    public class CategeoriaFinanceiraViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string NMCATEG { get; set; }
        [Display(Name = "Situação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public agilium.api.business.Enums.EAtivo STCATEG { get; set; }
    }
}

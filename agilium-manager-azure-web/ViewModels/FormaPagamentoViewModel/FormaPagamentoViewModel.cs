using agilium.webapp.manager.mvc.Enums;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace agilium.webapp.manager.mvc.ViewModels.FormaPagamentoViewModel
{
    public class FormaPagamentoViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long IDEmpresa { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string Descricao { get; set; }
        [Display(Name = "Situação")]
        public EAtivo? Situacao { get; set; }
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();
    }
}

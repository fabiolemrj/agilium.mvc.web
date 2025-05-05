using agilium.webapp.manager.mvc.Enums;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.ViewModels.DevolucaoViewModel
{
    public class MotivoDevolucaoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        public long? idEmpresa { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(30, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Descricao { get; set; }
        [Display(Name = "Situação")]
        public EAtivo? situacao { get; set; }
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel> { };
    }
}

using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.api.manager.ViewModels.ControleAcessoViewModel
{
    public class CreateEditPerfilViewModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "{0} é obrigatório")]
        [Display(Name = "Descrição")]
        [StringLength(20, ErrorMessage = "Quantidade de caracteres para o campo {0} deve estar entre 3 e 20", MinimumLength = 3)]
        public string Descricao { get; set; }
        [Display(Name = "Situação")]
        public string Situacao { get; set; }
        public long idEmpresa { get; set; }
    }
}

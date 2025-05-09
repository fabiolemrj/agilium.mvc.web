using agilium.api.business.Enums;
using agilum.mvc.web.ViewModels.Empresa;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace agilum.mvc.web.ViewModels.Produtos
{
    public class GrupoProdutoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        public long idEmpresa { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(50, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Nome { get; set; }
        [Display(Name = "Codigo")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(6, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Codigo { get; set; }
        [Display(Name = "Situaçao")]
        public EAtivo? Situacao { get; set; }
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel> { };
    }

    public class SubGrupoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Grupo de Produto")]
        public long? IDGRUPO { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(50, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Nome { get; set; }
        [Display(Name = "Situaçao")]
        public EAtivo? Situacao { get; set; }
        [Display(Name = "Grupo de Produto")]
        public string NomeGrupo { get; set; }
    }

}
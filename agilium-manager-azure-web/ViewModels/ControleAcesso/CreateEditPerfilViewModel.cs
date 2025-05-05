using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.ViewModels.ControleAcesso
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

    public class CreateEditPermissaoItemViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Descrição")]
        [StringLength(20, ErrorMessage = "Quantidade de caracteres para o campo {0} deve estar entre 3 e 20", MinimumLength = 3)]
        public string Descricao { get; set; }
        [Display(Name = "Descrição")]
        public string Situacao { get; set; }
    }

    public class CreateModeloViewModel
    {
        public long Id { get; set; }
        public long idPerfil { get; set; }
        public string Perfil { get; set; }
        public List<CreateModeloItemViewModel> Permissoes { get; set; } = new List<CreateModeloItemViewModel>();

    }


    public class CreateModeloItemViewModel
    {
        [Display(Name = "")]
        public long Id { get; set; }
        [Display(Name = "")]
        public long idPerfil { get; set; }
        [Display(Name = "")]
        public long idPermissao { get; set; }
        [Display(Name = "")]
        public string Permissao { get; set; }
        [Display(Name = "")]
        public bool selecaoIncluir { get; set; }
        [Display(Name = "")]
        public bool selecaoAlterar { get; set; }
        [Display(Name = "")]
        public bool selecaoExcluir { get; set; }
        [Display(Name = "")]
        public bool selecaoRelatorio { get; set; }
        [Display(Name = "")]
        public bool selecaoConsulta { get; set; }

        public CreateModeloItemViewModel(long idPermissao, string permissao)
        {
            this.idPermissao = idPermissao;
            Permissao = permissao;
        }

        public CreateModeloItemViewModel(long idPerfil, long idPermissao, string permissao)
        {
            this.idPerfil = idPerfil;
            this.idPermissao = idPermissao;
            Permissao = permissao;
        }

        public CreateModeloItemViewModel()
        {

        }
    }
}

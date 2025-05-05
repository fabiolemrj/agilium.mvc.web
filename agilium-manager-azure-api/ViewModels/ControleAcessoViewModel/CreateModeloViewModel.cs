using System.Collections.Generic;

namespace agilium.api.manager.ViewModels.ControleAcessoViewModel
{
    public class CreateModeloViewModel
    {
        public long Id { get; set; }
        public long idPerfil { get; set; }
        public string Perfil { get; set; }
        public List<CreateModeloItemViewModel> Permissoes { get; set; } = new List<CreateModeloItemViewModel>();

    }


    public class CreateModeloItemViewModel
    {
        public long Id { get; set; }
        public long idPerfil { get; set; }
        public long idPermissao { get; set; }
        public string Permissao { get; set; }
        public bool selecaoIncluir { get; set; }
        public bool selecaoAlterar { get; set; }
        public bool selecaoExcluir { get; set; }
        public bool selecaoRelatorio { get; set; }
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

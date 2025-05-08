using agilum.mvc.web.ViewModels.Empresa;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace agilum.mvc.web.ViewModels.EmpresaUsuario
{

    public class EmpresaUsuarioViewModel
    {
        public string IDEMPRESA { get; set; }
        public string NomeEmpresa { get; set; }
        public string IDUSUARIO { get; set; }
    }

    public class ListaEmpresasSelecao
    {
        [Display(Name = "Empresa Selecionada")]
        public string IDEMPRESA { get; set; }
        public string NomeEmpresa { get; set; }
        public List<EmpresaViewModel> EmpresasDisponiveisAssociacao { get; set; } = new List<EmpresaViewModel>();
    }

    public class AssociarEmpresaAuthViewModel
    {
        public EmpresaUsuarioViewModel EmpresaAssociada { get; set; } = new EmpresaUsuarioViewModel();
    }

    public class EmpresasAssociadasViewModel
    {
        public List<EmpresaUsuarioSelecaoViewModel> EmpresasAssociadas { get; set; } = new List<EmpresaUsuarioSelecaoViewModel>();
        public List<EmpresaViewModel> EmpresasDisponiveisAssociacao { get; set; } = new List<EmpresaViewModel>();
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();
        public List<EmpresaUsuarioSelecaoViewModel> EmpresasSelecao { get; set; } = new List<EmpresaUsuarioSelecaoViewModel>();
        public string UsuarioSelecionado { get; set; }
    }

    public class EmpresaUsuarioSelecaoViewModel
    {
        public string IDEMPRESA { get; set; }
        public string NomeEmpresa { get; set; }
        public string IDUSUARIO { get; set; }
        public bool Selecionado { get; set; }
    }
}

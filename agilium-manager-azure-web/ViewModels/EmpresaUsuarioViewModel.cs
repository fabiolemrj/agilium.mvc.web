using agilium.webapp.manager.mvc.ViewModels.Empresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.ViewModels
{
    public class EmpresaUsuarioViewModel
    {
        public string IDEMPRESA { get; set; }
        public string NomeEmpresa { get; set; }
        public string IDUSUARIO { get; set; }
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

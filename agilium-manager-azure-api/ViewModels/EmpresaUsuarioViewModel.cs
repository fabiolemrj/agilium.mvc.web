using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.manager.ViewModels
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
        public List<EmpresaUsuarioViewModel> EmpresasAssociadas { get; set; } = new List<EmpresaUsuarioViewModel>();
        public List<EmpresaViewModel.EmpresaViewModel> EmpresasDisponiveisAssociacao { get; set; } = new List<EmpresaViewModel.EmpresaViewModel>();
        public List<EmpresaViewModel.EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel.EmpresaViewModel>();
    }
}

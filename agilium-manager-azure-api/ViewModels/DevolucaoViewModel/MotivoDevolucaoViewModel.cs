using agilium.api.business.Enums;
using System.Collections.Generic;

namespace agilium.api.manager.ViewModels.DevolucaoViewModel
{
    public class MotivoDevolucaoViewModel
    {
        public long Id { get; set; }
        public long? idEmpresa { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public EAtivo? situacao { get; set; }
        public List<EmpresaViewModel.EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel.EmpresaViewModel> { };
    }
}

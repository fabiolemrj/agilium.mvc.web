using agilium.api.business.Enums;
using System.Collections.Generic;

namespace agilium.api.manager.ViewModels.ProdutoVewModel
{
    public class ProdutoMarcaViewModel
    {
        public long Id { get; set; }
        public long? idEmpresa { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public EAtivo? situacao { get; set; }
        public List<EmpresaViewModel.EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel.EmpresaViewModel> { };
    }
}

using agilium.api.business.Enums;
using System.Collections.Generic;

namespace agilium.api.manager.ViewModels.ProdutoVewModel
{
    public class GrupoProdutoViewModel
    {
        public long Id { get; set; }
        public long idEmpresa { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public EAtivo? Situacao { get; set; }
        public List<EmpresaViewModel.EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel.EmpresaViewModel> { };
    }
}

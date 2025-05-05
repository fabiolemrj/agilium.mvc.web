using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.manager.ViewModels.ContatoViewModel
{
    public class ContatoIndexViewModel
    {
        public long Id { get; set; }
        public ETipoContato TPCONTATO { get; set; }
        public string DESCR1 { get; set; }
        public string DESCR2 { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }

    public class ContatoEmpresaViewModel
    {
        public long IDCONTATO { get; set; }
        public ContatoIndexViewModel Contato { get; set; } = new ContatoIndexViewModel();
        public long IDEMPRESA { get; set; }
        //public agilium.api.manager.ViewModels.EmpresaViewModel.EmpresaViewModel Empresa { get; set; } = new EmpresaViewModel.EmpresaViewModel();
    }
}

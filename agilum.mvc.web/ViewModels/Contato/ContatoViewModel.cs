using agilium.api.business.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace agilum.mvc.web.ViewModels.Contato
{
    public class ContatoIndexViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Tipo Contato")]
        public ETipoContato TPCONTATO { get; set; }
        [Display(Name = "Descrição 1")]
        public string DESCR1 { get; set; }
        [Display(Name = "Descrição 2")]
        public string DESCR2 { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }

    public class ContatoEmpresaViewModel
    {
        public long IDCONTATO { get; set; }
        public ContatoIndexViewModel Contato { get; set; } = new ContatoIndexViewModel();
        public long IDEMPRESA { get; set; }
        // public EmpresaViewModel Empresa { get; set; } = new EmpresaViewModel();
    }

    public class ContatoIndexGenerico
    {
        public long IdGenrico { get; set; } = 0;
        public List<ContatoIndexViewModel> Contatos { get; set; } = new List<ContatoIndexViewModel>();
    }

    public class ContatoFornecedorViewModel
    {
        public long IDCONTATO { get; set; }
        public ContatoIndexViewModel Contato { get; set; } = new ContatoIndexViewModel();
        public long IDFORN { get; set; }
        // public EmpresaViewModel Empresa { get; set; } = new EmpresaViewModel();
    }
}

using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using agilium.webapp.manager.mvc.Enums;
using System.Collections.Generic;
using agilium.webapp.manager.mvc.ViewModels.Empresa;

namespace agilium.webapp.manager.mvc.ViewModels.PlanoContaViewModel
{
    public class PlanoContaViewModel: PlanoContaEditViewModel
    {
        public List<PlanoContaViewModel> PlanosContas { get; set; } = new List<PlanoContaViewModel>();
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();
    }

    public class PlanoContaEditViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64 IDEMPRESA { get; set; }
        [Display(Name = "Conta de nível hierarquico superior")]
        public Int64? IDCONTAPAI { get; set; }
        [Display(Name = "Codigo")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Codigo { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(50, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Descricao { get; set; }
        [Display(Name = "Tipo Conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ETipoContaLancacmento Tipo { get; set; }
        [Display(Name = "Situação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public EAtivo Situacao { get; set; }
        [Display(Name = "Conta de nível hierarquico superior")]
        public string NomeContaPai { get; set; }
        public double Saldo { get; set; } = 0;
    }
}

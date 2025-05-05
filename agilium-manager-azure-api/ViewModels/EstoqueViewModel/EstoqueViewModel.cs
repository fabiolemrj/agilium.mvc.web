using agilium.api.business.Enums;
using agilium.api.business.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.api.manager.ViewModels.EstoqueViewModel
{
    public class EstoqueViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long idEmpresa { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(50, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Descricao { get;set; }
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int Tipo { get; set; }
        [Display(Name = "Capacidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [Range(0,double.MaxValue,ErrorMessage ="O valor de {0} deve ser maior que {1}")]
        public decimal Capacidade { get; set; }
        [Display(Name = "Situação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public EAtivo? Situacao { get; set; }
        public List<EmpresaViewModel.EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel.EmpresaViewModel> { };
    }
}

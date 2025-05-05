using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.api.manager.ViewModels.PlanoContaViewModel
{
    public class PlanoContaViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64 IDEMPRESA { get; set; }
        [Display(Name = "Conta Pai")]
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
        [Display(Name = "Saldo")]
        public double Saldo { get; set; } = 0;

    }
}

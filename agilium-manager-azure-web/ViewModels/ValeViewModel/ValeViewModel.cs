using agilium.webapp.manager.mvc.Enums;
using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.ViewModels.Cliente;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.ViewModels.ValeViewModel
{
    public class ValeViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDEMPRESA { get; set; }
        public string EmpresaNome { get; set; }
        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDCLIENTE { get; set; }
        public string ClienteNome { get; set; }
        [Display(Name = "Codigo")]
        [StringLength(6, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Codigo { get; set; }
        public DateTime? DataHora { get; set; }
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ETipoVale? Tipo { get; set; }
        [Display(Name = "Situação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ESituacaoVale? Situacao { get; set; }
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        [Display(Name = "Valor")]
        [Moeda]
        public double? Valor { get; set; }
        [Display(Name = "Codigo de Barra")]
        [StringLength(10, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CodigoBarra { get; set; }
        public List<ClienteViewModel> Clientes { get; set; } = new List<ClienteViewModel>();
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();
    }
}

using agilium.webapp.manager.mvc.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using agilium.webapp.manager.mvc.ViewModels.Empresa;

namespace agilium.webapp.manager.mvc.ViewModels.NotaFiscalViewModel
{
    public class NotaFiscalnutilViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDEMPRESA { get; set; }
        [Display(Name = "Codigo")]
        [StringLength(6, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Codigo { get; set; }
        [Display(Name = "Motivo")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(50, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Motivo { get; set; }
        [Display(Name = "Ano")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int? Ano { get; set; }
        [Display(Name = "Modelo")]
        [StringLength(5, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Modelo { get; set; }
        [Display(Name = "Serie")]
        [StringLength(10, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Serie { get; set; }
        [Display(Name = "Numero Inicial")]
        public int? NumeroInicial { get; set; }
        [Display(Name = "Numero Final")]
        public int? NumeroFinal { get; set; }
        [Display(Name = "Data")]
        public DateTime? Data { get; set; }
        [Display(Name = "Situação")]
        public ESituacaoNFInutil? Situacao { get; set; }
        [Display(Name = "Protocolo")]
        public string Protocolo { get; set; }
        [Display(Name = "XML")]
        public string XML { get; set; }
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();
    }
}

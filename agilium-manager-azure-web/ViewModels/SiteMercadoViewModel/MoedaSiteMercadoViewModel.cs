using agilium.webapp.manager.mvc.Enums;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.ViewModels.SiteMercadoViewModel
{
    public class MoedaSiteMercadoViewModel
    {
        public long Id { get; set; }     
        public string MoedaPdv { get; set; }
        [Display(Name = "Moeda PDV")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long? IDMOEDA { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long? IDEMPRESA { get; set; }
        [Display(Name = "Moeda Site Mercado")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ETipoMoedaSiteMercado? IDSM { get; set; }
        [Display(Name = "Data/Hora")]
        public DateTime? DataHora { get; set; }
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();
        public List<MoedaViewModel.MoedaViewModel> Moedas { get; set; } = new List<MoedaViewModel.MoedaViewModel>();
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.ViewModels.Config
{
    public class ConfigIndexViewModel
    {
        [Display(Name = "Chave")]
        [Required]
        public string CHAVE { get; set; }
        [Display(Name = "Empresa")]
        public long? IDEMPRESA { get; set; }
        [Display(Name = "Valor")]
        public string VALOR { get; set; }
        public IFormFile Arquivo { get; set; }
    }
}

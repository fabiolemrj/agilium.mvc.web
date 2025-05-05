using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace agilium.api.manager.ViewModels.CompraViewModel.ImportacaoXmlNFE
{
    public class EnderecoNFE
    {
        [Display(Name = "Logradouro")]
        public string xLgr { get; set; }
        [Display(Name = "Numero")]
        public string nro { get; set; }
        [Display(Name = "Bairro")]
        public string xBairro { get; set; }
        [Display(Name = "Municipio")]
        public string cMun { get; set; }
        [Display(Name = "Municipio")]
        public string xMun { get; set; }
        [Display(Name = "Estado")]
        public string UF { get; set; }
        [Display(Name = "CEP")]
        public string CEP { get; set; }
        [Display(Name = "País")]
        public int cPais { get; set; }
        [Display(Name = "País")]
        public string xPais { get; set; }
        public string fone { get; set; }
    }
}

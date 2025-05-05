using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace agilium.api.business.Models.CustomReturn.ComprasNFEViewModel
{
    public class Identificacao
    {
        [Display(Name = "UF")]
        public int cUF { get; set; }
        public string cNF { get; set; }
        [Display(Name = "Natureza da Operação")]
        public string natOp { get; set; }
        public int indPag { get; set; }
        [Display(Name = "Modelo")]
        public string mod { get; set; }
        [Display(Name = "Serie")]
        public int serie { get; set; }
        [Display(Name = "Numero")]
        public string nNF { get; set; }
        [Display(Name = "Data Emissão")]
        public DateTime dhEmi { get; set; }
        public DateTime dhSaiEnt { get; set; }
      

    }
}

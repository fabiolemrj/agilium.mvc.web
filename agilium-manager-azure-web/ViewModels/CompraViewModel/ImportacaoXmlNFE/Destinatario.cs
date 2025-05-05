using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace agilium.webapp.manager.mvc.ViewModels.CompraViewModel.ImportacaoXmlNFE
{
    public class Destinatario
    {
        [Display(Name = "CNPJ")]
        public string CNPJ { get; set; }
        [Display(Name = "CPF")]
        public string CPF { get; set; }
        [Display(Name = "Nome Fantasia")]
        public string xNome { get; set; }
        [XmlElement("enderDest")]
        public Endereco Endereco { get; set; }= new Endereco();
        [Display(Name = "E-mail")]
        public string email { get; set; }



    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace agilium.api.manager.ViewModels.CompraViewModel.ImportacaoXmlNFE
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
        public EnderecoNFE Endereco { get; set; }= new EnderecoNFE();
        [Display(Name = "E-mail")]
        public string email { get; set; }



    }
}

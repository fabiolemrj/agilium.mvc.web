using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace agilium.webapp.manager.mvc.ViewModels.CompraViewModel.ImportacaoXmlNFE
{
    public class Detalhe
    {
        [XmlAttribute("nItem")]
        [Display(Name = "Item")]
        public int nItem { get; set; }

        [XmlElement("prod")]
        public Produto Produto { get; set; } = new Produto();
        [XmlElement("imposto")]
        public Imposto imposto { get; set; } = new Imposto();
    }
}

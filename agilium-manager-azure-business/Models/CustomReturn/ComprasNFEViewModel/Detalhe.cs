using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace agilium.api.business.Models.CustomReturn.ComprasNFEViewModel
{
    public class Detalhe
    {
        [XmlAttribute("nItem")]
        [Display(Name = "Item")]
        public int nItem { get; set; }

        [XmlElement("prod")]
        public ProdutoNFE Produto { get; set; } = new ProdutoNFE();
        [XmlElement("imposto")]
        public Imposto imposto { get; set; } = new Imposto();
    }
}

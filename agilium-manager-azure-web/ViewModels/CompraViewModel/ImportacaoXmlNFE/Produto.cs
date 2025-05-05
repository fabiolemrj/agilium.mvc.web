using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace agilium.webapp.manager.mvc.ViewModels.CompraViewModel.ImportacaoXmlNFE
{
    public class Produto
    {
        [XmlElement("cProd")]
        public string cProd { get; set; }
        [Display(Name = "Cod. EAN")]
        [XmlElement]
        public string cEAN { get; set; }
        [Display(Name = "Desc. Produto")]
        [XmlElement]
        public string xProd { get; set; }
        [Display(Name = "NCM")]
        [XmlElement]
        public string NCM { get; set; }
        [Display(Name = "CFOP")]
        [XmlElement]
        public string CFOP { get; set; }
        [Display(Name = "Unidade")]
        [XmlElement]
        public string uCom { get; set; }
        [Display(Name = "Quantidade")]
        [XmlElement]
        public double qCom { get; set; }
        [Display(Name = "Valor Untario")]
        [XmlElement]
        public double vUnCom { get; set; }
        [Display(Name = "Valor Total")]
        [XmlElement("vProd")]
        public double vProd { get; set; }
        [Display(Name = "CEST")]
        [XmlElement]
        public string CEST { get; set; }
    }
}

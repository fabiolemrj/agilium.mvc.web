using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;

namespace agilium.api.manager.ViewModels.CompraViewModel.ImportacaoXmlNFE
{
    public class Total
    {
        [XmlElement("ICMSTot")]
        public ICMSTotal IcmsTotal { get; set; }
    }
    public class ICMSTotal
    {

        [XmlElement("vBC")]
        public double ValorBaseCalculoICMS { get; set; }
        [XmlElement("vICMS")]
        public double ValorICMS { get; set; }
        [XmlElement("vICMSDeson")]
        public double ValorICMSRetido { get; set; }
        [XmlElement("vBCST")]
        public double ValorBaseCalculoCst { get; set; }
        [XmlElement("vST")]
        public double ValorIcmsSub { get; set; }
        [XmlElement("vIPI")]
        public double ValorIPI { get; set; }
        [XmlElement("vFrete")]
        public double ValorFrete { get; set; }
        [XmlElement("vSeg")]
        public double ValorSeguro { get; set; }
        [XmlElement("vOutro")]
        public double ValorOutros { get; set; }
        [XmlElement("vProd")]
        public double ValorTotalProduto { get; set; }
        [XmlElement("vDesc")]
        public double ValorDesconto { get; set; }
        [XmlElement("vNF")]
        public double ValorTotal { get; set; }
        [XmlElement("vTotTrib")]
        public double ValorTotalTributacao { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;

namespace agilium.webapp.manager.mvc.ViewModels.CompraViewModel.ImportacaoXmlNFE
{
    public class Total
    {
        [XmlElement("ICMSTot")]
        public ICMSTotal IcmsTotal { get; set; }
    }
    public class ICMSTotal
    {

        [XmlElement("vBC")]
        [Display(Name = "Base calc. Icms")]
        public double ValorBaseCalculoICMS { get; set; }
        [XmlElement("vICMS")]
        [Display(Name = "Vl Icms")]
        public double ValorICMS { get; set; }
        [XmlElement("vICMSDeson")]
        public double ValorICMSRetido { get; set; }
        [XmlElement("vBCST")]
        [Display(Name = "Base calc. subst. Icms ")]
        public double ValorBaseCalculoCst { get; set; }
        [XmlElement("vST")]
        [Display(Name = "Vl Icms Substituição")]
        public double ValorIcmsSub { get; set; }
        [XmlElement("vIPI")]
        [Display(Name = "Vl Frete")]
        public double ValorIPI { get; set; }
        [XmlElement("vFrete")]
        [Display(Name = "Vl Frete")]
        public double ValorFrete { get; set; }
        [XmlElement("vSeg")]
        [Display(Name = "Vl Seguro")]
        public double ValorSeguro { get; set; }
        [XmlElement("vOutro")]
        [Display(Name = "Outras Despesas")]
        public double ValorOutros { get; set; }
        [XmlElement("vProd")]
        [Display(Name = "Vl Total Produtos")]
        public double ValorTotalProduto { get; set; }
        [XmlElement("vDesc")]
        [Display(Name = "Vl Desconto")]
        public double ValorDesconto { get; set; }
        [XmlElement("vNF")]
        [Display(Name = "Vl Total Nota")]
        public double ValorTotal { get; set; }
        [XmlElement("Vl Total Tribut")]
        public double ValorTotalTributacao { get; set; }
    }
}

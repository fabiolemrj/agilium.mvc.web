using System.Xml.Serialization;

namespace agilium.api.manager.ViewModels.CompraViewModel.ImportacaoXmlNFE
{
    public class Imposto
    {
        [XmlElement]
        public double vTotTrib { get; set; }
        [XmlElement(ElementName = "ICMS")]
        public Icms ICMS { get; set; }= new Icms();
        [XmlElement(ElementName = "COFINS")]
        public Cofins COFINS { get; set; }=new Cofins();
        [XmlElement(ElementName = "PIS")]
        public PIS Pis { get; set; }= new PIS();
    }

    public class Icms
    {
        [XmlElement(ElementName = "ICMS00")]
        public ICMS00 ICMS00 { get; set; }=new ICMS00();
    }

    public class ICMS00
    {
        [XmlElement]
        public int orig { get; set; }
        [XmlElement]
        public int CST { get; set; }
        [XmlElement]
        public int modBC { get; set; }
        [XmlElement]
        public double vBC { get; set; }
        [XmlElement]
        public double pICMS { get; set; }
        [XmlElement]
        public double vICMS { get; set; }
    }

    public class PIS 
    {
        [XmlElement(ElementName = "PISAliq")]
        public PISAliq PISAliq { get; set; } = new PISAliq();
    }

    public class PISAliq
    {
        [XmlElement]
        public string CST { get; set; }
        [XmlElement]
        public double vBC { get; set; }
        [XmlElement]
        public double pPIS { get; set; }
        [XmlElement]
        public double vPIS { get; set; }
    }
    public class Cofins
    {
        [XmlElement]
        public COFINSAliq COFINSAliq { get; set; } = new COFINSAliq();
    }

    public class COFINSAliq
    {
        [XmlElement]
        public string CST { get; set; }
        [XmlElement]
        public double vBC { get; set; }
        [XmlElement]
        public double pCOFINS { get; set; }
        [XmlElement]
        public double vCOFINS { get; set; }
    }
}

using System.Xml.Serialization;

namespace agilium.api.business.Models.CustomReturn.ComprasNFEViewModel
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
        public ICMS10 ICMS10 { get; set; } = new ICMS10();
        public ICMS60 ICMS60 { get; set; } = new ICMS60();
        public ICMS70 ICMS70 { get; set; } = new ICMS70();
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

    public class ICMS60
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

    public class ICMS70
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

    public class ICMS10
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

    public class PISOutr
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
    public class PISNT
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
        [XmlElement]
        public COFINSNT COFINSNT { get; set; } = new COFINSNT();
        [XmlElement]
        public COFINSOutr COFINSOutr { get; set; } = new COFINSOutr();

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

    public class COFINSOutr
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

    public class COFINSNT
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

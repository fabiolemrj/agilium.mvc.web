
using System.Xml.Serialization;

namespace agilium.api.business.Models.CustomReturn.ComprasNFEViewModel
{
    public class protNFe
    {
        [XmlAttribute("versao")]
        public string versao { get; set; }
        [XmlElement("infProt")]
        public infProt InfProt { get; set; }=new infProt();
          
    }

    public class infProt
    {
        [XmlElement("tpAmb")]
        public int tpAmb { get; set; }
        [XmlElement("verAplic")]
        public string verAplic { get; set; }
        [XmlElement("chNFe")]
        public string chNFe { get; set; }
        [XmlElement("dhRecbto")]
        public string dhRecbto { get; set; }
        [XmlElement("nProt")]
        public ulong nProt { get; set; }
        [XmlElement("digVal")]
        public string digVal { get; set; }
        [XmlElement("cStat")]
        public int cStat { get; set; }
        [XmlElement("xMotivo")]
        public string xMotivo { get; set; }
    }

    public class Signature
    {
        [XmlElement("SignedInfo")]
        public SignedInfo SignedInfo { get; set; }=new SignedInfo();
        [XmlElement("SignatureValue")] 
        public string SignatureValue { get; set; }
        [XmlElement("KeyInfo")]
        public KeyInfo KeyInfo { get; set; }=new KeyInfo();
    }

    public class KeyInfo
    {
        [XmlElement("X509Data")]
        public X509Data X509Data { get; set; } = new X509Data();
    }

    public class X509Data
    {
        [XmlElement("X509Certificate")]
        public string X509Certificate { get; set; }
    }

    public class SignedInfo
    {
        [XmlElement("CanonicalizationMethod")]
        public Algoritmo CanonicalizationMethod { get; set; }=new Algoritmo();
        [XmlElement("SignatureMethod")]
        public Algoritmo SignatureMethod { get; set; }=new Algoritmo() { };
        [XmlElement("Reference")]
        public Reference Reference { get; set; }=new Reference() { };
    }

    public struct Algoritmo
    {
        [XmlAttribute]
        public string Algorithm { get; set; }
    }
    public class Reference
    {
        [XmlAttribute]
        public string URI { get; set; }

        [XmlArray]
        [XmlArrayItem(ElementName = "Transform")]
        public Algoritmo[] Transforms { get; set; }

        public Algoritmo DigestMethod { get; set; } = new Algoritmo();
        [XmlAttribute]
        public string DigestValue { get; set; }
    }

}

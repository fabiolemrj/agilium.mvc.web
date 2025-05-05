using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace agilium.api.business.Models.CustomReturn.ComprasNFEViewModel
{    
    public class NFe
    {
        [XmlElement(ElementName = "infNFe")]
        public InfNFe InformacoesNFe { get; set; }=new InfNFe();
        
        [XmlElement]
        public Signature Signature { get; set; }=new Signature();
    }
    public class InfNFe
    {
        [XmlElement("ide")]
        public Identificacao Identificacao { get; set; } = new Identificacao();

        [XmlElement("emit")]
        public Emitente Emitente { get; set; } = new Emitente();

        [XmlElement("dest")]
        public Destinatario Destinatario { get; set; } = new Destinatario();

        [XmlElement("det")]
        public List<Detalhe> Detalhe { get; set; } = new List<Detalhe>();
        [XmlElement("total")]
        public Total TotalNFE { get; set; } = new Total();
        [XmlElement("infAdic")]
        public infAdic InformacaoAdicional { get; set; }=new infAdic();
        [XmlElement("infRespTec")]
        public infRespTec infRespTec { get; set; } = new infRespTec();
    }

    public class infAdic
    {
        [XmlElement("infCpl")]
        public string infCpl { get; set; }
    }

    public class infRespTec
    {
        [XmlElement]
        public string CNPJ { get; set; }
        [XmlElement]
        public string xContato { get; set; }
        [XmlElement]
        public string email { get; set; }
        [XmlElement]
        public string fone { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace agilium.webapp.manager.mvc.ViewModels.CompraViewModel.ImportacaoXmlNFE
{    
    public class NFe
    {
        [XmlElement(ElementName = "infNFe")]
        public InfNFe InformacoesNFe { get; set; }=new InfNFe();
        
        [XmlElement]
        public Signature Signature { get; set; }

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
        public Total TotalNFE { get; set; }
        [XmlElement("infAdic")]
        public infAdic InformacaoAdicional { get; set; }
        [XmlElement("infRespTec")]
        public infRespTec infRespTec { get; set; }
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

    public class infAdic
    {
        [XmlElement("infCpl")]
        [Display(Name = "Informações Complementares")]
        public string infCpl { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace agilium.api.business.Models.CustomReturn.ComprasNFEViewModel
{
    [XmlRoot(ElementName = "nfeProc", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class NFeProc
    {
        [XmlAttribute("versao")]
        public string versao { get; set; }

        [XmlElement("NFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
        public NFe NotaFiscalEletronica { get; set; }= new NFe();
        [Display(Name = "Caminho Arquivo")]
        public string CaminhoArquivo { get; set; }
        public long idCompra { get; set; }
        [XmlElement("protNFe")]
        public protNFe ProtNFe { get; set; } =new protNFe();
    }
}

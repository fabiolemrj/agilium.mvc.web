using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace agilium.api.business.Models.CustomReturn.ComprasNFEViewModel
{
    public class ProdutoNFE
    {
        [Display(Name = "Produto")]
        public string cProd { get; set; }
        [Display(Name = "Codigo EAN")]
        public string cEAN { get; set; }
        [Display(Name = "Produto")]
        public string xProd { get; set; }
        [Display(Name = "NCM")]
        public string NCM { get; set; }
        [Display(Name = "CFOP")]
        public string CFOP { get; set; }
        [Display(Name = "Unidade")]
        public string uCom { get; set; }
        [Display(Name = "Quantidade")]
        public double qCom { get; set; }
        [Display(Name = "Valor Untario")]
        public double vUnCom { get; set; }
        [Display(Name = "Valor Total")]
        public double vProd { get; set; }
        [Display(Name = "CEST")]
        public string CEST { get; set; }
    }
}

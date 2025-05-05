using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace agilium.webapp.manager.mvc.ViewModels.ProdutoViewModel
{
    public class ProdutoCodigoBarraViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Produto")]
        public Int64? IDPRODUTO { get; set; }
        [Display(Name ="Codigo de Barra")]
        public string CDBARRA { get; set; }
        public virtual List<ProdutoViewModel> Produtos { get; set; }=new List<ProdutoViewModel>();
    }
}

using agilium.webapp.manager.mvc.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.ViewModels.ProdutoViewModel
{
    public class ProdutoPrecoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Produto")]
        public long? idProduto { get; set; }
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }
        [Display(Name = "Preço Atual")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        [Moeda]
        public decimal? Preco { get; set; }
        [Display(Name = "Preço Anterior")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        [Moeda]
        public decimal? PrecoAnterior { get; set; }
        [Display(Name = "Data")]
        public DateTime? DataPreco { get; set; }
    }
}

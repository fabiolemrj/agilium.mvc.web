using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.api.manager.ViewModels.EstoqueViewModel
{
    public class EstoqueProdutoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Produto")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDPRODUTO { get; set; }
        [Display(Name = "Estoque")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDESTOQUE { get; set; }
        [Display(Name = "Quantidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "O valor de {0} deve ser maior que {1}")]
        public double? Quantidade { get; set; }
     
    }

    public class EstoqueProdutoListaViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Produto")]
        public Int64 IDPRODUTO { get; set; }
        public Int64 IDESTOQUE { get; set; }
        [Display(Name = "Estoque")]
        public string Estoque { get; set; }
        [Display(Name = "Quantidade Atual")]
        public double QuantidadeAtual { get; set; }
        [Display(Name = "Situação")]
        public EAtivo Situacao { get; set; }
        public string TipoEsotque { get; set; }
        public decimal Capacidade { get; set; }
    }
    public class ProdutoPorEstoqueViewModel 
    {
        public long Id { get; set; }
        public long idProduto { get; set; }
        [Display(Name = "Produto")]
        public string Produto { get; set; }
        [Display(Name = "Codigo")]
        public string Codigo { get; set; }
        [Display(Name = "Quantidade Atual")]
        public double QuantidadeAtual { get; set; }
        [Display(Name = "Ultima Compra")]
        public double ValorUltimaCompra { get; set; }
        [Display(Name = "Custo Medio")]
        public double ValorCustoMedio { get; set; }
    }
}

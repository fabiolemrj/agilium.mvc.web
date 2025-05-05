using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using agilium.webapp.manager.mvc.Enums;

namespace agilium.webapp.manager.mvc.ViewModels.EstoqueViewModel
{
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

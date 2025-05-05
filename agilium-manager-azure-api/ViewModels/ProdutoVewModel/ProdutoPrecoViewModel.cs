using agilium.api.business.Models;
using System;

namespace agilium.api.manager.ViewModels.ProdutoVewModel
{
    public class ProdutoPrecoViewModel
    {
        public long Id { get; set; }
        public long? idProduto { get; set; }
        public string Usuario { get; set; }
        public decimal? Preco { get; set; }
        public decimal? PrecoAnterior { get; set; }
        public DateTime? DataPreco { get; set; }
    }
}

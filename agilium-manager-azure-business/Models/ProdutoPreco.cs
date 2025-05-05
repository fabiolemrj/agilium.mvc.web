using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class ProdutoPreco : Entity
    {
        public Int64? idProduto { get; private set; }
        public virtual Produto Produto { get; private set; }
        public string Usuario { get; private set; }
        public decimal? Preco { get; private set; }
        public decimal? PrecoAnterior { get; private set; }
        public DateTime? DataPreco { get; private set; }
        public ProdutoPreco()
        {

        }

        public ProdutoPreco(string usuario, decimal? preco, decimal? precoAnterior, DateTime? dataPreco)
        {
            Usuario = usuario;
            Preco = preco;
            PrecoAnterior = precoAnterior;
            DataPreco = dataPreco;
        }

        public ProdutoPreco(long? idProduto, string usuario, decimal? preco, decimal? precoAnterior, DateTime? dataPreco)
        {
            this.idProduto = idProduto;
            Usuario = usuario;
            Preco = preco;
            PrecoAnterior = precoAnterior;
            DataPreco = dataPreco;
        }

        public ProdutoPreco(Produto produto, string usuario, decimal? preco, decimal? precoAnterior, DateTime? dataPreco)
        {
            Produto = produto;
            Usuario = usuario;
            Preco = preco;
            PrecoAnterior = precoAnterior;
            DataPreco = dataPreco;
        }
    }
}

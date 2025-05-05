using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class ProdutoComposicao: Entity
    {
        public Int64? idProduto { get; private set;  }
        public virtual Produto Produto { get; private set;  }       
        public Int64? idProdutoComposicao { get; private set;  }
        public virtual Produto ProdComposicao { get; private set;  }
        public Int64? idEstoque { get; private set;  }
        public virtual Estoque Estoque { get; private set;  }
        public double? Quantidade { get; private set;  }
        public double? Preco { get; private set;  }

        public void AdicionarProduto(Produto produto) => Produto = produto;
        public void AdicionarProdutoComposicao(Produto produtoComposicao) => ProdComposicao = produtoComposicao;
        public void AdicionarEstoque(Estoque estoque) => Estoque = estoque;
        public ProdutoComposicao()
        {            
        }
        public ProdutoComposicao(long? idProduto, long? idProdutoComposicao, long? idEstoque, double? quantidade, double? preco)
        {
            this.idProduto = idProduto;
            this.idProdutoComposicao = idProdutoComposicao;
            this.idEstoque = idEstoque;
            Quantidade = quantidade;
            Preco = preco;
        }
    }
}

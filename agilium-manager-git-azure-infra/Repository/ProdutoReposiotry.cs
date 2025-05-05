using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using agilium.api.infra.Context;

namespace agilium.api.infra.Repository
{
    public class ProdutoReposiotry : Repository<Produto>, IProdutoRepository
    {
        public ProdutoReposiotry(AgiliumContext db) : base(db)
        {
        }
    }
    public class ProdutoDepartamentoRepository : Repository<ProdutoDepartamento>, IProdutoDepartamentoRepository
    {
        public ProdutoDepartamentoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class ProdutoMarcaRepository : Repository<ProdutoMarca>, IProdutoMarcaRepository
    {
        public ProdutoMarcaRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class GrupoProdutoRepository : Repository<GrupoProduto>, IGrupoProdutoRepository
    {
        public GrupoProdutoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class SubGrupoProdutoRepository : Repository<SubGrupoProduto>, ISubGrupoProdutoRepository
    {
        public SubGrupoProdutoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class ProdutoComposicaoRepository : Repository<ProdutoComposicao>, IProdutoComposicaoRepository
    {
        public ProdutoComposicaoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class ProdutoPrecoRepository : Repository<ProdutoPreco>, IProdutoPrecoRepository
    {
        public ProdutoPrecoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class ProdutoFotoRepository : Repository<ProdutoFoto>, IProdutoFotoRepository
    {
        public ProdutoFotoRepository(AgiliumContext db) : base(db)
        {
        }
    }
}

using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Repository
{
    public class EstoqueRepository : Repository<Estoque>, IEstoqueRepository
    {
        public EstoqueRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class EstoqueProdutoRepository : Repository<EstoqueProduto>, IEstoqueProdutoRepository
    {
        public EstoqueProdutoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class EstoqueHistoricoRepository : Repository<EstoqueHistorico>, IEstoqueHistoricoRepository
    {
        public EstoqueHistoricoRepository(AgiliumContext db) : base(db)
        {
        }
    }
}

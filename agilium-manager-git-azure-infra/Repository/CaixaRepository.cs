using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using agilium.api.infra.Context;

namespace agilium.api.infra.Repository
{
    public class CaixaRepository : Repository<Caixa>, ICaixaRepository
    {
        public CaixaRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class CaixaMovimentaoRepository : Repository<CaixaMovimento>, ICaixaMovimentoRepository
    {
        public CaixaMovimentaoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class CaixaMoedaRepository : Repository<CaixaMoeda>, ICaixaMoedaRepository
    {
        public CaixaMoedaRepository(AgiliumContext db) : base(db)
        {
        }
    }
}

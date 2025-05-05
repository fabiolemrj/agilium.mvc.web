using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using agilium.api.infra.Context;

namespace agilium.api.infra.Repository
{
    public class PlanoContaRepository : Repository<PlanoConta>, IPlanoContaRepository
    {
        public PlanoContaRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class PlanoContaSaldoRepository : Repository<PlanoContaSaldo>, IPlanoContaSaldoRepository
    {
        public PlanoContaSaldoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class PlanoContaLancamentoRepository : Repository<PlanoContaLancamento>, IPlanoContaLancamentoRepository
    {
        public PlanoContaLancamentoRepository(AgiliumContext db) : base(db)
        {
        }
    }
}

using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using agilium.api.infra.Context;

namespace agilium.api.infra.Repository
{
    public class ContaPagarRepository : Repository<ContaPagar>, IContaPagarRepository
    {
        public ContaPagarRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class ContaReceberRepository : Repository<ContaReceber>, IContaReceberRepository
    {
        public ContaReceberRepository(AgiliumContext db) : base(db)
        {
        }
    }
}

using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using agilium.api.infra.Context;

namespace agilium.api.infra.Repository
{
    public class CompraRepository : Repository<Compra>, ICompraRepository
    {
        public CompraRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class CompraFiscalRepository : Repository<CompraFiscal>, ICompraFiscalRepository
    {
        public CompraFiscalRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class CompraItemRepository : Repository<CompraItem>, ICompraItemRepository
    {
        public CompraItemRepository(AgiliumContext db) : base(db)
        {
        }
    }
}

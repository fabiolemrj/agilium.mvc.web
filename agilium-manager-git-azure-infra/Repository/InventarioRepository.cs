using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using agilium.api.infra.Context;

namespace agilium.api.infra.Repository
{
    public class InventarioRepository : Repository<Inventario>, IInventarioRepository
    {
        public InventarioRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class InventarioItemRepository : Repository<InventarioItem>, IInventarioItemRepository
    {
        public InventarioItemRepository(AgiliumContext db) : base(db)
        {
        }
    }
}

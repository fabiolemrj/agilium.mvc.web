using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using agilium.api.infra.Context;



namespace agilium.api.infra.Repository
{
    public class VendaRepository : Repository<Venda>, IVendaRepository
    {
        public VendaRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class VendaItemRepository : Repository<VendaItem>, IVendaItemRepository
    {
        public VendaItemRepository(AgiliumContext db) : base(db)
        {
        }

    }

    public class VendaMoedaRepository : Repository<VendaMoeda>, IVendaMoedaRepository
    {
        public VendaMoedaRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class VendaEspelhoRepository : Repository<VendaEspelho>, IVendaEspelhoRepository
    {
        public VendaEspelhoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class VendaCanceladaRepository : Repository<VendaCancelada>, IVendaCanceladaRepository
    {
        public VendaCanceladaRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class VendaFiscalRepository : Repository<VendaFiscal>, IVendaFiscalRepository
    {
        public VendaFiscalRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class VendaTemporariaRepository : Repository<VendaTemporaria>, IVendaTemporariaRepository
    {
        public VendaTemporariaRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class VendaTemporariaEspelhoRepository : Repository<VendaTemporariaEspelho>, IVendaTemporariaEspelhoRepository
    {
        public VendaTemporariaEspelhoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class VendaTemporariaItemRepository : Repository<VendaTemporariaItem>, IVendaTemporariaItemRepository
    {
        public VendaTemporariaItemRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class VendaTemporariaMoedaRepository : Repository<VendaTemporariaMoeda>, IVendaTemporariaMoedaRepository
    {
        public VendaTemporariaMoedaRepository(AgiliumContext db) : base(db)
        {
        }
    }
}

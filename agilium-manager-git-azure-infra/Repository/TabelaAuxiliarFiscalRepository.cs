using agilium.api.business.Interfaces.IRepository;
using agilium.api.infra.Context;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Repository
{
    public class CfopRepository : Repository<Cfop>, ICfopRepository
    {
        public CfopRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class CsosnRepository : Repository<Csosn>, ICsosnRepository
    {
        public CsosnRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class CestNcmRepository : Repository<CestNcm>, ICestNcmRepository
    {
        public CestNcmRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class NcmRepository : Repository<Ncm>, INcmRepository
    {
        public NcmRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class CstRepository : Repository<Cst>, ICstRepository
    {
        public CstRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class IbptRepository : Repository<Ibpt>, IIbptRepository
    {
        public IbptRepository(AgiliumContext db) : base(db)
        {
        }
    }
}

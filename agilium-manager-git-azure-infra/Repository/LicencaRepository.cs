
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Context;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.infra.Repository
{
    public class LicencaRepository : Repository<Licenca>, ILicencaRepository
    {
        public LicencaRepository(AgiliumContext db) : base(db)
        {
        }

    }
}

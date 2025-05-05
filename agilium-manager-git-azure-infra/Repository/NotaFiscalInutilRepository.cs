using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Repository
{
    public class NotaFiscalInutilRepository : Repository<NotaFiscalInutil>, INotaFiscalInutilRepository
    {
        public NotaFiscalInutilRepository(AgiliumContext db) : base(db)
        {
        }
    }
}

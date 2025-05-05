using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Repository
{
    public class PontoVendaRepository : Repository<PontoVenda>, IPontoVendaRepository
    {
        public PontoVendaRepository(AgiliumContext db) : base(db)
        {
        }
    }
}

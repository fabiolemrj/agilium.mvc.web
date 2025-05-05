using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Repository
{
    public class TurnoRepository : Repository<Turno>, ITurnoRepository
    {
        public TurnoRepository(AgiliumContext db) : base(db)
        {
        }
    }
    public class TurnoPrecoRepository : Repository<TurnoPreco>, ITurnoPrecoRepository
    {
        public TurnoPrecoRepository(AgiliumContext db) : base(db)
        {
        }
    }
}

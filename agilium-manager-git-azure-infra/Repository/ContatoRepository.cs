using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Repository
{
    public class ContatoRepository : Repository<Contato>, IContatoRepository
    {
        public ContatoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class ContatoEmpresaRepository : Repository<ContatoEmpresa>, IContatoEmpresaRepository
    {
        public ContatoEmpresaRepository(AgiliumContext db) : base(db)
        {
        }
    }
}

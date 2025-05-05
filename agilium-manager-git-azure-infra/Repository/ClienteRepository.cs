using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Repository
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class ClienteContatoRepository : Repository<ClienteContato>, IClienteContatoRepository
    {
        public ClienteContatoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class ClientePrecoRepository : Repository<ClientePreco>, IClientePrecoRepository
    {
        public ClientePrecoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class ClientePFRepository : Repository<ClientePF>, IClientePFRepository
    {
        public ClientePFRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class ClientePJRepository : Repository<ClientePJ>, IClientePJRepository
    {
        public ClientePJRepository(AgiliumContext db) : base(db)
        {
        }
    }
}

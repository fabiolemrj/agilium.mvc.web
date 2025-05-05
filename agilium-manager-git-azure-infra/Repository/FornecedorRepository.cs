using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using agilium.api.infra.Context;

namespace agilium.api.infra.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepsoitory
    {
        public FornecedorRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class FornecedorContatoRepository : Repository<FornecedorContato>, IFornecedorContatoRepsoitory
    {
        public FornecedorContatoRepository(AgiliumContext db) : base(db)
        {
        }
    }
}

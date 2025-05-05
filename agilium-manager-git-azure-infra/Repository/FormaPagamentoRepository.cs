using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Repository
{
    public class FormaPagamentoRepository : Repository<FormaPagamento>, IFormaPagamentoRepository
    {
        public FormaPagamentoRepository(AgiliumContext db) : base(db)
        {
        }
    }
}

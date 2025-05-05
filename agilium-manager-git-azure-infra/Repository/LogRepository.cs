using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Repository
{
    public class LogRepository : Repository<LogSistema>, ILogRepository
    {
        public LogRepository(AgiliumContext db) : base(db)
        {
        }
    }
    public class LogErroRepository : Repository<LogErro>, ILogErroRepository
    {
        public LogErroRepository(AgiliumContext db) : base(db)
        {
        }
    }
}

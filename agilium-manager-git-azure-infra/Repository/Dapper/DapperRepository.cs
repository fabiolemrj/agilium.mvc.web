using agilium.api.business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.infra.Repository.Dapper
{
    public class DapperRepository : IDapperRepository
    {
        private readonly DbSession _session;

        public DapperRepository(DbSession session)
        {
            _session = session;
        }

        public async Task BeginTransaction()
        {
              _session.Transaction = _session.Connection.BeginTransaction();
        }

        public async Task Commit()
        {
            _session.Transaction.Commit();
            Dispose();
        }

        public void Dispose()
        {
            _session.Transaction?.Dispose();
        }

        public async Task Rollback()
        {
            _session.Transaction.Rollback();
            Dispose();
        }
    }
}

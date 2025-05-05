using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces
{
    public interface IDapperRepository: IDisposable
    {
        Task BeginTransaction();
        Task Commit();
        Task Rollback();
    }
}

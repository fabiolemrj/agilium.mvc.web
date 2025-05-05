using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface INotaFiscalInutilRepository: IRepository<NotaFiscalInutil>
    {
    }

    public interface IPNotaFiscalDapperRepository
    {
        Task<bool> EnviarNotaFiscalInutil(long id);
        
    }
}

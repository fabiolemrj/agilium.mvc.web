using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface IEmpresaRepository: IRepository<Empresa>
    {
        Task<Empresa> ObterCompletoTracking(long id);
    }

    public interface IEmpresaDapperRepository
    {
        Task<bool> EditarEmpresa(Empresa empresa);
    }
}

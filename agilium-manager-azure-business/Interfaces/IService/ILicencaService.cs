
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium_manager_azure_business.Interfaces.IService
{
    public interface ILicencaService : IDisposable
    {
        Task<Licenca> ObterPorIdEmpresa(string idLicenca, string idEmpresa);
        Task<bool> DataValida(long idEmpresa);
    }
}

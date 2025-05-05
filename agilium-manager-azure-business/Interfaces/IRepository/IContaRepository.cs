using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface IContaPagarRepository: IRepository<ContaPagar>
    {
    }

    public interface IContaReceberRepository : IRepository<ContaReceber>
    {
    }

    public interface IPContaPagarDapperRepository
    {
        Task<bool> ConsolidarConta(long id);
        Task<bool> DesconsolidarConta(long id);
    }

    public interface IPContaReceberDapperRepository
    {
        Task<bool> ConsolidarConta(long id);
        Task<bool> DesconsolidarConta(long id);
    }
}

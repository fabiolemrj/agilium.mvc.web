using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface ITabelaAuxiliarFiscalService: IDisposable
    {
        Task<IEnumerable<CestNcm>> ObterTodosNcm();
        Task<CestNcm> ObterNcmPorId(string id);

        Task<IEnumerable<Cfop>> ObterTodosCfop();
        Task<Cfop> ObterCfopPorId(int id);

        Task<IEnumerable<CestNcm>> ObterTodosCestNcm();
        Task<CestNcm> ObterCestNcmPorId(long id);
        
        Task<IEnumerable<Cst>> ObterTodosCst();
        Task<Cst> ObterCstPorId(string id);

        Task<IEnumerable<Ibpt>> ObterTodosIbpt();
        Task<Ibpt> ObterIbptPorId(long id);

        Task<IEnumerable<Csosn>> ObterTodosCsosn();
        Task<Csosn> ObterCsosnPorId(string id);
    }
}

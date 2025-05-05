using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.PontoVendaViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IPontoVendaService
    {
        Task<PagedViewModel<PontoVendaViewModel>> ObterPorNome(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, PontoVendaViewModel pontoVendaViewModel);
        Task<ResponseResult> Adicionar(PontoVendaViewModel pontoVendaViewModel);
        Task<PontoVendaViewModel> ObterPorId(long id);
        Task<ResponseResult> Remover(long id);
        Task<IEnumerable<PontoVendaViewModel>> ObterTodas();
        Task<PontoVendaViewModel> ObterListasAuxiliares();
    }
}

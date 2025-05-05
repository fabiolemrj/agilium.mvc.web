using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.PerdaViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IPerdaService
    {
        Task<PagedViewModel<PerdaViewModel>> ObterPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, PerdaViewModel viewModel);
        Task<ResponseResult> Adicionar(PerdaViewModel viewModel);
        Task<PerdaViewModel> ObterPorId(long id);
        Task<ResponseResult> Remover(long id);
        Task<IEnumerable<PerdaViewModel>> ObterTodas(long idEmpresa);
    }
}

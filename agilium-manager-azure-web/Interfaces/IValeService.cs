using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.ValeViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IValeService
    {
        Task<PagedViewModel<ValeViewModel>> ObterPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, ValeViewModel valeViewModel);
        Task<ResponseResult> Adicionar(ValeViewModel valeViewModel);
        Task<ValeViewModel> ObterPorId(long id);
        Task<ResponseResult> Remover(long id);
        Task<IEnumerable<ValeViewModel>> ObterTodas(long idEmpresa);
        Task<ResponseResult> Cancelar(long id);
    }
}

using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.MoedaViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IMoedaService
    {
        Task<PagedViewModel<MoedaViewModel>> ObterPorNome(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, MoedaViewModel moedaViewModel);
        Task<ResponseResult> Adicionar(MoedaViewModel moedaViewModel);
        Task<MoedaViewModel> ObterPorId(long id);
        Task<ResponseResult> Remover(long id);
        Task<IEnumerable<MoedaViewModel>> ObterTodas();
        Task<MoedaViewModel> ObterListasAuxiliares();
    }
}

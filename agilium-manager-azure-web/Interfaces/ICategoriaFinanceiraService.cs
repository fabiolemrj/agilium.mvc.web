using agilium.webapp.manager.mvc.ViewModels.Empresa;
using agilium.webapp.manager.mvc.ViewModels;
using System.Threading.Tasks;
using agilium.webapp.manager.mvc.ViewModels.CategFinancViewModel;
using System.Collections.Generic;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface ICategoriaFinanceiraService
    {
        Task<PagedViewModel<CategeoriaFinanceiraViewModel>> ObterPaginacaoPorDescricao(string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, CategeoriaFinanceiraViewModel model);
        Task<ResponseResult> Adicionar(CategeoriaFinanceiraViewModel model);
        Task<CategeoriaFinanceiraViewModel> ObterPorId(long id);
        Task<ResponseResult> Remover(long id);
        Task<List<CategeoriaFinanceiraViewModel>> ObterTodos();
    }
}

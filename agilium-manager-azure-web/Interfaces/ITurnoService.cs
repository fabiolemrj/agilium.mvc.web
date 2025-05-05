using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.TurnoViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface ITurnoService
    {
        #region Turno
        Task<PagedViewModel<TurnoIndexViewModel>> ObterPaginacaoPorData(long idEmpresa, string dtInicial, string dtFinal, int page = 1, int pageSize = 15);
        Task<TurnoIndexViewModel> ObterTurnoIndexPorId(long id);
        Task<List<TurnoIndexViewModel>> ObterTodos(long idEmpresa);
        #endregion

        #region Cliente Preco
        Task<ResponseResult> Adicionar(TurnoPrecoViewModel viewModel);
        Task<TurnoPrecoViewModel> ObterTurnoPrecoPorId(long id);
        Task<ResponseResult> RemoverPreco(long id);
        Task<List<TurnoPrecoViewModel>> ObterTurnoPrecoPorProduto(long idProduto);
        Task<ResponseResult> AbrirTurno(long idEmpresa);
        Task<ResponseResult> FecharTurno(long idEmpresa);
        Task<ResponseResult> FecharTurno(TurnoIndexViewModel viewModel);
        #endregion
    }
}

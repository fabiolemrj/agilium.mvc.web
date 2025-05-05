using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.CaixaViewModel;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface ICaixaService
    {
        #region Caixa
        Task<PagedViewModel<CaixaindexViewModel>> ObterPaginacaoPorData(long idEmpresa, string dtInicial, string dtFinal, int page = 1, int pageSize = 15);
        Task<CaixaindexViewModel> ObterCaixaPorId(long id);
        #endregion

        #region CaixaMov
        Task<PagedViewModel<CaixaMovimentoViewModel>> ObterPaginacaoMovimentacaoPorCaixa(long idCaixa, int page = 1, int pageSize = 15);
        #endregion

        #region Caixa Moeda
        Task<PagedViewModel<CaixaMoedaViewModel>> ObterPaginacaoMoedaPorCaixa(long idCaixa, int page = 1, int pageSize = 15);
        Task<ResponseResult> RealizaCorrecaoValorMoeda(long id, CaixaMoedaViewModel caixaMoedaViewModel);
        Task<CaixaMoedaViewModel> ObterCaixaMoeidaPorId(long id);
        #endregion
    }
}

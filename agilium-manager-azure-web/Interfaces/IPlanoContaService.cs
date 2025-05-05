using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.PlanoContaViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IPlanoContaService
    {
        #region PlanoConta
        Task<PagedViewModel<PlanoContaViewModel>> ObterPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, PlanoContaViewModel planoContaViewModel);
        Task<ResponseResult> Adicionar(PlanoContaViewModel planoContaViewModel);
        Task<PlanoContaViewModel> ObterProdutoPorId(long id);
        Task<ResponseResult> Remover(long id);
        Task<IEnumerable<PlanoContaViewModel>> ObterTodas(long idEmpresa);
        #endregion

        #region Plano Conta Saldo
        Task<ResponseResult> Atualizar(long id, PlanoContaSaldoViewModel planoContaViewModel);
        Task<ResponseResult> Adicionar(PlanoContaSaldoViewModel planoContaViewModel);
        Task<PlanoContaSaldoViewModel> ObterSaldoPorId(long id);
        Task<ResponseResult> RemoverSaldo(long id);
        Task<ResponseResult> AtualizarSaldoPorConta(long id);
        Task<IEnumerable<PlanoContaLancamentoViewModel>> ObterLancamentosPorPlanoEData(PlanoContaLancamentoListaViewModel viewModel);
        #endregion

        #region Plano Conta Lancamento
        Task<PagedViewModel<PlanoContaLancamentoViewModel>> ObterLancamentoPaginacaoPorDescricao(long idPlano, string dtInicial, string dtFinal, int page = 1, int pageSize = 15);
        #endregion
    }
}

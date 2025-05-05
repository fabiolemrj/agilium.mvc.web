using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.ContaViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IContaService
    {
        #region Conta Pagar
        Task<PagedViewModel<ContaPagarViewModelIndex>> ObterPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> AtualizarContaPagar(long id, ContaPagarViewModel contaPagarViewModel);
        Task<ResponseResult> AdicionarContaPagar(ContaPagarViewModel contaPagarViewModel);
        Task<ContaPagarViewModel> ObterContaPagarPorId(long id);
        Task<ResponseResult> RemoverContaPagar(long id);
        Task<ResponseResult> RealizarConsolidacao(long id);
        Task<ResponseResult> RealizarDesConsolidacao(long id);
        #endregion

        #region Conta Receber
        Task<PagedViewModel<ContaReceberViewModelIndex>> ObterContaReceberPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> AtualizarContaReceber(long id, ContaReceberViewModel contaReceberViewModel);
        Task<ResponseResult> AdicionarContaReceber(ContaReceberViewModel contaReceberViewModel);
        Task<ContaReceberViewModel> ObterContaReceberPorId(long id);
        Task<ResponseResult> RemoverContaReceber(long id);
        Task<ResponseResult> RealizarConsolidacaoContaReceber(long id);
        Task<ResponseResult> RealizarDesConsolidacaoContaReceber(long id);
        #endregion
    }
}

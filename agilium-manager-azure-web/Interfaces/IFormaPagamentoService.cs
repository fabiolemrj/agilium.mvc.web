using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.FormaPagamentoViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IFormaPagamentoService
    {
        #region Forma Pagamento
        Task<PagedViewModel<FormaPagamentoViewModel>> ObterPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, FormaPagamentoViewModel model);
        Task<ResponseResult> Adicionar(FormaPagamentoViewModel model);
        Task<FormaPagamentoViewModel> ObterPorId(long id);
        Task<ResponseResult> Remover(long id);
        Task<IEnumerable<FormaPagamentoViewModel>> ObterTodas();
        #endregion
    }
}

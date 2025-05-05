using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.NotaFiscalViewModel;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface INotaFiscalService
    {
        #region Nota Fiscal Inutil
        Task<PagedViewModel<NotaFiscalnutilViewModel>> ObterPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> AtualizarNotaFiscalnutil(long id, NotaFiscalnutilViewModel nf);
        Task<ResponseResult> AdicionarNotaFiscalnutil(NotaFiscalnutilViewModel nf);
        Task<NotaFiscalnutilViewModel> ObterContaPagarPorId(long id);
        Task<ResponseResult> RemoverNotaFiscalnutil(long id);
        Task<ResponseResult> EnviarSefazNotaFiscalnutil(long id);
        #endregion
    }
}

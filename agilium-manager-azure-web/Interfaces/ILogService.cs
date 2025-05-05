using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.LogSistemaViewModel;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface ILogService
    {
        Task<PagedViewModel<LogSistemaViewModel>> ObterPaginacaoPorData(string dtInicial, string dtFinal, int page = 1, int pageSize = 15);
    }
}

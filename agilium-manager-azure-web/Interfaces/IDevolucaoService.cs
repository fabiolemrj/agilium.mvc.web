using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.DevolucaoViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IDevolucaoService
    {
        #region Devolucao
        Task<PagedViewModel<DevolucaoViewModel>> ObterPaginacaoPorData(long idEmpresa, string dtInicial, string dtFinal, int page = 1, int pageSize = 15);
        Task<ResponseResult> Adicionar(DevolucaoViewModel model);
        Task<ResponseResult> Atualizar(long id, DevolucaoViewModel model);
        Task<DevolucaoViewModel> ObterDevolucaoPorId(long id);
        Task<ResponseResult> CancelarDevolucao(long id);
        Task<ResponseResult> GerarVale(long id);
        Task<ResponseResult> Realizar(long id);
        #endregion

        #region Devolucao Item
        Task<List<DevolucaoItemViewModel>> ObterDevolucaoItemPorId(long id);
        Task<List<DevolucaoItemVendaViewModel>> ObterDevolucaoItemVendaPorId(long idDevolucao, long idVenda);
        #endregion

        #region MotivoDevolucao
        Task<PagedViewModel<MotivoDevolucaoViewModel>> ObterMotivoPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, MotivoDevolucaoViewModel model);
        Task<ResponseResult> Adicionar(MotivoDevolucaoViewModel model);
        Task<MotivoDevolucaoViewModel> ObterMotivoPorId(long id);
        Task<ResponseResult> RemoverMotivo(long id);
        Task<List<MotivoDevolucaoViewModel>> ObterTodosMotivos();
        #endregion
    }
}

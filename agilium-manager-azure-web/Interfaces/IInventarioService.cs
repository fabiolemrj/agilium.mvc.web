using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.InventarioViewModel;
using agilium.webapp.manager.mvc.ViewModels.ProdutoViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IInventarioService
    {
        #region Inventario
        Task<PagedViewModel<InventarioViewModel>> ObterPaginacaoPorDescricao(long idEmpresa, string descricao, int page = 1, int pageSize = 15);
        Task<ResponseResult> Adicionar(InventarioViewModel model);
        Task<ResponseResult> Atualizar(long id, InventarioViewModel model);
        Task<InventarioViewModel> ObterPorId(long id);
        Task<ResponseResult> Apagar(long id);
        Task<List<ProdutoViewModel>> ObterProdutoDisponivelInventario(long idEmpresa, long idInventario);
        Task<ResponseResult> IncluirProdutosInventario(AdicionarListaProdutosDisponiveisViewModel model);
        Task<ResponseResult> Concluir(long id);
        #endregion

        #region Item
        Task<List<InventarioItemViewModel>> ObterItemPorIdInventario(long id);
        Task<ResponseResult> Adicionar(InventarioItemViewModel model);
        Task<ResponseResult> Atualizar(long id, InventarioItemViewModel model);
        Task<InventarioItemViewModel> ObterItemPorId(long id);
        Task<ResponseResult> ApagarItem(long id);
        Task<ResponseResult> CadastrarProdutoPorEstoque(long id);
        Task<ResponseResult> Inventariar(long id);
        Task<ResponseResult> Apurar(List<InventarioItemViewModel> model);
        Task<ResponseResult> ApagarListaItem(List<InventarioItemViewModel> model);
        #endregion
    }
}

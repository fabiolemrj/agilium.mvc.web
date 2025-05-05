using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.SiteMercadoViewModel;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface ISiteMercadoService
    {
        #region Produto
        Task<PagedViewModel<ProdutoSiteMercadoViewModel>> ObterPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, ProdutoSiteMercadoViewModel produtoViewModel);
        Task<ResponseResult> Adicionar(ProdutoSiteMercadoViewModel produtoViewModel);
        Task<ProdutoSiteMercadoViewModel> ObterProdutoPorId(long id);
        Task<ResponseResult> Remover(long id);
        #endregion

        #region Moeda
        Task<PagedViewModel<MoedaSiteMercadoViewModel>> ObterPaginacaoMoedaPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, MoedaSiteMercadoViewModel produtoViewModel);
        Task<ResponseResult> Adicionar(MoedaSiteMercadoViewModel produtoViewModel);
        Task<MoedaSiteMercadoViewModel> ObterMoedaPorId(long id);
        Task<ResponseResult> RemoverMoeda(long id);
        #endregion

    }
}

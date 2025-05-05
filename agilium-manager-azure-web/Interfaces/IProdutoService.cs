using agilium.webapp.manager.mvc.ViewModels.CategFinancViewModel;
using agilium.webapp.manager.mvc.ViewModels;
using System.Threading.Tasks;
using agilium.webapp.manager.mvc.ViewModels.ProdutoViewModel;
using System.Collections.Generic;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IProdutoService
    {
        #region Produto
        Task<PagedViewModel<ProdutoViewModel>> ObterPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, ProdutoViewModel produtoViewModel);
        Task<ResponseResult> Adicionar(ProdutoViewModel produtoViewModel);
        Task<ProdutoViewModel> ObterProdutoPorId(long id);
        Task<ResponseResult> Remover(long id);
        Task<IEnumerable<ProdutoViewModel>> ObterTodas(long idEmpresa);
        Task<ListasAuxiliaresProdutoViewModel> ObterListasAuxiliares();
        Task<ResponseResult> AtualizarIBPT();
        #endregion

        #region ProdutoDepartamento
        Task<PagedViewModel<ProdutoDepartamentoViewModel>> ObterDepartamentoPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, ProdutoDepartamentoViewModel model);
        Task<ResponseResult> Adicionar(ProdutoDepartamentoViewModel model);
        Task<ProdutoDepartamentoViewModel> ObterDepartamentoPorId(long id);
        Task<ResponseResult> RemoverDepartamento(long id);
        #endregion

        #region ProdutoMarca
        Task<PagedViewModel<ProdutoMarcaViewModel>> ObterMarcaPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, ProdutoMarcaViewModel model);
        Task<ResponseResult> Adicionar(ProdutoMarcaViewModel model);
        Task<ProdutoMarcaViewModel> ObterMarcaPorId(long id);
        Task<ResponseResult> RemoverMarca(long id);
        #endregion

        #region Grupo
        Task<PagedViewModel<GrupoProdutoViewModel>> ObterGrupoPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, GrupoProdutoViewModel model);
        Task<ResponseResult> Adicionar(GrupoProdutoViewModel model);
        Task<GrupoProdutoViewModel> ObterGrupoPorId(long id);
        Task<ResponseResult> RemoverGrupo(long id);
        #endregion

        #region SubGrupo
        Task<PagedViewModel<SubGrupoViewModel>> ObterSubGrupoPaginacaoPorDescricao(long idGrupo, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, SubGrupoViewModel model);
        Task<ResponseResult> Adicionar(SubGrupoViewModel model);
        Task<SubGrupoViewModel> ObterSubGrupoPorId(long id);
        Task<ResponseResult> RemoverSubGrupo(long id);
        #endregion

        #region Codigo de Barra
        Task<ResponseResult> Atualizar(long id, ProdutoCodigoBarraViewModel produtoViewModel);
        Task<ResponseResult> Adicionar(ProdutoCodigoBarraViewModel produtoViewModel);
        Task<ProdutoCodigoBarraViewModel> ObterProdutoCodigoBarraPorId(long id);
        Task<ResponseResult> RemoverCodigoBarra(long id);
        Task<List<ProdutoCodigoBarraViewModel>> ObterCodigoBarraPorProduto(long idProduto);
        #endregion

        #region Produto Preco
        Task<ResponseResult> Atualizar(long id, ProdutoPrecoViewModel produtoViewModel);
        Task<ResponseResult> Adicionar(ProdutoPrecoViewModel produtoViewModel);
        Task<ProdutoPrecoViewModel> ObterProdutoPrecoPorId(long id);
        Task<ResponseResult> RemoverPreco(long id);
        Task<List<ProdutoPrecoViewModel>> ObterPrecoPorProduto(long idProduto);
        #endregion

        #region Produto Foto
        Task<ResponseResult> Atualizar(long id, ProdutoFotoViewModel produtoViewModel);
        Task<ResponseResult> Adicionar(ProdutoFotoViewModel produtoViewModel);
        Task<ProdutoFotoViewModel> ObterProdutoFotoPorId(long id);
        Task<ResponseResult> RemoverFoto(long id);
        Task<List<ProdutoFotoViewModel>> ObterFotoPorProduto(long idProduto);
        #endregion

    }
}

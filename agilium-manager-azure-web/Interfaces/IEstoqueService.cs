using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.EstoqueViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IEstoqueService
    {
        #region Estoque
        Task<PagedViewModel<EstoqueViewModel>> ObterPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, EstoqueViewModel model);
        Task<ResponseResult> Adicionar(EstoqueViewModel model);
        Task<EstoqueViewModel> ObterPorId(long id);
        Task<ResponseResult> Remover(long id);
        Task<IEnumerable<EstoqueViewModel>> ObterTodas();
        #endregion

        #region Estoque Produto
        Task<IEnumerable<EstoqueProdutoListaViewModel>> ObterProdutoEstoquePorIdProduto(long idProduto);
        Task<IEnumerable<ProdutoPorEstoqueViewModel>> ObterProdutoEstoquePorIdEstoque(long idEstoque);
        #endregion

        #region Estoque Historico
        Task<IEnumerable<EstoqueHistoricoViewModel>> ObterHistoricoEstoquePorIdProduto(long idProduto);
        #endregion

        #region Report
        Task<List<EstoquePosicaoReport>> ObterRelatorioPosicaoEstoque(long idEstoque);
        #endregion
    }
}

using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.CompraViewModel;
using agilium.webapp.manager.mvc.ViewModels.CompraViewModel.ImportacaoXmlNFE;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface ICompraService
    {
        #region Compra
        Task<PagedViewModel<CompraViewModel>> ObterPaginacaoPorData(long idEmpresa, string dtInicial, string dtFinal, int page = 1, int pageSize = 15);
        Task<ResponseResult> Adicionar(CompraViewModel model);
        Task<ResponseResult> Atualizar(long id, CompraViewModel model);
        Task<CompraViewModel> ObterCompraPorId(long id);
        Task<ResponseResult> Cancelar(long id);
        Task<ResponseResult> Efetivar(long id);
        Task<ResponseResult> CadastrarProdutosAutomaticamente(long id);
        #endregion

        #region Compra Item
        Task<List<CompraItemViewModel>> ObterItemPorIdCompra(long id);
        Task<ResponseResult> Adicionar(CompraItemViewModel model);
        Task<ResponseResult> Atualizar(long id, CompraItemViewModel model);
        Task<CompraItemViewModel> ObterItemPorId(long id);
        Task<ResponseResult> ImportarArquivo(NFeProc model);
        Task<ResponseResult> ImportarArquivo(ImportacaoArquivo model);
        Task<ResponseResult> ImportarArquivo(IFormFile formFile);
        Task<ResponseResult> ImportarArquivoXmlNfe(ImportacaoArquivo model);
        Task<ResponseResult> AtualizarProdutoItemCompra(CompraItemEditViewModel model);
        #endregion
    }
}

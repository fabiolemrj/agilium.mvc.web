using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.Contato;
using agilium.webapp.manager.mvc.ViewModels.Fornecedor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IFornecedorService
    {
        Task<PagedViewModel<FornecedorViewModel>> ObterPorRazaoSocial(string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, FornecedorViewModel fornecedorViewModel);
        Task<ResponseResult> Adicionar(FornecedorViewModel fornecedorViewModel);
        Task<FornecedorViewModel> ObterPorId(long id);
        Task<ResponseResult> Remover(long id);
        Task<IEnumerable<FornecedorViewModel>> ObterTodas();

        #region Contato
        Task<ResponseResult> Adicionar(ContatoFornecedorViewModel contato);
        Task<ContatoFornecedorViewModel> ObterPorId(long idContato, long idFornecedor);
        Task<ResponseResult> RemoverContato(long idContato, long idFornecedor);
        Task<ResponseResult> Atualizar(ContatoFornecedorViewModel contato);
        #endregion
    }
}

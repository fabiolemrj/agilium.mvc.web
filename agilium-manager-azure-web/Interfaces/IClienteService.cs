using agilium.webapp.manager.mvc.ViewModels.Contato;
using agilium.webapp.manager.mvc.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using agilium.webapp.manager.mvc.ViewModels.Cliente;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IClienteService
    {
        Task<PagedViewModel<ClienteViewModel>> ObterPorNome(string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, ClienteViewModel clienteViewModel);
        Task<ResponseResult> Adicionar(ClienteViewModel clienteViewModel);
        Task<ClienteViewModel> ObterPorId(long id);
        Task<ResponseResult> Remover(long id);
        Task<IEnumerable<ClienteViewModel>> ObterTodas();

        #region Contato
        Task<ResponseResult> Adicionar(ClienteContatoViewModel contato);
        Task<ClienteContatoViewModel> ObterPorId(long idContato, long idCliente);
        Task<ResponseResult> RemoverContato(long idContato, long idCliente);
        Task<ResponseResult> Atualizar(ClienteContatoViewModel contato);
        #endregion

        #region Cliente Preco
        Task<ResponseResult> Adicionar(ClientePrecoViewModel viewModel);
        Task<ClientePrecoViewModel> ObterClientePrecoPorId(long id);
        Task<ResponseResult> RemoverPreco(long id);
        Task<List<ClientePrecoViewModel>> ObterClientePrecoPorProduto(long idProduto);
        #endregion
    }
}

using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IFornecedorService : IDisposable
    {
        #region Fornecedor
        Task Adicionar(Fornecedor fornecedor);
        Task Atualizar(Fornecedor fornecedor);
        Task Apagar(long id);
        Task<Fornecedor> ObterPorId(long id);
        Task<List<Fornecedor>> ObterPorRazaoSocial(string descricao);
        Task<PagedResult<Fornecedor>> ObterPorRazaoSocialPaginacao(string descricao, int page = 1, int pageSize = 15);
        Task<Fornecedor> ObterCompletoPorId(long id);
        Task<List<Fornecedor>> ObterTodos();
        #endregion

        #region ContatoFornecedor
        Task AdicionarContato(FornecedorContato fornecedorContato);
        Task RemoverContato(FornecedorContato fornecedorContato);
        Task RemoverContato(long idFornecedor, long idContato);
        Task <List<FornecedorContato>> ObterFornecedoresContatosPorFornecedor(long idFornecedor);
        Task<FornecedorContato> ObterFornecedorContatoPorId(long idFornecedor, long idContato);
        Task Atualizar(FornecedorContato fornecedorContato);
        #endregion

        Task Salvar();
    }
}
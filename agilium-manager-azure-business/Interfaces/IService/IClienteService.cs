using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IClienteService: IDisposable
    {
        #region Cliente
        Task Adicionar(Cliente cliente);
        Task Adicionar(ClientePF cliente);
        Task Adicionar(ClientePJ cliente);
        Task Atualizar(Cliente cliente);
        Task Atualizar(ClientePF cliente);
        Task Atualizar(ClientePJ cliente);
        Task Apagar(long id);
        Task<Cliente> ObterPorId(long id);
        Task<List<Cliente>> ObterPorNome(string descricao);
        Task<PagedResult<Cliente>> ObterPorNomePaginacao(string descricao, int page = 1, int pageSize = 15);
        Task<Cliente> ObterCompletoPorId(long id);
        Task<List<Cliente>> ObterTodos();
        #endregion

        #region ContatoCliente
        Task AdicionarContato(ClienteContato clienteContato);
        Task RemoverContato(ClienteContato clienteContato);
        Task RemoverContato(long idCliente, long idContato);
        Task<List<ClienteContato>> ObterFornecedoresContatosPorFornecedor(long idCliente);
        Task<ClienteContato> ObterClienteContatoPorId(long idCliente, long idContato);
        Task Atualizar(ClienteContato clienteContato);
        #endregion

        #region Cliente Preco
        Task Adicionar(ClientePreco cliente);
        Task Remover(long id);
        Task<ClientePreco> ObteClientePrecoPorId(long id);
        Task<IEnumerable<ClientePreco>> ObterClientePrecoPorProduto(long idProduto);
        #endregion

        Task Salvar();

        #region Dapper
        Task<Cliente> ObterClientePorId(long id);
        Task<long> AdicionarClienteBasico(Cliente cliente,string cpf);
        Task<Cliente> ObterClienteComEnderecoPorId(long id);

        Task<Cliente> ObterClientePorCpf(string cpf);

        #endregion
    }
}

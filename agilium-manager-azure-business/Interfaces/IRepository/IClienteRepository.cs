using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface IClienteRepository : IRepository<Cliente>
    {
    }

    public interface IClientePFRepository : IRepository<ClientePF>
    {
    }

    public interface IClientePJRepository : IRepository<ClientePJ>
    {
    }

    public interface IClienteContatoRepository : IRepository<ClienteContato>
    {
    }

    public interface IClientePrecoRepository : IRepository<ClientePreco>
    {
    }

    public interface IClienteDapperRepository
    {
        Task<Cliente> ObterClientePorId(long id);
        Task<long> AdicionarClienteBasico(Cliente cliente);
        Task<bool> AdicionarClientePF(ClientePF cliente);
        Task<Cliente> ObterClienteComEnderecoPorId(long id);
        Task<Cliente> ObterClientePorCpf(string cpf);
    }
}

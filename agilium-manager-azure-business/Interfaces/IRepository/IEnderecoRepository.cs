using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface IEnderecoRepository : IRepository<Endereco>
    {
    }

    public interface ICepRepository : IRepository<Cep>
    {
    }

    public interface IEnderecoDapperRepository
    {
        Task<Endereco> AdicionarEndereco(string logradouro, string numero, string complemento,string bairro, string cep, string cidade,string uf, string pais);
        Task<Endereco> AdicionarEndereco(Endereco endereco);
        Task<Cep> ObterEnderecoPorCep(string cep);

    }
}

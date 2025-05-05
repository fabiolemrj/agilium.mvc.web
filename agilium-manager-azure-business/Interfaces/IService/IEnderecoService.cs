using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IEnderecoService: IDisposable
    {
        Task<bool> AdicionarEndereco(Endereco endereco);
        Task<bool> AtualizarEndereco(Endereco endereco);
        Task<bool> ApagarEndereco(long id);
        Task<Endereco> ObterEnderecoPorId(long id);
        Task<Cep> ObterCepPorNumeroCep(string cep);
        Task<bool> AtualizarAdicionar(Endereco endereco);
        Task Salvar();
    }
}

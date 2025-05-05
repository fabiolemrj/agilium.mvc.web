using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface IFornecedorRepsoitory : IRepository<Fornecedor>
    {
    }

    public interface IFornecedorContatoRepsoitory : IRepository<FornecedorContato>
    {
    }

    public interface IFornecedorDapperRepository
    {
        Task<Fornecedor> AdicionarFornecedor(string razaoSocial, string nomeFantasia, ETipoPessoa tipoPessoa, string cnpj, string inscricaoEstadual,ETipoFiscal tipoFiscal, Endereco endereco);
        Task<Fornecedor> ObterFornecedorPorCNPJ(string cnpj);
    }
}

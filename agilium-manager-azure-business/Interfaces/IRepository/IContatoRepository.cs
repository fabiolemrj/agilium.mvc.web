using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface IContatoRepository: IRepository<Contato>
    {
    }

    public interface IContatoEmpresaRepository : IRepository<ContatoEmpresa>
    {
    }

    public interface IContatoDapperRepository
    {
        Task<Contato> AdicionarContato(ETipoContato tipoContato, string descricao1,  string descricao2, long idFornecedor);
        Task<bool> AdicionarContatoFornecedor(long idContato, long idFornecedor);
    }
}

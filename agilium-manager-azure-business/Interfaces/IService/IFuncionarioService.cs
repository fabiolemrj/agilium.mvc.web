using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IFuncionarioService: IDisposable
    {
        Task Adicionar(Funcionario funcionario);
        Task Atualizar(Funcionario funcionario);
        Task Apagar(long id);
        Task<Funcionario> ObterPorId(long id);
        Task<List<Funcionario>> ObterPorNome(string descricao);
        Task<PagedResult<Funcionario>> ObterPorNomePaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<Funcionario> ObterCompletoPorId(long id);
        Task<List<Funcionario>> ObterTodas();
        Task Salvar();
    }
}

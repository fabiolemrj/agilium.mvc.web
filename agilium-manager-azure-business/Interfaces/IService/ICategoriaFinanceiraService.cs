using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface ICategoriaFinanceiraService : IDisposable
    {
        Task<bool> Adicionar(CategoriaFinanceira categoriaFinanceira);
        Task<bool> Atualizar(CategoriaFinanceira categoriaFinanceira);
        Task Remover(long id);
        Task<IEnumerable<CategoriaFinanceira>> ObterTodos();
        Task<CategoriaFinanceira> ObterPorId(long id);
        Task<bool> Existe(CategoriaFinanceira categoriaFinanceira);
        Task<PagedResult<CategoriaFinanceira>> ObterPorDescricaoPaginacao(string descricao, int page = 1, int pageSize = 15);
        Task Salvar();
    }
}

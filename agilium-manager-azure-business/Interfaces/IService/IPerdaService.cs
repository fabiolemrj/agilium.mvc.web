using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IPerdaService : IDisposable
    {
        Task Salvar();
        Task Adicionar(Perda perda);
        Task Atualizar(Perda perda);
        Task Apagar(long id);
        Task<Perda> ObterPorId(long id);
        Task<IEnumerable<Perda>> ObterTodas(long idEmpresa);
        Task<PagedResult<Perda>> ObterValePorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
    }
}

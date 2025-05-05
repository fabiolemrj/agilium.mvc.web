using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IUnidadeService : IDisposable
    {
        Task Adicionar(Unidade unidade);
        Task Atualizar(Unidade unidade);
        Task Apagar(long id);
        Task<Unidade> ObterPorId(long id);
        Task<PagedResult<Unidade>> ObterPorDescricaoPaginacao(string descricao, int page = 1, int pageSize = 15);
        Task<bool> MudarSituacao(long id);
        Task Salvar();
        Task<List<Unidade>> ObterTodas();
    }
}

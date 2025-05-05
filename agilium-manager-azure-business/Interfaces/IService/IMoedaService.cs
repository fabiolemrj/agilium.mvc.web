using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IMoedaService: IDisposable
    {
        Task Adicionar(Moeda moeda);
        Task Atualizar(Moeda moeda);
        Task Apagar(long id);
        Task<Moeda> ObterPorId(long id);
        Task<List<Moeda>> ObterPorDescricao(string descricao);
        Task<PagedResult<Moeda>> ObterPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<Moeda> ObterCompletoPorId(long id);
        Task<List<Moeda>> ObterTodas();
        Task Salvar();
    }
}

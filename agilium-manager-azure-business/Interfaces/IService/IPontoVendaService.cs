using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IPontoVendaService : IDisposable
    {
        Task Adicionar(PontoVenda pontoVenda);
        Task Atualizar(PontoVenda pontoVenda);
        Task Apagar(long id);
        Task<PontoVenda> ObterPorId(long id);
        Task<List<PontoVenda>> ObterPorDescricao(string descricao);
        Task<PagedResult<PontoVenda>> ObterPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<PontoVenda> ObterCompletoPorId(long id);
        Task<List<PontoVenda>> ObterTodas();
        Task Salvar();

    }
}

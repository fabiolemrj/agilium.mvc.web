using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface ISiteMercadoService: IDisposable
    {
        Task Salvar();

        #region ProdutoSM
        Task Adicionar(ProdutoSiteMercado produto);
        Task Atualizar(ProdutoSiteMercado produto);
        Task Apagar(long id);
        Task<ProdutoSiteMercado> ObterPorId(long id);
        Task<PagedResult<ProdutoSiteMercado>> ObterPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<IEnumerable<ProdutoSiteMercado>> ObterTodas(long idEmpresa);
        #endregion

        #region MoedaSM
        Task Adicionar(MoedaSiteMercado moeda);
        Task Atualizar(MoedaSiteMercado moeda);
        Task ApagarMoeda(long id);
        Task<MoedaSiteMercado> ObterMoedaPorId(long id);
        Task<PagedResult<MoedaSiteMercado>> ObterMoedaPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<bool> MoedaSMJaAssociada(ETipoMoedaSiteMercado idMoedaSm, long idEmpresa, long id);
        #endregion
    }
}

using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.ReportViewModel.EstoqueReportViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IEstoqueService: IDisposable
    {
        #region Estoque
        Task Adicionar(Estoque estoque);
        Task Atualizar(Estoque estoque);
        Task Apagar(long id);
        Task<Estoque> ObterPorId(long id);
        Task<List<Estoque>> ObterPorDescricao(string descricao);
        Task<PagedResult<Estoque>> ObterPorDescricaoPaginacao(long idEmpresa, string descricao, int page = 1, int pageSize = 15);
        Task<Estoque> ObterCompletoPorId(long id);
        Task<List<Estoque>> ObterTodas();
        #endregion

        #region Estoque Produto
        Task Adicionar(EstoqueProduto estoque);
        Task Atualizar(EstoqueProduto estoque);
        Task ApagarProduto(long id);
        Task<EstoqueProduto> ObterProdutoPorId(long id);
        Task<List<EstoqueProduto>> ObterProdutoEstoquePorProduto(long idProduto);
        Task<List<EstoqueProduto>> ObterProdutoEstoquePorEstoque(long idEstoque);
        #endregion

        #region Estoque Historico
        Task Adicionar(EstoqueHistorico estoque);
        Task Atualizar(EstoqueHistorico estoque);
        Task ApagarHistorico(long id);
        Task<EstoqueHistorico> ObterHistoricoPorId(long id);
        Task<List<EstoqueHistorico>> ObterHistoricoEstoquePorProduto(long idProduto);
        #endregion
        Task Salvar();

        #region Report
        Task<List<EstoquePosicaoReport>> ObterRelatorioPosicaoEstoque(long idEstoque);
        #endregion
    }
}

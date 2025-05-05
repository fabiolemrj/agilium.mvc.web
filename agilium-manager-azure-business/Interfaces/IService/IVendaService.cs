using agilium.api.business.Enums;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.ReportViewModel.VendaReportViewModel;
using agilium.api.business.Models.CustomReturn.VendaCustomViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IVendaService: IDisposable
    {
        Task Salvar();

        #region Venda
        Task Adicionar(Venda venda);
        Task Atualizar(Venda venda);
        Task Apagar(long id);
        Task<Venda> ObterPorId(long id);
        Task<List<Venda>> ObterPorDescricao(string descricao);
        Task<PagedResult<Venda>> ObterPorPaginacao(DateTime dtIni, DateTime dtFim, int page = 1, int pageSize = 15);
        Task<Venda> ObterCompletoPorId(long id);
        Task<List<Venda>> ObterTodas();
        Task<List<Venda>> ObterVendaPorData(DateTime dtIni, DateTime dtFim);

        Task<double> ObterValorTotalVenda(long id);
        #endregion

        #region Venda Item
        Task<List<VendaItem>> ObterItensVenda(long idVenda);
        Task<VendaItem> ObterItemPorVenda(long idItemVenda);
        #endregion

        #region Venda Moeda
        Task<List<VendaMoeda>> ObterMoedasVenda(long idVenda);
        #endregion

        #region Venda Espelho
        Task<VendaEspelho> ObterVendaEspelhoPorIdVenda(long idVenda);
        #endregion

        #region Report
        Task<VendasReportViewModel> ObterRelatorioVendaDetalhada(DateTime dataInicial, DateTime dataFinal);
        Task<VendasFornecedorViewModel> ObterRelatorioVendaPorFornecedor(DateTime dataInicial, DateTime dataFinal);
        Task<VendaMoedaReport> ObterRelatorioVendaPorMoeda(DateTime dataInicial, DateTime dataFinal);
        Task<List<VendaRankingReport>> ObterVendaRankingPorData(DateTime dataInicial, DateTime dataFinal, EResultadoFiltroRanking tipoResultado, EOrdenacaoFiltroRanking ordenacao);
        Task<List<VendaDiferencaCaixaReport>> ObterVendaDiferencaCaixa(DateTime dataInicial, DateTime dataFinal);
        #endregion

        #region Dapper
        Task<VendaNovaCustomViewModel> ObterDadosParaNovaVenda(long idUsuario, long idEmpresa);
        Task<bool> RealizarVenda(Venda venda, long idUsusario, long IdEmpresa);

        #endregion
    }
}

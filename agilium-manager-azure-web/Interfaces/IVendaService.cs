using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.VendaReportViewModel;
using agilium.webapp.manager.mvc.ViewModels.VendaViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IVendaService
    {
        #region Venda
        Task<PagedViewModel<VendaViewModel>> ObterPaginacaoPorData(string dtInicial, string dtFinal, int page = 1, int pageSize = 15);
        Task<VendaViewModel> ObterVendaPorId(long id);
        Task<VendaDetalhesViewModel> ObterDetalheVendaPorId(long id);
        Task<VendaRankingProdutoIndexViewModel> ObterVendaRankingPorduto(VendaRankingProdutoIndexViewModel model);
        Task<IEnumerable<VendaViewModel>> ObterVendaPorData(string dtInicial, string dtFinal);
        #endregion

        #region Venda Item
        Task<IEnumerable<VendaItemViewModel>> ObterItensPorVenda(long idVenda);
        #endregion

        #region Venda Moeda
        Task<IEnumerable<VendaMoedaViewModel>> ObterMoedasPorVenda(long idVenda);
        #endregion

        #region Venda Espelho
        Task<VendaEspelhoViewModel> ObterEspelhoPorVenda(long idVenda);
        #endregion

        #region Report
        Task<VendasReportViewModel> ObterRelatorioVendaDetalhada(DateTime dataInicial, DateTime dataFinal);
        Task<VendasFornecedorViewModel> ObterRelatorioVendaPorFornecedor(DateTime dataInicial, DateTime dataFinal);
        Task<VendaMoedaReport> ObterRelatorioVendaPorMoeda(DateTime dataInicial, DateTime dataFinal);
        Task<VendaFiltroRankingViewModel> ObterVendasPorRanking(VendaFiltroRankingViewModel viewModel);
        Task<List<VendaDiferencaCaixaReport>> ObterVendasPorDiferenca(DateTime dataInicial, DateTime dataFinal);
        #endregion
    }
}

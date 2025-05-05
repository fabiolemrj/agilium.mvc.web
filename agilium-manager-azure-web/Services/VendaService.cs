using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.VendaReportViewModel;
using agilium.webapp.manager.mvc.ViewModels.VendaViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class VendaService : Service, IVendaService
    {
        public VendaService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        #region Venda

        public async Task<VendaViewModel> ObterVendaPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/venda/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<VendaViewModel>(response);
        }

        public async Task<PagedViewModel<VendaViewModel>> ObterPaginacaoPorData(string dtInicial, string dtFinal, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/venda/obter-por-data?dtInicial={dtInicial}&dtFinal={dtFinal}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<VendaViewModel>>(response);
        }

        public async Task<VendaDetalhesViewModel> ObterDetalheVendaPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/venda/detalhes-venda/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<VendaDetalhesViewModel>(response);
        }

        public async Task<VendaRankingProdutoIndexViewModel> ObterVendaRankingPorduto(VendaRankingProdutoIndexViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/venda/ranking-por-produto",itemContent);

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<VendaRankingProdutoIndexViewModel>(response);
        }

        public async Task<IEnumerable<VendaViewModel>> ObterVendaPorData(string dtInicial, string dtFinal)
        {
            var response = await _httpClient.GetAsync($"/api/v1/venda/vendas-por-data?dtInicial={dtInicial}&dtFinal={dtFinal}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<IEnumerable<VendaViewModel>>(response);
        }

        #endregion

        #region Venda Item
        public async Task<IEnumerable<VendaItemViewModel>> ObterItensPorVenda(long idVenda)
        {
            var response = await _httpClient.GetAsync($"/api/v1/venda/itens/{idVenda}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<IEnumerable<VendaItemViewModel>>(response);
        }
        #endregion

        #region Venda Moeda
        public async Task<IEnumerable<VendaMoedaViewModel>> ObterMoedasPorVenda(long idVenda)
        {
            var response = await _httpClient.GetAsync($"/api/v1/venda/moedas/{idVenda}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<IEnumerable<VendaMoedaViewModel>>(response);
        }


        #endregion

        #region Venda Espelho
        public async Task<VendaEspelhoViewModel> ObterEspelhoPorVenda(long idVenda)
        {
            var response = await _httpClient.GetAsync($"/api/v1/venda/espelho/{idVenda}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<VendaEspelhoViewModel>(response);
        }

        public async Task<VendasReportViewModel> ObterRelatorioVendaDetalhada(DateTime dataInicial, DateTime dataFinal)
        {
            var response = await _httpClient.GetAsync($"/api/v1/venda/report/detalhada-por-data?dtInicial={dataInicial}&dtFinal={dataFinal}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<VendasReportViewModel>(response);
        }

        public async Task<VendasFornecedorViewModel> ObterRelatorioVendaPorFornecedor(DateTime dataInicial, DateTime dataFinal)
        {
            var response = await _httpClient.GetAsync($"/api/v1/venda/report/fornecedor-por-data?dtInicial={dataInicial}&dtFinal={dataFinal}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<VendasFornecedorViewModel>(response);
        }

        public async Task<VendaMoedaReport> ObterRelatorioVendaPorMoeda(DateTime dataInicial, DateTime dataFinal)
        {
            var response = await _httpClient.GetAsync($"/api/v1/venda/report/moeda-por-data?dtInicial={dataInicial}&dtFinal={dataFinal}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<VendaMoedaReport>(response);
        }

        public async Task<VendaFiltroRankingViewModel> ObterVendasPorRanking(VendaFiltroRankingViewModel viewModel)
        {
            var itemContent = ObterConteudo(viewModel);

            var response = await _httpClient.PostAsync($"/api/v1/venda/report/ranking",itemContent);

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<VendaFiltroRankingViewModel>(response);
        }

        public async Task<List<VendaDiferencaCaixaReport>> ObterVendasPorDiferenca(DateTime dataInicial, DateTime dataFinal)
        {
            var response = await _httpClient.GetAsync($"/api/v1/venda/report/diferenca?dtInicial={dataInicial}&dtFinal={dataFinal}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<VendaDiferencaCaixaReport>>(response);
        }

        #endregion
    }
}

using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.CaixaViewModel;
using agilium.webapp.manager.mvc.ViewModels.CaManagerViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class CaixaService : Service, ICaixaService
    {
        public CaixaService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        #region Caixa Movimentacao
        public async Task<PagedViewModel<CaixaMovimentoViewModel>> ObterPaginacaoMovimentacaoPorCaixa(long idCaixa, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/caixa/movimentacao?idCaixa={idCaixa}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<CaixaMovimentoViewModel>>(response);
        }
        #endregion

        #region Caixa
        public async Task<PagedViewModel<CaixaindexViewModel>> ObterPaginacaoPorData(long idEmpresa, string dtInicial, string dtFinal, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/caixa/obter-por-data?idEmpresa={idEmpresa}&dtInicial={dtInicial}&dtFinal={dtFinal}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<CaixaindexViewModel>>(response);
        }

        public async Task<CaixaindexViewModel> ObterCaixaPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/caixa/obter-por-id/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<CaixaindexViewModel>(response);
        }


        #endregion

        #region Caixa Moeda
        public async Task<PagedViewModel<CaixaMoedaViewModel>> ObterPaginacaoMoedaPorCaixa(long idCaixa, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/caixa/moedas?idCaixa={idCaixa}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<CaixaMoedaViewModel>>(response);
        }

        public async Task<ResponseResult> RealizaCorrecaoValorMoeda(long id, CaixaMoedaViewModel caixaMoedaViewModel)
        {
            var itemContent = ObterConteudo(caixaMoedaViewModel);

            var response = await _httpClient.PutAsync($"/api/v1/caixa/moeda/correcao/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<CaixaMoedaViewModel> ObterCaixaMoeidaPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/caixa/moeda/obter-por-id/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<CaixaMoedaViewModel>(response);
        }
        #endregion


    }
}

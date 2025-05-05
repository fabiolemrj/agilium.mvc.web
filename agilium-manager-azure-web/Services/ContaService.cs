using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.ContaViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class ContaService : Service, IContaService
    {
        public ContaService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }
              

        #region Conta Pagar
        public async Task<PagedViewModel<ContaPagarViewModelIndex>> ObterPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/conta/pagar/obter-por-descricao?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<ContaPagarViewModelIndex>>(response); 
        }

        public async Task<ResponseResult> AdicionarContaPagar(ContaPagarViewModel contaPagarViewModel)
        {
            var itemContent = ObterConteudo(contaPagarViewModel);

            var response = await _httpClient.PostAsync($"/api/v1/conta/pagar", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> AtualizarContaPagar(long id, ContaPagarViewModel contaPagarViewModel)
        {
            var itemContent = ObterConteudo(contaPagarViewModel);

            var response = await _httpClient.PutAsync($"/api/v1/conta/pagar/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ContaPagarViewModel> ObterContaPagarPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/conta/pagar/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ContaPagarViewModel>(response);
        }

        public async Task<ResponseResult> RemoverContaPagar(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/conta/pagar/{id.ToString()}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> RealizarConsolidacao(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/conta/pagar/consolidar/{id}");

            TratarErrosResponse(response);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> RealizarDesConsolidacao(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/conta/pagar/desconsolidar/{id}");

            TratarErrosResponse(response);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }


        #endregion

        #region Conta Receber
        public async Task<PagedViewModel<ContaReceberViewModelIndex>> ObterContaReceberPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/conta/receber/obter-por-descricao?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<ContaReceberViewModelIndex>>(response);
        }

        public async Task<ResponseResult> AtualizarContaReceber(long id, ContaReceberViewModel contaReceberViewModel)
        {
            var itemContent = ObterConteudo(contaReceberViewModel);

            var response = await _httpClient.PutAsync($"/api/v1/conta/receber/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> AdicionarContaReceber(ContaReceberViewModel contaReceberViewModel)
        {
            var itemContent = ObterConteudo(contaReceberViewModel);

            var response = await _httpClient.PostAsync($"/api/v1/conta/receber", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ContaReceberViewModel> ObterContaReceberPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/conta/receber/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ContaReceberViewModel>(response);
        }

        public async Task<ResponseResult> RemoverContaReceber(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/conta/receber/{id.ToString()}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> RealizarConsolidacaoContaReceber(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/conta/receber/consolidar/{id}");

            TratarErrosResponse(response);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> RealizarDesConsolidacaoContaReceber(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/conta/receber/desconsolidar/{id}");

            TratarErrosResponse(response);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }
        #endregion
    }
}

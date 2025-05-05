using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.PlanoContaViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class PlanoContaService : Service, IPlanoContaService
    {
        public PlanoContaService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        #region Plano Conta
        public async Task<ResponseResult> Adicionar(PlanoContaViewModel planoContaViewModel)
        {
            var itemContent = ObterConteudo(planoContaViewModel);

            var response = await _httpClient.PostAsync($"/api/v1/plano-conta", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, PlanoContaViewModel planoContaViewModel)
        {
            var itemContent = ObterConteudo(planoContaViewModel);

            var response = await _httpClient.PutAsync($"/api/v1/plano-conta/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }


        public async Task<PagedViewModel<PlanoContaViewModel>> ObterPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/plano-conta/obter-por-descricao?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<PlanoContaViewModel>>(response);
        }

        public async Task<PlanoContaViewModel> ObterProdutoPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/plano-conta/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PlanoContaViewModel>(response);
        }

        public async Task<IEnumerable<PlanoContaViewModel>> ObterTodas(long idEmpresa)
        {
            var response = await _httpClient.GetAsync($"/api/v1/plano-conta/obter-todos-por-empresa/{idEmpresa}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<PlanoContaViewModel>>(response);
        }

        public async Task<ResponseResult> Remover(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/plano-conta/{id.ToString()}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        #endregion

        #region Plano Saldo

        public async Task<ResponseResult> RemoverSaldo(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task<PlanoContaSaldoViewModel> ObterSaldoPorId(long id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ResponseResult> Atualizar(long id, PlanoContaSaldoViewModel planoContaViewModel)
        {
            throw new System.NotImplementedException();
        }

        public Task<ResponseResult> Adicionar(PlanoContaSaldoViewModel planoContaViewModel)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ResponseResult> AtualizarSaldoPorConta(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/plano-conta/saldo/atualizar/{id}");

            TratarErrosResponse(response);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);
            
            return RetornoOk();
        }

        public async Task<IEnumerable<PlanoContaLancamentoViewModel>> ObterLancamentosPorPlanoEData(PlanoContaLancamentoListaViewModel viewModel)
        {
            var itemContent = ObterConteudo(viewModel);

            var response = await _httpClient.PostAsync($"/api/v1/plano-conta/lancamentos", itemContent);

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<PlanoContaLancamentoViewModel>>(response);

        }

        #endregion

        #region Plano Conta Lancamento

        public async Task<PagedViewModel<PlanoContaLancamentoViewModel>> ObterLancamentoPaginacaoPorDescricao(long idPlano, string dtInicial, string dtFinal, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/plano-conta/lancamentos/obter-por-data?idPlano={idPlano}&dtInicial={dtInicial}&dtFinal={dtFinal}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<PlanoContaLancamentoViewModel>>(response);
        }

        #endregion
    }
}

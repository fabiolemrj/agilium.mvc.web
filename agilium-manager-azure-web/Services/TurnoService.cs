using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.TurnoViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class TurnoService : Service, ITurnoService
    {
        public TurnoService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        #region Turno

        public async Task<PagedViewModel<TurnoIndexViewModel>> ObterPaginacaoPorData(long idEmpresa, string dtInicial, string dtFinal, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/turno/obter-por-data?idEmpresa={idEmpresa}&dtInicial={dtInicial}&dtFinal={dtFinal}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<TurnoIndexViewModel>>(response);
        }

        public async Task<ResponseResult> AbrirTurno(long idEmpresa)
        {
        
            var response = await _httpClient.GetAsync($"/api/v1/turno/abrir/{idEmpresa}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> FecharTurno(long idEmpresa)
        {
            var response = await _httpClient.GetAsync($"/api/v1/turno/fechar/{idEmpresa}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<TurnoIndexViewModel> ObterTurnoIndexPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/turno/obter-turno-aberto/{id}");

            if (!TratarErrosResponse(response)) return null; 

            return await DeserializarObjetoResponse<TurnoIndexViewModel>(response);
        }


        public async Task<ResponseResult> FecharTurno(TurnoIndexViewModel viewModel)
        {
            var itemContent = ObterConteudo(viewModel);

            var response = await _httpClient.PostAsync($"/api/v1/turno/fechar", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<List<TurnoIndexViewModel>> ObterTodos(long idEmpresa)
        {
            var response = await _httpClient.GetAsync($"/api/v1/turno/obter-todos/{idEmpresa}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<TurnoIndexViewModel>>(response);
        }

        #endregion

        #region Turno Preco
        public async Task<ResponseResult> Adicionar(TurnoPrecoViewModel viewModel)
        {
            var itemContent = ObterConteudo(viewModel);

            var response = await _httpClient.PostAsync($"/api/v1/turno/preco/", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }


        public async Task<TurnoPrecoViewModel> ObterTurnoPrecoPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/turno/preco/obter-por-id/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<TurnoPrecoViewModel>(response);
        }

        public async Task<List<TurnoPrecoViewModel>> ObterTurnoPrecoPorProduto(long idProduto)
        {
            var response = await _httpClient.GetAsync($"/api/v1/turno/precos/obter-por-idproduto/{idProduto}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<TurnoPrecoViewModel>>(response);
        }

        public async Task<ResponseResult> RemoverPreco(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/turno/preco/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }





        #endregion
    }
}

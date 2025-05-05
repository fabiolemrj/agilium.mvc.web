using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.ValeViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class ValeService : Service, IValeService
    {
        public ValeService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        #region Vale
        public async Task<ResponseResult> Adicionar(ValeViewModel valeViewModel)
        {
            var itemContent = ObterConteudo(valeViewModel);

            var response = await _httpClient.PostAsync($"/api/v1/vale", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, ValeViewModel valeViewModel)
        {
            var itemContent = ObterConteudo(valeViewModel);

            var response = await _httpClient.PutAsync($"/api/v1/vale/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }


        public async Task<PagedViewModel<ValeViewModel>> ObterPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/vale/obter-por-nome-cliente?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<ValeViewModel>>(response);
        }

        public async Task<ValeViewModel> ObterPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/vale/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ValeViewModel>(response);
        }

        public async Task<IEnumerable<ValeViewModel>> ObterTodas(long idEmpresa)
        {
            var response = await _httpClient.GetAsync($"/api/v1/vale/obter-todos-por-empresa/{idEmpresa}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<ValeViewModel>>(response); ;
        }

        public async Task<ResponseResult> Remover(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/vale/{id.ToString()}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Cancelar(long id)
        {

            var response = await _httpClient.GetAsync($"/api/v1/vale/cancelar/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }
        #endregion
    }
}

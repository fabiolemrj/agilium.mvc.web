
using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.UnidadeViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class UnidadeService : Service, IUnidadeService
    {
        public UnidadeService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        public async Task<ResponseResult> Adicionar(UnidadeIndexViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/unidade/", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, UnidadeIndexViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PutAsync($"/api/v1/unidade/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<PagedViewModel<UnidadeIndexViewModel>> ObterPorRazaoSocial(string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/unidade/obter-por-descricao?q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<UnidadeIndexViewModel>>(response);
        }

        public async Task<UnidadeIndexViewModel> ObterPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/unidade/obter-por-id/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<UnidadeIndexViewModel>(response);
        }

        public async Task<ResponseResult> Remover(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/unidade/{ id.ToString()}");

            TratarErrosResponse(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> MudarSituacao(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/unidade/mudar-situacao/{ id.ToString()}");

            if(!TratarErrosResponse(response))
            {
                return await DeserializarObjetoResponse<ResponseResult>(response);
            }

            return RetornoOk();
        }

        public async  Task<List<UnidadeIndexViewModel>> ObterTodas()
        {
            var response = await _httpClient.GetAsync($"/api/v1/unidade/obter-todas");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<UnidadeIndexViewModel>>(response);
        }
    }
}

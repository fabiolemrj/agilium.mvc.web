
using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.CategFinancViewModel;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class CategoriaFinanceiraService : Service, ICategoriaFinanceiraService
    {
        public CategoriaFinanceiraService(HttpClient httpClient, IOptions<AppSettings> settings, 
            IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        public async Task<ResponseResult> Adicionar(CategeoriaFinanceiraViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/categ-financ/", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, CategeoriaFinanceiraViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PutAsync($"/api/v1/categ-financ/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<PagedViewModel<CategeoriaFinanceiraViewModel>> ObterPaginacaoPorDescricao(string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/categ-financ/obter-por-descricao?q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<CategeoriaFinanceiraViewModel>>(response);
        }

        public async Task<CategeoriaFinanceiraViewModel> ObterPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/categ-financ/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<CategeoriaFinanceiraViewModel>(response);
        }

        public async Task<List<CategeoriaFinanceiraViewModel>> ObterTodos()
        {
            var response = await _httpClient.GetAsync($"/api/v1/categ-financ/obter-todos");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<CategeoriaFinanceiraViewModel>>(response);
        }

        public async Task<ResponseResult> Remover(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/categ-financ/{id.ToString()}");

            TratarErrosResponse(response);

            return RetornoOk();
        }
    }
}

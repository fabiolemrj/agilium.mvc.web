using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.MoedaViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class MoedaService : Service, IMoedaService
    {
        public MoedaService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        public async Task<ResponseResult> Adicionar(MoedaViewModel moedaViewModel)
        {
            var itemContent = ObterConteudo(moedaViewModel);

            var response = await _httpClient.PostAsync($"/api/v1/moeda", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, MoedaViewModel moedaViewModel)
        {
            var itemContent = ObterConteudo(moedaViewModel);

            var response = await _httpClient.PutAsync($"/api/v1/moeda/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<MoedaViewModel> ObterListasAuxiliares()
        {
            var response = await _httpClient.GetAsync($"/api/v1/moeda/listas-auxiliares");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<MoedaViewModel>(response); ;
        }

        public async Task<MoedaViewModel> ObterPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/moeda/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<MoedaViewModel>(response);
        }

        public async Task<PagedViewModel<MoedaViewModel>> ObterPorNome(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/moeda/obter-por-descricao?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<MoedaViewModel>>(response);
        }

        public async Task<IEnumerable<MoedaViewModel>> ObterTodas()
        {
            var response = await _httpClient.GetAsync($"/api/v1/moeda/obter-todas");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<IEnumerable<MoedaViewModel>>(response);
        }

        public async Task<ResponseResult> Remover(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/moeda/{id.ToString()}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }
    }
}

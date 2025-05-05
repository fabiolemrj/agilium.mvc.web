using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class PerdaService : Service, IPerdaService
    {
        public PerdaService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        public async Task<ResponseResult> Adicionar(ViewModels.PerdaViewModel.PerdaViewModel viewModel)
        {
            var itemContent = ObterConteudo(viewModel);

            var response = await _httpClient.PostAsync($"/api/v1/perda", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, ViewModels.PerdaViewModel.PerdaViewModel viewModel)
        {
            var itemContent = ObterConteudo(viewModel);

            var response = await _httpClient.PutAsync($"/api/v1/perda/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<PagedViewModel<ViewModels.PerdaViewModel.PerdaViewModel>> ObterPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/perda/obter-por-nome-produto?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<ViewModels.PerdaViewModel.PerdaViewModel>>(response);
        }

        public async Task<ViewModels.PerdaViewModel.PerdaViewModel> ObterPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/perda/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ViewModels.PerdaViewModel.PerdaViewModel>(response);
        }

        public async Task<IEnumerable<ViewModels.PerdaViewModel.PerdaViewModel>> ObterTodas(long idEmpresa)
        {
            var response = await _httpClient.GetAsync($"/api/v1/perda/obter-todos-por-empresa/{idEmpresa}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<ViewModels.PerdaViewModel.PerdaViewModel>>(response);
        }

        public async Task<ResponseResult> Remover(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/perda/{id.ToString()}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }
    }
}

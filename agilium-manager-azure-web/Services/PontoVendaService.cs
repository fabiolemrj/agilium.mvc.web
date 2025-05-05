using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.PontoVendaViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class PontoVendaService : Service, IPontoVendaService
    {
        public PontoVendaService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        public async Task<ResponseResult> Adicionar(PontoVendaViewModel pontoVendaViewModel)
        {
            var itemContent = ObterConteudo(pontoVendaViewModel);

            var response = await _httpClient.PostAsync($"/api/v1/pdv", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, PontoVendaViewModel pontoVendaViewModel)
        {
            var itemContent = ObterConteudo(pontoVendaViewModel);

            var response = await _httpClient.PutAsync($"/api/v1/pdv/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public Task<PontoVendaViewModel> ObterListasAuxiliares()
        {
            throw new System.NotImplementedException();
        }

        public async Task<PontoVendaViewModel> ObterPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/pdv/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PontoVendaViewModel>(response);
        }

        public async Task<PagedViewModel<PontoVendaViewModel>> ObterPorNome(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/pdv/obter-por-descricao?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<PontoVendaViewModel>>(response);
        }

        public Task<IEnumerable<PontoVendaViewModel>> ObterTodas()
        {
            throw new System.NotImplementedException();
        }

        public async Task<ResponseResult> Remover(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/pdv/{id.ToString()}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }
    }
}

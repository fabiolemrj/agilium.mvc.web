using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.FormaPagamentoViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class FormaPagamentoService : Service, IFormaPagamentoService
    {
        public FormaPagamentoService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        public async Task<ResponseResult> Adicionar(FormaPagamentoViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/forma-pagamento", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, FormaPagamentoViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PutAsync($"/api/v1/forma-pagamento/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<PagedViewModel<FormaPagamentoViewModel>> ObterPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/forma-pagamento/obter-por-descricao?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<FormaPagamentoViewModel>>(response);
        }

        public async Task<FormaPagamentoViewModel> ObterPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/forma-pagamento/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<FormaPagamentoViewModel>(response);
        }

        public async Task<IEnumerable<FormaPagamentoViewModel>> ObterTodas()
        {
            var response = await _httpClient.GetAsync($"/api/v1/forma-pagamento/obter-todas");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<FormaPagamentoViewModel>>(response);
        }

        public async Task<ResponseResult> Remover(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/forma-pagamento/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }
    }
}

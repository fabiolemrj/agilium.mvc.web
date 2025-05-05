using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.NotaFiscalViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class NotaFiscalService : Service, INotaFiscalService
    {
        public NotaFiscalService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        #region Nota Fiscal Inutil
        public async Task<ResponseResult> AdicionarNotaFiscalnutil(NotaFiscalnutilViewModel nf)
        {
            var itemContent = ObterConteudo(nf);

            var response = await _httpClient.PostAsync($"/api/v1/nota-fiscal/inutil", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> AtualizarNotaFiscalnutil(long id, NotaFiscalnutilViewModel nf)
        {
            var itemContent = ObterConteudo(nf);

            var response = await _httpClient.PutAsync($"/api/v1/nota-fiscal/inutil/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> EnviarSefazNotaFiscalnutil(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/nota-fiscal/inutil/enviar/{id}");

            TratarErrosResponse(response);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<NotaFiscalnutilViewModel> ObterContaPagarPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/nota-fiscal/inutil/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<NotaFiscalnutilViewModel>(response); 
        }

        public async Task<PagedViewModel<NotaFiscalnutilViewModel>> ObterPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/nota-fiscal/inutil/obter-por-descricao?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<NotaFiscalnutilViewModel>>(response);
        }

        public async Task<ResponseResult> RemoverNotaFiscalnutil(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/nota-fiscal/inutil/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk(); 
        }
        #endregion
    }
}

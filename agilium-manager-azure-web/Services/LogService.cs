using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.LogSistemaViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class LogService : Service, ILogService
    {
        public LogService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        public async Task<PagedViewModel<LogSistemaViewModel>> ObterPaginacaoPorData(string dtInicial, string dtFinal, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/log/obter-por-data?dtInicial={dtInicial}&dtFinal={dtFinal}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<LogSistemaViewModel>>(response);
        }
    }
}

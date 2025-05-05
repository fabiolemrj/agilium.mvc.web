using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels.ImpostoViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class TabelaAuxiliarFiscalService : Service, ITabelaAuxiliarFiscalService
    {
        public TabelaAuxiliarFiscalService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        public async Task<TabelasAxuliaresFiscalViewModel> ObterTabelasAuxiliaresFiscal()
        {
            var response = await _httpClient.GetAsync($"/api/v1/tabela-auxiliar-fiscal/obter-todas");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<TabelasAxuliaresFiscalViewModel>(response);
        }
    }
}

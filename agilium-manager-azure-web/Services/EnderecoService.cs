
using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels.Endereco;
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
    public class EnderecoService : Service, IEnderecoService
    {
        public EnderecoService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        public async Task<CepViewModel> ObterCepPorNumeroCep(string cep)
        {
            var response = await _httpClient.GetAsync($"/api/v1/endereco/buscar-cep/{cep}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<CepViewModel>(response);
        }
    }
}

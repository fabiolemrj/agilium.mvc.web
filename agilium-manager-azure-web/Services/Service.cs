using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public abstract class Service
    {
        protected readonly HttpClient _httpClient;
        protected readonly IAspNetUser _user;
        protected readonly IConfiguration _configuration;
        public Service(HttpClient httpClient,
                        IOptions<AppSettings> settings,
                        IAspNetUser user,
                        IAuthenticationService authenticationService,
                        IConfiguration configuration)
        {
            _configuration = configuration;
            var autenticacaoUrl = configuration.GetSection("AppSettings").GetSection("AutenticacaoUrl").Value; 

            httpClient.BaseAddress = new Uri(autenticacaoUrl);
            _httpClient = httpClient;
            _user = user;

        }

        protected StringContent ObterConteudo(object dado)
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true,

            };
            return new StringContent(
                JsonSerializer.Serialize(dado,options),
                Encoding.UTF8,
                "application/json");
        }

    

        protected StringContent ObterConteudoImagem(object dado)
        {
           
            return new StringContent(
                JsonSerializer.Serialize(dado),
                Encoding.UTF8,
                "multipart/form-data");
        }


        protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        protected bool TratarErrosResponse(HttpResponseMessage response)
        {
            switch ((int)response.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(response.StatusCode);

                case 400:
                    return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }

        protected ResponseResult RetornoOk(object data = null)
        {
            var objeto = new ResponseResult();
            objeto.Status = (int)HttpStatusCode.OK;
            if (data != null)
                objeto.Data = data;
            return objeto;
        }

        protected ResponseResult RetornoBadRequest(object data = null)
        {
            var objeto = new ResponseResult();
            objeto.Status = (int)HttpStatusCode.BadRequest;
            if (data != null)
                objeto.Data = data;
            return objeto;
        }
    }
}

using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.Contato;
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
    public class ContatoService : Service, IContatoService
    {
        public ContatoService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        public async Task<ContatoEmpresaViewModel> ObterPorId(long idContato, long idEmpresa)
        {
            var response = await _httpClient.GetAsync($"/api/v1/contato/obter-por-id?idContato={idContato}&idEmpresa={idEmpresa}");
        
            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ContatoEmpresaViewModel>(response);
        }

        public async Task<ResponseResult> Adicionar(ContatoEmpresaViewModel contatoEmpresa)
        {
            var itemContent = ObterConteudo(contatoEmpresa);

            var response = await _httpClient.PostAsync($"/api/v1/contato/contato-empresa/", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(ContatoEmpresaViewModel contatoEmpresa)
        {
            var itemContent = ObterConteudo(contatoEmpresa);

            var response = await _httpClient.PutAsync($"/api/v1/contato/contato-empresa/", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> RemoverContatoEmpresa(long idContato, long idEmpresa)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/contato/contato-empresa?idContato={idContato}&idEmpresa={idEmpresa}");

            TratarErrosResponse(response);

            return RetornoOk();
        }
    }
}

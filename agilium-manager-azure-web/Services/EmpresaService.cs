

using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class EmpresaService : Service, IEmpresaService
    {
        public EmpresaService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        public async Task<ResponseResult> Adicionar(EmpresaCreateViewModel empresa)
        {
            var itemContent = ObterConteudo(empresa);

            var response = await _httpClient.PostAsync($"/api/v1/empresa/", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, EmpresaCreateViewModel empresa)
        {
            var itemContent = ObterConteudo(empresa);

            var response = await _httpClient.PutAsync($"/api/v1/empresa/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<PagedViewModel<EmpresaViewModel>> ObterEmpresasPorRazaoSocial(string nome, int page = 1, int pageSize = 15)
        {
                                                
            var response = await _httpClient.GetAsync($"/api/v1/empresa/obter-por-razaosocial?q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<EmpresaViewModel>>(response); 
        }

        public async Task<EmpresaCreateViewModel> ObterPorId(string id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/empresa/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<EmpresaCreateViewModel>(response);
        }

        public async Task<EmpresaCreateViewModel> ObterPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/empresa/obter-por-id/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<EmpresaCreateViewModel>(response);
        }

        public async Task<IEnumerable<EmpresaViewModel>> ObterTodas()
        {
            var response = await _httpClient.GetAsync($"/api/v1/empresa/obter-todas");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<EmpresaViewModel>>(response);
        }

        public async Task<ResponseResult> Remover(long id)
        {            
            var response = await _httpClient.DeleteAsync($"/api/v1/empresa/{ id.ToString()}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }
                
    }
}

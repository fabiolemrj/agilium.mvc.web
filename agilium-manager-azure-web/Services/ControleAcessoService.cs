
using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.CaManagerViewModel;
using agilium.webapp.manager.mvc.ViewModels.ControleAcesso;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class ControleAcessoService : Service, IControleAcessoService
    {
        public ControleAcessoService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        public async Task<ResponseResult> AdicionarModeloItem(CreateModeloViewModel caModelo)
        {
            var itemContent = ObterConteudo(caModelo);

            var response = await _httpClient.PostAsync($"/api/v1/ca/modelo", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> AdicionarPerfil(CreateEditPerfilViewModel perfil)
        {
            var itemContent = ObterConteudo(perfil);

            var response = await _httpClient.PostAsync($"/api/v1/ca/perfil", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> AdicionarPermissaoItem(CreateEditPermissaoItemViewModel caPermissaoItem)
        {
            var itemContent = ObterConteudo(caPermissaoItem);

            var response = await _httpClient.PostAsync($"/api/v1/ca/permissao", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> ApagarModeloItem(long idModelo)
        {
            throw new System.NotImplementedException();
        }

        public Task<ResponseResult> ApagarModelosPorPerfil(long idModelo)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ResponseResult> ApagarPerfil(long idPerfil)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/ca/perfil/{idPerfil}");

            TratarErrosResponse(response);

            return RetornoOk();
        }

        public Task<ResponseResult> ApagarPermissaoItem(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ResponseResult> AtivarPerfil(long idPerfil)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ResponseResult> AtualizarPerfil(CreateEditPerfilViewModel perfil)
        {
            var itemContent = ObterConteudo(perfil);

            var response = await _httpClient.PutAsync($"/api/v1/ca/perfil/{perfil.Id}", itemContent);

            TratarErrosResponse(response);

            return RetornoOk();
        }

        public async Task<CreateModeloViewModel> ObterModelosPorPerfil(long idPerfil)
        {
           
          var response = await _httpClient.GetAsync($"/api/v1/ca/obter-perfil-completo-por-id/{idPerfil}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<CreateModeloViewModel>(response);
        }

        public Task<PerfilIndexViewModel> ObterPerfilPorDescricao(string descricao)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PerfilIndexViewModel> ObterPerfilPorId(long idPerfil)
        {
            var response = await _httpClient.GetAsync($"/api/v1/ca/obter-perfil-por-id/{idPerfil}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PerfilIndexViewModel>(response);
        }

        public async Task<PagedViewModel<PermissaoItemIndexViewModel>> ObterPermissaoItemPorDescricao(string descricao, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/ca/permissao?q={descricao}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<PermissaoItemIndexViewModel>>(response);
        }

        public Task<IEnumerable<PermissaoItemIndexViewModel>> ObterTodosListaPermissao()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<PerfilIndexViewModel>> ObterTodosPerfis()
        {
            throw new System.NotImplementedException();
        }


        public Task<IEnumerable<CreateModeloItemViewModel>> ObterUsuariosPorPerfil(long idPerfil)
        {
            throw new System.NotImplementedException();
        }    

        public async Task<PagedViewModel<PerfilIndexViewModel>> ObterPerfilPorDescricao(long idEmpresa, string descricao, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/ca/perfil?idEmpresa={idEmpresa}&q={descricao}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<PerfilIndexViewModel>>(response);
        }

        #region Ca Manager
        public async Task<IEnumerable<CaAreaManagerViewModel>> ObterAreasTodas()
        {
            var response = await _httpClient.GetAsync($"/api/v1/ca/areas");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<IEnumerable<CaAreaManagerViewModel>>(response);
        }

        public async Task<PagedViewModel<CaPerfilManagerViewModel>> ObterPerfilManagerPorDescricao(string descricao, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/ca/perfil-por-descricao?q={descricao}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<CaPerfilManagerViewModel>>(response);
        }

        public async Task<ResponseResult> AdicionarPerfil(CaPerfilManagerViewModel perfil)
        {
            var itemContent = ObterConteudo(perfil);

            var response = await _httpClient.PostAsync($"/api/v1/ca/perfil-manager", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> AtualizarPerfil(CaPerfilManagerViewModel perfil)
        {
            var itemContent = ObterConteudo(perfil);

            var response = await _httpClient.PutAsync($"/api/v1/ca/perfil-manager/{perfil.IdPerfil}", itemContent);

            TratarErrosResponse(response);

            return RetornoOk();
        }

        public async Task<CaPerfilManagerViewModel> ObterPerfilManagerPorId(long idPerfil)
        {
            var response = await _httpClient.GetAsync($"/api/v1/ca/perfil-manager/{idPerfil}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<CaPerfilManagerViewModel>(response);
        }

        public async Task<CaPermissaoPerfilViewModel> ObterPermissaoPorPerfil(long idPerfil)
        {
            var response = await _httpClient.GetAsync($"/api/v1/ca/perfil-manager/permissoes/{idPerfil}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<CaPermissaoPerfilViewModel>(response);
        }

        public async Task<ResponseResult> AdicionarPermissoes(List<CaAreaManagerSalvarViewModel> permissoes)
        {
            var itemContent = ObterConteudo(permissoes);

            var response = await _httpClient.PostAsync($"/api/v1/ca/permissoes-manager", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<List<CaPerfilManagerViewModel>> ObterTodosPerfilManager()
        {
            var response = await _httpClient.GetAsync($"/api/v1/ca/perfil-manager/todos");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<CaPerfilManagerViewModel>>(response);
        }
        #endregion
    }
}

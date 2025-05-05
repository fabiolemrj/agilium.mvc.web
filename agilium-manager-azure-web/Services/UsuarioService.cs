using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.Models;
using agilium.webapp.manager.mvc.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver.Core.WireProtocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class UsuarioService : Service, IUsuarioService
    {
        public UsuarioService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        public async Task<ViewModels.ResponseResult> Atualizar(string id, UserFull usuario)
        {
            var itemContent = ObterConteudo(usuario);
         
            var response = await _httpClient.PutAsync($"/api/v1/usuarios/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ViewModels.ResponseResult> AtualizarClaimUsuario(string id, List<ClaimSelecionada> claimSelecionadas)
        {
            var itemContent = ObterConteudo(claimSelecionadas);

            var response = await _httpClient.PutAsync($"/api/v1/atualizar-usuario-claim/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ViewModels.ResponseResult> DuplicarClaimPorUsuario(DuplicarUsuarioRetornoViewModel viewModel)
        {
            var itemContent = ObterConteudo(viewModel);

            var response = await _httpClient.PostAsync($"/api/v1/duplicar-claim-usuario", itemContent);

            if (!TratarErrosResponse(response)) 
                return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ViewModels.ResponseResult> ObterClaimsPorUsuario(string id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/obter-claim-por-usuario/{id}");

            var listaRetorno = new List<ClaimSelecionada>();
            if (!TratarErrosResponse(response))
                return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            listaRetorno = await DeserializarObjetoResponse<List<ClaimSelecionada>>(response);

            return RetornoOk(listaRetorno);
        }

        public async Task<UserFull> ObterPorUsuarioPorId(string id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/usuarios/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<UserFull>(response);
        }

        public async Task<UserFull> ObterPorUsuarioPorUserId(string id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/usuarios/obter-por-userId/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<UserFull>(response);
        }

        public async Task<ViewModels.ResponseResult> ObterUsuarioComClaimsPorId(string id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/obter-usuario-e-claims-por-usuarioid/{id}");

            if (!TratarErrosResponse(response))
                return RetornoBadRequest(response);

            var Retorno = await DeserializarObjetoResponse<UsuarioClaimsViewModel>(response);

            return RetornoOk(Retorno);
        }

        public async Task<List<UserFull>> ObterUsuarios()
        {
            var response = await _httpClient.GetAsync($"/api/v1/usuarios/obter-todos");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<UserFull>>(response);
        }

        public async Task<List<UserFull>> ObterUsuarios(string nome)
        {
            var response = await _httpClient.GetAsync($"/api/v1/usuarios/obter-por-nome/{nome}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<UserFull>>(response);
        }

        public async Task<bool> Remover(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ViewModels.ResponseResult> RemoverClaimInvidualPorUsuario(ClaimEditaAcaoIndividualPorUsuario viewModel)
        {
            var itemContent = ObterConteudo(viewModel);

            var response = await _httpClient.PostAsync($"/api/v1/remover-claim-por-usuario/{viewModel.IdUserAspNet}", itemContent);

            if (!TratarErrosResponse(response))
                return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<PagedViewModel<UserFull>> ObterTodosUsuarios(int page, int pageSize)
        {
            var response = await _httpClient.GetAsync($"/api/v1/usuarios/obter-todos-paginacao?page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<UserFull>>(response);
        }

        public async Task<PagedViewModel<UserFull>> ObterUsuariosPorNome(string nome, int page, int pageSize)
        {
            var response = await _httpClient.GetAsync($"/api/v1/usuarios/obter-por-nome-pag/{nome}?page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<UserFull>>(response);
        }

        public async Task<ViewModels.ResponseResult> DesativarUsuario(string id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/desativar-usuario/{id}");

            if (!TratarErrosResponse(response))
                return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            var resultado = await DeserializarObjetoResponse<string>(response);

            return RetornoOk(resultado);
        }

        public async Task<ViewModels.ResponseResult> AtivarUsuario(string id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/ativar-usuario/{id}");

            if (!TratarErrosResponse(response))
                return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            var resultado = await DeserializarObjetoResponse<string>(response);

            return RetornoOk(resultado);
        }

        public async Task<ViewModels.ResponseResult> AtualizarFoto(UsuarioFotoViewModel usuarioFoto)
        {
            var itemContent = ObterConteudo(usuarioFoto);

            var response = await _httpClient.PostAsync($"/api/v1/usuarios/atualizar-foto-usuario", itemContent);

            if (!TratarErrosResponse(response))
                return RetornoBadRequest();

            return RetornoOk();
        }

        public async Task<ViewModels.ResponseResult> ObterFotoUsuarioPorId(string id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/usuarios/obter-foto-usuario-por-id/{id}");

            if (!TratarErrosResponse(response))
                return RetornoBadRequest();

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return RetornoOk();
            }

            var Retorno = await DeserializarObjetoResponse<UsuarioFotoViewModel>(response);

            return RetornoOk(Retorno);
        }

        public async Task<ViewModels.ResponseResult> ObterEmpresasPorUsuario(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/usuarios/obter-empresas-por-usuario/{id}");

            if (!TratarErrosResponse(response))
                return RetornoBadRequest();

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return RetornoOk();
            }

            var Retorno = await DeserializarObjetoResponse<IEnumerable<EmpresaUsuarioViewModel>>(response);

            return RetornoOk(Retorno);
        }

        public async Task<ViewModels.ResponseResult> ObterEmpresaPorId(long idUsuario, long idEmpresa)
        {
            var response = await _httpClient.GetAsync($"/api/v1/usuarios/obter-empresa-usuario-por-id/{idUsuario}/{idEmpresa}");

            if (!TratarErrosResponse(response))
                return RetornoBadRequest();

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return RetornoOk();
            }

            var Retorno = await DeserializarObjetoResponse<EmpresaUsuarioViewModel>(response);

            return RetornoOk(Retorno);
        }

        public async Task<ViewModels.ResponseResult> Adicionar(EmpresaUsuarioViewModel empresaAuth)
        {
            var itemContent = ObterConteudo(empresaAuth);

            var response = await _httpClient.PostAsync($"/api/v1/usuarios/empresa-usuario", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<bool> Remover(long idUsuario, long idEmpresa)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/usuarios/empresa-usuario/{idUsuario}/{idEmpresa}");

            if (!TratarErrosResponse(response))
                return false;

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return false;
            }

            var Retorno = await DeserializarObjetoResponse<EmpresaUsuarioViewModel>(response);

            return true;
        }

        public async Task<ViewModels.ResponseResult> ObterEmpresasDispiniveisPorUsuario(long idUsuario)
        {
            var response = await _httpClient.GetAsync($"/api/v1/usuarios/obter-empresas-disponiveis/{idUsuario}");

            if (!TratarErrosResponse(response))
                return RetornoBadRequest();

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return RetornoOk();
            }

            var Retorno = await DeserializarObjetoResponse<EmpresasAssociadasViewModel>(response);

            return RetornoOk(Retorno);
        }

        public async Task<ViewModels.ResponseResult> Adicionar(long idUsuario, List<EmpresaUsuarioSelecaoViewModel> empresaAuth)
        {
            var itemContent = ObterConteudo(empresaAuth);

            var response = await _httpClient.PostAsync($"/api/v1/usuarios/empresas-usuarios?idUsuario={idUsuario}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<SelecionarPerfilViewModel> ObterPorUsuarioPerfis(string id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/usuarios/obter-usuario-perfis/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<SelecionarPerfilViewModel>(response);
        }

        
        public async Task<ViewModels.ResponseResult> SelecionarPerfil(SelecionarPerfilViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/usuarios/selecionar-perfil", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            var Retorno = response.Content.ReadAsStringAsync();

            return RetornoOk(Retorno.Result);
        }
    }
}

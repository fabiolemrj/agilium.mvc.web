using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.Funcionario;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class FuncionarioService : Service, IFuncionarioService
    {
        public FuncionarioService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, 
            IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        public async Task<ResponseResult> Adicionar(FuncionarioViewModel fornecedorViewModel)
        {
            var itemContent = ObterConteudo(fornecedorViewModel);

            var response = await _httpClient.PostAsync($"/api/v1/funcionario", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, FuncionarioViewModel fornecedorViewModel)
        {
            var itemContent = ObterConteudo(fornecedorViewModel);

            string jsonString = JsonSerializer.Serialize(fornecedorViewModel);
            var response = await _httpClient.PutAsync($"/api/v1/funcionario/{fornecedorViewModel.Id}", itemContent);


            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<FuncionarioViewModel> ObterListasAuxiliares()
        {
            var response = await _httpClient.GetAsync($"/api/v1/funcionario/listas-auxiliares");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<FuncionarioViewModel>(response); ;
        }

        public async Task<FuncionarioViewModel> ObterPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/funcionario/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<FuncionarioViewModel>(response); ;
        }

        public async Task<PagedViewModel<FuncionarioViewModel>> ObterPorNome(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/funcionario/obter-por-nome?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<FuncionarioViewModel>>(response);
        }

        public Task<IEnumerable<FuncionarioViewModel>> ObterTodas()
        {
            throw new System.NotImplementedException();
        }

        public async Task<ResponseResult> Remover(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/funcionario/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }


    }
}

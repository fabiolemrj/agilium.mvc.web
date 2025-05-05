using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.Contato;
using agilium.webapp.manager.mvc.ViewModels.Fornecedor;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class FornecedorService : Service, IFornecedorService
    {

        public FornecedorService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, 
                                IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        #region fornecedor
        public async Task<ResponseResult> Adicionar(FornecedorViewModel fornecedorViewModel)
        {
            var itemContent = ObterConteudo(fornecedorViewModel);

            var response = await _httpClient.PostAsync($"/api/v1/fornecedor/", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, FornecedorViewModel fornecedorViewModel)
        {
            var itemContent = ObterConteudo(fornecedorViewModel);

            var response = await _httpClient.PutAsync($"/api/v1/fornecedor/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<PagedViewModel<FornecedorViewModel>> ObterPorRazaoSocial(string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/fornecedor/obter-por-razaosocial?q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<FornecedorViewModel>>(response);
        }

        public async Task<FornecedorViewModel> ObterPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/fornecedor/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<FornecedorViewModel>(response);
        }

        public async Task<IEnumerable<FornecedorViewModel>> ObterTodas()
        {
            var response = await _httpClient.GetAsync($"/api/v1/fornecedor/obter-todos");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<IEnumerable<FornecedorViewModel>>(response);
        }

        public async Task<ResponseResult> Remover(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/fornecedor/{id.ToString()}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }
        #endregion

        #region contato
        public async Task<ContatoFornecedorViewModel> ObterPorId(long idContato, long idFornecedor)
        {
            var response = await _httpClient.GetAsync($"/api/v1/fornecedor/contato/obter-por-id?idContato={idContato}&idFornecedor={idFornecedor}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ContatoFornecedorViewModel>(response);
        }

        public async Task<ResponseResult> Adicionar(ContatoFornecedorViewModel contato)
        {
            var itemContent = ObterConteudo(contato);

            var response = await _httpClient.PostAsync($"/api/v1/fornecedor/contato/", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(ContatoFornecedorViewModel contato)
        {
            var itemContent = ObterConteudo(contato);

            var response = await _httpClient.PutAsync($"/api/v1/fornecedor/contato/", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> RemoverContato(long idContato, long idFornecedor)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/fornecedor/contato?idContato={idContato}&idFornecedor={idFornecedor}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }
        #endregion
    }
}

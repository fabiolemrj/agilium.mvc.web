using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.Cliente;
using agilium.webapp.manager.mvc.ViewModels.Contato;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class ClienteService : Service, IClienteService
    {

        public ClienteService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        #region Cliente
        public async Task<ResponseResult> Adicionar(ClienteViewModel clienteViewModel)
        {
            var itemContent = ObterConteudo(clienteViewModel);

            var response = await _httpClient.PostAsync($"/api/v1/cliente/", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, ClienteViewModel clienteViewModel)
        {
            var json = JsonSerializer.Serialize(clienteViewModel);

            var itemContent = ObterConteudo(clienteViewModel);

            var response = await _httpClient.PutAsync($"/api/v1/cliente/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ClienteViewModel> ObterPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/cliente/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ClienteViewModel>(response);
        }

        public async Task<PagedViewModel<ClienteViewModel>> ObterPorNome(string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/cliente/obter-por-nome?q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<ClienteViewModel>>(response);
        }

        public async Task<IEnumerable<ClienteViewModel>> ObterTodas()
        {
            var response = await _httpClient.GetAsync($"/api/v1/cliente/todos");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<IEnumerable<ClienteViewModel>>(response);
        }

        public async Task<ResponseResult> Remover(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/cliente/{id.ToString()}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }
        #endregion

        #region Contato
        public async Task<ResponseResult> Adicionar(ClienteContatoViewModel contato)
        {
            var itemContent = ObterConteudo(contato);

            var response = await _httpClient.PostAsync($"/api/v1/cliente/contato/", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(ClienteContatoViewModel contato)
        {
            var itemContent = ObterConteudo(contato);

            var response = await _httpClient.PutAsync($"/api/v1/cliente/contato/", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ClienteContatoViewModel> ObterPorId(long idContato, long idCliente)
        {
            var response = await _httpClient.GetAsync($"/api/v1/cliente/contato/obter-por-id?idContato={idContato}&idCliente={idCliente}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ClienteContatoViewModel>(response);
        }

        public async Task<ResponseResult> RemoverContato(long idContato, long idCliente)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/cliente/contato?idContato={idContato}&idCliente={idCliente}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }


        #endregion

        #region Cliente Preco

        public async Task<ResponseResult> Adicionar(ClientePrecoViewModel viewModel)
        {
            var itemContent = ObterConteudo(viewModel);

            var response = await _httpClient.PostAsync($"/api/v1/cliente/preco/", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ClientePrecoViewModel> ObterClientePrecoPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/cliente/preco/obter-por-id/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ClientePrecoViewModel>(response);
        }

        public async Task<ResponseResult> RemoverPreco(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/cliente/preco/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<List<ClientePrecoViewModel>> ObterClientePrecoPorProduto(long idProduto)
        {
            var response = await _httpClient.GetAsync($"/api/v1/cliente/precos/obter-por-idproduto/{idProduto}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<ClientePrecoViewModel>>(response);
        }
        #endregion
    }
}

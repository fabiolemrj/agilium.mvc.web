using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.InventarioViewModel;
using agilium.webapp.manager.mvc.ViewModels.ProdutoViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class InventarioService : Service, IInventarioService
    {
        public InventarioService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        #region Inventario
        public async Task<ResponseResult> Adicionar(InventarioViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/inventario", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Apagar(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/inventario/cancelar/{id.ToString()}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, InventarioViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PutAsync($"/api/v1/inventario/{model.Id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }


        public async Task<PagedViewModel<InventarioViewModel>> ObterPaginacaoPorDescricao(long idEmpresa, string descricao, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/inventario/obter-por-descricao?idEmpresa={idEmpresa}&page={page}&ps={pageSize}&q={descricao}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<InventarioViewModel>>(response);
        }

        public async Task<InventarioViewModel> ObterPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/inventario/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<InventarioViewModel>(response);
        }

        public async Task<ResponseResult> CadastrarProdutoPorEstoque(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/inventario/cadastrar-produtos-por-estoque/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Inventariar(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/inventario/inventariar/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<List<ProdutoViewModel>> ObterProdutoDisponivelInventario(long idEmpresa, long idInventario)
        {
            var response = await _httpClient.GetAsync($"/api/v1/inventario/obter-produtos-inventario/{idEmpresa}/{idInventario}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<ProdutoViewModel>>(response);
        }
        #endregion

        #region Item

        public async Task<InventarioItemViewModel> ObterItemPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/inventario/item/obter-por-id/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<InventarioItemViewModel>(response);
        }

        public async Task<ResponseResult> Adicionar(InventarioItemViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/inventario/item", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> ApagarItem(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/inventario/item/{id.ToString()}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, InventarioItemViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PutAsync($"/api/v1/inventario/item/{model.Id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<List<InventarioItemViewModel>> ObterItemPorIdInventario(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/inventario/itens/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<InventarioItemViewModel>>(response);
        }

        public async Task<ResponseResult> IncluirProdutosInventario(AdicionarListaProdutosDisponiveisViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/inventario/incluir-produtos-inventario", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk(); ;
        }

        public async Task<ResponseResult> Apurar(List<InventarioItemViewModel> model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/inventario/apuracao", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk(); ;
        }

        public async Task<ResponseResult> ApagarListaItem(List<InventarioItemViewModel> model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/inventario/item/apagar", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk(); ;
        }

        public async Task<ResponseResult> Concluir(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/inventario/concluir/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk(); 
        }

        #endregion

    }
}

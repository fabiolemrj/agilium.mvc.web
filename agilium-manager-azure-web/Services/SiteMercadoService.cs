using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.SiteMercadoViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class SiteMercadoService : Service, ISiteMercadoService
    {
        public SiteMercadoService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        #region Produto
        public async Task<ResponseResult> Adicionar(ProdutoSiteMercadoViewModel produtoViewModel)
        {
            var itemContent = ObterConteudo(produtoViewModel);

            var response = await _httpClient.PostAsync($"/api/v1/sm/produto", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }
        public async Task<ResponseResult> Atualizar(long id, ProdutoSiteMercadoViewModel produtoViewModel)
        {
            var itemContent = ObterConteudo(produtoViewModel);

            var response = await _httpClient.PutAsync($"/api/v1/sm/produto/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<PagedViewModel<ProdutoSiteMercadoViewModel>> ObterPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/sm/produto/obter-por-descricao?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<ProdutoSiteMercadoViewModel>>(response);
        }

        public async Task<ProdutoSiteMercadoViewModel> ObterProdutoPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/sm/produto/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ProdutoSiteMercadoViewModel>(response);
        }

        public async Task<ResponseResult> Remover(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/sm/produto/{id.ToString()}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }
        #endregion

        #region Moeda

        public async Task<ResponseResult> Adicionar(MoedaSiteMercadoViewModel produtoViewModel)
        {
            var itemContent = ObterConteudo(produtoViewModel);

            var response = await _httpClient.PostAsync($"/api/v1/sm/moeda", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, MoedaSiteMercadoViewModel produtoViewModel)
        {
            var itemContent = ObterConteudo(produtoViewModel);

            var response = await _httpClient.PutAsync($"/api/v1/sm/moeda/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<MoedaSiteMercadoViewModel> ObterMoedaPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/sm/moeda/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<MoedaSiteMercadoViewModel>(response);
        }

        public async Task<PagedViewModel<MoedaSiteMercadoViewModel>> ObterPaginacaoMoedaPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/sm/moeda/obter-por-descricao?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<MoedaSiteMercadoViewModel>>(response);
        }

        public async Task<ResponseResult> RemoverMoeda(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/sm/moeda/{id.ToString()}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }
        #endregion


    }
}

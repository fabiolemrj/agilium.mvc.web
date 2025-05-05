using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.EstoqueViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class EstoqueService : Service, IEstoqueService
    {
        public EstoqueService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, 
                        IAuthenticationService authenticationService, IConfiguration configuration) 
                        : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        #region Estoque
        public async Task<ResponseResult> Adicionar(EstoqueViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/estoque", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, EstoqueViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PutAsync($"/api/v1/estoque/{model.Id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<PagedViewModel<EstoqueViewModel>> ObterPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/estoque/obter-por-descricao?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<EstoqueViewModel>>(response);
        }

        public async Task<EstoqueViewModel> ObterPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/estoque/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<EstoqueViewModel>(response);
        }

       
        public async Task<IEnumerable<EstoqueViewModel>> ObterTodas()
        {
            var response = await _httpClient.GetAsync($"/api/v1/estoque/obter-todas");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<EstoqueViewModel>>(response);
        }

        public async Task<ResponseResult> Remover(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/estoque/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }
        #endregion

        #region Produto Estoque
        public async Task<IEnumerable<EstoqueProdutoListaViewModel>> ObterProdutoEstoquePorIdProduto(long idProduto)
        {
            var response = await _httpClient.GetAsync($"/api/v1/estoque/produto/{idProduto}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<EstoqueProdutoListaViewModel>>(response);
        }

        public async Task<IEnumerable<ProdutoPorEstoqueViewModel>> ObterProdutoEstoquePorIdEstoque(long idEstoque)
        {
            var response = await _httpClient.GetAsync($"/api/v1/estoque/produto/obter-por-idestoque/{idEstoque}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<ProdutoPorEstoqueViewModel>>(response); ;
        }


        #endregion

        #region Estoque Historico
        public async Task<IEnumerable<EstoqueHistoricoViewModel>> ObterHistoricoEstoquePorIdProduto(long idProduto)
        {
            var response = await _httpClient.GetAsync($"/api/v1/estoque/historico/produtos/{idProduto}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<EstoqueHistoricoViewModel>>(response);
        }
        #endregion

        #region Report
      
        public async Task<List<EstoquePosicaoReport>> ObterRelatorioPosicaoEstoque(long idEstoque)
        {
            var response = await _httpClient.GetAsync($"/api/v1/estoque/report/posicao/{idEstoque}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<EstoquePosicaoReport>>(response);
        }
        #endregion

    }
}

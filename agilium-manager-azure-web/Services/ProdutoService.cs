
using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.ProdutoViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class ProdutoService : Service, IProdutoService
    {
        public ProdutoService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        #region Produto

        public async Task<PagedViewModel<ProdutoViewModel>> ObterPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/obter-por-descricao?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<ProdutoViewModel>>(response);
        }

        public async Task<ResponseResult> Atualizar(long id, ProdutoViewModel produtoViewModel)
        {
            var itemContent = ObterConteudo(produtoViewModel);

            var response = await _httpClient.PutAsync($"/api/v1/produto/{id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Adicionar(ProdutoViewModel produtoViewModel)
        {
            var itemContent = ObterConteudo(produtoViewModel);

            var response = await _httpClient.PostAsync($"/api/v1/produto", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ProdutoViewModel> ObterProdutoPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ProdutoViewModel>(response);
        }

        public async Task<IEnumerable<ProdutoViewModel>> ObterTodas(long idEmpresa)
        {            
            var response = await _httpClient.GetAsync($"/api/v1/produto/obter-produtos-por-idempresa/{idEmpresa}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<ProdutoViewModel>>(response);
        }

        public async Task<ListasAuxiliaresProdutoViewModel> ObterListasAuxiliares()
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/obter-listas-auxiliares");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ListasAuxiliaresProdutoViewModel>(response); ;
        }
        public async Task<ResponseResult> Remover(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/produto/{id.ToString()}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        #endregion

        #region ProdutoDepartamento
        public async Task<ResponseResult> Adicionar(ProdutoDepartamentoViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/produto/departamento", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }


        public async Task<ResponseResult> Atualizar(long id, ProdutoDepartamentoViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PutAsync($"/api/v1/produto/departamento/{model.Id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }


        public async Task<PagedViewModel<ProdutoDepartamentoViewModel>> ObterDepartamentoPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/departamentos?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<ProdutoDepartamentoViewModel>>(response);
        }

        public async Task<ProdutoDepartamentoViewModel> ObterDepartamentoPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/obter-departamento-por-id/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ProdutoDepartamentoViewModel>(response);
        }

        public async Task<ResponseResult> RemoverDepartamento(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/produto/departamento/{id}");

            TratarErrosResponse(response);

            return RetornoOk();
        }


        #endregion

        #region ProdutoMarca
        public async Task<ResponseResult> Adicionar(ProdutoMarcaViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/produto/marca", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> RemoverMarca(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/produto/marca/{id}");

            TratarErrosResponse(response);

            return RetornoOk();
        }

        public async Task<ProdutoMarcaViewModel> ObterMarcaPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/obter-marca-por-id/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ProdutoMarcaViewModel>(response);
        }

        public async Task<ResponseResult> Atualizar(long id, ProdutoMarcaViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PutAsync($"/api/v1/produto/marca/{model.Id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<PagedViewModel<ProdutoMarcaViewModel>> ObterMarcaPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/marcas?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<ProdutoMarcaViewModel>>(response);
        }

        #endregion

        #region Grupo

        public async Task<PagedViewModel<GrupoProdutoViewModel>> ObterGrupoPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/grupos?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<GrupoProdutoViewModel>>(response);
        }

        public async Task<ResponseResult> Atualizar(long id, GrupoProdutoViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PutAsync($"/api/v1/produto/grupo/{model.Id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Adicionar(GrupoProdutoViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/produto/grupo", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<GrupoProdutoViewModel> ObterGrupoPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/obter-grupo-por-id/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<GrupoProdutoViewModel>(response);
        }

        public async Task<ResponseResult> RemoverGrupo(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/produto/grupo/{id}");

            TratarErrosResponse(response);

            return RetornoOk();
        }

        #endregion

        #region SubGrupo

        public async Task<PagedViewModel<SubGrupoViewModel>> ObterSubGrupoPaginacaoPorDescricao(long idGrupo, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/subgrupos?idGrupo={idGrupo}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<SubGrupoViewModel>>(response);
        }

        public async Task<ResponseResult> Atualizar(long id, SubGrupoViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PutAsync($"/api/v1/produto/subgrupo/{model.Id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Adicionar(SubGrupoViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/produto/subgrupo", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<SubGrupoViewModel> ObterSubGrupoPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/obter-subgrupo-por-id/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<SubGrupoViewModel>(response);
        }

        public async Task<ResponseResult> RemoverSubGrupo(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/produto/subgrupo/{id}");

            TratarErrosResponse(response);

            return RetornoOk();
        }
        #endregion

        #region Codigo de Barra

        public async Task<ResponseResult> Atualizar(long id, ProdutoCodigoBarraViewModel produtoViewModel)
        {
            var itemContent = ObterConteudo(produtoViewModel);

            var response = await _httpClient.PutAsync($"/api/v1/produto/codigo-barra/{produtoViewModel.Id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Adicionar(ProdutoCodigoBarraViewModel produtoViewModel)
        {
            var itemContent = ObterConteudo(produtoViewModel);

            var response = await _httpClient.PostAsync($"/api/v1/produto/codigo-barra", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ProdutoCodigoBarraViewModel> ObterProdutoCodigoBarraPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/codigo-barra/obter-por-id/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ProdutoCodigoBarraViewModel>(response);
        }

        public async Task<ResponseResult> RemoverCodigoBarra(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/produto/codigo-barra/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<List<ProdutoCodigoBarraViewModel>> ObterCodigoBarraPorProduto(long idProduto)
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/codigos-barra/{idProduto}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<ProdutoCodigoBarraViewModel>>(response);
        }

        #endregion

        #region ProdutoPreco

        public async Task<ResponseResult> Atualizar(long id, ProdutoPrecoViewModel produtoViewModel)
        {
            var itemContent = ObterConteudo(produtoViewModel);

            var response = await _httpClient.PutAsync($"/api/v1/produto/preco/{produtoViewModel.Id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Adicionar(ProdutoPrecoViewModel produtoViewModel)
        {
            var itemContent = ObterConteudo(produtoViewModel);

            var response = await _httpClient.PostAsync($"/api/v1/produto/preco", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ProdutoPrecoViewModel> ObterProdutoPrecoPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/preco/obter-por-id/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ProdutoPrecoViewModel>(response);
        }

        public async Task<ResponseResult> RemoverPreco(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/produto/preco/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<List<ProdutoPrecoViewModel>> ObterPrecoPorProduto(long idProduto)
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/preco/{idProduto}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<ProdutoPrecoViewModel>>(response);
        }

        #endregion

        #region Produto Foto

        public async Task<ResponseResult> Atualizar(long id, ProdutoFotoViewModel produtoViewModel)
        {
            var itemContent = ObterConteudo(produtoViewModel);

            var response = await _httpClient.PutAsync($"/api/v1/produto/foto/{produtoViewModel.Id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Adicionar(ProdutoFotoViewModel produtoViewModel)
        {
            //var itemContent = ObterConteudoImagem(produtoViewModel);


            //var response = await _httpClient.PostAsync($"/api/v1/produto/foto", itemContent);

            byte[] data;
            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            if (produtoViewModel.Foto != null)
            {
                using (var br = new BinaryReader(produtoViewModel.Foto.OpenReadStream()))
                {
                    data = br.ReadBytes((int)produtoViewModel.Foto.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
    

                multiContent.Add(bytes, "Foto", produtoViewModel.Foto.FileName);
            }


            multiContent.Add(new StringContent(produtoViewModel.Id.ToString()), "Id");
            // multiContent.Add(new StringContent(JsonConvert.SerializeObject(produtoViewModel.idProduto.Value.ToString())), "idProduto");
            multiContent.Add(new StringContent(produtoViewModel.idProduto.Value.ToString()), "idProduto");
            multiContent.Add(new StringContent(produtoViewModel.Data.Value.ToString()), "Data");
            multiContent.Add(new StringContent(produtoViewModel.Descricao.ToString()), "Descricao");
            multiContent.Add(new StringContent(JsonConvert.SerializeObject(produtoViewModel.Descricao.ToString())), "produtoViewModel");

            //multiContent.Add(new StringContent(JsonConvert.SerializeObject(new { Descricao = produtoViewModel.Descricao, idProduto = produtoViewModel.idProduto,
            //Id = produtoViewModel.Id, Data = produtoViewModel.Data.Value})), "produtoViewModel");
    
            var response = await _httpClient.PostAsync($"/api/v1/produto/foto", multiContent);
            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ProdutoFotoViewModel> ObterProdutoFotoPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/foto/obter-por-id/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ProdutoFotoViewModel>(response);
        }

        public async Task<ResponseResult> RemoverFoto(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/produto/foto/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<List<ProdutoFotoViewModel>> ObterFotoPorProduto(long idProduto)
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/foto/{idProduto}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<ProdutoFotoViewModel>>(response);
        }
        #endregion

        #region IBPT
        public async Task<ResponseResult> AtualizarIBPT()
        {
            var response = await _httpClient.GetAsync($"/api/v1/produto/ibpt/atualizar");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }
        #endregion
    }
}

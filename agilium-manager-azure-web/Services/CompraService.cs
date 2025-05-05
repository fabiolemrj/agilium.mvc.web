using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.CompraViewModel;
using agilium.webapp.manager.mvc.ViewModels.CompraViewModel.ImportacaoXmlNFE;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SharpCompress.Common;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class CompraService : Service, ICompraService
    {
        public CompraService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        #region Compra
        public async Task<ResponseResult> Adicionar(CompraViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/compra", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, CompraViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PutAsync($"/api/v1/compra/{model.Id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }
        public async Task<ResponseResult> Cancelar(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/compra/cancelar/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Efetivar(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/compra/efetivar/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<CompraViewModel> ObterCompraPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/compra/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<CompraViewModel>(response); ;
        }

        public async Task<PagedViewModel<CompraViewModel>> ObterPaginacaoPorData(long idEmpresa, string dtInicial, string dtFinal, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/compra/obter-por-data?idEmpresa={idEmpresa}&dtInicial={dtInicial}&dtFinal={dtFinal}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<CompraViewModel>>(response);
        }
        #endregion

        #region Compra Item
        public async Task<ResponseResult> Adicionar(CompraItemViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/compra/item", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long id, CompraItemViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PutAsync($"/api/v1/compra/item/{model.Id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<CompraItemViewModel> ObterItemPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/compra/item/obter-por-id/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<CompraItemViewModel>(response); ;
        }

        public async Task<List<CompraItemViewModel>> ObterItemPorIdCompra(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/compra/itens/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<CompraItemViewModel>>(response); ;
        }

        public async Task<ResponseResult> ImportarArquivo(NFeProc model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/compra/item/importar", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> ImportarArquivo(ImportacaoArquivo model)
        {
            using var form = new MultipartFormDataContent();
            var itemContent = ObterConteudoImagem(model.XmlArquivo);

            form.Add(ObterConteudo(model.idCompra), "idCompra");
            byte[] data;
            if (model.XmlArquivo != null)
            {
                using (var br = new BinaryReader(model.XmlArquivo.OpenReadStream()))
                {
                    data = br.ReadBytes((int)model.XmlArquivo.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                form.Add(bytes, "XmlArquivo", model.XmlArquivo.FileName);
            }

            var response = await _httpClient.PostAsync($"/api/v1/compra/item/importar-xml", form);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk(await DeserializarObjetoResponse<NFeProc>(response)); 
        }

        public async Task<ResponseResult> ImportarArquivo(IFormFile formFile)
        {
            using var form = new MultipartFormDataContent();
            var itemContent = ObterConteudoImagem(formFile);
            itemContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            form.Add(itemContent, "XmlArquivo", formFile.FileName);
            

            var response = await _httpClient.PostAsync($"/api/v1/compra/item/importar-xml", form);
    
            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk(DeserializarObjetoResponse<ViewModels.ResponseResult>(response)); 
        }

        public async Task<ResponseResult> ImportarArquivoXmlNfe(ImportacaoArquivo model)
        {
            using var form = new MultipartFormDataContent();
            var itemContent = ObterConteudoImagem(model.XmlArquivo);
            //itemContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

            form.Add(ObterConteudo(model.idCompra), "idCompra");
            byte[] data;
            if (model.XmlArquivo != null)
            {
                using (var br = new BinaryReader(model.XmlArquivo.OpenReadStream()))
                {
                    data = br.ReadBytes((int)model.XmlArquivo.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                form.Add(bytes, "XmlArquivo", model.XmlArquivo.FileName);
            }

            var response = await _httpClient.PostAsync($"/api/v1/compra/item/importar-xml", form);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> AtualizarProdutoItemCompra(CompraItemEditViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PutAsync($"/api/v1/compra/item/produto/{model.Id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> CadastrarProdutosAutomaticamente(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/compra/cadastrar-produto-automaticamente/{id}");

            TratarErrosResponse(response);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }
        #endregion


    }
}

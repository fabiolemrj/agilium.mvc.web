

using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class ConfigService : Service, IConfigServices
    {
        public ConfigService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        public async Task<ResponseResult> Atualizar(long idEmpresa,IEnumerable<ChaveValorViewModel> contatoEmpresa)
        {
            var itemContent = ObterConteudo(contatoEmpresa);

            var response = await _httpClient.PutAsync($"/api/v1/config/{idEmpresa}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(long idEmpresa, ChaveValorViewModel chaveValorViewModel)
        {
            var itemContent = ObterConteudo(chaveValorViewModel);

            var response = await _httpClient.PutAsync($"/api/v1/config/atualiza-config/{idEmpresa}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Atualizar(ConfigImagemViewModel model)
        {
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(model.ImagemUpLoad.OpenReadStream());
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(model.ImagemUpLoad.ContentType);

            content.Add(fileContent, "arquivo", model.ImagemUpLoad.FileName);
            content.Add(ObterConteudo(model.IDEMPRESA), "idEmpresa");
            //content.Add(ObterConteudo(model.CHAVE), "chave");
            //content.Add(ObterConteudo(chaveValorViewModel.VALOR), "valor");
            var url = $"/api/v1/config/config-imagem/{model.CHAVE}";
            var response = await _httpClient.PutAsync(url, content);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> AtualizarCertificado(long idEmpresa, ChaveValorViewModel chaveValorViewModel)
        {
            var itemContent = ObterConteudo(chaveValorViewModel);

            var response = await _httpClient.PostAsync($"/api/v1/config/atualiza-certificado", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> AtualizarCertificado(long idEmpresa, IFormFile certificado)
        {
            var itemContent = ObterConteudo(certificado);

            var response = await _httpClient.PostAsync($"/api/v1/config/atualiza-certificado", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> AtualizarCertificado(long idEmpresa, ConfigIndexViewModel chaveValorViewModel)
        {
            //var itemContent = ObterConteudo(chaveValorViewModel);

            var itemContent = ObterConteudo(chaveValorViewModel.Arquivo);


            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(chaveValorViewModel.Arquivo.OpenReadStream());
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(chaveValorViewModel.Arquivo.ContentType);

            content.Add(fileContent, "arquivo", chaveValorViewModel.Arquivo.FileName);
            content.Add(ObterConteudo(chaveValorViewModel.IDEMPRESA), "idEmpresa" );
            //content.Add(ObterConteudo(chaveValorViewModel.CHAVE), "chave");
            //content.Add(ObterConteudo(chaveValorViewModel.VALOR), "valor");

            var response = await _httpClient.PostAsync($"/api/v1/config/atualiza-certificado", content);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ConfigImagemViewModel> ObterConfigImagemPorId(long idEmpresa, string chave)
        {
            var response = await _httpClient.GetAsync($"/api/v1/config/config-imagem-por-id/{idEmpresa}/{chave}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ConfigImagemViewModel>(response);
        }

        public async Task<PagedViewModel<ConfigIndexViewModel>> ObterConfigurcoesPorEmpresaEChave(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/config/configuracoes?q={nome}&page={page}&ps={pageSize}&idEmpresa={idEmpresa}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<ConfigIndexViewModel>>(response);
        }

        public async Task<IEnumerable<ConfigImagemViewModel>> ObterCongiImagemPorEmpresa(long idEmpresa)
        {
            var response = await _httpClient.GetAsync($"/api/v1/config/config-imagem/{idEmpresa}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<ConfigImagemViewModel>>(response);
        }

        public async Task<ConfigIndexViewModel> ObterPorChave(string chave, long idEmpresa)
        {
            var response = await _httpClient.GetAsync($"/api/v1/config/obter-config-por-chave?chave={chave}&idEmpresa={idEmpresa}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ConfigIndexViewModel>(response);
        }

        public async Task<List<ConfigIndexViewModel>> ObterTodosPorEmpresa(long idEmpresa)
        {
            var response = await _httpClient.GetAsync($"/api/v1/config/obter-por-empresa/{idEmpresa}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<ConfigIndexViewModel>>(response);
        }

    }
}

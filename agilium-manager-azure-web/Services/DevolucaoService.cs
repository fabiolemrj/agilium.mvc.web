using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.DevolucaoViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class DevolucaoService :Service, IDevolucaoService
    {
        public DevolucaoService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        #region Devolucao

        public async Task<PagedViewModel<DevolucaoViewModel>> ObterPaginacaoPorData(long idEmpresa, string dtInicial, string dtFinal, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/devolucao/obter-por-data?idEmpresa={idEmpresa}&dtInicial={dtInicial}&dtFinal={dtFinal}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<DevolucaoViewModel>>(response);
        }

        public async Task<ResponseResult> Adicionar(DevolucaoViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/devolucao", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }
        public async Task<ResponseResult> Atualizar(long id, DevolucaoViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PutAsync($"/api/v1/devolucao/{model.Id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> CancelarDevolucao(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/devolucao/cancelar/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<DevolucaoViewModel> ObterDevolucaoPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/devolucao/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<DevolucaoViewModel>(response); ;
        }

        public async Task<ResponseResult> GerarVale(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/devolucao/gerar-vale/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> Realizar(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/devolucao/realizar/{id}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        #endregion

        #region Devolucao Item
        public async Task<List<DevolucaoItemViewModel>> ObterDevolucaoItemPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/devolucao/itens/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<DevolucaoItemViewModel>>(response);
        }

        public async Task<List<DevolucaoItemVendaViewModel>> ObterDevolucaoItemVendaPorId(long idDevolucao, long idVenda)
        {
            var response = await _httpClient.GetAsync($"/api/v1/devolucao/itens/venda/{idDevolucao}/{idVenda}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<DevolucaoItemVendaViewModel>>(response);
        }
        #endregion

        #region MotivoDevolucao
        public async Task<ResponseResult> Adicionar(MotivoDevolucaoViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PostAsync($"/api/v1/devolucao/motivo", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }


        public async Task<ResponseResult> Atualizar(long id, MotivoDevolucaoViewModel model)
        {
            var itemContent = ObterConteudo(model);

            var response = await _httpClient.PutAsync($"/api/v1/devolucao/motivo/{model.Id}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ViewModels.ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<PagedViewModel<MotivoDevolucaoViewModel>> ObterMotivoPaginacaoPorDescricao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            var response = await _httpClient.GetAsync($"/api/v1/devolucao/motivos?idEmpresa={idEmpresa}&q={nome}&page={page}&ps={pageSize}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<MotivoDevolucaoViewModel>>(response);
        }

        public async Task<MotivoDevolucaoViewModel> ObterMotivoPorId(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/devolucao/obter-motivo-por-id/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<MotivoDevolucaoViewModel>(response);
        }


        public async Task<ResponseResult> RemoverMotivo(long id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/devolucao/motivo/{id}");

            TratarErrosResponse(response);

            return RetornoOk();
        }

        public async Task<List<MotivoDevolucaoViewModel>> ObterTodosMotivos()
        {
            var response = await _httpClient.GetAsync($"/api/v1/devolucao/motivo/obter-todos");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<MotivoDevolucaoViewModel>>(response);
        }


        #endregion
    }
}

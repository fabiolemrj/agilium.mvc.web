using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.PontoVendaViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.api.manager.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/pdv")]
    [ApiVersion("1.0")]
    [ApiController]
    public class PontoVendaController : MainController
    {
        private readonly IPontoVendaService _pontoVendaService;
        private readonly IEmpresaService _empresaService;
        private readonly IMapper _mapper;
        private const string _nomeEntidade = "Ponto de Venda";

        public PontoVendaController(INotificador notificador, IUser appUser, IConfiguration configuration,
            IPontoVendaService pontoVendaService, IEmpresaService empresaService, IMapper mapper, 
            IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, configuration, utilDapperRepository, logService)
        {
            _pontoVendaService = pontoVendaService;
            _empresaService = empresaService;
            _mapper = mapper;
        }

        #region endpoint
        [HttpGet("obter-por-descricao")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PontoVendaViewModel>>> IndexPagination([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterListaPaginado(idEmpresa, q, page, ps));
            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] PontoVendaViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Id == 0) viewModel.Id = await GerarId();

            var pontoVenda = _mapper.Map<PontoVenda>(viewModel);

            await _pontoVendaService.Adicionar(pontoVenda);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("PontoVenda", "Adicionar", "Web", Deserializar(pontoVenda)));
                return CustomResponse(msgErro);
            }
            await _pontoVendaService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "PontoVenda", "Adicionar", null);
            return CustomResponse(viewModel);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] PontoVendaViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _pontoVendaService.Atualizar(_mapper.Map<PontoVenda>(viewModel));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("PontoVenda", "Atualizar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _pontoVendaService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "PontoVenda", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PontoVendaViewModel>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var fornecedor = await _pontoVendaService.ObterCompletoPorId(_id);
            if (fornecedor != null)
            {
                var objeto = _mapper.Map<PontoVendaViewModel>(fornecedor);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("ponto de venda nao localizado"));

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(long id)
        {
            var viewModel = await _pontoVendaService.ObterPorId(id);

            if (viewModel == null) return NotFound();

            await _pontoVendaService.Apagar(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("PontoVenda", "Excluir", "Web", $"id:{id}"));
                return CustomResponse(msgErro);
            }
            await _pontoVendaService.Salvar();
            LogInformacao($"sucesso: id{id}", "PontoVenda", "Excluir", null);
            return CustomResponse(viewModel);
        }
        #endregion

        #region metodos privados
        private async Task<business.Models.PagedResult<PontoVendaViewModel>> ObterListaPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _pontoVendaService.ObterPorPaginacao(idEmpresa, filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<PontoVendaViewModel>>(retorno.List);

            return new business.Models.PagedResult<PontoVendaViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }
        #endregion
    }
}

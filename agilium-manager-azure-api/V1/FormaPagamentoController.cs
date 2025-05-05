using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.FormaPagamentoViewModel;
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
    [Route("api/v{version:apiVersion}/forma-pagamento")]
    [ApiVersion("1.0")]
    public class FormaPagamentoController : MainController
    {
        private readonly IFormaPagamentoService _formaPagamentoService;
        private readonly IMapper _mapper;

        public FormaPagamentoController(INotificador notificador, IUser appUser, 
            IConfiguration configuration, IUtilDapperRepository utilDapperRepository, ILogService logService, 
            IFormaPagamentoService formaPagamentoService, IMapper mapper) : base(notificador, appUser, configuration, utilDapperRepository, logService)
        {
            _formaPagamentoService = formaPagamentoService;
            _mapper = mapper;
        }

        #region forma pagamento
        [HttpGet("obter-por-descricao")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormaPagamentoViewModel>>> IndexPagination([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var lista = await ObterListaPaginado(idEmpresa, q, page, ps);
            ViewBag.Pesquisa = q;            

            return CustomResponse(lista);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] FormaPagamentoViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<FormaPagamento>(viewModel);

            if (objeto.Id == 0) objeto.Id = objeto.GerarId();
            await _formaPagamentoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("FormaPagamento", "Adicionar", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }
            await _formaPagamentoService.Salvar();
            LogInformacao($"sucesso: {Deserializar(objeto)}", "FormaPagamento", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FormaPagamentoViewModel>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var objeto = _mapper.Map<FormaPagamentoViewModel>(await _formaPagamentoService.ObterPorId(_id));
            return CustomResponse(objeto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] FormaPagamentoViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _formaPagamentoService.Atualizar(_mapper.Map<FormaPagamento>(viewModel));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("FormaPagamento", "Atualizar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _formaPagamentoService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "FormaPagamento", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(long id)
        {
            var viewModel = await _formaPagamentoService.ObterPorId(id);

            if (viewModel == null) return NotFound();

            await _formaPagamentoService.Apagar(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("FormaPagamento", "Excluir", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _formaPagamentoService.Salvar();
            LogInformacao($"excluir {Deserializar(viewModel)}", "FormaPagamento", "Excluir", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("obter-todas")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<FormaPagamentoViewModel>>> ObterTodas()
        {
            var objeto = _mapper.Map<IEnumerable<FormaPagamentoViewModel>>(await _formaPagamentoService.ObterTodas());
            return CustomResponse(objeto);
        }

        #endregion

        #region metodos privados
        private async Task<agilium.api.business.Models.PagedResult<FormaPagamentoViewModel>> ObterListaPaginado(long idempresa, string filtro, int page, int pageSize)
        {
            var retorno = await _formaPagamentoService.ObterPorPaginacao(idempresa, filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<FormaPagamentoViewModel>>(retorno.List);

            return new agilium.api.business.Models.PagedResult<FormaPagamentoViewModel>()
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

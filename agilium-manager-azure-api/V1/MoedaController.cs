using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.FornecedorViewModel;
using agilium.api.manager.ViewModels.MoedaViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.manager.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/moeda")]
    [ApiVersion("1.0")]
    [ApiController]
    public class MoedaController : MainController
    {
        private readonly IMoedaService _moedaService;
        private readonly IEmpresaService _empresaService;
        private readonly IMapper _mapper;
        private const string _nomeEntidade = "Moeda";
        public MoedaController(INotificador notificador, IUser appUser, IConfiguration configuration, IMoedaService moedaService, IEmpresaService empresaService,
            IMapper mapper, IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, configuration,utilDapperRepository, logService)
        {
            _moedaService = moedaService;
            _empresaService = empresaService;
            _mapper = mapper;
        }
        #region endpoint
        [HttpGet("obter-por-descricao")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MoedaViewModel>>> IndexPagination([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
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
        public async Task<ActionResult> Adicionar([FromBody] MoedaViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Id == 0) viewModel.Id = await GerarId();

            var moeda = _mapper.Map<Moeda>(viewModel);

            await _moedaService.Adicionar(moeda);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Moeda", "Adicionar", "Web", Deserializar(moeda)));
                return CustomResponse(msgErro);
            }
            await _moedaService.Salvar();
            LogInformacao($"sucesso: {Deserializar(moeda)}", "Moeda", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] MoedaViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _moedaService.Atualizar(_mapper.Map<Moeda>(viewModel));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Moeda", "Atualizar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _moedaService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Moeda", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MoedaViewModel>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var moeda = await _moedaService.ObterCompletoPorId(_id);
            if (moeda != null)
            {
                var objeto = _mapper.Map<MoedaViewModel>(moeda);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Moeda nao localizado"));
        }

        [HttpGet("obter-todas")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MoedaViewModel>>> ObterTodas(string id)
        {
            long _id = Convert.ToInt64(id);
            var moedas = await _moedaService.ObterTodas();
            if (moedas != null)
            {
                var objeto = _mapper.Map<IEnumerable<MoedaViewModel>>(moedas);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Moeda nao localizado"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(long id)
        {
            var viewModel = await _moedaService.ObterPorId(id);

            if (viewModel == null) return NotFound();

            await _moedaService.Apagar(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Moeda", "Excluir", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _moedaService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Moeda", "Excluir", null);
            return CustomResponse(viewModel);
        }
        #endregion

        #region metodos privados
        private async Task<business.Models.PagedResult<MoedaViewModel>> ObterListaPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _moedaService.ObterPorPaginacao(idEmpresa, filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<MoedaViewModel>>(retorno.List);

            
            return new business.Models.PagedResult<MoedaViewModel>()
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

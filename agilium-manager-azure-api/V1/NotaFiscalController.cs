using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.NotaFiscalViewModel;
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
    [Route("api/v{version:apiVersion}/nota-fiscal")]
    [ApiVersion("1.0")]
    public class NotaFiscalController : MainController
    {
        private readonly INotaFiscalService _notaFiscalService;
        private readonly IPNotaFiscalDapperRepository _notaFiscalDapperRepository;
        private readonly IMapper _mapper;
        public NotaFiscalController(INotificador notificador, IUser appUser, IConfiguration configuration, 
            INotaFiscalService notaFiscalService, IMapper mapper, IPNotaFiscalDapperRepository pNotaFiscalDapperRepository, 
            IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, configuration,utilDapperRepository,logService)
        {
            _notaFiscalService = notaFiscalService;
            _mapper = mapper;
            _notaFiscalDapperRepository = pNotaFiscalDapperRepository;
        }

        #region Nota Fiscal Inutil

        [HttpGet("inutil/obter-por-descricao")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<NotaFiscalViewModel>>> IndexPagination([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterListaPaginado(idEmpresa, q, page, ps));
            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }

        [HttpPost("inutil")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] NotaFiscalViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Id == 0) viewModel.Id = await GerarId();

            var objeto = _mapper.Map<NotaFiscalInutil>(viewModel);
            await _notaFiscalService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("NotaFiscal", "Adicionar", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }
            await _notaFiscalService.Salvar();
            LogInformacao($"sucesso: {Deserializar(objeto)}", "NotaFiscal", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("inutil/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id,[FromBody] NotaFiscalViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }


            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _notaFiscalService.Atualizar(_mapper.Map<NotaFiscalInutil>(viewModel));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("NotaFiscal", "Atualizar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _notaFiscalService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "NotaFiscal", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        [HttpDelete("inutil/{id}")]
        public async Task<ActionResult> ExcluirNotaFiscalInutil(string id)
        {
            long _id = Convert.ToInt64(id);
            await _notaFiscalService.ApagarNFInutil(_id);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("NotaFiscal", "Excluir", "Web", $"id:{id}"));
                return CustomResponse(msgErro);
            }
            await _notaFiscalService.Salvar();
            LogInformacao($"sucesso: id:{id}", "NotaFiscal", "Excluir", null);
            return CustomResponse("sucesso");
        }

        [HttpGet("inutil/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<NotaFiscalViewModel>> ObterNFInutilPorId(string id)
        {
            long _id = Convert.ToInt64(id);
            var produto = await _notaFiscalService.ObterNFInutilPorId(_id);
            if (produto != null)
            {
                var objeto = _mapper.Map<NotaFiscalViewModel>(produto);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Nota Fiscal Inutilizada nao localizada"));

        }

        [HttpPost("inutil/obter-todos-por-empresa")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<NotaFiscalViewModel>>> ObterNFInutilTodos([FromBody] long idEmpresa)
        {
            var objeto = _mapper.Map<IEnumerable<NotaFiscalViewModel>>(await _notaFiscalService.ObterTodasNFInutil(idEmpresa));
            return CustomResponse(objeto);
        }


        [Route("inutil/enviar/{id}")]
        [HttpGet]
        public async Task<IActionResult> EnviarNotaFiscalInutilSefaz(long id)
        {
            var msgResultado = "";
            try
            {
                await _notaFiscalDapperRepository.EnviarNotaFiscalInutil(id);
                msgResultado = "Nota fiscal inutilizada enviada ao Sefaz com sucesso!";
            }
            catch
            {
                NotificarErro("Erro ao tentar enviar nota fiscal inutilizada ao Sefaz");

            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("NotaFiscal", "EnviarNotaFiscalInutil", "Web", $"id:{id}"));
                return CustomResponse(msgErro);
            }

            LogInformacao($"sucesso: id:{id}", "NotaFiscal", "EnviarNotaFiscalInutil", null);
            return CustomResponse(msgResultado);
        }
        #endregion

        #region Private
        private async Task<business.Models.PagedResult<NotaFiscalViewModel>> ObterListaPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _notaFiscalService.ObterNFInutilPorPaginacao(idEmpresa, filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<NotaFiscalViewModel>>(retorno.List);

            return new business.Models.PagedResult<NotaFiscalViewModel>()
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

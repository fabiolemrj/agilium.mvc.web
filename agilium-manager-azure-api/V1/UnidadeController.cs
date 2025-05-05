using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.UnidadeViewModel;
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
    [Route("api/v{version:apiVersion}/unidade")]
    [ApiVersion("1.0")]
    public class UnidadeController : MainController
    {
        private readonly IUnidadeService _unidadeService;
        private readonly IMapper _mapper;
        private const string _nomeEntidade = "Unidade";

        public UnidadeController(INotificador notificador, IUser appUser, IUnidadeService unidadeService,
            IMapper mapper, IConfiguration configuration, 
            IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser,configuration, 
                utilDapperRepository,logService)
        {
            _unidadeService = unidadeService;
            _mapper = mapper;
        }

        #region EndPoints
        [HttpGet("obter-por-descricao")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<UnidadeIndexViewModel>>> IndexPagination([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterListaPaginado(q, page, ps));
            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] UnidadeIndexViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<Unidade>(viewModel);
            
            if (objeto.Id == 0) objeto.Id = await GerarId();

            await _unidadeService.Adicionar(objeto);


            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Unidade", "Adicionar", "Web"));
                ObterNotificacoes("Unidade", "Adicionar", "Web");
                return CustomResponse(msgErro);
            }
            await _unidadeService.Salvar();
            var objetoDeserialziado = Deserializar(viewModel);
            LogInformacao($"Objeto Criado com sucesso {objetoDeserialziado}", "Unidade", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] UnidadeIndexViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                ObterNotificacoes("Unidade", "Atualizar", "Web");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<Unidade>(viewModel);
            await _unidadeService.Atualizar(objeto);
         
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Unidade", "Atualizar", "Web"));

                return CustomResponse(msgErro);
            }
            await _unidadeService.Salvar();
            var objetoDeserialziado = Deserializar(objeto);
            LogInformacao($"Objeto atualizado com sucesso {objetoDeserialziado}", "Unidade", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        //   [ClaimsAuthorize("Usuario", "Excluir")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(long id)
        {
            var viewModel = await _unidadeService.ObterPorId(id);

            if (viewModel == null) return NotFound();

            await _unidadeService.Apagar(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Unidade", "Excluir", "Web");
                return CustomResponse(msgErro);
            }
            await _unidadeService.Salvar();
            var objetoDeserialziado = Deserializar(viewModel);
            LogInformacao($"Objeto excluido com sucesso {objetoDeserialziado}", "Unidade", "Atualizar", null);
            return CustomResponse(viewModel);
        }


        [HttpGet("obter-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UnidadeIndexViewModel>> ObterPorId(string id)
        {
            long _id = Convert.ToInt64(id);
            var objeto = _mapper.Map<UnidadeIndexViewModel>(await _unidadeService.ObterPorId(_id));
            return CustomResponse(objeto);
        }

        [HttpGet("mudar-situacao/{id}")]
        public async Task<ActionResult<UnidadeIndexViewModel>> MudarSituacao(string id)
        {
            long _id = Convert.ToInt64(id);
            var sucesso = await _unidadeService.MudarSituacao(_id);

            if (!sucesso)
            {
                
                NotificarErro("Erro: Não foi possível mudar a situação");
                ObterNotificacoes("Unidade", "MudarSituacao", "Web");
            }
            return CustomResponse(sucesso);
        }

        [HttpGet("obter-todas")]
        public async Task<ActionResult<List<UnidadeIndexViewModel>>> ObterTodas()
        {

            var objeto = _mapper.Map<List<UnidadeIndexViewModel>>(await _unidadeService.ObterTodas());
            return CustomResponse(objeto);
        }
        #endregion

        #region metodos privados
        private async Task<agilium.api.business.Models.PagedResult<UnidadeIndexViewModel>> ObterListaPaginado(string filtro, int page, int pageSize)
        {
            var retorno = await _unidadeService.ObterPorDescricaoPaginacao(filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<UnidadeIndexViewModel>>(retorno.List);

            return new agilium.api.business.Models.PagedResult<UnidadeIndexViewModel>()
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

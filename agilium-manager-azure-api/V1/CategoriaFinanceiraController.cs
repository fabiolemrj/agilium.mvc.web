using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.Validations;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.CategoriaFinancViewModel;

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
    [Route("api/v{version:apiVersion}/categ-financ")]
    [ApiVersion("1.0")]
    public class CategoriaFinanceiraController : MainController
    {
        private readonly ICategoriaFinanceiraService _categoriaService;
        private readonly IMapper _mapper;
        private const string _nomeEntidade = "Categoria Financeira";

        public CategoriaFinanceiraController(INotificador notificador, IUser appUser,
            ICategoriaFinanceiraService categoriaService, IMapper mapper, IConfiguration configuration, 
            IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, configuration, 
                utilDapperRepository, logService)
        {
            _categoriaService = categoriaService;
            _mapper = mapper;
        }
        #region EndPoints

        [HttpGet("obter-por-descricao")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CategoriaFinanceiraValidation>>> IndexPagination([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var lista = (await ObterListaPaginado(q, page, ps));
            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] CategoriaFinancViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Id == 0) viewModel.Id = GerarId().Result;

            var categoriaFinanceira = _mapper.Map<CategoriaFinanceira>(viewModel);
           
            await _categoriaService.Adicionar(categoriaFinanceira);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("CategoriaFinanceira", "Adicionar", "Web");
                return CustomResponse(msgErro);
            }
            await _categoriaService.Salvar();
            LogInformacao($"Objeto adicionado com sucesso {Deserializar(categoriaFinanceira)}", "CategoriaFinanceira", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CategoriaFinancViewModel>> Obter(long id)
        {
            var objeto = _mapper.Map<CategoriaFinancViewModel>(await _categoriaService.ObterPorId(id));
            return CustomResponse(objeto);
        }

        [HttpGet("obter-todos")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CategoriaFinancViewModel>>> ObterTodos(long id)
        {
            var objeto = _mapper.Map<List<CategoriaFinancViewModel>>(await _categoriaService.ObterTodos());
            return CustomResponse(objeto);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(long id, [FromBody] CategoriaFinancViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                ObterNotificacoes("CategoriaFinanceira", "Atualizar", "Web");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _categoriaService.Atualizar(_mapper.Map<CategoriaFinanceira>(viewModel));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("CategoriaFinanceira", "Atualizar", "Web");
                return CustomResponse(msgErro);
            }
            await _categoriaService.Salvar();
            LogInformacao($"Objeto atualizado com sucesso {Deserializar(viewModel)}", "CategoriaFinanceira", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(long id)
        {
            var viewModel = await _categoriaService.ObterPorId(id);

            if (viewModel == null) return NotFound();

            await _categoriaService.Remover(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("CategoriaFinanceira", "Excluir", "Web");
                return CustomResponse(msgErro);
            }
            await _categoriaService.Salvar();
            LogInformacao($"Objeto atualizado com sucesso id: {id}", "CategoriaFinanceira", "Excluir", null);
            return CustomResponse(viewModel);
        }

        #endregion

        #region metodos privados
        private async Task<PagedResult<CategoriaFinancViewModel>> ObterListaPaginado(string filtro, int page, int pageSize)
        {
            var retorno = await _categoriaService.ObterPorDescricaoPaginacao(filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<CategoriaFinancViewModel>>(retorno.List);

            return new PagedResult<CategoriaFinancViewModel>()
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

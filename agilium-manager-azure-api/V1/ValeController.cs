using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.ValeViewModel;
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
    [Route("api/v{version:apiVersion}/vale")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ValeController : MainController
    {
        private readonly IValeService _valeService;
        private readonly IMapper _mapper;
        public ValeController(INotificador notificador, IUser appUser, IConfiguration configuration, IValeService valeService,
            IMapper mapper, IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, 
                configuration,utilDapperRepository, logService)
        {
            _valeService = valeService;
            _mapper = mapper;
        }

        #region Vale

        [HttpGet("obter-por-nome-cliente")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ValeViewModel>>> IndexPagination([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
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
        public async Task<ActionResult> Adicionar([FromBody] ValeViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Id == 0) viewModel.Id = await GerarId();
            if (!viewModel.DataHora.HasValue)
                viewModel.DataHora = DateTime.Now;
            viewModel.CodigoBarra = _valeService.GerarCodigoBarraVale().Result;
                       
            var vale = _mapper.Map<Vale>(viewModel);

            await _valeService.Adicionar(vale);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Vale", "Adicionar", "Web", Deserializar(vale)));
                return CustomResponse(msgErro);
            }
            await _valeService.Salvar();
            LogInformacao($"sucesso: {Deserializar(vale)}", "Vale", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] ValeViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var vale = _mapper.Map<Vale>(viewModel);
            
            await _valeService.Atualizar(vale);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Vale", "Atualizar", "Web", Deserializar(vale)));
                return CustomResponse(msgErro);
            }

            await _valeService.Salvar();
            LogInformacao($"sucesso: {Deserializar(vale)}", "Vale", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ValeViewModel>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var vale = await _valeService.ObterPorId(_id);
            if (vale != null)
            {
                var objeto = _mapper.Map<ValeViewModel>(vale);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Vale nao localizado"));

        }


        [HttpGet("obter-todos-por-empresa/{idEmpresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ValeViewModel>>> ObterTodos(long idEmpresa)
        {
            long _id = Convert.ToInt64(idEmpresa);
            var vale = await _valeService.ObterTodas(_id);

            return CustomResponse(vale);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(long id)
        {
            var viewModel = await _valeService.ObterPorId(id);

            if (viewModel == null) return NotFound();

            await _valeService.Apagar(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Vale", "Excluir", "Web", $"id:{id}"));
                return CustomResponse(msgErro);
            }
            await _valeService.Salvar();
            LogInformacao($"sucesso: id:{id}", "Vale", "Excluir", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("cancelar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Cancelar(long id)
        {
            var vale = await _valeService.ObterPorId(id);

            if (vale == null) return NotFound();

            if(vale.STVALE == business.Enums.ESituacaoVale.Utilizado)
            {
                NotificarErro("Vales com status de UTILIZADO não podem ser cancelados."); 
                var msgErro = string.Join("\n\r", ObterNotificacoes("Vale", "cancelar", "Web", $"id:{id}"));
                return CustomResponse(msgErro);
            }

            vale.Cancelar();
            
            await _valeService.Atualizar(vale);
            
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                return CustomResponse(msgErro);
            }
            await _valeService.Salvar();
            LogInformacao($"sucesso: id:{id}", "Vale", "Cancelar", null);
            return CustomResponse(vale);
        }

        #endregion

        #region Private
        private async Task<business.Models.PagedResult<ValeViewModel>> ObterListaPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _valeService.ObterValePorPaginacao(idEmpresa, filtro, page, pageSize);

            var listaContaPagarViewModel = new List<ValeViewModel>();

            retorno.List.ToList().ForEach(vale => {
                var caixaMoedaViewModel = _mapper.Map<ValeViewModel>(vale);
                caixaMoedaViewModel.EmpresaNome = vale.Empresa != null && !string.IsNullOrEmpty(vale.Empresa.NMRZSOCIAL) ? vale.Empresa.NMRZSOCIAL : string.Empty;
                caixaMoedaViewModel.ClienteNome = vale.Cliente != null && !string.IsNullOrEmpty(vale.Cliente.NMCLIENTE) ? vale.Cliente.NMCLIENTE : string.Empty;
            
                listaContaPagarViewModel.Add(caixaMoedaViewModel);
            });

            return new business.Models.PagedResult<ValeViewModel>()
            {
                List = listaContaPagarViewModel,
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

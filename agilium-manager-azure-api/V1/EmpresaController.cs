using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.Extension;
using agilium.api.manager.ViewModels.EmpresaViewModel;
using agilium.api.manager.ViewModels.EnderecoViewModel;

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
    [Route("api/v{version:apiVersion}/empresa")]
    [ApiVersion("1.0")]
    public class EmpresaController : MainController
    {
        private readonly IEmpresaService _empresaService;
        private readonly IMapper _mapper;
        private const string _nomeEntidade = "Empresa";

        public EmpresaController(INotificador notificador, IUser appUser, IEmpresaService empresaService,
            IMapper mapper, IConfiguration configuration, IUtilDapperRepository utilDapperRepository,
            ILogService logService) : base(notificador, appUser,configuration,utilDapperRepository,logService)
        {
            _empresaService = empresaService;
            _mapper = mapper;
        }

        #region EndPoints
        [HttpGet("obter-por-razaosocial")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EmpresaViewModel>>> IndexPagination([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();
            
            var lista = (await ObterListaPaginado(q, page, ps));
            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] EmpresaCreateViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var empresa = _mapper.Map<Empresa>(viewModel);
            if (empresa.Endereco.Id == 0)
                empresa.Endereco.Id = await GerarId();
            await _empresaService.Adicionar(empresa);

            if (!OperacaoValida())
            {                
                var msgErro = string.Join("\n\r", ObterNotificacoes("Empresa", "Adicionar", "Web",Deserializar(empresa)));
               
                return CustomResponse(msgErro);
            }
            await _empresaService.Salvar();
            LogInformacao($"Inclusao {Deserializar(empresa)}", "Empresa", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EmpresaCreateViewModel>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var objeto = _mapper.Map<EmpresaCreateViewModel>(await _empresaService.ObterCompletoPorId(_id));
            return CustomResponse(objeto);
        }


        [HttpGet("obter-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EmpresaCreateViewModel>> ObterPorId(string id)
        {
            long _id = Convert.ToInt64(id);
            var objeto = _mapper.Map<EmpresaCreateViewModel>(await _empresaService.ObterPorId(_id));
            return CustomResponse(objeto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] EmpresaCreateViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _empresaService.Atualizar(_mapper.Map<Empresa>(viewModel));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Empresa", "Atualizar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _empresaService.Salvar();
            LogInformacao($"Inclusao {Deserializar(viewModel)}", "Empresa", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        //   [ClaimsAuthorize("Usuario", "Excluir")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(long id)
        {
            var viewModel = await _empresaService.ObterPorId(id);

            if (viewModel == null) return NotFound();

            await _empresaService.Apagar(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Empresa", "Excluir", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _empresaService.Salvar();
            LogInformacao($"Inclusao {Deserializar(viewModel)}", "Empresa", "Excluir", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("obter-todas")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EmpresaViewModel>>> ObterTodas()
        {
            var objeto = _mapper.Map<IEnumerable<EmpresaViewModel>>(await _empresaService.ObterTodas());
            return CustomResponse(objeto);
        }


        #endregion

        #region metodos privados
        private async Task<agilium.api.business.Models.PagedResult<EmpresaViewModel>> ObterListaPaginado(string filtro, int page, int pageSize)
        {
            var retorno = await _empresaService.ObterPorDescricaoPaginacao(filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<EmpresaViewModel>>(retorno.List);

            return new agilium.api.business.Models.PagedResult<EmpresaViewModel>()
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

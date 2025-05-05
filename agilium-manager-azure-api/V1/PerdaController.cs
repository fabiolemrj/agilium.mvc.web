using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.PerdaViewModel;
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
    [Route("api/v{version:apiVersion}/perda")]
    [ApiVersion("1.0")]
    [ApiController]
    public class PerdaController : MainController
    {
        private readonly IPerdaService _perdaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IProdutoService _produtoService;
        private readonly IPerdaDapperRepository _perdaDapperRepository;
            
        private readonly IMapper _mapper;

        #region Perda
        public PerdaController(INotificador notificador, IUser appUser, IConfiguration configuration,
            IPerdaService perdaService, IMapper mapper, IUsuarioService usuarioService, IProdutoService produtoService,
            IPerdaDapperRepository perdaDapperRepository, IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, configuration,utilDapperRepository,logService)
        {
            _perdaService = perdaService;
            _mapper = mapper;
            _usuarioService = usuarioService;
            _produtoService = produtoService;
            _perdaDapperRepository = perdaDapperRepository;
        }

        [HttpGet("obter-por-nome-produto")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PerdaViewModel>>> IndexPagination([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
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
        public async Task<ActionResult> Adicionar([FromBody] PerdaViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Id == 0) viewModel.Id = await GerarId();
            if (!viewModel.DataHora.HasValue)
                viewModel.DataHora = DateTime.Now;

            if (!viewModel.IDUSUARIO.HasValue || viewModel.IDUSUARIO.Value == 0)
            {
                var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
                if (usuario != null)
                    viewModel.IDUSUARIO = usuario.Id;
            }
            if (viewModel.IDPRODUTO.HasValue) 
            {
                var produto = _produtoService.ObterPorId(viewModel.IDPRODUTO.Value).Result;
                if(produto != null && produto.VLCUSTOMEDIO.HasValue)
                    viewModel.ValorCustoMedio = produto.VLCUSTOMEDIO.Value;
            }

            var perda = _mapper.Map<Perda>(viewModel);

            await _perdaService.Adicionar(perda);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Perda", "Adicionar", "Web", Deserializar(perda)));
                return CustomResponse(msgErro);
            }

            await _perdaService.Salvar();
            LogInformacao($"sucesso: {Deserializar(perda)}", "Perda", "Adicionar", null);
            var id = _perdaDapperRepository.lancarPerdaRetornaIdHistoricoGerado(perda.Id, AppUser.GetUserEmail()).Result;

            return CustomResponse(viewModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] PerdaViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var perda = _mapper.Map<Perda>(viewModel);

            await _perdaService.Atualizar(perda);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Perda", "Atualizar", "Web", Deserializar(perda)));
                return CustomResponse(msgErro);
            }

            await _perdaService.Salvar();
            LogInformacao($"sucesso: {Deserializar(perda)}", "Perda", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PerdaViewModel>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var vale = await _perdaService.ObterPorId(_id);
            if (vale != null)
            {
                var objeto = _mapper.Map<PerdaViewModel>(vale);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Perda nao localizada"));

        }

        [HttpGet("obter-todos-por-empresa/{idEmpresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PerdaViewModel>>> ObterTodos(long idEmpresa)
        {
            long _id = Convert.ToInt64(idEmpresa);
            var vale = await _perdaService.ObterTodas(_id);

            return CustomResponse(vale);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(long id)
        {
            var viewModel = await _perdaService.ObterPorId(id);

            if (viewModel == null) return NotFound();

            await _perdaService.Apagar(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                return CustomResponse(msgErro);
            }
            await _perdaService.Salvar();

            return CustomResponse(viewModel);
        }
        #endregion

        #region Private
        private async Task<business.Models.PagedResult<PerdaViewModel>> ObterListaPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _perdaService.ObterValePorPaginacao(idEmpresa, filtro, page, pageSize);

            var listaPerdaViewModel = new List<PerdaViewModel>();

            retorno.List.ToList().ForEach(perda => {
                var perdaViewModel = _mapper.Map<PerdaViewModel>(perda);
                perdaViewModel.EmpresaNome = perda.Empresa != null && !string.IsNullOrEmpty(perda.Empresa.NMRZSOCIAL) ? perda.Empresa.NMRZSOCIAL : string.Empty;
                perdaViewModel.ProdutoNome = perda.Produto != null && !string.IsNullOrEmpty(perda.Produto.NMPRODUTO) ? perda.Produto.NMPRODUTO : string.Empty;
                perdaViewModel.UsuarioNome = perda.Usuario != null && !string.IsNullOrEmpty(perda.Usuario.nome) ? perda.Usuario.nome : string.Empty;
                perdaViewModel.EstoqueHistoricoNome = perda.EstoqueHistorico != null && !string.IsNullOrEmpty(perda.EstoqueHistorico.DSHST) ? perda.EstoqueHistorico.DSHST : string.Empty;
                perdaViewModel.EstoqueNome = perda.Estoque != null && !string.IsNullOrEmpty(perda.Estoque.Descricao) ? perda.Estoque.Descricao: string.Empty;

                listaPerdaViewModel.Add(perdaViewModel);
            });

            return new business.Models.PagedResult<PerdaViewModel>()
            {
                List = listaPerdaViewModel,
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

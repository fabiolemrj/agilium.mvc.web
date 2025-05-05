using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.SiteMercadoViewModel;
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
    [Route("api/v{version:apiVersion}/sm")]
    [ApiVersion("1.0")]
    public class SiteMercadoController : MainController
    {
        private readonly ISiteMercadoService _siteMercadoService;
        private readonly IMapper _mapper;
        private readonly IProdutoService _produtoService;
        private readonly IMoedaService _moedaService;
        public SiteMercadoController(INotificador notificador, IUser appUser, IConfiguration configuration, 
            IUtilDapperRepository utilDapperRepository, ILogService logService, ISiteMercadoService siteMercadoService, 
            IMapper mapper, IProdutoService produtoService, IMoedaService moedaService) : base(notificador, appUser, configuration, utilDapperRepository, logService)
        {
            _siteMercadoService = siteMercadoService;
            _mapper = mapper;
            _produtoService = produtoService;
            _moedaService = moedaService;
        }

        #region Produto
        [HttpGet("produto/obter-por-descricao")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ProdutoSiteMercadoViewModel>>> IndexPagination([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterListaProdutoPaginado(idEmpresa, q, page, ps));
            ViewBag.Pesquisa = q;
            return CustomResponse(lista);
        }


        [HttpPost("produto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] ProdutoSiteMercadoViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Id == 0) viewModel.Id = await GerarId();

            if (!viewModel.DataHora.HasValue) viewModel.DataHora = DateTime.Now;

            var produto = _mapper.Map<ProdutoSiteMercado>(viewModel);
            
            await _siteMercadoService.Adicionar(produto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("SM Produto", "Adicionar", "Web", Deserializar(produto)));
                return CustomResponse(msgErro);
            }
            await _siteMercadoService.Salvar();
            LogInformacao($"sucesso: {Deserializar(produto)}", "SM Produto", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("produto/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] ProdutoSiteMercadoViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var produto = _mapper.Map<ProdutoSiteMercado>(viewModel);

            await _siteMercadoService.Atualizar(produto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("SM Produto", "Atualizar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }

            await _siteMercadoService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "SM Produto", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("produto/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProdutoSiteMercadoViewModel>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var produto = await _siteMercadoService.ObterPorId(_id);
            if (produto != null)
            {
                var objeto = _mapper.Map<ProdutoSiteMercadoViewModel>(produto);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Produto SM nao localizado"));
        }


        [HttpGet("produto/obter-todos-por-idempresa/{idEmpresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ProdutoSiteMercadoViewModel>>> ObterTodos(long idEmpresa)
        {
            long _id = Convert.ToInt64(idEmpresa);
            var produtos = _mapper.Map<List<ProdutoSiteMercadoViewModel>>(_siteMercadoService.ObterTodas(_id).Result.ToList());

            return CustomResponse(produtos);
        }

        [HttpDelete("produto/{id}")]
        public async Task<ActionResult> Excluir(long id)
        {
            var viewModel = await _siteMercadoService.ObterPorId(id);

            if (viewModel == null) ObterNotificacoes("SM Produto", "Excluir", "Web", $"Produto nao localizado id:{id}");

            await _siteMercadoService.Apagar(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("SM Produto", "Excluir", "Web", $"id:{id}"));
                return CustomResponse(msgErro);
            }
            await _siteMercadoService.Salvar();
            LogInformacao($"sucesso: id:{id}", "SM Produto", "Excluir", null);
            return CustomResponse(viewModel);
        }
        #endregion

        #region Moeda
        [HttpGet("moeda/obter-por-descricao")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MoedaSiteMercadoViewModel>>> IndexMoedaPagination([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterListaMoedaPaginado(idEmpresa, q, page, ps));
            ViewBag.Pesquisa = q;
            return CustomResponse(lista);
        }

        [HttpPost("moeda")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] MoedaSiteMercadoViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Id == 0) viewModel.Id = await GerarId();
            if (!viewModel.DataHora.HasValue) viewModel.DataHora = DateTime.Now;

            var produto = _mapper.Map<MoedaSiteMercado>(viewModel);

            if (await _siteMercadoService.MoedaSMJaAssociada(viewModel.IDSM.Value, viewModel.IDEMPRESA.Value,viewModel.Id))
            {
                NotificarErro("Moeda já associada");
            }
            else
                await _siteMercadoService.Adicionar(produto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("SM Moeda", "Adicionar", "Web", Deserializar(produto)));
                return CustomResponse(msgErro);
            }
            await _siteMercadoService.Salvar();
            LogInformacao($"sucesso: {Deserializar(produto)}", "SM Moeda", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("moeda/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] MoedaSiteMercadoViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var moeda = _mapper.Map<MoedaSiteMercado>(viewModel);

            if (await _siteMercadoService.MoedaSMJaAssociada(viewModel.IDSM.Value, viewModel.IDEMPRESA.Value, viewModel.Id))
            {
                NotificarErro("Moeda SM já associada");
            }
            else
                await _siteMercadoService.Atualizar(moeda);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("SM Moeda", "Atualizar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }

            await _siteMercadoService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "SM Moeda", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("moeda/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MoedaSiteMercadoViewModel>> ObterMoedaPorId(string id)
        {
            long _id = Convert.ToInt64(id);
            var moeda = await _siteMercadoService.ObterMoedaPorId(_id);
            if (moeda != null)
            {
                var objeto = _mapper.Map<MoedaSiteMercadoViewModel>(moeda);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Moeda nao localizada"));
        }

        [HttpDelete("moeda/{id}")]
        public async Task<ActionResult> ExcluirMoeda(long id)
        {
            var viewModel = await _siteMercadoService.ObterMoedaPorId(id);

            if (viewModel == null) ObterNotificacoes("SM Moeda", "Excluir", "Web", $"Moeda nao localizada id:{id}");

            await _siteMercadoService.ApagarMoeda(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("SM Moeda", "Excluir", "Web", $"id:{id}"));
                return CustomResponse(msgErro);
            }
            await _siteMercadoService.Salvar();
            LogInformacao($"sucesso: id:{id}", "SM Moeda", "Excluir", null);
            return CustomResponse(viewModel);
        }
        #endregion

        #region Private
        private async Task<business.Models.PagedResult<ProdutoSiteMercadoViewModel>> ObterListaProdutoPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _siteMercadoService.ObterPorPaginacao(idEmpresa, filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<ProdutoSiteMercadoViewModel>>(retorno.List);
            lista.ToList().ForEach(item => {
                if (item.IDPRODUTO.HasValue)
                {
                    var produto = _produtoService.ObterPorId(item.IDPRODUTO.Value).Result;
                    if (produto != null)
                        item.ProdutoPdv = produto.NMPRODUTO;

                }
                
                            });
            return new business.Models.PagedResult<ProdutoSiteMercadoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<business.Models.PagedResult<MoedaSiteMercadoViewModel>> ObterListaMoedaPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _siteMercadoService.ObterMoedaPorPaginacao(idEmpresa, filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<MoedaSiteMercadoViewModel>>(retorno.List);
            lista.ToList().ForEach(item => {
                if (item.IDMOEDA.HasValue)
                {
                    var moeda = _moedaService.ObterPorId(item.IDMOEDA.Value).Result;
                    if (moeda != null)
                        item.MoedaPdv = moeda.DSMOEDA;

                }

            });
            return new business.Models.PagedResult<MoedaSiteMercadoViewModel>()
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

using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.ReportViewModel.EstoqueReportViewModel;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.EstoqueViewModel;
using agilium.api.manager.ViewModels.ProdutoVewModel;
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
    [Route("api/v{version:apiVersion}/estoque")]
    [ApiVersion("1.0")]
    public class EstoqueController : MainController
    {
        private readonly IEstoqueService _estoqueService;
        private readonly IEmpresaService _empresaService;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;
        private const string _nomeEntidade = "Estoque";

        public EstoqueController(INotificador notificador, IUser appUser, IConfiguration configuration,
            IMapper mapper, IEstoqueService estoqueService, IEmpresaService empresaService, IProdutoService produtoService
            , IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, configuration,utilDapperRepository,logService)
        {
            _mapper = mapper;
            _estoqueService = estoqueService;
            _empresaService = empresaService;
            _produtoService = produtoService;
        }

        #region Estoque

        [HttpGet("obter-por-descricao")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EstoqueViewModel>>> IndexPagination([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var lista = await ObterListaPaginado(idEmpresa,q, page, ps);
            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] EstoqueViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var estoque = _mapper.Map<Estoque>(viewModel);

            if (estoque.Id == 0) estoque.Id = estoque.GerarId();
            await _estoqueService.Adicionar(estoque);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Estoque", "Adicionar", "Web", Deserializar(estoque)));
                return CustomResponse(msgErro);
            }
            await _estoqueService.Salvar();
            LogInformacao($"sucesso: {Deserializar(estoque)}", "Estoque", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EstoqueViewModel>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var objeto = _mapper.Map<EstoqueViewModel>(await _estoqueService.ObterCompletoPorId(_id));
            return CustomResponse(objeto);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] EstoqueViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _estoqueService.Atualizar(_mapper.Map<Estoque>(viewModel));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Estoque", "Atualizar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _estoqueService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Estoque", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(long id)
        {
            var viewModel = await _estoqueService.ObterPorId(id);

            if (viewModel == null) return NotFound();

            await _estoqueService.Apagar(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Estoque", "Excluir", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _estoqueService.Salvar();
            LogInformacao($"excluir {Deserializar(viewModel)}", "Estoque", "Excluir", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("obter-todas")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EstoqueViewModel>>> ObterTodas()
        {
            var objeto = _mapper.Map<IEnumerable<EstoqueViewModel>>(await _estoqueService.ObterTodas());
            return CustomResponse(objeto);
        }
        #endregion

        #region Estoque Produto

        [HttpGet("produto/{idProduto}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EstoqueProdutoListaViewModel>>> ObterPreco(long idProduto)
        {
            var lista = _estoqueService.ObterProdutoEstoquePorProduto(idProduto).Result;
            Estoque estoque = new Estoque(); ;

            var model = new List<EstoqueProdutoListaViewModel>();

            lista.ForEach(est => {
                if(est.IDESTOQUE != estoque.Id)
                    estoque = _estoqueService.ObterPorId(est.IDESTOQUE.Value).Result;
               
                var estoqueProduto = new EstoqueProdutoListaViewModel() {
                    Id = est.Id,
                    IDESTOQUE = est.IDESTOQUE.Value,
                    IDPRODUTO = est.IDPRODUTO.Value,
                    QuantidadeAtual = est.NUQTD.Value,
                    Estoque = estoque.Descricao,
                    Situacao = estoque.STESTOQUE.Value,
                    TipoEsotque = estoque.Tipo == 1 ? "Almoxarifado" : "Combustiveis",
                    Capacidade = estoque.Capacidade.Value
                };

                model.Add(estoqueProduto);
            });

            return CustomResponse(model);
        }


        [HttpPost("produto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] EstoqueProdutoViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var estoque = _mapper.Map<EstoqueProduto>(viewModel);

            if (estoque.Id == 0) estoque.Id = estoque.GerarId();
            await _estoqueService.Adicionar(estoque);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Estoque", "AdicionarProduto", "Web", Deserializar(estoque)));
                return CustomResponse(msgErro);
            }
            await _estoqueService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Estoque", "AdicionarProduto", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("produto/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] EstoqueProdutoViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _estoqueService.Atualizar(_mapper.Map<EstoqueProduto>(viewModel));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Estoque", "AtualizarProduto", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _estoqueService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Estoque", "AtualizarProduto", null);
            return CustomResponse(viewModel);
        }

        [HttpDelete("produto/{id}")]
        public async Task<ActionResult> ExcluirProduto(long id)
        {
            var viewModel = await _estoqueService.ObterProdutoPorId(id);

            if (viewModel == null) return NotFound();

            await _estoqueService.ApagarProduto(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Estoque", "ExcluirProduto", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _estoqueService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Estoque", "ExcluirProduto", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("produto/obter-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EstoqueProdutoViewModel>> ObterProdutoPorId(string id)
        {
            long _id = Convert.ToInt64(id);
            var objeto = _mapper.Map<EstoqueProdutoViewModel>(await _estoqueService.ObterProdutoPorId(_id));
            return CustomResponse(objeto);
        }

        [HttpGet("produto/obter-por-idestoque/{idEstoque}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ProdutoPorEstoqueViewModel>>> ObterProdutoPorIdEstoque(long idEstoque)
        {
            var lista = await _estoqueService.ObterProdutoEstoquePorEstoque(idEstoque);

            var model = new List<ProdutoPorEstoqueViewModel>();
            var produto = new Produto();

            lista.ForEach(prod => {

                if (prod.IDPRODUTO != produto.Id)
                    produto = _produtoService.ObterPorId(prod.IDPRODUTO.Value).Result;

                var estoqueProduto = new ProdutoPorEstoqueViewModel()
                {
                    Id = prod.Id,
                    idProduto = prod.IDPRODUTO.Value,
                    QuantidadeAtual = prod.NUQTD.HasValue? prod.NUQTD.Value:0,
                    Produto = produto.NMPRODUTO,
                    ValorCustoMedio = produto.VLCUSTOMEDIO.HasValue ?produto.VLCUSTOMEDIO.Value: 0,
                    ValorUltimaCompra = produto.VLULTIMACOMPRA.HasValue ? produto.VLULTIMACOMPRA.Value: 0,
                    Codigo = produto.CDPRODUTO
                };

                model.Add(estoqueProduto);
            });
            return CustomResponse(model);
        }
        #endregion

        #region Estoque Historico
        [HttpGet("historico/produtos/{idProduto}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EstoqueHistoricoViewModel>>> ObterHistoricoPorIdProduto(long idProduto)
        {
            var objeto = _mapper.Map<List<EstoqueHistoricoViewModel>>(await _estoqueService.ObterHistoricoEstoquePorProduto(idProduto));
            return CustomResponse(objeto);
        }
        #endregion

        #region Report
        [HttpGet("report/posicao/{idEstoque}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EstoquePosicaoReport>>> ObterRelatorioPosicaoEstoque(long idEstoque)
        {
            var resultado = _estoqueService.ObterRelatorioPosicaoEstoque(idEstoque).Result;

            return CustomResponse(resultado);
        }
        #endregion

        #region metodos privados
        private async Task<agilium.api.business.Models.PagedResult<EstoqueViewModel>> ObterListaPaginado(long idempresa, string filtro, int page, int pageSize)
        {
            var retorno = await _estoqueService.ObterPorDescricaoPaginacao(idempresa,filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<EstoqueViewModel>>(retorno.List);

            return new agilium.api.business.Models.PagedResult<EstoqueViewModel>()
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

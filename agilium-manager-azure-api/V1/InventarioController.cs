using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using agilium.api.manager.ViewModels.InventarioViewModel;
using System.Linq;
using Microsoft.AspNetCore.Http;
using agilium.api.manager.ViewModels.ProdutoVewModel;

namespace agilium.api.manager.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/inventario")]
    [ApiVersion("1.0")]
    public class InventarioController : MainController
    {
        private readonly IInventarioService _inventarioService;
        private readonly IProdutoService _produtoService;
        private readonly IEstoqueService _estoqueService;
        private readonly IUsuarioService _usuarioService;
        private readonly IPerdaService _perdaService;
        private readonly IMapper _mapper;

        public InventarioController(INotificador notificador, IUser appUser, IConfiguration configuration, IUtilDapperRepository utilDapperRepository,
            IInventarioService inventarioService, IProdutoService produtoService, IEstoqueService estoqueService,
            IUsuarioService usuarioService, IMapper mapper, IPerdaService perdaService, ILogService logService) : base(notificador, appUser, configuration, utilDapperRepository, logService)
        {
            _inventarioService = inventarioService;
            _produtoService = produtoService;
            _estoqueService = estoqueService;
            _usuarioService = usuarioService;
            _mapper = mapper;
            _perdaService = perdaService;
        }

        #region Inventario
        [HttpGet("obter-por-descricao")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<InventarioViewModel>>> ObterPorDataPaginacao([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var lista = (await ObterListaPaginado(idEmpresa,q, page, ps));
            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] InventarioViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Id == 0) viewModel.Id = await GerarId();
            if (!viewModel.Data.HasValue)
                viewModel.Data = DateTime.Now;
            
            var objeto = _mapper.Map<Inventario>(viewModel);

            await _inventarioService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "Adicionar", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }
            await _inventarioService.Salvar();
            LogInformacao($"sucesso: {Deserializar(objeto)}", "Inventario", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] InventarioViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<Inventario>(viewModel);

            await _inventarioService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "Atualizar", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }

            await _inventarioService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Inventario", "Atualizar", null);
            return CustomResponse(viewModel);
        }


        [HttpGet("cancelar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Cancelar(long id)
        {
            var objeto = await _inventarioService.ObterPorId(id);

            if(objeto == null) return NotFound();
            
            objeto.Cancelar();
            await _inventarioService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "Cancelar", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }
            await _inventarioService.Salvar();
            LogInformacao($"sucesso: {id}", "Inventario", "Atualizar", null);
            return CustomResponse();
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Excluir(long id)
        //{
        //    var viewModel = await _inventarioService.ObterPorId(id);

        //    if (viewModel == null) return NotFound();

        //    await _inventarioService.Apagar(id);
        //    if (!OperacaoValida())
        //    {
        //        var msgErro = string.Join("\n\r", ObterNotificacoes());
        //        return CustomResponse(msgErro);
        //    }
        //    await _inventarioService.Salvar();

        //    return CustomResponse(viewModel);
        //}

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InventarioViewModel>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var objeto = await _inventarioService.ObterPorId(_id);
            if (objeto != null)
            {
                var viewModel = await ConverterObjetoEmViewModel(objeto);
                return CustomResponse(viewModel);
            }

            return CustomResponse(BadRequest("Inventario nao localizado"));
        }


        [HttpGet("obter-todos/{idempresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InventarioViewModel>>> ObterTodos(long idempresa)
        {
            var objeto = _mapper.Map<IEnumerable<InventarioViewModel>>(await _inventarioService.ObterTodas(idempresa));
            return CustomResponse(objeto);
        }

        [HttpGet("cadastrar-produtos-por-estoque/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> CadastrarProdutoPorEstoque(long id)
        {
            var objeto = await _inventarioService.ObterPorId(id);

            if (objeto == null) return NotFound();

            await _inventarioService.IncluirProdutosPorEstoque(objeto.IDESTOQUE.Value, objeto.Id);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "CadastrarProdutoPorEstoque", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"sucesso: {Deserializar(objeto)}", "Inventario", "CadastrarProdutoPorEstoque", null);
            return CustomResponse();
        }

        [HttpGet("inventariar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Inventariar(string id)
        {
            long _id = Int64.Parse(id);
            var objeto = await _inventarioService.ObterPorId(_id);

            if (objeto == null) return NotFound();

            objeto.Executar();
            await _inventarioService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "Inventariar", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }
            await _inventarioService.Salvar();
            LogInformacao($"sucesso: {Deserializar(objeto)}", "Inventario", "Inventariar", null);
            return CustomResponse();
        }

        [HttpGet("concluir/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Concluir(string id)
        {
            long _id = Int64.Parse(id);
            var objeto = await _inventarioService.ObterPorId(_id);

            if (objeto == null) return NotFound("Erro");

            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;

            await _inventarioService.ConcluirInventario(objeto.Id,objeto.STINVENT.Value,usuario.Id);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "Concluir", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }
            await _inventarioService.Salvar();
            LogInformacao($"sucesso: {Deserializar(objeto)}", "Inventario", "Concluir", null);
            return CustomResponse();
        }
        #endregion

        #region Item
        [HttpGet("itens/{idInventario}")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<InventarioViewModel>>> ObterItemCompraViewModel(long idInventario)
        {
            var listaObjeto = _inventarioService.ObterItensPorInventario(idInventario).Result;
            var listaViewModel = new List<InventarioItemViewModel>();
            listaObjeto.ForEach(async item => {
                var viewModel = await ConverterObjetoEmViewModel(item);
                listaViewModel.Add(viewModel);
            });

            return CustomResponse(listaViewModel);
        }

        [HttpGet("item/obter-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InventarioViewModel>> ObterItem(string id)
        {
            long _id = Convert.ToInt64(id);
            var objeto = await _inventarioService.ObterItemPorId(_id);

            if (objeto != null)
            {
                var viewModel = await ConverterObjetoEmViewModel(objeto);
                return CustomResponse(viewModel);
            }

            return CustomResponse(BadRequest("Item do inventario nao localizado"));
        }


        [HttpPost("item")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> AdicionarItem([FromBody] InventarioItemViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Id == 0) viewModel.Id = await GerarId();
            if (!viewModel.DataHora.HasValue) viewModel.DataHora = DateTime.Now;
            if (!viewModel.QuantidadeEstoque.HasValue) viewModel.QuantidadeEstoque = 0;
            if (!viewModel.QuantidadeAnalise.HasValue) viewModel.QuantidadeAnalise = 0;

            var objeto = _mapper.Map<InventarioItem>(viewModel);

            await _inventarioService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "AdicionarItem", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }
            await _inventarioService.Salvar();
            LogInformacao($"sucesso: {Deserializar(objeto)}", "Inventario", "AdicionarItem", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("item/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> AtualizarItem(string id, [FromBody] InventarioItemViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<InventarioItem>(viewModel);

            await _inventarioService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "AtualizarItem", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }

            await _inventarioService.Salvar();
            LogInformacao($"sucesso: {Deserializar(objeto)}", "Inventario", "AtualizarItem", null);
            return CustomResponse(viewModel);
        }

        [HttpDelete("item/{id}")]
        public async Task<ActionResult> ExcluirItem(long id)
        {
            var viewModel = await _inventarioService.ObterPorId(id);

            if (viewModel == null) return NotFound();

            await _inventarioService.ApagarItem(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "ExcluirItem", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _inventarioService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Inventario", "ExcluirItem", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("obter-produtos-inventario/{idEmpresa}/{idInventario}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ProdutoViewModel>>> ObterProdutosParaInventario(long idEmpresa, long idInventario)
        {
            var objetos = await _inventarioService.ObetrProdutoDisponvelInventario(idEmpresa,idInventario);
            var listaViewModel = _mapper.Map<IEnumerable<ProdutoViewModel>>(objetos);
            return CustomResponse(listaViewModel);
        }

        [HttpPost("incluir-produtos-inventario")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> IncluirProdutosInventarios([FromBody] AdicionarListaProdutosDisponiveisViewModel model)
        {
            var itensInventario = new List<InventarioItem>();
            model.Produtos.ForEach(item => { 
                var inventarioItem = new InventarioItem(model.idInventario,item.Id,null,null,null,null,null,null);
                itensInventario.Add(inventarioItem);
            });
            
            await _inventarioService.IncluirProdutoInventario(itensInventario);
            
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "AdicionarProdutos", "Web", Deserializar(itensInventario)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"sucesso: {Deserializar(itensInventario)}", "Inventario", "AdicionarProdutos", null);
            return CustomResponse();
        }

        [HttpPost("apuracao")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ApuracaoInventario([FromBody] List<InventarioItemViewModel> model)
        {
            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
            var itens = _mapper.Map<List<InventarioItem>>(model).ToList();
            await _inventarioService.AlterarInventarioItem(itens,usuario.Id);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "ApuracaoInventario", "Web", Deserializar(model)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"sucesso: {Deserializar(model)}", "Inventario", "ApuracaoInventario", null);
            return CustomResponse();
        }

        [HttpPost("item/apagar")]
        public async Task<ActionResult> ApagarItemInventario([FromBody] List<InventarioItemViewModel> model)
        {
            var itens = _mapper.Map<List<InventarioItem>>(model).ToList();

            await _inventarioService.ApagarInventarioItem(itens);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "ApagarItemInventario", "Web", Deserializar(model)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"sucesso: {Deserializar(model)}", "Inventario", "ApagarItemInventario", null);
            return CustomResponse();
        }
        #endregion


        #region private
        private async Task<PagedResult<InventarioViewModel>> ObterListaPaginado(long idempresa, string descricao, int page, int pageSize)
        {
            var lista = new List<InventarioViewModel>();
            var retorno = await _inventarioService.ObterPorPaginacao(idempresa, descricao, page, pageSize);

            retorno.List.ToList().ForEach(async dev =>
            {
                InventarioViewModel viewModel = await ConverterObjetoEmViewModel(dev);

                lista.Add(viewModel);
            });
            return new PagedResult<InventarioViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<InventarioViewModel> ConverterObjetoEmViewModel(Inventario dev)
        {
            var viewModel = _mapper.Map<InventarioViewModel>(dev);

            if (dev.IDESTOQUE.HasValue)
            {
                var estoque = _estoqueService.ObterPorId(dev.IDESTOQUE.Value).Result;
                viewModel.NomeEstoque = estoque != null && !string.IsNullOrEmpty(estoque.Descricao) ? estoque.Descricao : string.Empty;
            }

            return viewModel;
        }

        private async Task<InventarioItemViewModel> ConverterObjetoEmViewModel(InventarioItem dev)
        {
            var viewModel = _mapper.Map<InventarioItemViewModel>(dev);

            if (dev.IDPERDA.HasValue)
            {
                var perda = _perdaService.ObterPorId(dev.IDPERDA.Value).Result;
                viewModel.NomePerda = perda != null && !string.IsNullOrEmpty(perda.CDPERDA) ? perda.CDPERDA : string.Empty;
            }

            if (dev.IDPRODUTO.HasValue)
            {
                var produto = _produtoService.ObterPorId(dev.IDPRODUTO.Value).Result;
                viewModel.NomeProduto = produto != null && !string.IsNullOrEmpty(produto.NMPRODUTO) ? produto.NMPRODUTO : "";
                viewModel.CodigoProduto= produto != null && !string.IsNullOrEmpty(produto.CDPRODUTO) ? produto.CDPRODUTO : "";

            }

            if (dev.IDUSUARIOANALISE.HasValue)
            {
                var usuarioAnalise = _usuarioService.ObterPorUsuarioPorId(dev.IDUSUARIOANALISE.Value).Result;
                viewModel.NomeUsuarioAnalise = usuarioAnalise != null && !string.IsNullOrEmpty(usuarioAnalise.nome)? usuarioAnalise.nome:string.Empty;
            }

            return viewModel;
        }


        #endregion
    }
}

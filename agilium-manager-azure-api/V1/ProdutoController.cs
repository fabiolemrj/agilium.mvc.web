using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Services;
using agilium.api.manager.Controllers;
using agilium.api.manager.Extension;
using agilium.api.manager.ViewModels.EmpresaViewModel;
using agilium.api.manager.ViewModels.ImpostoViewModel;
using agilium.api.manager.ViewModels.PontoVendaViewModel;
using agilium.api.manager.ViewModels.ProdutoVewModel;
using agilium.api.manager.ViewModels.UnidadeViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.manager.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/produto")]
    [ApiVersion("1.0")]
    public class ProdutoController : MainController
    {
        private readonly IMapper _mapper;
        private const string _nomeEntidadeDepart = "Produto Departamento";
        private readonly IProdutoService _produtoService;
        private readonly IEmpresaService _empresaService;
        private readonly IProdutoDapper _produtoDapper;
        private readonly IUnidadeRepository _unidadeRepository;
        private readonly ILogger<ProdutoController> _logger;

        public ProdutoController(INotificador notificador, IUser appUser,
            IMapper mapper, IProdutoService produtoService, IEmpresaService empresaService,
            IUnidadeRepository unidadeRepository,IConfiguration configuration, IProdutoDapper produtoDapper, 
            IUtilDapperRepository utilDapperRepository, ILogger<ProdutoController> logger,
            ILogService logService) : base(notificador, appUser,configuration, utilDapperRepository, logService)
        {
            _mapper = mapper;
            _produtoService = produtoService;
            _empresaService = empresaService;
            _unidadeRepository = unidadeRepository;
            _produtoDapper = produtoDapper;
            _logger = logger;
        }

        #region Produto
        [HttpGet("obter-por-descricao")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ProdutoViewModel>>> IndexPagination([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterListaProdutoPaginado(idEmpresa, q, page, ps));
            ViewBag.Pesquisa = q;
            _logger.LogError($"{AppUser.GetUserEmail()} - Erro produtos");
            return CustomResponse(lista);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] ProdutoViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Id == 0) viewModel.Id = await GerarId();

            var produto = _mapper.Map<Produto>(viewModel);
            if (viewModel.Categoria.HasValue && string.IsNullOrEmpty(produto.CTPRODUTO))
                produto.AdicionarCategoria(viewModel.Categoria.Value);
            
            await _produtoService.Adicionar(produto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "Adicionar", "Web", Deserializar(produto)));
                return CustomResponse(msgErro);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso: {Deserializar(produto)}", "Produto", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] ProdutoViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var produto = _mapper.Map<Produto>(viewModel);
            var precoAtual = _produtoService.ObterPrecoAtual(viewModel.Id).Result;
            
            if(viewModel.Categoria.HasValue && string.IsNullOrEmpty(produto.CTPRODUTO))
                produto.AdicionarCategoria(viewModel.Categoria.Value);

            //adicionar historico de precos
            if (precoAtual != produto.NUPRECO)
                await _produtoService.Adicionar(new ProdutoPreco(produto.Id, AppUser.GetUserEmail(),Convert.ToDecimal(produto.NUPRECO), Convert.ToDecimal(precoAtual), DateTime.Now));

            await _produtoService.Atualizar(produto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "Atualizar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Produto", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProdutoViewModel>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var produto = await _produtoService.ObterCompletoPorId(_id);
            if (produto != null)
            {
                var objeto = _mapper.Map<ProdutoViewModel>(produto);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Produto nao localizado"));

        }

        [HttpGet("obter-produtos-por-idempresa/{idEmpresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ProdutoViewModel>>> ObterTodos(long idEmpresa)
        {
            long _id = Convert.ToInt64(idEmpresa);
            var produtos = _mapper.Map<List<ProdutoViewModel>>(_produtoService.ObterTodas(_id).Result.ToList());

            return CustomResponse(produtos);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(long id)
        {
            var viewModel = await _produtoService.ObterPorId(id);

            if (viewModel == null) return NotFound();

            await _produtoService.Apagar(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "Excluir", "Web", $"id:{id}"));
                return CustomResponse(msgErro);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso: id:{id}", "Produto", "Excluir", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("obter-listas-auxiliares")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ListasAuxiliaresProdutoViewModel>> ObterListasAuxiliares()
        {   var produtos = new ListasAuxiliaresProdutoViewModel();
            produtos.Grupos = _mapper.Map<List<GrupoProdutoViewModel>>(_produtoService.ObterTodosGrupos().Result.ToList());
            produtos.SubGrupos = _mapper.Map<List<SubGrupoViewModel>>(_produtoService.ObterTodosSubGrupos().Result.ToList());
            produtos.Marcas = _mapper.Map<List<ProdutoMarcaViewModel>>(_produtoService.ObterTodosMarca().Result.ToList());
            produtos.Departamentos = _mapper.Map<List<ProdutoDepartamentoViewModel>>(_produtoService.ObterTodosDepartamento().Result.ToList());
            return CustomResponse(produtos);
        }
        #endregion

        #region ProdutoDepartamento
        //[ClaimsAuthorize("ADMINSTRADOR", "CONSULTAR")]
        [HttpGet("departamentos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PerfilIndex([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterPerfil(idEmpresa, q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return CustomResponse(lista);

        }

        [HttpGet("obter-departamento-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProdutoDepartamentoViewModel>> ObterDepartamentoCompletoPorId(long id)
        {
            var departamentos = _produtoService.ObterPorIdDepartamento(id).Result;

            var model = _mapper.Map<ProdutoDepartamentoViewModel>(departamentos);
            model.Empresas = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);
            
            return CustomResponse(model);
        }

        [HttpPost("departamento")]    
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "INCLUIR")]
        public async Task<IActionResult> AdicionarDepartamento([FromBody]ProdutoDepartamentoViewModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<ProdutoDepartamento>(model);

            if (objeto.Id == 0) objeto.Id = objeto.GerarId();
            await _produtoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AdicionarDepartamento", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso: {Deserializar(objeto)}", "Produto", "AdicionarDepartamento", null);
            return CustomResponse(model);
        }

        [HttpPut("departamento/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "ALTERAR")]
        public async Task<IActionResult> AtualizarDepartamento(string id, [FromBody] ProdutoDepartamentoViewModel objetoViewModel)
        {
            if (id != objetoViewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(objetoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<ProdutoDepartamento>(objetoViewModel);

            await _produtoService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AtualizarDepartamento", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso:{Deserializar(objeto)}", "Produto", "AtualizarDepartamento", null);
            return CustomResponse(objetoViewModel);
        }


        [HttpDelete("departamento/{id}")]
        //[ClaimsAuthorize("ADMINSTRADOR", "ALTERAR")]
        public async Task<ActionResult> ApagarDepartamento(long id)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (!_produtoService.ApagarDepartamento(id).Result)
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));
                return CustomResponse(msgErro);
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "ApagarDepartamento", "Web", $"id:{id}"));
                return CustomResponse(msgErro);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso: id:{id}", "Produto", "ApagarDepartamento", null);
            return CustomResponse();
        }
        #endregion

        #region ProdutoMarca
        //[ClaimsAuthorize("ADMINSTRADOR", "CONSULTAR")]
        [HttpGet("marcas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PerfilIndexMarcas([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterMarcas(idEmpresa, q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }


        [HttpGet("obter-marca-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProdutoMarcaViewModel>> ObterMarcaCompletoPorId(long id)
        {
            var departamentos = _produtoService.ObterPorIdMarca(id).Result;

            var model = _mapper.Map<ProdutoMarcaViewModel>(departamentos);
            model.Empresas = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);

            return CustomResponse(model);
        }

        [HttpPost("marca")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "INCLUIR")]
        public async Task<IActionResult> AdicionarMarca([FromBody] ProdutoMarcaViewModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<ProdutoMarca>(model);

            if (objeto.Id == 0) objeto.Id = objeto.GerarId();
            await _produtoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AdicionarMarca", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso: {Deserializar(objeto)}", "Produto", "ApagarDepartamento", null);
            return CustomResponse(model);
        }

        [HttpPut("marca/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "ALTERAR")]
        public async Task<IActionResult> AtualizarMarca(string id, [FromBody] ProdutoMarcaViewModel objetoViewModel)
        {
            if (id != objetoViewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(objetoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<ProdutoMarca>(objetoViewModel);

            await _produtoService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AtualizarMarca", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso:{Deserializar(objeto)}", "Produto", "AtualizarMarca", null);
            return CustomResponse(objetoViewModel);
        }

        [HttpDelete("marca/{id}")]
        //[ClaimsAuthorize("ADMINSTRADOR", "ALTERAR")]
        public async Task<ActionResult> ApagarMarca(long id)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (!_produtoService.ApagarProdutoMarca(id).Result)
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));
                
                NotificarErro(msgErro);
                return CustomResponse(msgErro);
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "ApagarMarca", "Web", $"id:{id}"));
                return CustomResponse(msgErro);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso: id:{id}", "Produto", "ApagarMarca", null);
            return CustomResponse();
        }
        #endregion

        #region Grupo
        //[ClaimsAuthorize("ADMINSTRADOR", "CONSULTAR")]
        [HttpGet("grupos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PerfilIndexGrupo([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterGrupo(idEmpresa, q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }



        [HttpGet("obter-grupo-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GrupoProdutoViewModel>> ObterGrupoPorId(long id)
        {
            var departamentos = _produtoService.ObterPorIdGrupo(id).Result;

            var model = _mapper.Map<GrupoProdutoViewModel>(departamentos);
            model.Empresas = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);

            return CustomResponse(model);
        }

        [HttpPost("grupo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "INCLUIR")]
        public async Task<IActionResult> AdicionarGrupo([FromBody] GrupoProdutoViewModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<GrupoProduto>(model);

            if (objeto.Id == 0) objeto.Id = objeto.GerarId();
            await _produtoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AdicionarGrupo", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso:{Deserializar(objeto)}", "Produto", "AdicionarGrupo", null);
            return CustomResponse(model);
        }


        [HttpPut("grupo/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "ALTERAR")]
        public async Task<IActionResult> AtualizarGrupo(string id, [FromBody] GrupoProdutoViewModel objetoViewModel)
        {
            if (id != objetoViewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(objetoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<GrupoProduto>(objetoViewModel);

            await _produtoService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AtualizarGrupo", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso:{Deserializar(objeto)}", "Produto", "AtualizarGrupo", null);
            return CustomResponse(objetoViewModel);
        }


        [HttpDelete("grupo/{id}")]
        //[ClaimsAuthorize("ADMINSTRADOR", "ALTERAR")]
        public async Task<ActionResult> ApagarGrupo(long id)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (!_produtoService.ApagarProdutoGrupo(id).Result)
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));

                NotificarErro(msgErro);
                return CustomResponse(msgErro);
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "ApagarGrupo", "Web", $"id:{id}"));
                return CustomResponse(msgErro);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso:id:{id}", "Produto", "ApagarGrupo", null);
            return CustomResponse();
        }
        #endregion

        #region SubGrupo
        [HttpGet("subgrupos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PerfilIndexSubGrupo([FromQuery] long idGrupo, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterSubGrupo(idGrupo, q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }

        [HttpGet("obter-subgrupo-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SubGrupoViewModel>> ObterSubGrupoPorId(long id)
        {
            var departamentos = _produtoService.ObterPorIdSubGrupo(id).Result;

            var model = _mapper.Map<SubGrupoViewModel>(departamentos);
            model.NomeGrupo = _produtoService.ObterTodosGrupos().Result.FirstOrDefault(x=>x.Id == departamentos.IDGRUPO).Nome;

            return CustomResponse(model);
        }

        [HttpPost("subgrupo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "INCLUIR")]
        public async Task<IActionResult> AdicionarSubGrupo([FromBody] SubGrupoViewModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<SubGrupoProduto>(model);

            if (objeto.Id == 0) objeto.Id = objeto.GerarId();
            await _produtoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AdicionarSubGrupo", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso:{Deserializar(objeto)}", "Produto", "AdicionarSubGrupo", null);
            return CustomResponse(model);
        }

        [HttpPut("subgrupo/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "ALTERAR")]
        public async Task<IActionResult> AtualizarSubGrupo(string id, [FromBody] SubGrupoViewModel objetoViewModel)
        {
            if (id != objetoViewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(objetoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<SubGrupoProduto>(objetoViewModel);

            await _produtoService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AdicionarSubGrupo", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso:{Deserializar(objeto)}", "Produto", "AdicionarSubGrupo", null);
            return CustomResponse(objetoViewModel);
        }

        [HttpDelete("subgrupo/{id}")]
        //[ClaimsAuthorize("ADMINSTRADOR", "ALTERAR")]
        public async Task<ActionResult> ApagarSubGrupo(long id)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (!_produtoService.ApagarProdutoSubGrupo(id).Result)
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));

                NotificarErro(msgErro);
                return CustomResponse(msgErro);
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "ApagarSubGrupo", "Web", $"id:{id}"));
                return CustomResponse(msgErro);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso:id:{id}", "Produto", "ApagarSubGrupo", null);
            return CustomResponse();
        }

        #endregion

        #region Codigo de Barra
        [HttpGet("codigos-barra/{idProduto}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ProdutoCodigoBarraViewModel>>> ObterCodigoBarra(long idProduto)
        {
            var codigoBarra = _produtoService.ObterTodosCodigoBarraPorProduto(idProduto).Result;

            var model = _mapper.Map<List<ProdutoCodigoBarraViewModel>>(codigoBarra);
            
            return CustomResponse(model);
        }

        [HttpPost("codigo-barra")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "INCLUIR")]
        public async Task<IActionResult> AdicionarCodigoBarra([FromBody] ProdutoCodigoBarraViewModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (model.Id == 0) model.Id = await GerarId();

            var objeto = _mapper.Map<ProdutoCodigoBarra>(model);

            await _produtoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AdicionarCodigoBarra", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso:{Deserializar(objeto)}", "Produto", "AdicionarCodigoBarra", null);
            return CustomResponse(model);
        }

        [HttpPut("codigo-barra/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> AtualizarCodigoBarra(string id, [FromBody] ProdutoCodigoBarraViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var produtoCodigoBarra = _mapper.Map<ProdutoCodigoBarra>(viewModel);
                        
            await _produtoService.Atualizar(produtoCodigoBarra);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AtualizarCodigoBarra", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso:{Deserializar(viewModel)}", "Produto", "AtualizarCodigoBarra", null);
            return CustomResponse(viewModel);
        }

        [HttpDelete("codigo-barra/{id}")]
        public async Task<ActionResult> ExcluirCodigoBarra(long id)
        {
            var viewModel = await _produtoService.ObterCodigoBarraPorId(id);

            if (viewModel == null) return NotFound();

            await _produtoService.ApagarCodigoBarra(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "ExcluirCodigoBarra", "Web", $"id:{id}"));
                return CustomResponse(msgErro);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso:id:{id}", "Produto", "ExcluirCodigoBarra", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("codigo-barra/obter-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProdutoViewModel>> ObterCodigoBarra(string id)
        {
            long _id = Convert.ToInt64(id);
            var produto = await _produtoService.ObterCodigoBarraPorId(_id);
            if (produto != null)
            {
                var objeto = _mapper.Map<ProdutoCodigoBarraViewModel>(produto);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Codigo de Barra do produto nao localizado"));

        }
        #endregion

        #region ProdutoPreco
        [HttpGet("preco/{idProduto}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ProdutoPrecoViewModel>>> ObterPreco(long idProduto)
        {
            var lista = _produtoService.ObterPrecoPorProduto(idProduto).Result;

            var model = _mapper.Map<List<ProdutoPrecoViewModel>>(lista);

            return CustomResponse(model);
        }


        [HttpPost("preco")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "INCLUIR")]
        public async Task<IActionResult> AdicionarPreco([FromBody] ProdutoPrecoViewModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (model.Id == 0) model.Id = await GerarId();

            var objeto = _mapper.Map<ProdutoPreco>(model);

            await _produtoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AdicionarPreco", "Web", Deserializar(objeto)));
                return CustomResponse(msgErro);
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso:{Deserializar(objeto)}", "Produto", "AdicionarPreco", null);
            return CustomResponse(model);
        }

        [HttpPut("preco/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> AtualizarPreco(string id, [FromBody] ProdutoPrecoViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var produtoCodigoBarra = _mapper.Map<ProdutoPreco>(viewModel);

            await _produtoService.Atualizar(produtoCodigoBarra);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AtualizarPreco", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso:{Deserializar(viewModel)}", "Produto", "AtualizarPreco", null);
            return CustomResponse(viewModel);
        }

        [HttpDelete("preco/{id}")]
        public async Task<ActionResult> ExcluirPreco(long id)
        {
            var viewModel = await _produtoService.ObterPrecoPorId(id);

            if (viewModel == null) return NotFound();

            await _produtoService.ApagarPreco(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "ExcluirPreco", "Web", $"id:{id}"));
                return CustomResponse(msgErro);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso:id:{id}", "Produto", "ExcluirPreco", null);
            return CustomResponse(viewModel);
        }


        [HttpGet("preco/obter-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProdutoViewModel>> ObterPreco(string id)
        {
            long _id = Convert.ToInt64(id);
            var produto = await _produtoService.ObterPrecoPorId(_id);
            if (produto != null)
            {
                var objeto = _mapper.Map<ProdutoPrecoViewModel>(produto);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Preço do produto nao localizado"));

        }
        #endregion

        #region Produto Foto
        [HttpGet("foto/{idProduto}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ProdutoFotoViewModel>>> ObterFoto(long idProduto)
        {
            var lista = _produtoService.ObterFotoPorProduto(idProduto).Result;

            //var model = _mapper.Map<List<ProdutoFotoViewModel>>(lista);
            var listaConvertida = new List<ProdutoFotoViewModel>();

            lista.ToList().ForEach(foto => {
                var model = new ProdutoFotoViewModel();
                model.idProduto = idProduto;
                model.Data = foto.Data;
                model.Descricao = foto.Descricao;
                model.Id = foto.Id;
                if (foto.Foto != null)
                    model.FotoConvertida =  foto.Foto;

                listaConvertida.Add(model);
            });

            return CustomResponse(listaConvertida);
        }

      

        [HttpPost("foto")]
        [RequestSizeLimit(bytes: 40_000_000)]
        public async Task<IActionResult> Teste([FromForm]  ProdutoFotoViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            //var objeto = _mapper.Map<ProdutoFoto>(viewModel);
            var objeto = ConverterParaProdutoFoto(viewModel);
            objeto.AdicionarId();
            objeto.AdiconarFoto(await ConverterFormFileToByte(viewModel.Foto));

            await _produtoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                return CustomResponse(msgErro);
            }

            await _produtoService.Salvar();

            return CustomResponse(viewModel);
        }


        [HttpPut("foto/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> AtualizarFoto(string id, [FromBody] ProdutoFotoViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<ProdutoFoto>(viewModel);

            await _produtoService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AtualizarFoto", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso:{Deserializar(objeto)}", "Produto", "AtualizarFoto", null);
            return CustomResponse(viewModel);
        }

        [HttpDelete("foto/{id}")]
        public async Task<ActionResult> ExcluirFoto(long id)
        {
            var viewModel = await _produtoService.ObterFotoPorId(id);

            if (viewModel == null) return NotFound();

            await _produtoService.ApagarFoto(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "ExcluirFoto", "Web", $"id:{id}"));
                return CustomResponse(msgErro);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso:id:{id}", "Produto", "ExcluirFoto", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("foto/obter-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProdutoViewModel>> ObterFoto(string id)
        {
            long _id = Convert.ToInt64(id);
            var produto = await _produtoService.ObterFotoPorId(_id);
            if (produto != null)
            {
                var objeto = _mapper.Map<ProdutoFotoViewModel>(produto);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Foto do produto nao localizado"));

        }
        #endregion

        #region IBPT
        [HttpGet("ibpt/atualizar")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AtualizarIBPT()
        {
            var msgResultado = "";
            try
            {
                await _produtoDapper.AtualizarIBPTTodosProdutos();
                msgResultado = "Produtos atualizados com sucesso!";
            }
            catch
            {
                NotificarErro("Erro ao tentar atualizar IBPT dos produtos");
                
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Atualizacao de IBPT", "AtualizarIBPT", "Web"));
                return CustomResponse(msgErro);
            }

            LogInformacao($"Atualizacao de IBPT", "Produto", "AtualizarIBPT", null);
            return CustomResponse(msgResultado);
        }
        #endregion

        #region private
        private async Task<PagedResult<ProdutoDepartamentoViewModel>> ObterPerfil(long idempresa, string filtro, int page, int pageSize)
        {
            var retorno = await _produtoService.ObterPaginacaoPorDescricao(idempresa, filtro, page, pageSize);
            var listaTeste = retorno.List;
            var lista = _mapper.Map<IEnumerable<ProdutoDepartamentoViewModel>>(listaTeste);

            return new PagedResult<ProdutoDepartamentoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<PagedResult<ProdutoMarcaViewModel>> ObterMarcas(long idempresa, string filtro, int page, int pageSize)
        {
            var retorno = await _produtoService.ObterMarcaPaginacaoPorDescricao(idempresa, filtro, page, pageSize);
            var listaTeste = retorno.List;
            var lista = _mapper.Map<IEnumerable<ProdutoMarcaViewModel>>(listaTeste);

            return new PagedResult<ProdutoMarcaViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<PagedResult<GrupoProdutoViewModel>> ObterGrupo(long idempresa, string filtro, int page, int pageSize)
        {
            var retorno = await _produtoService.ObterGrupoPaginacaoPorDescricao(idempresa, filtro, page, pageSize);
            var listaTeste = retorno.List;
            var lista = _mapper.Map<IEnumerable<GrupoProdutoViewModel>>(listaTeste);

            return new PagedResult<GrupoProdutoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<PagedResult<SubGrupoViewModel>> ObterSubGrupo(long idGrupo, string filtro, int page, int pageSize)
        {
            var retorno = await _produtoService.ObterSubGrupoPaginacaoPorDescricao(idGrupo, filtro, page, pageSize);
            var listaTeste = retorno.List;
            var lista = _mapper.Map<IEnumerable<SubGrupoViewModel>>(listaTeste);

            return new PagedResult<SubGrupoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<business.Models.PagedResult<ProdutoViewModel>> ObterListaProdutoPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _produtoService.ObterPorPaginacao(idEmpresa, filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<ProdutoViewModel>>(retorno.List);

            return new business.Models.PagedResult<ProdutoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private ProdutoFoto ConverterParaProdutoFoto(ProdutoFotoViewModel model)
        {
            return new ProdutoFoto(model.idProduto, model.Descricao, model.Data);
        }
        #endregion
    }
}

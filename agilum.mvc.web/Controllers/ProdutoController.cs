using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Services;

using agilum.mvc.web.Extensions;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Cliente;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Estoque;
using agilum.mvc.web.ViewModels.Impostos;
using agilum.mvc.web.ViewModels.Produtos;
using agilum.mvc.web.ViewModels.Turno;
using agilum.mvc.web.ViewModels.UnidadeViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Route("produto")]
    [Authorize]
    public class ProdutoController : MainController
    {
        #region constantes
        private readonly IProdutoService _produtoService;
        private readonly IEmpresaService _empresaService;
        private readonly IUnidadeService _unidadeService;
        private readonly IEstoqueService _estoqueService;
        private readonly IClienteService _clienteService;
        private readonly ITurnoService _turnoService;
        private readonly ITabelaAuxiliarFiscalService _tabelaAuxiliarFiscalService;
        private readonly IProdutoDapper _produtoDapper;

        private readonly string _nomeEntidadeDepart = "Departamento";
        private readonly string _nomeEntidadeMarca = "Marca";
        private readonly string _nomeEntidadeGrupo = "Grupo de Produtos";
        private readonly string _nomeEntidadeSubGrupo = "SubGrupo de Produtos";
        private readonly string _nomeEntidade = "Produtos";

        #endregion

        #region Listas Auxiliares
        private List<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();
        private List<GrupoProdutoViewModel> Grupos { get; set; } = new List<GrupoProdutoViewModel>();
        private List<CfopViewModel> Cfops { get; set; } = new List<CfopViewModel>();
        private List<CstViewModel> Csts { get; set; } = new List<CstViewModel>();
        private List<CestViewModel> Cests { get; set; } = new List<CestViewModel>();
        private List<SubGrupoViewModel> SubGrupos { get; set; } = new List<SubGrupoViewModel>();
        private List<ProdutoDepartamentoViewModel> Departamentos { get; set; } = new List<ProdutoDepartamentoViewModel>();
        private List<ProdutoMarcaViewModel> Marcas { get; set; } = new List<ProdutoMarcaViewModel>();
        private List<UnidadeIndexViewModel> Unidades { get; set; } = new List<UnidadeIndexViewModel>();
        private List<CsosnViewModel> Csosn { get; set; } = new List<CsosnViewModel>();

        #endregion

        #region construtor
        public ProdutoController(IProdutoService produtoService, IEmpresaService empresaService, INotificador notificador, 
            IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService, IMapper mapper,
            ITabelaAuxiliarFiscalService tabelaAuxiliarFiscalService,IUnidadeService unidadeService, IEstoqueService estoqueService, 
            IClienteService clienteService, ITurnoService turnoService, IProdutoDapper produtoDapper) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper)
        {
            _produtoService = produtoService;
            _empresaService = empresaService;
            _tabelaAuxiliarFiscalService = tabelaAuxiliarFiscalService;
            _unidadeService = unidadeService;
            _estoqueService = estoqueService;
            _clienteService = clienteService;
            _turnoService = turnoService;
            _produtoDapper = produtoDapper;

            if (!listaEmpresaViewModels.Any())
                listaEmpresaViewModels = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result.ToList());

            var listasAuxiliaresProdutos = ObterListasAuxiliaresProdutos().Result;
            var tabelasAuxiliares = ObterTabelasAuxiliares().Result;
            
            Cfops = tabelasAuxiliares.Cfops;
            Csts = tabelasAuxiliares.Csts;
            Csosn = tabelasAuxiliares.Csosn;
            Cests = tabelasAuxiliares.Cests;
            Grupos = listasAuxiliaresProdutos.Grupos;
            SubGrupos = listasAuxiliaresProdutos.SubGrupos;
            Marcas = listasAuxiliaresProdutos.Marcas;
            Departamentos = listasAuxiliaresProdutos.Departamentos;

            Unidades = _mapper.Map<List<UnidadeIndexViewModel>>(_unidadeService.ObterTodas().Result);
        }
        #endregion

        #region produto
        [Route("lista")]
        [ClaimsAuthorizeAttribute(2056)]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidadeGrupo}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidade;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = (await ObterListaProdutoPaginado(Convert.ToInt64(empresaSelecionada.IDEMPRESA), q, page, ps));
         
            ViewBag.Pesquisa = q;
            return View(lista);
        }

        [Route("novo")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2057)]
        public async Task<IActionResult> CreateProduto()
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidadeGrupo}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidade;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateProduto";

            var model = new ProdutoViewModel();
            model.Situacao = EAtivo.Ativo;
            PreencherListaAxuliaresProdutos(model);
            model.Id = 0;
            model.idEmpresa = Convert.ToInt64(empresaSelecionada.IDEMPRESA);
            return View("CreateEditProduto", model);
        }

        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> CreateProduto(ProdutoViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "CreateProduto";
            PreencherListaAxuliaresProdutos(model);

            if (!ModelState.IsValid) return View("CreateEditProduto", model);
            if (model.Id == 0) model.Id = await GerarId();

            var produto = _mapper.Map<Produto>(model);
            if (string.IsNullOrEmpty(produto.CTPRODUTO))
                produto.AdicionarCategoria(model.Categoria);

            await _produtoService.Adicionar(produto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "Adicionar", "Web", Deserializar(produto)));
                AdicionarErroValidacao(msgErro);
                return View("CreateEditProduto", model);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso: {Deserializar(produto)}", "Produto", "Adicionar", null);
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2060)]
        public async Task<IActionResult> EditProduto(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditProduto";
            var objeto = await Obter(id.ToString());
            if (objeto == null)
            {
                var msgErro = $"produto não localizado";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Produto";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            PreencherListaAxuliaresProdutos(objeto);

            return View("CreateEditProduto", objeto);
        }

        [Route("editar")]
        [HttpPost]
        public async Task<IActionResult> EditProduto(ProdutoViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditProduto";
            PreencherListaAxuliaresProdutos(model);

            if (!ModelState.IsValid) return View("CreateEditProduto", model);

            var produto = _mapper.Map<Produto>(model);
            var precoAtual = _produtoService.ObterPrecoAtual(model.Id).Result;

            if (string.IsNullOrEmpty(produto.CTPRODUTO))
                produto.AdicionarCategoria(model.Categoria);

            //adicionar historico de precos
            if (precoAtual != produto.NUPRECO)
                await _produtoService.Adicionar(new ProdutoPreco(produto.Id, AppUser.GetUserEmail(), Convert.ToDecimal(produto.NUPRECO), Convert.ToDecimal(precoAtual), DateTime.Now));

            await _produtoService.Atualizar(produto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "Atualizar", "Web", Deserializar(model)));
                AdicionarErroValidacao(msgErro);
                return View("CreateEditProduto", model);
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso: {Deserializar(model)}", "Produto", "Atualizar", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }
        #endregion

        #region GrupoProduto

        [Route("grupo/lista")]
        [ClaimsAuthorizeAttribute(2011)]
        public async Task<IActionResult> IndexGrupo([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidadeGrupo}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidadeGrupo;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeGrupo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = (await ObterGrupo(Convert.ToInt64(empresaSelecionada.IDEMPRESA), q, page, ps));

            ViewBag.Pesquisa = q;

            return View(lista);
        }

        [Route("grupo/novo")]
        [ClaimsAuthorizeAttribute(2012)]
        public async Task<IActionResult> CreateGrupo()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateGrupo";

            var empresaSelecionada = ObterObjetoEmpresaSelecionada();
            var model = new GrupoProdutoViewModel();
            model.Situacao = EAtivo.Ativo;
            model.Empresas = listaEmpresaViewModels.ToList();
            model.idEmpresa = Convert.ToInt64(empresaSelecionada.IDEMPRESA);
            return View("CreateEditGrupo", model);
        }


        [Route("grupo/novo")]
        [HttpPost]
        public async Task<IActionResult> CreateGrupo(GrupoProdutoViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateGrupo";
            if (!ModelState.IsValid) return View("CreateEditGrupo", model);

            var objeto = _mapper.Map<GrupoProduto>(model);
            
            if (objeto.Id == 0) objeto.Id = objeto.GerarId();
            await _produtoService.Adicionar(objeto);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AdicionarGrupo", "Web", Deserializar(objeto)));
                AdicionarErroValidacao(msgErro);
                return View("CreateEditGrupo", model);
            }

            await _produtoService.Salvar();
          
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexGrupo");
        }

        [Route("grupo/editar")]
        [ClaimsAuthorizeAttribute(2015)]
        public async Task<IActionResult> EditGrupo(long id)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditGrupo";
            var model = _mapper.Map <GrupoProdutoViewModel>(await _produtoService.ObterPorIdGrupo(id));
            if (model == null)
            {
                var msgErro = $"{_nomeEntidadeGrupo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeGrupo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexGrupo");
            }
            model.Empresas = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);

            return View("CreateEditGrupo", model);
        }

        [Route("grupo/editar")]
        [HttpPost]
        public async Task<IActionResult> EditGrupo(GrupoProdutoViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditGrupo";

            if (!ModelState.IsValid) return View("CreateEditGrupo", model);

            var objeto = _mapper.Map<GrupoProduto>(model);

            await _produtoService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AtualizarGrupo", "Web", Deserializar(objeto)));
                AdicionarErroValidacao(msgErro);
                return View("CreateEditGrupo", model);
            }

            await _produtoService.Salvar();

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexGrupo");
        }

        [Route("grupo/apagar")]
        [ClaimsAuthorizeAttribute(2013)]
        public async Task<IActionResult> DeleteGrupo(long id)
        {

            var model = _mapper.Map<GrupoProdutoViewModel>(await _produtoService.ObterPorIdGrupo(id));
            if (model == null)
            {
                var msgErro = $"{_nomeEntidadeGrupo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeGrupo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexGrupo");
            }
            model.Empresas = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);
            return View(model);
        }

        [Route("grupo/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteGrupo(GrupoProdutoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!_produtoService.ApagarProdutoGrupo(model.Id).Result)
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));

                NotificarErro(msgErro);
                return View(model);
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "ApagarGrupo", "Web", $"id:{model.Id}"));
                return View(model);
            }
            await _produtoService.Salvar();

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexGrupo");
        }
        #endregion

        #region SubGrupo
        [Route("subgrupo/lista")]
        public async Task<IActionResult> IndexSubGrupo([FromQuery] long idGrupo, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var lista = (await ObterSubGrupo(idGrupo, q, page, ps)); ;
            var modelGrupo = _mapper.Map<GrupoProdutoViewModel>(await _produtoService.ObterPorIdGrupo(idGrupo));
            ViewBag.Pesquisa = q;
            ViewBag.Grupo = modelGrupo != null ? modelGrupo.Nome : "";
            ViewBag.idGrupo = idGrupo;

            return View(lista);
        }

        [Route("subgrupo/novo")]
        public async Task<IActionResult> CreateSubGrupo(long idGrupo)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateSubGrupo";
            var modelGrupo = _mapper.Map<GrupoProdutoViewModel>(await _produtoService.ObterPorIdGrupo(idGrupo));

            var model = new SubGrupoViewModel();
            model.Situacao = EAtivo.Ativo;
            model.IDGRUPO = idGrupo;
            model.NomeGrupo = modelGrupo != null ? modelGrupo.Nome : "";

            return View("CreateEditSubGrupo", model);
        }

        [Route("subgrupo/novo")]
        [HttpPost]
        public async Task<IActionResult> CreateSubGrupo(SubGrupoViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateSubGrupo";
            if (!ModelState.IsValid) return View("CreateEditSubGrupo", model);

            var objeto = _mapper.Map<SubGrupoProduto>(model);

            if (objeto.Id == 0) objeto.Id = objeto.GerarId();
            await _produtoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AdicionarGrupo", "Web", Deserializar(objeto)));
                return View("CreateEditSubGrupo", model);
            }

            await _produtoService.Salvar();
                       
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";
            LogInformacao($"sucesso:{Deserializar(objeto)}", "Produto", "AdicionarGrupo", null);

            return RedirectToAction("IndexSubGrupo", new { idGrupo = model.IDGRUPO });
        }

        [Route("subgrupo/editar")]
        public async Task<IActionResult> EditSubGrupo(long id)
        {
            if (id ==0)
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AtualizarSubGrupo", "Web", id.ToString())); 
                AdicionarErroValidacao(msgErro);
                return RedirectToAction("IndexGrupo");
            }
            ViewBag.operacao = "E";
            ViewBag.acao = "EditSubGrupo";

            var model = _mapper.Map<SubGrupoViewModel>(await _produtoService.ObterPorIdSubGrupo(id));
            if(model == null)
            {
                var msgErro = $"{_nomeEntidadeSubGrupo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeSubGrupo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexSubGrupo");
            }    

            var modelGrupo = _mapper.Map<GrupoProdutoViewModel>(await _produtoService.ObterPorIdGrupo(model.IDGRUPO.Value));

            model.NomeGrupo = modelGrupo != null ? modelGrupo.Nome : "";
          

            return View("CreateEditSubGrupo", model);
        }

        [Route("subgrupo/editar")]
        [HttpPost]
        public async Task<IActionResult> EditSubGrupo(SubGrupoViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditSubGrupo";

            if (!ModelState.IsValid) return View("CreateEditSubGrupo", model);

            var objeto = _mapper.Map<SubGrupoProduto>(model);

            await _produtoService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AtualizarGrupo", "Web", Deserializar(objeto)));
                return View("CreateEditSubGrupo", model);
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso:{Deserializar(objeto)}", "Produto", "AtualizarGrupo", null);
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexSubGrupo", new { idGrupo = model.IDGRUPO });
        }

        [Route("subgrupo/apagar")]
        public async Task<IActionResult> DeleteSubGrupo(long id)
        {
            if (id == 0)
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "ApagarSubGrupo", "Web", id.ToString()));
                AdicionarErroValidacao(msgErro);
                return RedirectToAction("IndexGrupo");
            }

            var model = _mapper.Map<SubGrupoViewModel>(await _produtoService.ObterPorIdSubGrupo(id));
            var modelGrupo = _mapper.Map<GrupoProdutoViewModel>(await _produtoService.ObterPorIdGrupo(model.IDGRUPO.Value));
            model.NomeGrupo = modelGrupo != null ? modelGrupo.Nome : "";
            if (model == null)
            {
                var msgErro = $"{_nomeEntidadeSubGrupo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeSubGrupo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexSubGrupo");
            }

            return View(model);
        }

        [Route("subgrupo/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteSubGrupo(SubGrupoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!_produtoService.ApagarProdutoSubGrupo(model.Id).Result)
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));

                NotificarErro(msgErro);
                return View(model);
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "ApagarGrupo", "Web", $"id:{model.Id}"));
                return View(model);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso:id:{model.Id}", "Produto", "ApagarGrupo", null);
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexSubGrupo", new { idGrupo = model.IDGRUPO });
        }

        #endregion

        #region Departamento
        [Route("departamento/lista")]
        [ClaimsAuthorizeAttribute(2175)]
        public async Task<IActionResult> IndexDepartamentos([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidadeDepart}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidadeDepart;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeDepart;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = (await ObterPerfil(Convert.ToInt64(empresaSelecionada.IDEMPRESA), q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return View(lista);
        }

        [Route("departamento/novo")]
        [ClaimsAuthorizeAttribute(2176)]
        public async Task<IActionResult> CreateDepartamento()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateDepartamento";

            var model = new ProdutoDepartamentoViewModel();
            model.situacao = EAtivo.Ativo;
            model.Empresas = listaEmpresaViewModels.ToList();
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();
            if(empresaSelecionada != null && !string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
                model.idEmpresa = Convert.ToInt64(empresaSelecionada.IDEMPRESA);
            
            return View("CreateEditDepartamento", model);
        }

        [Route("departamento/novo")]
        [HttpPost]
        public async Task<IActionResult> CreateDepartamento(ProdutoDepartamentoViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateDepartamento";
            if (!ModelState.IsValid) return View("CreateEditDepartamento", model);

            var objeto = _mapper.Map<ProdutoDepartamento>(model);

            if (objeto.Id == 0) objeto.Id = objeto.GerarId();
            await _produtoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AdicionarDepartamento", "Web", Deserializar(objeto)));
                return View("CreateEditDepartamento", model);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso: {Deserializar(objeto)}", "Produto", "AdicionarDepartamento", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexDepartamentos");
        }

        [Route("departamento/editar")]
        [ClaimsAuthorizeAttribute(2179)]
        public async Task<IActionResult> EditDepartamento(long id)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditDepartamento";
            var model = await ObterDepartamentoCompletoPorId(id);
            if (model == null)
            {
                var msgErro = $"{_nomeEntidadeDepart} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeDepart;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexDepartamentos");
            }

            return View("CreateEditDepartamento", model);
        }

        [Route("departamento/editar")]
        [HttpPost]
        public async Task<IActionResult> EditDepartamento(ProdutoDepartamentoViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditDepartamento";

            if (!ModelState.IsValid) return View("CreateEditDepartamento", model);

            var objeto = _mapper.Map<ProdutoDepartamento>(model);

            await _produtoService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AtualizarDepartamento", "Web", Deserializar(objeto)));
                return View("CreateEditDepartamento", model);
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso: {Deserializar(objeto)}", "Produto", "AdicionarDepartamento", null);
         
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexDepartamentos");
        }

        [Route("departamento/apagar")]
        [ClaimsAuthorizeAttribute(2177)]
        public async Task<IActionResult> DeleteDepartamento(long id)
        {
            var model = await ObterDepartamentoCompletoPorId(id);
            if (model == null)
            {
                var msgErro = $"{_nomeEntidadeDepart} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeDepart;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexDepartamentos");
            }

            return View(model);
        }

        [Route("departamento/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteDepartamento(ProdutoDepartamentoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!_produtoService.ApagarDepartamento(model.Id).Result)
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));
                return View(model);
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "ApagarDepartamento", "Web", $"id:{model.Id}"));
                return View(model);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso: id:{model.Id}", "Produto", "ApagarDepartamento", null);
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexDepartamentos");
        }
        #endregion

        #region Marca

        [Route("marca/lista")]
        [ClaimsAuthorizeAttribute(2181)]
        public async Task<IActionResult> IndexMarcas([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidadeMarca}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidadeMarca;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeMarca;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = (await ObterMarcas(Convert.ToInt64(empresaSelecionada.IDEMPRESA), q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return View(lista);
        }


        [Route("marca/novo")]
        [ClaimsAuthorizeAttribute(2182)]
        public async Task<IActionResult> CreateMarca()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateMarca";

            var model = new ProdutoMarcaViewModel();
            model.situacao = EAtivo.Ativo;
            model.Empresas = listaEmpresaViewModels.ToList();

            var empresaSelecionada = ObterObjetoEmpresaSelecionada();
            
            if (empresaSelecionada != null && !string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
                model.idEmpresa = Convert.ToInt64(empresaSelecionada.IDEMPRESA);
            return View("CreateEditMarca", model);
        }

        [Route("marca/novo")]
        [HttpPost]
        public async Task<IActionResult> CreateMarca(ProdutoMarcaViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateMarca";
            if (!ModelState.IsValid) return View("CreateEditMarca", model);

            var objeto = _mapper.Map<ProdutoMarca>(model);

            if (objeto.Id == 0) objeto.Id = objeto.GerarId();
            await _produtoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AdicionarMarca", "Web", Deserializar(objeto)));
                return View("CreateEditMarca", model);
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso: {Deserializar(objeto)}", "Produto", "ApagarDepartamento", null);
           
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMarcas");
        }

        [Route("marca/editar")]
        [ClaimsAuthorizeAttribute(2185)]
        public async Task<IActionResult> EditMarca(long id)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditMarca";
            var objeto = await ObterMarcaCompletoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeDepart} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeDepart;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexMarcas");
            }

            return View("CreateEditMarca", objeto);
        }

        [Route("marca/editar")]
        [HttpPost]
        public async Task<IActionResult> EditMarca(ProdutoMarcaViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditMarca";

            if (!ModelState.IsValid) return View("CreateEditMarca", model);

            var objeto = _mapper.Map<ProdutoMarca>(model);

            await _produtoService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AtualizarMarca", "Web", Deserializar(objeto)));
                return View("CreateEditDepartamento", model);
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso:{Deserializar(objeto)}", "Produto", "AtualizarMarca", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMarcas");
        }

        [Route("marca/apagar")]
        [ClaimsAuthorizeAttribute(2183)]
        public async Task<IActionResult> DeleteMarca(long id)
        {
            var objeto = await ObterMarcaCompletoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeDepart} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeDepart;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexMarcas");
            }

            return View(objeto);
        }

        [Route("marca/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteMarca(ProdutoMarcaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!_produtoService.ApagarProdutoMarca(model.Id).Result)
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));

                NotificarErro(msgErro);
                return View(model);
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "ApagarMarca", "Web", $"id:{model.Id}"));
                return View(model);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso: id:{model.Id}", "Produto", "ApagarMarca", null);
           
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMarcas");
        }

        #endregion

        #region codigo de barras
        [Route("codigo-barra")]
        public async Task<IActionResult> ListaCodigoBarra(long idProduto)
        {
            var produto = Obter(idProduto.ToString()).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            var lista = ObterCodigoBarra(idProduto).Result;

            return View("_codigoBarra", lista);
        }

        [Route("codigo-barra/novo")]
        public async Task<IActionResult> AdicionarCodigoBarra(long idProduto)
        {
            ViewBag.acao = "AdicionarCodigoBarra";
            ViewBag.operacao = "I";

            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidadeGrupo}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidade;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            ProdutoCodigoBarraViewModel model = new ProdutoCodigoBarraViewModel();
            model.IDPRODUTO = idProduto;

            return View("_createEditCodigoBarra", model);
        }

        [Route("codigo-barra/novo")]
        [HttpPost]
        public async Task<IActionResult> AdicionarCodigoBarra(ProdutoCodigoBarraViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "AdicionarCodigoBarra";

            if (!ModelState.IsValid) return View("_createEditCodigoBarra", model);

            if (model.Id == 0) model.Id = await GerarId();

            var objeto = _mapper.Map<ProdutoCodigoBarra>(model);

            await _produtoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AdicionarCodigoBarra", "Web", Deserializar(objeto)));
                return View("_createEditCodigoBarra", model); 
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso:{Deserializar(objeto)}", "Produto", "AdicionarCodigoBarra", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaCodigoBarra", new { idProduto = model.IDPRODUTO });
        }

        [Route("codigo-barra/apagar")]
        public async Task<IActionResult> DeleteCodigoBarra(long id)
        {
            var objeto = await ObterCodigoBarraPorId(id.ToString());
            if (objeto == null)
            {
                var msgErro = $"Codigo de barra o produto não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Produto";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View(objeto);
        }

        [HttpPost]
        [Route("codigo-barra/apagar")]
        public async Task<IActionResult> DeleteCodigoBarra(ProdutoCodigoBarraViewModel model)
        {
            await _produtoService.ApagarCodigoBarra(model.Id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "ExcluirCodigoBarra", "Web", $"id:{model.Id}"));
                AdicionarErroValidacao(msgErro);
                return View(model);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso:id:{model.Id}", "Produto", "ExcluirCodigoBarra", null);
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaCodigoBarra", new { idProduto = model.IDPRODUTO });
        }

        #endregion

        #region Produto preço
        [Route("preco")]
        public async Task<IActionResult> ListaPreco(long idProduto)
        {
            var produto = Obter(idProduto.ToString()).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            var lista = _mapper.Map<List<ProdutoPrecoViewModel>>(_produtoService.ObterPrecoPorProduto(idProduto).Result);

            return View("_historicoPreco", lista);
        }
        #endregion

        #region Produto Estoque
        [Route("estoque")]
        public async Task<IActionResult> ListaEstoque(long idProduto)
        {
            var produto = Obter(idProduto.ToString()).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            var lista = ObterPreco(idProduto).Result;

            return View("_historicoEstoque", lista);
        }
        #endregion

        #region Estoque Historico
        [Route("estoque/historico")]
        public async Task<IActionResult> ListaEstoqueHistorico(long idProduto)
        {
            var produto = Obter(idProduto.ToString()).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            var lista = ObterHistoricoPorIdProduto(idProduto).Result;

            return View("_historicoMovEstoque", lista);
        }
        #endregion

        #region Cliente preço
        [Route("precos/cliente")]
        public async Task<IActionResult> ListaClientePreco(long idProduto)
        {
            var produto = Obter(idProduto.ToString()).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            var precos = _mapper.Map<List<ClientePrecoViewModel>>( _clienteService.ObterClientePrecoPorProduto(idProduto).Result);


            precos.ToList().ForEach(preco => {
                var cliente = _mapper.Map<ClienteViewModel>(_clienteService.ObterPorId(preco.IDCLIENTE).Result);
                preco.NomeCliente = cliente.Nome;
                preco.ValorFinal = produto.Preco.HasValue ? produto.Preco.Value : 0;

                if (preco.Diferenca == 1)//Enums.ETpDiferencaPreco.Acrescimo)
                {
                    preco.DescricaoTipoDiferenca = ETpDiferencaPreco.Acrescimo;
                    if (preco.TipoValor == 1)//Enums.ETipoValorPreco.Percentual)
                    {
                        preco.DescricaoTipoValor = ETipoValorPreco.Percentual;
                        preco.ValorFinal += (preco.ValorFinal * preco.Valor / 100);
                    }
                    else
                    {
                        preco.DescricaoTipoValor = ETipoValorPreco.Valor;
                        preco.ValorFinal += preco.Valor;
                    }
                }
                else
                {
                    preco.DescricaoTipoDiferenca = ETpDiferencaPreco.Desconto;
                    if (preco.TipoValor == 1)//Enums.ETipoValorPreco.Percentual)
                    {
                        preco.DescricaoTipoValor = ETipoValorPreco.Percentual;
                        preco.ValorFinal -= (preco.ValorFinal * preco.Valor / 100);
                    }
                    else
                    {
                        preco.DescricaoTipoValor = ETipoValorPreco.Valor;
                        preco.ValorFinal -= preco.Valor;
                    }
                }
            });

            return View("_clientePreco", precos);
        }


        [Route("preco/cliente")]
        public async Task<IActionResult> AdicionarPrecoCliente(long idProduto)
        {
            var produto = _mapper.Map<ProdutoViewModel>( _produtoService.ObterPorId(idProduto).Result);

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            ViewBag.acao = "AdicionarPrecoCliente";
            ViewBag.operacao = "I";

            var model = new ClientePrecoViewModel();
            model.IDPRODUTO = idProduto;
            model.Datahora = DateTime.Now;
            model.TipoValor = 1;
            model.Diferenca = 1;
            model.Clientes = ObterListaClientes();

            return View("_createEditClientePreco", model);
        }

        [Route("preco/cliente")]
        [HttpPost]
        public async Task<IActionResult> AdicionarPrecoCliente(ClientePrecoViewModel model)
        {
            var produto = _mapper.Map<ProdutoViewModel>(_produtoService.ObterPorId(model.IDPRODUTO).Result);

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = model.IDPRODUTO;

            ViewBag.operacao = "I";
            ViewBag.acao = "AdicionarPrecoCliente";

            if (!ModelState.IsValid) return View("_createEditClientePreco", model);

            model.Diferenca = model.DescricaoTipoDiferenca == ETpDiferencaPreco.Desconto ? 1 : 2;
            model.TipoValor = model.DescricaoTipoValor == ETipoValorPreco.Percentual ? 1 : 2;
            model.Datahora = DateTime.Now;
            if (model.Id == 0) model.Id = await GerarId();
            if (!ValidarValorPrecoCliente(produto.Preco.Value, model.Valor, model.DescricaoTipoDiferenca, model.DescricaoTipoValor))
            {
                AdicionarErroValidacao("O valor adicionado não pode, transformar o valor final do produto em valor menor que zero!");
                return View("_createEditClientePreco", model);
            }
            var objeto = _mapper.Map<ClientePreco>(model);

            await _clienteService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Cliente", "AdicionarPreco", "Web");
                return View("_createEditClientePreco", model);
            }

            await _clienteService.Salvar();
            LogInformacao($"Objeto adicionado com sucesso {Deserializar(objeto)}", "Cliente", "AtualiarContato", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaClientePreco", new { idProduto = model.IDPRODUTO });
        }

        [Route("preco/cliente/apagar")]
        public async Task<IActionResult> DeleteClientePreco(long id)
        {

            var objeto = _mapper.Map<ClientePrecoViewModel>(await _clienteService.ObteClientePrecoPorId(id));
            if (objeto == null)
            {
                var msgErro = $"Preço do Cliente não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Preço por cliente";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            var produto = _mapper.Map<ProdutoViewModel>( _produtoService.ObterPorId(objeto.IDPRODUTO).Result);

            objeto.DescricaoTipoDiferenca = objeto.Diferenca == 1 ? ETpDiferencaPreco.Desconto : ETpDiferencaPreco.Acrescimo;
            objeto.DescricaoTipoValor = objeto.TipoValor == 1 ? ETipoValorPreco.Percentual : ETipoValorPreco.Valor;

            objeto.Clientes = ObterListaClientes();

            ViewBag.NomeProduto = produto.Nome;

            return View(objeto);
        }

        [HttpPost]
        [Route("preco/cliente/apagar")]
        public async Task<IActionResult> DeleteClientePreco(ProdutoFotoViewModel model)
        {
            await _clienteService.Remover(model.Id);
            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar preço do produto por cliente" };
                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            await _clienteService.Salvar();
            LogInformacao($"Objeto excluido com sucesso id:{model.Id}", "Cliente", "ExcluirPreco", null);
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaClientePreco", new { idProduto = model.idProduto });
        }
        #endregion

        #region Turno Preco
        [Route("precos/turno")]
        public async Task<IActionResult> ListaTurnoPreco(long idProduto)
        {
             var produto = Obter(idProduto.ToString()).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            var precos = _mapper.Map<List<TurnoPrecoViewModel>>( _turnoService.ObterTurnoPrecoPorProduto(idProduto).Result);

            precos.ToList().ForEach(preco => {

                preco.ValorFinal = produto.Preco.HasValue ? produto.Preco.Value : 0;

                if (preco.Diferenca == 1)//Enums.ETpDiferencaPreco.Acrescimo)
                {
                    preco.DescricaoTipoDiferenca = ETpDiferencaPreco.Acrescimo;
                    if (preco.TipoValor == 1)//Enums.ETipoValorPreco.Percentual)
                    {
                        preco.DescricaoTipoValor = ETipoValorPreco.Percentual;
                        preco.ValorFinal += (preco.ValorFinal * preco.Valor / 100);
                    }
                    else
                    {
                        preco.DescricaoTipoValor = ETipoValorPreco.Valor;
                        preco.ValorFinal += preco.Valor;
                    }
                }
                else
                {
                    preco.DescricaoTipoDiferenca = ETpDiferencaPreco.Desconto;
                    if (preco.TipoValor == 1)//Enums.ETipoValorPreco.Percentual)
                    {
                        preco.DescricaoTipoValor = ETipoValorPreco.Percentual;
                        preco.ValorFinal -= (preco.ValorFinal * preco.Valor / 100);
                    }
                    else
                    {
                        preco.DescricaoTipoValor = ETipoValorPreco.Valor;
                        preco.ValorFinal -= preco.Valor;
                    }
                }
            });

            return View("_turnoPreco", precos);
        }

        [Route("preco/turno")]
        public async Task<IActionResult> AdicionarPrecoTurno(long idProduto)
        {
            var produto = Obter(idProduto.ToString()).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            ViewBag.acao = "AdicionarPrecoTurno";
            ViewBag.operacao = "I";

            var model = new TurnoPrecoViewModel();
            model.IDPRODUTO = idProduto;
            model.DataHora = DateTime.Now;
            model.TipoValor = 1;
            model.Diferenca = 1;

            return View("_createEditTurnoPreco", model);
        }

        [Route("preco/turno")]
        [HttpPost]
        public async Task<IActionResult> AdicionarPrecoTurno(TurnoPrecoViewModel model)
        {
            var produto = Obter(model.IDPRODUTO.ToString()).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = model.IDPRODUTO;

            ViewBag.operacao = "I";
            ViewBag.acao = "AdicionarPrecoTurno";

            if (!ModelState.IsValid) return View("_createEditTurnoPreco", model);

            model.Diferenca = model.DescricaoTipoDiferenca == ETpDiferencaPreco.Desconto ? 1 : 2;
            model.TipoValor = model.DescricaoTipoValor == ETipoValorPreco.Percentual ? 1 : 2;
            model.DataHora = DateTime.Now;
            if (!ValidarValorPrecoCliente(produto.Preco.Value, model.Valor, model.DescricaoTipoDiferenca, model.DescricaoTipoValor))
            {
                AdicionarErroValidacao("O valor adicionado não pode, transformar o valor final do produto em valor menor que zero!");
                return View("_createEditTurnoPreco", model);
            }
            if (model.Id == 0) model.Id = await GerarId();
            if (model.DataHora == null) model.DataHora = DateTime.Now;

            if (string.IsNullOrEmpty(model.Usuario)) model.Usuario = AppUser.GetUserEmail();

            var objeto = _mapper.Map<TurnoPreco>(model);

            await _turnoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Turno", "AdicionarPreco", "Web", $"{Deserializar(objeto)}"));
                AdicionarErroValidacao(msgErro);
                return View("_createEditTurnoPreco", model);
            }

            await _turnoService.Salvar();
            LogInformacao($"sucesso{Deserializar(objeto)}", "Turno", "AdicionarPreco", null);
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaTurnoPreco", new { idProduto = model.IDPRODUTO });
        }

        [Route("preco/turno/apagar")]
        public async Task<IActionResult> DeleteTurnoPreco(long id)
        {

            var objeto = _mapper.Map<TurnoPrecoViewModel>( await _turnoService.ObteClientePrecoPorId(id));
            if (objeto == null)
            {
                var msgErro = $"Preço do Turno não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Preço por cliente";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            var produto = Obter(objeto.IDPRODUTO.ToString()).Result;

            objeto.DescricaoTipoDiferenca = objeto.Diferenca == 1 ? ETpDiferencaPreco.Desconto : ETpDiferencaPreco.Acrescimo;
            objeto.DescricaoTipoValor = objeto.TipoValor == 1 ? ETipoValorPreco.Percentual : ETipoValorPreco.Valor;

            objeto.Clientes = ObterListaClientes();

            ViewBag.NomeProduto = produto.Nome;

            return View(objeto);
        }

        [HttpPost]
        [Route("preco/turno/apagar")]
        public async Task<IActionResult> DeleteTurnoPreco(TurnoPrecoViewModel model)
        {
            await _turnoService.Remover(model.Id);
            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar preço do produto por turno" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            await _turnoService.Salvar();

            LogInformacao($"sucesso id:{model.Id}", "Turno", "ExcluirPreco", null);
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaTurnoPreco", new { idProduto = model.IDPRODUTO });
        }
        #endregion

        #region Produto Foto  

        [Route("foto")]
        public async Task<IActionResult> ListaFoto(long idProduto)
        {
            var produto = Obter(idProduto.ToString()).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;
            
            var listaConvertida = new List<ProdutoFotoViewModel>();
            _produtoService.ObterFotoPorProduto(idProduto).Result.ToList()
                .ForEach(foto => {
                    var model = new ProdutoFotoViewModel();
                    model.idProduto = idProduto;
                    model.Data = foto.Data;
                    model.Descricao = foto.Descricao;
                    model.Id = foto.Id;
                    if (foto.Foto != null)
                        model.FotoConvertida = foto.Foto;

                     listaConvertida.Add(model);
                });

            return View("_fotoProduto", listaConvertida);
        }

        [Route("foto/novo")]
        public async Task<IActionResult> AdicionarFoto(long idProduto)
        {
            var produto = Obter(idProduto.ToString()).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            ViewBag.acao = "AdicionarFoto";
            ViewBag.operacao = "I";

            var model = new ProdutoFotoViewModel();
            model.idProduto = idProduto;
            model.Data = DateTime.Now;

            return View("_createEditFoto", model);
        }

        [Route("foto/novo")]
        [HttpPost]
        public async Task<IActionResult> AdicionarFoto(ProdutoFotoViewModel model)
        {
            var produto = Obter(model.idProduto.ToString()).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = model.idProduto;

            ViewBag.operacao = "I";
            ViewBag.acao = "AdicionarFoto";
            if (model.Foto == null)
            {
                var msgErro = "A seleção de uma imagem é obrigatoria";
                AdicionarErroValidacao(msgErro);

                TempData["TipoMensagem"] = "danger";
                TempData["Mensagem"] = msgErro;

                return View("_createEditFoto", model);
            }

            if (model.Foto.Length > 1048576)
            {
                var msgErro = "A imagem selecionada deve possuir tamanho de até 1 MB ";
                AdicionarErroValidacao(msgErro);

                TempData["TipoMensagem"] = "danger";
                TempData["Mensagem"] = msgErro;

                return View("_createEditFoto", model);
            }

            if (!ModelState.IsValid) return View("_createEditFoto", model);

            var objeto = ConverterParaProdutoFoto(model);
            objeto.AdicionarId();
            objeto.AdiconarFoto(ConverterFormFileToByte(model.Foto));

            await _produtoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AtualizarFoto", "Web", Deserializar(model)));
                return View("_createEditFoto", model);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso:{Deserializar(objeto)}", "Produto", "AtualizarFoto", null);
           
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaFoto", new { idProduto = model.idProduto });
        }

        [Route("foto/apagar")]
        public async Task<IActionResult> DeleteFoto(long id)
        {

            var objeto = _mapper.Map<ProdutoFotoViewModel>(await _produtoService.ObterFotoPorId(id));
            if (objeto == null)
            {
                var msgErro = $"Foto do produto não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Produto";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            var produto = await Obter(objeto.idProduto.ToString());

            ViewBag.NomeProduto = produto.Nome;

            return View(objeto);
        }

        [HttpPost]
        [Route("foto/apagar")]
        public async Task<IActionResult> DeleteFoto(ProdutoFotoViewModel model)
        {
            await _produtoService.ApagarFoto(model.Id);
            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar foto do produto" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso:id:{model.Id}", "Produto", "ExcluirFoto", null);
          
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaFoto", new { idProduto = model.idProduto });
        }

        protected byte[] ConverterFormFileToByte(IFormFile formFile)
        {
            long length = formFile.Length;
            if (length < 0)
                return null;

            using var fileStream = formFile.OpenReadStream();
            byte[] bytes = new byte[length];
            fileStream.Read(bytes, 0, (int)formFile.Length);

            return bytes;
        }

        private ProdutoFoto ConverterParaProdutoFoto(ProdutoFotoViewModel model)
        {
            return new ProdutoFoto(model.idProduto, model.Descricao, model.Data);
        }
        #endregion

        #region IBPT
        [Route("ibpt/atualizar")]
        [ClaimsAuthorizeAttribute(2060)]
        public async Task<IActionResult> AtualizarIBPT()
        {
           
            try
            {
                await _produtoService.AtualizarIBPTTodosProdutos();
                var msgResultado = "Produtos atualizados com sucesso!";

                TempData["Mensagem"] = msgResultado;
                TempData["TipoMensagem"] = "success";
            }
            catch
            {
                NotificarErro("Erro ao tentar atualizar IBPT dos produtos");

            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Atualizacao de IBPT", "AtualizarIBPT", "Web"));
                AdicionarErroValidacao(msgErro);
            }

            LogInformacao($"Atualizacao de IBPT", "Produto", "AtualizarIBPT", null);

            return RedirectToAction("Index");
        }

        #endregion

        #region metodos privados
        public async Task<ListasAuxiliaresProdutoViewModel> ObterListasAuxiliaresProdutos()
        {
            var produtos = new ListasAuxiliaresProdutoViewModel();
            produtos.Grupos = _mapper.Map<List<GrupoProdutoViewModel>>(_produtoService.ObterTodosGrupos().Result.ToList());
            produtos.SubGrupos = _mapper.Map<List<SubGrupoViewModel>>(_produtoService.ObterTodosSubGrupos().Result.ToList());
            produtos.Marcas = _mapper.Map<List<ProdutoMarcaViewModel>>(_produtoService.ObterTodosMarca().Result.ToList());
            produtos.Departamentos = _mapper.Map<List<ProdutoDepartamentoViewModel>>(_produtoService.ObterTodosDepartamento().Result.ToList());
            return produtos ;
        }

        private void PreencherListaAxuliaresProdutos(ProdutoViewModel viewModel)
        {
            viewModel.SubGrupos = SubGrupos;
            viewModel.Grupos = Grupos;
            viewModel.Marcas = Marcas;
            viewModel.Departamentos = Departamentos;
            viewModel.Unidades = Unidades;
            viewModel.Cfops = Cfops;
            viewModel.Cests = Cests;
            viewModel.Csosn = Csosn;
            viewModel.Csts = Csts;
            viewModel.Empresas = listaEmpresaViewModels.ToList();
        }

        private async Task<PagedViewModel<GrupoProdutoViewModel>> ObterGrupo(long idempresa, string filtro, int page, int pageSize)
        {
            var retorno = await _produtoService.ObterGrupoPaginacaoPorDescricao(idempresa, filtro, page, pageSize);
            var listaTeste = retorno.List;
            var lista = _mapper.Map<IEnumerable<GrupoProdutoViewModel>>(listaTeste);

            return new PagedViewModel<GrupoProdutoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "Index",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<PagedViewModel<SubGrupoViewModel>> ObterSubGrupo(long idGrupo, string filtro, int page, int pageSize)
        {
            var retorno = await _produtoService.ObterSubGrupoPaginacaoPorDescricao(idGrupo, filtro, page, pageSize);
            var listaTeste = retorno.List;
            var lista = _mapper.Map<IEnumerable<SubGrupoViewModel>>(listaTeste);

            return new PagedViewModel<SubGrupoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "IndexGrupo",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<PagedViewModel<ProdutoDepartamentoViewModel>> ObterPerfil(long idempresa, string filtro, int page, int pageSize)
        {
            var retorno = await _produtoService.ObterPaginacaoPorDescricao(idempresa, filtro, page, pageSize);
            var listaTeste = retorno.List;
            var lista = _mapper.Map<IEnumerable<ProdutoDepartamentoViewModel>>(listaTeste);

            return new PagedViewModel<ProdutoDepartamentoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "Index",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<ProdutoDepartamentoViewModel> ObterDepartamentoCompletoPorId(long id)
        {
            var departamentos = _produtoService.ObterPorIdDepartamento(id).Result;

            var model = _mapper.Map<ProdutoDepartamentoViewModel>(departamentos);
            model.Empresas = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);

            return model;
        }

        private async Task<PagedViewModel<ProdutoMarcaViewModel>> ObterMarcas(long idempresa, string filtro, int page, int pageSize)
        {
            var retorno = await _produtoService.ObterMarcaPaginacaoPorDescricao(idempresa, filtro, page, pageSize);
            var listaTeste = retorno.List;
            var lista = _mapper.Map<IEnumerable<ProdutoMarcaViewModel>>(listaTeste);

            return new PagedViewModel<ProdutoMarcaViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<ProdutoMarcaViewModel> ObterMarcaCompletoPorId(long id)
        {
            var objeto = _produtoService.ObterPorIdMarca(id).Result;

            var model = _mapper.Map<ProdutoMarcaViewModel>(objeto);
            model.Empresas = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);

            return model;
        }

        private async Task<PagedViewModel<ProdutoViewModel>> ObterListaProdutoPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _produtoService.ObterPorPaginacao(idEmpresa, filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<ProdutoViewModel>>(retorno.List);
            //lista.ToList().ForEach(x => {
            //    var unidadeCompra = Unidades.FirstOrDefault(y => y.Id.ToString() == x.UnidadeCompra);
            //    var unidadeVenda = Unidades.FirstOrDefault(y => y.Id.ToString() == x.UnidadeVenda);
            //    x.DescricaoUnidadeCompra = unidadeCompra != null ? unidadeCompra.Descricao : "";
            //    x.DescricaoUnidadeVenda = unidadeVenda != null ? unidadeVenda.Descricao : "";
            //});

            return new PagedViewModel<ProdutoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "lista" +
                "",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<ProdutoViewModel> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var produto = await _produtoService.ObterCompletoPorId(_id);
            return _mapper.Map<ProdutoViewModel>(produto);       
        }

        private async Task<TabelasAxuliaresFiscalViewModel> ObterTabelasAuxiliares()
        {
            var objeto = new TabelasAxuliaresFiscalViewModel();
            objeto.Cests = _mapper.Map<List<CestViewModel>>(await _tabelaAuxiliarFiscalService.ObterTodosCestNcm());
            objeto.Csosn = _mapper.Map<List<CsosnViewModel>>(await _tabelaAuxiliarFiscalService.ObterTodosCsosn());
            objeto.Csts = _mapper.Map<List<CstViewModel>>(await _tabelaAuxiliarFiscalService.ObterTodosCst());
            objeto.Cfops = _mapper.Map<List<CfopViewModel>>(await _tabelaAuxiliarFiscalService.ObterTodosCfop());

            return objeto;
        }

        private async Task<List<ProdutoCodigoBarraViewModel>> ObterCodigoBarra(long idProduto)
        {
            var codigoBarra = _produtoService.ObterTodosCodigoBarraPorProduto(idProduto).Result;

            var model = _mapper.Map<List<ProdutoCodigoBarraViewModel>>(codigoBarra);

            return model;
        }

        private async Task<ProdutoCodigoBarraViewModel> ObterCodigoBarraPorId(string id)
        {
            long _id = Convert.ToInt64(id);
            var produto = await _produtoService.ObterCodigoBarraPorId(_id);
            
                var objeto = _mapper.Map<ProdutoCodigoBarraViewModel>(produto);
                return objeto;
        }

        private async Task<List<EstoqueProdutoListaViewModel>> ObterPreco(long idProduto)
        {
            var lista = _estoqueService.ObterProdutoEstoquePorProduto(idProduto).Result;
            Estoque estoque = new Estoque(); ;

            var model = new List<EstoqueProdutoListaViewModel>();

            lista.ForEach(est => {
                if (est.IDESTOQUE != estoque.Id)
                    estoque = _estoqueService.ObterPorId(est.IDESTOQUE.Value).Result;

                var estoqueProduto = new EstoqueProdutoListaViewModel()
                {
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

            return model;
        }

        private async Task<List<EstoqueHistoricoViewModel>> ObterHistoricoPorIdProduto(long idProduto)
        {
            var objeto = _mapper.Map<List<EstoqueHistoricoViewModel>>(await _estoqueService.ObterHistoricoEstoquePorProduto(idProduto));
            return objeto;
        }

        private List<ClienteViewModel> ObterListaClientes()
        {
            return _mapper.Map<List<ClienteViewModel>>(_clienteService.ObterTodos().Result.ToList());
        }

        private bool ValidarValorPrecoCliente(double precoAtual, double precoNovo, ETpDiferencaPreco eTpDiferencaPreco, ETipoValorPreco eTipoValorPreco)
        {
            bool resultado = true;
            double valorFinal = precoAtual;
            if (eTpDiferencaPreco == ETpDiferencaPreco.Acrescimo)
            {
                if (eTipoValorPreco == ETipoValorPreco.Percentual)
                {
                    valorFinal += (valorFinal * precoNovo / 100);
                }
                else
                {
                    valorFinal += precoNovo;
                }
            }
            else
            {
                if (eTipoValorPreco == ETipoValorPreco.Percentual)
                {
                    valorFinal -= (valorFinal * precoNovo / 100);
                }
                else
                {
                    valorFinal -= precoNovo;
                }
            }

            return (valorFinal >= 0);
        }
        #endregion

    }
}

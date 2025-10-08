using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Services;
using agilium_manager_azure_business.Interfaces.IService;
using agilum.mvc.web.Data;
using agilum.mvc.web.Enums;
using agilum.mvc.web.Extensions;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Estoque;
using agilum.mvc.web.ViewModels.Perda;
using agilum.mvc.web.ViewModels.Produtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Route("perda")]
    [Authorize]
    public class PerdaController : MainController
    {
        private readonly IPerdaService _perdaService;
        private readonly IProdutoService _produtoService;
        private readonly IEmpresaService _empresaService;
        private readonly IEstoqueService _estoqueService;
        private readonly IUsuarioService _usuarioService;
        private readonly IPerdaDapperRepository _perdaDapperRepository;

        private readonly string _nomeEntidadeMotivo = "Perda/Sobra";

        #region construtores
        public PerdaController(
            INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, 
            ILogService logService, IMapper mapper, IPerdaService perdaService, IProdutoService produtoService, 
            IEmpresaService empresaService, IEstoqueService estoqueService, IUsuarioService usuarioService,
            IPerdaDapperRepository perdaDapperRepository, ILicencaService licencaService, SignInManager<AppUserAgiliumIdentity> signInManager) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper, licencaService, signInManager)
        {
            _perdaService = perdaService;
            _produtoService = produtoService;
            _empresaService = empresaService;
            _estoqueService = estoqueService;
            _usuarioService = usuarioService;
            _perdaDapperRepository = perdaDapperRepository;
        }
        #endregion

        #region Listas Auxiliares

        private void PopularListaAuxiliares(PerdaViewModel valeViewModel, long idEmpresa)
        {
            if (listaEmpresaViewModels.Count == 0)
                listaEmpresaViewModels = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result.ToList());
            if (listaEstoqueViewModels.Count == 0)
                listaEstoqueViewModels = _mapper.Map<List<EstoqueViewModel>>( _estoqueService.ObterTodas().Result.ToList());
            if (listaprodutoViewModels.Count == 0)
                listaprodutoViewModels = _mapper.Map<List<ProdutoViewModel>>(_produtoService.ObterTodosProdutos_IdDescricao(idEmpresa).Result.ToList());

            if (valeViewModel.Empresas.Count == 0)
                valeViewModel.Empresas = listaEmpresaViewModels;
            if (valeViewModel.Produtos.Count == 0)
                valeViewModel.Produtos = listaprodutoViewModels;
            if (valeViewModel.Estoques.Count == 0)
                valeViewModel.Estoques = listaEstoqueViewModels;
        }

        private List<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();
        private List<ProdutoViewModel> listaprodutoViewModels { get; set; } = new List<ProdutoViewModel>();
        private List<EstoqueViewModel> listaEstoqueViewModels { get; set; } = new List<EstoqueViewModel>();
        #endregion

        #region perda

        [Route("lista")]
        [ClaimsAuthorizeAttribute(2101)]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidadeMotivo}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidadeMotivo;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeMotivo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = (await ObterListaPaginado(Convert.ToInt64(empresaSelecionada.IDEMPRESA), q, page, ps));
            lista.ReferenceAction = "Index";
            lista.Query = q;
            ViewBag.Pesquisa = q;

            return View(lista);
        }

        [Route("novo")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2102)]
        public async Task<IActionResult> Create()
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidadeMotivo}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidadeMotivo;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeMotivo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";

            var model = new PerdaViewModel();
            model.Movimento = ETipoMovimentoPerda.Perda;
            model.Tipo = ETipoPerda.Quebra;
            model.DataHora = DateTime.Now;
            model.IDEMPRESA = Convert.ToInt64(empresaSelecionada.IDEMPRESA);
            model.Codigo = await _utilDapperRepository.GerarCodigo($"SELECT MAX(CAST(CDPERDA AS UNSIGNED)) AS CD FROM perda where IDEMPRESA={empresaSelecionada.IDEMPRESA}");

            model.Id = 0;
            PopularListaAuxiliares(model, Convert.ToInt64(empresaSelecionada.IDEMPRESA));
            return View("CreateEditPerda", model);
        }

        [Route("novo")]
        [HttpPost]
        [ClaimsAuthorizeAttribute(2102)]
        public async Task<IActionResult> Create(PerdaViewModel viewModel)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            PopularListaAuxiliares(viewModel, viewModel.IDEMPRESA.Value);
            if (!ModelState.IsValid) return View("CreateEditPerda", viewModel);

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
                if (produto != null && produto.VLCUSTOMEDIO.HasValue)
                    viewModel.ValorCustoMedio = produto.VLCUSTOMEDIO.Value;
            }

            var perda = _mapper.Map<Perda>(viewModel);

            await _perdaService.Adicionar(perda);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova perda/sobra" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditPerda", viewModel);
            }

            await _perdaService.Salvar();
            
            var id = _perdaDapperRepository.lancarPerdaRetornaIdHistoricoGerado(perda.Id, AppUser.GetUserEmail()).Result;
            LogInformacao($"Lançada perda/sobra Id: {perda.Id} - IdHistorico: {id} - Usuário: {AppUser.GetUserEmail()}","novo","Create",null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2105)]
        public async Task<IActionResult> Edit(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            var objeto = _mapper.Map<PerdaViewModel>(await _perdaService.ObterPorId(id));
            PopularListaAuxiliares(objeto, objeto.IDEMPRESA.Value);
            if (objeto == null)
            {
                var msgErro = $"perda/sobra não localizada";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "perda/sobra";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View("CreateEditPerda", objeto);
        }

        [Route("editar")]
        [HttpPost]
        [ClaimsAuthorizeAttribute(2105)]
        public async Task<IActionResult> Edit(PerdaViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";
            PopularListaAuxiliares(model, model.IDEMPRESA.Value);
            if (!ModelState.IsValid) return View("CreateEditPerda", model);

            var perda = _mapper.Map<Perda>(model);

            await _perdaService.Atualizar(perda);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao editar perda/sobra" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditPerda", model);
            }

            await _perdaService.Salvar();
            LogInformacao($"Editada perda/sobra Id: {perda.Id} - Usuário: {AppUser.GetUserEmail()}", "editar", "Edit", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2103)]
        public async Task<IActionResult> Delete(long id)
        {
            var objeto = _mapper.Map<PerdaViewModel>(await _perdaService.ObterPorId(id));
            PopularListaAuxiliares(objeto, objeto.IDEMPRESA.Value);
            if (objeto == null)
            {
                var msgErro = $"perda/sobra não localizada";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Perda";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View(objeto);
        }

        [Route("apagar")]
        [HttpPost]
        [ClaimsAuthorizeAttribute(2103)]
        public async Task<IActionResult> Delete(PerdaViewModel model)
        {
            var objeto = _mapper.Map<PerdaViewModel>(await _perdaService.ObterPorId(model.Id));

            if(objeto  != null)
            {
                await _perdaService.Apagar(model.Id);
                if (!OperacaoValida())
                {
                    var retornoErro = new { mensagem = $"Erro ao tentar apagar perda/sobra" };

                    AdicionarErroValidacao(retornoErro.mensagem);
                    return View(model);
                }
                await _perdaService.Salvar();

            }
            PopularListaAuxiliares(model, model.IDEMPRESA.Value);
            LogInformacao($"Apagada perda/sobra Id: {model.Id} - Usuário: {AppUser.GetUserEmail()}", "apagar", "Delete", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }
        #endregion

        #region Private
        private async Task<PagedViewModel<PerdaViewModel>> ObterListaPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _perdaService.ObterValePorPaginacao(idEmpresa, filtro, page, pageSize);

            var listaPerdaViewModel = new List<PerdaViewModel>();

            retorno.List.ToList().ForEach(perda => {
                var perdaViewModel = _mapper.Map<PerdaViewModel>(perda);
                perdaViewModel.EmpresaNome = perda.Empresa != null && !string.IsNullOrEmpty(perda.Empresa.NMRZSOCIAL) ? perda.Empresa.NMRZSOCIAL : string.Empty;
                perdaViewModel.ProdutoNome = perda.Produto != null && !string.IsNullOrEmpty(perda.Produto.NMPRODUTO) ? perda.Produto.NMPRODUTO : string.Empty;
                perdaViewModel.UsuarioNome = perda.Usuario != null && !string.IsNullOrEmpty(perda.Usuario.nome) ? perda.Usuario.nome : string.Empty;
                perdaViewModel.EstoqueHistoricoNome = perda.EstoqueHistorico != null && !string.IsNullOrEmpty(perda.EstoqueHistorico.DSHST) ? perda.EstoqueHistorico.DSHST : string.Empty;
                perdaViewModel.EstoqueNome = perda.Estoque != null && !string.IsNullOrEmpty(perda.Estoque.Descricao) ? perda.Estoque.Descricao : string.Empty;

                listaPerdaViewModel.Add(perdaViewModel);
            });

            return new PagedViewModel<PerdaViewModel>()
            {
                List = listaPerdaViewModel,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "Index",
                TotalResults = retorno.TotalResults
            };
        }
        #endregion
    }
}

using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Services;
using agilium_manager_azure_business.Interfaces.IService;
using agilum.mvc.web.Data;
using agilum.mvc.web.Extensions;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Estoque;
using agilum.mvc.web.ViewModels.Inventario;
using agilum.mvc.web.ViewModels.Produtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{

    [Route("inventario")]
    [Authorize]
    public class InventarioController : MainController
    {
        private readonly IInventarioService _inventarioService;
        private readonly IEmpresaService _empresaService;
        private readonly IProdutoService _produtoService;
        private readonly IEstoqueService _estoqueService;
        private readonly IPerdaService _perdaService;
        private readonly IUsuarioService _usuarioService;

        private readonly string _nomeEntidadeMotivo = "Inventario";

        #region construtor
        public InventarioController(IInventarioService inventarioService, IEmpresaService empresaService,
                                    IProdutoService produtoService, IEstoqueService estoqueService, IPerdaService perdaService,
                                    INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository,
                                    IUsuarioService usuarioService,
                                    ILogService logService, IMapper mapper, ILicencaService licencaService, SignInManager<AppUserAgiliumIdentity> signInManager) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper, licencaService, signInManager)
        {
            _inventarioService = inventarioService;
            _empresaService = empresaService;
            _produtoService = produtoService;
            _estoqueService = estoqueService;
            _perdaService = perdaService;
            _usuarioService = usuarioService;
        }
        #endregion

        #region lIstas Auxiliares
        private void PopularListasAuxiliares(InventarioViewModel model)
        {
            if (model.Empresas.Count() == 0)
            {
                var empresas = _empresaService.ObterTodas().Result;
                model.Empresas = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result.ToList());
            }

            if (model.Estoques.Count() == 0)
            {
                var estoques = _estoqueService.ObterTodas().Result;
                model.Estoques = _mapper.Map<List<EstoqueViewModel>>(_estoqueService.ObterTodas().Result.ToList());
            }
        }
        #endregion

        #region inventario

        [Route("lista")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2107)]
        public async Task<IActionResult> Index([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
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

            var lista = await ObterListaPaginado(Convert.ToInt64(empresaSelecionada.IDEMPRESA), q, page, ps);
            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "Index";
            lista.Query = q;
            return View(lista);
        }

        [Route("novo")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2108)]
        public async Task<ActionResult> Create()
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

            var model = new InventarioViewModel();
            model.Situacao = Enums.ESituacaoInventario.Aberta;
            model.Data = DateTime.Now;
            model.IDEMPRESA = Convert.ToInt64(empresaSelecionada.IDEMPRESA);

            model.Id = 0;
            PopularListasAuxiliares(model);
            return View("CreateEdit", model);
        }

        [Route("novo")]
        [HttpPost]
        [ClaimsAuthorizeAttribute(2108)]
        public async Task<IActionResult> Create(InventarioViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            PopularListasAuxiliares(model);
            if (!ModelState.IsValid) return View("CreateEdit", model);

            if (!model.Data.HasValue)
                model.Data = DateTime.Now;

            if (model.Id == 0) model.Id = await GerarId();

            var objeto = _mapper.Map<Inventario>(model);

            await _inventarioService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo inventario" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }
            await _inventarioService.Salvar();
                      
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2111)]
        public async Task<ActionResult> Edit(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            var model = _mapper.Map<InventarioViewModel>(await _inventarioService.ObterPorId(id));

            PopularListasAuxiliares(model);
            if (model == null)
            {
                var msgErro = $"Inventario não localizado";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Inventario";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View("CreateEdit", model);
        }

        [Route("editar")]
        [HttpPost]
        [ClaimsAuthorizeAttribute(2111)]
        public async Task<ActionResult> Edit(InventarioViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";
            PopularListasAuxiliares(model);

            if (!ModelState.IsValid) return View("CreateEdit", model);

            var objeto = _mapper.Map<Inventario>(model);

            await _inventarioService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao editar inventario" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }

            await _inventarioService.Salvar();

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2109)]
        public async Task<ActionResult> Cancelar(long id)
        {
            ;
            var objeto = _mapper.Map<InventarioViewModel>(await _inventarioService.ObterPorId(id));

            if (objeto == null)
            {
                var msgErro = $"Inventario não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Inventario";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            PopularListasAuxiliares(objeto);
            return View(objeto);
        }

        [Route("apagar")]
        [HttpPost]
        [ClaimsAuthorizeAttribute(2109)]
        public async Task<IActionResult> Cancelar(InventarioViewModel model)
        {

            if (!ModelState.IsValid) return View(model);

            var objeto = await _inventarioService.ObterPorId(model.Id);
            if(objeto == null)
            {
                NotificarErro("Erro ao tentar apagar inventario");
                PopularListasAuxiliares(model);
                var retornoErro = new { mensagem = $"Erro ao tentar apagar inventario" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model); 
            }

            objeto.Cancelar();
            await _inventarioService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                PopularListasAuxiliares(model);
                var retornoErro = new { mensagem = $"Erro ao tentar apagar inventario" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model); ;
            }
            await _inventarioService.Salvar();
           
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        #endregion

        #region Itens

        [Route("itens")]
        public async Task<IActionResult> IndexItem(long id)
        {

            var objeto = await _inventarioService.ObterPorId(id);

            if (objeto == null)
            {
                var msgErro = $"Inventario não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Inventario";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            var inventarioConvertido = await ConverterObjetoEmViewModel(objeto);
            var model = new ListaInventarioItemViewModel();
            
            var listaObjeto = _inventarioService.ObterItensPorInventario(objeto.Id).Result;
            
            listaObjeto.ForEach(async item => {
                var viewModel = await ConverterObjetoEmViewModel(item);
                model.Itens.Add(viewModel);
            });

            model.NomeInventario = inventarioConvertido.Descricao;
            model.Situacao = inventarioConvertido.Situacao.Value;
            model.idInventario = inventarioConvertido.Id;
            model.TipoAnalise = inventarioConvertido.TipoAnalise;

            return View("IndexItem", model);
        }

        [Route("editar-itens")]
        [ClaimsAuthorizeAttribute(2111)]
        public async Task<IActionResult> IndexItemEdit(long id)
        {

            var objeto = _mapper.Map<InventarioViewModel>(await _inventarioService.ObterPorId(id));

            if (objeto == null)
            {
                var msgErro = $"Inventario não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Inventario";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            if (objeto.Situacao != Enums.ESituacaoInventario.Aberta && objeto.Situacao != Enums.ESituacaoInventario.Execucao)
            {
                var msgErro = $"A situação do Inventario ser ser Aberto ou Em execução";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Inventario";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }


            var model = new ListaInventarioItemViewModel();
            
            var listaObjeto = _inventarioService.ObterItensPorInventario(id).Result;
            //var listaViewModel = new List<InventarioItemViewModel>();
            listaObjeto.ForEach(async item => {
                var viewModel = await ConverterObjetoEmViewModel(item);
                model.Itens.Add(viewModel);
            });

            model.NomeInventario = objeto.Descricao;
            model.Situacao = objeto.Situacao.Value;
            model.idInventario = objeto.Id;
            model.TipoAnalise = objeto.TipoAnalise;

            return View("ListItemEdit", model);
        }

        [Route("editar-itens")]
        [HttpPost]
        [ClaimsAuthorizeAttribute(2111)]
        public async Task<ActionResult> IndexItemEdit(ListaInventarioItemViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
            var itens = _mapper.Map<List<InventarioItem>>(model.Itens).ToList();
            await _inventarioService.AlterarInventarioItem(itens, usuario.Id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "ApuracaoInventario", "Web", Deserializar(model)));
                return View("ListItemEdit", model);
            }
          
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return View("ListItemEdit", model);
        }

        [Route("apagar-itens")]
        [ClaimsAuthorizeAttribute(2118)]
        public async Task<IActionResult> DeleteItemInventario(long id)
        {
            var objeto = _mapper.Map<InventarioViewModel>(await _inventarioService.ObterPorId(id));

            if (objeto == null)
            {
                var msgErro = $"Inventario não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Inventario";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            if (objeto.Situacao != Enums.ESituacaoInventario.Aberta && objeto.Situacao != Enums.ESituacaoInventario.Execucao)
            {
                var msgErro = $"A situação do Inventario ser ser Aberto ou Em execução";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Inventario";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            var model = new ListaInventarioItemViewModel();
            var listaObjeto = _inventarioService.ObterItensPorInventario(id).Result;
            //var listaViewModel = new List<InventarioItemViewModel>();
            listaObjeto.ForEach(async item => {
                var viewModel = await ConverterObjetoEmViewModel(item);
                model.Itens.Add(viewModel);
            });
            model.NomeInventario = objeto.Descricao;
            model.Situacao = objeto.Situacao.Value;
            model.idInventario = objeto.Id;
            model.TipoAnalise = objeto.TipoAnalise;

            return View(model);
        }

        [Route("apagar-itens")]
        [HttpPost]
        [ClaimsAuthorizeAttribute(2118)]
        public async Task<IActionResult> DeleteItemInventario(ListaInventarioItemViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var itensSelecionados = new ListaInventarioItemViewModel()
            {
                idInventario = model.idInventario,
                NomeInventario = model.NomeInventario,
                Situacao = model.Situacao
            };

            model.Itens.ForEach(item => {
                if (item.Selecionado)
                {
                    itensSelecionados.Itens.Add(item);
                }
            });

            var itens = _mapper.Map<List<InventarioItem>>(model.Itens).ToList();

            await _inventarioService.ApagarInventarioItem(itens);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "ApagarItemInventario", "Web", Deserializar(model)));
                return View(model);
            }

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexItem", new { id = model.idInventario });
        }

        [Route("CadastroAutomaticoProdutos")]
        [ClaimsAuthorizeAttribute(2119)]
        public async Task<IActionResult> CadastroAutomaticoProdutos(long id)
        {
            var objeto = await _inventarioService.ObterPorId(id);

            if (objeto == null) return NotFound();

            await _inventarioService.IncluirProdutosPorEstoque(objeto.IDESTOQUE.Value, objeto.Id);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "CadastrarProdutoPorEstoque", "Web", Deserializar(objeto)));
                return View("IndexItem", new { id = id });
            }
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            var url = Url.Action("IndexItem", "Inventario", new { id = id });

            return Json(new { success = true, url });

        }

        [Route("concluir")]
        [ClaimsAuthorizeAttribute(2114)]
        public async Task<IActionResult> concluir(long id)
        {
            var objeto =  await _inventarioService.ObterPorId(id);

            if (objeto == null)
            {
                var msgErro = $"Inventario não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Inventario";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            if (objeto.STINVENT != agilium.api.business.Enums.ESituacaoInventario.Execucao)
            {
                var msgErro = $"A situação do Inventario estar Em execução";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Inventario";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;

            await _inventarioService.ConcluirInventario(objeto.Id, objeto.STINVENT.Value, usuario.Id);
            var url = Url.Action("Index", "Inventario");
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "Concluir", "Web", Deserializar(objeto)));
                return Json(new { success = false, url, msgErro });
            }

            await _inventarioService.Salvar();
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return Json(new { success = true, url });
        }

        [Route("inventariar")]
        [ClaimsAuthorizeAttribute(2113)]
        public async Task<IActionResult> inventariar(long id)
        {
            var objeto = await _inventarioService.ObterPorId(id);

            if (objeto == null) return NotFound();

            objeto.Executar();
            await _inventarioService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "Inventariar", "Web", Deserializar(objeto)));
                return View("IndexItem", new { id = id });
            }
            await _inventarioService.Salvar();
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            var url = Url.Action("Index", "Inventario");

            return Json(new { success = true, url });

        }

        [Route("IncluirProdutosDisponiveisInventario")]
        [ClaimsAuthorizeAttribute(2117)]
        public async Task<ActionResult> IncluirProdutosDisponiveisInventario(long id)
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

            var objeto = _mapper.Map<InventarioViewModel>(await _inventarioService.ObterPorId(id));

            if (objeto == null)
            {
                var msgErro = $"Inventario não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Inventario";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            var objetos = await _inventarioService.ObetrProdutoDisponvelInventario(Convert.ToInt64(empresaSelecionada.IDEMPRESA), objeto.Id);
            var listaProdutos = _mapper.Map<List<ProdutoViewModel>>(objetos);

            var model = new AdicionarListaProdutosDisponiveisViewModel();
            model.idInventario = objeto.Id;
            model.IDEMPRESA = objeto.IDEMPRESA;
            model.NomeInventario = $"{objeto.Codigo} - {objeto.Descricao}";
            model.Situacao = objeto.Situacao;
            listaProdutos.ForEach(item => {
                model.Produtos.Add(new ProdutoDisponivelViewModel()
                {
                    Id = item.Id,
                    Categoria = item.Categoria,
                    Codigo = item.Codigo,
                    idEmpresa = item.idEmpresa,
                    IDGRUPO = item.IDGRUPO,
                    Nome = item.Nome,
                    Tipo = item.Tipo
                });
            });

            return View("AddProdutoDisp", model);
        }

        [Route("IncluirProdutosDisponiveisInventario")]
        [HttpPost]
        [ClaimsAuthorizeAttribute(2117)]
        public async Task<ActionResult> IncluirProdutosDisponiveisInventario(AdicionarListaProdutosDisponiveisViewModel model)
        {
            if (!ModelState.IsValid) return View("AddProdutoDisp", model);

            var itensSelecionados = new AdicionarListaProdutosDisponiveisViewModel()
            {
                IDEMPRESA = model.IDEMPRESA,
                idInventario = model.idInventario,
                NomeInventario = model.NomeInventario,
                Situacao = model.Situacao
            };

            model.Produtos.ForEach(item => {
                if (item.Selecionado)
                {
                    itensSelecionados.Produtos.Add(item);
                }
            });

            var itensInventario = new List<InventarioItem>();
            model.Produtos.ForEach(item => {
                var inventarioItem = new InventarioItem(model.idInventario, item.Id, null, null, null, null, null, null);
                itensInventario.Add(inventarioItem);
            });

            await _inventarioService.IncluirProdutoInventario(itensInventario);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "AdicionarProdutos", "Web", Deserializar(itensInventario)));
                return View("AddProdutoDisp", model);
            }

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexItem", new { id = model.idInventario });
        }

        public async Task<ActionResult> SalvarSelecao(long id)
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

            var objeto = _mapper.Map<InventarioViewModel>(await _inventarioService.ObterPorId(id));

            if (objeto == null)
            {
                var msgErro = $"Inventario não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Inventario";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            var objetos = await _inventarioService.ObetrProdutoDisponvelInventario(Convert.ToInt64(empresaSelecionada.IDEMPRESA), objeto.Id);
            var listaProdutos = _mapper.Map<List<ProdutoViewModel>>(objetos);

            var model = new AdicionarListaProdutosDisponiveisViewModel();
            model.idInventario = objeto.Id;
            model.IDEMPRESA = objeto.IDEMPRESA;
            model.NomeInventario = $"{objeto.Codigo} - {objeto.Descricao}";
            model.Situacao = objeto.Situacao;
            listaProdutos.ForEach(item => {
                model.Produtos.Add(new ProdutoDisponivelViewModel()
                {
                    Id = item.Id,
                    Categoria = item.Categoria,
                    Codigo = item.Codigo,
                    idEmpresa = item.idEmpresa,
                    IDGRUPO = item.IDGRUPO,
                    Nome = item.Nome,
                    Tipo = item.Tipo
                });
            });

            return View("_ProdutosDispInventarios",model);
        }

        [HttpPost]
        public async Task<ActionResult> SalvarSelecao(List<long> SelectedIds, string idInventario, string IDEMPRESA, string NomeInventario)
        {
            // Aqui você terá os IDs dos produtos selecionados
            // salvar no banco, processar, etc.
            var ids = SelectedIds;

            var itensInventario = new List<InventarioItem>();

            foreach(var id in SelectedIds)
            {
                var inventarioItem = new InventarioItem(Int64.Parse(idInventario), id, null, null, null, null, null, null);
                itensInventario.Add(inventarioItem);
            }
            
            await _inventarioService.IncluirProdutoInventario(itensInventario);

            if (!OperacaoValida())
            {
                var model = await PreencherObjetoModel(idInventario, IDEMPRESA, NomeInventario, SelectedIds);
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "AdicionarProdutos", "Web", Deserializar(itensInventario)));
                return View("_ProdutosDispInventarios", model);
            }

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexItem", new { id = idInventario });
        }

        private async Task<AdicionarListaProdutosDisponiveisViewModel> PreencherObjetoModel(string idInventario, string IDEMPRESA, string NomeInventario, List<long> SelectedIds)
        {
            var produtos = _mapper.Map<List<ProdutoViewModel>>(await _inventarioService.ObetrProdutoDisponvelInventario(Convert.ToInt64(IDEMPRESA), Convert.ToInt64(idInventario)));

            var retorno = new AdicionarListaProdutosDisponiveisViewModel();
            retorno.idInventario = Int64.Parse(idInventario);
            retorno.NomeInventario = NomeInventario;
            retorno.IDEMPRESA = Convert.ToInt64(IDEMPRESA);

            foreach (var item in produtos)
            {
                var select = SelectedIds.Any(x => x == item.Id);

                retorno.Produtos.Add(new ProdutoDisponivelViewModel()
                {
                    Id = item.Id,
                    Categoria = item.Categoria,
                    Codigo = item.Codigo,
                    idEmpresa = item.idEmpresa,
                    IDGRUPO = item.IDGRUPO,
                    Nome = item.Nome,
                    Tipo = item.Tipo,
                    Selecionado = select
                });
            }

            return retorno;
        }

        [Route("ApurarItens")]
        public IActionResult ApurarItens(long idInventario, int pagina = 1, int tamanhoPagina = 20)
        {
            var inventario = _inventarioService.ObterItensPorInventario(idInventario).Result;

            var totalItens = inventario.Count();

            var itens = inventario
                .OrderBy(x => x.Produto.CDPRODUTO)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToList();
            List<InventarioItemViewModel> listaItens = new List<InventarioItemViewModel>();
            itens.ForEach(async item =>
            {
                listaItens.Add(await ConverterObjetoEmViewModel(item));
            });

            var viewModel = new ListaInventarioItemViewModel
            {
                idInventario = idInventario,
                Itens = listaItens,
                PaginaAtual = pagina,
                TotalPaginas = (int)Math.Ceiling((double)totalItens / tamanhoPagina)
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("ApurarItens")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApurarItens(ListaInventarioItemViewModel model)
        {
            // 🔹 Validação
            if (!ModelState.IsValid)
                return View(model);

            // 🔹 Obtém o usuário logado
            var usuario = await _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString());

            // 🔹 Mapeia os itens do ViewModel para a entidade
            var itens = _mapper.Map<List<InventarioItem>>(model.Itens).ToList();

            // 🔹 Chama o serviço para atualizar os itens no inventário
            await _inventarioService.AlterarInventarioItem(itens, usuario.Id);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Inventario", "ApuracaoInventario", "Web", Deserializar(model)));
                return View("ListItemEdit", model);
            }

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            // 🔹 Redireciona para GET mantendo a paginação atual
            // Redireciona para a mesma página da lista
            return RedirectToAction(nameof(ApurarItens), new
            {
                idInventario = model.idInventario,
                pagina = model.PaginaAtual
            });
        }

        #endregion

        #region private
        private async Task<PagedViewModel<InventarioViewModel>> ObterListaPaginado(long idempresa, string descricao, int page, int pageSize)
        {
            var lista = new List<InventarioViewModel>();
            var retorno = await _inventarioService.ObterPorPaginacao(idempresa, descricao, page, pageSize);

            retorno.List.ToList().ForEach(async dev =>
            {
                InventarioViewModel viewModel = await ConverterObjetoEmViewModel(dev);

                lista.Add(viewModel);
            });
            return new PagedViewModel<InventarioViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "Index",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<PagedViewModel<InventarioItemViewModel>> ObterListaItemPaginado(long id, string descricao, int page, int pageSize)
        {
            var lista = new List<InventarioItemViewModel>();
            var retorno = await _inventarioService.ObterItensPorInventarioPaginacao(id, descricao, page, pageSize);

            retorno.List.ToList().ForEach(async dev =>
            {
                var viewModel = await ConverterObjetoEmViewModel(dev) ;

                lista.Add(viewModel);
            });
            return new PagedViewModel<InventarioItemViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "Index",
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
                viewModel.CodigoProduto = produto != null && !string.IsNullOrEmpty(produto.CDPRODUTO) ? produto.CDPRODUTO : "";

            }

            if (dev.IDUSUARIOANALISE.HasValue)
            {
                var usuarioAnalise = _usuarioService.ObterPorUsuarioPorId(dev.IDUSUARIOANALISE.Value).Result;
                viewModel.NomeUsuarioAnalise = usuarioAnalise != null && !string.IsNullOrEmpty(usuarioAnalise.nome) ? usuarioAnalise.nome : string.Empty;
            }

            return viewModel;
        }
        #endregion

    }
}

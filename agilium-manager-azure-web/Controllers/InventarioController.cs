using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels.InventarioViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    [Route("inventario")]
    public class InventarioController : MainController
    {
        private readonly IInventarioService _inventarioService;
        private readonly IEmpresaService _empresaService;
        private readonly IProdutoService _produtoService;
        private readonly IEstoqueService _estoqueService;
        private readonly IPerdaService _perdaService;
        public InventarioController(IInventarioService inventarioService, IEmpresaService empresaService, 
                                    IProdutoService produtoService, IEstoqueService estoqueService, IPerdaService perdaService)
        {
            _inventarioService = inventarioService;
            _empresaService = empresaService;
            _produtoService = produtoService;
            _estoqueService = estoqueService;
            _perdaService = perdaService;
        }

        #region Inventario
        [Route("lista")]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar o inventario";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Inventario";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Inventario";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = await _inventarioService.ObterPaginacaoPorDescricao(_idEmpresaSelec, q, page, ps);
            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "Index";

            return View(lista);
        }

        [Route("novo")]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar o inventario";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Inventario";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Inventario";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";

            var model = new InventarioViewModel();
            model.Situacao = Enums.ESituacaoInventario.Aberta;
            model.Data= DateTime.Now;
            model.IDEMPRESA = _idEmpresaSelec;
            
            model.Id = 0;
            PopularListasAuxiliares(model);
            return View("CreateEdit", model);
        }

        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> Create(InventarioViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            PopularListasAuxiliares(model);
            if (!ModelState.IsValid) return View("CreateEdit", model);

            var resposta = await _inventarioService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo inventario" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [HttpGet]
        public async Task<ActionResult> Edit(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            var model = await _inventarioService.ObterPorId(id);

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
        public async Task<ActionResult> Edit(InventarioViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";
            PopularListasAuxiliares(model);

            if (!ModelState.IsValid) return View("CreateEdit", model);

            
            var resposta = await _inventarioService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar inventario" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }


        [Route("apagar")]
        [HttpGet]
        public async Task<ActionResult> Cancelar(long id)
        {;
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
            PopularListasAuxiliares(objeto);
            return View(objeto);
        }

        [Route("apagar")]
        [HttpPost]
        public async Task<IActionResult> Cancelar(InventarioViewModel model)
        {

            if (!ModelState.IsValid) return View(model);

            var resposta = await _inventarioService.Apagar(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                PopularListasAuxiliares(model);
                var retornoErro = new { mensagem = $"Erro ao tentar apagar inventario" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        #endregion

        #region Item
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


            var model = new ListaInventarioItemViewModel();
            model.Itens = _inventarioService.ObterItemPorIdInventario(id).Result;
            model.NomeInventario = objeto.Descricao;
            model.Situacao = objeto.Situacao.Value;
            model.idInventario = objeto.Id;
            model.TipoAnalise = objeto.TipoAnalise;

            return View("IndexItem", model);
        }

        [Route("editar-itens")]
        public async Task<IActionResult> IndexItemEdit(long id)
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

            if(objeto.Situacao != Enums.ESituacaoInventario.Aberta && objeto.Situacao != Enums.ESituacaoInventario.Execucao)
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
            model.Itens = _inventarioService.ObterItemPorIdInventario(id).Result;
            model.NomeInventario = objeto.Descricao;
            model.Situacao = objeto.Situacao.Value;
            model.idInventario = objeto.Id;
            model.TipoAnalise = objeto.TipoAnalise;

            return View("IndexItemEdit", model);
        }

        [Route("editar-itens")]
        [HttpPost]
        public async Task<ActionResult> IndexItemEdit(ListaInventarioItemViewModel model)
        {
            if (!ModelState.IsValid) return View( model);
            
            var resposta = await _inventarioService.Apurar(model.Itens);

            if (ResponsePossuiErros(resposta))
            {    
                return View(model);
            }

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return View(model);
        }

        [Route("apagar-itens")]
        public async Task<IActionResult> DeleteItemInventario(long id)
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
            model.Itens = _inventarioService.ObterItemPorIdInventario(id).Result;
            model.NomeInventario = objeto.Descricao;
            model.Situacao = objeto.Situacao.Value;
            model.idInventario = objeto.Id;
            model.TipoAnalise = objeto.TipoAnalise;

            return View(model);
        }

        [Route("apagar-itens")]
        [HttpPost]
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

            var resposta = await _inventarioService.ApagarListaItem(itensSelecionados.Itens);

            if (ResponsePossuiErros(resposta))
            {
                ExibirErros();
                return View(model);
            }

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexItem", new {id = model.idInventario});
        }

        [Route("CadastroAutomaticoProdutos")]
        public async Task<IActionResult> CadastroAutomaticoProdutos(long id)
        {
            var resposta = await _inventarioService.CadastrarProdutoPorEstoque(id);
            if (ResponsePossuiErros(resposta))
            {
                return View("IndexItem",new { id = id});
            }

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            var url = Url.Action("IndexItem", "Inventario", new { id = id });

            return Json(new { success = true, url });

        }

        [Route("concluir")]
        public async Task<IActionResult> concluir(long id)
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

            if (objeto.Situacao != Enums.ESituacaoInventario.Execucao)
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

            var resposta = await _inventarioService.Concluir(id);
            var url = Url.Action("Index", "Inventario");

            if (ResponsePossuiErros(resposta))
            {
                var msg = ObterErrosResponse(resposta);

                return Json(new { success = false, url, msg});
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return Json(new { success = true, url });
        }

        [Route("inventariar")]
        public async Task<IActionResult> inventariar(long id)
        {
            var resposta = await _inventarioService.Inventariar(id);

            if (ResponsePossuiErros(resposta))
            {
                return View("IndexItem", new { id = id });
            }

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            var url = Url.Action("Index", "Inventario");

            return Json(new { success = true, url });

        }

        [Route("IncluirProdutosDisponiveisInventario")]
        public async Task<ActionResult> IncluirProdutosDisponiveisInventario(long id)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar o inventario";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Inventario";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Inventario";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

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

            var listaProdutos = await _inventarioService.ObterProdutoDisponivelInventario(_idEmpresaSelec,id);

            var model = new AdicionarListaProdutosDisponiveisViewModel();
            model.idInventario = objeto.Id;
            model.IDEMPRESA = objeto.IDEMPRESA;
            model.NomeInventario = $"{objeto.Codigo} - {objeto.Descricao}";
            model.Situacao = objeto.Situacao;
            listaProdutos.ForEach( item => {
                model.Produtos.Add(new ProdutoDisponivelViewModel() { 
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
        public async Task<ActionResult> IncluirProdutosDisponiveisInventario(AdicionarListaProdutosDisponiveisViewModel model)
        {
            if (!ModelState.IsValid) return View("AddProdutoDisp", model);

            var itensSelecionados = new AdicionarListaProdutosDisponiveisViewModel() {
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

            var resposta = await _inventarioService.IncluirProdutosInventario(itensSelecionados);
            
            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao tentar incluir produtos no inventario selecionado" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("AddProdutoDisp", model);
            }

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexItem", new { id = model.idInventario});
        }
        #endregion

        #region private
        private void PopularListasAuxiliares(InventarioViewModel model)
        {
            if (model.Empresas.Count() == 0)
            {
                var empresas = _empresaService.ObterTodas().Result;
                model.Empresas = empresas.ToList();
            }

            if (model.Estoques.Count() == 0)
            {
                var estoques = _estoqueService.ObterTodas().Result;
                model.Estoques = estoques.ToList();
            }

            var _idEmpresaSelec = ObterIdEmpresaSelecionada();
        }
        #endregion
    }
}

using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using agilium.webapp.manager.mvc.ViewModels.EstoqueViewModel;
using agilium.webapp.manager.mvc.ViewModels.PerdaViewModel;
using agilium.webapp.manager.mvc.ViewModels.ProdutoViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    [Route("perda")]
    public class PerdaController : MainController
    {
        private readonly IPerdaService _perdaService;
        private readonly IProdutoService _produtoService;
        private readonly IEmpresaService _empresaService;
        private readonly IEstoqueService _estoqueService;

        public PerdaController(IPerdaService perdaService, IProdutoService produtoService, IEmpresaService empresaService, IEstoqueService estoqueService)
        {
            _perdaService = perdaService;
            _produtoService = produtoService;
            _empresaService = empresaService;
            _estoqueService = estoqueService;
        }

        private List<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();
        private List<ProdutoViewModel> listaprodutoViewModels { get; set; } = new List<ProdutoViewModel>();
        private List<EstoqueViewModel> listaEstoqueViewModels { get; set; } = new List<EstoqueViewModel>();

        private void PopularListaAuxiliares(PerdaViewModel valeViewModel, long idEmpresa)
        {
            if (listaEmpresaViewModels.Count == 0)
                listaEmpresaViewModels = _empresaService.ObterTodas().Result.ToList();
            if (listaEstoqueViewModels.Count == 0)
                listaEstoqueViewModels = _estoqueService.ObterTodas().Result.ToList();
            if (listaprodutoViewModels.Count == 0)
                listaprodutoViewModels = _produtoService.ObterTodas(idEmpresa).Result.ToList();
            
            if (valeViewModel.Empresas.Count == 0)
                valeViewModel.Empresas = listaEmpresaViewModels;
            if (valeViewModel.Produtos.Count == 0)
                valeViewModel.Produtos = listaprodutoViewModels;
            if (valeViewModel.Estoques.Count == 0)
                valeViewModel.Estoques = listaEstoqueViewModels;
        }

        #region Perda

        [Route("lista")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar a perda/sobra";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "perda/sobra";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "perda/sobra";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = (await _perdaService.ObterPaginacaoPorDescricao(_idEmpresaSelec, q, page, ps));
            lista.ReferenceAction = "Index";

            ViewBag.Pesquisa = q;

            return View(lista);
        }

        [Route("novo")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar perda";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "perda/sobra";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "perda/sobra";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";

            var model = new PerdaViewModel();
            model.Movimento = Enums.ETipoMovimentoPerda.Perda;
            model.Tipo = Enums.ETipoPerda.Quebra;
            model.DataHora = DateTime.Now;
            model.IDEMPRESA = _idEmpresaSelec;

            model.Id = 0;
            PopularListaAuxiliares(model,_idEmpresaSelec);
            return View("CreateEditPerda", model);
        }

        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> Create(PerdaViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            PopularListaAuxiliares(model,model.IDEMPRESA.Value);
            if (!ModelState.IsValid) return View("CreateEditPerda", model);

            var resposta = await _perdaService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova perda/sobra" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditPerda", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            var objeto = await _perdaService.ObterPorId(id);
            PopularListaAuxiliares(objeto,objeto.IDEMPRESA.Value);
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
        public async Task<IActionResult> Edit(PerdaViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";
            PopularListaAuxiliares(model,model.IDEMPRESA.Value);
            if (!ModelState.IsValid) return View("CreateEditPerda", model);

            var resposta = await _perdaService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar perda/sobra" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditPerda", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            var objeto = await _perdaService.ObterPorId(id);
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
        public async Task<IActionResult> Delete(PerdaViewModel model)
        {
            var resposta = await _perdaService.Remover(model.Id);
            PopularListaAuxiliares(model,model.IDEMPRESA.Value);
            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar perda/sobra" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }
        #endregion
    }
}

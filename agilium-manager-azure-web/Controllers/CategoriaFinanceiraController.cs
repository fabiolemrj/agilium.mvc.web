using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.Services;
using agilium.webapp.manager.mvc.ViewModels.CategFinancViewModel;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    public class CategoriaFinanceiraController : MainController
    {
        private readonly ICategoriaFinanceiraService _categoriaFinanceiraService;
        private readonly ILogger<CategoriaFinanceiraController> _logger;
        private readonly string _nomeEntidade = "Categoria Financeira";

        public CategoriaFinanceiraController(ICategoriaFinanceiraService categoriaFinanceiraService, 
            ILogger<CategoriaFinanceiraController> logger)
        {
            _categoriaFinanceiraService = categoriaFinanceiraService;
            _logger = logger;
        }

        #region endPoint
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
        {

            var lista = await _categoriaFinanceiraService.ObterPaginacaoPorDescricao(q, page, ps);
            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "Index";
            return View(lista);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            var model = new CategeoriaFinanceiraViewModel();
            model.STCATEG = Enums.EAtivo.Ativo;
            return View("CreateEdit",model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategeoriaFinanceiraViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            if (!ModelState.IsValid) return View("CreateEdit",model);

            var resposta = await _categoriaFinanceiraService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova {_nomeEntidade}" };
                _logger.LogError(retornoErro.ToString());
                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit",model);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(long id)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";
            var objeto = await _categoriaFinanceiraService.ObterPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidade} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View("CreateEdit", objeto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategeoriaFinanceiraViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            if (!ModelState.IsValid) return View("CreateEdit", model);

            var resposta = await _categoriaFinanceiraService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar nova {_nomeEntidade}" };
                _logger.LogError(retornoErro.ToString());
                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(long id)
        {
            var objeto = await _categoriaFinanceiraService.ObterPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidade} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View(objeto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CategeoriaFinanceiraViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var resposta = await _categoriaFinanceiraService.Remover(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar nova {_nomeEntidade}" };
                _logger.LogError(retornoErro.ToString());
                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "danger";

            return RedirectToAction("Index");
        }


        #endregion

        #region private
        #endregion
    }
}

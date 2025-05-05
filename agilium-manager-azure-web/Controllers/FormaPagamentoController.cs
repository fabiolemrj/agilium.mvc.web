using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels.FormaPagamentoViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    [Route("forma-pagamento")]
    public class FormaPagamentoController : MainController
    {
        private readonly IFormaPagamentoService _formaPagamentoService;
        private readonly IEmpresaService _empresaService;
        public FormaPagamentoController(IFormaPagamentoService formaPagamentoService, IEmpresaService empresaService)
        {
            _formaPagamentoService = formaPagamentoService;
            _empresaService = empresaService;
        }

        [Route("lista")]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
            var _nomeEntidadeMotivo = "Forma de Pagamento";
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
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

            var lista = (await _formaPagamentoService.ObterPorDescricao(_idEmpresaSelec, q, page, ps));

            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "Index";

            return View(lista);
        }

        private void ObterListas(FormaPagamentoViewModel model)
        {
            model.Empresas =_empresaService.ObterTodas().Result.ToList();
        }

        [Route("novo")]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var _nomeEntidadeMotivo = "Forma de Pagamento";
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar o inventario";

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

            var model = new FormaPagamentoViewModel();
            model.Situacao = Enums.EAtivo.Ativo;
            model.IDEmpresa = _idEmpresaSelec;

            model.Id = 0;
            ObterListas(model);
            return View("CreateEdit", model);
        }

        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> Create(FormaPagamentoViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            ObterListas(model);
            if (!ModelState.IsValid) return View("CreateEdit", model);

            var resposta = await _formaPagamentoService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo Forma de pagamento" };

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

            var model = await _formaPagamentoService.ObterPorId(id);

            if (model == null)
            {
                var msgErro = $"Forma de pagamento não localizado";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Forma de pagamento";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            ObterListas(model);
            return View("CreateEdit", model);
        }

        [Route("editar")]
        [HttpPost]
        public async Task<ActionResult> Edit(FormaPagamentoViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";
            ObterListas(model);
            if (!ModelState.IsValid) return View("CreateEdit", model);


            var resposta = await _formaPagamentoService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar Forma de pagamento" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [HttpGet]
        public async Task<ActionResult> Delete(long id)
        {            
            var model = await _formaPagamentoService.ObterPorId(id);

            if (model == null)
            {
                var msgErro = $"Forma de pagamento não localizada";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Inventario";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            ObterListas(model);
            return View(model);
        }

        [Route("apagar")]
        [HttpPost]
        public async Task<IActionResult> Delete(FormaPagamentoViewModel model)
        {
            ObterListas(model);
            if (!ModelState.IsValid) return View(model);

            var resposta = await _formaPagamentoService.Remover(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar Forma de pagamento" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }
    }
}

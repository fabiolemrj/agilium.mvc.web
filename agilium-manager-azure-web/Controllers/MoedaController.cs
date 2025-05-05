using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.Services;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using agilium.webapp.manager.mvc.ViewModels.MoedaViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    [Route("moeda")]
    public class MoedaController : MainController
    {
        private readonly IMoedaService _moedaService;
        private readonly IEmpresaService _empresaService;
        private readonly string _nomeEntidadeMotivo = "Moeda";
        private readonly List<EmpresaViewModel> _empresaViewModels;

        public MoedaController(IMoedaService moedaService, IEmpresaService empresaService)
        {
            _moedaService = moedaService;
            _empresaService = empresaService;
            _empresaViewModels = _empresaService.ObterTodas().Result.ToList();
        }

        #region endpoints
        [Route("lista")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
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

            var lista = (await _moedaService.ObterPorNome(_idEmpresaSelec, q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return View(lista);
        }

        [Route("novo")]
        [HttpGet]
        public async Task<IActionResult> CreateMoeda()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateMoeda";
            ObterTeclasAtalho();
            var model = new MoedaViewModel();
            model.Situacao = Enums.EAtivo.Ativo;
            model.Empresas = _empresaViewModels;

            return View("CreateEditMoeda", model);
        }

        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> CreateMoeda(MoedaViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "CreateMoeda";
            if (!ModelState.IsValid) return View("CreateEditMoeda", model);

            var resposta = await _moedaService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                ObterTeclasAtalho();
                var retornoErro = new { mensagem = $"Erro ao criar novo {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditMoeda", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [HttpGet]
        public async Task<IActionResult> EditMoeda(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditMoeda";
            ObterTeclasAtalho();
            var objeto = await _moedaService.ObterPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeMotivo} não localizado";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeMotivo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            objeto.Empresas = _empresaViewModels;

            return View("CreateEditMoeda", objeto);
        }

        [Route("editar")]
        [HttpPost]
        public async Task<IActionResult> EditMoeda(MoedaViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditMoeda";

            ObterTeclasAtalho();
            if (!ModelState.IsValid) return View("CreateEditMoeda", model);

            var resposta = await _moedaService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                ObterTeclasAtalho();
                var retornoErro = new { mensagem = $"Erro ao editar {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditMoeda", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [HttpGet]
        public async Task<IActionResult> DeleteMoeda(long id)
        {
            ObterTeclasAtalho();
            var objeto = await _moedaService.ObterPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeMotivo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeMotivo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            objeto.Empresas = _empresaViewModels;
            return View(objeto);
        }

        [Route("apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteMoeda(MoedaViewModel model)
        {
            var resposta = await _moedaService.Remover(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }
        #endregion

        #region ViewBag
        private void ObterTeclasAtalho()
        {
            var teclasAtalho = ListasAuxilares.ObterTeclaAtalho();
            ViewBag.teclasAtalho = new SelectList( teclasAtalho.Select(x => new { value= x, text = x}), "value", "text");
            //ViewBag.teclasAtalho = new SelectList(teclasAtalho, "Sigla", "Nome", "");
        }
        #endregion
    }

}

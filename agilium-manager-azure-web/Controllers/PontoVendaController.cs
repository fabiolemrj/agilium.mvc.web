using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using agilium.webapp.manager.mvc.ViewModels.EstoqueViewModel;
using agilium.webapp.manager.mvc.ViewModels.PontoVendaViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    [Route("pdv")]
    public class PontoVendaController : MainController
    {
        private readonly IPontoVendaService _pontoVendaService;
        private readonly IEmpresaService _empresaService;
        private readonly IEstoqueService _estoqueService;
        private readonly string _nomeEntidadeMotivo = "Ponto de Venda";
        private readonly List<EmpresaViewModel> _empresasLista;
        private readonly List<EstoqueViewModel> _estoqueLista;

        public PontoVendaController(IPontoVendaService pontoVendaService, IEmpresaService empresaService, IEstoqueService estoqueService)
        {
            _pontoVendaService = pontoVendaService;
            _empresaService = empresaService;
            _estoqueService = estoqueService;
            _empresasLista = _empresaService.ObterTodas().Result.ToList();
            _estoqueLista = _estoqueService.ObterTodas().Result.ToList();
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

            var lista = (await _pontoVendaService.ObterPorNome(_idEmpresaSelec, q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return View(lista);
        }

        [Route("novo")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            var model = new PontoVendaViewModel();
            model.Situacao = Enums.EAtivo.Ativo;
            model.Empresas = _empresasLista;
            model.Estoque = _estoqueLista;

            return View("CreateEdit", model);
        }

        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> Create(PontoVendaViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            if (!ModelState.IsValid) return View("CreateEdit", model);

            var resposta = await _pontoVendaService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
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
            var objeto = await _pontoVendaService.ObterPorId(id);
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
            objeto.Empresas = _empresasLista;
            objeto.Estoque = _estoqueLista; 

            return View("CreateEdit", objeto);
        }

        [Route("editar")]
        [HttpPost]
        public async Task<IActionResult> Edit(PontoVendaViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            if (!ModelState.IsValid) return View("CreateEdit", model);

            var resposta = await _pontoVendaService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }


        [Route("apagar")]
        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            var objeto = await _pontoVendaService.ObterPorId(id);
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
            objeto.Empresas = _empresasLista;
            objeto.Estoque = _estoqueLista;

            return View(objeto);
        }

        [Route("apagar")]
        [HttpPost]
        public async Task<IActionResult> Delete(PontoVendaViewModel model)
        {
            var resposta = await _pontoVendaService.Remover(model.Id);

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
    }
}


using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.UnidadeViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    public class UnidadeController : MainController
    {
        private readonly IUnidadeService _unidadeService;
        private readonly ILogger<UnidadeController> _logger;
        private readonly string _nomeEntidade = "Unidade";

        public UnidadeController(IUnidadeService unidadeService, 
            ILogger<UnidadeController> logger)
        {
            _unidadeService = unidadeService;
            _logger = logger;
          
        }


        #region endPoints
        [Route("unidades")]
        [HttpGet]
        public async Task<IActionResult> IndexPagination([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
        {

             var lista = await _unidadeService.ObterPorRazaoSocial(q, page, ps);
            //var lista = new PagedViewModel<UnidadeIndexViewModel>();
            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "IndexPagination";
            return View(lista);
        }

         // [Route("unidade/nova")]
        public async Task<IActionResult> Create()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            var model = new UnidadeIndexViewModel();
        
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UnidadeIndexViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "Create";

            if (!ModelState.IsValid) return View(model);

            model.Ativo = Enums.EAtivo.Ativo;

            var resposta = await _unidadeService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova {_nomeEntidade}" };
                _logger.LogError(retornoErro.ToString());
                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }

            return RedirectToAction("IndexPagination");
        }

       // [Route("unidade/editar")]
        public async Task<IActionResult> Edit(long id)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            var objeto = await _unidadeService.ObterPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidade} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexPagination");
            }

            return View("Create", objeto);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(UnidadeIndexViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            if (!ModelState.IsValid) return View("Create", model);

            var resposta = await _unidadeService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar nova {_nomeEntidade}" };
                _logger.LogError(retornoErro.ToString());
                AdicionarErroValidacao(retornoErro.mensagem);
                return View("Create", model);
            }
            return RedirectToAction("IndexPagination");
        }

    //    [Route("unidade/apagar")]
        public async Task<IActionResult> Delete(long id)
        {
            var objeto = await _unidadeService.ObterPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidade} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexPagination");
            }

            return View(objeto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(UnidadeIndexViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var resposta = await _unidadeService.Remover(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar nova {_nomeEntidade}" };
                _logger.LogError(retornoErro.ToString());
                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            return RedirectToAction("IndexPagination");
        }

        public async Task<IActionResult> MudarSituacao(long id)
        {
            var resposta = await _unidadeService.MudarSituacao(id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao mudar situação da unidade {_nomeEntidade}" };
                _logger.LogError(retornoErro.ToString());
                AdicionarErroValidacao(retornoErro.mensagem);
            }
            return RedirectToAction("IndexPagination");
        }

        #endregion
    }
}

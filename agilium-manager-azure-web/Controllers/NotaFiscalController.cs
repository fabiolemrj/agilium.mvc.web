using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using agilium.webapp.manager.mvc.ViewModels.NotaFiscalViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{

    [Route("nota-fiscal")]
    public class NotaFiscalController : MainController
    {
        private readonly INotaFiscalService _notaFiscalService;
        private readonly IEmpresaService _empresaService;

        private List<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();
        public NotaFiscalController(INotaFiscalService notaFiscalService, IEmpresaService empresaService)
        {
            _notaFiscalService = notaFiscalService;
            _empresaService = empresaService;

        }

        private void AtualizarListaAuxiliares(NotaFiscalnutilViewModel viewModel)
        {
            if (listaEmpresaViewModels.Count == 0)
                listaEmpresaViewModels = _empresaService.ObterTodas().Result.ToList();
            if(viewModel.Empresas.Count == 0)
                viewModel.Empresas = listaEmpresaViewModels.ToList();
        }

        #region Nota Fiscal Inutil
        [Route("inutil/lista")]
        public async Task<IActionResult> IndexNFInutil([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar Nota Fiscal Inutil";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Contas Receber";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Conta Receber";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = (await _notaFiscalService.ObterPaginacaoPorDescricao(_idEmpresaSelec, q, page, ps)); ;

            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "IndexNFInutil";
            return View(lista);
        }

        [Route("inutil/novo")]
        [HttpGet]
        public async Task<IActionResult> CreateContaNFInutil()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateContaNFInutil";

            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            var model = new NotaFiscalnutilViewModel();
            model.Situacao = Enums.ESituacaoNFInutil.AguardandoEnvio;
            model.Id = 0;
            model.IDEMPRESA = _idEmpresaSelec > 0 ? _idEmpresaSelec : 0;
            model.Data = DateTime.Now;
            model.Ano = DateTime.Now.Year;

            AtualizarListaAuxiliares(model);

            return View("CreateEditNFInutil", model);
        }

        [Route("inutil/novo")]
        [HttpPost]
        public async Task<IActionResult> CreateContaNFInutil(NotaFiscalnutilViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "CreateContaNFInutil";
            AtualizarListaAuxiliares(model);

            if (!ModelState.IsValid) return View("CreateEditNFInutil", model);

            model.Data = DateTime.Now;

            var resposta = await _notaFiscalService.AdicionarNotaFiscalnutil(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova Nota Fiscal Inutilizada" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditNFInutil", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexNFInutil");
        }

        [Route("inutil/editar")]
        [HttpGet]
        public async Task<IActionResult> EditNFInutil(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditNFInutil";
            var model = await _notaFiscalService.ObterContaPagarPorId(id);
            if (model == null)
            {
                var msgErro = $"Nota Fiscal Inutil não localizada";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Nota Fiscal Inutil";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexNFInutil");
            }
            AtualizarListaAuxiliares(model);
            return View("CreateEditNFInutil", model);
        }

        [Route("inutil/editar")]
        [HttpPost]
        public async Task<IActionResult> EditNFInutil(NotaFiscalnutilViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditNFInutil";
            AtualizarListaAuxiliares(model);
            if (!ModelState.IsValid) return View("CreateEditNFInutil", model);

            var resposta = await _notaFiscalService.AtualizarNotaFiscalnutil(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar Nota Fiscal" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditNFInutil", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexNFInutil");
        }


        [Route("inutil/apagar")]
        [HttpGet]
        public async Task<IActionResult> DeleteNFInutil(long id)
        {
            var model = await _notaFiscalService.ObterContaPagarPorId(id);
            AtualizarListaAuxiliares(model);
            if (model == null)
            {
                var msgErro = $"Nota fiscal não localizada";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Conta Pagar";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexNFInutil");
            }

            return View(model);
        }

        [Route("inutil/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteNFInutil(NotaFiscalnutilViewModel model)
        {
            var resposta = await _notaFiscalService.RemoverNotaFiscalnutil(model.Id);
           
            if (ResponsePossuiErros(resposta))
            {
                AtualizarListaAuxiliares(model);
                var retornoErro = new { mensagem = $"Erro ao tentar apagar nota fiscal" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexNFInutil");
        }

        [Route("inutil/enviar-sefaz/{id}")]
        [HttpGet]
        public async Task<IActionResult> EnviarSefazNFInutil(long id)
        {
            var resposta = _notaFiscalService.EnviarSefazNotaFiscalnutil(id).Result;
            if (!ResponsePossuiErros(resposta))
            {
                TempData["Mensagem"] = "Operação realizada com sucesso";
                TempData["TipoMensagem"] = "success";
            }

            return RedirectToAction("IndexNFInutil");
        }
        #endregion
    }
}

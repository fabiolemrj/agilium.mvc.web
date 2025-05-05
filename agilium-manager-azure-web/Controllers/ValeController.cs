using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels.Cliente;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using agilium.webapp.manager.mvc.ViewModels.ValeViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    [Route("vale")]
    public class ValeController : MainController
    {
        private readonly IValeService _valeService;
        private readonly IClienteService _clienteService;
        private readonly IEmpresaService _empresaService;

        public ValeController(IValeService valeService, IClienteService clienteService, IEmpresaService empresaService)
        {
            _valeService = valeService;
            _clienteService = clienteService;
            _empresaService = empresaService;
        }

        private List<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();
        private List<ClienteViewModel> listaClienteViewModel { get; set; } = new List<ClienteViewModel>();

        private void PopularListaAuxiliares(ValeViewModel valeViewModel)
        {
            if(listaEmpresaViewModels.Count == 0)
                listaEmpresaViewModels = _empresaService.ObterTodas().Result.ToList();
            if (listaClienteViewModel.Count == 0)
                listaClienteViewModel = _clienteService.ObterTodas().Result.ToList();

            if(valeViewModel.Clientes.Count ==0)
                valeViewModel.Clientes= listaClienteViewModel;
            
            if (valeViewModel.Empresas.Count == 0)
                valeViewModel.Empresas = listaEmpresaViewModels;
        }

        #region Vale
        [Route("lista")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar produto";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "produto";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "produto";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = (await _valeService.ObterPaginacaoPorDescricao(_idEmpresaSelec, q, page, ps));
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
                var msgErro = $"Selecione uma empresa para acessar produto";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "produto";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "produto";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";

            var model = new ValeViewModel();
            model.Situacao = Enums.ESituacaoVale.Ativo;
            model.Tipo = Enums.ETipoVale.Promocao;
            model.DataHora = DateTime.Now;
            model.IDEMPRESA = _idEmpresaSelec;

            model.Id = 0;
            PopularListaAuxiliares(model);
            return View("CreateEditVale", model);
        }

        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> Create(ValeViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            PopularListaAuxiliares(model);
            if (!ModelState.IsValid) return View("CreateEditVale", model);

            var resposta = await _valeService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo vale" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditVale", model);
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

            var objeto = await _valeService.ObterPorId(id);
            PopularListaAuxiliares(objeto);
            if (objeto == null)
            {
                var msgErro = $"Vale presente não localizado";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Vale Presente";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View("CreateEditVale", objeto);
        }

        [Route("editar")]
        [HttpPost]
        public async Task<IActionResult> Edit(ValeViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";
            PopularListaAuxiliares(model);
            if (!ModelState.IsValid) return View("CreateEditVale", model);

            var resposta = await _valeService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar vale presente" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditVale", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            var objeto = await _valeService.ObterPorId(id);
            PopularListaAuxiliares(objeto);
            if (objeto == null)
            {
                var msgErro = $"Vale presente não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Vale presente";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View(objeto);
        }

        [Route("apagar")]
        [HttpPost]
        public async Task<IActionResult> Delete(ValeViewModel model)
        {
            var resposta = await _valeService.Remover(model.Id);
            PopularListaAuxiliares(model);
            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar Vale Presente" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("cancelar/{id}")]
        [HttpGet]
        public async Task<IActionResult> Cancel(long id)
        {
            var resposta = _valeService.Cancelar(id).Result;
            if (!ResponsePossuiErros(resposta))
            {
                TempData["Mensagem"] = "Operação realizada com sucesso";
                TempData["TipoMensagem"] = "success";
            }

            return RedirectToAction("Index");
        }

        #endregion
    }
}

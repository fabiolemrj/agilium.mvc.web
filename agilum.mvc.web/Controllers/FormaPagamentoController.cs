using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.FormaPagamento;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Route("forma-pagamento")]
    [Authorize]
    public class FormaPagamentoController : MainController
    {
        #region constantes
        private readonly IFormaPagamentoService _formaPagamentoService;
        private readonly IEmpresaService _empresaService;
        #endregion

        #region construtores
        public FormaPagamentoController(IFormaPagamentoService formaPagamentoService, IEmpresaService empresaService
            , INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService, IMapper mapper) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper)
        {
            _formaPagamentoService = formaPagamentoService;
            _empresaService = empresaService;
        }
        #endregion

        #region forma pagamento

        [Route("lista")]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
            var _nomeEntidadeMotivo = "Forma de Pagamento";
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

            var lista = (await ObterListaPaginado(Convert.ToInt64(empresaSelecionada.IDEMPRESA), q, page, ps));

            ViewBag.Pesquisa = q;

            return View(lista);
        }

        [Route("novo")]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var _nomeEntidadeMotivo = "Forma de Pagamento";
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

            var model = new FormaPagamentoViewModel();
            model.Situacao = EAtivo.Ativo;
            model.IDEmpresa = Convert.ToInt64(empresaSelecionada.IDEMPRESA);

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

            var objeto = _mapper.Map<FormaPagamento>(model);

            if (objeto.Id == 0) objeto.Id = objeto.GerarId();
            await _formaPagamentoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo Forma de pagamento" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }
            await _formaPagamentoService.Salvar();
            LogInformacao($"sucesso: {Deserializar(objeto)}", "FormaPagamento", "Adicionar", null);
          
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

            var model = await Obter(id.ToString());

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
            await _formaPagamentoService.Atualizar(_mapper.Map<FormaPagamento>(model));

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao editar Forma de pagamento" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }
            await _formaPagamentoService.Salvar();
            LogInformacao($"sucesso: {Deserializar(model)}", "FormaPagamento", "Atualizar", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [HttpGet]
        public async Task<ActionResult> Delete(long id)
        {
            var model = await Obter(id.ToString());

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
            await _formaPagamentoService.Apagar(model.Id);
            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar Forma de pagamento" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            await _formaPagamentoService.Salvar();
            LogInformacao($"excluir {Deserializar(model)}", "FormaPagamento", "Excluir", null);
         
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }
        #endregion

        #region metodos privados
        private async Task<PagedViewModel<FormaPagamentoViewModel>> ObterListaPaginado(long idempresa, string filtro, int page, int pageSize)
        {
            var retorno = await _formaPagamentoService.ObterPorPaginacao(idempresa, filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<FormaPagamentoViewModel>>(retorno.List);

            return new PagedViewModel<FormaPagamentoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "lista",
                ReferenceController = "forma-pagamento",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<FormaPagamentoViewModel> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            return _mapper.Map<FormaPagamentoViewModel>(await _formaPagamentoService.ObterPorId(_id));
        }

        private void ObterListas(FormaPagamentoViewModel model)
        {
            model.Empresas = _mapper.Map<List<EmpresaViewModel>>( _empresaService.ObterTodas().Result.ToList());
        }
        #endregion
    }
}

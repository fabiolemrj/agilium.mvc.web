using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilum.mvc.web.Extensions;
using agilum.mvc.web.Services;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Moedas;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Route("moeda")]
    [Authorize]
    public class MoedaController : MainController
    {
        #region constantes
        private readonly IMoedaService _moedaService;
        private readonly IEmpresaService _empresaService;
        private readonly string _nomeEntidadeMotivo = "Moeda";
        private readonly List<EmpresaViewModel> _empresaViewModels;
        #endregion

        #region construtor
        public MoedaController(IMoedaService moedaService, IEmpresaService empresaService
            , INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService, IMapper mapper) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper)
        {
            _moedaService = moedaService;
            _empresaService = empresaService;
            _empresaViewModels = _mapper.Map<List<EmpresaViewModel>>( _empresaService.ObterTodas().Result.ToList());
        }
        #endregion

        #region moeda

        [Route("lista")]
        [ClaimsAuthorizeAttribute(2127)]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
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
        [ClaimsAuthorizeAttribute(2128)]
        public async Task<IActionResult> CreateMoeda()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateMoeda";
            ObterTeclasAtalho();
            var model = new MoedaViewModel();
            model.Situacao = EAtivo.Ativo;
            model.Empresas = _empresaViewModels;
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();
            model.IDEMPRESA = Convert.ToInt64(empresaSelecionada.IDEMPRESA);

            return View("CreateEditMoeda", model);
        }

        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> CreateMoeda(MoedaViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "CreateMoeda";
            if (!ModelState.IsValid) return View("CreateEditMoeda", model);

            if (model.Id == 0) model.Id = await GerarId();

            var moeda = _mapper.Map<Moeda>(model);

            await _moedaService.Adicionar(moeda);

            if (!OperacaoValida())
            {
                ObterTeclasAtalho();
                var retornoErro = new { mensagem = $"Erro ao criar novo {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditMoeda", model);
            }
            await _moedaService.Salvar();
            LogInformacao($"sucesso: {Deserializar(model)}", "Moeda", "Atualizar", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2131)]
        public async Task<IActionResult> EditMoeda(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditMoeda";
            ObterTeclasAtalho();
            var objeto = await Obter(id.ToString());
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

            await _moedaService.Atualizar(_mapper.Map<Moeda>(model));

            if (!OperacaoValida())
            {
                ObterTeclasAtalho();
                var retornoErro = new { mensagem = $"Erro ao editar {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditMoeda", model);
            }
            await _moedaService.Salvar();
            LogInformacao($"sucesso: {Deserializar(model)}", "Moeda", "Atualizar", null);
          
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }


        [Route("apagar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2129)]
        public async Task<IActionResult> DeleteMoeda(long id)
        {
            ObterTeclasAtalho();
            var objeto = await Obter(id.ToString());
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

            await _moedaService.Apagar(model.Id);
            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            await _moedaService.Salvar();
            LogInformacao($"sucesso: {Deserializar(model)}", "Moeda", "Excluir", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        #endregion

        #region metodos privados
        private async Task<PagedViewModel<MoedaViewModel>> ObterListaPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _moedaService.ObterPorPaginacao(idEmpresa, filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<MoedaViewModel>>(retorno.List);


            return new PagedViewModel<MoedaViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "lista",
                ReferenceController = "moeda",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<MoedaViewModel> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var moeda = _mapper.Map<MoedaViewModel>( await _moedaService.ObterCompletoPorId(_id));
            return moeda;
        }
        #endregion

        #region ViewBag
        private void ObterTeclasAtalho()
        {
            var teclasAtalho = ListasAuxilares.ObterTeclaAtalho();
            ViewBag.teclasAtalho = new SelectList(teclasAtalho.Select(x => new { value = x, text = x }), "value", "text");
            //ViewBag.teclasAtalho = new SelectList(teclasAtalho, "Sigla", "Nome", "");
        }
        #endregion
    }
}

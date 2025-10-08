using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Services;
using agilium_manager_azure_business.Interfaces.IService;
using agilum.mvc.web.Data;
using agilum.mvc.web.Extensions;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.CategeoriaFinanceira;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Authorize]
    [Route("categoria-financeira")]
    public class CategoriaFinanceiraController : MainController
    {
        private readonly ICategoriaFinanceiraService _categoriaService;
        private const string _nomeEntidade = "Categoria Financeira";

        #region construtor
        public CategoriaFinanceiraController(ICategoriaFinanceiraService categoriaService, INotificador notificador, 
            IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService, IMapper mapper,
            ILicencaService licencaService, SignInManager<AppUserAgiliumIdentity> signInManager) 
            : base(notificador, configuration, appUser,utilDapperRepository, logService, mapper, 
                  licencaService, signInManager)
        {
            _categoriaService = categoriaService;
        }
        #endregion

        #region categoria

        [HttpGet]
        [Route("lista")]
        [ClaimsAuthorizeAttribute(2095)]
        public async Task<IActionResult> Index([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
        {

            var lista = await ObterListaPaginado(q, page, ps);
            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "lista";
            lista.Query = q;
            return View(lista);
        }

        [Route("novo")]
        [ClaimsAuthorizeAttribute(2096)]
        public async Task<IActionResult> Create()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            var model = new CategeoriaFinanceiraViewModel();
            model.STCATEG = agilium.api.business.Enums.EAtivo.Ativo;
            return View("CreateEdit", model);
        }

        [HttpPost]
        [Route("novo")]
        public async Task<IActionResult> Create(CategeoriaFinanceiraViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            if (!ModelState.IsValid) return View("CreateEdit", model);

            if (model.Id == 0) model.Id = GerarId().Result;

            var categoriaFinanceira = _mapper.Map<CategoriaFinanceira>(model);

            await _categoriaService.Adicionar(categoriaFinanceira);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova {_nomeEntidade}" };
                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }
            await _categoriaService.Salvar();
            LogInformacao($"Objeto adicionado com sucesso {Deserializar(model)}", "CategoriaFinanceira", "Adicionar", null);

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [ClaimsAuthorizeAttribute(2099)]
        public async Task<IActionResult> Edit(long id)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";
            var objeto = _mapper.Map<CategeoriaFinanceiraViewModel>(await _categoriaService.ObterPorId(id));
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
        [Route("editar")]
        public async Task<IActionResult> Edit(CategeoriaFinanceiraViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            if (!ModelState.IsValid) return View("CreateEdit", model);

            await _categoriaService.Atualizar(_mapper.Map<CategoriaFinanceira>(model));

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao editar nova {_nomeEntidade}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }
            await _categoriaService.Salvar();
            LogInformacao($"Objeto editado com sucesso {Deserializar(model)}", "CategoriaFinanceira", "Editar", null);

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [ClaimsAuthorizeAttribute(2096)]
        public async Task<IActionResult> Delete(long id)
        {
            var objeto = _mapper.Map<CategeoriaFinanceiraViewModel>(await _categoriaService.ObterPorId(id));
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
        [Route("apagar")]
        public async Task<IActionResult> Delete(CategeoriaFinanceiraViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _categoriaService.Remover(model.Id);
            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao editar nova {_nomeEntidade}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            await _categoriaService.Salvar();
            LogInformacao($"Objeto apagado com sucesso {Deserializar(model)}", "CategoriaFinanceira", "Delete", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }
        #endregion

        #region metodos privados
        private async Task<PagedViewModel<CategeoriaFinanceiraViewModel>> ObterListaPaginado(string filtro, int page, int pageSize)
        {
            var retorno = await _categoriaService.ObterPorDescricaoPaginacao(filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<CategeoriaFinanceiraViewModel>>(retorno.List);

            return new PagedViewModel<CategeoriaFinanceiraViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "lista",
                TotalResults = retorno.TotalResults
            };
        }

        #endregion
    }
}

﻿using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Services;

using agilum.mvc.web.ViewModels.UnidadeViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using agilum.mvc.web.Extensions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace agilum.mvc.web.Controllers
{

    [Authorize]
    [Route("unidade")]
    public class UnidadeController : MainController
    {
        #region constantes
        private readonly IUnidadeService _unidadeService;
        private readonly string _nomeEntidade = "Unidade";

        private readonly ICaService _caService;
        #endregion

        #region construtores

        public UnidadeController(IUnidadeService unidadeService, INotificador notificador, IConfiguration configuration,
            IUser appUser, IMapper mapper, IUtilDapperRepository utilDapperRepository, ILogService logService,
            ICaService caService) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper)
        {
            _unidadeService = unidadeService;
            _caService = caService;
        }
        #endregion

        #region unidades
        [Route("lista")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2006)]
         public async Task<IActionResult> Index([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            //ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterListaPaginado(q, page, ps));
            ViewBag.Pesquisa = q;
            return View(lista);
        }

        [Route("nova")]
        [ClaimsAuthorizeAttribute(2007)]
        public async Task<IActionResult> Create()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            var model = new UnidadeIndexViewModel();

            return View(model);
        }

        [Route("nova")]
        [HttpPost]
        public async Task<IActionResult> Create(UnidadeIndexViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "Create";

            if (!ModelState.IsValid) return View(model);

            model.Ativo = agilium.api.business.Enums.EAtivo.Ativo;

            var objeto = _mapper.Map<Unidade>(model);
            
            if (objeto.Id == 0) objeto.Id = await GerarId();

            await _unidadeService.Adicionar(objeto);
            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova {_nomeEntidade}" };
                ObterNotificacoes("Unidade", "Adicionar", "Web");
                return View(model);
            }
            await _unidadeService.Salvar();

            var objetoDeserialziado = Deserializar(model);
            LogInformacao($"Objeto Criado com sucesso {objetoDeserialziado}", "Unidade", "Adicionar", null);

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [ClaimsAuthorizeAttribute(2010)]
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
                return RedirectToAction("Index");
            }
            var viewModel = _mapper.Map<UnidadeIndexViewModel>(objeto);

            return View("Create", viewModel);
        }

        [HttpPost]
        [Route("editar")]
        public async Task<IActionResult> Edit(UnidadeIndexViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            if (!ModelState.IsValid) return View("Create", model);

            var objeto = _mapper.Map<Unidade>(model);
            await _unidadeService.Atualizar(objeto);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Unidade", "Atualizar", "Web"));
                NotificarErro(msgErro);
                return View("Create", model);
            }
            await _unidadeService.Salvar();
            var objetoDeserialziado = Deserializar(objeto);
            LogInformacao($"Objeto atualizado com sucesso {objetoDeserialziado}", "Unidade", "Atualizar", null);
            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [ClaimsAuthorizeAttribute(2008)]
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
                return RedirectToAction("Index");
            }
            var viewModel = _mapper.Map<UnidadeIndexViewModel>(objeto);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("apagar")]
        public async Task<IActionResult> Delete(UnidadeIndexViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _unidadeService.Apagar(model.Id);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Unidade", "Excluir", "Web");
                return View( model);
            }
            await _unidadeService.Salvar();
            var objetoDeserialziado = Deserializar(model);
            LogInformacao($"Objeto excluido com sucesso {objetoDeserialziado}", "Unidade", "Apagar", null);
            
            return RedirectToAction("Index");
        }
        #endregion

        #region metodos privados
        private async Task<agilum.mvc.web.ViewModels.PagedViewModel<UnidadeIndexViewModel>> ObterListaPaginado(string filtro, int page, int pageSize)
        {
            var retorno = await _unidadeService.ObterPorDescricaoPaginacao(filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<UnidadeIndexViewModel>>(retorno.List);

            return new agilum.mvc.web.ViewModels.PagedViewModel<UnidadeIndexViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "lista",
                ReferenceController = "unidade",
                TotalResults = retorno.TotalResults
            };
        }

        #endregion

    }
}

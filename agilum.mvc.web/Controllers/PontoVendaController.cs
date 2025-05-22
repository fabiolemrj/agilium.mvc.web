using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilum.mvc.web.Extensions;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Estoque;
using agilum.mvc.web.ViewModels.PontoVenda;
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
    [Route("pdv")]
    [Authorize]
    public class PontoVendaController : MainController
    {

        #region constantes
        private readonly IPontoVendaService _pontoVendaService;
        private readonly IEmpresaService _empresaService;
        private readonly IEstoqueService _estoqueService;
        private readonly string _nomeEntidadeMotivo = "Ponto de Venda";
        private readonly List<EmpresaViewModel> _empresasLista;
        private readonly List<EstoqueViewModel> _estoqueLista;
        #endregion

        #region construtores
        public PontoVendaController(IPontoVendaService pontoVendaService, IEmpresaService empresaService, IEstoqueService estoqueService
            , INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService, IMapper mapper) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper)
        {
            
            _pontoVendaService = pontoVendaService;
            _empresaService = empresaService;
            _estoqueService = estoqueService;
            _empresasLista = _mapper.Map<List<EmpresaViewModel>> (_empresaService.ObterTodas().Result.ToList());
            _estoqueLista = _mapper.Map<List<EstoqueViewModel>>(_estoqueService.ObterTodas().Result.ToList());
        }
        #endregion

        #region pdv
        [Route("lista")]
        [ClaimsAuthorizeAttribute(2120)]
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

            var lista = (await ObterListaPaginado(Convert.ToInt64(empresaSelecionada.IDEMPRESA), q, page, ps)) ;

            ViewBag.Pesquisa = q;

            return View(lista);
        }

        [Route("novo")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2121)]
        public async Task<IActionResult> Create()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            var model = new PontoVendaViewModel();
            model.Situacao = EAtivo.Ativo;
            model.Empresas = _empresasLista;
            model.Estoques = _estoqueLista;
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();
            model.IDEMPRESA = Convert.ToInt64(empresaSelecionada.IDEMPRESA);
            return View("CreateEdit", model);
        }

        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> Create(PontoVendaViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            if (!ModelState.IsValid) return View("CreateEdit", model);

            if (model.Id == 0) model.Id = await GerarId();

            var pontoVenda = _mapper.Map<PontoVenda>(model);

            await _pontoVendaService.Adicionar(pontoVenda);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }
            await _pontoVendaService.Salvar();
            LogInformacao($"sucesso: {Deserializar(model)}", "PontoVenda", "Adicionar", null);
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2124)]
        public async Task<IActionResult> Edit(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";
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
            objeto.Empresas = _empresasLista;
            objeto.Estoques = _estoqueLista;

            return View("CreateEdit", objeto);
        }

        [Route("editar")]
        [HttpPost]
        public async Task<IActionResult> Edit(PontoVendaViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            if (!ModelState.IsValid) return View("CreateEdit", model);

            await _pontoVendaService.Atualizar(_mapper.Map<PontoVenda>(model));

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao editar {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }
            await _pontoVendaService.Salvar();
            LogInformacao($"sucesso: {Deserializar(model)}", "PontoVenda", "Atualizar", null);
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2122)]
        public async Task<IActionResult> Delete(long id)
        {
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
            objeto.Empresas = _empresasLista;
            objeto.Estoques = _estoqueLista;

            return View(objeto);
        }

        [Route("apagar")]
        [HttpPost]
        public async Task<IActionResult> Delete(PontoVendaViewModel model)
        {
            await _pontoVendaService.Apagar(model.Id);
            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            await _pontoVendaService.Salvar();
            LogInformacao($"sucesso: id{model.Id}", "PontoVenda", "Excluir", null);
          
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        #endregion


        #region metodos privados
        private async Task<PagedViewModel<PontoVendaViewModel>> ObterListaPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _pontoVendaService.ObterPorPaginacao(idEmpresa, filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<PontoVendaViewModel>>(retorno.List);

            return new PagedViewModel<PontoVendaViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "lista",
                ReferenceController = "pdv",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<PontoVendaViewModel> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var fornecedor = await _pontoVendaService.ObterCompletoPorId(_id);
            return _mapper.Map<PontoVendaViewModel>(fornecedor);
            
        }
        #endregion
    }
}

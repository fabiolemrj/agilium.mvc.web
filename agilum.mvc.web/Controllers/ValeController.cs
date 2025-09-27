using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium_manager_azure_business.Interfaces.IService;
using agilum.mvc.web.Data;
using agilum.mvc.web.Enums;
using agilum.mvc.web.Extensions;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Cliente;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Vale;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Authorize]
    [Route("vale")]
    public class ValeController : MainController
    {
        private readonly IValeService _valeService;
        private readonly IClienteService _clienteService;
        private readonly IEmpresaService _empresaService;

        private readonly string _nomeEntidadeMotivo = "Vale";

        #region construtor
        public ValeController(INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository,
            ILogService logService, IMapper mapper, IValeService valeService, IClienteService clienteService, IEmpresaService empresaService, ILicencaService licencaService, SignInManager<AppUserAgiliumIdentity> signInManager) : 
            base(notificador, configuration, appUser, utilDapperRepository, logService, mapper, licencaService, signInManager)
        {
            _valeService = valeService;
            _clienteService = clienteService;
            _empresaService = empresaService;
        }

        #endregion

        #region listas auxiliares

        private List<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();
        private List<ClienteViewModel> listaClienteViewModel { get; set; } = new List<ClienteViewModel>();
        private void PopularListaAuxiliares(ValeViewModel valeViewModel)
        {
            if (listaEmpresaViewModels.Count == 0)
                listaEmpresaViewModels = _mapper.Map<List<EmpresaViewModel>>( _empresaService.ObterTodas().Result.ToList());
            if (listaClienteViewModel.Count == 0)
                listaClienteViewModel = _mapper.Map<List<ClienteViewModel>>( _clienteService.ObterTodos().Result.ToList());

            if (valeViewModel.Clientes.Count == 0)
                valeViewModel.Clientes = listaClienteViewModel;

            if (valeViewModel.Empresas.Count == 0)
                valeViewModel.Empresas = listaEmpresaViewModels;
        }
        #endregion

        #region vale
        [Route("lista")]
        [ClaimsAuthorizeAttribute(2198)]
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
            lista.ReferenceAction = "lista";
            lista.Query = q;
            ViewBag.Pesquisa = q;

            return View(lista);
        }


        [Route("novo")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2199)]
        public async Task<IActionResult> Create()
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

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";

            var model = new ValeViewModel();
            model.Situacao = ESituacaoVale.Ativo;
            model.Tipo = ETipoVale.Promocao;
            model.DataHora = DateTime.Now;
            model.IDEMPRESA = Convert.ToInt64(empresaSelecionada.IDEMPRESA);

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

            if (model.Id == 0) model.Id = await GerarId();
            if (!model.DataHora.HasValue)
                model.DataHora = DateTime.Now;
            model.CodigoBarra = _valeService.GerarCodigoBarraVale().Result;

            var vale = _mapper.Map<Vale>(model);

            await _valeService.Adicionar(vale);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo vale" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditVale", model);
            }
            await _valeService.Salvar();

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2202)]
        public async Task<IActionResult> Edit(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            var objeto = _mapper.Map <ValeViewModel>(await _valeService.ObterPorId(id));
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

            var vale = _mapper.Map<Vale>(model);

            await _valeService.Atualizar(vale);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao editar vale presente" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditVale", model);
            }

            await _valeService.Salvar();

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2200)]
        public async Task<IActionResult> Delete(long id)
        {
            var objeto = _mapper.Map<ValeViewModel>(await _valeService.ObterPorId(id));
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
            await _valeService.Apagar(model.Id);
            PopularListaAuxiliares(model);
            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar Vale Presente" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            await _valeService.Salvar();
                  
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("cancelar/{id}")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2200)]
        public async Task<IActionResult> Cancel(long id)
        {
            var vale = await _valeService.ObterPorId(id);
            if (vale == null) return NotFound();

            if (vale.STVALE == agilium.api.business.Enums.ESituacaoVale.Utilizado)
            {
                NotificarErro("Vales com status de UTILIZADO não podem ser cancelados.");
                var msgErro = string.Join("\n\r", ObterNotificacoes("Vale", "cancelar", "Web", $"id:{id}"));

                return RedirectToAction("Index");
            }

            vale.Cancelar();

            await _valeService.Atualizar(vale);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                return RedirectToAction("Index");
            }
            await _valeService.Salvar();
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }
        #endregion

        #region Private
        private async Task<PagedViewModel<ValeViewModel>> ObterListaPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _valeService.ObterValePorPaginacao(idEmpresa, filtro, page, pageSize);

            var listaContaPagarViewModel = new List<ValeViewModel>();

            retorno.List.ToList().ForEach(vale => {
                var caixaMoedaViewModel = _mapper.Map<ValeViewModel>(vale);
                caixaMoedaViewModel.EmpresaNome = vale.Empresa != null && !string.IsNullOrEmpty(vale.Empresa.NMRZSOCIAL) ? vale.Empresa.NMRZSOCIAL : string.Empty;
                caixaMoedaViewModel.ClienteNome = vale.Cliente != null && !string.IsNullOrEmpty(vale.Cliente.NMCLIENTE) ? vale.Cliente.NMCLIENTE : string.Empty;

                listaContaPagarViewModel.Add(caixaMoedaViewModel);
            });

            return new PagedViewModel<ValeViewModel>()
            {
                List = listaContaPagarViewModel,
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

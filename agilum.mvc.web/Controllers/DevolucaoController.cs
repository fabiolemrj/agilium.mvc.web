using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Services;
using agilum.mvc.web.Extensions;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Cliente;
using agilum.mvc.web.ViewModels.Devolucao;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Venda;
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
    [Route("devolucao")]
    [Authorize]
    public class DevolucaoController : MainController
    {
        #region constantes
        private readonly IDevolucaoService _devolucaoService;
        private readonly IEmpresaService _empresaService;
     //   private readonly IVendaService _vendaService;
        private readonly IClienteService _clienteService;
        #endregion

        #region Listas auxiliares
        private readonly string _nomeEntidadeMotivo = "Motivos de Devolução";
        private IEnumerable<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();
      //  private IEnumerable<VendaViewModel> listaVendasViewModel { get; set; } = new List<VendaViewModel>();
        private List<ClienteViewModel> listaClienteViewModel { get; set; } = new List<ClienteViewModel>();
        #endregion

        #region construtores
        public DevolucaoController(IDevolucaoService devolucaoService, IEmpresaService empresaService,
           // IVendaService vendaService,
            IClienteService clienteService,
            INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService, IMapper mapper) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper)
        {
            _devolucaoService = devolucaoService;
            _empresaService = empresaService;
            _clienteService = clienteService;
            //_vendaService = vendaService;
            if (!listaEmpresaViewModels.Any())
                listaEmpresaViewModels = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);

            if (!listaClienteViewModel.Any())
                listaClienteViewModel = _mapper.Map<List<ClienteViewModel>>(_clienteService.ObterTodos().Result);
        }
        #endregion

        #region motivos
        [Route("motivo/lista")]
        [ClaimsAuthorizeAttribute(2150)]
        public async Task<IActionResult> IndexMotivos([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
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

            var lista = (await ObterMotivos(Convert.ToInt64(empresaSelecionada.IDEMPRESA), q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return View(lista);
        }
        [Route("motivo/novo")]
        [ClaimsAuthorizeAttribute(2151)]
        public async Task<IActionResult> CreateMotivo()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateMotivo";

            var model = new MotivoDevolucaoViewModel();
            model.situacao = EAtivo.Ativo;
            model.Empresas = listaEmpresaViewModels.ToList();
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();
            model.idEmpresa = Convert.ToInt64(empresaSelecionada.IDEMPRESA);
            return View("CreateEditMotivo", model);
        }

        [Route("motivo/novo")]
        [HttpPost]
        public async Task<IActionResult> CreateMotivo(MotivoDevolucaoViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateMotivo";
            if (!ModelState.IsValid) return View("CreateEditMotivo", model);

            var objeto = _mapper.Map<MotivoDevolucao>(model);

            if (objeto.Id == 0) objeto.Id = objeto.GerarId();
            await _devolucaoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Devolucao", "AdicionarMotivo", "Web"));
                return View("CreateEditMotivo", model);
            }

            await _devolucaoService.Salvar();
            LogInformacao($"incluir {Deserializar(objeto)}", "Devolucao", "AdicionarMotivo", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMotivos");
        }

        [Route("motivo/editar")]
        [ClaimsAuthorizeAttribute(2154)]
        public async Task<IActionResult> EditMotivo(long id)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditMotivo";
            MotivoDevolucaoViewModel objeto = await ObterMotivoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeMotivo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeMotivo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexMotivos");
            }
            objeto.Empresas = listaEmpresaViewModels.ToList();
            return View("CreateEditMotivo", objeto);
        }

        [Route("motivo/editar")]
        [HttpPost]
        public async Task<IActionResult> EditMotivo(MotivoDevolucaoViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditMotivo";

            if (!ModelState.IsValid) return View("CreateEditMotivo", model);
            var objeto = _mapper.Map<MotivoDevolucao>(model);

            await _devolucaoService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Devolucao", "AtualizarMotivo", "Web"));
                return View("CreateEditMotivo", model);
            }

            await _devolucaoService.Salvar();
            LogInformacao($"incluir {Deserializar(objeto)}", "Devolucao", "AtualizarMotivo", null);
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMotivos");
        }

        [Route("motivo/apagar")]
        [ClaimsAuthorizeAttribute(2152)]
        public async Task<IActionResult> DeleteMotivo(long id)
        {
            var objeto = await ObterMotivoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeMotivo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeMotivo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexMotivos");
            }
            objeto.Empresas = listaEmpresaViewModels.ToList();
            return View(objeto);
        }

        [Route("motivo/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteMotivo(MotivoDevolucaoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!_devolucaoService.ApagarMotivo(model.Id).Result)
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));

                NotificarErro(msgErro);
                return View(model);
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Devolucao", "ApagarMotivo", "Web"));
                return View(model);
            }
            await _devolucaoService.Salvar();
            LogInformacao($"excluir id:{model.Id}", "Devolucao", "ApagarMotivo", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMotivos");
        }
        #endregion

        #region metodos privados
        private async Task<PagedViewModel<MotivoDevolucaoViewModel>> ObterMotivos(long idempresa, string filtro, int page, int pageSize)
        {
            var retorno = await _devolucaoService.ObterMotivoPaginacaoPorDescricao(idempresa, filtro, page, pageSize);
            var listaTeste = retorno.List;
            var lista = _mapper.Map<IEnumerable<MotivoDevolucaoViewModel>>(listaTeste);

            return new PagedViewModel<MotivoDevolucaoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "IndexMotivos",
                ReferenceController = "devolucao",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<MotivoDevolucaoViewModel> ObterMotivoPorId(long id)
        {
            return _mapper.Map<MotivoDevolucaoViewModel>(await _devolucaoService.ObterPorIdMotivo(id));
        }

        #endregion

        
    }

}

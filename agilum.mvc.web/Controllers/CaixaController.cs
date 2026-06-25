using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Caixa;
using agilium.api.business.Models;
using agilum.mvc.web.Extensions;
using agilium_manager_azure_business.Interfaces.IService;
using agilium.api.business.Models;
using agilum.mvc.web.Services;

namespace agilum.mvc.web.Controllers
{
    [Route("caixa")]
    [Authorize]
    public class CaixaController : MainController
    {
        private readonly ICaixaService _caixaService;
        private readonly IUsuarioService _usuarioService;
        private readonly string _nomeEntidadeMotivo = "Caixa";

        public CaixaController(IUsuarioService usuarioService, ICaixaService caixaService,
             INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, 
             ILogService logService, IMapper mapper, ILicencaService licencaService, IAuthService authService) :
            base(notificador, configuration, appUser, utilDapperRepository, logService, mapper, licencaService, authService)
        {
            _caixaService = caixaService;
            _usuarioService = usuarioService;
        }

        #region caixa
        [Route("lista")]
        [ClaimsAuthorizeAttribute(2156)]
        public async Task<IActionResult> IndexCaixa([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string? DataFinal = null, [FromQuery] string DataInicial = null)
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

            var dataAtual = DateTime.Now;
            DateTime _dtini, _dtFim;
            if (DataInicial == null)
            {
                DateTime primeiroDiaDoMes = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                _dtini = primeiroDiaDoMes;
            }
            else _dtini = Convert.ToDateTime(DataInicial);

            if (DataFinal == null)
            {
                DateTime ultimoDiaDoMes = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month));
                _dtFim = ultimoDiaDoMes;
            }
            else _dtFim = Convert.ToDateTime(DataFinal);

            if (_dtini > _dtFim)
            {
                AdicionarErroValidacao("Data Final deve ser maior ou igual a data inicial");
            }

            var lista = (await ObterListaCaixaPaginado(Convert.ToInt64(empresaSelecionada.IDEMPRESA), _dtini, _dtFim, page, ps));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            lista.ReferenceAction = "lista";

            return View(lista);
        }
        #endregion

        #region Caixa Movimentacao
        [Route("movimentacao")]
        [ClaimsAuthorizeAttribute(2156)]
        public async Task<IActionResult> IndexMovimentacao([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] long idCaixa = 0)
        {
            var caixa = _caixaService.ObterCompletoPorId(idCaixa).Result;
            var caixaViewModel = _mapper.Map<CaixaindexViewModel>(caixa);
            caixaViewModel.Empresa = caixa.Empresa != null && !string.IsNullOrEmpty(caixa.Empresa.NMRZSOCIAL) ? caixa.Empresa.NMRZSOCIAL : string.Empty;
            caixaViewModel.Turno = caixa.Turno != null && caixa.Turno.NUTURNO > 0 ? caixa.Turno.NUTURNO.ToString() : string.Empty;
            caixaViewModel.PDV = caixa.PontoVenda != null && !string.IsNullOrEmpty(caixa.PontoVenda.DSPDV) ? caixa.PontoVenda.DSPDV : string.Empty;
            caixaViewModel.Funcionario = caixa.Funcionario != null && !string.IsNullOrEmpty(caixa.Funcionario.NMFUNC) ? caixa.Funcionario.NMFUNC : string.Empty;

            var lista = (await ObterListaCaixaMovimentoPaginado(idCaixa, page, ps));

            lista.ReferenceAction = "IndexMovimentacao";
            ViewBag.idCaixa = idCaixa;
            ViewBag.caixa = caixa != null ? $@"Caixa: {caixaViewModel.Sequencial.Value.ToString("D3")} - {caixaViewModel.DataAbertura.Value.ToString("dd/MM/yyyy")} - {caixaViewModel.Funcionario}" : "";

            return View(lista);
        }
        #endregion

        #region Caixa Moeda
        [Route("moedas")]
        [ClaimsAuthorizeAttribute(2156)]
        public async Task<IActionResult> IndexMoeda([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] long idCaixa = 0)
        {
            var caixa = _caixaService.ObterCompletoPorId(idCaixa).Result;
            var caixaViewModel = _mapper.Map<CaixaindexViewModel>(caixa);
     
            caixaViewModel.Funcionario = caixa.Funcionario.NMFUNC;
            var lista = (await ObterListaCaixaMoedaPaginado(idCaixa, page, ps));

            lista.ReferenceAction = "IndexMoeda";
            ViewBag.idCaixa = idCaixa;
            ViewBag.Caixa = caixa != null ? $@"Caixa: {caixaViewModel.Sequencial.Value.ToString("D3")} - {caixaViewModel.DataAbertura.Value.ToString("dd/MM/yyyy")} - {caixaViewModel.Funcionario}" : "";
            ViewBag.total = ValorTotalMoedas(lista.List);
            return View(lista);
        }

        [Route("moeda/correcao")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2158)]
        public async Task<IActionResult> CorrecaoMoeda(long id, long idCaixa)
        {
            var caixa = _caixaService.ObterCompletoPorId(idCaixa).Result;
            var caixaViewModel = _mapper.Map<CaixaindexViewModel>(caixa);

            ViewBag.caixa = caixa != null ? $@"{caixa.SQCAIXA.Value.ToString("D3")} - {caixa.DTHRABT.Value.ToString("dd/MM/yyyy")} - {caixa.Funcionario.NMFUNC}" : "";

            ViewBag.operacao = "E";
            ViewBag.acao = "CorrecaoMoeda";
            var caixaMoeda = await _caixaService.ObterCaixaMoedaCompletoPorId(id);
            var objeto = _mapper.Map<CaixaMoedaViewModel>(caixaMoeda);
            if (objeto == null)
            {
                var msgErro = $"Caixa Moeda n�o localizado";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Caixa Moeda";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexCaixa");
            }
            objeto.CaixaNome = caixaMoeda.Caixa != null && caixaMoeda.Caixa.SQCAIXA > 0 ? caixaMoeda.Caixa.SQCAIXA.ToString() : string.Empty;
            objeto.MoedaNome = caixaMoeda.Moeda != null && !string.IsNullOrEmpty(caixaMoeda.Moeda.DSMOEDA) ? caixaMoeda.Moeda.DSMOEDA : string.Empty;
            objeto.UsuarioCorrecao = caixaMoeda.UsuarioCorrecao != null && !string.IsNullOrEmpty(caixaMoeda.UsuarioCorrecao.nome) ? caixaMoeda.UsuarioCorrecao.nome : string.Empty;


            return View("CorrecaoMoeda", objeto);
        }

        [Route("moeda/correcao")]
        [HttpPost]
        public async Task<IActionResult> CorrecaoMoeda(CaixaMoedaViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "CorrecaoMoeda";
            var caixa = _caixaService.ObterCompletoPorId(model.Id).Result;
            var caixaViewModel = _mapper.Map<CaixaindexViewModel>(caixa);
            if (!ModelState.IsValid)
            {
          

                ViewBag.caixa = caixa != null ? $@"{caixaViewModel.Sequencial.Value.ToString("D3")} - {caixaViewModel.DataAbertura.Value.ToString("dd/MM/yyyy")} - {caixa.Funcionario.NMFUNC}" : "";
                return View("CorrecaoMoeda", model);
            }
            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
            if (usuario == null)
            {
                ViewBag.caixa = caixa != null ? $@"{caixaViewModel.Sequencial.Value.ToString("D3")} - {caixaViewModel.DataAbertura.Value.ToString("dd/MM/yyyy")} - {caixaViewModel.Funcionario}" : "";
                NotificarErro("Erro ao tentar abrir Turno, usuario nao localizado");
                ObterNotificacoes("Moeda", "CorrecaoMoeda", "Web");
                return View("CorrecaoMoeda", model);

            }
            model.IDUSUARIOCORRECAO = usuario.Id;
            model.DataCorrecao = DateTime.Now;
            var caixaMoeda = _mapper.Map<CaixaMoeda>(model);
            
            await _caixaService.RealizarCorrecaoValor(caixaMoeda);

            if (!OperacaoValida())
            {
                ViewBag.caixa = caixa != null ? $@"{caixaViewModel.Sequencial.Value.ToString("D3")} - {caixaViewModel.DataAbertura.Value.ToString("dd/MM/yyyy")} - {caixaViewModel.Funcionario}" : "";
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Moeda", "CorrecaoMoeda", "Web");
                return View("CorrecaoMoeda", model);
            }

            await _caixaService.Salvar();

            TempData["Mensagem"] = "Opera��o realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMoeda", new { idCaixa = model.IDCAIXA });
        }

        private double ValorTotalMoedas(IEnumerable<CaixaMoedaViewModel> model)
        {
            double total = 0;
            total = model.Sum(x => x.ValorCorrecao.HasValue ? x.ValorCorrecao.Value : x.ValorOriginal.Value);
            return total;
        }
        #endregion

        #region Private
        private async Task<PagedViewModel<CaixaindexViewModel>> ObterListaCaixaPaginado(long idEmpresa, DateTime dtIni, DateTime dtFinal, int page, int pageSize)
        {
            var lista = new List<CaixaindexViewModel>();
            var retorno = await _caixaService.ObterPorPaginacao(idEmpresa, dtIni, dtFinal, page, pageSize);

            retorno.List.ToList().ForEach(caixa => {
                var caixaViewModel = _mapper.Map<CaixaindexViewModel>(caixa);
                caixaViewModel.Empresa = caixa.Empresa != null && !string.IsNullOrEmpty(caixa.Empresa.NMRZSOCIAL) ? caixa.Empresa.NMRZSOCIAL : string.Empty;
                caixaViewModel.Turno = caixa.Turno != null && caixa.Turno.NUTURNO > 0 ? caixa.Turno.NUTURNO.ToString() : string.Empty;
                caixaViewModel.PDV = caixa.PontoVenda != null && !string.IsNullOrEmpty(caixa.PontoVenda.DSPDV) ? caixa.PontoVenda.DSPDV : string.Empty;
                caixaViewModel.Funcionario = caixa.Funcionario != null && !string.IsNullOrEmpty(caixa.Funcionario.NMFUNC) ? caixa.Funcionario.NMFUNC : string.Empty;

                lista.Add(caixaViewModel);
            });

            return new PagedViewModel<CaixaindexViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "IndexCaixa",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<PagedViewModel<CaixaMovimentoViewModel>> ObterListaCaixaMovimentoPaginado(long idCaixa, int page, int pageSize)
        {
            var lista = new List<CaixaMovimentoViewModel>();
            var retorno = await _caixaService.ObterMovimentacaoPorPaginacao(idCaixa, page, pageSize);

            retorno.List.ToList().ForEach(mov => {
                var caixaMovViewModel = _mapper.Map<CaixaMovimentoViewModel>(mov);
                caixaMovViewModel.Caixa = mov.Caixa != null && mov.Caixa.SQCAIXA > 0 ? mov.Caixa.SQCAIXA.ToString() : string.Empty;

                lista.Add(caixaMovViewModel);
            });

            return new PagedViewModel<CaixaMovimentoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<PagedViewModel<CaixaMoedaViewModel>> ObterListaCaixaMoedaPaginado(long idCaixa, int page, int pageSize)
        {
            var lista = new List<CaixaMoedaViewModel>();
            var retorno = await _caixaService.ObterMoedaPorPaginacao(idCaixa, page, pageSize);

            retorno.List.ToList().ForEach(moeda => {
                var caixaMoedaViewModel = _mapper.Map<CaixaMoedaViewModel>(moeda);
                caixaMoedaViewModel.CaixaNome = moeda.Caixa != null && moeda.Caixa.SQCAIXA > 0 ? moeda.Caixa.SQCAIXA.ToString() : string.Empty;
                caixaMoedaViewModel.MoedaNome = moeda.Moeda != null && !string.IsNullOrEmpty(moeda.Moeda.DSMOEDA) ? moeda.Moeda.DSMOEDA : string.Empty;
                caixaMoedaViewModel.UsuarioCorrecao = moeda.UsuarioCorrecao != null && !string.IsNullOrEmpty(moeda.UsuarioCorrecao.nome) ? moeda.UsuarioCorrecao.nome : string.Empty;
                lista.Add(caixaMoedaViewModel);
            });

            return new PagedViewModel<CaixaMoedaViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        #endregion
    }
}

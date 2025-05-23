using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Turno;
using System.Linq;
using agilium.api.business.Models;

namespace agilum.mvc.web.Controllers
{
    [Route("turno")]
    [Authorize]
    public class TurnoController : MainController
    {
        #region constantes
        private readonly ITurnoService _turnoService;
        private readonly string _nomeEntidadeMotivo = "Turno";
        private readonly IEmpresaService _empresaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IPTurnoDapperRepository _TurnoDapperRepository;
        #endregion

        #region construtores
        public TurnoController(ITurnoService turnoService, IMapper mapper, IUsuarioService usuarioService,
                             IEmpresaService empresaService, IPTurnoDapperRepository turnoDapperRepository,
            INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper)
        {
            _turnoService = turnoService;
            _empresaService = empresaService;
            _usuarioService = usuarioService;
            _TurnoDapperRepository = turnoDapperRepository;
        }
        #endregion

        #region turno

        [Route("lista")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null)
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

            var lista = (await ObterListaPlanoContaPaginado(Convert.ToInt64(empresaSelecionada.IDEMPRESA), _dtini, _dtFim, page, ps));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            return View(lista);
        }


        [Route("abrir")]
        [HttpGet]
        public async Task<IActionResult> AbrirTurno()
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

            if (Convert.ToInt64(empresaSelecionada.IDEMPRESA) <= 0)
            {
                var msgErro = $"Selecione uma empresa para abrir Turno";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Turno";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Turno";
                ViewBag.Mensagem = msgErro;

                return RedirectToAction("Index", "Home");
            }
            var msgResultado = "";
            try
            {
                var turnoAberto = _turnoService.TurnoAbertoPorId(Convert.ToInt64(empresaSelecionada.IDEMPRESA)).Result;

                if (turnoAberto)
                {
                    var msgErro= "Já existe um turno aberto para a empresa atual";
                    NotificarErro(msgErro);

                    TempData["TipoMensagem"] = "danger";
                    TempData["Titulo"] = _nomeEntidadeMotivo;
                    TempData["Mensagem"] = msgErro;

                    return RedirectToAction("Index");
                }

                var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
                if (usuario == null)
                {
                    AdicionarErroValidacao("Erro ao tentar abrir Turno, usuario nao localizado");
                }

                if (!OperacaoValida())
                {
                    var msgErro = string.Join("\n\r", ObterNotificacoes("Turno", "AbrirTurno", "Web"));
                    AdicionarErroValidacao(msgErro);

                    TempData["TipoMensagem"] = "danger";
                    TempData["Titulo"] = _nomeEntidadeMotivo;
                    TempData["Mensagem"] = msgErro;

                    return RedirectToAction("Index");
                }

                await _turnoService.AbrirTurno(Convert.ToInt64(empresaSelecionada.IDEMPRESA), usuario.Id);
                msgResultado = "Turno Aberto com sucesso!";
            }
            catch
            {
                AdicionarErroValidacao("Erro ao tentar abrir turno");

            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                AdicionarErroValidacao(msgErro);

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidadeMotivo;
                TempData["Mensagem"] = msgErro;
            }

            LogInformacao($"TurnoAberto", "Turno", "AbrirTurno", null);
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";
            
            return RedirectToAction("Index");
        }

        [Route("fechar")]
        [HttpGet]
        public async Task<IActionResult> FecharTurno()
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

            var objeto = ObterPorId(Convert.ToInt64(empresaSelecionada.IDEMPRESA)).Result;
    
            if (objeto == null)
            {
                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Turno";
                TempData["Mensagem"] = "Não foi encontrado turno aberto";

                return RedirectToAction("Index");
            }

            return View(objeto);
        }

        [Route("fechar")]
        [HttpPost]
        public async Task<IActionResult> FecharTurno(TurnoIndexViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var msgResultado = "";
            try
            {
                var turnoAberto = _turnoService.TurnoAbertoPorId(Convert.ToInt64(viewModel.IDEMPRESA)).Result;

                if (!turnoAberto)
                {
                    NotificarErro("Não existe um turno aberto para a empresa atual");
                    return RedirectToAction("Index");
                }

                var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
                if (usuario == null)
                {
                    NotificarErro("Erro ao tentar fechar Turno, usuario nao localizado");
                }

                if (!OperacaoValida())
                {
                    var msgErro = string.Join("\n\r", ObterNotificacoes());
                    NotificarErro(msgErro);
                    return RedirectToAction("Index");
                }

                await _turnoService.FecharTurno(viewModel.IDEMPRESA.Value, usuario.Id, viewModel.Obs);
                msgResultado = "Turno fechado com sucesso!";
            }
            catch
            {
                NotificarErro("Erro ao tentar fechar turno");

            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                NotificarErro(msgErro);
            }
            LogInformacao($"Turno Fechado", "Turno", "FecharTurno", null);

            return RedirectToAction("Index");
        }
        #endregion

        #region private
        private async Task<PagedViewModel<TurnoIndexViewModel>> ObterListaPlanoContaPaginado(long idEmpresa, DateTime dtIni, DateTime dtFinal, int page, int pageSize)
        {
            var retorno = await _turnoService.ObterPorPaginacao(idEmpresa, dtIni, dtFinal, page, pageSize);

            var lista = _mapper.Map<IEnumerable<TurnoIndexViewModel>>(retorno.List);
            lista.ToList().ForEach(turno => {
                if (turno.IDEMPRESA.HasValue)
                {
                    var empresa = _empresaService.ObterPorId(turno.IDEMPRESA.Value).Result;
                    turno.Empresa = (empresa != null && !string.IsNullOrEmpty(empresa.NMRZSOCIAL)) ? empresa.NMRZSOCIAL : "Não localizado";
                }

                if (turno.IDUSUARIOINI.HasValue)
                {
                    var usuarioINicial = _usuarioService.ObterPorUsuarioPorId(turno.IDUSUARIOINI.Value).Result;
                    turno.UsuarioInicial = (usuarioINicial != null && !string.IsNullOrEmpty(usuarioINicial.nome)) ? usuarioINicial.nome : "";
                }
                if (turno.IDUSUARIOFIM.HasValue)
                {
                    var usuarioFinal = _usuarioService.ObterPorUsuarioPorId(turno.IDUSUARIOFIM.Value).Result;
                    turno.UsuarioFinal = (usuarioFinal != null && !string.IsNullOrEmpty(usuarioFinal.nome)) ? usuarioFinal.nome : "";
                }
            });

            return new PagedViewModel<TurnoIndexViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "lista",
                ReferenceController = "turno",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<TurnoIndexViewModel> ObterPorId(long id)
        {
            long _id = Convert.ToInt64(id);
            var turno = await _turnoService.ObterTurnoAbertoCompletoPorId(id);
            if(turno == null)
            {
                return null;

            }

            var objeto = _mapper.Map<TurnoIndexViewModel>(turno);
            objeto.Empresa = turno.Empresa != null && !string.IsNullOrEmpty(turno.Empresa.NMRZSOCIAL) ? turno.Empresa.NMRZSOCIAL : string.Empty;
            objeto.UsuarioInicial = turno.UsuarioInicial != null && !string.IsNullOrEmpty(turno.UsuarioInicial.nome) ? turno.UsuarioInicial.nome : string.Empty;
            objeto.UsuarioFinal = turno.UsuarioFinal != null && !string.IsNullOrEmpty(turno.UsuarioFinal.nome) ? turno.UsuarioFinal.nome : string.Empty;

                return objeto;
        }
        #endregion
    }
}

using agilium.webapp.manager.mvc.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using agilium.webapp.manager.mvc.ViewModels.TurnoViewModel;

namespace agilium.webapp.manager.mvc.Controllers
{

    [Route("turno")]
    public class TurnoController : MainController
    {
        private readonly ITurnoService _turnoService; 
        private readonly string _nomeEntidadeMotivo = "Turno";

        public TurnoController(ITurnoService turnoService)
        {
            _turnoService = turnoService;
        }

        #region Turno
        [Route("lista")]
        public async Task<IActionResult> IndexTurnos([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null)
        {

            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar Turno";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Turno";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Turno";
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

            if (ExibirErros())
            {

            }
            var lista = (await _turnoService.ObterPaginacaoPorData(_idEmpresaSelec, _dtini.ToString(), _dtFim.ToString(), page, ps));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            lista.ReferenceAction = "IndexTurnos";

            return View(lista);
        }

        [Route("abrir")]
        [HttpGet]
        public async Task<IActionResult> AbrirTurno()
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
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

            var resposta = _turnoService.AbrirTurno(_idEmpresaSelec).Result;
            if (!ResponsePossuiErros(resposta))
            {
                TempData["Mensagem"] = "Operação realizada com sucesso";
                TempData["TipoMensagem"] = "success";
            }

            return RedirectToAction("IndexTurnos");
        }

        [Route("fechar")]
        [HttpGet]
        public async Task<IActionResult> FecharTurno()
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para fechar Turno";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Turno";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Turno";
                ViewBag.Mensagem = msgErro;

                return RedirectToAction("Index", "Home");
            }

            var objeto = _turnoService.ObterTurnoIndexPorId(_idEmpresaSelec).Result;
            //TurnoFechamentoViewModel turnoViewModel = new TurnoFechamentoViewModel(objeto.Id,objeto.IDEMPRESA,objeto.IDUSUARIOINI,objeto.IDUSUARIOFIM,objeto.Data,
            //    objeto.NumeroTurno,objeto.DataInicial,objeto.DataFinal,objeto.Obs,objeto.Empresa,objeto.UsuarioInicial,objeto.UsuarioFinal);
            if(objeto == null)
            {
                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Turno";
                TempData["Mensagem"] = "Não foi encontrado turno aberto";

                return RedirectToAction("IndexTurnos");
            }

            return View(objeto);
        }

        [Route("fechar")]
        [HttpPost]
        public async Task<IActionResult> FecharTurno(TurnoIndexViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var resposta = await _turnoService.FecharTurno(viewModel);
            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao tentar fechar turno" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(viewModel);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexTurnos");
        }
        #endregion
    }
}

using agilium.webapp.manager.mvc.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace agilium.webapp.manager.mvc.Controllers
{
    [Route("log")]
    public class LogController : MainController
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [Route("lista")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 25, [FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null)
        {
            
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
            var lista = (await _logService.ObterPaginacaoPorData(_dtini.ToString(), _dtFim.ToString(), page, ps));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            lista.ReferenceAction = "Index";

            return View("Index", lista);
        }
    }
}

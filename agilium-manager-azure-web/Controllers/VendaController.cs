using agilium.webapp.manager.mvc.Interfaces;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;
using System;
using agilium.webapp.manager.mvc.ViewModels.VendaViewModel;
using System.Web.Helpers;
using agilium.webapp.manager.mvc.ViewModels.VendaReportViewModel;

namespace agilium.webapp.manager.mvc.Controllers
{
    [Route("venda")]
    public class VendaController : MainController
    {
        private readonly IVendaService _vendaService;

        public VendaController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        #region Venda
        [Route("lista")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null)
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
            var lista = (await _vendaService.ObterPaginacaoPorData(_dtini.ToString(), _dtFim.ToString(), page, ps));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            lista.ReferenceAction = "Index";

            return View(lista);
        }
        [Route("detalhes")]
        public async Task<ActionResult> VendaDetalhe(long idVenda)
        {
            var model = await _vendaService.ObterDetalheVendaPorId(idVenda);

            return PartialView("_vendaDetalhes", model);
        }

        [Route("espelho")]
        public async Task<ActionResult> VendaEspelho(long idVenda)
        {
            var model = await _vendaService.ObterEspelhoPorVenda(idVenda);

            return PartialView("_vendaEspelho", model);
        }

        [Route("dashboard")]
        public async Task<ActionResult> VendaDashboard(long idVenda)
        {
            var vendaRankingProduto = new VendaRankingProdutoIndexViewModel();
            vendaRankingProduto.dataInicial = DateTime.Now.AddMonths(-1);
            vendaRankingProduto.dataFinal = DateTime.Now;

            var model = await _vendaService.ObterVendaRankingPorduto(vendaRankingProduto);

        
            return PartialView("_vendaDashboard",model);
        }

        [HttpPost]
        [Route("dashboard")]
        public async Task<JsonResult> VendaDashboard()
        {
            var vendaRankingProduto = new VendaRankingProdutoIndexViewModel();
            vendaRankingProduto.dataInicial = DateTime.Now.AddMonths(-1);
            vendaRankingProduto.dataFinal = DateTime.Now;

            var model = await _vendaService.ObterVendaRankingPorduto(vendaRankingProduto);


            return Json(model.Ranking);
        }

        #endregion

        #region Venda Item
        [Route("itens")]
        public async Task<IActionResult> ListaItem(long idCaixa)
        {
            var itensVendas = _vendaService.ObterItensPorVenda(idCaixa).Result;

            return View("_itensVenda", itensVendas);
        }
        #endregion

        #region Venda Moeda
        [Route("formas-pagamento")]
        public async Task<IActionResult> ListaMoedas(long idCaixa)
        {
            var itensVendas = _vendaService.ObterMoedasPorVenda(idCaixa).Result;

            return View("_itensMoeda", itensVendas);
        }
        #endregion

        #region Report
        [Route("report/detalhada")]
        public async Task<IActionResult> ReportVendaDetalhada([FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null)
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
            var lista = (await _vendaService.ObterRelatorioVendaDetalhada(_dtini, _dtFim));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            return View("ReportVendaDetalhe",lista);
        }

        [Route("report/simples")]
        public async Task<IActionResult> ReportVendaSimples([FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null)
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
            var lista = (await _vendaService.ObterRelatorioVendaDetalhada(_dtini, _dtFim));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            return View("ReportVendaSimples", lista);
        }

        [Route("report/fornecedor")]
        public async Task<IActionResult> ReportVendaFornecedor([FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null)
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
            var lista = (await _vendaService.ObterRelatorioVendaPorFornecedor(_dtini, _dtFim));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            return View("ReportVendaFornecedor", lista);
        }

        [Route("report/moeda")]
        public async Task<IActionResult> ReportVendaMoeda([FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null)
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
            var lista = (await _vendaService.ObterRelatorioVendaPorMoeda(_dtini, _dtFim));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            return View("ReportVendaMoeda", lista);
        }

        [Route("report/diferenca")]
        public async Task<IActionResult> ReportVendaDiferenca([FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null)
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
            var lista = (await _vendaService.ObterVendasPorDiferenca(_dtini, _dtFim));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            return View("ReportVendaDiferenca", lista);
        }


        [Route("report/ranking")]
        public async Task<IActionResult> ReportVendaRanking()
        {
            var model = new VendaFiltroRankingViewModel();
            model.dataInicial = DateTime.Now;
            model.dataFinal = new DateTime(model.dataInicial.Year, model.dataInicial.Month, DateTime.DaysInMonth(model.dataInicial.Year, model.dataInicial.Month));
            model.Ordenacao = Enums.EOrdenacaoFiltroRanking.Venda;
            model.TipoResultado = Enums.EResultadoFiltroRanking.Grupo;


            var lista = await _vendaService.ObterVendasPorRanking(model);
            return View("ReportVendaRanking", lista);
        }

        [Route("report/ranking")]
        [HttpPost]
        public async Task<IActionResult> ReportVendaRanking(VendaFiltroRankingViewModel model)
        {
            if (!ModelState.IsValid) return View("ReportVendaRanking", model);

            if (model.dataInicial > model.dataFinal)
            {
                AdicionarErroValidacao("Data Final deve ser maior ou igual a data inicial");
            }

            if (ExibirErros())
            {
                return View("ReportVendaRanking", model);
            }
            var lista = await _vendaService.ObterVendasPorRanking(model);

            return View("ReportVendaRanking", lista);
        }

        #endregion
    }
}

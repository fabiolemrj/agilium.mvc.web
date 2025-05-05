using agilium.webapp.manager.mvc.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;
using agilium.webapp.manager.mvc.ViewModels.CaixaViewModel;
using System.Collections;
using System.Collections.Generic;

namespace agilium.webapp.manager.mvc.Controllers
{
    [Route("caixa")]
    public class CaixaController : MainController
    {
        private readonly ICaixaService _caixaService;

        public CaixaController(ICaixaService caixaService)
        {
            _caixaService = caixaService;
        }

        #region Caixa
        [Route("lista")]
        public async Task<IActionResult> IndexCaixa([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null)
        {

            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar Caixa";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Caixa";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Caixa";
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
            var lista = (await _caixaService.ObterPaginacaoPorData(_idEmpresaSelec, _dtini.ToString(), _dtFim.ToString(), page, ps));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            lista.ReferenceAction = "IndexCaixa";

            return View(lista);
        }
        #endregion

        #region Caixa Movimentacao
        [Route("movimentacao")]
        public async Task<IActionResult> IndexMovimentacao([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] long idCaixa =0)
        {
            var caixa = _caixaService.ObterCaixaPorId(idCaixa).Result;
            
            var lista = (await _caixaService.ObterPaginacaoMovimentacaoPorCaixa(idCaixa, page, ps));

            lista.ReferenceAction = "IndexMovimentacao";
            ViewBag.idCaixa = idCaixa;
            ViewBag.caixa = caixa != null ? $@"Caixa: {caixa.Sequencial.Value.ToString("D3")} - {caixa.DataAbertura.Value.ToString("dd/MM/yyyy")} - {caixa.Funcionario}" : "";

            return View(lista);
        }
        #endregion

        #region Caixa Moeda
        [Route("moedas")]
        public async Task<IActionResult> IndexMoeda([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] long idCaixa = 0)
        {
            var caixa = _caixaService.ObterCaixaPorId(idCaixa).Result;

            var lista = (await _caixaService.ObterPaginacaoMoedaPorCaixa(idCaixa, page, ps));

            lista.ReferenceAction = "IndexMoeda";
            ViewBag.idCaixa = idCaixa;
            ViewBag.caixa = caixa != null ? $@"Caixa: {caixa.Sequencial.Value.ToString("D3")} - {caixa.DataAbertura.Value.ToString("dd/MM/yyyy")} - {caixa.Funcionario}" : "";
            ViewBag.total = ValorTotalMoedas(lista.List);
            return View(lista);
        }

        [Route("moeda/correcao")]
        [HttpGet]
        public async Task<IActionResult> CorrecaoMoeda(long id, long idCaixa)
        {
            var caixa = _caixaService.ObterCaixaPorId(idCaixa).Result;

            ViewBag.caixa = caixa != null ? $@"{caixa.Sequencial.Value.ToString("D3")} - {caixa.DataAbertura.Value.ToString("dd/MM/yyyy")} - {caixa.Funcionario}" : "";

            ViewBag.operacao = "E";
            ViewBag.acao = "CorrecaoMoeda";
            var objeto = await _caixaService.ObterCaixaMoeidaPorId(id);
            if (objeto == null)
            {
                var msgErro = $"Caixa Moeda não localizado";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Caixa Moeda";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexCaixa");
            }
            
            return View("CorrecaoMoeda", objeto);
        }

        [Route("moeda/correcao")]
        [HttpPost]
        public async Task<IActionResult> CorrecaoMoeda(CaixaMoedaViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "CorrecaoMoeda";

            if (!ModelState.IsValid)
            {
                var caixa = _caixaService.ObterCaixaPorId(model.IDCAIXA.Value).Result;

                ViewBag.caixa = caixa != null ? $@"{caixa.Sequencial.Value.ToString("D3")} - {caixa.DataAbertura.Value.ToString("dd/MM/yyyy")} - {caixa.Funcionario}" : "";
                return View("CorrecaoMoeda", model);
            }
            var resposta = await _caixaService.RealizaCorrecaoValorMoeda(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var caixa = _caixaService.ObterCaixaPorId(model.IDCAIXA.Value).Result;

                ViewBag.caixa = caixa != null ? $@"{caixa.Sequencial.Value.ToString("D3")} - {caixa.DataAbertura.Value.ToString("dd/MM/yyyy")} - {caixa.Funcionario}" : "";

                var retornoErro = new { mensagem = $"Erro ao corrigir valor de moedas de caixa" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CorrecaoMoeda", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMoeda", new { idCaixa = model.IDCAIXA});
        }

        private double ValorTotalMoedas(IEnumerable<CaixaMoedaViewModel> model)
        {
            double total = 0;
            total = model.Sum(x => x.ValorCorrecao.HasValue ? x.ValorCorrecao.Value : x.ValorOriginal.Value);
            return total;
        }
        #endregion
    }
}

using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels.Contato;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using agilium.webapp.manager.mvc.ViewModels.PlanoContaViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    [Route("plano-conta")]
    public class PlanoContaController : MainController
    {
        private readonly IPlanoContaService _planoContaService;
        private readonly IEmpresaService _empresaService;
        private readonly string _nomeEntidade= "Plano de Conta";
        public List<PlanoContaViewModel> PlanosContas { get; set; } = new List<PlanoContaViewModel>();
        private List<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();

        public PlanoContaController(IPlanoContaService planoContaService, IEmpresaService empresaService)
        {
            _planoContaService = planoContaService;
            _empresaService = empresaService;

            listaEmpresaViewModels = _empresaService.ObterTodas().Result.ToList();
        }

        #region Plano Conta
        [Route("lista")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();
         
            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidade}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidade;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                
                return RedirectToAction("Index", "Home");
            }
            if(PlanosContas.Count == 0) 
                PlanosContas = _planoContaService.ObterTodas(_idEmpresaSelec).Result.ToList();

            var lista = (await _planoContaService.ObterPaginacaoPorDescricao(_idEmpresaSelec, q, page, ps));
            
            lista.List.ToList().ForEach(x => {
                var contaPai = PlanosContas.FirstOrDefault(y => y.Id == x.IDCONTAPAI);
                x.NomeContaPai = contaPai != null ? contaPai.Descricao : string.Empty;
            });

            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "Index";
            return View(lista);
        }

        private void AtualizarPlanosConta()
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec >= 0)
                if (PlanosContas.Count == 0)
                    PlanosContas = _planoContaService.ObterTodas(_idEmpresaSelec).Result.ToList();
            
        }

        [Route("novo")]
        [HttpGet]
        public async Task<IActionResult> CreatePlanoConta()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreatePlanoConta";

            AtualizarPlanosConta();

            var model = new PlanoContaViewModel();
            model.Situacao = Enums.EAtivo.Ativo;
            model.Id = 0;
            model.IDEMPRESA = ObterIdEmpresaSelecionada();
            model.PlanosContas = PlanosContas;
            model.Empresas = listaEmpresaViewModels;

            return View("CreateEditPlanoConta", model);
        }

        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> CreatePlanoConta(PlanoContaViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "CreatePlanoConta";
            AtualizarPlanosConta();

            if (!ModelState.IsValid)
            {
                model.PlanosContas = PlanosContas;
                model.Empresas = listaEmpresaViewModels;
                return View("CreateEditPlanoConta", model);
            }

            var resposta = await _planoContaService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo {_nomeEntidade}" };
                model.PlanosContas = PlanosContas;
                model.Empresas = listaEmpresaViewModels;

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditPlanoConta", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [HttpGet]
        public async Task<IActionResult> EditPlanoConta(long id)
        {
            AtualizarPlanosConta();
            ViewBag.operacao = "E";
            ViewBag.acao = "EditPlanoConta";
            var objeto = await _planoContaService.ObterProdutoPorId(id);
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
            objeto.Empresas = listaEmpresaViewModels;
            objeto.PlanosContas = PlanosContas;
            return View("CreateEditPlanoConta", objeto);
        }

        [Route("editar")]
        [HttpPost]
        public async Task<IActionResult> EditPlanoConta(PlanoContaViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditPlanoConta";
            AtualizarPlanosConta();
            model.Empresas = listaEmpresaViewModels;
            model.PlanosContas = PlanosContas;

            if (!ModelState.IsValid)
            {
                return View("CreateEditPlanoConta", model);
            }

            var resposta = await _planoContaService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar {_nomeEntidade}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditPlanoConta", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [HttpGet]
        public async Task<IActionResult> DeletePlanoConta(long id)
        {
            AtualizarPlanosConta();
            var objeto = await _planoContaService.ObterProdutoPorId(id);
            if (objeto == null)
            {
              

                var msgErro = $"{_nomeEntidade} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            objeto.PlanosContas = PlanosContas;
            objeto.Empresas = listaEmpresaViewModels;
            return View(objeto);
        }

        [Route("apagar")]
        [HttpPost]
        public async Task<IActionResult> DeletePlanoConta(PlanoContaViewModel model)
        {
            AtualizarPlanosConta();
            var resposta = await _planoContaService.Remover(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                model.PlanosContas = PlanosContas;
                model.Empresas = listaEmpresaViewModels;

                var retornoErro = new { mensagem = $"Erro ao tentar apagar {_nomeEntidade}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }
        #endregion

        #region Plano Conta Saldo

        [Route("saldo/atualizar")]
        [HttpGet]
        public async Task<IActionResult> AtualizarSaldoPorId(long id)
        {
            var resposta = _planoContaService.AtualizarSaldoPorConta(id).Result;
            if (!ResponsePossuiErros(resposta))
            {
                TempData["Mensagem"] = "Operação realizada com sucesso";
                TempData["TipoMensagem"] = "success";
            }

            return RedirectToAction("Index");
        }

        [Route("lacamentos")]
        public async Task<IActionResult> LancamentoPorPlano(long id)
        {
            var dataAtual = DateTime.Now;
            DateTime primeiroDiaDoMes = new DateTime(dataAtual.Year, dataAtual.Month, 1);
            DateTime ultimoDiaDoMes = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month));

            var model = new PlanoContaLancamentoListaViewModel();
            model.DataInicial = primeiroDiaDoMes;
            model.DataFinal = ultimoDiaDoMes;
            model.IdPlano = id;

            model.Lancamentos = _planoContaService.ObterLancamentosPorPlanoEData(model).Result.ToList();

            return View("_planoContaLancamento", model);
        }

        [Route("lacamentos")]
        [HttpPost]
        public async Task<IActionResult> LancamentoPorPlano(PlanoContaLancamentoListaViewModel model)
        {
            
            if (!ModelState.IsValid) return View("_planoContaLancamento", model);
            
            if (model.DataInicial == null)
                    AdicionarErroValidacao("Data Inicial é obrigatoria");
            
            if (model.DataFinal == null)
                    AdicionarErroValidacao("Data Final é obrigatoria");
            
            if(model.DataInicial.HasValue && model.DataFinal.HasValue)
            {
                if(model.DataFinal.Value < model.DataInicial.Value)
                    AdicionarErroValidacao("Data Final deve ser maior ou igual a data inicial");
            }
           

            if(!ExibirErros())
                model.Lancamentos = _planoContaService.ObterLancamentosPorPlanoEData(model).Result.ToList();

            return View("_planoContaLancamento", model);
        }

        [Route("lacamentos/lista")]
        public async Task<IActionResult> IndexLancamentos([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null, [FromQuery] long idConta =0)
        {
           

            var dataAtual = DateTime.Now;
            DateTime _dtini, _dtFim;
            if (DataInicial == null)
            {
                DateTime primeiroDiaDoMes = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                _dtini = primeiroDiaDoMes;
            }else _dtini = Convert.ToDateTime(DataInicial);

            if (DataFinal == null)
            {
                DateTime ultimoDiaDoMes = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month));
                _dtFim = ultimoDiaDoMes;
            }else _dtFim = Convert.ToDateTime(DataFinal);

            if(_dtini > _dtFim)
            {
                AdicionarErroValidacao("Data Final deve ser maior ou igual a data inicial");
            }

            if (ExibirErros())
            {

            }
            var lista = (await _planoContaService.ObterLancamentoPaginacaoPorDescricao(idConta, _dtini.ToString(),_dtFim.ToString(), page, ps));

            var planoConta = await _planoContaService.ObterProdutoPorId(idConta);

            if (planoConta != null && !string.IsNullOrEmpty(planoConta.Descricao))
                ViewBag.Conta = planoConta.Descricao;
            else
                ViewBag.Conta = "**Não Localizada**";

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;
            ViewBag.idConta = idConta;
            ViewBag.Saldo = CalcularSaldo(lista.List.ToList());

            lista.ReferenceAction = "IndexLancamentos";

            return View(lista);
        }

        private double CalcularSaldo(List<PlanoContaLancamentoViewModel> viewModel)
        {
            double resultado = 0;
            viewModel.ForEach(x => {
                var valorSomar = x.Tipo == Enums.ETipoContaLancacmento.Debito ? x.Valor * (-1) : x.Valor;
                resultado += valorSomar;
            });

            return resultado;
        }
        #endregion
    }
}

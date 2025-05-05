using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels.CategFinancViewModel;
using agilium.webapp.manager.mvc.ViewModels.Cliente;
using agilium.webapp.manager.mvc.ViewModels.ContaViewModel;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using agilium.webapp.manager.mvc.ViewModels.Fornecedor;
using agilium.webapp.manager.mvc.ViewModels.PlanoContaViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{

    [Route("conta")]
    public class ContaController : MainController
    {
        private readonly IContaService _contaService;
        private readonly IPlanoContaService _planoContaService;
        private readonly ICategoriaFinanceiraService _categoriaFinanceiraService;
        private readonly IFornecedorService _fornecedorService;
        private readonly IEmpresaService _empresaService;
        private readonly IClienteService _clienteService;

        private List<PlanoContaViewModel> PlanosContas { get; set; } = new List<PlanoContaViewModel>();
        private List<CategeoriaFinanceiraViewModel> CategoriasFinanceiras { get; set; } = new List<CategeoriaFinanceiraViewModel>();
        private List<FornecedorViewModel> Fornecedores { get; set; } = new List<FornecedorViewModel>();
        private List<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();
        private List<ClienteViewModel> Clientes { get; set; } = new List<ClienteViewModel>();

        public ContaController(IContaService contaService, IPlanoContaService planoContaService, 
            ICategoriaFinanceiraService categoriaFinanceiraService, IEmpresaService empresaService, IFornecedorService fornecedorService, 
            IClienteService clienteService)
        {
            _contaService = contaService;
            _planoContaService = planoContaService;
            _categoriaFinanceiraService = categoriaFinanceiraService;
            _empresaService = empresaService;
            _fornecedorService = fornecedorService;
            _clienteService = clienteService;
        }

        private void AtualizarListaAuxiliares()
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
                return;            

            if (listaEmpresaViewModels.Count == 0)
                listaEmpresaViewModels = _empresaService.ObterTodas().Result.ToList();

            if (PlanosContas.Count == 0)
                PlanosContas = _planoContaService.ObterTodas(_idEmpresaSelec).Result.ToList();

            if (Fornecedores.Count == 0)
                Fornecedores = _fornecedorService.ObterTodas().Result.ToList();

            if(CategoriasFinanceiras.Count == 0)
                CategoriasFinanceiras = _categoriaFinanceiraService.ObterTodos().Result.ToList();

            if(Clientes.Count ==0)
                Clientes = _clienteService.ObterTodas().Result.ToList();
        }

        private void PreencherListaAuxiliaresContaPagar(ContaPagarViewModel model)
        {
            AtualizarListaAuxiliares();

            if(model.CategoriasFinanceiras.Count == 0)
                model.CategoriasFinanceiras = CategoriasFinanceiras;
            if (model.Fornecedores.Count == 0)
                model.Fornecedores = Fornecedores;
            if (model.PlanosContas.Count == 0)
                model.PlanosContas = PlanosContas;
            if (model.Empresas.Count == 0)
                model.Empresas = listaEmpresaViewModels;            
        }

        private void PreencherListaAuxiliaresContaReceber(ContaReceberViewModel model)
        {
            AtualizarListaAuxiliares();

            if (model.CategoriasFinanceiras.Count == 0)
                model.CategoriasFinanceiras = CategoriasFinanceiras;
            if (model.Clientes.Count == 0)
                model.Clientes = Clientes;
            if (model.PlanosContas.Count == 0)
                model.PlanosContas = PlanosContas;
            if (model.Empresas.Count == 0)
                model.Empresas = listaEmpresaViewModels;
        }


        #region Conta Pagar
        [Route("pagar/lista")]
        public async Task<IActionResult> IndexContaPagar([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar Contas Pagar";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Contas Pagar";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Conta Pagar";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = (await _contaService.ObterPaginacaoPorDescricao(_idEmpresaSelec, q, page, ps)); ;

            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "IndexContaPagar";
            return View(lista);
        }

        [Route("pagar/novo")]
        [HttpGet]
        public async Task<IActionResult> CreateContaPagar()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateContaPagar";

            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            var model = new ContaPagarViewModel();
            model.Situacao = 1;
            model.Id = 0;
            model.IDEMPRESA = _idEmpresaSelec > 0 ? _idEmpresaSelec : 0;
            
            PreencherListaAuxiliaresContaPagar(model);
            
            return View("CreateEditContaPagar", model);
        }

        [Route("pagar/novo")]
        [HttpPost]
        public async Task<IActionResult> CreateContaPagar(ContaPagarViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "CreateContaPagar";
            PreencherListaAuxiliaresContaPagar(model);

            if (!ModelState.IsValid) return View("CreateEditContaPagar", model);
            
            model.DatCadastro = DateTime.Now;

            var resposta = await _contaService.AdicionarContaPagar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova conta a pagar" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditContaPagar", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexContaPagar");
        }

        [Route("pagar/editar")]
        [HttpGet]
        public async Task<IActionResult> EditContaPagar(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditContaPagar";
            var model = await _contaService.ObterContaPagarPorId(id);
            if (model == null)
            {
                var msgErro = $"Conta Pagar não localizada";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Produto";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexContaPagar");
            }
            PreencherListaAuxiliaresContaPagar(model);
            return View("CreateEditContaPagar", model);
        }

        [Route("pagar/editar")]
        [HttpPost]
        public async Task<IActionResult> EditContaPagar(ContaPagarViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditContaPagar";
            PreencherListaAuxiliaresContaPagar(model); 
            if (!ModelState.IsValid) return View("CreateEditContaPagar", model);

            var resposta = await _contaService.AtualizarContaPagar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar conta pagar" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditContaPagar", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexContaPagar");
        }

        [Route("pagar/apagar")]
        [HttpGet]
        public async Task<IActionResult> DeleteContaPagar(long id)
        {
            var model = await _contaService.ObterContaPagarPorId(id);
            PreencherListaAuxiliaresContaPagar(model);
            if (model == null)
            {
                var msgErro = $"Conta Pagar não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Conta Pagar";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexContaPagar");
            }

            return View(model);
        }

        [Route("pagar/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteContaPagar(ContaPagarViewModel model)
        {
            var resposta = await _contaService.RemoverContaPagar(model.Id);
            PreencherListaAuxiliaresContaPagar(model);
            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar Conta Pagar" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexContaPagar");
        }

        [Route("pagar/consolidar/{id}")]
        [HttpGet]
        public async Task<IActionResult> ConsolidarContaPagarPorId(long id)
        {
            var resposta = _contaService.RealizarConsolidacao(id).Result;
            if (!ResponsePossuiErros(resposta))
            {
                TempData["Mensagem"] = "Operação realizada com sucesso";
                TempData["TipoMensagem"] = "success";
            }

            return RedirectToAction("IndexContaPagar");
        }

        [Route("pagar/desconsolidar/{id}")]
        [HttpGet]
        public async Task<IActionResult> DesConsolidarContaPagarPorId(long id)
        {
            var resposta = _contaService.RealizarDesConsolidacao(id).Result;
            if (!ResponsePossuiErros(resposta))
            {
                TempData["Mensagem"] = "Operação realizada com sucesso";
                TempData["TipoMensagem"] = "success";
            }

            return RedirectToAction("IndexContaPagar");
        }
        #endregion

        #region Conta Receber
        
        [Route("receber/lista")]
        public async Task<IActionResult> IndexContaReceber([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar Contas Receber";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Contas Receber";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Conta Receber";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = (await _contaService.ObterContaReceberPaginacaoPorDescricao(_idEmpresaSelec, q, page, ps)); ;

            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "IndexContaReceber";
            return View(lista);
        }


        [Route("receber/novo")]
        [HttpGet]
        public async Task<IActionResult> CreateContaReceber()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateContaReceber";

            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            var model = new ContaReceberViewModel();
            model.Situacao = 1;
            model.Id = 0;
            model.IDEMPRESA = _idEmpresaSelec > 0 ? _idEmpresaSelec : 0;

            PreencherListaAuxiliaresContaReceber(model);

            return View("CreateEditContaReceber", model);
        }

        [Route("receber/novo")]
        [HttpPost]
        public async Task<IActionResult> CreateContaReceber(ContaReceberViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "CreateContaReceber";
            PreencherListaAuxiliaresContaReceber(model);

            if (!ModelState.IsValid) return View("CreateEditContaReceber", model);

            model.DatCadastro = DateTime.Now;

            var resposta = await _contaService.AdicionarContaReceber(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova conta a receber" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditContaReceber", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexContaReceber");
        }


        [Route("receber/editar")]
        [HttpGet]
        public async Task<IActionResult> EditContaReceber(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditContaReceber";
            var model = await _contaService.ObterContaReceberPorId(id);
            if (model == null)
            {
                var msgErro = $"Conta Receber não localizada";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Conta Receber";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexContaReceber");
            }
            PreencherListaAuxiliaresContaReceber(model);
            return View("CreateEditContaReceber", model);
        }

        [Route("receber/editar")]
        [HttpPost]
        public async Task<IActionResult> EditContaReceber(ContaReceberViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditContaReceber";
            PreencherListaAuxiliaresContaReceber(model);
            if (!ModelState.IsValid) return View("CreateEditContaReceber", model);

            var resposta = await _contaService.AtualizarContaReceber(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar conta Receber" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditContaReceber", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexContaReceber");
        }


        [Route("receber/apagar")]
        [HttpGet]
        public async Task<IActionResult> DeleteContaReceber(long id)
        {
            var model = await _contaService.ObterContaReceberPorId(id);
            PreencherListaAuxiliaresContaReceber(model);
            if (model == null)
            {
                var msgErro = $"Conta receber não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Conta Pagar";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexContaReceber");
            }

            return View(model);
        }

        [Route("receber/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteContaReceber(ContaReceberViewModel model)
        {
            var resposta = await _contaService.RemoverContaReceber(model.Id);
            PreencherListaAuxiliaresContaReceber(model);
            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar Conta receber" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexContaReceber");
        }

        [Route("receber/consolidar/{id}")]
        [HttpGet]
        public async Task<IActionResult> ConsolidarContaReceberPorId(long id)
        {
            var resposta = _contaService.RealizarConsolidacaoContaReceber(id).Result;
            if (!ResponsePossuiErros(resposta))
            {
                TempData["Mensagem"] = "Operação realizada com sucesso";
                TempData["TipoMensagem"] = "success";
            }

            return RedirectToAction("IndexContaReceber");
        }

        [Route("receber/desconsolidar/{id}")]
        [HttpGet]
        public async Task<IActionResult> DesConsolidarContaReceberPorId(long id)
        {
            var resposta = _contaService.RealizarDesConsolidacaoContaReceber(id).Result;
            if (!ResponsePossuiErros(resposta))
            {
                TempData["Mensagem"] = "Operação realizada com sucesso";
                TempData["TipoMensagem"] = "success";
            }

            return RedirectToAction("IndexContaReceber");
        }
        #endregion
    }
}

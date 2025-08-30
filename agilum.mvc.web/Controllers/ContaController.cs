using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilum.mvc.web.Enums;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.CategeoriaFinanceira;
using agilum.mvc.web.ViewModels.Cliente;
using agilum.mvc.web.ViewModels.Conta;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Fornecedor;
using agilum.mvc.web.ViewModels.PlanoConta;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Route("conta")]
    [Authorize]
    public class ContaController : MainController
    {
        private readonly IContaService _contaService;
        private readonly IPlanoContaService _planoContaService;
        private readonly ICategoriaFinanceiraService _categoriaFinanceiraService;
        private readonly IFornecedorService _fornecedorService;
        private readonly IEmpresaService _empresaService;
        private readonly IClienteService _clienteService;
        private readonly IPContaPagarDapperRepository _contaPagarDapperRepository;
        private readonly IPContaReceberDapperRepository _contaReceberDapperRepository;
        private readonly IUsuarioService _usuarioService;

        private readonly string _nomeEntidadePagar = "Contas a Pagar";

        #region construtor
        public ContaController(IUsuarioService usuarioService, IPContaPagarDapperRepository pContaPagarDapperRepository,
            IPContaReceberDapperRepository contaReceberDapperRepository, IContaService contaService, IPlanoContaService planoContaService,
            ICategoriaFinanceiraService categoriaFinanceiraService, IEmpresaService empresaService, IFornecedorService fornecedorService,
            IClienteService clienteService,
            INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, 
            ILogService logService, IMapper mapper) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper)
        {
            _contaService = contaService;
            _planoContaService = planoContaService;
            _categoriaFinanceiraService = categoriaFinanceiraService;
            _empresaService = empresaService;
            _fornecedorService = fornecedorService;
            _clienteService = clienteService;
            _usuarioService = usuarioService;
            _contaPagarDapperRepository = pContaPagarDapperRepository;
            _contaReceberDapperRepository = contaReceberDapperRepository;
        }
        #endregion

        #region listas auxiliares
        private List<PlanoContaViewModel> PlanosContas { get; set; } = new List<PlanoContaViewModel>();
        private List<CategeoriaFinanceiraViewModel> CategoriasFinanceiras { get; set; } = new List<CategeoriaFinanceiraViewModel>();
        private List<FornecedorViewModel> Fornecedores { get; set; } = new List<FornecedorViewModel>();
        private List<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();
        private List<ClienteViewModel> Clientes { get; set; } = new List<ClienteViewModel>();

        private void AtualizarListaAuxiliares()
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();
                      
            if (empresaSelecionada == null)
                return;

            if (listaEmpresaViewModels.Count == 0)
                listaEmpresaViewModels = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result.ToList());

            if (PlanosContas.Count == 0)
                PlanosContas = _mapper.Map<List<PlanoContaViewModel>>( _planoContaService.ObterTodas(Convert.ToInt64(empresaSelecionada.IDEMPRESA)).Result.ToList());

            if (Fornecedores.Count == 0)
                Fornecedores = _mapper.Map<List<FornecedorViewModel>>( _fornecedorService.ObterTodos().Result.ToList());

            if (CategoriasFinanceiras.Count == 0)
                CategoriasFinanceiras = _mapper.Map<List<CategeoriaFinanceiraViewModel>>( _categoriaFinanceiraService.ObterTodos().Result.ToList());

            if (Clientes.Count == 0)
                Clientes = _mapper.Map<List<ClienteViewModel>>(_clienteService.ObterTodos().Result.ToList());
        }

        private void PreencherListaAuxiliaresContaPagar(ContaPagarViewModel model)
        {
            AtualizarListaAuxiliares();

            if (model.CategoriasFinanceiras.Count == 0)
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
        #endregion

        #region contas pagar

        [Route("pagar/lista")]
        public async Task<IActionResult> IndexContaPagar([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidadePagar}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidadePagar;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadePagar;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }


            var lista = (await ObterListaContaPaginado(Convert.ToInt64(empresaSelecionada.IDEMPRESA), q, page, ps)); ;

            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "pagar/lista";
            return View(lista);
        }

        [Route("pagar/novo")]
        [HttpGet]
        public async Task<IActionResult> CreateContaPagar()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateContaPagar";

            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidadePagar}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidadePagar;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadePagar;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var model = new ContaPagarViewModel();
            model.Situacao = 1;
            model.Id = 0;
            model.IDEMPRESA = Convert.ToInt64(empresaSelecionada.IDEMPRESA) > 0 ? Convert.ToInt64(empresaSelecionada.IDEMPRESA) : 0;

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

            if (model.Id == 0) model.Id = await GerarId();

            if (model.IDCONTAPAI == null)
                model.IDCONTAPAI = model.Id;

            if (!model.IDUSUARIO.HasValue || model.IDUSUARIO.Value == 0)
            {
                var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
                if (usuario != null)
                    model.IDUSUARIO = usuario.Id;
            }

            var contaPagar = _mapper.Map<ContaPagar>(model);

            await _contaService.Adicionar(contaPagar);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova conta a pagar" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditContaPagar", model);
            }
            await _contaService.Salvar();

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
            var model = _mapper.Map<ContaPagarViewModel>(await _contaService.ObterCompletoPorId(id));
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

            var produto = _mapper.Map<ContaPagar>(model);

            await _contaService.Atualizar(produto);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao editar conta pagar" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditContaPagar", model);
            }

            await _contaService.Salvar();

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexContaPagar");
        }

        [Route("pagar/apagar")]
        [HttpGet]
        public async Task<IActionResult> DeleteContaPagar(long id)
        {
            var model = _mapper.Map<ContaPagarViewModel>(await _contaService.ObterCompletoPorId(id));
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
            await _contaService.Apagar(model.Id);
            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar Conta Pagar" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            await _contaService.Salvar();
            PreencherListaAuxiliaresContaPagar(model);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexContaPagar");
        }

        [Route("pagar/consolidar/{id}")]
        [HttpGet]
        public async Task<IActionResult> ConsolidarContaPagarPorId(long id)
        {
            var msgResultado = "";
            try
            {
                await _contaService.ConsolidarContaPorId(id);
                msgResultado = "Conta a pagar consolidada com sucesso!";
            }
            catch
            {
                NotificarErro("Erro ao tentar Conta a pagar consolidar conta a pagar");

            }

            if (OperacaoValida())
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
            var msgResultado = "";
            try
            {
                await _contaService.DesconsolidarContaReceberPorId(id);
                msgResultado = "Conta a pagar desconsolidada com sucesso!";
            }
            catch
            {
                NotificarErro("Erro ao tentar Conta a pagar desconsolidar conta a pagar");

            }

            if (OperacaoValida())
            {
                TempData["Mensagem"] = "Operação realizada com sucesso";
                TempData["TipoMensagem"] = "success";
            }

            return RedirectToAction("IndexContaPagar");
        }
        #endregion

        #region contas receber

        [Route("receber/lista")]
        public async Task<IActionResult> IndexContaReceber([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {

            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidadePagar}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Contas Receber";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Conta Receber";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = (await ObterListaContaReceberPaginado(Convert.ToInt64(empresaSelecionada.IDEMPRESA), q, page, ps)); ;

            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "receber/lista";
            return View(lista);
        }

        [Route("receber/novo")]
        [HttpGet]
        public async Task<IActionResult> CreateContaReceber()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateContaReceber";

            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            var model = new ContaReceberViewModel();
            model.Situacao = 1;
            model.Id = 0;
            model.IDEMPRESA = empresaSelecionada != null && Convert.ToInt64(empresaSelecionada.IDEMPRESA) > 0 ? Convert.ToInt64(empresaSelecionada.IDEMPRESA) : 0;

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

            if (model.Id == 0) model.Id = await GerarId();

            if (model.IDCONTAPAI == null)
                model.IDCONTAPAI = model.Id;

            if (!model.IDUSUARIO.HasValue || model.IDUSUARIO.Value == 0)
            {
                var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
                if (usuario != null)
                    model.IDUSUARIO = usuario.Id;
            }

            var objeto = _mapper.Map<ContaReceber>(model);

            await _contaService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova conta a receber" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditContaReceber", model);
            }
            await _contaService.Salvar();

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
            var model = _mapper.Map<ContaReceberViewModel>(await _contaService.ObterContaReceberCompletoPorId(id));
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

            var objeto = _mapper.Map<ContaReceber>(model);

            await _contaService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao editar conta Receber" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditContaReceber", model);
            }

            await _contaService.Salvar();
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexContaReceber");
        }

        [Route("receber/apagar")]
        [HttpGet]
        public async Task<IActionResult> DeleteContaReceber(long id)
        {
            var model = _mapper.Map<ContaReceberViewModel>(await _contaService.ObterContaReceberCompletoPorId(id));
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
            await _contaService.ApagarContaReceber(model.Id);
            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar Conta receber" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            await _contaService.Salvar();

            PreencherListaAuxiliaresContaReceber(model);
           
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexContaReceber");
        }

        [Route("receber/consolidar/{id}")]
        [HttpGet]
        public async Task<IActionResult> ConsolidarContaReceberPorId(long id)
        {
            var msgResultado = "";
            try
            {
                await _contaService.ConsolidarContaReceberPorId(id);
                msgResultado = "Conta a receber consolidada com sucesso!";
            }
            catch
            {
                NotificarErro("Erro ao tentar consolidar conta a receber");

            }

            if (OperacaoValida())
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
            var msgResultado = "";
            try
            {
                await _contaService.DesconsolidarContaReceberPorId(id);
                msgResultado = "Conta a receber desconsolidada com sucesso!";
            }
            catch
            {
                NotificarErro("Erro ao tentar desconsolidar conta a receber");

            }

            if (OperacaoValida())
            {
                TempData["Mensagem"] = "Operação realizada com sucesso";
                TempData["TipoMensagem"] = "success";
            }

            return RedirectToAction("IndexContaReceber");
        }
        #endregion

        #region Private
        private async Task<PagedViewModel<ContaPagarViewModelIndex>> ObterListaContaPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _contaService.ObterPorPaginacao(idEmpresa, filtro, page, pageSize);

            var listaContaPagarViewModel = new List<ContaPagarViewModelIndex>();

            retorno.List.ToList().ForEach(contaPagar => {

                var viewModel = new ContaPagarViewModelIndex();
                viewModel.IDCONTAPAI = contaPagar.IDCONTAPAI;
                viewModel.IDCATEG_FINANC = contaPagar.IDCATEG_FINANC;
                viewModel.IDLANC = contaPagar.IDLANC;
                viewModel.IDEMPRESA = contaPagar.IDEMPRESA;
                viewModel.IDFORNEC = contaPagar.IDFORNEC;
                viewModel.IDUSUARIO = contaPagar.IDUSUARIO;
                viewModel.DatCadastro = contaPagar.DTCAD;
                viewModel.DataPagamento = contaPagar.DTPAG;
                viewModel.DataNotaFiscal = contaPagar.DTNF;
                viewModel.Descricao = contaPagar.DESCR;
                viewModel.Id = contaPagar.Id;
                viewModel.OBS = contaPagar.OBS;
                viewModel.ParcelaInicial = contaPagar.PARCINI;
                viewModel.NumeroNotaFiscal = contaPagar.NUMNF;
                viewModel.Situacao = contaPagar.STCONTA;
                viewModel.TipoConta = contaPagar.TPCONTA.Value == 1 ? agilium.api.business.Enums.ETipoConta.Eventual : agilium.api.business.Enums.ETipoConta.Fixa;
                viewModel.ValorAcrescimo = contaPagar.VLACRESC;
                viewModel.ValorConta = contaPagar.VLCONTA;
                viewModel.ValorDesconto = contaPagar.VLDESC;
                viewModel.DataVencimento = contaPagar.DTVENC;


                if (contaPagar.Fornecedor != null && !string.IsNullOrEmpty(contaPagar.Fornecedor.NMRZSOCIAL))
                    viewModel.Fornecedor = contaPagar.Fornecedor.NMRZSOCIAL;
                if (contaPagar.CategFinanc != null && !string.IsNullOrEmpty(contaPagar.CategFinanc.NMCATEG))
                    viewModel.CategoriaFinanceira = contaPagar.CategFinanc.NMCATEG;
                if (contaPagar.PlanoConta != null && !string.IsNullOrEmpty(contaPagar.PlanoConta.DSCONTA))
                    viewModel.Conta = contaPagar.PlanoConta.DSCONTA;
                listaContaPagarViewModel.Add(viewModel);
            });

            return new PagedViewModel<ContaPagarViewModelIndex>()
            {
                List = listaContaPagarViewModel,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "pagar/lista",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<PagedViewModel<ContaReceberViewModelIndex>> ObterListaContaReceberPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _contaService.ObterContaReceberPorPaginacao(idEmpresa, filtro, page, pageSize);

            var listaContaReceberViewModel = new List<ContaReceberViewModelIndex>();

            retorno.List.ToList().ForEach(contaRec => {

                var viewModel = new ContaReceberViewModelIndex();
                viewModel.IDCONTAPAI = contaRec.IDCONTAPAI;
                viewModel.IDCATEG_FINANC = contaRec.IDCATEG_FINANC;
                viewModel.IDLANC = contaRec.IDLANC;
                viewModel.IDEMPRESA = contaRec.IDEMPRESA;
                viewModel.IDCLIENTE = contaRec.IDCLIENTE;
                viewModel.IDUSUARIO = contaRec.IDUSUARIO;
                viewModel.DatCadastro = contaRec.DTCAD;
                viewModel.DataPagamento = contaRec.DTPAG;
                viewModel.DataNotaFiscal = contaRec.DTNF;
                viewModel.Descricao = contaRec.DESCR;
                viewModel.Id = contaRec.Id;
                viewModel.OBS = contaRec.OBS;
                viewModel.ParcelaInicial = contaRec.PARCINI;
                viewModel.NumeroNotaFiscal = contaRec.NUMNF;
                viewModel.Situacao = contaRec.STCONTA;
                viewModel.TipoConta = contaRec.TPCONTA.Value == 1 ? ETipoConta.Eventual : ETipoConta.Fixa;
                viewModel.ValorAcrescimo = contaRec.VLACRES;
                viewModel.ValorConta = contaRec.VLCONTA;
                viewModel.ValorDesconto = contaRec.VLDESC;
                viewModel.DataVencimento = contaRec.DTVENC;


                if (contaRec.Cliente != null && !string.IsNullOrEmpty(contaRec.Cliente.NMCLIENTE))
                    viewModel.Cliente = contaRec.Cliente.NMCLIENTE;
                if (contaRec.CategFinanc != null && !string.IsNullOrEmpty(contaRec.CategFinanc.NMCATEG))
                    viewModel.CategoriaFinanceira = contaRec.CategFinanc.NMCATEG;
                if (contaRec.PlanoConta != null && !string.IsNullOrEmpty(contaRec.PlanoConta.DSCONTA))
                    viewModel.Conta = contaRec.PlanoConta.DSCONTA;
                listaContaReceberViewModel.Add(viewModel);
            });

            return new PagedViewModel<ContaReceberViewModelIndex>()
            {
                List = listaContaReceberViewModel,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "receber/lista",
                TotalResults = retorno.TotalResults
            };
        }
        #endregion

    }
}

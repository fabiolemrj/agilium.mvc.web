using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium_manager_azure_business.Interfaces.IService;
using agilium.api.business.Models;
using agilum.mvc.web.Extensions;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.PlanoConta;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Authorize]
    [Route("plano-conta")]
    public class PlanoContaController : MainController
    {
        private readonly IPlanoContaService _planoContaService;
        private readonly IPlanoContaDapperRepository _planoContaDapperRepository;
        private readonly IEmpresaService _empresaService;

        private const string _nomeEntidadeDepart = "Plano de Conta";

        #region construtor
        public PlanoContaController(INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, IEmpresaService empresaService,
            ILogService logService, IMapper mapper, IPlanoContaService planoContaService, IPlanoContaDapperRepository planoContaDapperRepository, ILicencaService licencaService, SignInManager<CaUsuarioIdentity> signInManager) : 
            base(notificador, configuration, appUser, utilDapperRepository, logService, mapper, licencaService, signInManager)
        {
            _planoContaService = planoContaService;
            _planoContaDapperRepository = planoContaDapperRepository;
            _empresaService = empresaService;

            listaEmpresaViewModels = _mapper.Map<List<EmpresaViewModel>>( _empresaService.ObterTodas().Result.ToList());
        }
        #endregion

        #region listas auxiliares
        public List<PlanoContaViewModel> PlanosContas { get; set; } = new List<PlanoContaViewModel>();
        private List<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();
        #endregion

        #region plano conta
        [ClaimsAuthorizeAttribute(2073)]
        [Route("lista")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null, string tipoLancamento = null)
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidadeDepart}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidadeDepart;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeDepart;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            if (PlanosContas.Count == 0)
                PlanosContas = _mapper.Map<List<PlanoContaViewModel>>( await _planoContaService.ObterTodas(Convert.ToInt64(empresaSelecionada.IDEMPRESA)));

            var lista = (await ObterListaPlanoContaPaginado(Convert.ToInt64(empresaSelecionada.IDEMPRESA), q, page, ps,tipoLancamento));

            lista.List.ToList().ForEach(x => {
                var contaPai = PlanosContas.FirstOrDefault(y => y.Id == x.IDCONTAPAI);
                x.NomeContaPai = contaPai != null ? contaPai.Descricao : string.Empty;
            });

            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "lista";
            lista.ReferenceController = "plano-conta";
            lista.Query = q;
            ViewBag.TipoLancamento = tipoLancamento;
            return View(lista);
        }

        [Route("novo")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2074)]
        public async Task<IActionResult> CreatePlanoConta()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreatePlanoConta";
            
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            AtualizarPlanosConta();

            var model = new PlanoContaViewModel();
            model.Situacao = agilium.api.business.Enums.EAtivo.Ativo;
            model.Id = 0;
            model.IDEMPRESA = Convert.ToInt64(empresaSelecionada.IDEMPRESA);
            model.PlanosContas = PlanosContas;
            model.Empresas = listaEmpresaViewModels;
            model.Codigo = await _utilDapperRepository.GerarCodigo($"SELECT MAX(CAST(CDCONTA AS UNSIGNED)) AS CD FROM planoconta where IDEMPRESA={empresaSelecionada.IDEMPRESA}");

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
            if (model.Id == 0) model.Id = await GerarId();

            var planoConta = _mapper.Map<PlanoConta>(model);
          
            await _planoContaService.Adicionar(planoConta);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo {_nomeEntidadeDepart}" };
                model.PlanosContas = PlanosContas;
                model.Empresas = listaEmpresaViewModels;

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditPlanoConta", model);
            }
            await _planoContaService.Salvar();
            LogInformacao($"Novo {_nomeEntidadeDepart} criado: {planoConta.DSCONTA} - ID: {planoConta.Id}","novo", "CreatePlanoConta",null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2077)]
        public async Task<IActionResult> EditPlanoConta(long id)
        {
            AtualizarPlanosConta();
            ViewBag.operacao = "E";
            ViewBag.acao = "EditPlanoConta";
            var objeto = _mapper.Map<PlanoContaViewModel>(await _planoContaService.ObterCompletoPorId(id));
            if (objeto == null)
            {


                var msgErro = $"{_nomeEntidadeDepart} não localizado";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeDepart;
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

            var produto = _mapper.Map<PlanoConta>(model);

            await _planoContaService.Atualizar(produto);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao editar {_nomeEntidadeDepart}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditPlanoConta", model);
            }

            await _planoContaService.Salvar();
            LogInformacao($"Plano de Conta editado: {produto.DSCONTA} - ID: {produto.Id}", "editar", "EditPlanoConta", null);
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2075)]
        public async Task<IActionResult> DeletePlanoConta(long id)
        {
            AtualizarPlanosConta();
            var objeto = _mapper.Map<PlanoContaViewModel>(await _planoContaService.ObterCompletoPorId(id));
            if (objeto == null)
            {


                var msgErro = $"{_nomeEntidadeDepart} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeDepart;
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
            
            await _planoContaService.Apagar(model.Id);
            if (!OperacaoValida())
            {
                model.PlanosContas = PlanosContas;
                model.Empresas = listaEmpresaViewModels;

                var retornoErro = new { mensagem = $"Erro ao tentar apagar {_nomeEntidadeDepart}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            await _planoContaService.Salvar();
            LogInformacao($"Plano de Conta apagado: {model.Descricao} - ID: {model.Id}", "apagar", "DeletePlanoConta", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }
        #endregion

        #region plano conta saldo

        [Route("saldo/atualizar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2078)]
        public async Task<IActionResult> AtualizarSaldoPorId(long id)
        {
            await _planoContaDapperRepository.AtualizarSaldoContaESubConta(id);
            if (OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("PlanoConta", "AtualizarSaldoPorId", "Web", $"id:{id}"));
                AdicionarErroValidacao(msgErro);
            }
            else
            {
                LogInformacao($"Saldo do Plano de Conta atualizado - ID: {id}", "atualizar", "AtualizarSaldoPorId", null);
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

            model.Lancamentos = _mapper.Map <List<PlanoContaLancamentoViewModel>>(await _planoContaDapperRepository.ObterLancamentosPorPlanoEData(model.IdPlano, model.DataFinal.Value, model.DataFinal.Value));
            var viewModels = _mapper.Map<List<PlanoContaLancamentoViewModel>>(model.Lancamentos);
            viewModels.ToList().ForEach(plano => {
              //  plano.TipoConta = _planoContaDapperRepository.ObterDescricaoPlano(model.IdPlano).Result;
            });

            return View("_planoContaLancamento", model);
        }

        [Route("lacamentos-por-plano")]
        [HttpPost]
        public async Task<IActionResult> LancamentoPorPlano(PlanoContaLancamentoListaViewModel model)
        {

            if (!ModelState.IsValid) return View("_planoContaLancamento", model);

            if (model.DataInicial == null)
                AdicionarErroValidacao("Data Inicial é obrigatoria");

            if (model.DataFinal == null)
                AdicionarErroValidacao("Data Final é obrigatoria");

            if (model.DataInicial.HasValue && model.DataFinal.HasValue)
            {
                if (model.DataFinal.Value < model.DataInicial.Value)
                    AdicionarErroValidacao("Data Final deve ser maior ou igual a data inicial");
            }


            if (OperacaoValida())
                model.Lancamentos = _mapper.Map<List<PlanoContaLancamentoViewModel>>(await _planoContaDapperRepository.ObterLancamentosPorPlanoEData(model.IdPlano, model.DataFinal.Value, model.DataFinal.Value));

            return View("_planoContaLancamento", model);
        }


        
        [ClaimsAuthorizeAttribute(2073)]
        [HttpGet("lacamentos-por-data")]
        public async Task<IActionResult> IndexLancamentos([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null, [FromQuery] long idConta = 0, string tipoLancamento = null)
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

            if (OperacaoValida())
            {

            }
            var lista = (await ObterListaPlanoContaPaginado(idConta, _dtini, _dtFim, page, ps, tipoLancamento));

            var planoConta = _mapper.Map<PlanoContaViewModel>(await _planoContaService.ObterCompletoPorId(idConta));

            if (planoConta != null && !string.IsNullOrEmpty(planoConta.Descricao))
                ViewBag.Conta = planoConta.Descricao;
            else
                ViewBag.Conta = "**Não Localizada**";

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;
            ViewBag.idConta = idConta;
            ViewBag.Saldo = CalcularSaldo(lista.List.ToList());

            ViewBag.TipoLancamento = tipoLancamento;

            lista.ReferenceAction = "lacamentos-por-data";
            lista.ReferenceController = "plano-conta";

            return View("ListaLancamentos",lista);
        }

        #endregion

        #region Private
        private async Task<PagedViewModel<PlanoContaViewModel>> ObterListaPlanoContaPaginado(long idEmpresa, string filtro, int page, int pageSize, string tipoLancamento)
        {
            var retorno = await _planoContaService.ObterPorPaginacao(idEmpresa, filtro, page, pageSize,tipoLancamento);

            var lista = _mapper.Map<IEnumerable<PlanoContaViewModel>>(retorno.List);

            lista.ToList().ForEach(saldo => {
                saldo.Saldo = _planoContaService.ObterSaldoPorIdPlano(saldo.Id).Result;
            });

            return new PagedViewModel<PlanoContaViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "lista",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<PagedViewModel<PlanoContaLancamentoViewModel>> ObterListaPlanoContaPaginado(long idPlano, DateTime dtIni, DateTime dtFinal, int page, int pageSize, string tipoLancamento)
        {
            var retorno = await _planoContaService.ObterLancamentoPorPaginacao(idPlano, dtIni, dtFinal, page, pageSize, tipoLancamento);

            var lista = _mapper.Map<IEnumerable<PlanoContaLancamentoViewModel>>(retorno.List);

            return new PagedViewModel<PlanoContaLancamentoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private void AtualizarPlanosConta()
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (Convert.ToInt64(empresaSelecionada.IDEMPRESA) >= 0)
                if (PlanosContas.Count == 0)
                    PlanosContas = _mapper.Map <List<PlanoContaViewModel>>(_planoContaService.ObterTodas(Convert.ToInt64(empresaSelecionada.IDEMPRESA)).Result.ToList());

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

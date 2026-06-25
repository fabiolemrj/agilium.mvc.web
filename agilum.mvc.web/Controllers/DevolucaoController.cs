
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Services;
using agilium_manager_azure_business.Interfaces.IService;
using agilium.api.business.Models;
using agilum.mvc.web.Enums;
using agilum.mvc.web.Extensions;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Cliente;
using agilum.mvc.web.ViewModels.Devolucao;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Venda;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Route("devolucao")]
    [Authorize]
    public class DevolucaoController : MainController
    {
        #region constantes
        private readonly IDevolucaoService _devolucaoService;
        private readonly IEmpresaService _empresaService;
        private readonly IVendaService _vendaService;
        private readonly IClienteService _clienteService;
        private readonly IValeService _valeService;
        private readonly IDevolucaoDapperRepository _devolucaoDapperRepository;
        private readonly IUsuarioService _usuarioService;
        #endregion

        #region Listas auxiliares
        private readonly string _nomeEntidadeMotivo = "Motivos de Devolução";
        private IEnumerable<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();
        private IEnumerable<VendaViewModel> listaVendasViewModel { get; set; } = new List<VendaViewModel>();
        private List<ClienteViewModel> listaClienteViewModel { get; set; } = new List<ClienteViewModel>();
        #endregion

        #region construtores
        public DevolucaoController(IDevolucaoService devolucaoService, IEmpresaService empresaService,
            IVendaService vendaService, IUsuarioService usuarioService,
            IClienteService clienteService, IValeService valeService, IDevolucaoDapperRepository devolucaoDapperRepository,
            INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService, IMapper mapper,
            ILicencaService licencaService, SignInManager<CaUsuarioIdentity> signInManager) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper, licencaService, signInManager)
        {
            _devolucaoService = devolucaoService;
            _empresaService = empresaService;
            _clienteService = clienteService;
            _vendaService = vendaService;
            _valeService = valeService;
            _devolucaoDapperRepository = devolucaoDapperRepository;
            _usuarioService = usuarioService;

            if (!listaEmpresaViewModels.Any())
                listaEmpresaViewModels = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);

            if (!listaClienteViewModel.Any())
                listaClienteViewModel = _mapper.Map<List<ClienteViewModel>>(_clienteService.ObterTodos().Result);
        }
        #endregion

        #region Devolução
        [Route("lista")]
        [ClaimsAuthorizeAttribute(1)]
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

            var lista = (await ObterListaDevolucaoPaginado(Convert.ToInt64(empresaSelecionada.IDEMPRESA), _dtini, _dtFim, page, ps));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            lista.ReferenceAction = "lista";

            return View("Index", lista);
        }


        [Route("itens")]
        public async Task<ActionResult> ObterItemDevolucao(long id)
        {

            var model = _mapper.Map<List<DevolucaoItemViewModel>>( await _devolucaoService.ObterDevolucaoItemPorId(id));

            if (model != null && model.Count > 0)
                ViewBag.devolucao = model.FirstOrDefault().DevolucaoNome;

            return PartialView("_indexItemDevolucao", model);
        }


        [Route("ObterItemVendaPorId")]
        public async Task<ActionResult> ObterItemVendaPorId(string idvenda, string iddev)
        {
            long _id = Convert.ToInt64(idvenda);
            long _iddev = Convert.ToInt64(iddev);
            var model = _mapper.Map<List<DevolucaoItemVendaViewModel>>(_devolucaoDapperRepository.ObterItensComVendaItens(Convert.ToInt64(idvenda), Convert.ToInt64(iddev)).Result);

            return new JsonResult(new { model });
        }

        [Route("ObterVendaPorData")]
        [ClaimsAuthorizeAttribute(1)]
        public async Task<ActionResult> ObterVendaPorData(string data)
        {
            DateTime data_venda = await FormatarDataConsulta(data, true);
            var listaVendas = ( await _vendaService.ObterVendaPorData(data_venda, data_venda.AddDays(1)));
            var listaConvertida = new List<VendaViewModel>();

            listaVendas.ForEach(venda => {
                var vendaViewModel = _mapper.Map<VendaViewModel>(venda);
                vendaViewModel.CaixaNome = venda.Caixa != null && venda.Caixa.SQCAIXA.HasValue ? venda.Caixa.SQCAIXA.ToString() : string.Empty;
                vendaViewModel.PDVNome = venda.Caixa != null && venda.Caixa.PontoVenda != null && !string.IsNullOrEmpty(venda.Caixa.PontoVenda.DSPDV) ? venda.Caixa.PontoVenda.DSPDV : string.Empty;
                vendaViewModel.FuncionarioNome = venda.Caixa != null && venda.Caixa.Funcionario != null && !string.IsNullOrEmpty(venda.Caixa.Funcionario.NMFUNC) ? venda.Caixa.Funcionario.NMFUNC : string.Empty;

                listaConvertida.Add(vendaViewModel);
            });

            var viewDevolucao = new DevolucaoEditarViewModel();
            viewDevolucao.Idvenda = 0;

            listaConvertida.ToList().ForEach(venda => {
                var valor = venda.ValorTotal.HasValue ? venda.ValorTotal.Value : 0;
                if (viewDevolucao.Idvenda == 0)
                    viewDevolucao.Idvenda = venda.Id;

             viewDevolucao.VendasItens.Add(new DevolucaoItemEditarViewModel()
                {
                    idVenda = venda.Id.ToString(),
                    VendaNome = $@"Caixa: {venda.CaixaNome} - Venda: {venda.Sequencial} - Total: {valor.ToString("N")}"
                });
            });
         
            return new JsonResult(new { viewDevolucao });
        }

        [Route("novo")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(1)]
        public async Task<IActionResult> Create()
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


            ViewBag.operacao = "I";
            ViewBag.acao = "Create";

            var model = new DevolucaoViewModel();
            model.Situacao = ESituacaoDevolucao.Aberta;
            model.DataHora = DateTime.Now;
            model.IDEMPRESA = Convert.ToInt64(empresaSelecionada.IDEMPRESA);
            model.DataConsulta = new DateTime(DateTime.Now.Year, 7, 27);

            model.Id = 0;
            PopularListaAuxiliares(model);
            return View("CreateEdit", model);
        }

        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> Create(DevolucaoViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            PopularListaAuxiliares(model);
            if (!ModelState.IsValid) return View("CreateEdit", model);

            PopularDadosItemDevolucao(model);

            model.DataHora = DateTime.Now;

            if (model.Id == 0) model.Id = await GerarId();
            if (!model.DataHora.HasValue)
                model.DataHora = DateTime.Now;

            var devolucao = _mapper.Map<Devolucao>(model);

            await _devolucaoService.Adicionar(devolucao);

            if (model.DevolucaoItens.Count > 0)
            {
                model.DevolucaoItens.ForEach(item =>
                {
                    item.idDevolucao = devolucao.Id;

                });
            }

            if (!AdicionarItens(model.DevolucaoItens).Result)
                NotificarErro("Erro ao tentar adicionar Item da devolução");

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova devolução" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }
            LogInformacao($"incluir {Deserializar(devolucao)}", "Devolucao", "Adicionar", null);
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(1)]
        public async Task<IActionResult> Edit(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            var model =_mapper.Map<DevolucaoViewModel>( await _devolucaoService.ObterDevolucaoPorId(id));

            PopularListaAuxiliares(model);
            if (model == null)
            {
                var msgErro = $"Devolução/perda não localizada";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "perda/sobra";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            model.DataConsulta = !string.IsNullOrEmpty(model.VendaData) ? Convert.ToDateTime(model.VendaData) : model.DataConsulta;
            model.DevolucaoItens.ForEach(item => {
                if (item.idDevolucao == 0)
                    item.idDevolucao = model.Id;
            });


            return View("CreateEdit", model);
        }

        [HttpGet]
        [Route("cancelar")]
        [ClaimsAuthorizeAttribute(1)]
        public async Task<IActionResult> Cancel(long id)
        {
            var model = _mapper.Map<DevolucaoViewModel>(await _devolucaoService.ObterDevolucaoPorId(id));

            PopularListaAuxiliares(model);
            if (model == null)
            {
                var msgErro = $"Devolução não localizada";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Devolução";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View("Cancel", model);
        }

        [Route("cancelar")]
        [HttpPost]
        public async Task<IActionResult> Cancel(DevolucaoViewModel viewModel)
        {
            var devolucao = await _devolucaoService.ObterPorId(viewModel.Id);

            if (devolucao == null) return NotFound();

            if (devolucao.STDEV != agilium.api.business.Enums.ESituacaoDevolucao.Aberta)
            {
                PopularListaAuxiliares(viewModel);
                var retornoErro = new { mensagem = $"Erro ao tentar apagar Vale Presente" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(viewModel);
            }

            devolucao.Cancelar();

            await _devolucaoService.Atualizar(devolucao);

            if (!OperacaoValida())
            {
                PopularListaAuxiliares(viewModel);
                var retornoErro = new { mensagem = $"Erro ao tentar apagar Vale Presente" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(viewModel);
            }
            await _devolucaoService.Salvar();

            LogInformacao($"cancelar {Deserializar(devolucao)}", "Devolucao", "Cancelar", null);
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("gerar-vale/{id}")]
        [HttpGet]
        public async Task<IActionResult> GerarVale(long id)
        {
            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
            var nomeUsuario = usuario != null ? usuario.nome : AppUser.GetUserEmail();
            if (!_devolucaoDapperRepository.RealizarDevolucao(id, nomeUsuario).Result)
            {
                NotificarErro("Erro: Nao foi possivel realizar devolução");
            }

            if (!OperacaoValida())
            {
                TempData["Mensagem"] = "Operação realizada com sucesso";
                TempData["TipoMensagem"] = "success";
            }
            LogInformacao($"gerar-vale id:{id}", "Devolucao", "GerarVale", null);

            return RedirectToAction("Index");
        }

        [Route("realizar/{id}")]
        [HttpGet]
        public async Task<IActionResult> Realizar(long id)
        {
            await _valeService.GerarVale(id);
            if (!OperacaoValida())
            {
                TempData["Mensagem"] = "Operação realizada com sucesso";
                TempData["TipoMensagem"] = "success";
            }else
                LogInformacao($"realizar id:{id}", "Devolucao", "Realizar", null);

            return RedirectToAction("Index");
        }
        #endregion

        #region motivos
        [Route("motivo/lista")]
        [ClaimsAuthorizeAttribute(2150)]
        public async Task<IActionResult> IndexMotivos([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
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

            var lista = (await ObterMotivos(Convert.ToInt64(empresaSelecionada.IDEMPRESA), q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return View(lista);
        }
        [Route("motivo/novo")]
        [ClaimsAuthorizeAttribute(2151)]
        public async Task<IActionResult> CreateMotivo()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateMotivo";

            var model = new MotivoDevolucaoViewModel();
            model.situacao = agilium.api.business.Enums.EAtivo.Ativo;
            model.Empresas = listaEmpresaViewModels.ToList();
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();
            model.idEmpresa = Convert.ToInt64(empresaSelecionada.IDEMPRESA);
            return View("CreateEditMotivo", model);
        }

        [Route("motivo/novo")]
        [HttpPost]
        public async Task<IActionResult> CreateMotivo(MotivoDevolucaoViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateMotivo";
            if (!ModelState.IsValid) return View("CreateEditMotivo", model);

            var objeto = _mapper.Map<MotivoDevolucao>(model);

            if (objeto.Id == 0) objeto.Id = objeto.GerarId();
            await _devolucaoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Devolucao", "AdicionarMotivo", "Web"));
                return View("CreateEditMotivo", model);
            }

            await _devolucaoService.Salvar();
            LogInformacao($"incluir {Deserializar(objeto)}", "Devolucao", "AdicionarMotivo", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMotivos");
        }

        [Route("motivo/editar")]
        [ClaimsAuthorizeAttribute(2154)]
        public async Task<IActionResult> EditMotivo(long id)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditMotivo";
            MotivoDevolucaoViewModel objeto = await ObterMotivoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeMotivo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeMotivo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexMotivos");
            }
            objeto.Empresas = listaEmpresaViewModels.ToList();
            return View("CreateEditMotivo", objeto);
        }

        [Route("motivo/editar")]
        [HttpPost]
        public async Task<IActionResult> EditMotivo(MotivoDevolucaoViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditMotivo";

            if (!ModelState.IsValid) return View("CreateEditMotivo", model);
            var objeto = _mapper.Map<MotivoDevolucao>(model);

            await _devolucaoService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Devolucao", "AtualizarMotivo", "Web"));
                return View("CreateEditMotivo", model);
            }

            await _devolucaoService.Salvar();
            LogInformacao($"incluir {Deserializar(objeto)}", "Devolucao", "AtualizarMotivo", null);
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMotivos");
        }

        [Route("motivo/apagar")]
        [ClaimsAuthorizeAttribute(2152)]
        public async Task<IActionResult> DeleteMotivo(long id)
        {
            var objeto = await ObterMotivoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeMotivo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeMotivo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexMotivos");
            }
            objeto.Empresas = listaEmpresaViewModels.ToList();
            return View(objeto);
        }

        [Route("motivo/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteMotivo(MotivoDevolucaoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!_devolucaoService.ApagarMotivo(model.Id).Result)
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));

                NotificarErro(msgErro);
                return View(model);
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Devolucao", "ApagarMotivo", "Web"));
                return View(model);
            }
            await _devolucaoService.Salvar();
            LogInformacao($"excluir id:{model.Id}", "Devolucao", "ApagarMotivo", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMotivos");
        }
        #endregion

        #region metodos privados
        private async Task<PagedViewModel<MotivoDevolucaoViewModel>> ObterMotivos(long idempresa, string filtro, int page, int pageSize)
        {
            var retorno = await _devolucaoService.ObterMotivoPaginacaoPorDescricao(idempresa, filtro, page, pageSize);
            var listaTeste = retorno.List;
            var lista = _mapper.Map<IEnumerable<MotivoDevolucaoViewModel>>(listaTeste);

            return new PagedViewModel<MotivoDevolucaoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "IndexMotivos",
                ReferenceController = "devolucao",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<MotivoDevolucaoViewModel> ObterMotivoPorId(long id)
        {
            return _mapper.Map<MotivoDevolucaoViewModel>(await _devolucaoService.ObterPorIdMotivo(id));
        }


        private async Task<PagedViewModel<DevolucaoViewModel>> ObterListaDevolucaoPaginado(long idempresa, DateTime dtIni, DateTime dtFinal, int page, int pageSize)
        {
            var lista = new List<DevolucaoViewModel>();
            //  var retorno = await _devolucaoService.ObterDevolucaoPorPaginacao(idempresa, dtIni, dtFinal, page, pageSize);
            var retorno = await _devolucaoService.ObterDevolucaoPorPaginacao(idempresa, dtIni, dtFinal, page, pageSize);

            retorno.List.ToList().ForEach(dev => {
                var cliente = dev.IDCLIENTE.HasValue ? _clienteService.ObterPorId(dev.IDCLIENTE.Value).Result : null;
                var vendaViewModel = _mapper.Map<DevolucaoViewModel>(dev);
                vendaViewModel.ClienteNome = cliente != null && !string.IsNullOrEmpty(cliente.NMCLIENTE) ? cliente.NMCLIENTE : string.Empty;

                vendaViewModel.MotivoDevolucaoNome = dev.MotivoDevolucao != null && !string.IsNullOrEmpty(dev.MotivoDevolucao.DSMOTDEV) ? dev.MotivoDevolucao.DSMOTDEV : string.Empty;
                vendaViewModel.VendaNome = dev.Venda != null && dev.Venda.SQVENDA.HasValue ? dev.Venda.SQVENDA.ToString() : string.Empty;
                vendaViewModel.EmpresaNome = dev.Empresa != null && !string.IsNullOrEmpty(dev.Empresa.NMRZSOCIAL) ? dev.Empresa.NMRZSOCIAL : string.Empty;
                vendaViewModel.VendaData = dev.Venda != null && dev.Venda.DTHRVENDA.HasValue ? dev.Venda.DTHRVENDA.Value.ToString("dd/MM/yyyy") : string.Empty;
                vendaViewModel.CaixaNome = dev.Venda != null && dev.Venda.Caixa != null && dev.Venda.Caixa.SQCAIXA.HasValue ? dev.Venda.Caixa.SQCAIXA.Value.ToString() : string.Empty;
                if (vendaViewModel.IDVALE.HasValue)
                {
                    var vale = _valeService.ObterPorId(vendaViewModel.IDVALE.Value).Result;
                    vendaViewModel.ValeNome = vale != null && !string.IsNullOrEmpty(vale.CDVALE) ? vale.CDVALE : string.Empty;
                }

                lista.Add(vendaViewModel);
            });

            return new PagedViewModel<DevolucaoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        protected async Task<DateTime> FormatarDataConsulta(string data, bool Inicial)
        {
            DateTime _data = Convert.ToDateTime(data);

            return Inicial ? new DateTime(_data.Year, _data.Month, _data.Day, 0, 0, 0) : new DateTime(_data.Year, _data.Month, _data.Day, 23, 59, 59);
        }


        private void PopularListaAuxiliares(DevolucaoViewModel model)
        {
            if (model.Clientes.Count() == 0)
                model.Clientes = listaClienteViewModel;
            if (model.Empresas.Count() == 0)
                model.Empresas = listaEmpresaViewModels.ToList();
            if (model.MotivosDevolucao.Count() == 0)
                model.MotivosDevolucao = _mapper.Map<List<MotivoDevolucaoViewModel>>( _devolucaoService.ObterTodosMotivos().Result);
        }


        private static void PopularDadosItemDevolucao(DevolucaoViewModel model)
        {
            model.DevolucaoItens.ForEach(item =>
            {
                if (item.selecionado)
                {
                    item.QuantidadeDevolucao = item.QuantidadeVendida;
                    item.ValorDevolucao = item.ValorVendido;
                }
            });
        }

        private async Task<bool> AdicionarItens(List<DevolucaoItemVendaViewModel> viewModel)
        {
            var resultado = false;

            viewModel.ForEach(async item => {
                if (item.selecionado)
                {

                    var itemDevolucao = new DevolucaoItem(item.idDevolucao, item.idItemVenda, item.QuantidadeVendida, item.ValorTotal);

                    if (item.idDevolucaoItem > 0)
                        itemDevolucao.Id = item.idDevolucaoItem;

                    await _devolucaoService.AdicionarAtualizar(itemDevolucao);
                }

            });

            resultado = true;
            return resultado;
        }



        #endregion


    }

}

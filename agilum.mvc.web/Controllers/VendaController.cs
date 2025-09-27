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
using System.Linq;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Venda;
using agilium.api.business.Enums;
//using agilium.api.manager.ViewModels.VendaViewModel;
using System.Security.Cryptography;
using VendaItemViewModel = agilum.mvc.web.ViewModels.Venda.VendaItemViewModel;
using VendaMoedaViewModel = agilum.mvc.web.ViewModels.Venda.VendaMoedaViewModel;
using VendaEspelhoViewModel = agilum.mvc.web.ViewModels.Venda.VendaEspelhoViewModel;
using VendaViewModel = agilum.mvc.web.ViewModels.Venda.VendaViewModel;

using agilium.api.business.Services;
using agilum.mvc.web.Extensions;
using agilium_manager_azure_business.Interfaces.IService;
using agilum.mvc.web.Data;
using Microsoft.AspNetCore.Identity;

namespace agilum.mvc.web.Controllers
{
    [Route("venda")]
    [Authorize]
    public class VendaController : MainController
    {

        private readonly IVendaDapperRepository _vendaDapperRepository;
        private readonly IVendaService _vendaService;

        #region construtor
        public VendaController(IVendaDapperRepository vendaDapperRepository, IVendaService vendaService,
            INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService, 
            IMapper mapper, ILicencaService licencaService, SignInManager<AppUserAgiliumIdentity> signInManager) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper, licencaService, signInManager)
        {
            _vendaDapperRepository = vendaDapperRepository;
            _vendaService = vendaService;
        }
        #endregion

        #region venda

        [Route("lista")]
        [ClaimsAuthorizeAttribute(2159)]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null)
        {
            VerificarValidadeLicenca();
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
                        
            var lista = (await ObterListaVendaPaginado(_dtini, _dtFim, page, ps));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            lista.ReferenceAction = "lista";

            return View(lista);
        }

        [Route("detalhes")]
        [ClaimsAuthorizeAttribute(2159)]
        public async Task<ActionResult> VendaDetalhe(long idVenda)
        {
            var venda = _vendaService.ObterPorId(idVenda).Result;
            var detalheVenda = new VendaDetalhesViewModel();
            detalheVenda.VendaItens = _mapper.Map<List<VendaItemViewModel>>(ObterListaVendaItemPaginado(idVenda).Result.ToList());
            detalheVenda.VendaMoedas = _mapper.Map<List<VendaMoedaViewModel>>(ObterListaVendaMoedaPaginado(idVenda).Result.ToList());
            if (venda != null)
            {
                detalheVenda.SequencialVenda = $@"{venda.SQVENDA} - {venda.DTHRVENDA.Value.ToString("dd/MM/yyyy")}";
                detalheVenda.idVenda = idVenda;
            }


            return PartialView("_vendaDetalhes", detalheVenda);
        }

        [Route("espelho")]
        [ClaimsAuthorizeAttribute(2163)]
        public async Task<ActionResult> VendaEspelho(long idVenda)
        {
            var venda = _vendaService.ObterPorId(idVenda).Result;
            var espelhoVenda = _mapper.Map<VendaEspelhoViewModel>(_vendaService.ObterVendaEspelhoPorIdVenda(idVenda).Result);
            if (espelhoVenda != null && venda != null)
            {
                espelhoVenda.SequencialVenda = $@"{venda.SQVENDA} - {venda.DTHRVENDA.Value.ToString("dd/MM/yyyy")}";
            }else
                espelhoVenda = new VendaEspelhoViewModel();

            return PartialView("_vendaEspelho", espelhoVenda);
        }

        [Route("dashboard")]
        [ClaimsAuthorizeAttribute(2162)]
        public async Task<ActionResult> VendaDashboard(long idVenda)
        {
            var vendaRankingProduto = new VendaRankingProdutoIndexViewModel();
            vendaRankingProduto.dataInicial = DateTime.Now.AddYears(-2);
            vendaRankingProduto.dataFinal = DateTime.Now;

            var limiteItens = 8;

            var resultado = _vendaDapperRepository.ObterVendasRankingPorProduto(vendaRankingProduto.dataInicial, vendaRankingProduto.dataFinal).Result;
            var listaRanking = new List<VendaRankingProdutoViewModel>();
            var contador = 1;
            var outros = new VendaRankingProdutoViewModel()
            {
                valor = 0,
                produto = "Outros"
            };
            foreach (var item in resultado)
            {
                if (contador >= limiteItens)
                {
                    outros.valor += item.valor;
                }
                else
                {
                    var raking = new VendaRankingProdutoViewModel
                    {
                        produto = item.produto,
                        valor = item.valor
                    };
                    listaRanking.Add(raking);

                }
                contador++;
            };

            if (outros.valor > 0) listaRanking.Add(outros);

            vendaRankingProduto.Ranking = listaRanking;
            vendaRankingProduto.Total = vendaRankingProduto.Ranking.Sum(x => x.valor);


            return PartialView("_vendaDashboard", vendaRankingProduto);
        }

        [HttpPost]
        [Route("dashboard")]
        public async Task<JsonResult> VendaDashboard()
        {
            var vendaRankingProduto = new VendaRankingProdutoIndexViewModel();
            vendaRankingProduto.dataInicial = DateTime.Now.AddYears(-2);
            vendaRankingProduto.dataFinal = DateTime.Now;

            var limiteItens = 8;

            var resultado = _vendaDapperRepository.ObterVendasRankingPorProduto(vendaRankingProduto.dataInicial, vendaRankingProduto.dataFinal).Result;
            var listaRanking = new List<VendaRankingProdutoViewModel>();
            var contador = 1;
            var outros = new VendaRankingProdutoViewModel()
            {
                valor = 0,
                produto = "Outros"
            };
            foreach (var item in resultado)
            {

                if (contador >= limiteItens)
                {
                    outros.valor += item.valor;
                }
                else
                {
                    var raking = new VendaRankingProdutoViewModel
                    {
                        produto = item.produto,
                        valor = item.valor
                    };
                    listaRanking.Add(raking);

                }
                contador++;
            };

            if (outros.valor > 0) listaRanking.Add(outros);

            vendaRankingProduto.Ranking = listaRanking;
            vendaRankingProduto.Total = vendaRankingProduto.Ranking.Sum(x => x.valor);



            return Json(vendaRankingProduto.Ranking);
        }
        #endregion

        #region Venda Item
        [Route("itens")]
        public async Task<IActionResult> ListaItem(long idCaixa)
        {
            var itensVendas = (await ObterListaVendaItemPaginado(idCaixa));

            return View("_itensVenda", itensVendas);
        }
        #endregion

        #region Venda Moeda
        [Route("formas-pagamento")]
        [ClaimsAuthorizeAttribute(2159)]
        public async Task<IActionResult> ListaMoedas(long idCaixa)
        {
            var itensVendas = await ObterListaVendaMoedaPaginado(idCaixa);

            return View("_itensMoeda", itensVendas);
        }
        #endregion

        #region Report
        [Route("report/detalhada")]
        [ClaimsAuthorizeAttribute(2159)]
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
            var lista = await _vendaService.ObterRelatorioVendaDetalhada(_dtini, _dtFim);
            

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            return View("ReportVendaDetalhe", lista);
        }

        [Route("report/simples")]
        [ClaimsAuthorizeAttribute(2159)]
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

            var lista = (await _vendaService.ObterRelatorioVendaDetalhada(_dtini, _dtFim));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            return View("ReportVendaSimples", lista);
        }

        [Route("report/fornecedor")]
        [ClaimsAuthorizeAttribute(2159)]
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

            var lista = await _vendaService.ObterRelatorioVendaPorFornecedor(_dtini, _dtFim);

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            return View("ReportVendaFornecedor", lista);
        }

        [Route("report/moeda")]
        [ClaimsAuthorizeAttribute(2159)]
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

            var lista = (await _vendaService.ObterRelatorioVendaPorMoeda(_dtini, _dtFim));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            return View("ReportVendaMoeda", lista);
        }


        [Route("report/diferenca")]
        [ClaimsAuthorizeAttribute(2159)]
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

            var lista = _vendaService.ObterVendaDiferencaCaixa(_dtini, _dtFim).Result; 

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            return View("ReportVendaDiferenca", lista);
        }

        [Route("report/ranking")]
        [ClaimsAuthorizeAttribute(2159)]
        public async Task<IActionResult> ReportVendaRanking()
        {
            var model = new VendaFiltroRankingViewModel();
            model.dataInicial = DateTime.Now;
            model.dataFinal = new DateTime(model.dataInicial.Year, model.dataInicial.Month, DateTime.DaysInMonth(model.dataInicial.Year, model.dataInicial.Month));
            model.Ordenacao = EOrdenacaoFiltroRanking.Venda;
            model.TipoResultado = EResultadoFiltroRanking.Grupo;
            try
            {

                model.ListaVendas = await _vendaService.ObterVendaRankingPorData(model.dataInicial, model.dataFinal, model.TipoResultado, model.Ordenacao);

            }
            catch (Exception ex)
            {
                AdicionarErroValidacao(ex.Message);
                throw;
            }
            return View("ReportVendaRanking", model);
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

            if (!OperacaoValida())
            {
                return View("ReportVendaRanking", model);
            }
            model.ListaVendas = _vendaService.ObterVendaRankingPorData(model.dataInicial, model.dataFinal, model.TipoResultado, model.Ordenacao).Result;

            return View("ReportVendaRanking", model);
        }
        #endregion

        #region Private
        private async Task<PagedViewModel<VendaViewModel>> ObterListaVendaPaginado(DateTime dtIni, DateTime dtFinal, int page, int pageSize)
        {
            var lista = new List<VendaViewModel>();
            var retorno = await _vendaService.ObterPorPaginacao(dtIni, dtFinal, page, pageSize);

            retorno.List.ToList().ForEach(venda => {
                var vendaViewModel = _mapper.Map<VendaViewModel>(venda);
                vendaViewModel.CaixaNome = venda.Caixa != null && venda.Caixa.SQCAIXA.HasValue ? venda.Caixa.SQCAIXA.ToString() : string.Empty;
                vendaViewModel.PDVNome = venda.Caixa != null && venda.Caixa.PontoVenda != null && !string.IsNullOrEmpty(venda.Caixa.PontoVenda.DSPDV) ? venda.Caixa.PontoVenda.DSPDV : string.Empty;
                vendaViewModel.FuncionarioNome = venda.Caixa != null && venda.Caixa.Funcionario != null && !string.IsNullOrEmpty(venda.Caixa.Funcionario.NMFUNC) ? venda.Caixa.Funcionario.NMFUNC : string.Empty;

                lista.Add(vendaViewModel);
            });

            return new PagedViewModel<VendaViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<List<VendaItemViewModel>> ObterListaVendaItemPaginado(long idVenda)
        {
            var lista = new List<VendaItemViewModel>();
            var retorno = await _vendaService.ObterItensVenda(idVenda);

            retorno.ToList().ForEach(vendaItem =>
            {
                var vendaItemViewModel = _mapper.Map<VendaItemViewModel>(vendaItem);
                vendaItemViewModel.VendaNome = vendaItem.Venda != null ? vendaItem.Venda.SQVENDA.ToString() : string.Empty;
                vendaItemViewModel.ProdutoNome = vendaItem.Produto != null && !string.IsNullOrEmpty(vendaItem.Produto.NMPRODUTO) ? vendaItem.Produto.NMPRODUTO : string.Empty;
                vendaItemViewModel.CodigoProduto = vendaItem.Produto != null && !string.IsNullOrEmpty(vendaItem.Produto.CDPRODUTO) ? vendaItem.Produto.CDPRODUTO : string.Empty;
                vendaItemViewModel.SituacaoProduto = vendaItem.Produto != null && vendaItem.Produto.STPRODUTO.HasValue ?
                                                    (vendaItem.Produto.STPRODUTO == EAtivo.Ativo ? "Ativo" : "Inativo") : string.Empty;

                lista.Add(vendaItemViewModel);
            });

            return lista;
        }

        private async Task<List<VendaMoedaViewModel>> ObterListaVendaMoedaPaginado(long idVenda)
        {
            var lista = new List<VendaMoedaViewModel>();
            var retorno = await _vendaService.ObterMoedasVenda(idVenda);

            retorno.ToList().ForEach(vendaMoeda =>
            {
                var vendaMoedaViewModel = _mapper.Map<VendaMoedaViewModel>(vendaMoeda);
                vendaMoedaViewModel.VendaNome = vendaMoeda.Venda != null ? vendaMoeda.Venda.SQVENDA.ToString() : string.Empty;
                vendaMoedaViewModel.MoedaNome = vendaMoeda.Moeda != null && !string.IsNullOrEmpty(vendaMoeda.Moeda.DSMOEDA) ? vendaMoeda.Moeda.DSMOEDA : string.Empty;

                lista.Add(vendaMoedaViewModel);
            });

            return lista;
        }

        #endregion

    }
}

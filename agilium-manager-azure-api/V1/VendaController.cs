using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.manager.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using agilium.api.manager.ViewModels.VendaViewModel;
using System.Linq;
using System.Collections;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models.CustomReturn.ReportViewModel.VendaReportViewModel;

namespace agilium.api.manager.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/venda")]
    [ApiVersion("1.0")]
    [ApiController]
    public class VendaController : MainController
    {
        private readonly IVendaDapperRepository _vendaDapperRepository;
        private readonly IVendaService _vendaService;
        private readonly IMapper _mapper;
        public VendaController(INotificador notificador, IUser appUser, IConfiguration configuration, IVendaService vendaService,
            IMapper mapper, IVendaDapperRepository vendaDapperRepository, 
            IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, configuration,
                utilDapperRepository,logService)
        {
            _vendaDapperRepository = vendaDapperRepository;
            _vendaService = vendaService;
            _mapper = mapper;
        }

        #region Venda
        [HttpGet("obter-por-data")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<VendaViewModel>>> ObterPorDataPaginacao([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string dtInicial = null, [FromQuery] string dtFinal = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();
            DateTime _dtInicial = Convert.ToDateTime(dtInicial);
            DateTime _dtFinal = Convert.ToDateTime(dtFinal);
            var lista = (await ObterListaVendaPaginado(_dtInicial, _dtFinal, page, ps));

            return CustomResponse(lista);
        }

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VendaViewModel>> ObterPorId(string id)
        {
            long _id = Convert.ToInt64(id);
            var vale = await _vendaService.ObterPorId(_id);
            if (vale != null)
            {
                var objeto = _mapper.Map<VendaViewModel>(vale);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Venda nao localizada"));

        }

        [HttpGet("detalhes-venda/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VendaDetalhes>> ObterDetalheVendaPorIdVenda(long id)
        {
            long _id = Convert.ToInt64(id);
            var venda = _vendaService.ObterPorId(id).Result;
            var detalheVenda = new VendaDetalhes();
            detalheVenda.VendaItens = _mapper.Map<List<VendaItemViewModel>>(ObterListaVendaItemPaginado(_id).Result.ToList());
            detalheVenda.VendaMoedas = _mapper.Map<List<VendaMoedaViewModel>>(ObterListaVendaMoedaPaginado(_id).Result.ToList());
            if (venda != null)
            {
                detalheVenda.SequencialVenda = $@"{venda.SQVENDA} - {venda.DTHRVENDA.Value.ToString("dd/MM/yyyy")}";
                detalheVenda.idVenda = id;
            }

            return CustomResponse(detalheVenda);

        }

        [HttpGet("vendas-por-data")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<VendaViewModel>>> ObterPorData([FromQuery] string dtInicial = null, [FromQuery] string dtFinal = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();
            DateTime _dtInicial = await FormatarDataConsulta(dtInicial,true);
            DateTime _dtFinal = await FormatarDataConsulta(dtFinal, false);
            var lista = _vendaService.ObterVendaPorData(_dtInicial, _dtFinal).Result.ToList();

            var listaConvertida = new List<VendaViewModel>();

            lista.ForEach(venda => {
                var vendaViewModel = _mapper.Map<VendaViewModel>(venda);
                vendaViewModel.CaixaNome = venda.Caixa != null && venda.Caixa.SQCAIXA.HasValue ? venda.Caixa.SQCAIXA.ToString() : string.Empty;
                vendaViewModel.PDVNome = venda.Caixa != null && venda.Caixa.PontoVenda != null && !string.IsNullOrEmpty(venda.Caixa.PontoVenda.DSPDV) ? venda.Caixa.PontoVenda.DSPDV : string.Empty;
                vendaViewModel.FuncionarioNome = venda.Caixa != null && venda.Caixa.Funcionario != null && !string.IsNullOrEmpty(venda.Caixa.Funcionario.NMFUNC) ? venda.Caixa.Funcionario.NMFUNC : string.Empty;

                listaConvertida.Add(vendaViewModel);
            });


            return CustomResponse(listaConvertida);
        }
        #endregion

        #region Venda Item
        [HttpGet("itens/{idCaixa}")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<VendaItemViewModel>>> ObterItemPorVenda(long idCaixa)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();
            var lista = (await ObterListaVendaItemPaginado(idCaixa));

            return CustomResponse(lista);
        }

        [HttpGet("moedas/{idCaixa}")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<VendaMoedaViewModel>>> ObterMoedaPorVenda(long idCaixa)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();
            var lista = (await ObterListaVendaMoedaPaginado(idCaixa));

            return CustomResponse(lista);
        }
        #endregion

        #region Venda Espelho
        [HttpGet("espelho/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VendaEspelhoViewModel>> ObterVendaEselhoPorId(long id)
        {
            var venda = _vendaService.ObterPorId(id).Result;
            var espelhoVenda = _mapper.Map<VendaEspelhoViewModel>(_vendaService.ObterVendaEspelhoPorIdVenda(id).Result);
            if(espelhoVenda == null)
            {
                NotificarErro("Espelho da venda não localizado");

            }

            if (espelhoVenda != null && venda != null)
            {
                espelhoVenda.SequencialVenda = $@"{venda.SQVENDA} - {venda.DTHRVENDA.Value.ToString("dd/MM/yyyy")}";
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Venda", "ObterEspelhoVenda", "Web");
                return CustomResponse(msgErro);
            }

            return CustomResponse(espelhoVenda);

        }

        //[AllowAnonymous]
        [HttpPost("ranking-por-produto")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VendaEspelhoViewModel>> ObterVendaRankingProduto([FromBody] VendaRankingProdutoIndexViewModel model)
        {
            var limiteItens = 8;

            var resultado = _vendaDapperRepository.ObterVendasRankingPorProduto(model.dataInicial,model.dataFinal).Result;
            var listaRanking = new List<VendaRankingProdutoViewModel>();
            var contador = 1;
            var outros = new VendaRankingProdutoViewModel() {
                valor = 0,
                produto = "Outros"
            };
            foreach (var item in resultado) {
                if(contador >= limiteItens)
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

            model.Ranking = listaRanking;
            model.Total = model.Ranking.Sum(x => x.valor);

            return CustomResponse(model);

        }
        #endregion

        #region Report
        [HttpGet("report/detalhada-por-data")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VendasReportViewModel>> ObterVendaDatlahePorData([FromQuery] string dtInicial = null, [FromQuery] string dtFinal = null)
        {
            DateTime _dtInicial = await FormatarDataConsulta(dtInicial, true);
            DateTime _dtFinal = await FormatarDataConsulta(dtFinal, false);

            var resultado = _vendaService.ObterRelatorioVendaDetalhada(_dtInicial, _dtFinal).Result;
            
            return CustomResponse(resultado);
        }

        [HttpGet("report/fornecedor-por-data")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VendasFornecedorViewModel>> ObterVendaFornecedorPorData([FromQuery] string dtInicial = null, [FromQuery] string dtFinal = null)
        {
            DateTime _dtInicial = await FormatarDataConsulta(dtInicial, true);
            DateTime _dtFinal = await FormatarDataConsulta(dtFinal, false);

            var resultado = _vendaService.ObterRelatorioVendaPorFornecedor(_dtInicial, _dtFinal).Result;

            return CustomResponse(resultado);
        }

        [HttpGet("report/moeda-por-data")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VendaMoedaReport>> ObterVendaMoedaPorData([FromQuery] string dtInicial = null, [FromQuery] string dtFinal = null)
        {
            DateTime _dtInicial = await FormatarDataConsulta(dtInicial, true);
            DateTime _dtFinal = await FormatarDataConsulta(dtFinal, false);

            var resultado = _vendaService.ObterRelatorioVendaPorMoeda(_dtInicial, _dtFinal).Result;

            return CustomResponse(resultado);
        }

        [HttpPost("report/ranking")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VendaFiltroRankingViewModel>> ObterVendaRanking([FromBody] VendaFiltroRankingViewModel viewModel)
        {
            DateTime _dtInicial = await FormatarDataConsulta(viewModel.dataInicial.ToString(), true);
            DateTime _dtFinal = await FormatarDataConsulta(viewModel.dataFinal.ToString(), false);

            viewModel.ListaVendas = _vendaService.ObterVendaRankingPorData(_dtInicial, _dtFinal,viewModel.TipoResultado,viewModel.Ordenacao).Result;
            
            return CustomResponse(viewModel);
        }

        [HttpGet("report/diferenca")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<VendaDiferencaCaixaReport>>> ObterVendaDiferencaPorData([FromQuery] string dtInicial = null, [FromQuery] string dtFinal = null)
        {
            DateTime _dtInicial = await FormatarDataConsulta(dtInicial, true);
            DateTime _dtFinal = await FormatarDataConsulta(dtFinal, false);

            var resultado = _vendaService.ObterVendaDiferencaCaixa(_dtInicial, _dtFinal).Result;

            return CustomResponse(resultado);
        }
        #endregion

        #region Venda Dapper

        #endregion

        #region Private
        private async Task<business.Models.PagedResult<VendaViewModel>> ObterListaVendaPaginado(DateTime dtIni, DateTime dtFinal, int page, int pageSize)
        {
            var lista = new List<VendaViewModel>();
            var retorno = await _vendaService.ObterPorPaginacao(dtIni, dtFinal, page, pageSize);

            retorno.List.ToList().ForEach(venda => {
                var vendaViewModel = _mapper.Map<VendaViewModel>(venda);
                vendaViewModel.CaixaNome = venda.Caixa != null && venda.Caixa.SQCAIXA.HasValue ? venda.Caixa.SQCAIXA.ToString(): string.Empty;
                vendaViewModel.PDVNome = venda.Caixa != null && venda.Caixa.PontoVenda != null && !string.IsNullOrEmpty(venda.Caixa.PontoVenda.DSPDV) ? venda.Caixa.PontoVenda.DSPDV : string.Empty;
                vendaViewModel.FuncionarioNome = venda.Caixa != null && venda.Caixa.Funcionario != null && !string.IsNullOrEmpty(venda.Caixa.Funcionario.NMFUNC) ? venda.Caixa.Funcionario.NMFUNC : string.Empty;

                lista.Add(vendaViewModel);
            });

            return new business.Models.PagedResult<VendaViewModel>()
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

            retorno.ToList().ForEach(vendaItem => {
                var vendaItemViewModel = _mapper.Map<VendaItemViewModel>(vendaItem);
                vendaItemViewModel.VendaNome = vendaItem.Venda != null? vendaItem.Venda.SQVENDA.ToString() : string.Empty;
                vendaItemViewModel.ProdutoNome = vendaItem.Produto != null && !string.IsNullOrEmpty(vendaItem.Produto.NMPRODUTO) ? vendaItem.Produto.NMPRODUTO : string.Empty;
                vendaItemViewModel.CodigoProduto = vendaItem.Produto != null && !string.IsNullOrEmpty(vendaItem.Produto.CDPRODUTO) ? vendaItem.Produto.CDPRODUTO : string.Empty;
                vendaItemViewModel.SituacaoProduto = vendaItem.Produto != null && vendaItem.Produto.STPRODUTO.HasValue ? 
                                                    (vendaItem.Produto.STPRODUTO == business.Enums.EAtivo.Ativo ? "Ativo": "Inativo") : string.Empty;

                lista.Add(vendaItemViewModel);
            });

            return lista;
        }

        private async Task<List<VendaMoedaViewModel>> ObterListaVendaMoedaPaginado(long idVenda)
        {
            var lista = new List<VendaMoedaViewModel>();
            var retorno = await _vendaService.ObterMoedasVenda(idVenda);

            retorno.ToList().ForEach(vendaMoeda => {
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

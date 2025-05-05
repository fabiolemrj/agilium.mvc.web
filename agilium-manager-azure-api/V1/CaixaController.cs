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
using System.Linq;
using agilium.api.manager.ViewModels.CaixaViewModel;
using agilium.api.manager.ViewModels.TurnoViewModel;
using agilium.api.business.Models;

namespace agilium.api.manager.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/caixa")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CaixaController : MainController
    {
        private readonly ICaixaService _caixaService;
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioService;
        public CaixaController(INotificador notificador, IUser appUser, IConfiguration configuration, ICaixaService caixaService,
            IMapper mapper,IUsuarioService usuarioService, IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, configuration, utilDapperRepository,logService)
        {
            _caixaService = caixaService;
            _mapper = mapper;
            _usuarioService = usuarioService;
        }

        #region Caixa
        [HttpGet("obter-por-data")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CaixaindexViewModel>>> ObterPorDataPaginacao([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string dtInicial = null, [FromQuery] string dtFinal = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();
            DateTime _dtInicial = Convert.ToDateTime(dtInicial);
            DateTime _dtFinal = Convert.ToDateTime(dtFinal);
            var lista = (await ObterListaCaixaPaginado(idEmpresa, _dtInicial, _dtFinal, page, ps));

            return CustomResponse(lista);
        }

        [HttpGet("obter-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CaixaindexViewModel>> ObterPorId(long id)
        {
            var caixa = await _caixaService.ObterCompletoPorId(id);
            if (caixa != null)
            {
                var objeto = _mapper.Map<CaixaindexViewModel>(caixa);
                objeto.Empresa = caixa.Empresa != null && !string.IsNullOrEmpty(caixa.Empresa.NMRZSOCIAL) ? caixa.Empresa.NMRZSOCIAL : string.Empty;
                objeto.Turno = caixa.Turno != null && caixa.Turno.NUTURNO > 0 ? caixa.Turno.NUTURNO.ToString() : string.Empty;
                objeto.PDV = caixa.PontoVenda != null && !string.IsNullOrEmpty(caixa.PontoVenda.DSPDV) ? caixa.PontoVenda.DSPDV : string.Empty;
                objeto.Funcionario = caixa.Funcionario != null && !string.IsNullOrEmpty(caixa.Funcionario.NMFUNC) ? caixa.Funcionario.NMFUNC : string.Empty;

                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Caixa nao localizado"));

        }
        #endregion

        #region Caixa Movimentacao
        [HttpGet("movimentacao")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CaixaindexViewModel>>> ObterPorDataPaginacao([FromQuery] long idCaixa, [FromQuery] int page = 1, [FromQuery] int ps = 15)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();
            var lista = (await ObterListaCaixaMovimentoPaginado(idCaixa, page, ps));

            return CustomResponse(lista);
        }
        #endregion

        #region Caixa Moeda
        [HttpGet("moedas")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CaixaMoedaViewModel>>> ObterMoedaPorDataPaginacao([FromQuery] long idCaixa, [FromQuery] int page = 1, [FromQuery] int ps = 15)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();
            var lista = (await ObterListaCaixaMoedaPaginado(idCaixa, page, ps));

            return CustomResponse(lista);
        }

        [HttpPut("moeda/correcao/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> CorrecaoMoeda(string id, [FromBody] CaixaMoedaViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                ObterNotificacoes("Moeda", "CorrecaoMoeda", "Web");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);
            
            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
            if (usuario == null)
            {
                NotificarErro("Erro ao tentar abrir Turno, usuario nao localizado"); 
                ObterNotificacoes("Moeda", "CorrecaoMoeda", "Web");
                return CustomResponse(viewModel);

            }
            viewModel.IDUSUARIOCORRECAO = usuario.Id;
            viewModel.DataCorrecao = DateTime.Now;
            var caixaMoeda = _mapper.Map<CaixaMoeda>(viewModel);
            
            await _caixaService.RealizarCorrecaoValor(caixaMoeda);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Moeda", "CorrecaoMoeda", "Web");
                return CustomResponse(msgErro);
            }

            await _caixaService.Salvar();
            LogInformacao($"Objeto excluido com sucesso {Deserializar(viewModel)}", "Moeda", "CorrecaoMoeda", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("moeda/obter-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CaixaMoedaViewModel>> ObterMoedaPorId(long id)
        {
            var caixaMoeda = await _caixaService.ObterCaixaMoedaCompletoPorId(id);
            if (caixaMoeda != null)
            {
                var objeto = _mapper.Map<CaixaMoedaViewModel>(caixaMoeda);
                objeto.CaixaNome = caixaMoeda.Caixa != null && caixaMoeda.Caixa.SQCAIXA > 0 ? caixaMoeda.Caixa.SQCAIXA.ToString() : string.Empty;
                objeto.MoedaNome = caixaMoeda.Moeda != null && !string.IsNullOrEmpty(caixaMoeda.Moeda.DSMOEDA) ? caixaMoeda.Moeda.DSMOEDA : string.Empty;
                objeto.UsuarioCorrecao = caixaMoeda.UsuarioCorrecao != null && !string.IsNullOrEmpty(caixaMoeda.UsuarioCorrecao.nome) ? caixaMoeda.UsuarioCorrecao.nome : string.Empty;
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Caixa Moeda nao localizado"));

        }
        #endregion



        #region Private
        private async Task<business.Models.PagedResult<CaixaindexViewModel>> ObterListaCaixaPaginado(long idEmpresa, DateTime dtIni, DateTime dtFinal, int page, int pageSize)
        {
            var lista = new List<CaixaindexViewModel>();
            var retorno = await _caixaService.ObterPorPaginacao(idEmpresa, dtIni, dtFinal, page, pageSize);

            retorno.List.ToList().ForEach(caixa => {
                var caixaViewModel = _mapper.Map<CaixaindexViewModel>(caixa);
                caixaViewModel.Empresa = caixa.Empresa != null && !string.IsNullOrEmpty(caixa.Empresa.NMRZSOCIAL) ? caixa.Empresa.NMRZSOCIAL : string.Empty;
                caixaViewModel.Turno = caixa.Turno != null && caixa.Turno.NUTURNO > 0 ? caixa.Turno.NUTURNO.ToString() : string.Empty;
                caixaViewModel.PDV = caixa.PontoVenda != null && !string.IsNullOrEmpty(caixa.PontoVenda.DSPDV) ? caixa.PontoVenda.DSPDV : string.Empty;
                caixaViewModel.Funcionario = caixa.Funcionario != null && !string.IsNullOrEmpty(caixa.Funcionario.NMFUNC)? caixa.Funcionario.NMFUNC : string.Empty;

                lista.Add(caixaViewModel);
            });            

            return new business.Models.PagedResult<CaixaindexViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<business.Models.PagedResult<CaixaMovimentoViewModel>> ObterListaCaixaMovimentoPaginado(long idCaixa, int page, int pageSize)
        {
            var lista = new List<CaixaMovimentoViewModel>();
            var retorno = await _caixaService.ObterMovimentacaoPorPaginacao(idCaixa, page, pageSize);

            retorno.List.ToList().ForEach(mov => {
                var caixaMovViewModel = _mapper.Map<CaixaMovimentoViewModel>(mov);
                caixaMovViewModel.Caixa = mov.Caixa != null && mov.Caixa.SQCAIXA > 0 ? mov.Caixa.SQCAIXA.ToString() : string.Empty;
                
                lista.Add(caixaMovViewModel);
            });

            return new business.Models.PagedResult<CaixaMovimentoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<business.Models.PagedResult<CaixaMoedaViewModel>> ObterListaCaixaMoedaPaginado(long idCaixa, int page, int pageSize)
        {
            var lista = new List<CaixaMoedaViewModel>();
            var retorno = await _caixaService.ObterMoedaPorPaginacao(idCaixa, page, pageSize);

            retorno.List.ToList().ForEach(moeda => {
                var caixaMoedaViewModel = _mapper.Map<CaixaMoedaViewModel>(moeda);
                caixaMoedaViewModel.CaixaNome = moeda.Caixa != null && moeda.Caixa.SQCAIXA > 0 ? moeda.Caixa.SQCAIXA.ToString() : string.Empty;
                caixaMoedaViewModel.MoedaNome = moeda.Moeda != null && !string.IsNullOrEmpty(moeda.Moeda.DSMOEDA) ? moeda.Moeda.DSMOEDA : string.Empty;
                caixaMoedaViewModel.UsuarioCorrecao = moeda.UsuarioCorrecao != null && !string.IsNullOrEmpty(moeda.UsuarioCorrecao.nome)? moeda.UsuarioCorrecao.nome : string.Empty;
                lista.Add(caixaMoedaViewModel);
            });

            return new business.Models.PagedResult<CaixaMoedaViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        #endregion
    }
}

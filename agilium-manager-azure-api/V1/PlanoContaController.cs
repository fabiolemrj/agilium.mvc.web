using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.infra.Repository.Dapper;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.PlanoContaViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.manager.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/plano-conta")]
    [ApiVersion("1.0")]
    public class PlanoContaController : MainController
    {
        private readonly IPlanoContaService _planoContaService;
        private readonly IPlanoContaDapperRepository _planoContaDapperRepository;
        private readonly IMapper _mapper;
        private const string _nomeEntidadeDepart = "Plano de Conta";
        public PlanoContaController(INotificador notificador, IUser appUser, IConfiguration configuration, IPlanoContaService planoContaService,
            IMapper mapper, IPlanoContaDapperRepository planoContaDapperRepository, 
            IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, configuration,utilDapperRepository,logService)
        {
            _planoContaService = planoContaService;
            _mapper = mapper;
            _planoContaDapperRepository = planoContaDapperRepository;
        }

        #region Plano Conta
        [HttpGet("obter-por-descricao")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PlanoContaViewModel>>> IndexPagination([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterListaPlanoContaPaginado(idEmpresa, q, page, ps));
            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] PlanoContaViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Id == 0) viewModel.Id = await GerarId();

            var planoConta = _mapper.Map<PlanoConta>(viewModel);
            
            await _planoContaService.Adicionar(planoConta);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("PlanoConta", "Adicionar", "Web", Deserializar(planoConta)));
                return CustomResponse(msgErro);
            }
            await _planoContaService.Salvar();
            LogInformacao($"sucesso: {Deserializar(planoConta)}", "PlanoConta", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] PlanoContaViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var produto = _mapper.Map<PlanoConta>(viewModel);
                        
            await _planoContaService.Atualizar(produto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("PlanoConta", "Atualizar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }

            await _planoContaService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "PlanoConta", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(long id)
        {
            var viewModel = await _planoContaService.ObterPorId(id);

            if (viewModel == null) return NotFound();

            await _planoContaService.Apagar(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("PlanoConta", "Excluir", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _planoContaService.Salvar();
            LogInformacao($"sucesso: id:{id}", "PlanoConta", "Excluir", null);
            return CustomResponse(viewModel);
        }


        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PlanoContaViewModel>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var objeto = await _planoContaService.ObterCompletoPorId(_id);
            if (objeto != null)
            {
                var viewModel = _mapper.Map<PlanoContaViewModel>(objeto);
                return CustomResponse(viewModel);
            }

            return CustomResponse(BadRequest("Plano de conta não localizado"));

        }
        [HttpGet("obter-todos-por-empresa/{idEmpresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PlanoContaViewModel>>> ObterTodos(long idEmpresa)
        {
            long _id = Convert.ToInt64(idEmpresa);
            var produtos = await _planoContaService.ObterTodas(_id);
            var viewModels = _mapper.Map<List<PlanoContaViewModel>>(produtos);
            return CustomResponse(viewModels);
        }

        #endregion

        #region Plano Conta Saldo


        [HttpPost("saldo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] PlanoContaSaldoViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Id == 0) viewModel.Id = await GerarId();

            var saldo = _mapper.Map<PlanoContaSaldo>(viewModel);

            await _planoContaService.Adicionar(saldo);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("PlanoConta", "AdicionarSaldo", "Web", Deserializar(saldo)));
                return CustomResponse(msgErro);
            }
            await _planoContaService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "PlanoConta", "AdicionarSaldo", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("saldo/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] PlanoContaSaldoViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var saldo = _mapper.Map<PlanoContaSaldo>(viewModel);

            await _planoContaService.Atualizar(saldo);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("PlanoConta", "AtualizarSaldo", "Web", Deserializar(saldo)));
                return CustomResponse(msgErro);
            }
      
            await _planoContaService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "PlanoConta", "AtualizarSaldo", null);
            return CustomResponse(viewModel);
        }

        [HttpDelete("saldo/{id}")]
        public async Task<ActionResult> ExcluirSaldo(long id)
        {
            var objeto = await _planoContaService.ObterSaldoPorId(id);

            if (objeto == null) return NotFound();

            await _planoContaService.Apagar(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("PlanoConta", "ExcluirSaldo", "Web", $"id:{id}"));
                return CustomResponse(msgErro);
            }
            await _planoContaService.Salvar();
            LogInformacao($"sucesso: id:{id}", "PlanoConta", "ExcluirSaldo", null);
            return CustomResponse(objeto);
        }

        [HttpGet("saldo/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PlanoContaViewModel>> ObterSaldoPorId(string id)
        {
            long _id = Convert.ToInt64(id);
            var objeto = await _planoContaService.ObterSaldoPorId(_id);
            if (objeto != null)
            {
                var viewModel = _mapper.Map<PlanoContaSaldoViewModel>(objeto);
                return CustomResponse(viewModel);
            }

            return CustomResponse(BadRequest("Saldo do Plano de conta não localizado"));

        }

        [HttpGet("obter-saldos-por-plano/{idPlano}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PlanoContaSaldoViewModel>>> ObterPlanoSaldoPorIdPlano(long idPlano)
        {
            long _id = Convert.ToInt64(idPlano);
            var saldos = await _planoContaService.ObterSaldoPorPlano(_id);
            var viewModels = _mapper.Map<List<PlanoContaSaldoViewModel>>(saldos);
            return CustomResponse(viewModels);
        }

        [HttpGet("saldo/atualizar/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AtualizarSaldoPorId(long id)
        {
            var msgResultado = "";
            try
            {
                await _planoContaDapperRepository.AtualizarSaldoContaESubConta(id);
                msgResultado = "Saldo da conta atualizado com sucesso!";
            }
            catch
            {
                NotificarErro("Erro ao tentar atualizar saldo de conta");

            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("PlanoConta", "AtualizarSaldoPorId", "Web", $"id:{id}"));
                return CustomResponse(msgErro);
            }

            LogInformacao($"sucesso: id:{id}", "PlanoConta", "AtualizarSaldoPorId", null);
            return CustomResponse(msgResultado);

        }
        #endregion

        #region Plano Conta Lançamento
        [HttpPost("lancamentos")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PlanoContaViewModel>>> ObterLacamentos([FromBody] PlanoContaLancamentoListaViewModel viewModel)
        {
            var lancamentos = await _planoContaDapperRepository.ObterLancamentosPorPlanoEData(viewModel.IdPlano,viewModel.DataInicial,viewModel.DataFinal);
            var viewModels = _mapper.Map<List<PlanoContaLancamentoViewModel>>(lancamentos);
            viewModels.ToList().ForEach( plano => {
                plano.TipoConta = _planoContaDapperRepository.ObterDescricaoPlano(viewModel.IdPlano).Result;
            });
            return CustomResponse(viewModels);
        }

        [HttpGet("lancamentos/obter-por-data")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PlanoContaViewModel>>> LancamentosPagination([FromQuery] long idPlano, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string dtInicial = null, [FromQuery] string dtFinal = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();
            DateTime _dtInicial = Convert.ToDateTime(dtInicial);
            DateTime _dtFinal = Convert.ToDateTime(dtFinal);
            var lista = (await ObterListaPlanoContaPaginado(idPlano, _dtInicial,_dtFinal, page, ps));

            return CustomResponse(lista);
        }

        [HttpPost("lancamento")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] PlanoContaLancamentoViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Id == 0) viewModel.Id = await GerarId();

            var lancamento = _mapper.Map<PlanoContaLancamento>(viewModel);

            await _planoContaService.Adicionar(lancamento);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("PlanoConta", "AdicionarLancamento", "Web", Deserializar(lancamento)));
                return CustomResponse(msgErro);
            }
            await _planoContaService.Salvar();
            LogInformacao($"sucesso: {Deserializar(lancamento)}", "PlanoConta", "AdicionarLancamento", null);
            return CustomResponse(viewModel);
        }

        #endregion

        #region Private
        private async Task<business.Models.PagedResult<PlanoContaViewModel>> ObterListaPlanoContaPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _planoContaService.ObterPorPaginacao(idEmpresa, filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<PlanoContaViewModel>>(retorno.List);

            lista.ToList().ForEach(saldo => {
                saldo.Saldo = _planoContaService.ObterSaldoPorIdPlano(saldo.Id).Result;
            });

            return new business.Models.PagedResult<PlanoContaViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<business.Models.PagedResult<PlanoContaLancamentoViewModel>> ObterListaPlanoContaPaginado(long idPlano, DateTime dtIni, DateTime dtFinal, int page, int pageSize)
        {   
            var retorno = await _planoContaService.ObterLancamentoPorPaginacao(idPlano, dtIni, dtFinal, page, pageSize);

            var lista = _mapper.Map<IEnumerable<PlanoContaLancamentoViewModel>>(retorno.List);

            return new business.Models.PagedResult<PlanoContaLancamentoViewModel>()
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

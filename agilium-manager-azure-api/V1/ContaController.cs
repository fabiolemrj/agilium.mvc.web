using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.ContaViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.manager.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/conta")]
    [ApiVersion("1.0")]
    public class ContaController : MainController
    {
        private readonly IContaService _contaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IPContaPagarDapperRepository _contaPagarDapperRepository;
        private readonly IPContaReceberDapperRepository _contaReceberDapperRepository;


        private readonly IMapper _mapper;
        public ContaController(INotificador notificador, IUser appUser, IConfiguration configuration, IContaService contaService,
            IMapper mapper, IUsuarioService usuarioService,IPContaPagarDapperRepository pContaPagarDapperRepository, 
            IPContaReceberDapperRepository contaReceberDapperRepository, IUtilDapperRepository utilDapperRepository, 
            ILogService logService) : base(notificador, appUser, configuration,utilDapperRepository,logService)
        {
            _contaService = contaService;
            _mapper = mapper;
            _usuarioService = usuarioService;
            _contaPagarDapperRepository = pContaPagarDapperRepository;
            _contaReceberDapperRepository = contaReceberDapperRepository;
        }

        #region Conta Pagar
        [HttpGet("pagar/obter-por-descricao")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ContaPagarViewModelIndex>>> IndexPaginationContaPagar([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterListaContaPaginado(idEmpresa, q, page, ps));
            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }

        [HttpPost("pagar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> AdicionarContaPagar([FromBody] ContaPagarViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            
            if (viewModel.Id == 0) viewModel.Id = await GerarId();
            
            if(viewModel.IDCONTAPAI == null)
                viewModel.IDCONTAPAI = viewModel.Id;

            if (!viewModel.IDUSUARIO.HasValue || viewModel.IDUSUARIO.Value == 0)
            {
                var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
                if (usuario != null)
                    viewModel.IDUSUARIO = usuario.Id;
            }

            var contaPagar = _mapper.Map<ContaPagar>(viewModel);
            
            await _contaService.Adicionar(contaPagar);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Conta", "AdicionarContaPagar", "Web"));
                return CustomResponse(msgErro);
            }
            await _contaService.Salvar();
            LogInformacao($"Objeto Criado com sucesso {contaPagar}", "Conta", "AdicionarContaPagar", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("pagar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> AtualizarContaPagar(string id, [FromBody] ContaPagarViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var produto = _mapper.Map<ContaPagar>(viewModel);
            
            await _contaService.Atualizar(produto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Conta", "AtualizarContaPagar", "Web"));
                return CustomResponse(msgErro);
            }

            await _contaService.Salvar();
            LogInformacao($"Objeto atualizar com sucesso {Deserializar(viewModel)}", "Conta", "AtualizarContaPagar", null);
            return CustomResponse(viewModel);
        }


        [HttpGet("pagar/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ContaPagarViewModel>> ObterContaPagarPorId(string id)
        {
            long _id = Convert.ToInt64(id);
            var produto = await _contaService.ObterCompletoPorId(_id);
            if (produto != null)
            {
                var objeto = _mapper.Map<ContaPagarViewModel>(produto);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Conta a pagar nao localizada"));

        }

        [HttpDelete("pagar/{id}")]
        public async Task<ActionResult> ExcluirContaPagar(long id)
        {
            var viewModel = await _contaService.ObterPorId(id);

            if (viewModel == null) return NotFound();

            await _contaService.Apagar(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Conta", "ExcluirContaPagar", "Web"));
                return CustomResponse(msgErro);
            }
            await _contaService.Salvar();
            LogInformacao($"Objeto atualizar com sucesso id:{id}", "Conta", "ExcluirContaPagar", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("pagar/consolidar/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ConsolidarContaPagarPorId(long id)
        {
            var msgResultado = "";
            try
            {
                await _contaPagarDapperRepository.ConsolidarConta(id);
                msgResultado = "Conta a pagar consolidada com sucesso!";
            }
            catch
            {
                NotificarErro("Erro ao tentar Conta a pagar consolidar conta a pagar");

            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Conta", "ConsolidarContaPagarPorId", "Web"));
                return CustomResponse(msgErro);
            }

            LogInformacao($"Objeto consolidado com sucesso id:{id}", "Conta", "ConsolidarContaPagarPorId", null);
            return CustomResponse(msgResultado);
        }

        [HttpGet("pagar/desconsolidar/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DesConsolidarContaPagarPorId(long id)
        {
            var msgResultado = "";
            try
            {
                await _contaPagarDapperRepository.DesconsolidarConta(id);
                msgResultado = "Conta a pagar desconsolidada com sucesso!";
            }
            catch
            {
                NotificarErro("Erro ao tentar Conta a pagar desconsolidar conta a pagar");

            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Conta", "DesConsolidarContaPagarPorId", "Web"));
                return CustomResponse(msgErro);
            }

            LogInformacao($"Objeto desconsolidado com sucesso id:{id}", "Conta", "DesConsolidarContaPagarPorId", null);
            return CustomResponse(msgResultado);
        }
        #endregion

        #region Conta Receber
        [HttpGet("receber/obter-por-descricao")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ContaReceberViewModelIndex>>> IndexPaginationContaReceber([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterListaContaReceberPaginado(idEmpresa, q, page, ps));
            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }

        [HttpPost("receber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> AdicionarContaReceber([FromBody] ContaReceberViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Id == 0) viewModel.Id = await GerarId();

            if (viewModel.IDCONTAPAI == null)
                viewModel.IDCONTAPAI = viewModel.Id;

            if (!viewModel.IDUSUARIO.HasValue || viewModel.IDUSUARIO.Value == 0)
            {
                var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
                if (usuario != null)
                    viewModel.IDUSUARIO = usuario.Id;
            }

            var objeto = _mapper.Map<ContaReceber>(viewModel);

            await _contaService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Conta", "AdicionarContaReceber", "Web"));
                return CustomResponse(msgErro);
            }
            await _contaService.Salvar();
            LogInformacao($"Objeto adicionado com sucesso id:{Deserializar(viewModel)}", "Conta", "AdicionarContaReceber", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("receber/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> AtualizarContaReceber(string id, [FromBody] ContaReceberViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<ContaReceber>(viewModel);

            await _contaService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Conta", "AtualizarContaReceber", "Web"));
                return CustomResponse(msgErro);
            }

            await _contaService.Salvar();
            LogInformacao($"Objeto atualizado com sucesso {Deserializar(viewModel)}", "Conta", "AtualizarContaReceber", null);
            return CustomResponse(viewModel);
        }

        [HttpDelete("receber/{id}")]
        public async Task<ActionResult> ExcluirContaReceber(long id)
        {
            var viewModel = await _contaService.ObterContaReceberPorId(id);

            if (viewModel == null) return NotFound();

            await _contaService.ApagarContaReceber(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Conta", "ExcluirContaReceber", "Web"));
                return CustomResponse(msgErro);
            }
            await _contaService.Salvar();
            LogInformacao($"Objeto adicionado com sucesso id:{id}", "Conta", "ExcluirContaReceber", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("receber/consolidar/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ConsolidarContaReceberPorId(long id)
        {
            var msgResultado = "";
            try
            {
                await _contaReceberDapperRepository.ConsolidarConta(id);
                msgResultado = "Conta a receber consolidada com sucesso!";
            }
            catch
            {
                NotificarErro("Erro ao tentar consolidar conta a receber");

            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Conta", "ConsolidarContaReceberPorId", "Web"));
                return CustomResponse(msgErro);
            }

            LogInformacao($"Objeto consolidado com sucesso id:{id}", "Conta", "ConsolidarContaReceberPorId", null);
            return CustomResponse(msgResultado);
        }

        [HttpGet("receber/desconsolidar/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ConsolidarDesContaReceberPorId(long id)
        {
            var msgResultado = "";
            try
            {
                await _contaReceberDapperRepository.DesconsolidarConta(id);
                msgResultado = "Conta a receber desconsolidada com sucesso!";
            }
            catch
            {
                NotificarErro("Erro ao tentar desconsolidar conta a receber");

            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Conta", "ConsolidarContaReceberPorId", "Web"));
                return CustomResponse(msgErro);
            }

            LogInformacao($"Objeto desconsolidado com sucesso id:{id}", "Conta", "ConsolidarDesContaReceberPorId", null);
            return CustomResponse(msgResultado);
        }

        [HttpGet("receber/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ContaReceberViewModel>> ObterContaReceberPorId(string id)
        {
            long _id = Convert.ToInt64(id);
            var produto = await _contaService.ObterContaReceberCompletoPorId(_id);
            if (produto != null)
            {
                var objeto = _mapper.Map<ContaReceberViewModel>(produto);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Conta a receber nao localizada"));

        }
        #endregion

        #region Private
        private async Task<business.Models.PagedResult<ContaPagarViewModelIndex>> ObterListaContaPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _contaService.ObterPorPaginacao(idEmpresa, filtro, page, pageSize);

            var listaContaPagarViewModel = new List<ContaPagarViewModelIndex>();

            retorno.List.ToList().ForEach( contaPagar => {
                
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
                viewModel.TipoConta = contaPagar.TPCONTA.Value == 1? business.Enums.ETipoConta.Eventual : business.Enums.ETipoConta.Fixa;
                viewModel.ValorAcrescimo = contaPagar.VLACRESC;
                viewModel.ValorConta = contaPagar.VLCONTA;
                viewModel.ValorDesconto = contaPagar.VLDESC;
                viewModel.DataVencimento = contaPagar.DTVENC;


                if (contaPagar.Fornecedor != null && !string.IsNullOrEmpty(contaPagar.Fornecedor.NMRZSOCIAL))
                    viewModel.Fornecedor = contaPagar.Fornecedor.NMRZSOCIAL;
                if (contaPagar.CategFinanc != null && !string.IsNullOrEmpty(contaPagar.CategFinanc.NMCATEG))
                    viewModel.CategoriaFinanceira= contaPagar.CategFinanc.NMCATEG;
                if (contaPagar.PlanoConta != null && !string.IsNullOrEmpty(contaPagar.PlanoConta.DSCONTA))
                    viewModel.Conta = contaPagar.PlanoConta.DSCONTA;
                listaContaPagarViewModel.Add(viewModel);
            });

            return new business.Models.PagedResult<ContaPagarViewModelIndex>()
            {
                List = listaContaPagarViewModel,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<business.Models.PagedResult<ContaReceberViewModelIndex>> ObterListaContaReceberPaginado(long idEmpresa, string filtro, int page, int pageSize)
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
                viewModel.TipoConta = contaRec.TPCONTA.Value == 1 ? business.Enums.ETipoConta.Eventual : business.Enums.ETipoConta.Fixa;
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

            return new business.Models.PagedResult<ContaReceberViewModelIndex>()
            {
                List = listaContaReceberViewModel,
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

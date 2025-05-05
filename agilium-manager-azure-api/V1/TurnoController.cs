using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.infra.Repository.Dapper;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.TurnoViewModel;
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
    [Route("api/v{version:apiVersion}/turno")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TurnoController : MainController
    {
        private readonly ITurnoService _turnoService;
        private readonly IEmpresaService _empresaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IPTurnoDapperRepository _TurnoDapperRepository;
        private readonly IMapper _mapper;
        private const string _nomeEntidade = "Turno";
        
        public TurnoController(INotificador notificador, IUser appUser, IConfiguration configuration, 
                             ITurnoService turnoService, IMapper mapper, IUsuarioService usuarioService,
                             IEmpresaService empresaService, IPTurnoDapperRepository turnoDapperRepository, 
                             IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, 
                                 configuration,utilDapperRepository, logService)
        {
            _turnoService = turnoService;
            _mapper = mapper;
            _empresaService = empresaService;
            _usuarioService = usuarioService;
            _TurnoDapperRepository = turnoDapperRepository;
        }

        #region Turno

        [HttpGet("obter-por-data")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<TurnoIndexViewModel>>> ObterPorDataPaginacao([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string dtInicial = null, [FromQuery] string dtFinal = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();
            DateTime _dtInicial = Convert.ToDateTime(dtInicial);
            DateTime _dtFinal = Convert.ToDateTime(dtFinal);
            var lista = (await ObterListaPlanoContaPaginado(idEmpresa, _dtInicial, _dtFinal, page, ps));

            return CustomResponse(lista);
        }

        [HttpGet("abrir/{idempresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AbrirTurno(long idempresa)
        {
            var msgResultado = "";
            try
            {
                var turnoAberto = _TurnoDapperRepository.TurnoAberto(idempresa).Result;
                
                if (turnoAberto)
                {
                    NotificarErro("Já existe um turno aberto para a empresa atual.");
                    var msgErro = string.Join("\n\r", ObterNotificacoes());
                    return CustomResponse(msgErro);
                }

                var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
                if (usuario == null)
                {              
                    NotificarErro("Erro ao tentar abrir Turno, usuario nao localizado");
                }
                
                if (!OperacaoValida())
                {
                    var msgErro = string.Join("\n\r", ObterNotificacoes("Turno", "AbrirTurno", "Web"));
                    return CustomResponse(msgErro);
                }

                await _TurnoDapperRepository.AbrirTurno(idempresa,usuario.Id);
                msgResultado = "Turno Aberto com sucesso!";
            }
            catch
            {
                NotificarErro("Erro ao tentar abrir turno");

            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                return CustomResponse(msgErro);
            }

            LogInformacao($"TurnoAberto", "Turno", "AbrirTurno", null);
            return CustomResponse(msgResultado);
        }

        //[HttpGet("fechar/{idempresa}")]
        ////[ClaimsAuthorize("USUARIO", "CONSULTA")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult> FecharTurno(long idempresa)
        //{
        //    var msgResultado = "";
        //    try
        //    {
        //        var turnoAberto = _TurnoDapperRepository.TurnoAberto(idempresa).Result;

        //        if (!turnoAberto)
        //        {
        //            NotificarErro("Não existe um turno aberto para a empresa atual");
        //            var msgErro = string.Join("\n\r", ObterNotificacoes());
        //            return CustomResponse(msgErro);
        //        }

        //        var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
        //        if (usuario == null)
        //        {
        //            NotificarErro("Erro ao tentar fechar Turno, usuario nao localizado");
        //        }

        //        if (!OperacaoValida())
        //        {
        //            var msgErro = string.Join("\n\r", ObterNotificacoes());
        //            return CustomResponse(msgErro);
        //        }

        //       // await _TurnoDapperRepository.FecharTurno(idempresa,usuario.Id);
        //        msgResultado = "Turno fechado com sucesso!";
        //    }
        //    catch
        //    {
        //        NotificarErro("Erro ao tentar abrir turno");

        //    }

        //    if (!OperacaoValida())
        //    {
        //        var msgErro = string.Join("\n\r", ObterNotificacoes());
        //        return CustomResponse(msgErro);
        //    }


        //    return CustomResponse(msgResultado);
        //}

        [HttpPost("fechar")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> FecharTurno([FromBody]TurnoViewModel viewModel)
        {
            var msgResultado = "";
            try
            {
                var turnoAberto = _TurnoDapperRepository.TurnoAberto(viewModel.IDEMPRESA.Value).Result;

                if (!turnoAberto)
                {
                    NotificarErro("Não existe um turno aberto para a empresa atual");
                    var msgErro = string.Join("\n\r", ObterNotificacoes("Turno", "FecharTurno", "Web"));
                    return CustomResponse(msgErro);
                }

                var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
                if (usuario == null)
                {
                    NotificarErro("Erro ao tentar fechar Turno, usuario nao localizado");
                }

                if (!OperacaoValida())
                {
                    var msgErro = string.Join("\n\r", ObterNotificacoes());
                    return CustomResponse(msgErro);
                }

                await _TurnoDapperRepository.FecharTurno(viewModel.IDEMPRESA.Value, usuario.Id,viewModel.Obs);
                msgResultado = "Turno fechado com sucesso!";
            }
            catch
            {
                NotificarErro("Erro ao tentar fechar turno");

            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                return CustomResponse(msgErro);
            }
            LogInformacao($"Turno Fechado", "Turno", "FecharTurno", null);
            return CustomResponse(msgResultado);
        }

        [HttpGet("obter-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TurnoViewModel>> ObterPorId(string id)
        {
            long _id = Convert.ToInt64(id);
            var turno = await _turnoService.ObterCompletoPorId(_id);
            if (turno != null)
            {
                var objeto = _mapper.Map<TurnoIndexViewModel>(turno);
                objeto.Empresa = turno.Empresa != null && !string.IsNullOrEmpty(turno.Empresa.NMRZSOCIAL) ? turno.Empresa.NMRZSOCIAL : string.Empty;
                objeto.UsuarioInicial = turno.UsuarioInicial != null && !string.IsNullOrEmpty(turno.UsuarioInicial.nome) ? turno.UsuarioInicial.nome : string.Empty;
                objeto.UsuarioFinal = turno.UsuarioFinal != null && !string.IsNullOrEmpty(turno.UsuarioFinal.nome) ? turno.UsuarioFinal.nome : string.Empty;

                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Turno aberto nao localizado"));
        }

        [HttpGet("obter-todos/{idEmpresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TurnoViewModel>> ObterTodos(string idEmpresa)
        {
            long _id = Convert.ToInt64(idEmpresa);
            var turno = _mapper.Map<List<TurnoIndexViewModel>>(_turnoService.ObterTodos(_id).Result);
            
            return CustomResponse(turno);
        }

        [HttpGet("obter-turno-aberto/{idEmpresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TurnoViewModel>> ObterTurnoAberto(long idEmpresa)
        {
            var turno = await _turnoService.ObterTurnoAbertoCompletoPorId(idEmpresa);
            if (turno != null)
            {
                var objeto = _mapper.Map<TurnoIndexViewModel>(turno);
                objeto.Empresa = turno.Empresa != null && !string.IsNullOrEmpty(turno.Empresa.NMRZSOCIAL) ? turno.Empresa.NMRZSOCIAL : string.Empty;
                objeto.UsuarioInicial = turno.UsuarioInicial != null && !string.IsNullOrEmpty(turno.UsuarioInicial.nome) ? turno.UsuarioInicial.nome : string.Empty;
                objeto.UsuarioFinal = turno.UsuarioFinal != null && !string.IsNullOrEmpty(turno.UsuarioFinal.nome) ? turno.UsuarioFinal.nome : string.Empty;

                return CustomResponse(objeto);
            }
            NotificarErro("Turno aberto nao localizado");
            return CustomResponse();

        }
        #endregion

        #region Turno Preco
        [HttpGet("precos/obter-por-idproduto/{idProduto}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<TurnoPrecoViewModel>>> ObterPreco(long idProduto)
        {
            var lista = _turnoService.ObterTurnoPrecoPorProduto(idProduto).Result;

            var model = _mapper.Map<List<TurnoPrecoViewModel>>(lista);

            return CustomResponse(model);
        }

        [HttpPost("preco")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "INCLUIR")]
        public async Task<IActionResult> AdicionarPreco([FromBody] TurnoPrecoViewModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (model.Id == 0) model.Id = await GerarId();
            if (model.DataHora == null) model.DataHora = DateTime.Now;
            if (string.IsNullOrEmpty(model.Usuario)) model.Usuario = AppUser.GetUserEmail();

            var objeto = _mapper.Map<TurnoPreco>(model);

            await _turnoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Turno", "AdicionarPreco", "Web", $"{Deserializar(objeto)}"));
                return CustomResponse(msgErro);
            }

            await _turnoService.Salvar();
            LogInformacao($"sucesso{Deserializar(objeto)}", "Turno", "AdicionarPreco", null);
            return CustomResponse(model);
        }

        [HttpDelete("preco/{id}")]
        public async Task<ActionResult> ExcluirPreco(long id)
        {
            var viewModel = await _turnoService.ObteClientePrecoPorId(id);

            if (viewModel == null) return NotFound();

            await _turnoService.Remover(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes($"Turno", "ExcluirPreco", "Web",$"id:{id}"));
                return CustomResponse(msgErro);
            }
            await _turnoService.Salvar();
            LogInformacao($"sucesso id:{id}", "Turno", "ExcluirPreco", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("preco/obter-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TurnoPrecoViewModel>> ObterPreco(string id)
        {
            long _id = Convert.ToInt64(id);
            var produto = await _turnoService.ObteClientePrecoPorId(_id);
            if (produto != null)
            {
                var objeto = _mapper.Map<TurnoPrecoViewModel>(produto);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Preço do produto por turno nao localizado"));

        }

        #endregion

        #region private
        private async Task<business.Models.PagedResult<TurnoIndexViewModel>> ObterListaPlanoContaPaginado(long idEmpresa, DateTime dtIni, DateTime dtFinal, int page, int pageSize)
        {
            var retorno = await _turnoService.ObterPorPaginacao(idEmpresa, dtIni, dtFinal, page, pageSize);

            var lista = _mapper.Map<IEnumerable<TurnoIndexViewModel>>(retorno.List);
            lista.ToList().ForEach( turno => {
                if(turno.IDEMPRESA.HasValue)
                {
                    var empresa = _empresaService.ObterPorId(turno.IDEMPRESA.Value).Result;
                    turno.Empresa = (empresa != null && !string.IsNullOrEmpty(empresa.NMRZSOCIAL)) ? empresa.NMRZSOCIAL : "Não localizado";
                }

                if (turno.IDUSUARIOINI.HasValue)
                {
                    var usuarioINicial = _usuarioService.ObterPorUsuarioPorId(turno.IDUSUARIOINI.Value).Result;
                    turno.UsuarioInicial = (usuarioINicial != null && !string.IsNullOrEmpty(usuarioINicial.nome)) ? usuarioINicial.nome : "";
                }
                if (turno.IDUSUARIOFIM.HasValue)
                {
                    var usuarioFinal = _usuarioService.ObterPorUsuarioPorId(turno.IDUSUARIOFIM.Value).Result;
                    turno.UsuarioFinal = (usuarioFinal != null && !string.IsNullOrEmpty(usuarioFinal.nome)) ? usuarioFinal.nome : "";
                }
            });

            return new business.Models.PagedResult<TurnoIndexViewModel>()
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

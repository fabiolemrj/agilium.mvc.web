using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.pdv.Controllers;
using agilium.api.pdv.ViewModels.TurnoViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace agilium.api.pdv.V1
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
                             IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, configuration, utilDapperRepository, logService)
        {
            _turnoService = turnoService;
            _mapper = mapper;
            _empresaService = empresaService;
            _usuarioService = usuarioService;
            _TurnoDapperRepository = turnoDapperRepository;
        }

        #region Turno

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
                    var erro= CustomResponse(msgErro);
                    return erro;
                }

                var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
                if (usuario == null)
                {
                    NotificarErro("Erro ao tentar abrir Turno, usuario nao localizado");
                }

                
                if (!OperacaoValida())
                {
                    var msgErro = string.Join("\n\r", ObterNotificacoes("Turno", "AbrirTurnoPDV", "WebPdv"));
                    return CustomResponse(msgErro);
                }

                await _TurnoDapperRepository.AbrirTurno(idempresa, usuario.Id);
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

            LogInformacao($"TurnoAberto", "Turno", "AbrirTurnoPDV", null);
            return CustomResponse();
        }

        [HttpPost("fechar")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> FecharTurno([FromBody] TurnoViewModel viewModel)
        {
            var msgResultado = "";
            try
            {
                long id = !string.IsNullOrEmpty(viewModel.IDEMPRESA) ? Convert.ToInt64(viewModel.IDEMPRESA) : 0;
                var turnoAberto = _TurnoDapperRepository.TurnoAberto(id).Result;

                if (!turnoAberto)
                {
                    NotificarErro("Não existe um turno aberto para a empresa atual");
                    var msgErro = string.Join("\n\r", ObterNotificacoes("Turno", "FecharTurnoPDV", "WebPDV"));
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

                await _TurnoDapperRepository.FecharTurno(id, usuario.Id, viewModel.Obs);
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
            LogInformacao($"Turno Fechado", "Turno", "FecharTurnoPDV", null);
            return CustomResponse();
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
    }
}

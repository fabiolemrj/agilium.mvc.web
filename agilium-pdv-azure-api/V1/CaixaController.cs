using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.pdv.Controllers;
using agilium.api.pdv.ViewModels.CaixaViewModel;
using agilium.api.pdv.ViewModels.ConfigViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace agilium.api.pdv.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/caixa")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CaixaController : MainController
    {
        private readonly ICaixaService _caixaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        private const string _nomeEntidade = "Caixa";
        public CaixaController(INotificador notificador, IUser appUser, IConfiguration configuration, IUtilDapperRepository utilDapperRepository, ILogService logService,
            ICaixaService caixaService, IMapper mapper, IUsuarioService usuarioService) : base(notificador, appUser, configuration, utilDapperRepository, logService)
        {
            _usuarioService = usuarioService;
            _caixaService = caixaService;
            _mapper = mapper;
        }

        #region Caixa
      
        [HttpPost("abrir")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AbrirCaixa([FromBody]AbrirCaixaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        NotificarErro(error.ErrorMessage);
                    }
                }
                return CustomResponse(string.Join("\n\r", ObterNotificacoes()));
            }

            var msgResultado = new AbrirCaixaRetornoViewModel();
            try
            {
                var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
                if (usuario == null)
                {
                    NotificarErro("Erro ao tentar abrir Caixa, usuario nao localizado");
                }

                var caixaAberto = _caixaService.AbrirCaixa(Int64.Parse(model.idempresa), usuario.Id, Int64.Parse(model.idpdv)).Result;
                
                if (!OperacaoValida())
                {
                    var msgErro = string.Join("\n\r", ObterNotificacoes("Caixa", "AbrirCaixaPDV", "WebCaixa"));
                    return CustomResponse(msgErro);
                }

                msgResultado.NumeroCaixa = caixaAberto.ToString();
            }
            catch
            {
                NotificarErro("Erro ao tentar abrir Caixa");
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                return CustomResponse(msgErro);
            }

            LogInformacao($"CaixaAberto", "Caixa", "AbrirCaixaPDV", null);
            return CustomResponse(msgResultado);
        }

        [HttpPost("movimento-caixa")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RealizarMovimentoCaixa([FromBody] MovimentoCaixaViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        NotificarErro(error.ErrorMessage);
                    }
                }
                return CustomResponse(string.Join("\n\r", ObterNotificacoes()));
            }

            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
            if (usuario == null)
            {
                NotificarErro("Erro ao tentar sangria no caixa, usuario nao localizado");
            }

            var identificaçãoTipoMovimento = "Sangria";

            if(model.TipoMovimento == business.Enums.ETipoMovCaixa.Sangria)
                await _caixaService.RealizarSangria(Convert.ToInt64(model.idCaixa), usuario.Id, model.ValorSangria, model.Obs);
            else
            {
                await _caixaService.RealizarSuprimento(Convert.ToInt64(model.idCaixa), usuario.Id, model.ValorSangria, model.Obs);
                identificaçãoTipoMovimento = "Suprimento";
            }
         

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes(identificaçãoTipoMovimento, "RealizarMovimentoCaixa", "WebCaixa"));
                return CustomResponse(msgErro);
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                return CustomResponse(msgErro);
            }

            LogInformacao("Caixa", "RealizarMovimentoCaixa", identificaçãoTipoMovimento, null);
            return CustomResponse();
        }

        [HttpPost("fechar")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> FecharCaixa([FromBody]FecharCaixa model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        NotificarErro(error.ErrorMessage);
                    }
                }
                return CustomResponse(string.Join("\n\r", ObterNotificacoes()));
            }
          
            
            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
            if (usuario == null)
            {
                NotificarErro("Erro ao tentar fechar Caixa, usuario nao localizado");
            }

            await _caixaService.FecharCaixa(Convert.ToInt64(model.idCaixa), usuario.Id, model.ValorSangria, model.Obs);
                
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Caixa", "FecharCaixaPDV", "WebCaixa"));
                return CustomResponse(msgErro);
            }
          
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                return CustomResponse(msgErro);
            }

            LogInformacao($"CaixaFechar", "Caixa", "FecharCaixaPDV", null);
            return CustomResponse();
        }

        [HttpGet("obter-caixa-fechamento/{idUsuario}/{idEmpresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConfigPreVendaViewModel>> ObterCaixaParaFechamento(string idUsuario, string idEmpresa)
        {
            long _idempresa = Convert.ToInt64(idEmpresa);
            var usuario = await _usuarioService.ObterPorUsuarioAspNetPorId(idUsuario);
            if(usuario == null)
            {
                NotificarErro("Usuario não localizado");
                var msgErro = string.Join("\n\r", ObterNotificacoes("CaixaPDV", "ObterCaixaParaFechamento", "WebCaixa"));
                return CustomResponse(msgErro);
            }

            var caixa = await _caixaService.ObterCaixaParaFechamento(_idempresa, usuario.Id);
            var result = _mapper.Map<FecharCaixaInicializa>(caixa);

            if(result == null)
            {
                NotificarErro("Não foi localizado caixa aberto para o ususario");
            }
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("CaixaPDV", "ObterCaixaParaFechamento", "WebCaixa"));
                return CustomResponse(msgErro);
            }

            return CustomResponse(result);
        }

        [HttpGet("obter-caixa-aberto/{idUsuario}/{idEmpresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CaixaindexViewModel>> ObterCaixaAberto(string idUsuario, string idEmpresa)
        {
            if(string.IsNullOrEmpty(idEmpresa))
            {
                NotificarErro("Empresa não localizada");
                var msgErro = string.Join("\n\r", ObterNotificacoes("CaixaPDV", "ObterCaixaParaAberto", "WebCaixa"));
                return CustomResponse(msgErro);
            }
            var usuario = await _usuarioService.ObterPorUsuarioAspNetPorId(idUsuario);
            if (usuario == null)
            {
                NotificarErro("Usuario não localizado");
                var msgErro = string.Join("\n\r", ObterNotificacoes("CaixaPDV", "ObterCaixaParaAberto", "WebCaixa"));
                return CustomResponse(msgErro);
            }

            long _idempresa = Convert.ToInt64(idEmpresa);
            
            var caixa = await _caixaService.ObterCaixaAbertoPorEmpresa(_idempresa, usuario.Id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("CaixaPDV", "ObterCaixaParaFechamento", "WebCaixa"));
                return CustomResponse(msgErro);
            }
            return CustomResponse(_mapper.Map<CaixaindexViewModel>(caixa));
        }
        #endregion
    }
}

using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.PedidoViewModel;
using agilium.api.business.Services;
using agilium.api.pdv.Controllers;
using agilium.api.pdv.ViewModels.PedidoViewModel;
using agilium.api.pdv.ViewModels.VendaViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.api.pdv.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/venda")]
    [ApiVersion("1.0")]
    [ApiController]
    public class VendaController : MainController
    {
        private readonly IVendaService _vendaService;
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioService;

        public VendaController(INotificador notificador, IUser appUser, IConfiguration configuration, IUtilDapperRepository utilDapperRepository, 
            ILogService logService, IVendaService vendaService, IMapper mapper, IUsuarioService usuarioService) : base(notificador, appUser, configuration, utilDapperRepository, logService)
        {
            _mapper = mapper;
            _vendaService = vendaService;
            _usuarioService = usuarioService;
        }

        [HttpGet("nova-venda/{idempresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> NovaVenda(string idempresa)
        {
            var _idempresa = Convert.ToInt64(idempresa);

            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
            if (usuario == null)
            {
                NotificarErro("Erro ao criar nova venda, usuario nao localizado");
            }

            var novaVenda = await _vendaService.ObterDadosParaNovaVenda(usuario.Id,_idempresa);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Venda", "Nova", "WebPDV", $"usuario: {usuario.Id} empresa:{idempresa}"));
                return CustomResponse(msgErro);
            }

            return CustomResponse(novaVenda);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] VendaIncluirViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var objeto = _mapper.Map<Venda>(viewModel);
            objeto.VendaItem.AddRange(_mapper.Map<List<VendaItem>>(viewModel.VendaItens));
            objeto.VendaMoeda.AddRange(_mapper.Map<List<VendaMoeda>>(viewModel.VendaMoedas));
            
            var usuario = await _usuarioService.ObterPorUsuarioAspNetPorId(viewModel.Idusuario);
            if (usuario == null)
            {
                NotificarErro("Usuario não localizado");
                var msgErro = string.Join("\n\r", ObterNotificacoes("Venda", "Adicionar", "Web"));
                return CustomResponse(msgErro);
            }
            long _idEmpresa = Convert.ToInt64(viewModel.IdEmpresa);

            await _vendaService.RealizarVenda(objeto,usuario.Id,_idEmpresa);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Venda", "Adicionar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Venda", "Adicionar", null);
            return CustomResponse(viewModel);
        }
    }
}

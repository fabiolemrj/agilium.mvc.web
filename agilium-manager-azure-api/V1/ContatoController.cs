using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.ContatoViewModel;
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
    [Route("api/v{version:apiVersion}/contato")]
    [ApiVersion("1.0")]
    public class ContatoController : MainController
    {

        private readonly IContatoService _contatoService;
        private readonly IMapper _mapper;
        private const string _nomeEntidade = "Empresa";

        public ContatoController(INotificador notificador, IUser appUser, IContatoService contatoService,
            IMapper mapper, IConfiguration configuration, IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser,configuration,utilDapperRepository, logService)
        {
            _contatoService = contatoService;
            _mapper = mapper;
        }

        [HttpGet("obter-por-id")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ContatoEmpresa>> ObterPorId([FromQuery] long idContato, [FromQuery] long idEmpresa)
        {
            var objeto = _mapper.Map<ContatoEmpresaViewModel>(await _contatoService.ObterPorId(idContato, idEmpresa));
            return CustomResponse(objeto);
        }

        [HttpPost("contato-empresa")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Adicionar([FromBody] ContatoEmpresaViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var contatoEmpresa = _mapper.Map<ContatoEmpresa>(viewModel);

            if (contatoEmpresa.IDCONTATO == 0)
                contatoEmpresa.PopularContato(await GerarId());

            if (contatoEmpresa.Contato.Id == 0)
                contatoEmpresa.Contato.Id = contatoEmpresa.IDCONTATO;

            await _contatoService.Adicionar(contatoEmpresa);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Contato", "Adicionar", "Web"));
                return CustomResponse(msgErro);
            }
            await _contatoService.Salvar();
            LogInformacao($"Objeto Criado com sucesso {contatoEmpresa}", "Contato", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("contato-empresa")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar([FromBody] ContatoEmpresaViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _contatoService.Atualizar(_mapper.Map<ContatoEmpresa>(viewModel));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Contato", "Atualizar", "Web"));
                return CustomResponse(msgErro);
            }
            await _contatoService.Salvar();
            LogInformacao($"Objeto atualizado com sucesso {viewModel}", "Contato", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        [HttpDelete("contato-empresa")]
        public async Task<ActionResult> ExcluirContatoEmpresa([FromQuery] long idContato, [FromQuery] long idEmpresa)
        {
            await _contatoService.Apagar(idContato, idEmpresa);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Contato", "ExcluirContatoEmpresa", "Web"));
                return CustomResponse(msgErro);
            }
            await _contatoService.Salvar();
            LogInformacao($"Objeto excluir com sucesso idcontato: {idContato} idempresa:{idEmpresa}", "Contato", "Atualizar", null);
            return CustomResponse("sucesso");
        }
    }
}

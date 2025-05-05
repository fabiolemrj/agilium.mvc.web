using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.EnderecoViewModel;
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

  //  [Authorize]
    [Route("api/v{version:apiVersion}/endereco")]
    [ApiVersion("1.0")]
    public class EnderecoController : MainController
    {


        private readonly IEnderecoService _enderecoService;
        private readonly IMapper _mapper;
        public EnderecoController(INotificador notificador, IUser appUser, IEnderecoService enderecoService, IMapper mapper,
            IConfiguration configuration, IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser,configuration, utilDapperRepository,logService)
        {
            _enderecoService = enderecoService;
            _mapper = mapper;
        }


        [HttpGet("buscar-cep/{cep}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BuscarCep(string cep)
        {
            var objetoCep = await _enderecoService.ObterCepPorNumeroCep(cep);
            if (objetoCep == null)
            {
                var msgErro = "Numero de Cep não localizado";
                NotificarErro(msgErro);
                return CustomResponse(msgErro);
            }
            var cepViewModel = _mapper.Map<CepViewModel>(objetoCep);

            return CustomResponse(cepViewModel);
        }

        [HttpGet("teste")]
        public async Task<IActionResult> BuscarCep()
        {
            return Ok("sucesso");
        }
    }
}

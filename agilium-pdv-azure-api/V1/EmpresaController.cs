using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.pdv.Controllers;
using agilium.api.pdv.ViewModels.EmpresaViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.api.pdv.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/empresa")]
    [ApiVersion("1.0")]
    public class EmpresaController : MainController
    {
        private readonly IEmpresaService _empresaService;
        private readonly IMapper _mapper;
        private const string _nomeEntidade = "Empresa";
        public EmpresaController(INotificador notificador, IUser appUser, IEmpresaService empresaService,
         IMapper mapper, IConfiguration configuration, IUtilDapperRepository utilDapperRepository,
         ILogService logService) : base(notificador, appUser, configuration, utilDapperRepository, logService)
        {
            _empresaService = empresaService;
            _mapper = mapper;
        }

        [HttpGet("obter-todas")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EmpresaSimplesViewModel>>> ObterTodas()
        {
            var objeto = _mapper.Map<IEnumerable<EmpresaSimplesViewModel>>(await _empresaService.ObterTodas());
            return CustomResponse(objeto);
        }

    }
}

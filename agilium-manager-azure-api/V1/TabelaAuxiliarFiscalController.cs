using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.ImpostoViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.api.manager.V1
{

    [Authorize]
    [Route("api/v{version:apiVersion}/tabela-auxiliar-fiscal")]
    [ApiVersion("1.0")]
    public class TabelaAuxiliarFiscalController : MainController
    {
        private readonly ITabelaAuxiliarFiscalService _tabelaAuxiliarFiscalService;
        private readonly IMapper _mapper;
        public TabelaAuxiliarFiscalController(INotificador notificador, IUser appUser, IConfiguration configuration,
            ITabelaAuxiliarFiscalService tabelaAuxiliarFiscalService, IMapper mapper, 
            IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, 
                configuration, utilDapperRepository,logService)
        {
            _tabelaAuxiliarFiscalService = tabelaAuxiliarFiscalService;
            _mapper = mapper;
        }

        [HttpGet("obter-todas")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TabelasAxuliaresFiscalViewModel>>> ObterTodas()
        {
            var objeto = new TabelasAxuliaresFiscalViewModel();
            objeto.Cests  = _mapper.Map<List<CestViewModel>>(await _tabelaAuxiliarFiscalService.ObterTodosCestNcm());
            objeto.Csosn = _mapper.Map<List<CsosnViewModel>>(await _tabelaAuxiliarFiscalService.ObterTodosCsosn());
            objeto.Csts = _mapper.Map<List<CstViewModel>>(await _tabelaAuxiliarFiscalService.ObterTodosCst());
            objeto.Cfops = _mapper.Map<List<CfopViewModel>>(await _tabelaAuxiliarFiscalService.ObterTodosCfop());

            return CustomResponse(objeto);
        }
    }
}

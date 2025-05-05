using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.manager.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using agilium.api.business.Models;
using agilium.api.manager.ViewModels.LogSistemaViewModel;
using System.Linq;
using AutoMapper;

namespace agilium.api.manager.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/log")]
    [ApiVersion("1.0")]
    public class LogController : MainController
    {
        private readonly IMapper _mapper;
        public LogController(IMapper mapper,INotificador notificador, IUser appUser, IConfiguration configuration, IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, configuration, utilDapperRepository, logService)
        {
            _mapper = mapper;
        }

        [HttpGet("obter-por-data")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<LogSistema>>> ObterPorDataPaginacao([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string dtInicial = null, [FromQuery] string dtFinal = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();
            DateTime _dtInicial = Convert.ToDateTime(dtInicial);
            DateTime _dtFinal = Convert.ToDateTime(dtFinal);
            var lista = (await ObterListaPaginado( _dtInicial, _dtFinal, page, ps));

            return CustomResponse(lista);
        }

        #region Private
        private async Task<PagedResult<LogSistemaViewModel>> ObterListaPaginado( DateTime dtIni, DateTime dtFinal, int page, int pageSize)
        {
            var retorno = await _logService.ObterPorData(dtIni, dtFinal, page, pageSize);

            var lista = _mapper.Map<IEnumerable<LogSistemaViewModel>>(retorno.List);

            return new PagedResult<LogSistemaViewModel>()
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

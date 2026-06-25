using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Log;
using agilum.mvc.web.Extensions;
using agilium_manager_azure_business.Interfaces.IService;
using agilium.api.business.Models;
using Microsoft.AspNetCore.Identity;

namespace agilum.mvc.web.Controllers
{
    [Route("log")]
    [Authorize]
    public class LogController : MainController
    {

        public LogController(INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository,
            ILogService logService, IMapper mapper, ILicencaService licencaService, SignInManager<CaUsuarioIdentity> signInManager) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper, licencaService, signInManager)
        {
        }

        [Route("lista")]
        [ClaimsAuthorizeAttribute(1016)]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 25, [FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null)
        {

            var dataAtual = DateTime.Now;
            DateTime _dtini, _dtFim;
            if (DataInicial == null)
            {
                DateTime primeiroDiaDoMes = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                _dtini = primeiroDiaDoMes;
            }
            else _dtini = Convert.ToDateTime(DataInicial);

            if (DataFinal == null)
            {
                DateTime ultimoDiaDoMes = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month));
                _dtFim = ultimoDiaDoMes;
            }
            else _dtFim = Convert.ToDateTime(DataFinal);

            if (_dtini > _dtFim)
            {
                AdicionarErroValidacao("Data Final deve ser maior ou igual a data inicial");
            }

            var lista = (await ObterListaPaginado(_dtini, _dtFim, page, ps));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            lista.ReferenceAction = "lista";

            return View("Index", lista);
        }

        #region Private
        private async Task<PagedViewModel<LogSistemaViewModel>> ObterListaPaginado(DateTime dtIni, DateTime dtFinal, int page, int pageSize)
        {
            var retorno = await _logService.ObterPorData(dtIni, dtFinal, page, pageSize);

            var lista = _mapper.Map<IEnumerable<LogSistemaViewModel>>(retorno.List);

            return new PagedViewModel<LogSistemaViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "lista",
                ReferenceController ="  log",
                TotalResults = retorno.TotalResults
            };
        }
        #endregion
    }
}

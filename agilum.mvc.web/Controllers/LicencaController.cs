using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium_manager_azure_business.Interfaces.IService;
using agilum.mvc.web.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Authorize]
    [Route("licenca")]
    public class LicencaController : MainController
    {
        private readonly ILicencaService _licenca;
        public LicencaController(ILicencaService licenca, INotificador notificador, IConfiguration configuration,
            IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService, IMapper mapper, ILicencaService licencaService, SignInManager<AppUserAgiliumIdentity> signInManager) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper, licencaService, signInManager)
        {
            _licenca = licenca;
        }

        [Route("Index")]
        public async Task<ActionResult> Index()
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                return RedirectToAction("Index", "Home");
            }

            var objeto = await _licenca.ObterPorIdEmpresa("0",empresaSelecionada.IDEMPRESA);
            return View();
        }
    }
}

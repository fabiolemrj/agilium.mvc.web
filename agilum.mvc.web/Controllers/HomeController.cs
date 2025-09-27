using agilum.mvc.web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using agilium_manager_azure_business.Interfaces.IService;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using AutoMapper;
using agilum.mvc.web.ViewModels.Licenca;
using PassCrypto;
using agilium_manager_azure_business.Services;
using agilum.mvc.web.Data;
using Microsoft.AspNetCore.Identity;
using agilium.api.business.Services;
using agilum.mvc.web.Services;

namespace agilum.mvc.web.Controllers
{
    [Authorize]
    public class HomeController : MainController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;


        public HomeController(ILicencaService licenca,INotificador notificador,  IUser appUser, IUtilDapperRepository utilDapperRepository, IEmailSender emailSender,
        ILogService logService, IMapper mapper, IConfiguration configuration, ILicencaService licencaService, SignInManager<AppUserAgiliumIdentity> signInManager) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper, licencaService, signInManager)
        {
            _emailSender = emailSender;
        }

        [Route("licenca")]
        public async Task<ActionResult> Licenca()
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                return RedirectToAction("Index", "Home");
            }
            await Criptografar();
            await ObterEmailConfig();

            //var objeto = await _licenca.ObterPorIdEmpresa("0", empresaSelecionada.IDEMPRESA);
            //var viewModel = _mapper.Map<LicencaViewModel>(objeto);
            //viewModel.K1 = Descriptografar(objeto.K1);
            //viewModel.K2 = Descriptografar(objeto.K2);
            //viewModel.K3 = Descriptografar(objeto.K3);
            //viewModel.K4 = Descriptografar(objeto.K4);
            //viewModel.K5 = Descriptografar(objeto.K5);
            //viewModel.K6 = Descriptografar(objeto.K6);
            //viewModel.K7 = Descriptografar(objeto.K7);
            return RedirectToAction("Index");
        }

        private async Task Criptografar()
        {
            var emailService = _emailSender.GravarCriptografia();
        }

        private async Task ObterEmailConfig()
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                return;
            }

            var emailService = _emailSender.ObterConfigEmail(Convert.ToInt64(empresaSelecionada.IDEMPRESA));
        }
       
        public ActionResult Index()
        {
            //VerificarValidadeLicenca();
            return View();
        }

        [AllowAnonymous]
        [Route("ObterVersaoSistema")]
        public ActionResult ObterVersaoSistema()
        {
            
            var versaobd_major = Convert.ToInt32(_configuration.GetConnectionString("versaobd-major"));
            var versao_major = _configuration.GetConnectionString("versao-major");
            var _versao = $"Versão: {_configuration.GetConnectionString("versao-major")}.{_configuration.GetConnectionString("versao-minor")}.{_configuration.GetConnectionString("versao-build")}" ;
            return Json(new{versao = _versao });
        }

        [AllowAnonymous]
        [Route("sistema-indisponivel")]
        public IActionResult SistemaIndisponivel()
        {
            var modelErro = new ErrorViewModel
            {
                Mensagem = "O sistema está temporariamente indisponível, porque ocorreu um erro interno do sistema.",
                Titulo = "Sistema indisponível.",
                ErroCode = 500
            };

            return View("Error", modelErro);
        }

        [AllowAnonymous]
        [Route("error/{id}")]
        public IActionResult Error(int id)
        {
            var modelErro = new ErrorViewModel();

           
            if (id == 404)
            {
                modelErro.Mensagem = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
                modelErro.Titulo = "Ops! Página não encontrada.";
                modelErro.ErroCode = id;
            }
            else if (id == 403)
            {
                modelErro.Mensagem = "Você não tem permissão para fazer isto.";
                modelErro.Titulo = "Acesso Negado";
                modelErro.ErroCode = id;
            }
            else if (id == 500)
            {
                modelErro.Mensagem = "Ocorreu um erro interno! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Titulo = "Ocorreu um erro!";
                modelErro.ErroCode = id;
            }
            else
            {
                modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Titulo = "Ocorreu um erro!";
                modelErro.ErroCode = id;
            }

            return View("Error", modelErro);
        }

    }
}


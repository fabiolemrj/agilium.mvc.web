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
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.business.Services;
using agilum.mvc.web.Services;
using System.Linq;

namespace agilum.mvc.web.Controllers
{
    [Authorize]
    public class HomeController : MainController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IConfigImagemRepository _configImagemRepository;


        public HomeController(ILicencaService licenca,INotificador notificador,  IUser appUser, IUtilDapperRepository utilDapperRepository, IEmailSender emailSender,
        ILogService logService, IMapper mapper, IConfiguration configuration, ILicencaService licencaService, IAuthService authService, IConfigImagemRepository configImagemRepository) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper, licencaService, authService)
        {
            _emailSender = emailSender;
            _configImagemRepository = configImagemRepository;
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
       
        public async Task<ActionResult> Index()
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                TempData["TipoMensagem"] = "warning";
                TempData["Titulo"] = "Empresa";
                TempData["Mensagem"] = "Selecione uma empresa para acessar o sistema";
                return RedirectToAction("ObterListasEmpresasPorUsuario", "Empresa");
            }

            ViewBag.LogoCliente = null;
            try
            {
                var configImagem = (await _configImagemRepository.Obter(x => x.CHAVE == "IMG_LOGO" && x.IDEMPRESA == Convert.ToInt64(empresaSelecionada.IDEMPRESA)))
                    .FirstOrDefault();
                if (configImagem?.IMG != null && configImagem.IMG.Length > 0)
                    ViewBag.LogoCliente = Convert.ToBase64String(configImagem.IMG);
            }
            catch
            {
                // fallback para logo padrão
            }

//            await VerificarValidadeLicenca();
            return View();
        }

        [AllowAnonymous]
        [Route("ObterVersaoSistema")]
        public ActionResult ObterVersaoSistema()
        {
            
            var versaobd_major = Convert.ToInt32(_configuration.GetConnectionString("versaobd-major"));
            var versao_major = _configuration.GetConnectionString("versao-major");
            var _versao = $"Vers�o: {_configuration.GetConnectionString("versao-major")}.{_configuration.GetConnectionString("versao-minor")}.{_configuration.GetConnectionString("versao-build")}" ;
            return Json(new{versao = _versao });
        }

        [AllowAnonymous]
        [Route("sistema-indisponivel")]
        public IActionResult SistemaIndisponivel()
        {
            var modelErro = new ErrorViewModel
            {
                Mensagem = "O sistema est� temporariamente indispon�vel, porque ocorreu um erro interno do sistema.",
                Titulo = "Sistema indispon�vel.",
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
                modelErro.Mensagem = "A p�gina que est� procurando n�o existe! <br />Em caso de d�vidas entre em contato com nosso suporte";
                modelErro.Titulo = "Ops! P�gina n�o encontrada.";
                modelErro.ErroCode = id;
            }
            else if (id == 403)
            {
                modelErro.Mensagem = "Voc� n�o tem permiss�o para fazer isto.";
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


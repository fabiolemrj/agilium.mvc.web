using agilum.mvc.web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public ActionResult Index()
        {
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


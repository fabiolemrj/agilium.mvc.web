using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using agilium.webapp.manager.mvc.Models;
using agilium.webapp.manager.mvc.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace agilium.webapp.manager.mvc.Controllers
{
    [Authorize]
    public class HomeController : MainController
    {

        public async Task<ActionResult> Index([FromServices] IConfigServices configServices)
        {
            ViewBag.LogoCliente = null;

            var idEmpresa = ObterIdEmpresaSelecionada();
            if (idEmpresa > 0)
            {
                try
                {
                    var configImagem = await configServices.ObterConfigImagemPorId(idEmpresa, "IMG_LOGO");
                    if (configImagem?.IMG != null && configImagem.IMG.Length > 0)
                        ViewBag.LogoCliente = Convert.ToBase64String(configImagem.IMG);
                }
                catch
                {
                    // fallback para logo padrão
                }
            }

            return View();
        }

        [AllowAnonymous]
        [Route("sistema-indisponivel")]
        public IActionResult SistemaIndisponivel()
        {
            var modelErro = new ErrorViewModel
            {
                Mensagem = "O sistema está temporariamente indisponível, isto pode ocorrer em momentos de sobrecarga de usuários.",
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

            if (id == 500)
            {
                modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Titulo = "Ocorreu um erro!";
                modelErro.ErroCode = id;
            }
            else if (id == 404)
            {
                modelErro.Mensagem =
                    "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
                modelErro.Titulo = "Ops! Página não encontrada.";
                modelErro.ErroCode = id;
            }
            else if (id == 403)
            {
                modelErro.Mensagem = "Você não tem permissão para fazer isto.";
                modelErro.Titulo = "Acesso Negado";
                modelErro.ErroCode = id;
            }
            else
            {
                return StatusCode(404);
            }

            return View("Error", modelErro);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace agilium.webapp.manager.mvc.Controllers
{
    public class IdentidadeController : MainController
    {
        private readonly IAutenticacaoService _autenticacaoService;
        private readonly IAspNetUser _aspNetUser;
        private readonly ILogger<UsuarioController> _logger;
        private readonly string senhaPadrao;

        public IdentidadeController(IAutenticacaoService autenticacaoService,
            IAspNetUser aspNetUser,
            ILogger<UsuarioController> logger)
        {
            _autenticacaoService = autenticacaoService;
            _aspNetUser = aspNetUser;
            _logger = logger;
            senhaPadrao = "Abc@123";
        }


        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UsuarioLogin usuarioLogin, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            try
            {
                if (!ModelState.IsValid) return View(usuarioLogin);

                var resposta = await _autenticacaoService.Login(usuarioLogin);

                if (ResponsePossuiErros(resposta.ResponseResult)) return View(usuarioLogin);

                await _autenticacaoService.RealizarLogin(resposta);

                if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

                return LocalRedirect(returnUrl);
            }
            catch (Exception)
            {
                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Login";
                TempData["Mensagem"] = "Não foi possivel realizar login, entre em contato com a administração do sistema";

                return RedirectToAction("login", "identidade");
            }
            
        }
        
        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> Logout()
        {

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("fotousuario")))
                HttpContext.Session.Remove("fotousuario");

            await _autenticacaoService.Logout();
            return RedirectToAction("login", "identidade");
        }

        [HttpGet]
        [Route("mudar-senha")]
        [Authorize]
        public async Task<IActionResult> MudarSenha()
        {
            if (!_aspNetUser.EstaAutenticado())
            {
                return NotFound($"Incapaz de carregar usuário '{_aspNetUser.ObterUserEmail()}'.");
            }

            var usuario = new UserChangePassword();
            usuario.Email = _aspNetUser.ObterUserEmail();
            return View(usuario);
        }

        [Route("mudar-senha")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MudarSenha(UserChangePassword userChangePassword, string returnUrl = null)
        {
            if (!ModelState.IsValid) return View(userChangePassword);

            var resposta = await _autenticacaoService.MudarSenha(userChangePassword);

            if (ResponsePossuiErros(resposta.ResponseResult)) 
            {
                var msgErro = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                if (ResponsePossuiErros(resposta.ResponseResult)) TempData["Erros"] = msgErro;
                       

                var retornoErro = new { mensagem = msgErro, usuarioLogado = _aspNetUser.ObterUserEmail() };
                _logger.LogError(retornoErro.ToString());

                return View(userChangePassword); 
            }

            TempData["TipoMensagem"] = "Success";
            TempData["Mensagem"] = resposta.objeto.ToString();


            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        [Route("nova-conta")]   
        public IActionResult Registro()
        {
            return View("NovoUsuario");
        }

        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro(RegisterModel usuarioRegistro)
        {
            usuarioRegistro.Password = senhaPadrao;
            usuarioRegistro.ConfirmPassword = senhaPadrao;

            if (!ModelState.IsValid)
            {
                return RedirectToAction("NovoUsuario",usuarioRegistro);
                
            }

            var resposta = await _autenticacaoService.Registro(usuarioRegistro);

            if (ResponsePossuiErros(resposta.ResponseResult))
            {
                var retornoErro = new { mensagem = "Erro ao criar novo usuario", usuarioLogado = _aspNetUser.ObterUserEmail() };
                _logger.LogError(retornoErro.ToString());
                return RedirectToAction("NovoUsuario", usuarioRegistro);
                //return View(usuarioRegistro);
            }
                

            TempData["TipoMensagem"] = "Success";
            TempData["Mensagem"] = $@"Usuario criado com sucesso!";

           // await _autenticacaoService.RealizarLogin(resposta);

            return RedirectToAction("ObterTodosUsuarios", "Usuario");
        }

        [HttpGet]
        [Route("esqueci-senha")]
        public IActionResult EsqueciSenha()
        {
            var userForgotPassword = new UserForgotPassword();
            return View(userForgotPassword);
        }

        [HttpPost]
        [Route("esqueci-senha")]
        public async Task<IActionResult> EsqueciSenha(UserForgotPassword userForgotPassword)
        {
            if (!ModelState.IsValid) return View(userForgotPassword);

            var resposta = await _autenticacaoService.EsqueciSenha(userForgotPassword);

            if (ResponsePossuiErros(resposta.ResponseResult))
            {
                if (ResponsePossuiErros(resposta.ResponseResult)) TempData["Erros"] =
                        ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

                return View(userForgotPassword);
            }

            TempData["TipoMensagem"] = "Success";
            TempData["Mensagem"] = resposta.objeto.ToString();
            
            return RedirectToAction("ConfirmacaoEsqueciSenha", "Identidade");

        }

        public  IActionResult ConfirmacaoEsqueciSenha()
        {
            return View();
        }

        public IActionResult RedefinirSenha(string code = null, string userId = null, string email = null)
        {
            if (code == null)
            {
                AdicionarErroValidacao("Um código deve ser fornecido para redefinir a senha.");
                return View();
            }

            var user = new UserResetPassword() { Code = code, Email = email  };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RedefinirSenha(UserResetPassword userResetPassword)
        {
            if (!ModelState.IsValid) return View(userResetPassword);

            var resposta = await _autenticacaoService.RedefinirSenha(userResetPassword);

            if (ResponsePossuiErros(resposta.ResponseResult))
            {
                if (ResponsePossuiErros(resposta.ResponseResult)) TempData["Erros"] =
                        ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

                return View(userResetPassword);
            }

            TempData["TipoMensagem"] = "Success";
            TempData["Mensagem"] = resposta.objeto.ToString();

            return RedirectToAction("ConfirmacaoRedefinirSenha", "Identidade");
        }

        public IActionResult ConfirmacaoRedefinirSenha()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObterClaims()
        {
            var viewModel = new ListaClaims();
            viewModel.Claims =  await _autenticacaoService.ObterClaims();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> NovaClaim()
        {
            var claim = new UserClaimViewModel();
            return View(claim);
        }

        [HttpPost]
        public async Task<IActionResult> NovaClaim(UserClaimViewModel userClaim)
        {
            if (!ModelState.IsValid) return View(userClaim);

            var resposta = await _autenticacaoService.NovaClaim(userClaim);

            if (ResponsePossuiErros(resposta))
            {
                if (ResponsePossuiErros(resposta)) 
                    TempData["Erros"] =  ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

                return View(userClaim);
            }

            TempData["TipoMensagem"] = "Success";
            TempData["Mensagem"] = "Claim criada com sucesso";

            return RedirectToAction("ObterClaims", "Identidade");
        }


    }
}
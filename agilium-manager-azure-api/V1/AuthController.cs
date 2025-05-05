using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.Data;
using agilium.api.manager.Extension;
using agilium.api.manager.Services;
using agilium.api.manager.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using static agilium.api.manager.ViewModels.UserViewModel;
using KissLog;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using agilium.api.business.Interfaces.IService;

namespace agilium.api.manager.V1
{
    [Route("api/v{version:apiVersion}")]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    public class AuthController : MainController
    {

        private readonly SignInManager<AppUserAgilium> _signInManager;
        private readonly UserManager<AppUserAgilium> _userManager;
        private readonly AppSettings _appSettings;
        private readonly IUsuarioService _usuarioService;
        private readonly IEmailSender _emailSender;
        private readonly AppTokenSettings _appTokenSettingsSettings;
        private readonly ApplicationDbContext _context;
        private readonly IUserClaimsManagerService _userClaimsManagerService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;
        private string urlFrontBase = string.Empty;
        
        public AuthController(INotificador notificador,
                                IUser appUser,
                                SignInManager<AppUserAgilium> signInManager,
                                UserManager<AppUserAgilium> userManager,
                                IUsuarioService usuarioService,
                                IOptions<AppSettings> appSettings,
                                IOptions<AppTokenSettings> appTokenSettingsSettings,
                                ApplicationDbContext context,
                                 IEmailSender emailSender,
                                 IUserClaimsManagerService userClaimsManagerService,
                                 IMapper mapper,
                                  IConfiguration configuration,
                                 ILogger<AuthController> logger,
                                 IUtilDapperRepository utilDapperRepository,
                                 ILogService logService) : base(notificador, appUser, configuration, utilDapperRepository, logService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _usuarioService = usuarioService;
            _emailSender = emailSender;
            _context = context;
            _appTokenSettingsSettings = appTokenSettingsSettings.Value;
            _userClaimsManagerService = userClaimsManagerService;
            _mapper = mapper;
            _logger = logger;
            urlFrontBase = _configuration.GetSection("front-info").GetSection("resetPass").Value;
         
        }

        [HttpPost("usuarioTeste")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Adicionar([FromBody] UsuarioPadrao viewModel)
        {
            if (!ModelState.IsValid)
            {
                NotificarErro("erro ao tentar adicionar usuario");
                ObterNotificacoes().ToList().ForEach( erro => {
                    LogErro(erro, "Auth", "Adicionar", null, "Web");  
                });
                return CustomResponse(ModelState);
            }

            //  await _usuarioService.Adicionar(_mapper.Map<Usuario>(viewModel));

            return

                CustomResponse(viewModel);
        }

        #region endpoints
        [HttpGet("teste")]
        public async Task<ActionResult> Teste()
        {
            return Ok("Sucesso");

        }

        /// <summary>
        /// Endpoint responsavel pelo cadastro de um novo usuario
        /// </summary>
        /// <param name="registerUser">Estrutura necessaria para cadastro de um novo usuário</param>
        /// <returns></returns>
        [HttpPost("nova-conta")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Authorize]
        public async Task<ActionResult> Registrar([FromBody]RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) 
            {
                NotificarErro("erro ao tentar adicionar usuario");
                ObterNotificacoes("Auth", "Registrar","Web");

                return CustomResponse(ModelState); 
            }

            var user = new AppUserAgilium
            {
                Ativo = (int)EAtivo.Ativo,
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true,
                Nome = registerUser.Usuario,
                CPF = registerUser.CPF
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                LogInformacao($"Usuario Criado com sucesso {registerUser.Email}","Auth","Registrar",null);
                var usarioCriado = new { Nome = user.Nome, email = user.UserName };

                if (!AdicionarUsuario(registerUser, user.Id))
                {
                    NotificarErro("Não foi possivel criar o usuário");
                 
                    await _userManager.DeleteAsync(user);
                }
                if(!await NotificarNovoUsuarioCriado(user.UserName))
                {
                    NotificarErro("O usuário foi criado, mas não foi possível enviar o email para troca da senha");
                    await _userManager.DeleteAsync(user);
                }

                ObterNotificacoes("Auth", "Registrar", "Web");
                return CustomResponse(usarioCriado);
               // await _signInManager.SignInAsync(user, false);
               // return CustomResponse(await GerarJwt(user.Email));

            }
            foreach (var error in result.Errors)
            {
                NotificarErro(error.Description);
            }

            ObterNotificacoes("Auth", "Registrar", "Web");
            return CustomResponse(registerUser);
        }

        /// <summary>
        /// Endpoint responsavel por realizar login no sistema
        /// </summary>
        /// <param name="loginUser">estrutra de campos necessaria para realizar login</param>
        /// <returns></returns>
        [HttpPost("entrar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<ActionResult> Login([FromBody] LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                LogInformacao($"Login {loginUser.Email}", "Auth", "Login", null);
                var appUserAgilium = await _userManager.FindByEmailAsync(loginUser.Email);

                if (appUserAgilium.Ativo == (int)EAtivo.Ativo)
                {
            
                    var resp = await GerarJwt(loginUser.Email);
                    await _logService.Adicionar(loginUser.Email,"Login realizado","Auth","Login","Web",null,null);
                    return CustomResponse(resp);
                }

            
                NotificarErro("Usuário está inativo");
                ObterNotificacoes("Auth", "Login", "Web");
                return CustomResponse(loginUser);
            }

            if (result.IsLockedOut)
            {
                _logger.LogInformation("Usuário está inativo"); 
                NotificarErro("Usuário temporariamente bloqueado por tentativas inválidas");
                ObterNotificacoes("Auth", "Login", "Web");
                return CustomResponse(loginUser);
            }

            _logger.LogError("Usuário ou Senha incorretos");
            NotificarErro("Usuário ou Senha incorretos");
            ObterNotificacoes("Auth", "Login", "Web");
            return CustomResponse(loginUser);
        }

        [HttpPost("login-refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> LoginRefreshToken([FromBody]  RefreshTokenUser refreshToken)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (string.IsNullOrEmpty(refreshToken.refreshToken))
            {
                NotificarErro("Refresh Token inválido");
                ObterNotificacoes("Auth", "LoginRegreshToken", "Web");
                return CustomResponse();
            }

            var token = await ObterRefreshToken(Guid.Parse(refreshToken.refreshToken));

            if (token is null)
            {
                NotificarErro("Refresh Token expirado");
                ObterNotificacoes("Auth", "LoginRegreshToken", "Web");
                return CustomResponse();
            }

            var appUserAgilium = await _userManager.FindByEmailAsync(token.Username);

            if (appUserAgilium == null)
            {
                NotificarErro("Usuário não localizado");
                NotificarErro("Não foi possivel fazer login/refreshToken com usuario atual");
                ObterNotificacoes("Auth", "LoginRegreshToken", "Web");
                return CustomResponse();
            }
            
            await _signInManager.SignInAsync(appUserAgilium,false);

            if (appUserAgilium.Ativo == (int)EAtivo.Ativo)
            {
                var resp = await GerarJwt(token.Username);
      
                return CustomResponse(resp);
            }

            NotificarErro("Não foi possível fazer login com usuario");
            ObterNotificacoes("Auth", "LoginRegreshToken", "Web");

            return CustomResponse(Forbid());

        }

        /// <summary>
        /// Endpoint responavel por desativar usuarios que possuem acesso ao sistema
        /// </summary>
        /// <param name="idUsuarioAspNet">Id do usuario</param>
        /// <returns></returns>
        [HttpGet("desativar-usuario/{idUsuarioAspNet}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Authorize]
        public async Task<ActionResult> DesativarUsuario(string idUsuarioAspNet)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var appUserAgilium = await _userManager.FindByIdAsync(idUsuarioAspNet);

            if (appUserAgilium == null)
            {
                NotificarErro("Usuário Não localizado, procure o administrador do sistema!");
                ObterNotificacoes("Auth", "DesativarUsuario", "Web");
                return CustomResponse($@"Id do usuario: {idUsuarioAspNet} ");
            }

            var usuario = await _usuarioService.ObterPorUsuarioAspNetPorId(appUserAgilium.Id);

            if (usuario == null)
            {
            
                NotificarErro("Usuário Não localizado, procure o administrador do sistema!");
                ObterNotificacoes("Auth", "DesativarUsuario", "Web");
                return CustomResponse($@"Id do usuario: {idUsuarioAspNet} ");
            }

            await _usuarioService.DesativarUsuario(usuario.Id);
            appUserAgilium.Ativo = (int)EAtivo.Inativo;
            appUserAgilium.LockoutEnabled = false;
            await _userManager.UpdateAsync(appUserAgilium);
            //
            var msg = $@"Usuário: {usuario.nome} Bloqueado e/ou Inativado com sucesso!";
            LogInformacao($"{msg}", "Auth","DesativarUsuario", "Web");
            return CustomResponse(msg);
        }

        /// <summary>
        /// Endpoint responsável por reativar os usuarios inativos
        /// </summary>
        /// <param name="idUsuarioAspNet">id do usuario</param>
        /// <returns></returns>
        [HttpGet("ativar-usuario/{idUsuarioAspNet}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Authorize]
        public async Task<ActionResult> AtivarUsuario(string idUsuarioAspNet)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var appUserAgilium = await _userManager.FindByIdAsync(idUsuarioAspNet);

            if (appUserAgilium == null)
            {
                NotificarErro("Usuário Não localizado, procure o administrador do sistema!");
                ObterNotificacoes("Auth", "AtivarUsuario", "Web");
                return CustomResponse($@"Id do usuario: {idUsuarioAspNet} ");
            }

            var usuario = await _usuarioService.ObterPorUsuarioAspNetPorId(appUserAgilium.Id);

            if (usuario == null)
            {
                NotificarErro("Usuário Não localizado, procure o administrador do sistema!");
                ObterNotificacoes("Auth", "AtivarUsuario", "Web");
                return CustomResponse($@"Id do usuario: {idUsuarioAspNet} ");
            }

            await _usuarioService.AtivarUsuario(usuario.Id);
            appUserAgilium.Ativo = (int)EAtivo.Ativo;
            appUserAgilium.LockoutEnabled = true;
            await _userManager.UpdateAsync(appUserAgilium);
            //
            var msg = $@"Usuário: {usuario.nome} Ativado e/ou desbloqueado com sucesso!";
            LogInformacao($"{msg}", "Auth", "AtivarUsuario", "Web");
            return CustomResponse(msg);
        }

        [HttpPost("mudar-senha")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Authorize]
        public async Task<ActionResult> MudarSenha([FromBody]UserChangePassword userChangePassword)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = await _userManager.FindByEmailAsync(userChangePassword.Email);

            if (user == null)
            {
                NotificarErro("Usuário Não localizado, procure o administrador do sistema!");
                ObterNotificacoes("Auth", "MudarSenha", "Web");
                return CustomResponse($@"Email: {user.Email} ");
            }


            var changePasswordResult = await _userManager.ChangePasswordAsync(user, userChangePassword.OldPassword, userChangePassword.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    NotificarErro(error.Description);
                }
                ObterNotificacoes("Auth", "MudarSenha", "Web");
                return CustomResponse($@"Email: {user.Email} ");
            }

            await _signInManager.RefreshSignInAsync(user);

            var msg = "Senha alterada com sucesso!";
            var resposta = new RespPadrao()
            {
                objeto = msg
            };
            LogInformacao($"{msg}", "Auth", "MudarSenha", "Web");
            return CustomResponse(resposta);
        }

        [HttpPost("esqueci-senha")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<ActionResult> EsqueciSenha([FromBody]UserForgotPassword userForgotPassword)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var user = await _userManager.FindByEmailAsync(userForgotPassword.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                NotificarErro("Usuário Não localizado, procure o administrador do sistema!");
                ObterNotificacoes("Auth", "EsqueciSenha", "Web");
                return CustomResponse($@"Email: {userForgotPassword.Email} ");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            //var callbackUrl = Url.Page("/api/v1/esqueci-senha",
            //    pageHandler: null,
            //    values: new { userId = user.Id, code = code },
            //    protocol: Request.Scheme);
            var callbackUrl = Url.Action("RedefinirSenha", "Identidade", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme, host: urlFrontBase);

            await _emailSender.SendEmailAsync(
                userForgotPassword.Email,
                "Alterar a senha - Agilium Manager",
                $"Por favor, redefina sua senha clicando <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'> aqui</a>.");

            var msg = "Foi enviado um email para redefinir senha do usuario!";
            var resposta = new RespPadrao()
            {
                objeto = msg
            };
            LogInformacao($"{msg}", "Auth", "EsqueciSenha", "Web");
            return CustomResponse(resposta);
        }

        [HttpPost("esqueci-senhaApp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<ActionResult> EsqueciSenhaApp([FromBody]UserForgotPassword userForgotPassword)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (string.IsNullOrEmpty(urlFrontBase))
            {
                NotificarErro("URL do sistema para refazer a senha não encontrado, procure o administrador do sistema!");
                ObterNotificacoes("Auth", "EsqueciSenhaApp", "Web");
                return CustomResponse($@"Email: {userForgotPassword.Email} ");
            }

            var user = await _userManager.FindByEmailAsync(userForgotPassword.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                NotificarErro("Usuário Não localizado, procure o administrador do sistema!");
                ObterNotificacoes("Auth", "EsqueciSenhaApp", "Web");
                return CustomResponse($@"Email: {userForgotPassword.Email} ");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            //var callbackUrl = Url.Page("/api/v1/esqueci-senha",
            //    pageHandler: null,
            //    values: new { userId = user.Id, code = code },
            //    protocol: Request.Scheme);
            var callbackUrl = Url.Action("redefinir-senha", "conta", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme, host: urlFrontBase);

            await _emailSender.SendEmailAsync(
                userForgotPassword.Email,
                "Alterar a senha - Agilium Manager",
                $"Por favor, redefina sua senha clicando <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'> aqui</a>.");

            var msg = "Foi enviado um email para redefinir senha do usuario!";
            var resposta = new RespPadrao()
            {
                objeto = msg
            };
            LogInformacao($"{msg}", "Auth", "EsqueciSenhaApp", "Web");
            return CustomResponse(resposta);
        }


        [HttpPost("redefinir-senha")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<ActionResult> RedefinirSenha([FromBody]UserResetPassword userReset)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            
            if (string.IsNullOrEmpty(urlFrontBase))
            {
                NotificarErro("URL do sistema para refazer a senha não encontrado, procure o administrador do sistema!");
                ObterNotificacoes("Auth", "RedefinirSenha", "Web");
                return CustomResponse($@"Email: {userReset.Email} ");
            }

            var user = await _userManager.FindByEmailAsync(userReset.Email);
            if (user == null)
            {
                NotificarErro("Usuário Não localizado, procure o administrador do sistema!");
                ObterNotificacoes("Auth", "RedefinirSenha", "Web");
                return CustomResponse($@"Email: {userReset.Email} ");
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userReset.Code));
            var result = await _userManager.ResetPasswordAsync(user, code, userReset.Password);
            if (result.Succeeded)
            {
                var msg = "Senha foi alterada com sucesso!";
                var resposta = new RespPadrao()
                {
                    objeto = msg
                };
                LogInformacao($"{msg}", "Auth", "EsqueciSenhaApp", "Web");
                return CustomResponse(resposta);
            }

            foreach (var error in result.Errors)
            {
                NotificarErro(error.Description);
            }
            ObterNotificacoes("Auth", "RedefinirSenha", "Web");
            return CustomResponse($@"Email: {user.Email} ");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                NotificarErro("Refresh Token inválido");
               // ObterNotificacoes("Auth", "RefreshToken", "Web");
                return CustomResponse();
            }

            var token = await ObterRefreshToken(Guid.Parse(refreshToken));

            if (token is null)
            {
                NotificarErro("Refresh Token expirado");
                //ObterNotificacoes("Auth", "RefreshToken", "Web");
                return CustomResponse();
            }

            return CustomResponse(await GerarJwt(token.Username));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost("atualiza-claims")]
        public async Task<ActionResult> AtualizarClaimsUsuarios([FromBody] UserClaim userClaim)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = await _userManager.FindByEmailAsync(userClaim.email);
            if (user == null)
            {
                NotificarErro("Usuário Não localizado, procure o administrador do sistema!");
                ObterNotificacoes("Auth", "AtualizarClaimsUsuarios", "Web");
                return CustomResponse($@"Email: {user.Email} ");
            }
            var claims = new List<Claim>();

            var oldClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(oldClaims);

            if (!await RemoverClaimsUsuario(user))
                return CustomResponse();

            userClaim.claimModels.ForEach(x => claims.Add(new Claim(x.ClaimType, x.ClaimValue)));

            if (!await AtualizarClaimsUsuario(user, claims))
                return CustomResponse();

            var resposta = new RespPadrao()
            {
                objeto = "Claims atualizados com sucesso!"
            };

            return CustomResponse(resposta);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet("claims")]
        public async Task<ActionResult> ListaClaims()
        {
            var claims = _mapper.Map<List<UserClaimViewModel>>(await _userClaimsManagerService.ObterTodos()); 
            return CustomResponse(claims);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost("adicionar-claim")]
        public async Task<ActionResult> AdicionarClaim([FromBody] UserClaimViewModel userClaim)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _userClaimsManagerService.Adicionar(_mapper.Map<ObjetoClaim>(userClaim));

            return CustomResponse(userClaim);
        }

        [ClaimsAuthorize("Usuario", "Atualizar")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserClaimViewModel>> Atualizar(string id, [FromBody]  UserClaimViewModel viewModel)
        {

            if (id != viewModel.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                ObterNotificacoes("Auth", "Atualizar", "Web");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _userClaimsManagerService.Atualizar(_mapper.Map<ObjetoClaim>(viewModel));

            var objetoDeserialziado = Deserializar(viewModel);
            LogInformacao($"{objetoDeserialziado} - atualizacao", "Auth", "Atualizar", "Web");

            return CustomResponse(viewModel);
        }

        //   [ClaimsAuthorize("Usuario", "Excluir")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserClaimViewModel>> Excluir(string id)
        {
            var viewModel = await _userClaimsManagerService.ObterClaimPorId(Convert.ToInt64(id));

            if (viewModel == null) return NotFound();

            if(await ClaimEmUso(viewModel.ClaimType))
            {
                NotificarErro("Esta claim não pode ser excluida por que está em uso!");
                ObterNotificacoes("Auth", "Excluir", "Web");
                return CustomResponse(viewModel);
            }

            await _userClaimsManagerService.Remover(viewModel.Id);

            return CustomResponse(viewModel);
        }

        [HttpGet("obter-claim/{claim}")]
        public async Task<bool> ListaClaims(string claim)
        {
            return await ClaimEmUso(claim);
            
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet("claims-values")]
        public async Task<ActionResult<List<string>>> ObterListaClaimValue()
        {
            var listaClaimValue = await _userClaimsManagerService.ObterListaClaimValues();
            
            if (listaClaimValue == null) return NotFound();
            var listaConvertidaStringClaimValue = new List<string>();
            listaClaimValue.ForEach(claimValue =>
            {
                listaConvertidaStringClaimValue.Add(claimValue.Value);
            });
            return CustomResponse(listaConvertidaStringClaimValue);

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPut("atualizar-usuario-claim/{id}")]
        public async Task<ActionResult> AtualizarClaimUsuario(string id, [FromBody] List<ClaimSelecionadaViewModel> viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var appUserAgilium = await _userManager.FindByIdAsync(id);

            if (appUserAgilium == null)
            {
                NotificarErro("Usuário Não localizado, procure o administrador do sistema!");
                ObterNotificacoes("Auth", "AtualizarClaimUsuario", "Web");
                var ret = CustomResponse($@"Id do usuario: {id} ");
                return ret;
            }

            var userClaimsExistentes = await _userManager.GetClaimsAsync(appUserAgilium);
            var novasClaims = new List<Claim>();
            viewModel.ForEach(claim => {
                
                claim.ClaimValue.ForEach(acao => {
                    if(!userClaimsExistentes.Any(x => x.Type.ToUpper().Trim() == claim.claim.ToUpper().Trim() && x.Value.ToUpper().Trim() == acao.ToUpper().Trim()))
                    {
                        var novaClaim = new Claim(claim.claim.ToUpper().Trim(), acao.ToUpper().Trim());
                        novasClaims.Add(novaClaim);
                    }                    
                });                
            });

            await _userManager.AddClaimsAsync(appUserAgilium,novasClaims);

            return CustomResponse(true);
        }

        [Produces("application/json")]
        [HttpPost("duplicar-claim-usuario")]
        public async Task<ActionResult> DuplicarClaimPorUsuarioBase([FromBody] DuplicarClaimUsuarioViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
                        
            var userClaimsExistentesBase = await ObterClaimsPorUsuarioId(viewModel.idOrigem);
            if (userClaimsExistentesBase == null)
            {
                return CustomResponse("Não foram encontradas claims para o usuario base para a operação");
            }


            var appUserAgiliumDestino = await _userManager.FindByIdAsync(viewModel.idDestino);

            if (appUserAgiliumDestino == null)
            {
                return CustomResponse("Usuario destino não foi localizado");
            }
            
            var userClaimsExistentesDestino = await ObterClaimsPorUsuarioId(viewModel.idDestino);

            var novasClaims = new List<Claim>();
            userClaimsExistentesBase.ToList().ForEach(claim => {

                if (!userClaimsExistentesDestino.Any(x => x.Type.ToUpper().Trim() == claim.Type.ToUpper().Trim() && x.Value.ToUpper().Trim() == claim.Value.ToUpper().Trim()))
                {
                    var novaClaim = new Claim(claim.Type.ToUpper().Trim(), claim.Value.ToUpper().Trim());
                    novasClaims.Add(novaClaim);
                }
            });

            await _userManager.AddClaimsAsync(appUserAgiliumDestino, novasClaims);
            
            return CustomResponse(true);
        }

        [Produces("application/json")]
        [HttpGet("obter-usuario-e-claims-por-usuarioid/{id}")]
        public async Task<ActionResult> ObterUsuarioComClaimsPorId(string id)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var usuarioClaim = new UsuarioClaimsViewModel();
            var usuario = _mapper.Map<UsuarioPadrao>(await _usuarioService.ObterPorUsuarioAspNetPorId(id));
            if (usuario == null)
            {
                NotificarErro($@"Usuario não localizado");
                return CustomResponse();
            }
            usuarioClaim.Usuario = usuario;

            var claimsAspNet = await ObterClaimsPorUsuarioId(id);
            var claimsSelec = new List<ClaimSelecionadaViewModel>();
            
            if(claimsAspNet != null)
            {
                claimsSelec = await ConverterClaimsPAraViewModel(claimsAspNet.OrderBy(x => x.Type.ToUpper()).ToList());
            }
           
            usuarioClaim.ClaimSelecionadas = claimsSelec;
            return CustomResponse(usuarioClaim);
        }
          
        [Produces("application/json")]
        [HttpGet("obter-claim-por-usuario/{id}")]
        public async Task<ActionResult> ObterClaimPorUsuario(string id)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var appUserAgilium = await _userManager.FindByIdAsync(id);
            if (appUserAgilium == null)
            {
                NotificarErro("Usuário Não localizado, procure o administrador do sistema!");

                var ret = CustomResponse($@"Id do usuario: {id} ");
                return ret;
            }

            var userClaimsExistentes = await _userManager.GetClaimsAsync(appUserAgilium);

            var claismConvertidas = await ConverterClaimsPAraViewModel(userClaimsExistentes.OrderBy(x => x.Type.ToUpper()).ToList());
            //if (!claismConvertidas.Any())
            //{
            //    NotificarErro("Não foram localizados claims para o usuario informado!");
            //    var ret = CustomResponse($@"Id do usuario: {id} ");
            //    return ret;
            //}
            return CustomResponse(claismConvertidas);
        }

        [Produces("application/json")]
        [HttpPost("remover-claim-por-usuario/{id}")]
        public async Task<ActionResult> RemoverClaimDeUsuarioPorId(string id, [FromBody] ClaimAcaoIndividualPorUsuario viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var userAspNet = await ObterUsuarioAspNetIdentity(id);
            if(userAspNet == null)
            {
                return CustomResponse();
            }

            var claims = await ObterClaimsPorUsuarioId(id);

            var claimsRemove = claims.Where(claim => claim.Type.ToUpper() == viewModel.ClaimType.ToUpper() 
                                                    && claim.Value.ToUpper() == viewModel.ClaimValue.ToUpper());

            await _userManager.RemoveClaimsAsync(userAspNet, claimsRemove);

            return CustomResponse(true);
        }

        #endregion

        #region metodos auxiliares
        private async Task<LoginResponseViewModel> GerarJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var refreshToken = await GerarRefreshToken(email);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            claims.Add(new Claim("refreshToken", refreshToken.Token.ToString()));
            claims.Add(new Claim("nome", user.Nome));
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);


            var response = new LoginResponseViewModel
            {
                AccessToken = encodedToken,
                RefreshToken = refreshToken.Token,
                ExpiresIn = TimeSpan.FromMinutes(_appSettings.ExpiracaoHoras).TotalSeconds,
                UserToken = new UserTokenViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Nome = user.Nome,
                    Claims = claims.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value })
                }
            };

            return response;
        }

        private bool AdicionarUsuario(RegisterUserViewModel usuarioNovo, string idUserAspNet)
        {
            var usuarioExistente = _usuarioService.ObterPorUsuarioAspNetPorId(idUserAspNet).Result;

            if (usuarioExistente == null)
            {
                var ativo = (int)EAtivo.Ativo;
                //var usuario = new Usuario(usuarioNovo.Nome, ativo.ToString(), idUserAspNet);
                var usuario = new Usuario(usuarioNovo.Nome, usuarioNovo.CPF, usuarioNovo.Usuario,usuarioNovo.Email, ativo.ToString(), idUserAspNet);
                return _usuarioService.Adicionar(usuario).Result;
            }

            return true;
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private async Task<RefreshToken> GerarRefreshToken(string email)
        {
            var refreshToken = new RefreshToken
            {
                Username = email,
                ExpirationDate = DateTime.UtcNow.AddHours(_appTokenSettingsSettings.RefreshTokenExpiration)
            };

            _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(u => u.Username == email));
            await _context.RefreshTokens.AddAsync(refreshToken);

            await _context.SaveChangesAsync();

            return refreshToken;
        }

        private async Task<RefreshToken> ObterRefreshToken(Guid refreshToken)
        {
            var token = await _context.RefreshTokens.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Token == refreshToken);

            return token != null && token.ExpirationDate.ToLocalTime() > DateTime.Now
                ? token
                : null;
        }

        private async Task<bool> NotificarNovoUsuarioCriado(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                NotificarErro("Usuário Não localizado, procure o administrador do sistema!");
                return false;
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action("RedefinirSenha", "Identidade", new { userId = user.Id, code = code, email = user.Email }, protocol: HttpContext.Request.Scheme, host: "localhost:44360");

            await _emailSender.SendEmailAsync(
                email,"Alterar a senha - Agilium Manager",
                @$"Foi criado novo usuario com acesso ao sistema Agilium Manager para este email. Por favor, redefina a senha clicando <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'> aqui</a>.");

            return true;
        }


        private async Task<bool> AtualizarClaimsUsuario(AppUserAgilium user, List<Claim> claims)
        {
            var result = await _userManager.AddClaimsAsync(user, claims);
            if (!result.Succeeded)
            {
                NotificarErro("Não foi possivel adicionar a permissão ao usuário");
                return false;
            }
            return true;
        }

        private async Task<bool> RemoverClaimsUsuario(AppUserAgilium user)
        {
            // Get all the user existing claims and delete them
            var oldClaims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, oldClaims);
                      
            if (!result.Succeeded)
            {
                NotificarErro("Não foi possivel remover a permissão ao usuário");
                return false;
            }
            return true;
        }

        private async Task<bool> AtualizarClaimsUsuario(AppUserAgilium appUserAgilium, string claimType, string claimValue)
        {
            var claim = new Claim(claimType, claimValue);
            var result = await _userManager.AddClaimAsync(appUserAgilium, claim);
            if (!result.Succeeded)
            {
                NotificarErro("Não foi possivel adicionar a permissão ao usuário");
                return false;
            }
            return true;
        }

        private async Task<bool> ClaimEmUso(string claimType)
        {
            var users =  User.Claims.Where(x => x.Type == claimType);
            return users.Any();
            
        }


        private async Task<List<ClaimSelecionadaViewModel>> ConverterClaimsPAraViewModel(List<Claim> claims)
        {
            var claimsConvertidas = new List<ClaimSelecionadaViewModel>();

            var ultimaClaimType = claims.Any() ? claims.FirstOrDefault().Type : string.Empty;


            var listaClaimsTypes = (from clm in claims
                                    group new { clm.Type } by clm.Type.ToUpper() into grupoClaim
                                    orderby grupoClaim.Key
                                    select grupoClaim).ToList();

            listaClaimsTypes.ForEach(type => {
                var claimConvertida = new ClaimSelecionadaViewModel();
                claimConvertida.claim = type.Key;
                var listaSelecionadas = claims.Where(x => type.Key.ToUpper() == x.Type.ToUpper());
                var listaAcoesString = string.Empty;
                listaSelecionadas.ToList().ForEach(x =>
                    listaAcoesString = x.Value.ToUpper()
                );
                var lista2 = listaSelecionadas.Select(x => x.Value);
                foreach (var item in lista2)
                {
                    var lst = item.Split(',');
                    lst.ToList().ForEach(x => claimConvertida.ClaimValue.Add(x));
                }
                claimsConvertidas.Add(claimConvertida);
            });


            return claimsConvertidas;
        }

        private async Task<AppUserAgilium> ObterUsuarioAspNetIdentity(string id)
        {
            var appUserAgilium = await _userManager.FindByIdAsync(id);
            if (appUserAgilium == null)
            {
                NotificarErro("Usuário destino para realizar a copia, não foi localizado, procure o administrador do sistema!");
                NotificarErro($@"Id do usuario: {id} ");
            }

            return appUserAgilium;

        }

        private async Task<IList<Claim>> ObterClaimsPorUsuarioId(string id)
        {
            var usuarioAspNet = await ObterUsuarioAspNetIdentity(id);
            if (usuarioAspNet == null)
                return null;
            return await _userManager.GetClaimsAsync(usuarioAspNet);
        }

        private async Task<ClaimSelecionadaViewModel> ConverterClaimEmClaimSelecionada(Claim claim)
        {
            var claimSelec = new ClaimSelecionadaViewModel();
            claimSelec.claim = claim.Type;
            claimSelec.ClaimValue.Add(claim.Value);

            return claimSelec;
        }

        #endregion
    }
}
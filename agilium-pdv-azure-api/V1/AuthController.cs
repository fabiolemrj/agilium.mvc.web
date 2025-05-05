using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.pdv.Configuration;
using agilium.api.pdv.Controllers;
using agilium.api.pdv.Extension;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using System;
using static agilium.api.pdv.ViewModels.UserViewModel;
using agilium.api.business.Enums;
using agilium.api.pdv.ViewModels;
using Microsoft.AspNetCore.Http;

namespace agilium.api.pdv.V1
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
        private readonly IUsuarioFotoEntityService _usuarioFotoServiceService;


        public AuthController(INotificador notificador, IUser appUser, IConfiguration configuration, IUtilDapperRepository utilDapperRepository, ILogService logService,
              IUsuarioService usuarioService,
                                IOptions<AppSettings> appSettings,
                                IOptions<AppTokenSettings> appTokenSettingsSettings,
                                ApplicationDbContext context,
                                 IEmailSender emailSender,
                                 IUserClaimsManagerService userClaimsManagerService,
                                 IMapper mapper, ILogger<AuthController> logger,
                                             SignInManager<AppUserAgilium> signInManager,
                                UserManager<AppUserAgilium> userManager, 
                                IUsuarioFotoEntityService usuarioFotoServiceService) : base(notificador, appUser, configuration, utilDapperRepository, logService)
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
            _usuarioFotoServiceService = usuarioFotoServiceService;
        }

        #region Autenticacao

        [HttpGet("obter-foto")]
        public async Task<ActionResult> ObterUsuarioFoto([FromQuery] string idusuario)
        {
            var usuarioFoto = await ObterFotoConvertidaUsuario(idusuario.Trim());
            return CustomResponse(usuarioFoto.Imagem);
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
                    var usuarioFoto = await ObterFotoConvertidaUsuario(appUserAgilium.Id);
                    if(usuarioFoto != null)
                    {
                        resp.Foto = await ConverterByteToBase64(usuarioFoto.Imagem);
                        resp.Arquivo = usuarioFoto.Imagem;
                    }

                    await _logService.Adicionar(loginUser.Email, "Login realizado", "Auth", "Login", "Web", null, null);
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
        public async Task<ActionResult> LoginRefreshToken([FromBody] RefreshTokenUser refreshToken)
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

            await _signInManager.SignInAsync(appUserAgilium, false);

            if (appUserAgilium.Ativo == (int)EAtivo.Ativo)
            {
                var resp = await GerarJwt(token.Username);

                return CustomResponse(resp);
            }

            NotificarErro("Não foi possível fazer login com usuario");
            ObterNotificacoes("Auth", "LoginRegreshToken", "Web");

            return CustomResponse(Forbid());

        }
        
        [HttpPost("mudar-senha")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Authorize]
        public async Task<ActionResult> MudarSenha([FromBody] UserChangePassword userChangePassword)
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
                ObterNotificacoes("Auth", "MudarSenha", "WebPDV");
                return CustomResponse($@"Email: {user.Email} ");
            }

            await _signInManager.RefreshSignInAsync(user);

            var msg = "Senha alterada com sucesso!";
            var resposta = new RespPadrao()
            {
                objeto = msg
            };
            LogInformacao($"{msg}", "Auth", "MudarSenha", "WebPDV");
            return CustomResponse(resposta);
        }

        [HttpPost("esqueci-senha")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<ActionResult> EsqueciSenha([FromBody] UserForgotPassword userForgotPassword)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            
            var user = await _userManager.FindByEmailAsync(userForgotPassword.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                NotificarErro("Usuário Não localizado, procure o administrador do sistema!");
                ObterNotificacoes("Auth", "EsqueciSenha", "WebPDV");
                return CustomResponse($@"Email: {userForgotPassword.Email} ");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            //var callbackUrl = Url.Page("/api/v1/esqueci-senha",
            //    pageHandler: null,
            //    values: new { userId = user.Id, code = code },
            //    protocol: Request.Scheme);

            //var callbackUrl = Url.Action("RedefinirSenha", null, new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme, host: urlFrontBase);
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
            LogInformacao($"{msg}", "Auth", "EsqueciSenha", "Web");
            return CustomResponse(resposta);
        }

        [HttpPost("esqueci-senhaApp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<ActionResult> EsqueciSenhaApp([FromBody] UserForgotPassword userForgotPassword)
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
        public async Task<ActionResult> RedefinirSenha([FromBody] UserResetPassword userReset)
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
        

        #endregion
        #region metodos auxiliares

        private async Task<UsuarioFotoEntity> ObterFotoConvertidaUsuario(string codigo)
        {
            return await _usuarioFotoServiceService.ObterPorUsuarioFotoPorId(codigo);
            //if(usuarioFoto !=null && usuarioFoto.Imagem.Length > 0)
            //{
            //    return await ConverterByteToBase64(usuarioFoto.Imagem);
            //}

            //return string.Empty;
        }

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
                var usuario = new Usuario(usuarioNovo.Nome, usuarioNovo.CPF, usuarioNovo.Usuario, usuarioNovo.Email, ativo.ToString(), idUserAspNet);
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
                email, "Alterar a senha - Agilium Manager",
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
            var users = User.Claims.Where(x => x.Type == claimType);
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

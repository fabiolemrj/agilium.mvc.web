using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        protected readonly IAuthenticationService _authenticationService;
        public AutenticacaoService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) 
            : base(httpClient, settings, user, authenticationService, configuration)
        {
            _authenticationService = authenticationService;
        }

        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
        {
            var loginContent = ObterConteudo(usuarioLogin);

            var response = await _httpClient.PostAsync("/api/v1/entrar", loginContent);

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }

        public async Task<RespPadrao> MudarSenha(UserChangePassword userChange)
        {
            var registroContent = ObterConteudo(userChange);
            var response = await _httpClient.PostAsync("/api/v1/mudar-senha", registroContent);

            if (!TratarErrosResponse(response))
            {
                return new RespPadrao
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<RespPadrao>(response);
        }

        //private async Task RealizarLogin(UsuarioRespostaLogin resposta)
        //{
        //    var token = ObterTokenFormatado(resposta.AccessToken);

        //    var claims = new List<Claim>();
        //    claims.Add(new Claim("JWT", resposta.AccessToken));
        //    claims.AddRange(token.Claims);

        //    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //    var authProperties = new AuthenticationProperties
        //    {
        //        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
        //        IsPersistent = true
        //    };

        //    await _authenticationService.SignInAsync(
        //     _user.ObterHttpContext(),
        //     CookieAuthenticationDefaults.AuthenticationScheme,
        //     new ClaimsPrincipal(claimsIdentity),
        //     authProperties);

        //    //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
        //    //                                new ClaimsPrincipal(claimsIdentity),
        //    //                                authProperties);
        //}

        public async Task RealizarLogin(UsuarioRespostaLogin resposta)
        {
            var token = ObterTokenFormatado(resposta.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", resposta.AccessToken));
            claims.Add(new Claim("RefreshToken", resposta.RefreshToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                IsPersistent = true
            };
            try
            {
                await _authenticationService.SignInAsync(
                _user.ObterHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            }
            catch 
            {
                await _authenticationService.SignOutAsync(
                _user.ObterHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                null);
            }
        }

        public async Task<UsuarioRespostaLogin> Registro(RegisterModel usuarioRegistro)
        {
            var registroContent = ObterConteudo(usuarioRegistro);

            var response = await _httpClient.PostAsync("/api/v1/nova-conta", registroContent);

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }

        private static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }

  
        public async Task<RespPadrao> EsqueciSenha(UserForgotPassword userForgotPassword)
        {
            var registroContent = ObterConteudo(userForgotPassword);
            var response = await _httpClient.PostAsync("/api/v1/esqueci-senha", registroContent);

            if (!TratarErrosResponse(response))
            {
                return new RespPadrao
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<RespPadrao>(response);
        }

        public async Task<RespPadrao> RedefinirSenha(UserResetPassword userResetPassword)
        {
            var registroContent = ObterConteudo(userResetPassword);
            var response = await _httpClient.PostAsync("/api/v1/redefinir-senha", registroContent);

            if (!TratarErrosResponse(response))
            {
                return new RespPadrao
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<RespPadrao>(response);
        }

        public async Task<UsuarioRespostaLogin> UtilizarRefreshToken(string refreshToken)
        {
            var refreshTokenContent = ObterConteudo(refreshToken);

            var response = await _httpClient.PostAsync("/api/v1/refresh-token", refreshTokenContent);

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }

        public bool TokenExpirado()
        {
            var jwt = _user.ObterUserToken();
            if (string.IsNullOrEmpty(jwt)) return false;

            var token = ObterTokenFormatado(jwt);
            return token.ValidTo.ToLocalTime() < DateTime.Now;
        }

        public async Task<bool> RefreshTokenValido()
        {
            var resposta = await UtilizarRefreshToken(_user.ObterUserRefreshToken());

            if (resposta.AccessToken != null && resposta.ResponseResult == null)
            {
                await RealizarLogin(resposta);
                return true;
            }

            return false;
        }

        public async Task Logout()
        {
            await _authenticationService.SignOutAsync(
                _user.ObterHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                null);
        }

        public async Task<List<UserClaimViewModel>> ObterClaims()
        {
            var response = await _httpClient.GetAsync($"/api/v1/claims");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<UserClaimViewModel>>(response);
        }

        public async Task<ResponseResult> NovaClaim(UserClaimViewModel usuarioRegistro)
        {
            var registroContent = ObterConteudo(usuarioRegistro);

            var response = await _httpClient.PostAsync("/api/v1/adicionar-claim", registroContent);

            if (!TratarErrosResponse(response)) 
                return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<List<string>> ObterListaClaims()
        {
            var response = await _httpClient.GetAsync($"/api/v1/claims");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<string>>(response);
        }

        public async Task<List<string>> ObterListaClaimValue()
        {
            var response = await _httpClient.GetAsync($"/api/v1/claims-values");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<List<string>>(response);
        }
    }
}

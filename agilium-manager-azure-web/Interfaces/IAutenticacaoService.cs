using agilium.webapp.manager.mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin);
        Task<UsuarioRespostaLogin> Registro(RegisterModel usuarioRegistro);
        Task<RespPadrao> MudarSenha(UserChangePassword userChange);
        Task RealizarLogin(UsuarioRespostaLogin resposta);
        Task<RespPadrao> EsqueciSenha(UserForgotPassword userForgotPassword);
        Task<RespPadrao> RedefinirSenha(UserResetPassword userResetPassword);
        bool TokenExpirado();
        Task<bool> RefreshTokenValido();
        Task Logout();
        Task<List<UserClaimViewModel>> ObterClaims();
        Task<List<string>> ObterListaClaims();
        Task<List<string>> ObterListaClaimValue();
        Task<ResponseResult> NovaClaim(UserClaimViewModel usuarioRegistro);
    }
}

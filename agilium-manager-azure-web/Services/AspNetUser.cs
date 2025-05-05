using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public class AspNetUser : IAspNetUser
    {
        private readonly IHttpContextAccessor _accessor;
      //  private readonly IUsuarioService _usuarioService;

        public AspNetUser(IHttpContextAccessor accessor
        //    ,IUsuarioService usuarioService
            )
        {
            _accessor = accessor;
        //    _usuarioService = usuarioService;
        }

        public string Name => _accessor.HttpContext.User.Identity.Name;

        public string ObterUserNome()
        {
            return EstaAutenticado() ? _accessor.HttpContext.User.GetUserName() : string.Empty;
        }

        public string ObterUserId()
        {
            return EstaAutenticado() ? _accessor.HttpContext.User.GetUserId():string.Empty;
        }

        public string ObterUserEmail()
        {
            return EstaAutenticado() ? _accessor.HttpContext.User.GetUserEmail() : "";
        }

        public string ObterUserToken()
        {
            return EstaAutenticado() ? _accessor.HttpContext.User.GetUserToken() : "";
        }

        public bool EstaAutenticado()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool PossuiRole(string role)
        {
            return _accessor.HttpContext.User.IsInRole(role);
        }

        public IEnumerable<Claim> ObterClaims()
        {
            return _accessor.HttpContext.User.Claims;
        }

        public HttpContext ObterHttpContext()
        {
            return _accessor.HttpContext;
        }

        public string ObterUserRefreshToken()
        {
            return EstaAutenticado() ? _accessor.HttpContext.User.GetUserRefreshToken() : "";
        }

        public bool EhAtivo()
        {
            throw new NotImplementedException();
        }

    }
}

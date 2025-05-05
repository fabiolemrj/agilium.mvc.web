using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IAspNetUser
    {
        string Name { get; }
        string ObterUserId();
        string ObterUserEmail();
        string ObterUserToken();
        string ObterUserRefreshToken();
        string ObterUserNome();
        bool EstaAutenticado();
        bool PossuiRole(string role);
        IEnumerable<Claim> ObterClaims();
        HttpContext ObterHttpContext();
        bool EhAtivo();
     
    }
}

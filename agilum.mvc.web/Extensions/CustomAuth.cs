using agilium.api.business.Interfaces.IService;
using agilum.mvc.web.ViewModels;
using KissLog.RestClient.Requests.CreateRequestLog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Polly;
using System.Linq;
using System.Security.Claims;

namespace agilum.mvc.web.Extensions
{
    public class CustomAuthorization
    {
    

        public CustomAuthorization()
        {
       
        }

        public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
        {
       
            return true;
        }

        public static bool ValidarUsuario (ICaService caService, string idUsuario, int idTag)
        {
            return caService.UsuarioTemPermissao(idUsuario, idTag).Result;
        }

    }

    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(int idTag) : base(typeof(RequisitoClaimFilter))
        {
            Arguments = new object[] { idTag };
        }
    }

    public class RequisitoClaimFilter : IAuthorizationFilter
    {
        private readonly int _idTag;
        private readonly ICaService _caService;

        public RequisitoClaimFilter(int idTag, ICaService caService)
        {
            _idTag = idTag;
            _caService = caService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "Identity", page = "/Account/Login", ReturnUrl = context.HttpContext.Request.Path.ToString() }));
                return;
            }

            var id = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!CustomAuthorization.ValidarUsuario(_caService,id,_idTag))
            {
                context.Result = Error(context, 403);
            }
        }

        public IActionResult Error(AuthorizationFilterContext context, int statusCode)
        {          

            return new RedirectToRouteResult(new RouteValueDictionary(new {controller ="Home", action = $"Error",id=statusCode}));
        }
    }
}

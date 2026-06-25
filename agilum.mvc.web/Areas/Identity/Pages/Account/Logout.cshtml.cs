using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using agilium.api.infra.Context;
using agilium.api.business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace agilum.mvc.web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<CaUsuarioIdentity> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<CaUsuarioIdentity> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(string returnUrl = null)
        {
            HttpContext.Session.Remove("_empSelec");

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            HttpContext.Session.Remove("_empSelec");

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
       
        }
    }
}

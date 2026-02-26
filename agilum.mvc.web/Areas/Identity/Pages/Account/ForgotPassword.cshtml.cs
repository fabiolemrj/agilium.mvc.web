using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

using agilium.api.infra.Context;
using agilum.mvc.web.Data;
using static agilum.mvc.web.Areas.Identity.Pages.Account.LoginModel;
using agilium.api.business.Interfaces.IService;
using System.Linq;
using AutoMapper;

namespace agilum.mvc.web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<AppUserAgiliumIdentity> _userManager;
        private readonly agilum.mvc.web.Services.IEmailSender _emailSender;
        private readonly IEmpresaService _empresaService;
        protected readonly IMapper _mapper;
        private IEnumerable<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();

        public ForgotPasswordModel(UserManager<AppUserAgiliumIdentity> userManager, agilum.mvc.web.Services.IEmailSender emailSender,
            IEmpresaService empresaService, IMapper mapper)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _empresaService = empresaService;
            _mapper = mapper;

            if (listaEmpresaViewModels.Count() == 0)
                listaEmpresaViewModels = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);

            ObterEmpresas();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = "Campo {0} obrigatório")]
            public string Empresa { get; set; }
        }

        private async void ObterEmpresas()
        {
            Empresas = listaEmpresaViewModels.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Alterar a senha",
                    $"Redefina sua senha em < a href='{HtmlEncoder.Default.Encode(callbackUrl)}'> clicando aqui</a>.",
                    Input.Empresa);

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}

using System;
using agilium.api.business.Models;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium_manager_azure_business.Interfaces.IService;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace agilum.mvc.web.Areas.Identity.Pages.Account
{
    public class LoginEmpresaModel : PageModel
    {
        private readonly UserManager<CaUsuarioIdentity> _userManager;
        private readonly SignInManager<CaUsuarioIdentity> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmpresaService _empresaService;
        private readonly IMapper _mapper;
        private readonly ILicencaService _licencaService;

        public LoginEmpresaModel(SignInManager<CaUsuarioIdentity> signInManager,
            ILogger<LoginModel> logger,
            UserManager<CaUsuarioIdentity> userManager,
            IEmpresaService empresaService,
            IMapper mapper,
            ILicencaService licencaService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _empresaService = empresaService;
            _mapper = mapper;
            _licencaService = licencaService;

            if (!listaEmpresaViewModels.Any())
                listaEmpresaViewModels = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);
        }

        private IEnumerable<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();

        public class EmpresaViewModel
        {
            public long Id { get; set; }
            public string NMRZSOCIAL { get; set; }
        }

        public class EmpresaUsuarioViewModel
        {
            public string IDEMPRESA { get; set; }
            public string NomeEmpresa { get; set; }
            public string IDUSUARIO { get; set; }
        }

        public class InputModel
        {
            [Required(ErrorMessage = "Campo {0} obrigatório")]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = "Campo {0} obrigatório")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            [Required(ErrorMessage = "Selecione uma empresa para continuar")]
            public string Empresa { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ObterEmpresas();
            ReturnUrl = returnUrl;
        }

        private void ObterEmpresas()
        {
            Empresas = listaEmpresaViewModels.ToList();
        }

        private async Task<bool> ValidarEmpresa()
        {
            return (!string.IsNullOrEmpty(Input.Empresa) && Convert.ToInt64(Input.Empresa) > 0);
        }

        private async Task GravarEmpresa(string idempresa, string email)
        {
            var empresa = await _empresaService.ObterPorId(Convert.ToInt64(idempresa));
            var user = await _userManager.FindByEmailAsync(email);
            var empresaSelecionada = new EmpresaUsuarioViewModel()
            {
                IDEMPRESA = empresa.Id.ToString(),
                IDUSUARIO = user?.UserName ?? email,
                NomeEmpresa = empresa.NMRZSOCIAL
            };
            HttpContext.Session.SetString("_empSelec", System.Text.Json.JsonSerializer.Serialize(empresaSelecionada));
        }

        private async Task AdicionarClaim(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var customClaims = new[] { new Claim("id", user.Id) };
                var res = await _userManager.AddClaimsAsync(user, customClaims);
                if (!res.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Erro ao tentar criar claim");
                }
            }
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // Valida se uma empresa foi selecionada antes de autenticar
                if (!await ValidarEmpresa())
                {
                    ObterEmpresas();
                    ModelState.AddModelError(string.Empty, "Selecione uma empresa para continuar");
                    return Page();
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    await GravarEmpresa(Input.Empresa, Input.Email);
                    await AdicionarClaim(Input.Email);
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    ObterEmpresas();
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    ObterEmpresas();
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ObterEmpresas();
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            ObterEmpresas();
            return Page();
        }
    }
}

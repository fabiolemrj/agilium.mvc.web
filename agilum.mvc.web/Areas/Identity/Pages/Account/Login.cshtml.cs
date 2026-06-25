using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using agilium.api.infra.Context;
using agilium.api.business.Models;
using KissLog.RestClient.Requests.CreateRequestLog;
using System.Security.Claims;
using agilium.api.business.Interfaces.IService;
using Microsoft.AspNetCore.Mvc.Rendering;
using agilium.api.business.Enums;
using AutoMapper;
using agilium.api.business.Models;
using Microsoft.AspNetCore.Http;
using agilium_manager_azure_business.Interfaces.IService;

namespace agilum.mvc.web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<CaUsuarioIdentity> _userManager;
        private readonly SignInManager<CaUsuarioIdentity> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmpresaService _empresaService;
        protected readonly IMapper _mapper;
        protected readonly ILicencaService _licencaService;

        public LoginModel(SignInManager<CaUsuarioIdentity> signInManager,
            ILogger<LoginModel> logger,
            UserManager<CaUsuarioIdentity> userManager,
            IEmpresaService empresaService, IMapper mapper, ILicencaService licencaService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _empresaService = empresaService;
            _mapper = mapper;
            _licencaService = licencaService;

            if (listaEmpresaViewModels.Count() == 0)
                listaEmpresaViewModels = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);
        }

        private IEnumerable<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [EmailAddress]
            [Required(ErrorMessage = "Campo {0} obrigat�rio")]
            public string Email { get; set; }

            [Required(ErrorMessage ="Campo {0} obrigat�rio")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            [Required(ErrorMessage = "Campo {0} obrigat�rio")]
            public string Empresa { get; set; }
            
        }

        public class EmpresaUsuarioViewModel
        {
            public string IDEMPRESA { get; set; }
            public string NomeEmpresa { get; set; }
            public string IDUSUARIO { get; set; }
        }

        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();

        public class EmpresaViewModel
        {
            public long Id { get; set; }          
            public string NUCNPJ { get; set; }         
            public long IDENDERECO { get; set; }            
            public string CDEMPRESA { get; set; }         
            public string NMRZSOCIAL { get; set; }           
            public string NMFANTASIA { get; set; }           
            public string DSINSCREST { get; set; }            
            public string DSINSCRESTVINC { get; set; }            
            public string DSINSCRMUN { get; set; }           
            public string NMDISTRIBUIDORA { get; set; }            
            public string NUREGJUNTACOM { get; set; }           
            public decimal? NUCAPARM { get; set; } = 0;            
            public ESimNao? STMICROEMPRESA { get; set; }            
            public ESimNao? STLUCROPRESUMIDO { get; set; }           
            public ETipoEmpresa? TPEMPRESA { get; set; }           
            public ECodigoRegimeTributario CRT { get; set; }           
            public string IDCSC { get; set; }         
            public string CSC { get; set; }            
            public string NUCNAE { get; set; }            
            public string IDCSC_HOMOL { get; set; }            
            public string CSC_HOMOL { get; set; }          
            public string IDLOJA_SITEMARCADO { get; set; }            
            public string CLIENTID_SITEMERCADO { get; set; }           
            public string CLIENTSECRET_SITEMERCADO { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            ObterEmpresas();
            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        private async Task AdicionarClaim(string email)
        {
           // var user = await _userManager.FindByEmailAsync(email);


            //if(user!=null)
            //{
            //    var customClaims = new[] { new Claim("id", user.Id) };
            //    var res = await _userManager.AddClaimsAsync(user, customClaims);
            //    if (!res.Succeeded)
            //    {
            //        ModelState.AddModelError(string.Empty, "Erro ao tentar ccriar claim");
            //    }
            //}
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
                IDUSUARIO = user.UserName,
                NomeEmpresa = empresa.NMRZSOCIAL
            };
            HttpContext.Session.SetString("_empSelec", System.Text.Json.JsonSerializer.Serialize(empresaSelecionada));
        }
        private async void ObterEmpresas()
        {
            Empresas = listaEmpresaViewModels.ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                if (!await ValidarEmpresa())
                {
                    ObterEmpresas();
                    ModelState.AddModelError(string.Empty, "Selecione uma empresa");
                    return Page();
                }

               
                    //if (!_licencaService.DataValida(Convert.ToInt64(Input.Empresa)).Result)
                    //{
                    //    ObterEmpresas();

                    //    var mensagem = $"Licen�a da empresa selecionada est� vencida ou inv�lida";
                    //    TempData["TipoMensagem"] = "danger";
                    //    TempData["Mensagem"] = mensagem;

                    //    return Page();
                    //}
                

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
            else
            {
                ObterEmpresas();
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

       
    }

   
}

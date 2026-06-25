using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

using agilium.api.business.Models;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Enums;
using AutoMapper;
using agilum.mvc.web.Services;

namespace agilum.mvc.web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmpresaService _empresaService;
        protected readonly IMapper _mapper;

        public LoginModel(
            IAuthService authService,
            ILogger<LoginModel> logger,
            IEmpresaService empresaService,
            IMapper mapper)
        {
            _authService = authService;
            _logger = logger;
            _empresaService = empresaService;
            _mapper = mapper;

            if (listaEmpresaViewModels.Count() == 0)
                listaEmpresaViewModels = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);
        }

        private IEnumerable<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            //[EmailAddress]
            [Required(ErrorMessage = "Campo {0} obrigatório")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Campo {0} obrigatório")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            [Required(ErrorMessage = "Campo {0} obrigatório")]
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
            ReturnUrl = returnUrl;
        }

        private async Task<bool> ValidarEmpresa()
        {
            return (!string.IsNullOrEmpty(Input.Empresa) && Convert.ToInt64(Input.Empresa) > 0);
        }

        private async Task GravarEmpresa(string idempresa, string login)
        {
            var empresa = await _empresaService.ObterPorId(Convert.ToInt64(idempresa));
            var empresaSelecionada = new EmpresaUsuarioViewModel()
            {
                IDEMPRESA = empresa.Id.ToString(),
                IDUSUARIO = login,
                NomeEmpresa = empresa.NMRZSOCIAL
            };
            HttpContext.Session.SetString("_empSelec", System.Text.Json.JsonSerializer.Serialize(empresaSelecionada));
            await HttpContext.Session.CommitAsync();
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

                var usuario = await _authService.ValidarLogin(Input.Email, Input.Password);

                if (usuario != null)
                {
                    var empresa = await _empresaService.ObterPorId(Convert.ToInt64(Input.Empresa));
                    // SignIn com empresa nos claims (não depende de sessão)
                    await _authService.SignInAsync(HttpContext, usuario, Input.RememberMe,
                        empresa.Id.ToString(), empresa.NMRZSOCIAL);
                    // Também grava na sessão para compatibilidade
                    await GravarEmpresa(Input.Empresa, usuario.usuario);
                    _logger.LogInformation("User logged in.");
                    // Redireciona para Home/Index (area vazia = raiz, não Identity)
                    if (string.IsNullOrEmpty(returnUrl) || returnUrl == "/" || returnUrl == "~/")
                        return RedirectToAction("Index", "Home", new { area = "" });
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ObterEmpresas();
                    ModelState.AddModelError(string.Empty, "Login ou senha inválidos.");
                    return Page();
                }
            }

            ObterEmpresas();
            return Page();
        }
    }
}

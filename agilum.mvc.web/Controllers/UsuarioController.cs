using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilum.mvc.web.Interfaces;
using agilum.mvc.web.ViewModels.Usuarios;
using agilum.mvc.web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using agilium.api.business.Models;
using agilium.api.infra.Repository;
using System.Linq;
using agilium.api.business.Services;
using System.Collections.Generic;
using agilium.api.business.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using agilum.mvc.web.Data;
using KissLog.RestClient.Requests.CreateRequestLog;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using agilum.mvc.web.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using agilum.mvc.web.Extensions;

namespace agilum.mvc.web.Controllers
{
    [Authorize]
    [Route("usuario")]
    public class UsuarioController : MainController
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAutenticacaoService _autenticacaoService;
        private readonly ICaService _controleAcessoService;
        private readonly UserManager<AppUserAgiliumIdentity> _userManager;
        private readonly IEmailSender _emailSender;
        private IEnumerable<CaPerfilManagerViewModel> ListaPerfis { get; set; } = new List<CaPerfilManagerViewModel>();

        #region construtor
        public UsuarioController(INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository,
            ILogService logService, IMapper mapper, IUsuarioService usuarioService, IAutenticacaoService autenticacaoService, ICaService controleAcessoService,
            UserManager<AppUserAgiliumIdentity> userManager, IEmailSender emailSender) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper)
        {
            _usuarioService = usuarioService;
            _autenticacaoService = autenticacaoService;
            _controleAcessoService = controleAcessoService;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        #endregion

        #region usuarios

        [Route("lista")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(1000)]
        public async Task<IActionResult> Index([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
        {

            var lista = await ObterListaPaginado(q, page, ps);
            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "lista";
            lista.ReferenceController = "usuario";
            return View(lista);
        }

        [HttpGet]
        [Route("criar-novo-usuario")]
        [ClaimsAuthorizeAttribute(1002)]
        public async Task<IActionResult> CreateNovoUsuarioWeb(long idUsuario)
        {
            var usuario = await _usuarioService.ObterPorUsuarioPorId(idUsuario);

            var msgErro = "";
            var sucesso = false;
            if (usuario != null)
            {
                if (string.IsNullOrEmpty(usuario.email))
                {
                    msgErro = "Campo de email obrigatório para criar novo usuario web";
                    return Json(new { sucesso = sucesso, erro = msgErro });
                }
            }
            else
            {
                msgErro = "Erro ao tentar localizar usuario";
                return Json(new { sucesso = sucesso, erro = msgErro });
            }

            var novaSenhaTemporaria = "Agilium_123";

            var userNewWeb = new AppUserAgiliumIdentity { UserName = usuario.email, Email = usuario.email, Nome = usuario.nome};
            var result = await _userManager.CreateAsync(userNewWeb, novaSenhaTemporaria);

            if (result.Succeeded)
            {
                var returnUrl = Url.Content("~/");
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(userNewWeb);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmarEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userNewWeb.Id, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);
                try
                {
                    await _emailSender.SendEmailAsync(userNewWeb.Email, "Agilium Manager",
                       $"<h3>Confirme seu email</h3> Por favor, confirme sua conta para acesso ao sistema Agilium Manager Web <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clique aqui</a>.");
                }
                catch (Exception ex)
                {

                    msgErro = ex.Message;
                }
                
            }
            else
            {
              
                result.Errors.ToList().ForEach (item =>
                {
                    msgErro += item.Description + "\r\n";
                }) ;
            }

            if(result.Succeeded)
            {
                msgErro = "Usuario criado com sucesso";
                sucesso = true;
            }               
            
            return Json(new { sucesso = sucesso, erro = msgErro });
        }

        [HttpGet]
        [Route("editar")]
        [ClaimsAuthorizeAttribute(1004)]
        public async Task<IActionResult> Edit(string id)
        {
            ObterEstados();

            var model = _mapper.Map<UserFull>(await _usuarioService.ObterPorUsuarioPorId(Convert.ToInt64(id)));
            model.UsuarioPossuiAcessoWeb = (!await _controleAcessoService.UsuarioPossuiAcessoWeb(id));
            MontarViewBagListas();

            return View("CreateEdit", model);
        }

        [HttpPost]
        [Route("editar")]
        [ClaimsAuthorizeAttribute(1004)]
        public async Task<IActionResult> Edit(UserFull viewModel)
        {
            if (!ModelState.IsValid)
            {
                MontarViewBagListas();
                return View("CreateEdit", viewModel);
            }

            if (!string.IsNullOrEmpty(viewModel.cep)) viewModel.cep = viewModel.cep.Replace(".", "").Replace("-", "");

            await _usuarioService.AtualizarSemSalvar(_mapper.Map<Usuario>(viewModel));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Usuario", "Atualizar", "Web", Deserializar(viewModel)));
                TempData["TipoMensagem"] = "danger";
                TempData["Mensagem"] = msgErro;
                return View("CreateEdit", viewModel);
            }
            await _usuarioService.Salvar();

            TempData["TipoMensagem"] = "Success";
            TempData["Mensagem"] = "Operação realizada com Sucesso.";
            return RedirectToAction("Index");
        }

        #endregion

        #region metodos privados
        private async Task<PagedViewModel<UserFull>> ObterListaPaginado(string filtro, int page, int pageSize)
        {
            var retorno = await _usuarioService.ObterUsuariosPorNomePuro(filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<UserFull>>(retorno.List);
            foreach (var item in lista)
            {
                item.UsuarioPossuiAcessoWeb = (await _controleAcessoService.UsuarioPossuiAcessoWeb(item.id));
            }

            return new agilum.mvc.web.ViewModels.PagedViewModel<UserFull>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "lista",
                ReferenceController = "usuario",
                TotalResults = retorno.TotalResults
            };
        }

        private void ObterEstados()
        {

            List<Estado> estados = new List<Estado>();
            estados.Add(new Estado() { Sigla = "RJ", Nome = "Rio de Janeiro" });
            estados.Add(new Estado() { Sigla = "MG", Nome = "Minas Gerais" });
            estados.Add(new Estado() { Sigla = "SP", Nome = "São Paulo" });
            estados.Add(new Estado() { Sigla = "AC", Nome = "Acre" });
            estados.Add(new Estado() { Sigla = "AL", Nome = "Alagoas" });
            estados.Add(new Estado() { Sigla = "AP", Nome = "Amapá" });
            estados.Add(new Estado() { Sigla = "AM", Nome = "Amazonas" });
            estados.Add(new Estado() { Sigla = "BA", Nome = "Bahia" });
            estados.Add(new Estado() { Sigla = "CE", Nome = "Ceará" });
            estados.Add(new Estado() { Sigla = "DF", Nome = "Distrito Federal" });
            estados.Add(new Estado() { Sigla = "ES", Nome = "Espírito Santo" });
            estados.Add(new Estado() { Sigla = "GO", Nome = "Goiás" });
            estados.Add(new Estado() { Sigla = "MA", Nome = "Maranhão" });
            estados.Add(new Estado() { Sigla = "RS", Nome = "Rio Grande do Sul" });
            estados.Add(new Estado() { Sigla = "SC", Nome = "Santa Catarina" });
            estados.Add(new Estado() { Sigla = "PR", Nome = "Parana" });
            estados.Add(new Estado() { Sigla = "MT", Nome = "Mato Grosso" });
            estados.Add(new Estado() { Sigla = "MS", Nome = "Mato Grosso do Sul" });
            estados.Add(new Estado() { Sigla = "RR", Nome = "Roraima" });
            estados.Add(new Estado() { Sigla = "RD", Nome = "Rondonia" });
            estados.Add(new Estado() { Sigla = "TO", Nome = "Tocantis" });
            estados.Add(new Estado() { Sigla = "PA", Nome = "Pará" });
            estados.Add(new Estado() { Sigla = "RN", Nome = "Rio Grande do Norte" });
            estados.Add(new Estado() { Sigla = "RS", Nome = "Paraíba" });
            estados.Add(new Estado() { Sigla = "PI", Nome = "Piauí" });
            estados.Add(new Estado() { Sigla = "SE", Nome = "Sergipe" });

            ViewBag.estados = new SelectList(estados, "Sigla", "Nome", "");

        }

        private void MontarViewBagListas()
        {
            if (ListaPerfis == null || !ListaPerfis.Any())
            {
                ListaPerfis = _mapper.Map< List < CaPerfilManagerViewModel >>( _controleAcessoService.ObterTodosCaPerfilManager().Result.ToList());
            }
            ViewBag.Perfis = new SelectList(ListaPerfis, "IdPerfil", "Descricao");
        }


        #endregion
    }
}

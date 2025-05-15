using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Services;
using agilum.mvc.web.Extensions;
using agilum.mvc.web.Services;
using agilum.mvc.web.ViewModels.Contato;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.EmpresaUsuario;
using agilum.mvc.web.ViewModels.Usuarios;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Authorize]
    [Route("empresa")]
    public class EmpresaController : MainController
    {
        private readonly IEmpresaService _empresaService;
        private readonly IContatoService _contatoService;
        private readonly ILogger<EmpresaController> _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly ICaService _caService;
        private readonly string _nomeEntidade = "Empresa";

        public EmpresaController(IEmpresaService empresaService, IUsuarioService usuarioService, ILogger<EmpresaController> logger,
            IContatoService contatoService,INotificador notificador, IConfiguration configuration, IUser appUser, 
            IUtilDapperRepository utilDapperRepository, ILogService logService, IMapper mapper, ICaService caService) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper)
        {
            _empresaService = empresaService;
            _logger = logger;
            _contatoService = contatoService;
            _usuarioService = usuarioService;
            _caService = caService;
        }

        #region endpoint

        #region empresa
        [Route("lista")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2001)]
        public async Task<IActionResult> Index([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
        {

            var lista = await ObterListaPaginado(q, page, ps);
            ViewBag.Pesquisa = q;
            return View(lista);
        }


        [Route("nova")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2002)]
        public async Task<IActionResult> Create()
        {
            ObterEstados();
            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            var model = new EmpresaCreateViewModel();
            model.TPEMPRESA = agilium.api.business.Enums.ETipoEmpresa.Loja;
            model.STLUCROPRESUMIDO = agilium.api.business.Enums.ESimNao.Sim;
            model.STMICROEMPRESA = agilium.api.business.Enums.ESimNao.Nao;
            model.Endereco.Id = 0;

            return View(model);
        }

        [Route("nova")]
        [HttpPost]
        public async Task<IActionResult> Create(EmpresaCreateViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            if (!ModelState.IsValid) return View(model);

            model.NUCNPJ = RetirarPontos(model.NUCNPJ);

            if (model.Endereco != null && !string.IsNullOrEmpty(model.Endereco.Cep))
                model.Endereco.Cep = RetirarPontos(model.Endereco.Cep);

            var empresa = _mapper.Map<Empresa>(model);
            if (empresa.Endereco.Id == 0)
                empresa.Endereco.Id = await GerarId();

            if (empresa.Id == 0) empresa.Id = await GerarId(); 
            
            await _empresaService.Adicionar(empresa);
            await _empresaService.Salvar();

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Empresa", "Adicionar", "Web", Deserializar(empresa)));
                var retornoErro = new { mensagem = "Erro ao criar nova empresa" };
                _logger.LogError(retornoErro.ToString());
                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("editar")]
        [ClaimsAuthorizeAttribute(2005)]
        public async Task<IActionResult> Edit(long id)
        {
            ObterEstados();
            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";
            var objeto = _mapper.Map<EmpresaCreateViewModel>(await _empresaService.ObterCompletoPorId(id));
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidade} não localizada";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View("Create", objeto);
        }

        [HttpPost]
        [Route("editar")]
        public async Task<IActionResult> Edit(EmpresaCreateViewModel model)
        {
            ObterEstados();
            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            if (!ModelState.IsValid) return View("Create", model);

            model.NUCNPJ = RetirarPontos(model.NUCNPJ);

            if (model.Endereco != null && !string.IsNullOrEmpty(model.Endereco.Cep))
                model.Endereco.Cep = RetirarPontos(model.Endereco.Cep);
            
            var empresa = _mapper.Map<Empresa>(model);
            await _empresaService.Atualizar(empresa);
            await _empresaService.Salvar();
            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao editar nova {_nomeEntidade}" };

                _logger.LogError(retornoErro.ToString());
                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("apagar")]
        [ClaimsAuthorizeAttribute(2003)]
        public async Task<IActionResult> Delete(long id)
        {
            var objeto = _mapper.Map<EmpresaCreateViewModel>(await _empresaService.ObterPorId(id));
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidade} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View(objeto);
        }

        [HttpPost]
        [Route("apagar")]
        public async Task<IActionResult> Delete(EmpresaCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _empresaService.Apagar(model.Id);
            await _empresaService.Salvar();
            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao editar nova {_nomeEntidade}" };

                _logger.LogError(retornoErro.ToString());
                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Contato
        [HttpGet]
        [Route("contato/novo")]
        public async Task<IActionResult> AdicionarContato(long idEmpresa)
        {
            ViewBag.acao = "AdicionarContato";
            ViewBag.operacao = "I";

            var model = new ContatoEmpresaViewModel();
            model.IDEMPRESA = idEmpresa;

            return PartialView("_createContato", model);
        }

        [HttpGet]
        [Route("contato/editar")]
        public async Task<IActionResult> EditarContato(long idEmpresa, long idContato)
        {
            ViewBag.acao = "EditarContato";
            ViewBag.operacao = "E";

            var contatoEmpresa = await _contatoService.ObterPorId(idContato, idEmpresa);
            if (contatoEmpresa == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<ContatoEmpresaViewModel>(contatoEmpresa);

            return PartialView("_createContato", model);
        }
        [HttpGet]
        [Route("contato/apagar")]
        public async Task<IActionResult> DeleteContato(long idContato, long idEmpresa)
        {
            var contatoEmpresa = await _contatoService.ObterPorId(idContato, idEmpresa);
            if (contatoEmpresa == null)
            {
                var msg = "Erro ao tentar remover endereço contato!";
                return Json(new { erro = msg });
            }

            var url = Url.Action("ObterEndereco", "Empresa", new { id = idEmpresa });
            await _contatoService.Apagar(idContato, idEmpresa);
            await _contatoService.Salvar();
            if (!OperacaoValida())
            {
                AdicionarErroValidacao("Erro ao tentar remover endereço contato!");
                var msgErro = string.Join("\n\r", ModelState.Values
                              .SelectMany(x => x.Errors)
                              .Select(x => x.ErrorMessage));

                return Json(new { erro = msgErro });
            }

            return Json(new { success = true, url });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("contato/editar")]
        public async Task<IActionResult> EditarContato(ContatoEmpresaViewModel model)
        {
            ViewBag.acao = "EditarContato";
            ViewBag.operacao = "E";

            if (!ModelState.IsValid) return PartialView("_createContato", model);
            if (model.Contato.Id == 0)
                model.Contato.Id = model.IDCONTATO;

            await _contatoService.Atualizar(_mapper.Map<ContatoEmpresa>(model));
            await _contatoService.Salvar();
            if (!OperacaoValida()) return PartialView("_createContato", model);

            var url = Url.Action("ObterEndereco", "Empresa", new { id = model.IDEMPRESA });


            return Json(new { success = true, url });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("contato/novo")]
        public async Task<IActionResult> AdicionarContato(ContatoEmpresaViewModel model)
        {
            ViewBag.acao = "AdicionarContato";
            ViewBag.operacao = "I";

            if (!ModelState.IsValid) return PartialView("_createContato", model);

            var contatoEmpresa = _mapper.Map<ContatoEmpresa>(model);

            if (contatoEmpresa.IDCONTATO == 0)
                contatoEmpresa.PopularContato(await GerarId());

            if (contatoEmpresa.Contato.Id == 0)
                contatoEmpresa.Contato.Id = contatoEmpresa.IDCONTATO;

            await _contatoService.Adicionar(contatoEmpresa);
            await _contatoService.Salvar();

            if (!OperacaoValida()) return PartialView("_createContato", model);

            var url = Url.Action("ObterEndereco", "Empresa", new { id = model.IDEMPRESA });


            return Json(new { success = true, url });
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("contato")]
        public async Task<IActionResult> ObterEndereco(long id)
        {
            var empresa = _mapper.Map<EmpresaCreateViewModel>(await _empresaService.ObterCompletoPorId(id));

            if (empresa == null)
            {
                return NotFound();
            }

            return PartialView("_contato", empresa);
        }
        #endregion

        #region selecionar empresa modal
        [HttpPost]       
        public async Task<ActionResult> SelecionarEmpresa(string idEmpresaSelecionada)
        {
            var _id = "";
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("_idSelecEmp")))
            {
                if (idEmpresaSelecionada != "-1")
                    HttpContext.Session.SetString("_idSelecEmp", idEmpresaSelecionada);

                _id = idEmpresaSelecionada;
            }
            else
            {
                _id = HttpContext.Session.GetString("_idSelecEmp");
                if (_id != idEmpresaSelecionada)
                {
                    HttpContext.Session.SetString("_idSelecEmp", idEmpresaSelecionada);
                    _id = idEmpresaSelecionada;
                }

            }
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        [Route("obter-empresas-por-usuario")]
        public async Task<ActionResult> ObterEmpresasUsuarioJson()
        {
            var idUserAspNet = AppUser.GetUserId().ToString();
            var listaUsuarioEmpresa = new List<EmpresaUsuarioViewModel>();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("listaEmpresaUsuario")))
            {

                //var listaEmpresaUsuario =

                var usuario = (await _usuarioService.ObterPorUsuarioAspNetPorId(idUserAspNet));
                if (usuario == null)
                {
                    var msgErro = "Erro ao tentar obter empresas qu eo usuario tem autorização";
                    AdicionarErroValidacao(msgErro);
                    msgErro = string.Join("\n\r", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));

                    var retornoErro = new { mensagem = msgErro };

                    return new JsonResult(new { error = true, msg = msgErro });

                }

                listaUsuarioEmpresa = _mapper.Map<List<EmpresaUsuarioViewModel>>(_usuarioService.ObterEmpresasPorUsuario(usuario.Id).Result);
                var listaConvertida = JsonConvert.SerializeObject(listaUsuarioEmpresa);
                HttpContext.Session.SetString("listaEmpresaUsuario", listaConvertida);
            }
            else
            {
                var lista = HttpContext.Session.GetString("listaEmpresaUsuario");
                listaUsuarioEmpresa = JsonConvert.DeserializeObject<List<EmpresaUsuarioViewModel>>(lista);
            }

            var idEmpresaSelecionado = "";
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("idEmpresaSelecionado")))
            {
                idEmpresaSelecionado = HttpContext.Session.GetString("idEmpresaSelecionado");
            }
            return PartialView("_SelecionarEmpresa");
            //return new JsonResult(new { lista = (List<EmpresaUsuarioViewModel>)listaUsuarioEmpresa, idEmpresaSelecionada = idEmpresaSelecionado });
        }
        [HttpPost]
        [Route("SelecionarEmpresa")]
        public async Task<ActionResult> SelecionarEmpresa(ListaEmpresasSelecao model)
        {
            if (!ModelState.IsValid) return View(model);

            if (string.IsNullOrEmpty(model.IDEMPRESA))
            {
                var retornoErro = new { mensagem = "Erro ao tentar salvar empresae selecionada" };
                _logger.LogError(retornoErro.ToString());
                AdicionarErroValidacao(retornoErro.mensagem);
                return RedirectToAction("Index", "Home");
            }

            var empresa = await _empresaService.ObterPorId(Convert.ToInt64(model.IDEMPRESA));
            var empresaSelecionada = new EmpresaUsuarioViewModel()
            {
                IDEMPRESA = empresa.Id.ToString(),
                IDUSUARIO = AppUser.GetUserId().ToString(),
                NomeEmpresa = empresa.NMRZSOCIAL
            };
            HttpContext.Session.SetString("_empSelec", System.Text.Json.JsonSerializer.Serialize(empresaSelecionada)) ;

            return RedirectToAction("Index", "Home");

        }

        [Route("ObterListasEmpresasPorUsuario")]
        [HttpGet]
        public async Task<ActionResult> ObterListasEmpresasPorUsuario()
        {
            var idUserAspNet = AppUser.GetUserId().ToString();
            var usuario = (await _usuarioService.ObterPorUsuarioAspNetPorId(idUserAspNet));
            if(usuario == null)
            {
               var retornoErro = new { mensagem = "Erro ao tentar obter empresas do usuario" };
                _logger.LogError(retornoErro.ToString());
                AdicionarErroValidacao(retornoErro.mensagem);
                return RedirectToAction("index","Home");
            }
            var listaUsuarioEmpresa = new ListaEmpresasSelecao();
            _caService.ObterEmpresasAssociadasPorUsuario(idUserAspNet).Result.ToList().ForEach(item =>
            {
                listaUsuarioEmpresa.EmpresasDisponiveisAssociacao.Add(_mapper.Map<EmpresaViewModel>(item));
            });

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_empSelec")))
            {
                var empresaSelecionada = System.Text.Json.JsonSerializer.Deserialize<EmpresaUsuarioViewModel>(HttpContext.Session.GetString("_empSelec"));
                listaUsuarioEmpresa.IDEMPRESA = empresaSelecionada.IDEMPRESA;
            }
            return View("_SelecionarEmpresa",listaUsuarioEmpresa);


        }

        [Route("ObterEmpresaSelecionada")]
        public async Task<ActionResult> ObterEmpresaSelecionada()
        {
            //var empresaSelecionada = System.Text.Json.JsonSerializer.Deserialize<EmpresaUsuarioViewModel>( ObterStringEmpresaSelecionada());
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();
            return Json(empresaSelecionada?.NomeEmpresa);
        }

        #endregion

        #endregion

        #region metodos privados
        private async Task<agilum.mvc.web.ViewModels.PagedViewModel<EmpresaViewModel>> ObterListaPaginado(string filtro, int page, int pageSize)
        {
            var retorno = await _empresaService.ObterPorDescricaoPaginacao(filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<EmpresaViewModel>>(retorno.List);

            return new agilum.mvc.web.ViewModels.PagedViewModel<EmpresaViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "Index",
                TotalResults = retorno.TotalResults
            };
        }

        private void ObterEstados()
        {
            var estados = ListasAuxilares.ObterEstados();
            ViewBag.estados = new SelectList(estados, "Sigla", "Nome", "");
        }
        #endregion
    }
}

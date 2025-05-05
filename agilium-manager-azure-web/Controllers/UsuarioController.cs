
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.Models;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.CaManagerViewModel;
using agilium.webapp.manager.mvc.ViewModels.ControleAcesso;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    public class UsuarioController : MainController
    {

        private readonly IUsuarioService _usuarioService;
        private readonly IAspNetUser _aspNetUser;
        private readonly IAutenticacaoService _autenticacaoService;
        private readonly ILogger<UsuarioController> _logger;
        private readonly IControleAcessoService _controleAcessoService;

        private IEnumerable<CaPerfilManagerViewModel> ListaPerfis { get; set; }= new List<CaPerfilManagerViewModel>();


        public UsuarioController(IUsuarioService usuarioService,
            IAutenticacaoService autenticacaoService,
             IAspNetUser aspNetUser,
             IControleAcessoService controleAcessoService,
             ILogger<UsuarioController> logger)
        {
            _usuarioService = usuarioService;
            _aspNetUser = aspNetUser;
            _autenticacaoService = autenticacaoService;
            _logger = logger;
            _controleAcessoService = controleAcessoService;

            if(ListaPerfis == null || !ListaPerfis.Any())
            {
                ListaPerfis = _controleAcessoService.ObterTodosPerfilManager().Result.ToList();
            }

        }

        [HttpGet]
        public async Task<IActionResult> ObterUsuario(string idUserAspNet)
        {
            ObterEstados();

            var usuario = await _usuarioService.ObterPorUsuarioPorUserId(idUserAspNet);
            MontarViewBagListas();

            return View(usuario);
        }

        private void MontarViewBagListas()
        {
            if (ListaPerfis == null || !ListaPerfis.Any())
            {
                ListaPerfis = _controleAcessoService.ObterTodosPerfilManager().Result.ToList();
            }
            ViewBag.Perfis = new SelectList(ListaPerfis, "IdPerfil", "Descricao");
        }

        [HttpPost]
        public async Task<IActionResult> ObterUsuario(string id, UserFull viewModel)
        {
            if (!ModelState.IsValid)
            {
                MontarViewBagListas();
                return View(viewModel); }
            if (!string.IsNullOrEmpty(viewModel.cep)) viewModel.cep = viewModel.cep.Replace(".", "").Replace("-", "");
         
            var response = await _usuarioService.Atualizar(id, viewModel);
            if (ResponsePossuiErros(response))
            {
                MontarViewBagListas();
                var msgErro = string.Join("\n\r", ModelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));
                AdicionarErroValidacao(msgErro);

                TempData["TipoMensagem"] = "danger";
                TempData["Mensagem"] = msgErro;

                return View(viewModel);
            }

            TempData["TipoMensagem"] = "Success";
            TempData["Mensagem"] = "Operação realizada com Sucesso.";
            return RedirectToAction("ObterTodosUsuarios");           
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodosUsuarios()
        {
            var viewModel = new ListaUsuarioViewModel();

            viewModel.Usuarios = await ObterUsuarios(null);

            return View(viewModel);
        }

        [Route("usuarios")]
        [HttpGet]
        public async Task<IActionResult> ObterTodosUsuarios([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
         
            var lista = await ObterUsuarios(q, page,ps);
            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "ObterTodosUsuarios";
            return View(lista);
        }

        [HttpPost]
        public async Task<IActionResult> ObterTodosUsuarios(ListaUsuarioViewModel viewModel)
        {
          
            if (!ModelState.IsValid) return View(viewModel);
            viewModel.Usuarios = await ObterUsuarios(viewModel.Filtro);

            return View(viewModel);
        }

        #region Claims
        [HttpGet]
        public async Task<ActionResult> AssociarUsuarioClaim(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                AdicionarErroValidacao("Id não associado");
                return RedirectToAction("ObterTodosUsuarios");
            }

            var viewModel = new AssociarUsuarioClaims();
            var usuario = await _usuarioService.ObterPorUsuarioPorId(id);
            
            if(usuario == null)
            {
                AdicionarErroValidacao("Erro ao tentar exibir usuario selecionado");
                return RedirectToAction("ObterTodosUsuarios");
            }
            await ObterClaims();
            viewModel.id = usuario.id;
            viewModel.idUserAspNet = usuario.idUserAspNet;
            viewModel.Nome = usuario.nome;
            //viewModel.Acoes = await _autenticacaoService.ObterListaClaimValue();
            var lista = await ObterAcoesSelecionadas(await _autenticacaoService.ObterListaClaimValue());
            viewModel.AcoesClaims = lista;

            if (TempData["ClaimSelecionada"] != null)
                TempData["ClaimSelecionada"] = null;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> ListaClaimUsuario(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                AdicionarErroValidacao("Id não associado");
                return RedirectToAction("ObterTodosUsuarios");
            }
            var usuario = await _usuarioService.ObterPorUsuarioPorId(id);

            var viewModel = new ClaimsPorUsuarioViewModel();
            var responseData = await _usuarioService.ObterClaimsPorUsuario(usuario.idUserAspNet);
            
            if (ResponsePossuiErros(responseData))
                return RedirectToAction("ObterTodosUsuarios");
          
            viewModel.ClaimsSelecionadas = (List<ClaimSelecionada>)responseData.Data;
            viewModel.idUserAspNet = usuario.idUserAspNet;
            viewModel.Nome = usuario.nome;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> ListaClaimUsuarioCopia(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                AdicionarErroValidacao("Id não associado");
                return View("ObterTodosUsuarios");
            }
            var usuario = await _usuarioService.ObterPorUsuarioPorId(id);
            var viewModel = new DuplicarUsuarioClaimViewModel();
            viewModel.idUserAspNet = usuario.idUserAspNet;
            viewModel.Nome = usuario.nome;

            await ObterUsuariosClaims();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> ListaUsuariosClaims(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                AdicionarErroValidacao("Id não associado");
                return View("ObterTodosUsuarios");
            }
            var responseData = await _usuarioService.ObterUsuarioComClaimsPorId(id);
            if(responseData.Status >= 400)
            {
                AdicionarErroValidacao("Erro ao tentar obter lista de claims do usuario");
            }

            if (!OperacaoValida())
            {
                TempData["TipoMensagem"] = "danger";
                TempData["Mensagem"] = "Erro ao tentar obter lista de claims do usuario";

                return RedirectToAction("ObterTodosUsuarios");           
            }
               

            var listaAcoesClaims = await ObterAcoesSelecionadas(await _autenticacaoService.ObterListaClaimValue());
            
            var usuario = (UsuarioClaimsViewModel)responseData.Data;
            
            var viewModel = new DuplicarUsuarioClaimViewModel();
            viewModel.id= usuario.Usuario.id;
            viewModel.idUserAspNet = usuario.Usuario.idUserAspNet;
            viewModel.Nome = usuario.Usuario.nome;
            viewModel.ClaimsAdicionadas = usuario.ClaimSelecionadas;
            viewModel.AcoesClaims = listaAcoesClaims;

            await ObterClaims();
            //await ObterUsuariosClaims();
            await ObterUsuariosClaimsRemovendoUsuarioSelecionado(usuario.Usuario.id);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ListaClaimUsuarioCopia(DuplicarUsuarioClaimViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var viewModelDuplicar = new DuplicarUsuarioRetornoViewModel();
            
            viewModelDuplicar.idOrigem = viewModel.idUserSelecionado;
            viewModelDuplicar.idDestino = viewModel.idUserAspNet;
            
            var responseData = await _usuarioService.DuplicarClaimPorUsuario(viewModelDuplicar);

            TempData["TipoMensagem"] = "Success";
            TempData["Mensagem"] = "Operação realizada com Sucesso.";

            return RedirectToAction("ObterTodosUsuarios");
        }

        [HttpPost]
        public async Task<ActionResult> ListaClaimUsuarioCopiaRetorno(string objetosAdd)
        {
            var usuarioRetorno = JsonConvert.DeserializeObject<DuplicarUsuarioRetornoViewModel>(objetosAdd);
            if(string.IsNullOrEmpty(usuarioRetorno.idDestino) || string.IsNullOrEmpty(usuarioRetorno.idDestino))
            {
                var msgErro = "Usuario Destino e/ou Base não selecionados";
                AdicionarErroValidacao(msgErro);
                
                return new JsonResult(new { error = true, msg = msgErro });
            }

            var responseData = await _usuarioService.ObterClaimsPorUsuario(usuarioRetorno.idOrigem);
            
            if (ResponsePossuiErros(responseData) )
            {
                var msgErro = "Erro ao tentar atualizar claims ado usuario";
                AdicionarErroValidacao(msgErro);
                msgErro = string.Join("\n\r", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

                return new JsonResult(new { error = true, msg = msgErro });
            }

            var lista = (List<ClaimSelecionada>)responseData.Data;

            return new JsonResult(new { lista });
        }

        public async Task<List<ClaimSelecionada>> ObterFormClaim(List<ClaimSelecionada> claimSelecionadas)
        {

            return claimSelecionadas;
        }

        [HttpPost]
        public async Task<ActionResult> AdicionarAcao(string objetosAdd)
        {
            var lista = new List<ClaimSelecionada>();
            if (TempData["lista"] != null)
                lista = JsonConvert.DeserializeObject<List<ClaimSelecionada>>( TempData["lista"].ToString());

            var objeto = JsonConvert.DeserializeObject<AcoesSelecionadas>(objetosAdd);
          

            var claimSelecionada = new ClaimSelecionada();
            claimSelecionada.claim = objeto.Claim;
            objeto.AcoesClaims.ForEach(acao => {
                if(acao.Selecao)
                    claimSelecionada.AdicionarClaimType(acao.Acao);
            });

            if(lista.Any(x=>x.claim.Trim().ToUpper() == claimSelecionada.claim.Trim().ToUpper()))
            {
                TempData["lista"] = JsonConvert.SerializeObject(lista);
                TempData.Keep("lista");

                return new JsonResult(new { error = true, lista  });
            }
            
            lista.Add(claimSelecionada);

            TempData["lista"] = JsonConvert.SerializeObject(lista);
            TempData.Keep("lista");
            
            return new JsonResult(new { lista}) ;
            
        }

        [HttpPost]
        public async Task<ActionResult> RemoverAcao(string objetosAdd)
        {
            var lista = new List<ClaimSelecionada>();
            if (TempData["lista"] != null)
                lista = JsonConvert.DeserializeObject<List<ClaimSelecionada>>(TempData["lista"].ToString());

            var listaClaimSelecionadas = JsonConvert.DeserializeObject<List<string>>(objetosAdd);

            listaClaimSelecionadas.ForEach(claimSelec => {
                var claim = lista.FirstOrDefault(x => x.claim.Trim().ToUpper() == claimSelec.Trim().ToUpper());
                if (claim != null) lista.Remove(claim);
            });

            TempData["lista"] = JsonConvert.SerializeObject(lista);
            TempData.Keep("lista");
            return new JsonResult(new { lista });
            
        }

        public async Task<ActionResult> SalvarClaim(string idUser)
        {
            var lista = new List<ClaimSelecionada>();
            if (TempData["lista"] != null)
                lista = JsonConvert.DeserializeObject<List<ClaimSelecionada>>(TempData["lista"].ToString());

            if (!lista.Any())
            {
                TempData["lista"] = JsonConvert.SerializeObject(lista);
                TempData.Keep("lista");
                var msgErro = "Erro: Não existem claims selecionadas";
                return new JsonResult(new { error = true, msg = msgErro, lista });
            }


            var result = await _usuarioService.AtualizarClaimUsuario(idUser, lista);
            if(result.Status >= 400)
            {
                var msgErro = "Erro ao tentar atualizar Claims";
                TempData["lista"] = JsonConvert.SerializeObject(lista);
                TempData.Keep("lista");
                AdicionarErroValidacao(msgErro);

                return new JsonResult(new { error = true, msg = msgErro,lista });
            }

            return new JsonResult("/Usuario/ObterTodosUsuarios");
        }

        public async Task<ActionResult> RemoverClaimIndividualPorUsuario(string objetosAdd)
        {
            //TODO - analisar futuramente viabilidade de unificar as requisições deste metodos

            var objeto = JsonConvert.DeserializeObject<ClaimEditaAcaoIndividualPorUsuario>(objetosAdd);            
            var result = await _usuarioService.RemoverClaimInvidualPorUsuario(objeto);
            var responseData = await _usuarioService.ObterClaimsPorUsuario(objeto.IdUserAspNet);

            if (ResponsePossuiErros(responseData) || ResponsePossuiErros(result))
            {
                var msgErro = "Erro ao tentar atualizar claims ado usuario";
                AdicionarErroValidacao(msgErro);
                msgErro = string.Join("\n\r", ModelState.Values
                                     .SelectMany(x => x.Errors)
                                     .Select(x => x.ErrorMessage));

                var retornoErro = new { mensagem = msgErro, objeto = objetosAdd, usuarioLogado = _aspNetUser.ObterUserEmail() };
                _logger.LogError(retornoErro.ToString());

                return new JsonResult(new { error = true, msg = msgErro });
            }

            var retorno = new { mensagem = "Remover Claim Individual com Sucesso", objeto = objetosAdd, usuarioLogado = _aspNetUser.ObterUserEmail() };
            _logger.LogInformation(retorno.ToString());

            //return new JsonResult("/Usuario/ListaUsuariosClaims");
            return new JsonResult(new { lista = (List<ClaimSelecionada>)responseData.Data });
           
        }

        public async Task<ActionResult> AdicionarClaimIndividualPorUsuario(string objetosAdd)
        {
            var objeto = JsonConvert.DeserializeObject<ClaimAcoesUsuario>(objetosAdd);
            var lista = new List<ClaimSelecionada>();
            var claimSelec = new ClaimSelecionada();
            claimSelec.claim = objeto.ClaimType;
            claimSelec.ClaimValue = objeto.ClaimValue;
            lista.Add(claimSelec); 

            var result = await _usuarioService.AtualizarClaimUsuario(objeto.IdUserAspNet, lista);
            var responseData = await _usuarioService.ObterClaimsPorUsuario(objeto.IdUserAspNet);
            if (ResponsePossuiErros(responseData) || ResponsePossuiErros(result))
            {
                var msgErro = "Erro ao tentar adicionar nova Claim";
                AdicionarErroValidacao(msgErro);
                msgErro = string.Join("\n\r", ModelState.Values
                                     .SelectMany(x => x.Errors)
                                     .Select(x => x.ErrorMessage));

                var retornoErro = new { mensagem = msgErro, objeto = objetosAdd,usuarioLogado = _aspNetUser.ObterUserEmail() };
                _logger.LogError(retornoErro.ToString());

                return new JsonResult(new { error = true, msg = msgErro});
            }

            var retorno = new { mensagem = "Adicionar Claim Individual com Sucesso", objeto = objetosAdd, usuarioLogado = _aspNetUser.ObterUserEmail() };
            _logger.LogInformation(retorno.ToString());

            return new JsonResult(new { lista = (List<ClaimSelecionada>)responseData.Data });

        }

        public async Task<ActionResult> AtualizarListaClaimUsuarioAtual(string id)
        {
            var responseData = await _usuarioService.ObterClaimsPorUsuario(id);
            
            if (ResponsePossuiErros(responseData))
            {
                var msgErro = "Erro ao tentar atualizar claims ado usuario";
                AdicionarErroValidacao(msgErro);
                msgErro = string.Join("\n\r", ModelState.Values
                                     .SelectMany(x => x.Errors)
                                     .Select(x => x.ErrorMessage));

                var retornoErro = new { mensagem = msgErro, idUsuario = id, usuarioLogado = _aspNetUser.ObterUserEmail() };
                _logger.LogError(retornoErro.ToString());

                return new JsonResult(new { error = true, msg = msgErro });
            }

            var lista = (List<ClaimSelecionada>)responseData.Data;

            return new JsonResult(new { lista });
        }

        public async Task<ActionResult> DuplicarClaimsPorUsuarioBase(string objetosAdd)
        {
            var objeto = JsonConvert.DeserializeObject<DuplicarUsuarioRetornoViewModel>(objetosAdd);
            
            //TODO - avaliar necessidade de unificar requisicoes
            var responseDataDuplicar = await _usuarioService.DuplicarClaimPorUsuario(objeto);
            var responseData = await _usuarioService.ObterClaimsPorUsuario(objeto.idDestino);
            if (ResponsePossuiErros(responseData) || ResponsePossuiErros(responseDataDuplicar))
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage)); 
                AdicionarErroValidacao(msgErro);

                return new JsonResult(new { error = true, msg = msgErro, usuarioLogado = _aspNetUser.ObterUserEmail() });
            }

            var lista = (List<ClaimSelecionada>)responseData.Data;

            var retorno = new { mensagem = "Claims duplicadas com sucesso com Sucesso", objeto = objetosAdd, usuarioLogado = _aspNetUser.ObterUserEmail() };
            _logger.LogInformation(retorno.ToString());

            return new JsonResult(new { lista });
        }
        #endregion

        public async Task<ActionResult> MudarSituacaoUsuario(string id, string ativo)
        {
            var responseData = ativo == "S" ? await _usuarioService.DesativarUsuario(id) : await _usuarioService.AtivarUsuario(id);
            if (ResponsePossuiErros(responseData))
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                AdicionarErroValidacao(msgErro);

                return new JsonResult(new { error = true, msg = msgErro });
            };
            var msg = responseData.Data.ToString();
            TempData["TipoMensagem"] = "Success";
            TempData["Mensagem"] = responseData.Data.ToString();

            return new JsonResult(new { erro = false, msg = msg});
        }

        public async Task<ActionResult> ExibirImagemUsuario(string idUserAspNet)
        {

            var responseData = await _usuarioService.ObterFotoUsuarioPorId(idUserAspNet);
            if (ResponsePossuiErrosStatusCode(responseData))
            {
                var msgErro = "Erro de requisição";
                AdicionarErroValidacao(msgErro);

                TempData["TipoMensagem"] = "danger";
                TempData["Mensagem"] = msgErro;

                return RedirectToAction("Index", "Home");
            }
            if (ResponsePossuiErros(responseData))
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                AdicionarErroValidacao(msgErro);

                TempData["TipoMensagem"] = "danger";
                TempData["Mensagem"] = msgErro;

                return new JsonResult(new { error = true, msg = msgErro });
            }

            var usuario = (UsuarioFotoViewModel)responseData.Data;
            return View(usuario);
        }

        public async Task<ActionResult> ObterEmpresasUsuarioJson(string idUserAspNet)
        {
            var listaUsuarioEmpresa = new List<EmpresaUsuarioViewModel>();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("listaEmpresaUsuario")))
            {
                var usuario = _usuarioService.ObterPorUsuarioPorUserId(idUserAspNet).Result;
                if (usuario == null)
                {
                    var msgErro = "Erro ao tentar obter empresas qu eo usuario tem autorização";
                    AdicionarErroValidacao(msgErro);
                    msgErro = string.Join("\n\r", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));

                    var retornoErro = new { mensagem = msgErro };

                    return new JsonResult(new { error = true, msg = msgErro });

                    //HttpContext.Session.SetString("fotousuario", usuario.ImagemConvertida);
                }

                listaUsuarioEmpresa = (List<EmpresaUsuarioViewModel>) _usuarioService.ObterEmpresasPorUsuario(Convert.ToInt64(usuario.id)).Result.Data;
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
            
            return new JsonResult(new { lista = (List<EmpresaUsuarioViewModel>)listaUsuarioEmpresa, idEmpresaSelecionada = idEmpresaSelecionado });
        }


        public async Task<ActionResult> SelecionarEmpresa(string idEmpresaSelecionada)
        {
            var _id = "";
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("idEmpresaSelecionado")))
            {
                if (idEmpresaSelecionada != "-1")
                    HttpContext.Session.SetString("idEmpresaSelecionado", idEmpresaSelecionada);

                _id = idEmpresaSelecionada;
            }
            else
            {
                _id = HttpContext.Session.GetString("idEmpresaSelecionado");
                if(_id != idEmpresaSelecionada)
                {
                    HttpContext.Session.SetString("idEmpresaSelecionado", idEmpresaSelecionada);
                    _id = idEmpresaSelecionada;
                }
                    
            }
            return new JsonResult(new { idSelecionado = _id});
        }

        public async Task<ActionResult> ExibirImagemUsuarioJson(string idUserAspNet)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("fotousuario")))
            {
                var responseData = await _usuarioService.ObterFotoUsuarioPorId(idUserAspNet);
                if (ResponsePossuiErrosStatusCode(responseData))
                {
                    var msgErro = "Erro de requisição";
                    AdicionarErroValidacao(msgErro);

                    TempData["TipoMensagem"] = "danger";
                    TempData["Mensagem"] = msgErro;

                    return new JsonResult(new { error = true, msg = msgErro });
                }

                if (ResponsePossuiErros(responseData))
                {
                    var msgErro = string.Join("\n\r", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    AdicionarErroValidacao(msgErro);

                    TempData["TipoMensagem"] = "danger";
                    TempData["Mensagem"] = msgErro;

                    return new JsonResult(new { error = true, msg = msgErro });
                }
                var usuario = (UsuarioFotoViewModel)responseData.Data;
                if (usuario == null || string.IsNullOrEmpty(usuario.ImagemConvertida))
                {
                    var msgErro = "imagem não encontrada";
                    return new JsonResult(new { error = true, msg = msgErro });
                }
                HttpContext.Session.SetString("fotousuario", usuario.ImagemConvertida);
            }

            return new JsonResult(new { imagem = HttpContext.Session.GetString("fotousuario") });
            //return new JsonResult(new { imagem = "" });
        }

        [HttpPost]
        public async Task<ActionResult> ExibirImagemUsuario(UsuarioFotoViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);
            if(viewModel.ImagemUpLoad.Length > 1048576)
            {
                var msgErro = "A imagem selecionada deve possuir tamanho de até 1 MB ";
               AdicionarErroValidacao(msgErro);

                TempData["TipoMensagem"] = "danger";
                TempData["Mensagem"] = msgErro;

                return View(viewModel);
            }
               
            var novoUsuarioFotoViewModel = new UsuarioFotoViewModel();
            novoUsuarioFotoViewModel.id = viewModel.id;
            novoUsuarioFotoViewModel.idAspNetUser = viewModel.idAspNetUser;
            novoUsuarioFotoViewModel.ImagemConvertida = viewModel.ImagemConvertida.Replace("data:image/png;base64,","")
                                                                                    .Replace("data:image/jpeg;base64,", "")
                                                                                    .Replace("data:image/jpg;base64,", "")
                                                                                    .Replace("data:image/bmp;base64,", "");
            novoUsuarioFotoViewModel.NomeArquivo = viewModel.ImagemUpLoad.Name;
            novoUsuarioFotoViewModel.NomeArquivoExtensao = viewModel.ImagemUpLoad.FileName;
            //novoUsuarioFotoViewModel.ImagemUpLoad = viewModel.ImagemUpLoad;

            var responseData = await _usuarioService.AtualizarFoto(novoUsuarioFotoViewModel);
            if (ResponsePossuiErros(responseData))
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                AdicionarErroValidacao(msgErro);

                TempData["TipoMensagem"] = "danger";
                TempData["Mensagem"] = msgErro;

                return View(viewModel);
            }
            viewModel.ImagemConvertida = ConverterFormFileParaString(viewModel.ImagemUpLoad);
            TempData["TipoMensagem"] = "success";
            TempData["Mensagem"] = "Foto atualizada com sucesso!";

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("fotousuario")))
                HttpContext.Session.Remove("fotousuario");
            
            return View(viewModel);
        }

        public async Task<IActionResult> AssociarEmpresa(long id)
        {
            ViewBag.acao = "AdicionarContato";
            ViewBag.operacao = "I";

            var model = (EmpresasAssociadasViewModel) _usuarioService.ObterEmpresasDispiniveisPorUsuario(id).Result.Data;
            
            var usuario = _usuarioService.ObterPorUsuarioPorId(id.ToString()).Result;
            if(usuario != null)
            {
                model.UsuarioSelecionado = usuario.nome;
            }

            model.Empresas.ForEach(x => {
                var empresaAuth = model.EmpresasAssociadas.Find(a => a.IDEMPRESA == x.Id.ToString());

                model.EmpresasSelecao.Add(new EmpresaUsuarioSelecaoViewModel() { 
                    IDEMPRESA = x.Id.ToString(),
                    NomeEmpresa = x.NMRZSOCIAL,
                    Selecionado = (empresaAuth != null),
                    IDUSUARIO = id.ToString()
                });
            });
                      
            return PartialView("_AutorizarEmpresa",model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AssociarEmpresa(EmpresasAssociadasViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_AutorizarEmpresa", model);

            var listaEmpresaSelecionados = new List<EmpresaUsuarioSelecaoViewModel>();

            var idUsuario = Convert.ToInt64(model.EmpresasSelecao.FirstOrDefault().IDUSUARIO);
            
            model.EmpresasSelecao.ForEach( selec =>{
                if (selec.Selecionado)
                    listaEmpresaSelecionados.Add(selec);
            });
            
            var resposta = await _usuarioService.Adicionar(idUsuario,listaEmpresaSelecionados);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = "Erro ao tentar associar empresa" };
                AdicionarErroValidacao(retornoErro.mensagem);
                PartialView("_AutorizarEmpresa", model);
            }

            if (!OperacaoValida()) return PartialView("_AutorizarEmpresa", model);

            TempData["TipoMensagem"] = "success";
            TempData["Mensagem"] = "Operação realizada com Sucesso.";
            
            var url = Url.Action("ObterTodosUsuarios", "Usuario");

            return Json(new { success = true, url });
        }

        public async Task<ActionResult> SelecionarPerfil(string id)
        {
            
            var usuarioPerfil = await _usuarioService.ObterPorUsuarioPerfis(id);
            if (usuarioPerfil == null)
            {
                return NotFound();
            }
            await ObterViewModelPerfil(usuarioPerfil.Perfis);
          
            return PartialView("_selecionarPerfil", usuarioPerfil);
        }

        [HttpPost]
        public async Task<ActionResult> SelecionarPerfil(SelecionarPerfilViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await ObterViewModelPerfil(model.Perfis);
                return PartialView("_selecionarPerfil", model);
            }
            var retorno = await _usuarioService.SelecionarPerfil(model);

            if (!OperacaoValida())
            {
                await ObterViewModelPerfil(model.Perfis);
                return PartialView("_selecionarPerfil", model);
            }

            var url = Url.Action("ObterTodosUsuarios", "Usuario");

            TempData["TipoMensagem"] = "success";
            TempData["Mensagem"] = retorno.Data.ToString();

            return Json(new { success = true, url });
        }

            #region metodos Auxiliares
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

        private async Task ObterUsuariosClaims()
        {
            var usuarios = await ObterUsuarios(null);
            ViewBag.Usuarios = new SelectList(usuarios, "idUserAspNet", "nome","");
        }

        private async Task ObterViewModelPerfil(List<PerfilIndexViewModel> perfis)
        {
            ViewBag.Perfil = new SelectList(perfis, "Id", "Descricao", "");
        }

        private async Task ObterUsuariosClaimsRemovendoUsuarioSelecionado(string idUsuarioSelecionado)
        {
            var usuarios = await ObterUsuarios(null);
            ViewBag.Usuarios = new SelectList(usuarios.Where(usuarioSelec => usuarioSelec.id != idUsuarioSelecionado).OrderBy( usuarioSelec => usuarioSelec.nome)
                , "idUserAspNet", "nome", "");
        }

        private async Task<List<UserFull>> ObterUsuarios(string filtro)
        {
            return !string.IsNullOrEmpty(filtro)
                                    ? await _usuarioService.ObterUsuarios(filtro)
                                    : await _usuarioService.ObterUsuarios();
        }

        private async Task<PagedViewModel<UserFull>> ObterUsuarios(string filtro, int page, int pageSize)
        {
            return !string.IsNullOrEmpty(filtro)
                                    ? await _usuarioService.ObterUsuariosPorNome(filtro,page,pageSize)
                                    : await _usuarioService.ObterTodosUsuarios(page, pageSize);
        }

        private async Task ObterClaims()
        {
            //var listaClaimString = new List<string>();
            var listaClaim = await _autenticacaoService.ObterClaims();
            //listaClaim.ForEach(claim => listaClaimString.Add(claim.claimType));
            var item = new SelectListItem { Value = "", Text = "Selecione" };

            var selectLists = new SelectList(listaClaim.OrderBy(claim => claim.claimType), "claimType", "claimType", "") ;
       
            ViewBag.claims = selectLists;
        }

        private async Task<List<AcoesClaims>> ObterAcoesSelecionadas(List<string> acoes)
        {
            var listaAcoes = new List<AcoesClaims>();

            acoes.ForEach(acao =>
            {
                var acaoClaim = new AcoesClaims();
                acaoClaim.Acao = acao;
                listaAcoes.Add(acaoClaim);

            });

            return listaAcoes;
        }

        private string ConverterFormFileParaString(IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                string s = Convert.ToBase64String(fileBytes);

                // act on the Base64 data
                return String.Format("data:image/png;base64,{0}", s);
            }
        }

        #endregion

    }
}

using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.Extension;
using agilium.api.manager.ViewModels.ControleAcessoViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Security.Claims;
using agilium.api.manager.Data;
using Microsoft.AspNetCore.Http;
using AutoMapper.Configuration.Annotations;
using agilium.api.business.Interfaces.IRepository;
using Microsoft.Extensions.Configuration;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using agilium.api.manager.ViewModels.CaManagerViewModel;
using agilium.api.manager.ViewModels.CategoriaFinancViewModel;
using agilium.api.infra.Repository;

namespace agilium.api.manager.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/ca")]
    [ApiVersion("1.0")]
    public class CaController : MainController
    {
        private readonly ICaService _caService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUserAgilium> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private const string _nomeEntidade = "Controle Acesso";
        private readonly ICaRepositoryDapper _caRepositoryDapper;

        public CaController(INotificador notificador, IUser appUser, ICaService caService, IMapper mapper,
            UserManager<AppUserAgilium> userManager, RoleManager<IdentityRole> roleManager,
            ICaRepositoryDapper caRepositoryDapper, IConfiguration configuration, IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, configuration, utilDapperRepository, logService)
        {
            _userManager = userManager;
            _caService = caService;
            _roleManager = roleManager;
            _mapper = mapper;
            _caRepositoryDapper = caRepositoryDapper;
        }

        #region Perfil
        //[ClaimsAuthorize("ADMINSTRADOR", "CONSULTAR")]
        [HttpGet("perfil")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PerfilIndex([FromQuery] long idEmpresa ,[FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterPerfil(idEmpresa,q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return CustomResponse(lista);

        }

        [HttpGet("obter-perfil-por-id/{id}")]
       // [ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PerfilIndexViewModel>> ObterPorId(string id)
        {
            long _id = Convert.ToInt64(id);
            var objeto = _mapper.Map<PerfilIndexViewModel>(await _caService.ObterPerfilPorId(_id));
            return CustomResponse(objeto);
        }

        [HttpGet("obter-perfil-completo-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateModeloViewModel>> ObterPerfilCompletoPorId(long id)
        {
            var model = new CreateModeloViewModel();

            var permissoes = await _caService.ObterTodosListaPermissao();
            model.idPerfil = id;

            var modelosPorPerfil = _caService.ObterModelosPorPerfil(id).Result;
            
            permissoes.ToList().ForEach(permissao =>
            {
                var modeloPerfil = modelosPorPerfil.FirstOrDefault(x => x.idPermissao == permissao.Id);

                var novoModeloPermissao = new CreateModeloItemViewModel(id, permissao.Id, permissao.Descricao);

                if (modeloPerfil != null)
                {
                    novoModeloPermissao.Id = modeloPerfil.Id;
                    novoModeloPermissao.selecaoConsulta = ConverterSimNaoStringToBool(modeloPerfil.Consulta).Result;
                    novoModeloPermissao.selecaoIncluir = ConverterSimNaoStringToBool(modeloPerfil.Incluir).Result;
                    novoModeloPermissao.selecaoAlterar = ConverterSimNaoStringToBool(modeloPerfil.Alterar).Result;
                    novoModeloPermissao.selecaoExcluir = ConverterSimNaoStringToBool(modeloPerfil.Excluir).Result;
                    novoModeloPermissao.selecaoRelatorio = ConverterSimNaoStringToBool(modeloPerfil.Relatorio).Result;
                }

                model.Permissoes.Add(novoModeloPermissao);
            });

            var perfil = _caService.ObterPerfilPorId(id).Result;
            if(perfil != null)
                model.Perfil = perfil.Descricao;

            return CustomResponse(model);
        }

        [HttpPost("perfil")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "INCLUIR")]
        public async Task<IActionResult> AdicionarPerfil(CreateEditPerfilViewModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var perfil = _mapper.Map<CaPerfil>(model);

            if (perfil.Id == 0) perfil.Id = perfil.GerarId();
            await _caService.AdicionarPerfil(perfil);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("ControleAcesso", "AdicionarPerfil", "Web");
                return CustomResponse(msgErro);
            }

            await _caService.Salvar();

            LogInformacao($"Objeto adicionado com sucesso {Deserializar(perfil)}", "Ca", "AdicionarPerfil", null);
            return CustomResponse(model);
        }


        [HttpPut("perfil/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "ALTERAR")]
        public async Task<IActionResult> AtualizarPerfil(string id,[FromBody] CreateEditPerfilViewModel objetoViewModel)
        {
            if (id != objetoViewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                ObterNotificacoes("ControleAcesso", "AtualizarPerfil", "Web");
                return CustomResponse(objetoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<CaPerfil>(objetoViewModel);

            await _caService.AtualizarPerfil(objeto);
          
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("ControleAcesso", "AtualizarPerfil", "Web");
                return CustomResponse(msgErro);
            }

            await _caService.Salvar();
            LogInformacao($"Objeto atualizado com sucesso {Deserializar(objeto)}", "Ca", "AtualizarPerfil", null);
            return CustomResponse(objetoViewModel);
        }


        [HttpDelete("perfil/{id}")]
        public async Task<ActionResult> ApagarPerfil(string id,PerfilDeleteViewModel model)
        {
            if (id != model.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(model);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (!_caService.ApagarPerfil(model.Id).Result)
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));
                TempData["TipoMensagem"] = "danger";
                TempData["Mensagem"] = msgErro;

                NotificarErro(msgErro);
                ObterNotificacoes("ControleAcesso", "ApagarPerfil", "Web");
                return View(model);
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("ControleAcesso", "ApagarPerfil", "Web");
                return CustomResponse(msgErro);
            }
            await _caService.Salvar();
            LogInformacao($"Objeto apagado com sucesso {Deserializar(model)}", "Ca", "ApagarPerfil", null);
            return CustomResponse(model);
        }

        #endregion

        #region Permissao Item

        [HttpGet("permissao")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "CONSULTAR")]
        public async Task<IActionResult> PermissaoItemIndex([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterPermissaoItem(q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }


        [HttpPost("permissao")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // [ClaimsAuthorize("ADMINSTRADOR", "INCLUIR")]
        public async Task<IActionResult> AdicionarPermissaoItem(CreateEditPermissaoItemViewModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var permissaoItem = _mapper.Map<CaPermissaoItem>(model);

            if (permissaoItem.Id == 0) permissaoItem.Id = permissaoItem.GerarId();

            await _caService.AdicionarPermissaoItem(permissaoItem);

            await _roleManager.CreateAsync(new IdentityRole(permissaoItem.Descricao));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("ControleAcesso", "AdicionarPermissaoItem", "Web");
                return CustomResponse(msgErro);
            }

            await _caService.Salvar();
            LogInformacao($"Objeto adicionado com sucesso {Deserializar(model)}", "Ca", "AdicionarPermissaoItem", null);
            return CustomResponse(model);
        }

        #endregion

        #region Modelo


        [HttpPost("modelo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "INCLUIR")]
        public async Task<IActionResult> AdicionarModelo([FromBody]CreateModeloViewModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            //var camodelo = _mapper.Map<CaModelo>(model);
            //await _caService.ApagarModelosPorPerfil(model.idPerfil);
            //await _caService.Salvar();

            var novosModelos = new List<CaModelo>();
            model.Permissoes.ForEach(async permissao =>
            {
                var modelo = new CaModelo(permissao.idPerfil, permissao.idPermissao,
                    ConverterSimNaoBoolToString(permissao.selecaoIncluir), ConverterSimNaoBoolToString(permissao.selecaoAlterar),
                    ConverterSimNaoBoolToString(permissao.selecaoExcluir), ConverterSimNaoBoolToString(permissao.selecaoRelatorio),
                    ConverterSimNaoBoolToString(permissao.selecaoConsulta));

                if (modelo.Id == 0) modelo.Id = modelo.GerarId();


                novosModelos.Add(modelo);
              });

            await _caRepositoryDapper.AdicionarModeloPorPerfil(novosModelos);

            await AtualizarTodosUsuariosPorModelo(model.idPerfil);
            
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("ControleAcesso", "AdicionarModelo", "Web");
                return CustomResponse(msgErro);
            }
            await _caService.Salvar();
            var listaPermissoes = string.Join(" | ", model.Permissoes.ToList().Select(x => x.Permissao));
            LogInformacao($"Objeto adicionado com sucesso {listaPermissoes}", "Ca", "AdicionarModelo", null);
            return CustomResponse(listaPermissoes);
        }

        #endregion

        #region ca manager
        [HttpGet("areas")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CaAreaManagerViewModel>>> ObterAreas()
        {
            var lista = _mapper.Map<IEnumerable<CaAreaManagerViewModel>>(await _caService.ObterTodasCaAreas());

            return CustomResponse(lista);
        }

        [HttpGet("perfil-manager/permissoes/{id}")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CaPermissaoPerfilViewModel>>> ObterPermissaoPerfil(int id)
        {
            var permissaoPerfil = new CaPermissaoPerfilViewModel();
            var perfil = await _caService.ObterCompletoPerfilManagerPorId(id);
            if(perfil == null)
            {
                NotificarErro("Perfil nao localizado");
                var msgErro = string.Join("\n\r", ObterNotificacoes("Ca", "ObterPerfilPermissao", "Web"));
                return CustomResponse(msgErro);
            }

            var listaTodasAreas = _mapper.Map<IEnumerable<CaAreaManagerViewModel>>(await _caService.ObterTodasCaAreas());
            listaTodasAreas.ToList().ForEach(area => {
                area.Selecao = perfil.CaPermissaoManagers.Any(x => x.IdArea == area.IdArea);
                
                permissaoPerfil.Area.Add(area);
            });

            permissaoPerfil.Perfil.IdPerfil = perfil.IdPerfil;
            permissaoPerfil.Perfil.Descricao = perfil.Descricao;

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Ca", "ObterPerfilPermissao", "Web"));
                return CustomResponse(msgErro);
            }
            return CustomResponse(permissaoPerfil);
        }

        [HttpGet("perfil-por-descricao")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PerfilManagerIndex([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterPerfil(q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return CustomResponse(lista);

        }

        [HttpPost("perfil-manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] CaPerfilManagerViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
                        
            var perfil = _mapper.Map<CaPerfiManager>(viewModel);

            await _caService.AdicionarPerfil(perfil);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Perfil", "Adicionar", "Web");
                return CustomResponse(msgErro);
            }
            await _caService.Salvar();
            LogInformacao($"Objeto adicionado com sucesso {Deserializar(perfil)}", "Perfil", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("perfil-manager/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CaPerfilManagerViewModel>> ObterPerfilManager(int id)
        {
            var objeto = _mapper.Map<CaPerfilManagerViewModel>(await _caService.ObterPerfilManagerPorId(id));
            return CustomResponse(objeto);
        }

        [HttpGet("perfil-manager/todos")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CaPerfilManagerViewModel>>> ObterTodosPerfilManager()
        {
            var objeto = _mapper.Map<List<CaPerfilManagerViewModel>>(await _caService.ObterTodosCaPerfilManager());
            return CustomResponse(objeto);
        }

        [HttpPut("perfil-manager/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> AtualizarPerfil(long id, [FromBody] CaPerfiManager viewModel)
        {
            if (id != viewModel.IdPerfil)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                ObterNotificacoes("Perfil", "Atualizar", "Web");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _caService.AtualizarPerfil(_mapper.Map<CaPerfiManager>(viewModel));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Perfil", "Atualizar", "Web");
                return CustomResponse(msgErro);
            }
            await _caService.Salvar();
            LogInformacao($"Objeto atualizado com sucesso {Deserializar(viewModel)}", "Perfil", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        [HttpPost("permissoes-manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> AdicionarPermissoes([FromBody] List<CaAreaManagerSalvarViewModel> viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var perfil = _mapper.Map<List<CaPermissaoManager>>(viewModel);

            await _caService.AdicionarPermissoes(perfil);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Permissoes", "Adicionar", "Web");
                return CustomResponse(msgErro);
            }
            await _caService.Salvar();
            LogInformacao($"Objeto adicionado com sucesso {Deserializar(perfil)}", "Permissoes", "Adicionar", null);
            return CustomResponse(viewModel);
        }
        #endregion

        #region metodos Auxiliares

        private CaPerfil AdicionarPewrfilModelos(CreateModeloViewModel modelo)
        {

            var perfilAtualizar = _caService.ObterPerfilPorId(modelo.idPerfil).Result;
            
            if (perfilAtualizar == null) return null;

            modelo.Permissoes.ForEach(perm => {

                perfilAtualizar.AdicionarModelo(new CaModelo(perm.idPerfil,perm.idPermissao, ConverterSimNaoBoolToString(perm.selecaoIncluir), ConverterSimNaoBoolToString(perm.selecaoAlterar),
                    ConverterSimNaoBoolToString(perm.selecaoExcluir), ConverterSimNaoBoolToString(perm.selecaoRelatorio), ConverterSimNaoBoolToString(perm.selecaoConsulta)));
            });

            return perfilAtualizar;
        }

        private async Task AtualizarTodosUsuariosPorModelo(long idPerfil, AppUserAgilium usuarioAspNet)
        {
            //var usuarioAspNet = await _userManager.FindByIdAsync(IdAspNetUser);

            var claims = await _userManager.GetClaimsAsync(usuarioAspNet);

            if (claims.Count > 0)
                _userManager.RemoveClaimsAsync(usuarioAspNet, claims).Wait();

            var modelosPerfis = await _caService.ObterModelosPorPerfil(idPerfil);

            var novasClaims = new List<Claim>();

            modelosPerfis.ToList().ForEach(modelo =>
            {
                if (modelo.Incluir == "S") novasClaims.Add(new Claim(modelo.CaPermissaoItem.Descricao.ToUpper(), "INCLUIR"));
                if (modelo.Alterar == "S") novasClaims.Add(new Claim(modelo.CaPermissaoItem.Descricao.ToUpper(), "ALTERAR"));
                if (modelo.Excluir == "S") novasClaims.Add(new Claim(modelo.CaPermissaoItem.Descricao.ToUpper(), "EXCLUIR"));
                if (modelo.Relatorio == "S") novasClaims.Add(new Claim(modelo.CaPermissaoItem.Descricao.ToUpper(), "RELATORIO"));
                if (modelo.Consulta == "S") novasClaims.Add(new Claim(modelo.CaPermissaoItem.Descricao.ToUpper(), "CONSULTAR"));
            });

            if (novasClaims.Count > 0)
                await _userManager.AddClaimsAsync(usuarioAspNet, novasClaims);


        }
        private async Task AtualizarTodosUsuariosPorModelo(long idPerfil)
        {
            var usuariosPerfil = _caService.ObterUsuariosPorPerfil(idPerfil).Result.ToList();
            usuariosPerfil.ForEach(usu =>
            {

                var usuarioAspNet = _userManager.FindByIdAsync(usu.idUserAspNet).Result;
                AtualizarTodosUsuariosPorModelo(idPerfil, usuarioAspNet).Wait();
                _caService.Salvar();
            });
            
        }

        private string ConverterSimNaoBoolToString(bool valor)
        {
            return valor ? "S" : "N";
        }

        private async Task<bool> ConverterSimNaoStringToBool(string valor)
        {
            return valor == "S" ? true : false;
        }

        private async Task<PagedResult<PerfilIndexViewModel>> ObterPerfil(long idempresa, string filtro, int page, int pageSize)
        {
            var retorno = await _caService.ObterUsuariosPorDescricao(idempresa, filtro, page, pageSize);
            var listaTeste = retorno.List;
            var lista = _mapper.Map<IEnumerable<PerfilIndexViewModel>>(listaTeste);

            return new PagedResult<PerfilIndexViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<PagedResult<PermissaoItemIndexViewModel>> ObterPermissaoItem(string filtro, int page, int pageSize)
        {
            var retorno = await _caService.ObterPermissaoItemPorDescricao(filtro, page, pageSize);

            return new PagedResult<PermissaoItemIndexViewModel>()
            {
                List = _mapper.Map<IEnumerable<PermissaoItemIndexViewModel>>(retorno.List),
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
               // ReferenceAction = "PermissaoItemIndex",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<List<PermissaoItemIndexViewModel>> MontarListaModelosViewBag()
        {
            var permissoes = new List<PermissaoItemIndexViewModel>();
            permissoes = _mapper.Map<List<PermissaoItemIndexViewModel>>(await _caService.ObterTodosListaPermissao());
            ViewBag.Permissoes = new SelectList(permissoes, "id", "descricao", "");
            return permissoes;
        }

        private async Task<PagedResult<CaPerfilManagerViewModel>> ObterPerfil(string filtro, int page, int pageSize)
        {
            var retorno = await _caService.ObterPerfilPorDescricaoPaginacao(filtro, page, pageSize);
            var listaTeste = retorno.List;
            var lista = _mapper.Map<IEnumerable<CaPerfilManagerViewModel>>(listaTeste);

            return new PagedResult<CaPerfilManagerViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        #endregion
    }
}

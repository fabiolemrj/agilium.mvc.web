using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Services;
using agilium_manager_azure_business.Interfaces.IService;
using agilium.api.business.Models;
using agilum.mvc.web.Extensions;
using agilum.mvc.web.Services;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Endereco;
using agilum.mvc.web.ViewModels.Funcionarios;
using agilum.mvc.web.ViewModels.Usuarios;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Route("funcionario")]
    [Authorize]
    public class FuncionarioController : MainController
    {
        #region constantes
        private readonly IFuncionarioService _funcionarioService;
        private readonly IEmpresaService _empresaService;
        private readonly IUsuarioService _usuarioService;
        private const string _nomeEntidade = "Funcionario";
        private readonly IEnderecoService _enderecoService;
        private readonly ICaService _caService;
        #endregion

        #region construtor

        public FuncionarioController(IFuncionarioService funcionarioService, IEnderecoService enderecoService, ICaService caService,
            IEmpresaService empresaService, IUsuarioService usuarioService
            , INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService, IMapper mapper,
            ILicencaService licencaService, SignInManager<CaUsuarioIdentity> signInManager) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper, licencaService, signInManager)
        {
            _funcionarioService = funcionarioService;
            _enderecoService = enderecoService;
            _caService = caService;
            _empresaService = empresaService;
            _usuarioService = usuarioService;
        }
        #endregion

        #region funcionarios
        
        [Route("lista")]
        [ClaimsAuthorizeAttribute(2031)]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {

            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidade}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidade;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }
            var lista = (await ObterListaPaginado(Convert.ToInt64(empresaSelecionada.IDEMPRESA), q, page, ps)); ;

            ViewBag.Pesquisa = q;
            lista.Query = q;

            return View(lista);
        }

        [Route("novo")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2032)]
        public async Task<IActionResult> CreateFuncionario()
        {
            ObterEstados();
            var objeto = await ObterListaEmpresaUsuario();
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateFuncionario";

            var model = new FuncionarioViewModel();
            model.Situacao = EAtivo.Ativo;
            model.Endereco.Id = 0;
            model.DataAdmissao = DateTime.Now;
            if (objeto != null)
            {
                model.Empresas = objeto.Empresas;
                model.Usuarios = objeto.Usuarios;
            }
        
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();
            model.Codigo = await _funcionarioService.GerarCodigo(Convert.ToInt64(empresaSelecionada.IDEMPRESA));
            model.IDEMPRESA = Convert.ToInt64(empresaSelecionada.IDEMPRESA);
            return View("CreateEditFuncionario", model);
        }

        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> CreateFuncionario(FuncionarioViewModel model)
        {
            ObterEstados();
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateFuncionario";
            if (!ModelState.IsValid) return View("CreateEditFuncionario", model);

            if (string.IsNullOrEmpty(model.Endereco.Logradouro)) AdicionarErroValidacao("Campo Logradouro é obrigatório");
            if (string.IsNullOrEmpty(model.Endereco.Cep)) AdicionarErroValidacao("Campo Cep é obrigatório");
            if (string.IsNullOrEmpty(model.Endereco.Uf)) AdicionarErroValidacao("Campo Uf é obrigatório");
            if (string.IsNullOrEmpty(model.Endereco.Cidade)) AdicionarErroValidacao("Campo Cidade é obrigatório");


            ValidarFuncionario(model);
            if (model.IDENDERECO != null)
            {
                await _enderecoService.AtualizarAdicionar(_mapper.Map<Endereco>(model.Endereco));
                model.Endereco = null;
            }
            else
            {
                model.Endereco = null;
                model.IDENDERECO = null;
            }

            var funcionario = _mapper.Map<Funcionario>(model);

            if (funcionario.Id == 0) funcionario.Id = funcionario.GerarId();
            await _funcionarioService.Adicionar(funcionario);

            if (!OperacaoValida())
            {
                var objeto = await ObterListaEmpresaUsuario();
                if (objeto != null)
                {
                    model.Empresas = objeto.Empresas;
                    model.Usuarios = objeto.Usuarios;
                }

                var msgErro = string.Join("\n\r", ObterNotificacoes("Funcionario", "Adicionar", "Web", Deserializar(model)));
                AdicionarErroValidacao(msgErro);
                return View("CreateEditFuncionario", model);
            }
            await _funcionarioService.Salvar();
            LogInformacao($"sucesso: {Deserializar(funcionario)}", "Funcionario", "Adicionar", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }


        [Route("editar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2035)]
        public async Task<IActionResult> EditFuncionario(long id)
        {
            ObterEstados();
            ViewBag.operacao = "E";
            ViewBag.acao = "EditFuncionario";
            var objeto = await Obter(id.ToString());
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

            return View("CreateEditFuncionario", objeto);
        }

        [Route("editar")]
        [HttpPost]
        public async Task<IActionResult> EditFuncionario(FuncionarioViewModel model)
        {
            ObterEstados();
            ViewBag.operacao = "E";
            ViewBag.acao = "EditFuncionario";

            if (!ModelState.IsValid) return View("CreateEditFornecedor", model);

            if (string.IsNullOrEmpty(model.Endereco.Logradouro)) AdicionarErroValidacao("Campo Logradouro é obrigatório");
            if (string.IsNullOrEmpty(model.Endereco.Cep)) AdicionarErroValidacao("Campo Cep é obrigatório");
            if (string.IsNullOrEmpty(model.Endereco.Uf)) AdicionarErroValidacao("Campo Uf é obrigatório");
            if (string.IsNullOrEmpty(model.Endereco.Cidade)) AdicionarErroValidacao("Campo Cidade é obrigatório");

            if (!OperacaoValida())
                View("CreateEditFuncionario", model);

            ValidarFuncionario(model);
            if (model.IDENDERECO != null)
            {
                await _enderecoService.AtualizarAdicionar(_mapper.Map<Endereco>(model.Endereco));
                model.Endereco = null;
            }
            else
            {
                model.Endereco = null;
                model.IDENDERECO = null;
            }


            await _funcionarioService.Atualizar(_mapper.Map<Funcionario>(model));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Funcionario", "Atualizar", "Web", Deserializar(model)));
                return View("CreateEditFuncionario", model);
            }
            await _funcionarioService.Salvar();
            LogInformacao($"sucesso: {Deserializar(model)}", "Funcionario", "Atualizar", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2033)]
        public async Task<IActionResult> DeleteFuncionario(long id)
        {
            ObterEstados();
            var objeto = await Obter(id.ToString());
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidade} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View(objeto);
        }

        [Route("apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteFuncionario(FuncionarioViewModel model)
        {
            ObterEstados();
            if (!ModelState.IsValid) return View(model);

            await _funcionarioService.Apagar(model.Id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Funcionario", "Excluir", "Web", Deserializar(model)));
                AdicionarErroValidacao(msgErro);
                return View(model);
            }

            await _funcionarioService.Salvar();
            
            LogInformacao($"sucesso: {Deserializar(model)}", "Funcionario", "Excluir", null);
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        #endregion

        #region metodos privados
        private async Task<PagedViewModel<FuncionarioViewModel>> ObterListaPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _funcionarioService.ObterPorNomePaginacao(idEmpresa, filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<FuncionarioViewModel>>(retorno.List);

            lista.ToList().ForEach(func => {
                if (func.Usuario != null && !string.IsNullOrEmpty(func.Usuario.idperfilManager))
                {
                    var perfil = _caService.ObterPerfilPorId(Convert.ToInt64(func.Usuario.idperfilManager)).Result;
                    if (perfil != null && !string.IsNullOrEmpty(perfil.Descricao))
                        func.Perfil = perfil.Descricao;
                }

            });
            return new PagedViewModel<FuncionarioViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "lista",
                ReferenceController = "funcionario",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<FuncionarioViewModel> ObterListaEmpresaUsuario()
        {
            var objeto = new FuncionarioViewModel()
            {
                Id = 0
            };
            objeto.Empresas = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);
            objeto.Usuarios = _mapper.Map<List<UserFull>>(_usuarioService.ObterTodosUsuariosValidos().Result);

            return objeto;
        }


        private void ValidarFuncionario(FuncionarioViewModel viewModel)
        {
            if (viewModel.Endereco != null && viewModel.Endereco.Id > 0 && (viewModel.Endereco.Id != viewModel.IDENDERECO))
                viewModel.IDENDERECO = viewModel.Endereco.Id;
            if (viewModel.Endereco != null && viewModel.Endereco.Id == 0)
            {
                if (string.IsNullOrEmpty(viewModel.Endereco.Cep))
                {
                    viewModel.Endereco = null;
                    viewModel.IDENDERECO = null;
                }
                else
                {
                    viewModel.Endereco.Id = GerarId().Result;
                    if (viewModel.IDENDERECO == null || viewModel.IDENDERECO == 0) viewModel.IDENDERECO = viewModel.Endereco.Id;
                }
            }
            if (viewModel.Usuario != null && Convert.ToInt64(viewModel.Usuario.id) > 0 && (Convert.ToInt64(viewModel.Usuario.id) != viewModel.IDUSUARIO))
                viewModel.IDUSUARIO = Convert.ToInt64(viewModel.Usuario.id);
            if (viewModel.Usuario != null)
                viewModel.Usuario = null;
        }

        public async Task<FuncionarioViewModel> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var fornecedor = await _funcionarioService.ObterCompletoPorId(_id);
            if (fornecedor != null)
            {
                var objeto = _mapper.Map<FuncionarioViewModel>(fornecedor);
                objeto.Endereco = _mapper.Map<EnderecoIndexViewModel>(fornecedor.Endereco);
                objeto.Empresas = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);
                objeto.Usuarios = _mapper.Map<List<UserFull>>(_usuarioService.ObterTodosUsuariosValidos().Result);
                return objeto;
            }

            return null;

        }

        #endregion

        #region ViewBag
        private void ObterEstados()
        {
            var estados = ListasAuxilares.ObterEstados();
            ViewBag.estados = new SelectList(estados, "Sigla", "Nome", "");
        }
        #endregion
    }
}

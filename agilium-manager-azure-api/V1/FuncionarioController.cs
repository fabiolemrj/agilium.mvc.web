using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels;
using agilium.api.manager.ViewModels.EmpresaViewModel;
using agilium.api.manager.ViewModels.EnderecoViewModel;
using agilium.api.manager.ViewModels.FornecedorViewModel;
using agilium.api.manager.ViewModels.FuncionarioViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.manager.V1
{

    [Authorize]
    [Route("api/v{version:apiVersion}/funcionario")]
    [ApiVersion("1.0")]
    [ApiController]
    public class FuncionarioController : MainController
    {

        private readonly IFuncionarioService _funcionarioService;
        private readonly ICaService _caService;
        private readonly IEmpresaService _empresaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        private const string _nomeEntidade = "Funcionario";
        private readonly IEnderecoService _enderecoService;
        public FuncionarioController(INotificador notificador, IUser appUser, IConfiguration configuration,
            IFuncionarioService funcionarioService, IMapper mapper, IEnderecoService enderecoService, ICaService caService,
            IEmpresaService empresaService, IUsuarioService usuarioService, IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, configuration,utilDapperRepository,logService)
        {
            _funcionarioService = funcionarioService;
            _mapper = mapper;
            _enderecoService = enderecoService;
            _caService = caService;
            _empresaService = empresaService;
            _usuarioService = usuarioService;
        }

        #region endpoint
        [HttpGet("obter-por-nome")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FuncionarioViewModel>>> IndexPagination([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterListaPaginado(idEmpresa, q, page, ps));
            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] FuncionarioViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            ValidarFuncionario(viewModel);
            if (viewModel.IDENDERECO != null)
            {
                await _enderecoService.AtualizarAdicionar(_mapper.Map<Endereco>(viewModel.Endereco));
                viewModel.Endereco = null;
            }

            var funcionario = _mapper.Map<Funcionario>(viewModel);

            if (funcionario.Id == 0) funcionario.Id = funcionario.GerarId();
            await _funcionarioService.Adicionar(funcionario);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Funcionario", "Adicionar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _funcionarioService.Salvar();
            LogInformacao($"sucesso: {Deserializar(funcionario)}", "Funcionario", "Adicionar", null);
            return CustomResponse(viewModel);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] FuncionarioViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            ValidarFuncionario(viewModel);
            if (viewModel.IDENDERECO != null)
            {
                await _enderecoService.AtualizarAdicionar(_mapper.Map<Endereco>(viewModel.Endereco));
                viewModel.Endereco = null;
            }
              

            await _funcionarioService.Atualizar(_mapper.Map<Funcionario>(viewModel));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Funcionario", "Atualizar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _funcionarioService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Funcionario", "Atualizar", null);
            return CustomResponse(viewModel);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(long id)
        {
            var viewModel = await _funcionarioService.ObterPorId(id);

            if (viewModel == null) return NotFound();

            await _funcionarioService.Apagar(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Funcionario", "Excluir", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _funcionarioService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Funcionario", "Excluir", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("obter-todas")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<FuncionarioViewModel>>> ObterTodas()
        {
            var objeto = _mapper.Map<IEnumerable<FuncionarioViewModel>>(await _funcionarioService.ObterTodas());
            return CustomResponse(objeto);
        }

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FuncionarioViewModel>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var fornecedor = await _funcionarioService.ObterCompletoPorId(_id);
            if (fornecedor != null)
            {
                var objeto = _mapper.Map<FuncionarioViewModel>(fornecedor);
                objeto.Endereco = _mapper.Map<EnderecoIndexViewModel>(fornecedor.Endereco);
                objeto.Empresas = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);
                objeto.Usuarios = _mapper.Map<List<UsuarioPadrao>>(_usuarioService.ObterTodosUsuarios().Result);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("fornecedor nao localizado"));

        }

        [HttpGet("listas-auxiliares")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FuncionarioViewModel>> ObterListaEmpresaUsuario()
        {
            var objeto = new FuncionarioViewModel() {
                Id = 0
            };
            objeto.Empresas = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);
            objeto.Usuarios = _mapper.Map<List<UsuarioPadrao>>(_usuarioService.ObterTodosUsuarios().Result);

            return CustomResponse(objeto);
        }

        #endregion

        #region metodos privados
        private async Task<business.Models.PagedResult<FuncionarioViewModel>> ObterListaPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _funcionarioService.ObterPorNomePaginacao(idEmpresa,filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<FuncionarioViewModel>>(retorno.List);

            lista.ToList().ForEach( func => {
                if (func.Usuario != null && !string.IsNullOrEmpty(func.Usuario.idperfil))
                {
                    var perfil = _caService.ObterPerfilPorId(Convert.ToInt64(func.Usuario.idperfil)).Result;
                    if (perfil != null && !string.IsNullOrEmpty(perfil.Descricao))
                        func.Perfil = perfil.Descricao;
                }
                
            });
            return new business.Models.PagedResult<FuncionarioViewModel>()
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

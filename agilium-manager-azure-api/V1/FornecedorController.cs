using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.CategoriaFinancViewModel;
using agilium.api.manager.ViewModels.EnderecoViewModel;
using agilium.api.manager.ViewModels.FornecedorViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.api.manager.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/fornecedor")]
    [ApiVersion("1.0")]
    public class FornecedorController : MainController
    {
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;
        private const string _nomeEntidade = "Fornecedor";
        public FornecedorController(INotificador notificador, IUser appUser, IConfiguration configuration,
            IFornecedorService fornecedorService, IMapper mapper, IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, configuration, utilDapperRepository, logService)
        {
            _fornecedorService = fornecedorService;
            _mapper = mapper;
        }

        #region Fornecedor
        [HttpGet("obter-por-razaosocial")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FornecedorContatoViewModel>>> IndexPagination([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterListaPaginado(q, page, ps));
            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] FornecedorViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);                       
           
            var fornecedor = _mapper.Map<Fornecedor>(viewModel);

            if (fornecedor.Id == 0) fornecedor.Id = fornecedor.GerarId();

            if (viewModel.Endereco != null)
            {
                //var endereco = _mapper.Map<Endereco>(viewModel.Endereco);

                //if (endereco != null && endereco.Id == 0)
                //    endereco.Id = GerarId();

                // fornecedor.AdicionarEndereco(fornecedor.Id);

                if (fornecedor.Endereco.Id == 0) fornecedor.Endereco.Id = await GerarId();
            }

            await _fornecedorService.Adicionar(fornecedor);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Fornecedor", "Adicionar", "Web", Deserializar(fornecedor)));
                return CustomResponse(msgErro);
            }
            await _fornecedorService.Salvar();
            LogInformacao($"sucesso: {Deserializar(fornecedor)}", "Fornecedor", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FornecedorViewModel>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var fornecedor = await _fornecedorService.ObterCompletoPorId(_id);
            if(fornecedor != null)
            {
                var objeto = _mapper.Map<FornecedorViewModel>(fornecedor);
                objeto.Endereco = _mapper.Map<EnderecoIndexViewModel>(fornecedor.Endereco);
                objeto.Contatos = _mapper.Map<List<FornecedorContatoViewModel>>(fornecedor.FornecedoresContatos);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("fornecedor nao localizado"));
         
        }

        [HttpGet("obter-todos")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> ObterTodos()
        {
            var objeto = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorService.ObterTodos());
            return CustomResponse(objeto);

        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] FornecedorViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(viewModel));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Fornecedor", "Atualizar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _fornecedorService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Fornecedor", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(long id)
        {
            var viewModel = await _fornecedorService.ObterPorId(id);

            if (viewModel == null) return NotFound();

            await _fornecedorService.Apagar(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Fornecedor", "Excluir", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _fornecedorService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Fornecedor", "Excluir", null);
            return CustomResponse(viewModel);
        }
        #endregion

        #region FornecedorContato
        [HttpGet("contato/obter-por-id")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FornecedorContatoViewModel>> ObterPorId([FromQuery] long idContato, [FromQuery] long idFornecedor)
        {
            var objeto = _mapper.Map<FornecedorContatoViewModel>(await _fornecedorService.ObterFornecedorContatoPorId(idFornecedor, idContato));
            return CustomResponse(objeto);
        }



        [HttpPost("contato")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Adicionar([FromBody] FornecedorContatoViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var fornecedorContato = _mapper.Map<FornecedorContato>(viewModel);

            if (fornecedorContato.Contato.Id == 0)
                fornecedorContato.Contato.Id = await GerarId();
                        
            await _fornecedorService.AdicionarContato(fornecedorContato);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Fornecedor", "AdicionarContato", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            await _fornecedorService.Salvar();
            LogInformacao($"sucesso: {Deserializar(fornecedorContato)}", "Fornecedor", "AdicionarContato", null);
            return CustomResponse(viewModel);
        }

        [HttpDelete("contato")]
        public async Task<ActionResult> ExcluirContatoEmpresa([FromQuery] long idContato, [FromQuery] long idFornecedor)
        {
            await _fornecedorService.RemoverContato(idFornecedor,idContato);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Fornecedor", "ExcluirContatoEmpresa", "Web"));
                return CustomResponse(msgErro);
            }
            await _fornecedorService.Salvar();
            LogInformacao($"sucesso: idcontato {idContato}", "Fornecedor", "ExcluirContatoEmpresa", null);
            return CustomResponse("sucesso");
        }

        [HttpPut("contato")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Atualizar([FromBody] FornecedorContatoViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var fornecedorContato = _mapper.Map<FornecedorContato>(viewModel);

            await _fornecedorService.Atualizar(fornecedorContato);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Fornecedor", "AtualizarContatoEmpresa", "Web"));
                return CustomResponse(msgErro);
            }
            await _fornecedorService.Salvar();
            LogInformacao($"sucesso: idcontato {Deserializar(fornecedorContato)}", "Fornecedor", "AtualizarContatoEmpresa", null);
            return CustomResponse(viewModel);
        }

        #endregion

        #region Metodos Privados
        private async Task<PagedResult<FornecedorViewModel>> ObterListaPaginado(string filtro, int page, int pageSize)
        {
            var retorno = await _fornecedorService.ObterPorRazaoSocialPaginacao(filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<FornecedorViewModel>>(retorno.List);

            return new PagedResult<FornecedorViewModel>()
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

using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Services;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.ClienteViewModel;
using agilium.api.manager.ViewModels.EnderecoViewModel;
using agilium.api.manager.ViewModels.FornecedorViewModel;
using AutoMapper;
using Dapper;
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
    [Route("api/v{version:apiVersion}/cliente")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ClienteController : MainController
    {
        private readonly IClienteService _clienteService;
        private readonly IMapper _mapper;
        private const string _nomeEntidade = "Cliente";
        private readonly IEnderecoService  _enderecoService;
        public ClienteController(INotificador notificador, IUser appUser, IConfiguration configuration,
            IClienteService clienteService, IMapper mapper, IEnderecoService enderecoService, 
            IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, configuration, 
                utilDapperRepository, logService)
        {
            _clienteService = clienteService;
            _mapper = mapper;
            _enderecoService = enderecoService;
        }

        #region Cliente
        [HttpGet("obter-por-nome")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ClienteViewModel>>> IndexPagination([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
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
        public async Task<ActionResult> Adicionar([FromBody] ClienteViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            Cliente cliente = await ValidarCliente(viewModel);
            //  var cliente = _mapper.Map<Cliente>(viewModel);

            if (cliente.Id == 0) cliente.Id = cliente.GerarId();
            
            await _clienteService.Adicionar(cliente);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Cliente", "Adicionar", "Web");
                return CustomResponse(msgErro);
            }
            await _clienteService.Salvar();
            LogInformacao($"Objeto adicionado com sucesso {Deserializar(cliente)}", "Cliente", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClienteViewModel>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var fornecedor = await _clienteService.ObterCompletoPorId(_id);
            if (fornecedor != null)
            {
                var objeto = _mapper.Map<ClienteViewModel>(fornecedor);
                objeto.Endereco = _mapper.Map<EnderecoIndexViewModel>(fornecedor.Endereco);
                objeto.EnderecoFaturamento = _mapper.Map<EnderecoIndexViewModel>(fornecedor.EnderecoFaturamento);
                objeto.EnderecoCobranca = _mapper.Map<EnderecoIndexViewModel>(fornecedor.EnderecoCobranca);
                objeto.EnderecoEntrega = _mapper.Map<EnderecoIndexViewModel>(fornecedor.EnderecoEntrega);
                objeto.Contatos = _mapper.Map<List<ClienteContatoViewModel>>(fornecedor.ClienteContatos);
                objeto.ClientePessoaFisica = _mapper.Map<ClientePFViewModel>(fornecedor.ClientesPFs);
                objeto.ClientePessoaJuridica = _mapper.Map<ClientePJViewModel>(fornecedor.ClientesPJs);

                if (objeto.EnderecoCobranca == null) objeto.EnderecoCobranca = new EnderecoIndexViewModel();
                if (objeto.EnderecoEntrega == null) objeto.EnderecoEntrega = new EnderecoIndexViewModel();
                if (objeto.EnderecoFaturamento == null) objeto.EnderecoFaturamento = new EnderecoIndexViewModel();
                if (objeto.Endereco == null) objeto.Endereco = new EnderecoIndexViewModel();

                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("cliente nao localizado"));
        }

        [HttpGet("todos")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ClienteViewModel>>> ObterTodos()
        {
            var fornecedor = await _clienteService.ObterTodos();
            var objeto = _mapper.Map<List<ClienteViewModel>>(fornecedor);
            
            return CustomResponse(objeto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] ClienteViewModel viewModel)
        {

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                ObterNotificacoes("Cliente", "Atualizar", "Web");
                return CustomResponse(viewModel);
            }

            Cliente cliente = await ValidarCliente(viewModel);

            await _clienteService.Atualizar(cliente);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Cliente", "Atualizar", "Web");
                return CustomResponse(msgErro);
            }
            await _clienteService.Salvar();
            LogInformacao($"Objeto adicionado com sucesso {Deserializar(cliente)}", "Cliente", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        private async Task<Cliente> ValidarCliente(ClienteViewModel viewModel)
        {
            if (viewModel.EnderecoCobranca != null && viewModel.EnderecoCobranca.Id > 0 && (viewModel.EnderecoCobranca.Id != viewModel.IDENDERECOCOB))
                viewModel.IDENDERECOCOB = viewModel.EnderecoCobranca.Id;
            else if (viewModel.EnderecoCobranca != null && viewModel.EnderecoCobranca.Id == 0)
            {
                if (string.IsNullOrEmpty(viewModel.EnderecoCobranca.Cep)) viewModel.EnderecoCobranca = null;
                else
                {
                    viewModel.EnderecoCobranca.Id = await GerarId();
                    if (viewModel.IDENDERECOCOB == null || viewModel.IDENDERECOCOB == 0) viewModel.IDENDERECOCOB = viewModel.EnderecoCobranca.Id;
                }
            }

            if (viewModel.EnderecoEntrega != null && viewModel.EnderecoEntrega.Id > 0 && (viewModel.EnderecoEntrega.Id != viewModel.IDENDERECONTREGA))
                viewModel.IDENDERECONTREGA = viewModel.EnderecoEntrega.Id;
            else if (viewModel.EnderecoEntrega != null && viewModel.EnderecoEntrega.Id == 0)
            {
                if (string.IsNullOrEmpty(viewModel.EnderecoEntrega.Cep)) viewModel.EnderecoEntrega = null;
                else
                {
                    viewModel.EnderecoEntrega.Id = await GerarId();
                    if (viewModel.IDENDERECONTREGA == null || viewModel.IDENDERECONTREGA == 0) viewModel.IDENDERECONTREGA = viewModel.EnderecoEntrega.Id;
                }
            }

            if (viewModel.EnderecoFaturamento != null && viewModel.EnderecoFaturamento.Id > 0 && (viewModel.EnderecoFaturamento.Id != viewModel.IDENDERECOFAT))
                viewModel.IDENDERECOFAT = viewModel.EnderecoFaturamento.Id;
            else if (viewModel.EnderecoFaturamento != null && viewModel.EnderecoFaturamento.Id == 0)
            {
                if (string.IsNullOrEmpty(viewModel.EnderecoFaturamento.Cep)) viewModel.EnderecoFaturamento = null;
                else
                {
                    if (viewModel.IDENDERECOFAT == null || viewModel.IDENDERECOFAT == 0) viewModel.IDENDERECOFAT = viewModel.EnderecoFaturamento.Id;
                }
            }

            if (viewModel.Endereco != null && viewModel.Endereco.Id > 0 && (viewModel.Endereco.Id != viewModel.IDENDERECO))
                viewModel.IDENDERECO = viewModel.Endereco.Id;
            if (viewModel.Endereco != null && viewModel.Endereco.Id == 0)
            {
                if (string.IsNullOrEmpty(viewModel.Endereco.Cep)) viewModel.Endereco = null;
                else
                {
                    viewModel.Endereco.Id = await GerarId();
                    if (viewModel.IDENDERECO == null || viewModel.IDENDERECO == 0) viewModel.IDENDERECO = viewModel.Endereco.Id;
                }
            }

            var cliente = _mapper.Map<Cliente>(viewModel);

            if (viewModel.Endereco != null && viewModel.Endereco.Id > 0)
            {
                var _endereco = _mapper.Map<Endereco>(viewModel.Endereco);
                await _enderecoService.AtualizarAdicionar(_endereco);
                cliente.AdicionarEndereco(null);
            }

            if (viewModel.EnderecoCobranca != null && viewModel.EnderecoCobranca.Id > 0)
            {
                var _endereco = _mapper.Map<Endereco>(viewModel.EnderecoCobranca);
                await _enderecoService.AtualizarAdicionar(_endereco);
                cliente.AdicionarEnderecoCobranca(null);
            }
            if (viewModel.EnderecoFaturamento != null && viewModel.EnderecoFaturamento.Id > 0)
            {
                var _endereco = _mapper.Map<Endereco>(viewModel.EnderecoFaturamento);
                await _enderecoService.AtualizarAdicionar(_endereco);
                cliente.AdicionarEnderecoFaturamento(null);
            }
            if (viewModel.EnderecoEntrega != null && viewModel.EnderecoEntrega.Id > 0)
            {
                var _endereco = _mapper.Map<Endereco>(viewModel.EnderecoEntrega);
                await _enderecoService.AtualizarAdicionar(_endereco);
                cliente.AdicionarEnderecoEntrega(null);
            }

            if (viewModel.TipoPessoa == business.Enums.ETipoPessoa.F && viewModel.ClientePessoaFisica != null)
            {
                if (viewModel.ClientePessoaFisica.Id == 0) viewModel.ClientePessoaFisica.Id = viewModel.Id;
                var clientePF = _mapper.Map<ClientePF>(viewModel.ClientePessoaFisica);
                cliente.AdicionarPessoaFisica(clientePF);
            }
            else if (viewModel.TipoPessoa == business.Enums.ETipoPessoa.J && viewModel.ClientePessoaJuridica != null)
            {
                if (viewModel.ClientePessoaJuridica.Id == 0) viewModel.ClientePessoaJuridica.Id = viewModel.Id;
                var clientePJ = _mapper.Map<ClientePJ>(viewModel.ClientePessoaJuridica);
                cliente.AdicionarPessoaJuridica(clientePJ);
            }

            return cliente;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(long id)
        {
            var viewModel = await _clienteService.ObterPorId(id);

            if (viewModel == null) return NotFound();

            await _clienteService.Apagar(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Cliente", "Excluir", "Web");
                return CustomResponse(msgErro);
            }
            await _clienteService.Salvar();
            LogInformacao($"Objeto excluido com sucesso id:{id}", "Cliente", "Excluir", null);
            return CustomResponse(viewModel);
        }
        #endregion

        #region ClienteContato
        [HttpGet("contato/obter-por-id")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClienteContatoViewModel>> ObterPorId([FromQuery] long idContato, [FromQuery] long idCliente)
        {
            var objeto = _mapper.Map<ClienteContatoViewModel>(await _clienteService.ObterClienteContatoPorId(idCliente, idContato));
            return CustomResponse(objeto);
        }

        [HttpPost("contato")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Adicionar([FromBody] ClienteContatoViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var fornecedorContato = _mapper.Map<ClienteContato>(viewModel);

            if (fornecedorContato.Contato.Id == 0)
                fornecedorContato.Contato.Id = await GerarId();

            await _clienteService.AdicionarContato(fornecedorContato);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Cliente", "AdicionarContato", "Web");
                return CustomResponse(msgErro);
            }
            await _clienteService.Salvar();
            LogInformacao($"Objeto adicionado com sucesso {Deserializar(fornecedorContato)}", "Cliente", "AdicionarContato", null);
            return CustomResponse(viewModel);
        }

        [HttpDelete("contato")]
        public async Task<ActionResult> ExcluirContatoEmpresa([FromQuery] long idContato, [FromQuery] long idCliente)
        {
            await _clienteService.RemoverContato(idCliente, idContato);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Cliente", "ExcluirContato", "Web");
                return CustomResponse(msgErro);
            }
            await _clienteService.Salvar();
            LogInformacao($"Objeto excluido com sucesso idcontato:{idContato} idcliente:{idCliente}", "Cliente", "AdicionarContato", null);
            return CustomResponse("sucesso");
        }

        [HttpPut("contato")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Atualizar([FromBody] ClienteContatoViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var fornecedorContato = _mapper.Map<ClienteContato>(viewModel);

            await _clienteService.Atualizar(fornecedorContato);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Cliente", "AtualizarContato", "Web");
                return CustomResponse(msgErro);
            }
            await _clienteService.Salvar();
            LogInformacao($"Objeto atualizado com sucesso {Deserializar(viewModel)}", "Cliente", "AtualiarContato", null);
            return CustomResponse(viewModel);
        }
        #endregion

        #region Cliente Preco
        [HttpGet("precos/obter-por-idproduto/{idProduto}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ClientePrecoViewModel>>> ObterClientePreco(long idProduto)
        {
            var lista = _clienteService.ObterClientePrecoPorProduto(idProduto).Result;

            var model = _mapper.Map<List<ClientePrecoViewModel>>(lista);

            return CustomResponse(model);
        }

        [HttpPost("preco")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "INCLUIR")]
        public async Task<IActionResult> AdicionarPreco([FromBody] ClientePrecoViewModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (model.Id == 0) model.Id = await GerarId();
            if (model.Datahora == null) model.Datahora = DateTime.Now;
            if (string.IsNullOrEmpty(model.Usuario)) model.Usuario = AppUser.GetUserEmail();

            var objeto = _mapper.Map<ClientePreco>(model);

            await _clienteService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Cliente", "AdicionarPreco", "Web");
                return CustomResponse(msgErro);
            }

            await _clienteService.Salvar();
            LogInformacao($"Objeto adicionado com sucesso {Deserializar(objeto)}", "Cliente", "AtualiarContato", null);
            return CustomResponse(model);
        }

        [HttpDelete("preco/{id}")]
        public async Task<ActionResult> ExcluirPreco(long id)
        {
            var viewModel = await _clienteService.ObteClientePrecoPorId(id);

            if (viewModel == null) return NotFound();

            await _clienteService.Remover(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                return CustomResponse(msgErro);
            }
            await _clienteService.Salvar();
            LogInformacao($"Objeto excluido com sucesso id:{id}", "Cliente", "ExcluirPreco", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("preco/obter-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientePrecoViewModel>> ObterPreco(string id)
        {
            long _id = Convert.ToInt64(id);
            var produto = await _clienteService.ObteClientePrecoPorId(_id);
            if (produto != null)
            {
                var objeto = _mapper.Map<ClientePrecoViewModel>(produto);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Preço do produto por cliente nao localizado"));

        }

        #endregion

        #region metodos privados
        private async Task<PagedResult<ClienteViewModel>> ObterListaPaginado(string filtro, int page, int pageSize)
        {
            var retorno = await _clienteService.ObterPorNomePaginacao(filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<ClienteViewModel>>(retorno.List);

            return new PagedResult<ClienteViewModel>()
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

using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.PedidoViewModel;
using agilium.api.pdv.Controllers;
using agilium.api.pdv.ViewModels.EmpresaViewModel;
using agilium.api.pdv.ViewModels.EnderecoViewModel;
using agilium.api.pdv.ViewModels.FuncionarioViewModel;
using agilium.api.pdv.ViewModels.MoedaViewModel;
using agilium.api.pdv.ViewModels.PedidoViewModel;
using agilium.api.pdv.ViewModels.ProdutoViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.pdv.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/pedido")]
    [ApiVersion("1.0")]
    [ApiController]
    public class PedidoController : MainController
    {
        private readonly IPedidoService _pedidoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public PedidoController(INotificador notificador, IUser appUser, IConfiguration configuration, IUtilDapperRepository utilDapperRepository, ILogService logService,
            IPedidoService pedidoService, IMapper mapper, IUsuarioService usuarioService) : base(notificador, appUser, configuration, utilDapperRepository, logService)
        {
            _pedidoService = pedidoService;
            _mapper = mapper;
            _usuarioService = usuarioService;
        }

        [HttpPost("obter-por-data")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PedidoViewModel>>> ObterTodas([FromBody]PedidoFiltroConsultaViewModel model)
        {
            if(model.dataFinal.Hour == 0 && model.dataFinal.Minute == 0 && model.dataFinal.Second == 0)
            {
                model.dataFinal = model.dataFinal.AddHours(23).AddMinutes(59).AddSeconds(59);
            }
                

            if (model.dataInicial >= model.dataFinal)
                model.dataFinal = new DateTime(model.dataFinal.Year, model.dataFinal.Month, model.dataFinal.Day,23,59,59);

            var objeto = _mapper.Map<IEnumerable<PedidoViewModel>>(await _pedidoService.ObterListaPedido(model.dataInicial,model.dataFinal,model.numeroPedido,
                model.nomeCliente,model.nomeEntregador,model.bairroEntrega));
            return CustomResponse(objeto);
        }


        [HttpGet("detalhes/{idpedido}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PedidoDetalhesRetornoViewModel>> ObterDetalhePedidos(string idpedido)
        {
            var _idpedido = Convert.ToInt64(idpedido);
            var model = new PedidoDetalhesRetornoViewModel();
        
            model.idPedido = idpedido;
            model.Itens = _mapper.Map<IEnumerable<PedidoItemViewModel>>(await _pedidoService.ObterListaItemPedido(_idpedido));
            model.FormasPagamento = _mapper.Map<IEnumerable<PedidoFormaPagamentoViewModel>>(await _pedidoService.ObterListaFormaPagamentoPedido(_idpedido));
                        
            return CustomResponse(model);
        }

        [HttpGet("estatisticas")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PedidosEstatisticasListaViewModel>> ObterEstatisticas()
        {
            var model = _mapper.Map<PedidosEstatisticasViewModel>(await _pedidoService.ObterEstatistica());

            return CustomResponse(model);
        }

        [HttpGet("obter-clientes-por-nome/{nome}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ClientePedidoViewModel>>> ObterCliente(string nome)
        {
            var parametro = !string.IsNullOrEmpty(nome) ? nome : "";
            var clientes = await _pedidoService.ObterTodosClientes(nome);
            var model = new List<ClientePedidoViewModel>();
            foreach (var item in clientes.ToList())
            {
                var enderecos = await _pedidoService.ObterEnderecosPorCliente(item.Id);
                var cliente = _mapper.Map<ClientePedidoViewModel>(item);
                if(enderecos.Any()) cliente.Enderecos = _mapper.Map<IEnumerable<EnderecoIndexViewModel>>(enderecos);
                model.Add(cliente);
            }           

            return CustomResponse(model);
        }

        [HttpGet("obter-clientes-todos")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ClientePedidoViewModel>>> ObterCliente()
        {
            var clientes = await _pedidoService.ObterTodosClientes("");
            var model = new List<ClientePedidoViewModel>();
            foreach (var item in clientes.ToList())
            {
                var enderecos = await _pedidoService.ObterEnderecosPorCliente(item.Id);
                var cliente = _mapper.Map<ClientePedidoViewModel>(item);
                if (enderecos.Any()) cliente.Enderecos = _mapper.Map<IEnumerable<EnderecoIndexViewModel>>(enderecos);
                model.Add(cliente);
            }

            return CustomResponse(model);
        }

        [HttpGet("obter-moedas/{idempresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MoedaViewModel>>> ObterMoedas(string idempresa)
        {
            long _id = Convert.ToInt64(idempresa);
            var model = _mapper.Map<IEnumerable<MoedaViewModel>>(await _pedidoService.ObterMoedas(_id));
            return model.ToList();
        }

        [HttpGet("obter-produtos/{descricao}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProdutoViewModel>>> ObterProdutos(string descricao)
        {
            return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _pedidoService.ObterTodosProdutos(descricao)).ToList();
        }

        [HttpGet("obter-produtos-por-descricao")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProdutoPesqViewModel>>> ObterProdutosPorDescricao([FromQuery] string descricao = null)
        {
            return _mapper.Map<IEnumerable<ProdutoPesqViewModel>>(await _pedidoService.ObterProdutosPorDescricao(descricao)).ToList();
        }

        [HttpGet("obter-produto-por-descricao-cod-barra-codigo")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProdutoPesqViewModel>> ObterProdutosPorDescricaoCodiBarraCodigo([FromQuery] string descricao = null)
        {
            return _mapper.Map<ProdutoPesqViewModel>(await _pedidoService.ObterProdutosPorDescricaoCodigoCodBarra(descricao));
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] PedidoSalvarViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var objeto = _mapper.Map<PedidoSalvarCustomViewModel>(viewModel);
            await _pedidoService.AdicionarPedido(objeto);
          
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Pedido", "Adicionar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Pedido", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> cancelar(string id)
        {
            long _id = Convert.ToInt64(id);
            
            var viewModel = await _pedidoService.ObterPorId(_id);

            if (viewModel == null)
            {
                NotificarErro("Pedido nao localizado");

                var msgErro = string.Join("\n\r", ObterNotificacoes("Pedido", "Excluir", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }   
            
            await _pedidoService.CancelarPedido(_id);
        
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Pedido", "Excluir", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"Cancelar{Deserializar(viewModel)}", "Pedido", "Cancelar", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("obter-entregadores/{idempresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<FuncionarioViewModel>>> ObterFuncionarios(string idempresa)
        {
            long _id = Convert.ToInt64(idempresa);
            var model = _mapper.Map<IEnumerable<FuncionarioViewModel>>(await _pedidoService.ObterEntregadoresPorEmpresa(_id));
            return model.ToList();
        }

        [HttpGet("definir-entregador/{idpedido}/{idfuncionario}")]
        public async Task<ActionResult> DefinirEntregador(string idpedido, string idfuncionario)
        {
            long _idpedido = Convert.ToInt64(idpedido);
            // var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(idfuncionario).Result;
            //  if (usuario == null)
            //  {
            //      NotificarErro("Erro ao tentar obert funcionario, usuario nao localizado");

            //   }
            // if (!OperacaoValida())
            //   {
            //     var msgErro = string.Join("\n\r", ObterNotificacoes("Pedido", "definir entregador", "Web", $""));
            //     return CustomResponse(msgErro);
            //   }

            long idFuncInt64 = Convert.ToInt64(idfuncionario);
           
            await _pedidoService.DefinirEntregador(_idpedido,idFuncInt64);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Pedido", "definir entregador", "Web", $"idpedido: {idpedido} idfuncionario:{idfuncionario}"));
                return CustomResponse(msgErro);
            }
            LogInformacao($"idpedido: {idpedido} idfuncionario:{idfuncionario}", "Pedido", "definir entregador", null);
            return CustomResponse();
        }

        [HttpGet("obter-pedidos-por-entregador/{idfuncionario}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PedidoPorFuncionarioViewModel>> ObterPedidosPorFuncionarios(string idfuncionario)
        {
            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(idfuncionario).Result;
            if (usuario == null)
            {
                NotificarErro("Erro ao tentar obert funcionario, usuario nao localizado");
               
            }
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Pedido", "definir entregador", "Web", $""));
                return CustomResponse(msgErro);
            }
            long _id = Convert.ToInt64(idfuncionario);
            var model = _mapper.Map<PedidoPorFuncionarioViewModel>(await _pedidoService.ObterPedidosPorFuncionario(_id));
            return model;
        }

        [HttpPost("concluir")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Concluir([FromBody] PedidoConclusaoViewModel viewModel)
        {
            var usuario = await _usuarioService.ObterPorUsuarioAspNetPorId(viewModel.idUsuario);
            if (usuario == null)
            {
                NotificarErro("Usuario não localizado");
                var msgErro = string.Join("\n\r", ObterNotificacoes("Pedido", "Concluir", "Web"));
                return CustomResponse(msgErro);
            }
;
            long idPedido = Convert.ToInt64(viewModel.idPedido);

            await _pedidoService.Concluir(idPedido, usuario.Id);
            
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Pedido", "Concluir", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Pedido", "Concluir", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PedidoSalvarViewModel>> ObterPorId(string id)
        {
            long _id = Convert.ToInt64(id);
            var model = _mapper.Map<PedidoSalvarViewModel>(await _pedidoService.ObterPedidoEditar(_id));
            return model;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] PedidoSalvarViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var objeto = _mapper.Map<PedidoSalvarCustomViewModel>(viewModel);
            await _pedidoService.AtualizarPedido(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Pedido", "Atualizar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Pedido", "Atualizar", null);
            return CustomResponse(viewModel);
        }

    }
}

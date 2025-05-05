using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Services;
using agilium.api.pdv.Controllers;
using agilium.api.pdv.ViewModels.PedidoViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using agilium.api.pdv.ViewModels.ClienteViewModel;
using AutoMapper;
using agilium.api.business.Models;
using System.Security.Cryptography;


namespace agilium.api.pdv.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/cliente")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ClienteController : MainController
    {
        private readonly IClienteService _clienteService;
        private readonly IMapper _mapper;

        public ClienteController(INotificador notificador, IUser appUser, IConfiguration configuration, 
            IUtilDapperRepository utilDapperRepository, ILogService logService, IClienteService clienteService,
            IMapper mapper) : base(notificador, appUser, configuration, utilDapperRepository, logService)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClienteBasicoViewModel>> ObterClienteBasicoPorId(string id)
        {
            var _id = Convert.ToInt64(id);
            var model = await ObterClienteBasicoPorId(_id);
            return CustomResponse(model);
        }

        private async Task<ClienteBasicoViewModel> ObterClienteBasicoPorId(long id)
        {   
            var model = new ClienteBasicoViewModel();

            var cliente = await _clienteService.ObterClienteComEnderecoPorId(id);
            if (cliente != null)
            {
                model.Id = cliente.Id.ToString();
                model.Nome = cliente.NMCLIENTE;
                if (cliente.Endereco != null)
                {
                    model.Cep = cliente.Endereco.Cep;
                    model.Logradouro = cliente.Endereco.Logradouro;
                    model.Num = cliente.Endereco.Numero;
                    model.Compl = cliente.Endereco.Complemento;
                    model.Cidade = cliente.Endereco.Cidade;
                    model.Bairro = cliente.Endereco.Bairro;
                    model.PontoReferencia = cliente.Endereco.PontoReferencia;
                    model.Uf = cliente.Endereco.Uf;
                    model.idEndereco = cliente.Endereco.Id.ToString();
                }
            }
            return model;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] ClienteBasicoViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            
            var cliente = new Cliente(string.Empty,viewModel.Nome,business.Enums.ETipoPessoa.F,0,0,0,0,business.Enums.EAtivo.Ativo);
            var endereco = new Endereco(viewModel.Logradouro,viewModel.Compl,viewModel.Bairro,viewModel.Cep,viewModel.Cidade,viewModel.Uf,"Brasil",null,viewModel.PontoReferencia,viewModel.Num);

            if(!string.IsNullOrEmpty(viewModel.Cpf.Trim()))
            {
                var clienteExistente = (await _clienteService.ObterClientePorCpf(viewModel.Cpf.Replace(".", "").Replace("-", "")));
                if (clienteExistente != null)
                {
                    return CustomResponse(await ObterClienteBasicoPorId(clienteExistente.Id));
                }

            }

            cliente.AdicionarEndereco(endereco);
            
            var idNovoCliente = await _clienteService.AdicionarClienteBasico(cliente, viewModel.Cpf);
            var model = await ObterClienteBasicoPorId(idNovoCliente);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Cliente", "Adicionar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Cliente", "Adicionar", null);
            return CustomResponse(model);
        }

        [HttpGet("obter-por-cpf/{cpf}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClienteBasicoViewModel>> ObterClienteBasicoPorCpf(string cpf)
        {
            var model = new ClienteBasicoViewModel();

            var cliente = await _clienteService.ObterClientePorCpf(cpf.Replace(".","").Replace("-",""));
            if (cliente != null)
            {
                model.Id = cliente.Id.ToString();
                model.Nome = cliente.NMCLIENTE;
                if (cliente.Endereco != null)
                {
                    model.Cep = cliente.Endereco.Cep;
                    model.Logradouro = cliente.Endereco.Logradouro;
                    model.Num = cliente.Endereco.Numero;
                    model.Compl = cliente.Endereco.Complemento;
                    model.Cidade = cliente.Endereco.Cidade;
                    model.Bairro = cliente.Endereco.Bairro;
                    model.PontoReferencia = cliente.Endereco.PontoReferencia;
                    model.Uf = cliente.Endereco.Uf;
                    model.idEndereco = cliente.Endereco.Id.ToString();
                }
            }
            return CustomResponse(model);
        }
    }
}

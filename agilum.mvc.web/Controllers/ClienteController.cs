using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilum.mvc.web.Services;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Cliente;
using agilum.mvc.web.ViewModels.Endereco;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Route("cliente")]
    [Authorize]
    public class ClienteController : MainController
    {
        #region constante
        private readonly IClienteService _clienteService;
        private readonly IContatoService _contatoService;
        private readonly IProdutoService _produtoService;
        private readonly IEnderecoService _enderecoService;
        private readonly string _nomeEntidadeMotivo = "Cliente";
        #endregion

        #region construtor
        public ClienteController(IClienteService clienteService, IEnderecoService enderecoService, IContatoService contatoService, IProdutoService produtoService,
            INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService, IMapper mapper) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper)
        {
            _clienteService = clienteService;
            _contatoService = contatoService;
            _produtoService = produtoService; 
            _enderecoService = enderecoService;
        }
        #endregion

        #region cliente

        [Route("lista")]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
        {

            var lista = await ObterListaPaginado(q, page, ps);
            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "lista";
            return View(lista);
        }

        [Route("novo")]
        [HttpGet]
        public async Task<IActionResult> CreateCliente()
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidadeMotivo}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidadeMotivo;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeMotivo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            ObterEstados();
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateCliente";

            var model = new ClienteViewModel();
            model.Situacao = EAtivo.Ativo;
            model.TipoPessoa = ETipoPessoa.F;
            model.PublicaEmail = ESimNao.Nao;
            model.PublicaSMS = ESimNao.Nao;

            model.Endereco.Id = 0;
            model.EnderecoCobranca.Id = 0;
            model.EnderecoEntrega.Id = 0;
            model.EnderecoFaturamento.Id = 0;
            
            InicializarObjetos(model);

            return View("CreateEditCliente", model);
        }

        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> CreateCliente(ClienteViewModel model)
        {
            InicializarObjetos(model);
            ObterEstados();
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateCliente";
            if (!ModelState.IsValid) return View("CreateEditCliente", model);

            if (model.Endereco != null && !string.IsNullOrEmpty(model.Endereco.Cep))
                model.Endereco.Cep = RetirarPontos(model.Endereco.Cep);

            if (model.EnderecoCobranca != null && !string.IsNullOrEmpty(model.EnderecoCobranca.Cep))
                model.EnderecoCobranca.Cep = RetirarPontos(model.EnderecoCobranca.Cep);

            if (model.EnderecoEntrega != null && !string.IsNullOrEmpty(model.EnderecoEntrega.Cep))
                model.EnderecoEntrega.Cep = RetirarPontos(model.EnderecoEntrega.Cep);

            if (model.EnderecoFaturamento != null && !string.IsNullOrEmpty(model.EnderecoFaturamento.Cep))
                model.EnderecoFaturamento.Cep = RetirarPontos(model.EnderecoFaturamento.Cep);


            Cliente cliente = await ValidarCliente(model);
           
            if (cliente.Id == 0) cliente.Id = cliente.GerarId();

            await _clienteService.Adicionar(cliente);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Cliente", "Adicionar", "Web");
                AdicionarErroValidacao(msgErro);
                return View("CreateEditCliente", model);
            }
            await _clienteService.Salvar();
            LogInformacao($"Objeto adicionado com sucesso {Deserializar(cliente)}", "Cliente", "Adicionar", null);
                      
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [HttpGet]
        public async Task<IActionResult> EditCliente(long id)
        {

            ObterEstados();
            ViewBag.operacao = "E";
            ViewBag.acao = "EditCliente";
            var objeto = await Obter(id.ToString());
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeMotivo} não localizado";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeMotivo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            InicializarObjetos(objeto);
            return View("CreateEditCliente", objeto);
        }

        [Route("editar")]
        [HttpPost]
        public async Task<IActionResult> EditCliente(ClienteViewModel model)
        {
            ObterEstados();
            ViewBag.operacao = "E";
            ViewBag.acao = "EditCliente";

            if (!ModelState.IsValid) return View("CreateEditCliente", model);

            if (model.Endereco != null && !string.IsNullOrEmpty(model.Endereco.Cep))
                model.Endereco.Cep = RetirarPontos(model.Endereco.Cep);

            if (model.EnderecoCobranca != null && !string.IsNullOrEmpty(model.EnderecoCobranca.Cep))
                model.EnderecoCobranca.Cep = RetirarPontos(model.EnderecoCobranca.Cep);

            if (model.EnderecoEntrega != null && !string.IsNullOrEmpty(model.EnderecoEntrega.Cep))
                model.EnderecoEntrega.Cep = RetirarPontos(model.EnderecoEntrega.Cep);

            if (model.EnderecoFaturamento != null && !string.IsNullOrEmpty(model.EnderecoFaturamento.Cep))
                model.EnderecoFaturamento.Cep = RetirarPontos(model.EnderecoFaturamento.Cep);

            InicializarObjetos(model);

            Cliente cliente = await ValidarCliente(model);

            await _clienteService.Atualizar(cliente);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Cliente", "Atualizar", "Web");
                AdicionarErroValidacao(msgErro);
                return View("CreateEditCliente", model);
            }
            await _clienteService.Salvar();
            LogInformacao($"Objeto adicionado com sucesso {Deserializar(cliente)}", "Cliente", "Atualizar", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [HttpGet]
        public async Task<IActionResult> DeleteCliente(long id)
        {
            ObterEstados();
            var objeto = await Obter(id.ToString());
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeMotivo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeMotivo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View(objeto);
        }


        [Route("apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteCliente(ClienteViewModel model)
        {
            ObterEstados();
            if (!ModelState.IsValid) return View(model);

            await _clienteService.Apagar(model.Id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Cliente", "Excluir", "Web");
                AdicionarErroValidacao(msgErro);
                return View(model);
            }
            await _clienteService.Salvar();
            LogInformacao($"Objeto excluido com sucesso id:{model.Id}", "Cliente", "Excluir", null);
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }
        #endregion

        #region contato cliente
        
        [Route("AdicionarContato")]
        public async Task<IActionResult> AdicionarContato(long idCliente)
        {
            ViewBag.acao = "AdicionarContato";
            ViewBag.operacao = "I";

            var model = new ClienteContatoViewModel();
            model.IDCLIENTE = idCliente;

            await PreencherViewBagNomeFornecedor(idCliente);


            return View("_createContato", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AdicionarContato")]
        public async Task<IActionResult> AdicionarContato(ClienteContatoViewModel model)
        {
            ViewBag.acao = "AdicionarContato";
            ViewBag.operacao = "I";
            
            await PreencherViewBagNomeFornecedor(model.IDCLIENTE);

            if (!ModelState.IsValid) return View("_createContato", model);


            var fornecedorContato = _mapper.Map<ClienteContato>(model);

            if (fornecedorContato.Contato.Id == 0)
                fornecedorContato.Contato.Id = await GerarId();

            await _clienteService.AdicionarContato(fornecedorContato);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Cliente", "AdicionarContato", "Web");
                return View("_createContato",model);
            }
            await _clienteService.Salvar();

            LogInformacao($"Objeto adicionado com sucesso {Deserializar(fornecedorContato)}", "Cliente", "AdicionarContato", null);

            //var url = Url.Action("ObterEndereco", "Cliente", new { id = model.IDCLIENTE });
            return RedirectToAction("EditCliente", new { id = model.IDCLIENTE });
        }

        [Route("EditarContato")]
        public async Task<IActionResult> EditarContato(long idCliente, long idContato)
        {
            ViewBag.acao = "EditarContato";
            ViewBag.operacao = "E";
            await PreencherViewBagNomeFornecedor(idCliente);

            var contatoCliente = await ObterPorId(idContato, idCliente);
            if (contatoCliente == null)
            {
                ViewBag.operacao = "E";
                ViewBag.acao = "EditCliente";

                return RedirectToAction("CreateEditCliente", new { id = idCliente });
            }

            return View("_createContato", contatoCliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("EditarContato")]
        public async Task<IActionResult> EditarContato(ClienteContatoViewModel model)
        {
            ViewBag.acao = "EditarContato";
            ViewBag.operacao = "E";

            if (!ModelState.IsValid)
            {
                return View("_createContato", model);
            }
            if (model.Contato.Id == 0)
                model.Contato.Id = model.IDCONTATO;

            var fornecedorContato = _mapper.Map<ClienteContato>(model);

            await _clienteService.Atualizar(fornecedorContato);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Cliente", "AtualizarContato", "Web");

                return View("_createContato", model);
            }
            await _clienteService.Salvar();
            LogInformacao($"Objeto atualizado com sucesso {Deserializar(model)}", "Cliente", "AtualiarContato", null);

            return RedirectToAction("EditCliente", new { id = model.IDCLIENTE });
        }

        [Route("DeleteContato")]
        
        public async Task<IActionResult> DeleteContato(long idCliente, long idContato)
        {
            var contatoCliente = await ObterPorId(idContato, idCliente);
            if (contatoCliente == null)
            {
                var msg = "Erro ao tentar remover endereço contato!";
                AdicionarErroValidacao(msg);

                return RedirectToAction("CreateEditCliente", new { id = idCliente });
            }
          
            return View(contatoCliente);
        }

        [Route("DeleteContato")]
        [HttpPost]
        public async Task<IActionResult> DeleteContato(ClienteContatoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _clienteService.RemoverContato(model.IDCLIENTE,model.IDCONTATO);
            await _clienteService.Salvar();
            if (!OperacaoValida())
            {
                AdicionarErroValidacao("Erro ao tentar remover endereço contato!");
                return View(model);
            }
            return RedirectToAction("EditCliente", new { id = model.IDCLIENTE });
        }

      


        #endregion

        #region ViewBag
        private void ObterEstados()
        {
            var estados = ListasAuxilares.ObterEstados();
            ViewBag.estados = new SelectList(estados, "Sigla", "Nome", "");
        }

        private async Task PreencherViewBagNomeFornecedor(long idcliente)
        {
            var fornecedor = await Obter(idcliente.ToString());
            if (fornecedor != null)
            {
                ViewBag.nome = fornecedor.Nome;
            }
        }
        #endregion

        #region metodos privados
        private async Task<PagedViewModel<ClienteViewModel>> ObterListaPaginado(string filtro, int page, int pageSize)
        {
            var retorno = await _clienteService.ObterPorNomePaginacao(filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<ClienteViewModel>>(retorno.List);

            return new PagedViewModel<ClienteViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "Index",
                TotalResults = retorno.TotalResults
            };
        }

        private void InicializarObjetos(ClienteViewModel model)
        {
            if (model.Endereco.Id == null) model.Endereco.Id = 0;
            if (model.EnderecoCobranca.Id == null) model.EnderecoCobranca.Id = 0;
            if (model.EnderecoFaturamento.Id == null) model.EnderecoFaturamento.Id = 0;
            if (model.EnderecoEntrega.Id == null) model.EnderecoEntrega.Id = 0;
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
                    viewModel.EnderecoFaturamento.Id = await GerarId();
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

            if (viewModel.TipoPessoa == agilium.api.business.Enums.ETipoPessoa.F && viewModel.ClientePessoaFisica != null)
            {
                if (viewModel.ClientePessoaFisica.Id == 0) viewModel.ClientePessoaFisica.Id = viewModel.Id;
                var clientePF = _mapper.Map<ClientePF>(viewModel.ClientePessoaFisica);
                cliente.AdicionarPessoaFisica(clientePF);
            }
            else if (viewModel.TipoPessoa == agilium.api.business.Enums.ETipoPessoa.J && viewModel.ClientePessoaJuridica != null)
            {
                if (viewModel.ClientePessoaJuridica.Id == 0) viewModel.ClientePessoaJuridica.Id = viewModel.Id;
                var clientePJ = _mapper.Map<ClientePJ>(viewModel.ClientePessoaJuridica);
                cliente.AdicionarPessoaJuridica(clientePJ);
            }

            return cliente;
        }

        public async Task<ClienteViewModel> Obter(string id)
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

                return objeto;
            }

            return null;
        }


        private async Task<ClienteContatoViewModel> ObterPorId([FromQuery] long idContato, [FromQuery] long idCliente)
        {
            var objeto = _mapper.Map<ClienteContatoViewModel>(await _clienteService.ObterClienteContatoPorId(idCliente, idContato));
            return objeto;
        }
        #endregion
    }
}

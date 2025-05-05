using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.Services;
using agilium.webapp.manager.mvc.ViewModels.Cliente;
using agilium.webapp.manager.mvc.ViewModels.Contato;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    [Route("cliente")]
    public class ClienteController : MainController
    {
        private readonly IClienteService _clienteService;
        private readonly IContatoService _contatoService;
        private readonly IProdutoService _produtoService;
        private readonly string _nomeEntidadeMotivo = "Cliente";

        public ClienteController(IClienteService clienteService, IContatoService contatoService,IProdutoService produtoService)
        {
            _clienteService = clienteService;
            _contatoService = contatoService;
            _produtoService = produtoService;
        }


        #region Cliente
        
        [Route("lista")]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
        {

            var lista = await _clienteService.ObterPorNome(q, page, ps);
            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "Index";
            return View(lista);
        }

        [Route("novo")]
        [HttpGet]
        public async Task<IActionResult> CreateCliente()
        {            
            ObterEstados();
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateCliente";

            var model = new ClienteViewModel();
            model.Situacao = Enums.EAtivo.Ativo;
            model.TipoPessoa = Enums.ETipoPessoa.F;
            model.PublicaEmail= Enums.ESimNao.Nao;
            model.PublicaSMS = Enums.ESimNao.Nao;

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

            //if (model.TipoPessoa == Enums.ETipoPessoa.F)
            //    model.ClientePessoaJuridica = null;
            //else
            //    model.ClientePessoaFisica = null;
            
            var resposta = await _clienteService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditCliente", model);
            }
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
            var objeto = await _clienteService.ObterPorId(id);
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

            var resposta = await _clienteService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditCliente", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        private void InicializarObjetos(ClienteViewModel model)
        {
            if (model.Endereco.Id == null) model.Endereco.Id = 0;
            if (model.EnderecoCobranca.Id == null) model.EnderecoCobranca.Id = 0;
            if (model.EnderecoFaturamento.Id == null) model.EnderecoFaturamento.Id = 0;
            if (model.EnderecoEntrega.Id == null) model.EnderecoEntrega.Id = 0;
        }

        [Route("apagar")]
        [HttpGet]
        public async Task<IActionResult> DeleteCliente(long id)
        {
            ObterEstados();
            var objeto = await _clienteService.ObterPorId(id);
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

            var resposta = await _clienteService.Remover(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }
        #endregion

        #region ClienteContato
        
        [Route("AdicionarContato")]
        public async Task<IActionResult> AdicionarContato(long idCliente)
        {
            ViewBag.acao = "AdicionarContato";
            ViewBag.operacao = "I";

            var model = new ClienteContatoViewModel();
            model.IDCLIENTE = idCliente;

            return PartialView("_createContato", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AdicionarContato")]
        public async Task<IActionResult> AdicionarContato(ClienteContatoViewModel model)
        {
            ViewBag.acao = "AdicionarContato";
            ViewBag.operacao = "I";

            if (!ModelState.IsValid) return PartialView("_createContato", model);

            await _clienteService.Adicionar(model);

            if (!OperacaoValida()) return PartialView("_createContato", model);

            var url = Url.Action("ObterEndereco", "Cliente", new { id = model.IDCLIENTE });


            return Json(new { success = true, url });
        }

        [Route("DeleteContato")]
        public async Task<IActionResult> DeleteContato(long idCliente, long idContato)
        {
            var contatoCliente = await _clienteService.ObterPorId(idContato, idCliente);
            if (contatoCliente == null)
            {
                var msg = "Erro ao tentar remover endereço contato!";
                return Json(new { erro = msg });
            }

            var url = Url.Action("ObterEndereco", "Cliente", new { id = idCliente });
            await _clienteService.RemoverContato(idContato, idCliente);
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

        [Route("EditarContato")]
        public async Task<IActionResult> EditarContato(long idCliente, long idContato)
        {
            ViewBag.acao = "EditarContato";
            ViewBag.operacao = "E";

            var contatoCliente = await _clienteService.ObterPorId(idContato, idCliente);
            if (contatoCliente == null)
            {
                return NotFound();
            }

            return PartialView("_createContato", contatoCliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("EditarContato")]
        public async Task<IActionResult> EditarContato(ClienteContatoViewModel model)
        {
            ViewBag.acao = "EditarContato";
            ViewBag.operacao = "E";

            if (!ModelState.IsValid) return PartialView("_createContato", model);
            if (model.Contato.Id == 0)
                model.Contato.Id = model.IDCONTATO;
            await _clienteService.Atualizar(model);

            if (!OperacaoValida()) return PartialView("_createContato", model);

            var url = Url.Action("ObterEndereco", "Cliente", new { id = model.IDCLIENTE });

            return Json(new { success = true, url });
        }

        [Route("ObterEndereco")]
        public async Task<IActionResult> ObterEndereco(long id)
        {
            var cliente = await _clienteService.ObterPorId(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return PartialView("_contatos", cliente);
        }
        #endregion

        #region ClientePreco
        [Route("preco")]
        public async Task<IActionResult> ListaPreco(long idProduto)
        {
            var produto = _produtoService.ObterProdutoPorId(idProduto).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;
            
            var precos = _clienteService.ObterClientePrecoPorProduto(idProduto).Result;

            return View("_ProdutoPreco", precos);
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

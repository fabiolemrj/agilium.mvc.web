using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.Services;
using agilium.webapp.manager.mvc.ViewModels.Contato;
using agilium.webapp.manager.mvc.ViewModels.Fornecedor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    [Route("fornecedor")]
    public class FornecedorController : MainController
    {
        private readonly IFornecedorService _fornecedorService;
        private readonly IContatoService _contatoService;
        private readonly string _nomeEntidadeMotivo = "Fornecedor";

        public FornecedorController(IFornecedorService fornecedorService, IContatoService contatoService)
        {
            _fornecedorService = fornecedorService;
            _contatoService = contatoService;
        }

        #region endpoints
        [Route("lista")]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
        {

            var lista = await _fornecedorService.ObterPorRazaoSocial(q, page, ps);
            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "Index";
            return View(lista);
        }

        [Route("novo")]
        [HttpGet]
        public async Task<IActionResult> CreateFornecedor()
        {
            ObterEstados();
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateFornecedor";

            var model = new FornecedorViewModel();
            model.Situacao = Enums.EAtivo.Ativo;
            model.TipoPessoa = Enums.ETipoPessoa.F;
            model.TipoFiscal = Enums.ETipoFiscal.SimplesNacional;
            model.Endereco.Id = 0;

            return View("CreateEditFornecedor", model);
        }

        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> CreateFornecedor(FornecedorViewModel model)
        {
            ObterEstados();
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateFornecedor";
            if (!ModelState.IsValid) return View("CreateEditFornecedor", model);

            if (!string.IsNullOrEmpty(model.CpfCnpj))
                model.CpfCnpj = RetirarPontos(model.CpfCnpj);

            if (model.Endereco != null && !string.IsNullOrEmpty(model.Endereco.Cep))
                model.Endereco.Cep = RetirarPontos(model.Endereco.Cep);

            var resposta = await _fornecedorService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo {_nomeEntidadeMotivo}" };
                
                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditFornecedor", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [HttpGet]
        public async Task<IActionResult> EditFornecedor(long id)
        {
            ObterEstados();
            ViewBag.operacao = "E";
            ViewBag.acao = "EditFornecedor";
            var objeto = await _fornecedorService.ObterPorId(id);
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

            return View("CreateEditFornecedor", objeto);
        }

        [Route("editar")]
        [HttpPost]
        public async Task<IActionResult> EditFornecedor(FornecedorViewModel model)
        {
            ObterEstados();
            ViewBag.operacao = "E";
            ViewBag.acao = "EditFornecedor";

            if (!ModelState.IsValid) return View("CreateEditFornecedor", model);

            if(!string.IsNullOrEmpty(model.CpfCnpj))
                model.CpfCnpj = RetirarPontos(model.CpfCnpj);
            
            if (model.Endereco != null && !string.IsNullOrEmpty(model.Endereco.Cep))
                model.Endereco.Cep = RetirarPontos(model.Endereco.Cep);

            var resposta = await _fornecedorService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditFornecedor", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [HttpGet]
        public async Task<IActionResult> DeleteFornecedor(long id)
        {
            ObterEstados();
            var objeto = await _fornecedorService.ObterPorId(id);
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
        public async Task<IActionResult> DeleteFornecedor(FornecedorViewModel model)
        {
            ObterEstados();
            if (!ModelState.IsValid) return View(model);

            var resposta = await _fornecedorService.Remover(model.Id);

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

        #region Contatos
        [Route("AdicionarContato")]
        public async Task<IActionResult> AdicionarContato(long idFornecedor)
        {
            ViewBag.acao = "AdicionarContato";
            ViewBag.operacao = "I";

            var model = new ContatoFornecedorViewModel();
            model.IDFORN = idFornecedor;

            return PartialView("_createContato", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AdicionarContato")]
        public async Task<IActionResult> AdicionarContato(ContatoFornecedorViewModel model)
        {
            ViewBag.acao = "AdicionarContato";
            ViewBag.operacao = "I";

            if (!ModelState.IsValid) return PartialView("_createContato", model);

            await _fornecedorService.Adicionar(model);

            if (!OperacaoValida()) return PartialView("_createContato", model);

            var url = Url.Action("ObterEndereco", "Fornecedor", new { id = model.IDFORN });


            return Json(new { success = true, url });
        }

        [Route("DeleteContato")]
        public async Task<IActionResult> DeleteContato(long idFornecedor, long idContato)
        {
            var contatoEmpresa = await _fornecedorService.ObterPorId(idContato, idFornecedor);
            if (contatoEmpresa == null)
            {
                var msg = "Erro ao tentar remover endereço contato!";
                return Json(new { erro = msg });
            }

            var url = Url.Action("ObterEndereco", "Fornecedor", new { id = idFornecedor });
            await _fornecedorService.RemoverContato(idContato, idFornecedor);
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
        public async Task<IActionResult> EditarContato(long idFornecedor, long idContato)
        {
            ViewBag.acao = "EditarContato";
            ViewBag.operacao = "E";

            var contatoEmpresa = await _fornecedorService.ObterPorId(idContato, idFornecedor);
            if (contatoEmpresa == null)
            {
                return NotFound();
            }

            return PartialView("_createContato", contatoEmpresa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("EditarContato")]
        public async Task<IActionResult> EditarContato(ContatoFornecedorViewModel model)
        {
            ViewBag.acao = "EditarContato";
            ViewBag.operacao = "E";

            if (!ModelState.IsValid) return PartialView("_createContato", model);
            if (model.Contato.Id == 0)
                model.Contato.Id = model.IDCONTATO;
            await _fornecedorService.Atualizar(model);

            if (!OperacaoValida()) return PartialView("_createContato", model);

            var url = Url.Action("ObterEndereco", "Fornecedor", new { id = model.IDFORN});

            return Json(new { success = true, url });
        }

        [Route("ObterEndereco")]
        public async Task<IActionResult> ObterEndereco(long id)
        {
            var empresa = await _fornecedorService.ObterPorId(id);

            if (empresa == null)
            {
                return NotFound();
            }

            return PartialView("_contatos", empresa);
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


using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.Services;
using agilium.webapp.manager.mvc.ViewModels.Contato;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using agilium.webapp.manager.mvc.ViewModels.Endereco;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace agilium.webapp.manager.mvc.Controllers
{

    public class EmpresaController : MainController
    {
        private readonly IEmpresaService _empresaService;
        private readonly IContatoService _contatoService;
        private readonly ILogger<EmpresaController> _logger;
        private readonly string _nomeEntidade = "Empresa";

        public EmpresaController(IEmpresaService empresaService, ILogger<EmpresaController> logger,
            IContatoService contatoService)
        {
            _empresaService = empresaService;
            _logger = logger;
            _contatoService = contatoService;
        }

        #region endPoints
        [Route("empresa")]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
        
            var lista = await _empresaService.ObterEmpresasPorRazaoSocial(q, page, ps);
            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "Index";
            return View(lista);
        }

        [Route("nova")]
        public async Task<IActionResult> Create()
        {
            ObterEstados();
            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            var model = new EmpresaCreateViewModel();
            model.TPEMPRESA = Enums.ETipoEmpresa.Loja;
            model.STLUCROPRESUMIDO = Enums.ESimNao.Sim;
            model.STMICROEMPRESA = Enums.ESimNao.Nao;
            model.Endereco.Id = 0;

            //AdicionarTeste(model);
            return View(model);
        }

    
   

        [Route("nova")]
        [HttpPost]
        public async Task<IActionResult> Create(EmpresaCreateViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            if (!ModelState.IsValid) return View(model);

            model.NUCNPJ = RetirarPontos(model.NUCNPJ);
            
            if(model.Endereco != null && !string.IsNullOrEmpty(model.Endereco.Cep))
                model.Endereco.Cep = RetirarPontos(model.Endereco.Cep);

            var resposta = await _empresaService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                ObterEstados();

                var retornoErro = new { mensagem = "Erro ao criar nova empresa"};
                _logger.LogError(retornoErro.ToString());
                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Edit(long id)
        {
            ObterEstados();
            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";
            var objeto = await _empresaService.ObterPorId(id.ToString());
            if(objeto == null)
            {
                var msgErro = $"{_nomeEntidade} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexPagination");
            }
            
            return View("Create",objeto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmpresaCreateViewModel model)
        {
            ObterEstados();
            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            if (!ModelState.IsValid) return View("Create", model);

            model.NUCNPJ = RetirarPontos(model.NUCNPJ);

            if (model.Endereco != null && !string.IsNullOrEmpty(model.Endereco.Cep))
                model.Endereco.Cep = RetirarPontos(model.Endereco.Cep);

            var resposta = await _empresaService.Atualizar(model.Id,model);

            if (ResponsePossuiErros(resposta))
            {            
                var retornoErro = new { mensagem = $"Erro ao editar nova {_nomeEntidade}" };
                _logger.LogError(retornoErro.ToString());
                AdicionarErroValidacao(retornoErro.mensagem);
                return View("Create", model);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(long id)
        {
            var objeto = await _empresaService.ObterPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidade} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexPagination");
            }

            return View(objeto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EmpresaCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var resposta = await _empresaService.Remover(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar nova {_nomeEntidade}" };
                _logger.LogError(retornoErro.ToString());
                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AdicionarContato(long idEmpresa)
        {
            ViewBag.acao = "AdicionarContato";
            ViewBag.operacao = "I";

            var model = new ContatoEmpresaViewModel();
            model.IDEMPRESA = idEmpresa;

            return PartialView("_createContato", model);
        }

        public async Task<IActionResult> EditarContato(long idEmpresa, long idContato)
        {
            ViewBag.acao = "EditarContato";
            ViewBag.operacao = "E";

            var contatoEmpresa = await _contatoService.ObterPorId(idContato, idEmpresa);
            if(contatoEmpresa == null)
            {
                return NotFound();
            }
            
            return PartialView("_createContato", contatoEmpresa);
        }

        public async Task<IActionResult> DeleteContato(long idEmpresa, long idContato)
        {
            var contatoEmpresa = await _contatoService.ObterPorId(idContato, idEmpresa);
            if (contatoEmpresa == null)
            {
                var msg = "Erro ao tentar remover endereço contato!";
                return Json(new { erro = msg });
            }

            var url = Url.Action("ObterEndereco", "Empresa", new { id = idEmpresa });
            await _contatoService.RemoverContatoEmpresa(idContato,idEmpresa);
            if (!OperacaoValida()) 
            {
                AdicionarErroValidacao("Erro ao tentar remover endereço contato!");
                var msgErro = string.Join("\n\r", ModelState.Values
                              .SelectMany(x => x.Errors)
                              .Select(x => x.ErrorMessage));

                return Json(new { erro = msgErro});
            }

            return Json(new { success = true, url });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarContato(ContatoEmpresaViewModel model)
        {
            ViewBag.acao = "EditarContato";
            ViewBag.operacao = "E";

            if (!ModelState.IsValid) return PartialView("_createContato", model);
            if (model.Contato.Id == 0)
                model.Contato.Id = model.IDCONTATO;
            await _contatoService.Atualizar(model);

            if (!OperacaoValida()) return PartialView("_createContato", model);

            var url = Url.Action("ObterEndereco", "Empresa", new { id = model.IDEMPRESA });


            return Json(new { success = true, url });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarContato(ContatoEmpresaViewModel model)
        {
            ViewBag.acao = "AdicionarContato";
            ViewBag.operacao = "I";

            if (!ModelState.IsValid) return PartialView("_createContato", model);
           
            await _contatoService.Adicionar(model);

            if (!OperacaoValida()) return PartialView("_createContato", model);

            var url = Url.Action("ObterEndereco", "Empresa", new { id = model.IDEMPRESA });
            

            return Json(new { success = true, url });
        }

        [AllowAnonymous]
        //[Route("obter-contato/{id}")]
        public async Task<IActionResult> ObterEndereco(long id)
        {
            var empresa = await _empresaService.ObterPorId(id.ToString());

            if (empresa == null)
            {
                return NotFound();
            }

            return PartialView("_contato", empresa);
        }
        //public async Task<ActionResult> AdicionarContato([FromQuery] string tpcontato, [FromQuery] string descr1, [FromQuery] string descr2)
        //{

        //}
        #endregion

        #region ViewBag
        private void ObterEstados()
        {
            var estados = ListasAuxilares.ObterEstados();
            ViewBag.estados = new SelectList(estados, "Sigla", "Nome", "");
        }

        private void AdicionarTeste(EmpresaCreateViewModel model)
        {
            model.CDEMPRESA = "0004";
            model.NMFANTASIA = "Teste2";
            model.NMRZSOCIAL = "Teste2";
            model.TPEMPRESA = Enums.ETipoEmpresa.Loja;
            model.STLUCROPRESUMIDO = Enums.ESimNao.Nao;
            model.STMICROEMPRESA = Enums.ESimNao.Nao;
            model.NUCAPARM = 0;
            model.NUCNPJ = "47.527.868/0001-08";
            var endereco = new EnderecoIndexViewModel();

            endereco.Logradouro = "logra";
            endereco.Cep = "202239";
            endereco.Bairro = "bairro";
            endereco.Complemento = "compl";
            endereco.Numero = "0";
            endereco.Cidade = "cidade";
            endereco.Id = 0;
            model.Endereco = endereco;
            // var resposta = _empresaService.Adicionar(model);
        }
        #endregion
    }
}

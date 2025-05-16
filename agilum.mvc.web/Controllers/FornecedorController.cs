using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilum.mvc.web.Services;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Contato;
using agilum.mvc.web.ViewModels.Endereco;
using agilum.mvc.web.ViewModels.Fornecedor;
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
    [Route("fornecedor")]
    [Authorize]
    public class FornecedorController : MainController
    {
        #region constantes
        private readonly IFornecedorService _fornecedorService;
        private readonly IContatoService _contatoService;
        private readonly string _nomeEntidadeMotivo = "Fornecedor";


        #endregion

        #region construtores
        public FornecedorController(IFornecedorService fornecedorService, IContatoService contatoService,
            INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService, IMapper mapper) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper)
        {

            _fornecedorService = fornecedorService;
            _contatoService = contatoService;
        }
        #endregion

        #region fornecedor
        [Route("lista")]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
        {

            var lista = await ObterListaPaginado(q, page, ps);
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
            model.Situacao = EAtivo.Ativo;
            model.TipoPessoa = ETipoPessoa.F;
            model.TipoFiscal = ETipoFiscal.SimplesNacional;
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

            var fornecedor = _mapper.Map<Fornecedor>(model);

            if (fornecedor.Id == 0) fornecedor.Id = fornecedor.GerarId();

            if (model.Endereco != null)
            {
                if (fornecedor.Endereco.Id == 0) fornecedor.Endereco.Id = await GerarId();
            }

            await _fornecedorService.Adicionar(fornecedor);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Fornecedor", "Adicionar", "Web", Deserializar(fornecedor)));
                AdicionarErroValidacao(msgErro);
                return View("CreateEditFornecedor", model);
            }
            await _fornecedorService.Salvar();
            LogInformacao($"sucesso: {Deserializar(fornecedor)}", "Fornecedor", "Adicionar", null);
           
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

            if (!string.IsNullOrEmpty(model.CpfCnpj))
                model.CpfCnpj = RetirarPontos(model.CpfCnpj);

            if (model.Endereco != null && !string.IsNullOrEmpty(model.Endereco.Cep))
                model.Endereco.Cep = RetirarPontos(model.Endereco.Cep);

            await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(model));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Fornecedor", "Atualizar", "Web", Deserializar(model)));
                AdicionarErroValidacao(msgErro);
                return View("CreateEditFornecedor", model);
            }
            await _fornecedorService.Salvar();
            LogInformacao($"sucesso: {Deserializar(model)}", "Fornecedor", "Atualizar", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [HttpGet]
        public async Task<IActionResult> DeleteFornecedor(long id)
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
        public async Task<IActionResult> DeleteFornecedor(FornecedorViewModel model)
        {
            ObterEstados();
            if (!ModelState.IsValid) return View(model);

            await _fornecedorService.Apagar(model.Id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Fornecedor", "Excluir", "Web", Deserializar(model)));
                AdicionarErroValidacao(msgErro);
                return View(model);
            }
            await _fornecedorService.Salvar();
            LogInformacao($"sucesso: {Deserializar(model)}", "Fornecedor", "Excluir", null);
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }
        #endregion

        #region contato e endereço
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

            var fornecedorContato = _mapper.Map<FornecedorContato>(model);

            if (fornecedorContato.Contato.Id == 0)
                fornecedorContato.Contato.Id = await GerarId();

            await _fornecedorService.AdicionarContato(fornecedorContato);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Fornecedor", "AdicionarContato", "Web", Deserializar(model)));
                PartialView("_createContato", model);
            }
            await _fornecedorService.Salvar();
            LogInformacao($"sucesso: {Deserializar(fornecedorContato)}", "Fornecedor", "AdicionarContato", null);

            var url = Url.Action("ObterEndereco", "Fornecedor", new { id = model.IDFORN });

            return Json(new { success = true, url });
        }

        [Route("EditarContato")]
        public async Task<IActionResult> EditarContato(long idFornecedor, long idContato)
        {
            ViewBag.acao = "EditarContato";
            ViewBag.operacao = "E";

            var contatoEmpresa = await ObterContatoPorId(idContato, idFornecedor);
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
            var fornecedorContato = _mapper.Map<FornecedorContato>(model);

            await _fornecedorService.Atualizar(fornecedorContato);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Fornecedor", "AtualizarContatoEmpresa", "Web"));
                PartialView("_createContato", model);
            }
            await _fornecedorService.Salvar();
            LogInformacao($"sucesso: idcontato {Deserializar(fornecedorContato)}", "Fornecedor", "AtualizarContatoEmpresa", null);
            var url = Url.Action("ObterEndereco", "Fornecedor", new { id = model.IDFORN });

            return Json(new { success = true, url });
        }

        [Route("DeleteContato")]
        public async Task<IActionResult> DeleteContato(long idFornecedor, long idContato)
        {
            var contatoEmpresa = await ObterContatoPorId(idContato, idFornecedor);
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
        #endregion

        #region Metodos Privados
        private async Task<PagedViewModel<FornecedorViewModel>> ObterListaPaginado(string filtro, int page, int pageSize)
        {
            var retorno = await _fornecedorService.ObterPorRazaoSocialPaginacao(filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<FornecedorViewModel>>(retorno.List);

            return new PagedViewModel<FornecedorViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "Index",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<FornecedorViewModel> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var fornecedor = await _fornecedorService.ObterCompletoPorId(_id);
            if (fornecedor != null)
            {
                var objeto = _mapper.Map<FornecedorViewModel>(fornecedor);
                objeto.Endereco = _mapper.Map<EnderecoIndexViewModel>(fornecedor.Endereco);
                objeto.Contatos = _mapper.Map<List<ContatoFornecedorViewModel>>(fornecedor.FornecedoresContatos);
                return objeto;
            }
            return new FornecedorViewModel();

        }

        private async Task<ContatoFornecedorViewModel> ObterContatoPorId([FromQuery] long idContato, [FromQuery] long idFornecedor)
        {
            var objeto = _mapper.Map<ContatoFornecedorViewModel>(await _fornecedorService.ObterFornecedorContatoPorId(idFornecedor, idContato));
            return objeto;
        }


        private void ObterEstados()
        {
            var estados = ListasAuxilares.ObterEstados();
            ViewBag.estados = new SelectList(estados, "Sigla", "Nome", "");
        }
        #endregion
    }
}

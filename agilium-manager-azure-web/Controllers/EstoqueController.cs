using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using agilium.webapp.manager.mvc.ViewModels.EstoqueViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    public class EstoqueController : MainController
    {
        private readonly IEstoqueService _estoqueService;
        private readonly IEmpresaService _empresaService;
        private readonly string _nomeEntidadeMotivo = "Estoque";
        private IEnumerable<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();

        public EstoqueController(IEstoqueService estoqueService, IEmpresaService empresaService)
        {
            _estoqueService = estoqueService;
            _empresaService = empresaService;

            listaEmpresaViewModels = _empresaService.ObterTodas().Result;
        }

        #region Estoque
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
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

            var lista = (await _estoqueService.ObterPorDescricao(_idEmpresaSelec, q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return View(lista);
        }

        public async Task<IActionResult> CreateEstoque()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateEstoque";

            var model = new EstoqueViewModel();
            model.situacao = Enums.EAtivo.Ativo;
            model.Empresas = listaEmpresaViewModels.ToList();
            return View("CreateEditEstoque", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEstoque(EstoqueViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateEstoque";
            if (!ModelState.IsValid) return View("CreateEditEstoque", model);

            var resposta = await _estoqueService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova {_nomeEntidadeMotivo}" };
                model.Empresas = listaEmpresaViewModels.ToList();

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditEstoque", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditEstoque(long id)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditEstoque";
            var objeto = await _estoqueService.ObterPorId(id);
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
            objeto.Empresas = listaEmpresaViewModels.ToList();

            return View("CreateEditEstoque", objeto);
        }

        [HttpPost]
        public async Task<IActionResult> EditEstoque(EstoqueViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditEstoque";

            if (!ModelState.IsValid) return View("CreateEditEstoque", model);

            var resposta = await _estoqueService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                model.Empresas = listaEmpresaViewModels.ToList();
                var retornoErro = new { mensagem = $"Erro ao editar {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditEstoque", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteEstoque(long id)
        {
            var objeto = await _estoqueService.ObterPorId(id);
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
            objeto.Empresas = listaEmpresaViewModels.ToList();
            return View(objeto);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEstoque(EstoqueViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var resposta = await _estoqueService.Remover(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar {_nomeEntidadeMotivo}" };
                model.Empresas = listaEmpresaViewModels.ToList();
                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }


        #endregion

        #region Estoque Produto

        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [Route("produtos")]
        public async Task<ActionResult> ProdutoEstoque(long idEstoque)
        {
            var estoque = _estoqueService.ObterPorId(idEstoque).Result;

            ViewBag.Estoque = estoque.Descricao;
            ViewBag.idEstoque = idEstoque;

            var lista = _estoqueService.ObterProdutoEstoquePorIdEstoque(idEstoque).Result;

            return View(lista);
        }


        #endregion

        #region Report
        [Route("report/posicao")]
        public async Task<IActionResult> ReportEstoquePosicao()
        {
            var viewModel = new FiltroEstoquePosicao();
            viewModel.IdEstoque = 0;
            viewModel.Estoques = _estoqueService.ObterTodas().Result.ToList();
            return View("ReportPosicaoEstoque", viewModel);
        }

        [Route("report/posicao")]
        [HttpPost]
        public async Task<IActionResult> ReportEstoquePosicao(FiltroEstoquePosicao viewModel)
        {
            viewModel.Estoques = _estoqueService.ObterTodas().Result.ToList();

            if (!ModelState.IsValid) return View("ReportPosicaoEstoque", viewModel);

      
            viewModel.Lista = (await _estoqueService.ObterRelatorioPosicaoEstoque(viewModel.IdEstoque));

            return View("ReportPosicaoEstoque", viewModel);

        }
        #endregion
    }
}

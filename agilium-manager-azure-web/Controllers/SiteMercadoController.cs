using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels.SiteMercadoViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    [Route("sm")]
    public class SiteMercadoController : MainController
    {
        private readonly ISiteMercadoService _siteMercadoService;
        private readonly IProdutoService _produtoService;
        private readonly IMoedaService _moedaService;
        private readonly IEmpresaService _empresaService;

        public SiteMercadoController(ISiteMercadoService siteMercadoService, IProdutoService produtoService, IMoedaService moedaService,
            IEmpresaService empresaService)
        {
            _siteMercadoService = siteMercadoService;
            _produtoService = produtoService;
            _moedaService = moedaService;
            _empresaService = empresaService;
        }

        #region Produto
        [Route("produtos")]
        public async Task<IActionResult> IndexProduto([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar produto";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "produto SM";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "produto Sm";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = (await _siteMercadoService.ObterPaginacaoPorDescricao(_idEmpresaSelec, q, page, ps));
            
            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "IndexProduto";

            return View(lista);
        }

        [Route("produto/novo")]
        [HttpGet]
        public async Task<IActionResult> CreateProduto()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateProduto";

            var model = new ProdutoSiteMercadoViewModel();
            model.Situacao = Enums.ESituacaoProdutoSiteMercada.Disponivel;
            model.Validade = Enums.EValidadeSiteMercado.Nao;
            model.Id = 0;
            model.ValorAtacado = 0;
            model.ValorCompra = 0;
            model.ValorPromocao = 0;
            model.QuantidadeAtacado = 0;

            PreencherListaAuxiliaresProdutos(model);
            return View("CreateEditProdutoSM", model);
        }

        [Route("produto/novo")]
        [HttpPost]
        public async Task<IActionResult> CreateProduto(ProdutoSiteMercadoViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "CreateProduto";
            PreencherListaAuxiliaresProdutos(model);
            if (!ModelState.IsValid) return View("CreateEditProdutoSM", model);

            var resposta = await _siteMercadoService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo produto SM" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditProdutoSM", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexProduto");
        }

        [Route("produto/editar")]
        [HttpGet]
        public async Task<IActionResult> EditProduto(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditProduto";
            var objeto = await _siteMercadoService.ObterProdutoPorId(id);
            PreencherListaAuxiliaresProdutos(objeto);
            if (objeto == null)
            {
                var msgErro = $"produto SM não localizado";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Produto";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexProduto");
            }
            return View("CreateEditProdutoSM", objeto);
        }

        [Route("produto/editar")]
        [HttpPost]
        public async Task<IActionResult> EditProduto(ProdutoSiteMercadoViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditProduto";
            PreencherListaAuxiliaresProdutos(model);
            if (!ModelState.IsValid) return View("CreateEditProdutoSM", model);

            var resposta = await _siteMercadoService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar produto SM" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditProdutoSM", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexProduto");
        }

        [Route("produto/apagar")]
        [HttpGet]
        public async Task<IActionResult> DeleteProduto(long id)
        {
           
            var objeto = await _siteMercadoService.ObterProdutoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"produto não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Produto";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexProduto");
            }
            PreencherListaAuxiliaresProdutos(objeto);
            return View(objeto);
        }

        [Route("produto/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteProduto(ProdutoSiteMercadoViewModel model)
        {
            var resposta = await _siteMercadoService.Remover(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                PreencherListaAuxiliaresProdutos(model);
                var retornoErro = new { mensagem = $"Erro ao tentar apagar Produto SM" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexProduto");
        }

        private void PreencherListaAuxiliaresProdutos(ProdutoSiteMercadoViewModel viewModel)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();
            if(_idEmpresaSelec > 0)
                viewModel.Produtos = _produtoService.ObterTodas(_idEmpresaSelec).Result.ToList();
            viewModel.Empresas = _empresaService.ObterTodas().Result.ToList();
        }
        #endregion

        #region Moeda
        [Route("moedas")]
        public async Task<IActionResult> IndexMoeda([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar produto";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "moeda SM";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "moeda Sm";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = (await _siteMercadoService.ObterPaginacaoMoedaPorDescricao(_idEmpresaSelec, q, page, ps));

            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "IndexMoeda";

            return View(lista);
        }

        [Route("moeda/novo")]
        [HttpGet]
        public async Task<IActionResult> CreateMoeda()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateMoeda";

            var model = new MoedaSiteMercadoViewModel();
            model.Id = 0;
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();
            if (_idEmpresaSelec > 0)
                model.IDEMPRESA = _idEmpresaSelec;
;
            PreencherListaAuxiliaresProdutos(model);
            return View("CreateEditMoedaSM", model);
        }

        [Route("moeda/novo")]
        [HttpPost]
        public async Task<IActionResult> CreateMoeda(MoedaSiteMercadoViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "CreateMoeda";
            PreencherListaAuxiliaresProdutos(model);
            if (!ModelState.IsValid) return View("CreateEditMoedaSM", model);

            var resposta = await _siteMercadoService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao associar nova moeda SM" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditMoedaSM", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMoeda");
        }

        [Route("moeda/editar")]
        [HttpGet]
        public async Task<IActionResult> EditMoeda(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditMoeda";
            var objeto = await _siteMercadoService.ObterMoedaPorId(id);
            PreencherListaAuxiliaresProdutos(objeto);
            if (objeto == null)
            {
                var msgErro = $"moeda SM não localizado";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Moeda";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexMoeda");
            }
            return View("CreateEditMoedaSM", objeto);
        }

        [Route("moeda/editar")]
        [HttpPost]
        public async Task<IActionResult> EditMoeda(MoedaSiteMercadoViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditMoeda";
            PreencherListaAuxiliaresProdutos(model);
            if (!ModelState.IsValid) return View("CreateEditMoedaSM", model);

            var resposta = await _siteMercadoService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar moeda SM" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditMoedaSM", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMoeda");
        }
        [Route("moeda/apagar")]
        [HttpGet]
        public async Task<IActionResult> DeleteMoeda(long id)
        {

            var objeto = await _siteMercadoService.ObterMoedaPorId(id);
            if (objeto == null)
            {
                var msgErro = $"moeda não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Moeda";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexMoeda");
            }
            PreencherListaAuxiliaresProdutos(objeto);
            return View(objeto);
        }

        [Route("moeda/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteMoeda(MoedaSiteMercadoViewModel model)
        {
            var resposta = await _siteMercadoService.RemoverMoeda(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                PreencherListaAuxiliaresProdutos(model);
                var retornoErro = new { mensagem = $"Erro ao tentar apagar Moeda SM" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMoeda");
        }

        private void PreencherListaAuxiliaresProdutos(MoedaSiteMercadoViewModel viewModel)
        {
            viewModel.Moedas = _moedaService.ObterTodas().Result.ToList();
            viewModel.Empresas = _empresaService.ObterTodas().Result.ToList();
        }
        #endregion
    }
}

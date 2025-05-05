
using agilium.webapp.manager.mvc.Enums;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.Services;
using agilium.webapp.manager.mvc.ViewModels.CategFinancViewModel;
using agilium.webapp.manager.mvc.ViewModels.Cliente;
using agilium.webapp.manager.mvc.ViewModels.Contato;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using agilium.webapp.manager.mvc.ViewModels.ImpostoViewModel;
using agilium.webapp.manager.mvc.ViewModels.ProdutoViewModel;
using agilium.webapp.manager.mvc.ViewModels.TurnoViewModel;
using agilium.webapp.manager.mvc.ViewModels.UnidadeViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{

    [Route("produto")]
    public class ProdutoController : MainController
    {
        private readonly IProdutoService _produtoService;
        private readonly IEmpresaService _empresaService;
        private readonly IEstoqueService _estoqueService;
        private readonly IClienteService _clienteService;
        private readonly ITurnoService _turnoService;

        private readonly string _nomeEntidadeDepart = "Departamento";
        private readonly string _nomeEntidadeMarca = "Marca";
        private readonly string _nomeEntidadeGrupo = "Grupo de Produtos";
        private readonly string _nomeEntidadeSubGrupo = "SubGrupo de Produtos";
        private readonly IUnidadeService _unidadeService;
        private readonly ITabelaAuxiliarFiscalService _tabelaAuxiliarFiscalService;



        #region Listas Auxiliares
        private List<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();
        private List<CfopViewModel> Cfops { get; set; } = new List<CfopViewModel>();
        private List<CstViewModel> Csts { get; set; } = new List<CstViewModel>();
        private List<CestViewModel> Cests { get; set; } = new List<CestViewModel>();
        private List<GrupoProdutoViewModel> Grupos { get; set; } = new List<GrupoProdutoViewModel>();
        private List<SubGrupoViewModel> SubGrupos { get; set; } = new List<SubGrupoViewModel>();
        private List<ProdutoDepartamentoViewModel> Departamentos { get; set; } = new List<ProdutoDepartamentoViewModel>();
        private List<ProdutoMarcaViewModel> Marcas { get; set; } = new List<ProdutoMarcaViewModel>();
        private List<UnidadeIndexViewModel> Unidades { get; set; } = new List<UnidadeIndexViewModel>();
        private List<CsosnViewModel> Csosn { get; set; } = new List<CsosnViewModel>();
        #endregion

        public ProdutoController(IProdutoService produtoService, IEmpresaService empresaService, ITabelaAuxiliarFiscalService tabelaAuxiliarFiscalService,
            IUnidadeService unidadeService, IEstoqueService estoqueService, IClienteService clienteService, ITurnoService turnoService)
        {
            _produtoService = produtoService;
            _empresaService = empresaService;
            _tabelaAuxiliarFiscalService = tabelaAuxiliarFiscalService;
            _unidadeService = unidadeService;
            _estoqueService = estoqueService;
            _clienteService = clienteService;
            _turnoService = turnoService;

            listaEmpresaViewModels = _empresaService.ObterTodas().Result.ToList();
            var tabelasAuxiliares = tabelaAuxiliarFiscalService.ObterTabelasAuxiliaresFiscal().Result;
            var listasAuxiliaresProdutos = _produtoService.ObterListasAuxiliares().Result;
            Cfops = tabelasAuxiliares.Cfops;
            Csts = tabelasAuxiliares.Csts;
            Csosn = tabelasAuxiliares.Csosn;
            Cests = tabelasAuxiliares.Cests;
            Grupos = listasAuxiliaresProdutos.Grupos;
            SubGrupos = listasAuxiliaresProdutos.SubGrupos;
            Marcas = listasAuxiliaresProdutos.Marcas;
            Departamentos = listasAuxiliaresProdutos.Departamentos;
            Unidades = _unidadeService.ObterTodas().Result;
            
        }

        #region endpoints

        #region Produto
        [Route("lista")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar produto";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "produto";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "produto";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = (await _produtoService.ObterPaginacaoPorDescricao(_idEmpresaSelec, q, page, ps));
            lista.List.ToList().ForEach(x => {
                var unidadeCompra = Unidades.FirstOrDefault(y => y.Id.ToString() == x.UnidadeCompra);
                var unidadeVenda = Unidades.FirstOrDefault(y => y.Id.ToString() == x.UnidadeVenda);
                x.DescricaoUnidadeCompra = unidadeCompra != null ?unidadeCompra.Descricao : "";
                x.DescricaoUnidadeVenda = unidadeVenda != null ? unidadeVenda.Descricao : "";
            });

            ViewBag.Pesquisa = q;
            lista.ReferenceAction = "Index";

            return View(lista);
        }

        private void PreencherListaAxuliaresProdutos(ProdutoViewModel viewModel)
        {
            viewModel.SubGrupos = SubGrupos;
            viewModel.Grupos = Grupos;
            viewModel.Marcas = Marcas;
            viewModel.Departamentos = Departamentos;
            viewModel.Unidades = Unidades;
            viewModel.Cfops = Cfops;
            viewModel.Cests = Cests;
            viewModel.Csosn = Csosn;
            viewModel.Csts = Csts;
            viewModel.Empresas = listaEmpresaViewModels.ToList();
        }

        [Route("ObterProduto")]
        public async Task<IActionResult> ObterProduto(long id)
        {

            var objeto = await _produtoService.ObterProdutoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"produto não localizado";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Produto";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return Json(objeto);
        }


        [Route("novo")]
        [HttpGet]
        public async Task<IActionResult> CreateProduto()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateProduto";

            var model = new ProdutoViewModel();
            model.Situacao = Enums.EAtivo.Ativo;
            PreencherListaAxuliaresProdutos(model);
            model.Id = 0;
            return View("CreateEditProduto", model);
        }

        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> CreateProduto(ProdutoViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "CreateProduto";
            PreencherListaAxuliaresProdutos(model);

            if (!ModelState.IsValid) return View("CreateEditProduto", model);

            var resposta = await _produtoService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo produto" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditProduto", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [HttpGet]
        public async Task<IActionResult> EditProduto(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditProduto";
            var objeto = await _produtoService.ObterProdutoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"produto não localizado";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Produto";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            PreencherListaAxuliaresProdutos(objeto);

            return View("CreateEditProduto", objeto);
        }

        [Route("editar")]
        [HttpPost]
        public async Task<IActionResult> EditProduto(ProdutoViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditProduto";
            PreencherListaAxuliaresProdutos(model);

            if (!ModelState.IsValid) return View("CreateEditProduto", model);

            var resposta = await _produtoService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar produto" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditProduto", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [HttpGet]
        public async Task<IActionResult> DeleteProduto(long id)
        {
            var objeto = await _produtoService.ObterProdutoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"produto não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Produto";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            PreencherListaAxuliaresProdutos(objeto);

            return View(objeto);
        }

        [Route("apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteProduto(ProdutoViewModel model)
        {
            var resposta = await _produtoService.Remover(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                PreencherListaAxuliaresProdutos(model);
                var retornoErro = new { mensagem = $"Erro ao tentar apagar Produto" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }
        #endregion

        #region CodigoBarra
        [Route("codigo-barra")]
        public async Task<IActionResult> ListaCodigoBarra(long idProduto)
        {
            var produto = _produtoService.ObterProdutoPorId(idProduto).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;
            
            var lista = _produtoService.ObterCodigoBarraPorProduto(idProduto).Result;

            return View("_codigoBarra", lista);
        }

        [Route("codigo-barra/novo")]
        public async Task<IActionResult> AdicionarCodigoBarra(long idProduto)
        {
            ViewBag.acao = "AdicionarCodigoBarra";
            ViewBag.operacao = "I";

            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar produto";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "produto";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "produto";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }
            ProdutoCodigoBarraViewModel model = new ProdutoCodigoBarraViewModel();           
            model.IDPRODUTO = idProduto;

            return View("_createEditCodigoBarra",model);
        }

        [HttpPost]
        [Route("codigo-barra/apagar")]
        public async Task<IActionResult> DeleteCodigoBarra(ProdutoCodigoBarraViewModel model)
        {
            var resposta = await _produtoService.RemoverCodigoBarra(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar Codigo de barra " };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaCodigoBarra", new { idProduto = model.IDPRODUTO });
        }

        [Route("codigo-barra/apagar")]
        public async Task<IActionResult> DeleteCodigoBarra(long id)
        {
            var objeto = await _produtoService.ObterProdutoCodigoBarraPorId(id);
            if (objeto == null)
            {
                var msgErro = $"Codigo de barra o produto não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Produto";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View(objeto);
        }

        [Route("codigo-barra/novo")]
        [HttpPost]
        public async Task<IActionResult> AdicionarCodigoBarra(ProdutoCodigoBarraViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "AdicionarCodigoBarra";

            var _idEmpresaSelec = ObterIdEmpresaSelecionada();
            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar produto";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "produto";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "produto";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid) return View("_createEditCodigoBarra", model);

            var resposta = await _produtoService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo produto" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("_createEditCodigoBarra", model); ;
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaCodigoBarra", new { idProduto = model.IDPRODUTO});
        }

        #endregion

        #region ProdutoDepartamento
        [Route("departamento/lista")]
        public async Task<IActionResult> IndexDepartamentos([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidadeDepart}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidadeDepart;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeDepart;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = (await _produtoService.ObterDepartamentoPaginacaoPorDescricao(_idEmpresaSelec, q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return View(lista);
        }

        [Route("departamento/novo")]
        public async Task<IActionResult> CreateDepartamento()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateDepartamento";

            var model = new ProdutoDepartamentoViewModel();
            model.situacao = Enums.EAtivo.Ativo;
            model.Empresas = listaEmpresaViewModels.ToList();
            return View("CreateEditDepartamento", model);
        }

        [Route("departamento/novo")]
        [HttpPost]
        public async Task<IActionResult> CreateDepartamento(ProdutoDepartamentoViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateDepartamento";
            if (!ModelState.IsValid) return View("CreateEditDepartamento", model);

            var resposta = await _produtoService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova {_nomeEntidadeDepart}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditDepartamento", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexDepartamentos");
        }

        [Route("departamento/editar")]
        public async Task<IActionResult> EditDepartamento(long id)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditDepartamento";
            var objeto = await _produtoService.ObterDepartamentoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeDepart} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeDepart;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexDepartamentos");
            }

            return View("CreateEditDepartamento", objeto);
        }

        [Route("departamento/editar")]
        [HttpPost]
        public async Task<IActionResult> EditDepartamento(ProdutoDepartamentoViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditDepartamento";

            if (!ModelState.IsValid) return View("CreateEditDepartamento", model);

            var resposta = await _produtoService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar {_nomeEntidadeDepart}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditDepartamento", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexDepartamentos");
        }

        [Route("departamento/apagar")]
        public async Task<IActionResult> DeleteDepartamento(long id)
        {
            var objeto = await _produtoService.ObterDepartamentoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeDepart} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeDepart;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexDepartamentos");
            }

            return View(objeto);
        }

        [Route("departamento/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteDepartamento(ProdutoDepartamentoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var resposta = await _produtoService.Remover(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar nova {_nomeEntidadeDepart}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexDepartamentos");
        }
        #endregion

        #region ProdutoMarca
        [Route("marca/lista")]
        public async Task<IActionResult> IndexMarcas([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidadeMarca}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidadeMarca;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeMarca;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = (await _produtoService.ObterMarcaPaginacaoPorDescricao(_idEmpresaSelec, q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return View(lista);
        }

        [Route("marca/novo")]
        public async Task<IActionResult> CreateMarca()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateMarca";

            var model = new ProdutoMarcaViewModel();
            model.situacao = Enums.EAtivo.Ativo;
            model.Empresas = listaEmpresaViewModels.ToList();
            return View("CreateEditMarca", model);
        }

        [Route("marca/novo")]
        [HttpPost]
        public async Task<IActionResult> CreateMarca(ProdutoMarcaViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateMarca";
            if (!ModelState.IsValid) return View("CreateEditMarca", model);

            var resposta = await _produtoService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova {_nomeEntidadeMarca}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditMarca", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMarcas");
        }

        [Route("marca/editar")]
        public async Task<IActionResult> EditMarca(long id)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditMarca";
            var objeto = await _produtoService.ObterMarcaPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeDepart} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeDepart;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexMarcas");
            }

            return View("CreateEditMarca", objeto);
        }

        [Route("marca/editar")]
        [HttpPost]
        public async Task<IActionResult> EditMarca(ProdutoMarcaViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditMarca";

            if (!ModelState.IsValid) return View("CreateEditMarca", model);

            var resposta = await _produtoService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar {_nomeEntidadeMarca}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditDepartamento", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMarcas");
        }

        [Route("marca/apagar")]
        public async Task<IActionResult> DeleteMarca(long id)
        {
            var objeto = await _produtoService.ObterMarcaPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeDepart} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeDepart;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexMarcas");
            }

            return View(objeto);
        }

        [Route("marca/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteMarca(ProdutoMarcaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var resposta = await _produtoService.RemoverMarca(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar nova {_nomeEntidadeMarca}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMarcas");
        }
        #endregion

        #region Grupo
        [Route("grupo/lista")]
        public async Task<IActionResult> IndexGrupo([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar {_nomeEntidadeGrupo}";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidadeGrupo;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeGrupo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var lista = (await _produtoService.ObterGrupoPaginacaoPorDescricao(_idEmpresaSelec, q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return View(lista);
        }

        [Route("grupo/novo")]
        public async Task<IActionResult> CreateGrupo()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateGrupo";

            var model = new GrupoProdutoViewModel();
            model.Situacao = Enums.EAtivo.Ativo;
            model.Empresas = listaEmpresaViewModels.ToList();
            return View("CreateEditGrupo", model);
        }

        [Route("grupo/novo")]
        [HttpPost]
        public async Task<IActionResult> CreateGrupo(GrupoProdutoViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateGrupo";
            if (!ModelState.IsValid) return View("CreateEditGrupo", model);

            var resposta = await _produtoService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova {_nomeEntidadeMarca}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditGrupo", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexGrupo");
        }

        [Route("grupo/editar")]
        public async Task<IActionResult> EditGrupo(long id)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditGrupo";
            var objeto = await _produtoService.ObterGrupoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeGrupo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeGrupo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexGrupo");
            }

            return View("CreateEditGrupo", objeto);
        }

        [Route("grupo/editar")]
        [HttpPost]
        public async Task<IActionResult> EditGrupo(GrupoProdutoViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditGrupo";

            if (!ModelState.IsValid) return View("CreateEditGrupo", model);

            var resposta = await _produtoService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar {_nomeEntidadeGrupo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditGrupo", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexGrupo");
        }

        [Route("grupo/apagar")]
        public async Task<IActionResult> DeleteGrupo(long id)
        {
            var objeto = await _produtoService.ObterGrupoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeGrupo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeGrupo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexGrupo");
            }

            return View(objeto);
        }

        [Route("grupo/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteGrupo(GrupoProdutoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var resposta = await _produtoService.RemoverGrupo(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar nova {_nomeEntidadeGrupo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexGrupo");
        }
        #endregion

        #region SubGrupo
        [Route("subgrupo/lista")]
        public async Task<IActionResult> IndexSubGrupo([FromQuery] long idGrupo, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var lista = (await _produtoService.ObterSubGrupoPaginacaoPorDescricao(idGrupo, q, page, ps)); ;
            var modelGrupo = ObterGrupoProdutoPorId(idGrupo);
            ViewBag.Pesquisa = q;
            ViewBag.Grupo = modelGrupo != null ? modelGrupo.Nome : "";
            ViewBag.idGrupo = idGrupo;

            return View(lista);
        }

        [Route("subgrupo/novo")]
        public async Task<IActionResult> CreateSubGrupo(long idGrupo)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateSubGrupo";
            var modelGrupo = ObterGrupoProdutoPorId(idGrupo);

            var model = new SubGrupoViewModel();
            model.Situacao = Enums.EAtivo.Ativo;
            model.IDGRUPO = idGrupo;
            model.NomeGrupo = modelGrupo != null ? modelGrupo.Nome : "";

            return View("CreateEditSubGrupo", model);
        }

        [Route("subgrupo/novo")]
        [HttpPost]
        public async Task<IActionResult> CreateSubGrupo(SubGrupoViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateSubGrupo";
            if (!ModelState.IsValid) return View("CreateEditSubGrupo", model);

            var resposta = await _produtoService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova {_nomeEntidadeSubGrupo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditSubGrupo", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexSubGrupo", new { idGrupo = model.IDGRUPO });
        }

        [Route("subgrupo/editar")]
        public async Task<IActionResult> EditSubGrupo(long id)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditSubGrupo";

            var objeto = await _produtoService.ObterSubGrupoPorId(id);
            var modelGrupo = ObterGrupoProdutoPorId(objeto.IDGRUPO.Value);
            objeto.NomeGrupo = modelGrupo != null ? modelGrupo.Nome : "";
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeSubGrupo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeSubGrupo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexSubGrupo");
            }

            return View("CreateEditSubGrupo", objeto);
        }

        [Route("subgrupo/editar")]
        [HttpPost]
        public async Task<IActionResult> EditSubGrupo(SubGrupoViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditSubGrupo";

            if (!ModelState.IsValid) return View("CreateEditSubGrupo", model);

            var resposta = await _produtoService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar {_nomeEntidadeSubGrupo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditSubGrupo", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexSubGrupo", new { idGrupo = model.IDGRUPO });
        }

        [Route("subgrupo/apagar")]
        public async Task<IActionResult> DeleteSubGrupo(long id)
        {
            var objeto = await _produtoService.ObterSubGrupoPorId(id);
            var modelGrupo = ObterGrupoProdutoPorId(objeto.IDGRUPO.Value);
            objeto.NomeGrupo = modelGrupo != null ? modelGrupo.Nome : "";
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeSubGrupo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeSubGrupo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexSubGrupo");
            }

            return View(objeto);
        }

        [Route("subgrupo/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteSubGrupo(SubGrupoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var resposta = await _produtoService.RemoverSubGrupo(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar nova {_nomeEntidadeSubGrupo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexSubGrupo", new { idGrupo = model.IDGRUPO });
        }
        #endregion

        #region ProdutoPreco
        [Route("preco")]
        public async Task<IActionResult> ListaPreco(long idProduto)
        {
            var produto = _produtoService.ObterProdutoPorId(idProduto).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            var lista = _produtoService.ObterPrecoPorProduto(idProduto).Result;

            return View("_historicoPreco", lista);
        }
        #endregion

        #region Produto Estoque
        [Route("estoque")]
        public async Task<IActionResult> ListaEstoque(long idProduto)
        {
            var produto = _produtoService.ObterProdutoPorId(idProduto).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            var lista = _estoqueService.ObterProdutoEstoquePorIdProduto(idProduto).Result;

            return View("_historicoEstoque", lista);
        }
        #endregion

        #region Estoque Historico
        [Route("estoque/historico")]
        public async Task<IActionResult> ListaEstoqueHistorico(long idProduto)
        {
            var produto = _produtoService.ObterProdutoPorId(idProduto).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            var lista = _estoqueService.ObterHistoricoEstoquePorIdProduto(idProduto).Result;

            return View("_historicoMovEstoque", lista);
        }
        #endregion

        #region Produto Foto
        [Route("foto")]
        public async Task<IActionResult> ListaFoto(long idProduto)
        {
            var produto = _produtoService.ObterProdutoPorId(idProduto).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            var lista = _produtoService.ObterFotoPorProduto(idProduto).Result;
           
            return View("_fotoProduto", lista);
        }

        protected byte[] ConverterFormFileToByte(IFormFile formFile)
        {
            long length = formFile.Length;
            if (length < 0)
                return null;

            using var fileStream = formFile.OpenReadStream();
            byte[] bytes = new byte[length];
            fileStream.Read(bytes, 0, (int)formFile.Length);

            return bytes;
        }

        [Route("foto/novo")]
        public async Task<IActionResult> AdicionarFoto(long idProduto)
        {
            var produto = _produtoService.ObterProdutoPorId(idProduto).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            ViewBag.acao = "AdicionarFoto";
            ViewBag.operacao = "I";
            
            var model = new ProdutoFotoViewModel();
            model.idProduto = idProduto;
            model.Data = DateTime.Now;
        
            return View("_createEditFoto", model);
        }

        [Route("foto/novo")]
        [HttpPost]
        public async Task<IActionResult> AdicionarFoto(ProdutoFotoViewModel model)
        {
            var produto = _produtoService.ObterProdutoPorId(model.idProduto.Value).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = model.idProduto;

            ViewBag.operacao = "I";
            ViewBag.acao = "AdicionarFoto";
            if(model.Foto == null)
            {
                var msgErro = "A seleção de uma imagem é obrigatoria";
                AdicionarErroValidacao(msgErro);

                TempData["TipoMensagem"] = "danger";
                TempData["Mensagem"] = msgErro;

                return View("_createEditFoto", model);
            }

            if (model.Foto.Length > 1048576)
            {
                var msgErro = "A imagem selecionada deve possuir tamanho de até 1 MB ";
                AdicionarErroValidacao(msgErro);

                TempData["TipoMensagem"] = "danger";
                TempData["Mensagem"] = msgErro;

                return View("_createEditFoto", model);
            }

            if (!ModelState.IsValid) return View("_createEditFoto", model);
            
            var resposta = await _produtoService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova foto de produto" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("_createEditFoto", model); ;
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaFoto", new { idProduto = model.idProduto });
        }

  
        [HttpPost]
        [Route("foto/apagar")]
        public async Task<IActionResult> DeleteFoto(ProdutoFotoViewModel model)
        {
            var resposta = await _produtoService.RemoverFoto(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar foto do produto" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaFoto", new { idProduto = model.idProduto });
        }

        [Route("foto/apagar")]
        public async Task<IActionResult> DeleteFoto(long id)
        {

            var objeto = await _produtoService.ObterProdutoFotoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"Foto do produto não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Produto";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            var produto = _produtoService.ObterProdutoPorId(objeto.idProduto.Value).Result;

            ViewBag.NomeProduto = produto.Nome;

            return View(objeto);
        }
        #endregion

        #region ClientePreco
        [Route("precos/cliente")]
        public async Task<IActionResult> ListaClientePreco(long idProduto)
        {
            var produto = _produtoService.ObterProdutoPorId(idProduto).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            var precos = _clienteService.ObterClientePrecoPorProduto(idProduto).Result;
  
            
            precos.ToList().ForEach(preco => {
                var cliente = _clienteService.ObterPorId(preco.IDCLIENTE).Result;
                preco.NomeCliente = cliente.Nome;
                preco.ValorFinal = produto.Preco.HasValue ? produto.Preco.Value : 0;

                if (preco.Diferenca == 1)//Enums.ETpDiferencaPreco.Acrescimo)
                {
                    preco.DescricaoTipoDiferenca = Enums.ETpDiferencaPreco.Acrescimo;
                    if (preco.TipoValor == 1)//Enums.ETipoValorPreco.Percentual)
                    {
                        preco.DescricaoTipoValor = Enums.ETipoValorPreco.Percentual;
                        preco.ValorFinal += (preco.ValorFinal * preco.Valor / 100);
                    }
                    else
                    {
                        preco.DescricaoTipoValor = Enums.ETipoValorPreco.Valor;
                        preco.ValorFinal += preco.Valor;
                    }
                }
                else
                {
                    preco.DescricaoTipoDiferenca = Enums.ETpDiferencaPreco.Desconto;
                    if (preco.TipoValor == 1)//Enums.ETipoValorPreco.Percentual)
                    {
                        preco.DescricaoTipoValor = Enums.ETipoValorPreco.Percentual;
                        preco.ValorFinal -= (preco.ValorFinal * preco.Valor / 100);
                    }
                    else
                    {
                        preco.DescricaoTipoValor = Enums.ETipoValorPreco.Valor;
                        preco.ValorFinal -= preco.Valor;
                    }
                }
            });

            return View("_clientePreco", precos);
        }

        [Route("preco/cliente")]
        public async Task<IActionResult> AdicionarPrecoCliente(long idProduto)
        {
            var produto = _produtoService.ObterProdutoPorId(idProduto).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            ViewBag.acao = "AdicionarPrecoCliente";
            ViewBag.operacao = "I";

            var model = new ClientePrecoViewModel();
            model.IDPRODUTO = idProduto;
            model.Datahora = DateTime.Now;
            model.TipoValor = 1;
            model.Diferenca = 1;
            model.Clientes = ObterListaClientes();

            return View("_createEditClientePreco", model);
        }

        [Route("preco/cliente")]
        [HttpPost]
        public async Task<IActionResult> AdicionarPrecoCliente(ClientePrecoViewModel model)
        {
            var produto = _produtoService.ObterProdutoPorId(model.IDPRODUTO).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = model.IDPRODUTO;

            ViewBag.operacao = "I";
            ViewBag.acao = "AdicionarPrecoCliente";
            
            if (!ModelState.IsValid) return View("_createEditClientePreco", model);
            
            model.Diferenca = model.DescricaoTipoDiferenca == Enums.ETpDiferencaPreco.Desconto ? 1 : 2;
            model.TipoValor = model.DescricaoTipoValor == Enums.ETipoValorPreco.Percentual ? 1 : 2;
            model.Datahora = DateTime.Now;
            if(!ValidarValorPrecoCliente(produto.Preco.Value, model.Valor, model.DescricaoTipoDiferenca, model.DescricaoTipoValor))
            {
                AdicionarErroValidacao("O valor adicionado não pode, transformar o valor final do produto em valor menor que zero!");
                return View("_createEditClientePreco", model);
            }
            var resposta = await _clienteService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar preço diferencido por cliente" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("_createEditClientePreco", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaClientePreco", new { idProduto = model.IDPRODUTO });
        }

        [Route("preco/cliente/apagar")]
        public async Task<IActionResult> DeleteClientePreco(long id)
        {

            var objeto = await _clienteService.ObterClientePrecoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"Preço do Cliente não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Preço por cliente";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            var produto = _produtoService.ObterProdutoPorId(objeto.IDPRODUTO).Result;

            objeto.DescricaoTipoDiferenca = objeto.Diferenca == 1? Enums.ETpDiferencaPreco.Desconto : Enums.ETpDiferencaPreco.Acrescimo;
            objeto.DescricaoTipoValor = objeto.TipoValor == 1? Enums.ETipoValorPreco.Percentual : Enums.ETipoValorPreco.Valor;

            objeto.Clientes = ObterListaClientes();

            ViewBag.NomeProduto = produto.Nome;

            return View(objeto);
        }

        [HttpPost]
        [Route("preco/cliente/apagar")]
        public async Task<IActionResult> DeleteClientePreco(ProdutoFotoViewModel model)
        {
            var resposta = await _clienteService.RemoverPreco(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar preço do produto por cliente" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaClientePreco", new { idProduto = model.idProduto });
        }
        #endregion

        #region Turno Preco
        [Route("precos/turno")]
        public async Task<IActionResult> ListaTurnoPreco(long idProduto)
        {
            var produto = _produtoService.ObterProdutoPorId(idProduto).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            var precos = _turnoService.ObterTurnoPrecoPorProduto(idProduto).Result;


            precos.ToList().ForEach(preco => {

                preco.ValorFinal = produto.Preco.HasValue ? produto.Preco.Value : 0;

                if (preco.Diferenca == 1)//Enums.ETpDiferencaPreco.Acrescimo)
                {
                    preco.DescricaoTipoDiferenca = Enums.ETpDiferencaPreco.Acrescimo;
                    if (preco.TipoValor == 1)//Enums.ETipoValorPreco.Percentual)
                    {
                        preco.DescricaoTipoValor = Enums.ETipoValorPreco.Percentual;
                        preco.ValorFinal += (preco.ValorFinal * preco.Valor / 100);
                    }
                    else
                    {
                        preco.DescricaoTipoValor = Enums.ETipoValorPreco.Valor;
                        preco.ValorFinal += preco.Valor;
                    }
                }
                else
                {
                    preco.DescricaoTipoDiferenca = Enums.ETpDiferencaPreco.Desconto;
                    if (preco.TipoValor == 1)//Enums.ETipoValorPreco.Percentual)
                    {
                        preco.DescricaoTipoValor = Enums.ETipoValorPreco.Percentual;
                        preco.ValorFinal -= (preco.ValorFinal * preco.Valor / 100);
                    }
                    else
                    {
                        preco.DescricaoTipoValor = Enums.ETipoValorPreco.Valor;
                        preco.ValorFinal -= preco.Valor;
                    }
                }
            });

            return View("_turnoPreco", precos);
        }

        [Route("preco/turno")]
        public async Task<IActionResult> AdicionarPrecoTurno(long idProduto)
        {
            var produto = _produtoService.ObterProdutoPorId(idProduto).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = idProduto;

            ViewBag.acao = "AdicionarPrecoTurno";
            ViewBag.operacao = "I";

            var model = new TurnoPrecoViewModel();
            model.IDPRODUTO = idProduto;
            model.DataHora = DateTime.Now;
            model.TipoValor = 1;
            model.Diferenca = 1;

            return View("_createEditTurnoPreco", model);
        }

        [Route("preco/turno")]
        [HttpPost]
        public async Task<IActionResult> AdicionarPrecoTurno(TurnoPrecoViewModel model)
        {
            var produto = _produtoService.ObterProdutoPorId(model.IDPRODUTO).Result;

            ViewBag.NomeProduto = produto.Nome;
            ViewBag.idProduto = model.IDPRODUTO;

            ViewBag.operacao = "I";
            ViewBag.acao = "AdicionarPrecoTurno";

            if (!ModelState.IsValid) return View("_createEditTurnoPreco", model);

            model.Diferenca = model.DescricaoTipoDiferenca == Enums.ETpDiferencaPreco.Desconto ? 1 : 2;
            model.TipoValor = model.DescricaoTipoValor == Enums.ETipoValorPreco.Percentual ? 1 : 2;
            model.DataHora = DateTime.Now;
            if (!ValidarValorPrecoCliente(produto.Preco.Value, model.Valor, model.DescricaoTipoDiferenca, model.DescricaoTipoValor))
            {
                AdicionarErroValidacao("O valor adicionado não pode, transformar o valor final do produto em valor menor que zero!");
                return View("_createEditTurnoPreco", model);
            }
            var resposta = await _turnoService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar preço diferencido por turno" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("_createEditTurnoPreco", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaTurnoPreco", new { idProduto = model.IDPRODUTO });
        }

        [Route("preco/turno/apagar")]
        public async Task<IActionResult> DeleteTurnoPreco(long id)
        {

            var objeto = await _turnoService.ObterTurnoPrecoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"Preço do Turno não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Preço por cliente";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            var produto = _produtoService.ObterProdutoPorId(objeto.IDPRODUTO).Result;

            objeto.DescricaoTipoDiferenca = objeto.Diferenca == 1 ? Enums.ETpDiferencaPreco.Desconto : Enums.ETpDiferencaPreco.Acrescimo;
            objeto.DescricaoTipoValor = objeto.TipoValor == 1 ? Enums.ETipoValorPreco.Percentual : Enums.ETipoValorPreco.Valor;

            objeto.Clientes = ObterListaClientes();

            ViewBag.NomeProduto = produto.Nome;

            return View(objeto);
        }

        [HttpPost]
        [Route("preco/turno/apagar")]
        public async Task<IActionResult> DeleteTurnoPreco(TurnoPrecoViewModel model)
        {
            var resposta = await _turnoService.RemoverPreco(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar preço do produto por turno" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaTurnoPreco", new { idProduto = model.IDPRODUTO });
        }
        #endregion

        #region IBPT
        [Route("ibpt/atualizar")]
        public async Task<IActionResult> AtualizarIBPT()
        {
            var resposta = _produtoService.AtualizarIBPT().Result;
            if (!ResponsePossuiErros(resposta))
            {
                TempData["Mensagem"] = "Operação realizada com sucesso";
                TempData["TipoMensagem"] = "success";
            }  

            return RedirectToAction("Index");
        }

        #endregion

        #endregion

        #region private

        private bool ValidarValorPrecoCliente(double precoAtual, double precoNovo, ETpDiferencaPreco eTpDiferencaPreco, ETipoValorPreco eTipoValorPreco)
        {
            bool resultado = true;
            double valorFinal = precoAtual;
            if (eTpDiferencaPreco == Enums.ETpDiferencaPreco.Acrescimo)
            {
                if (eTipoValorPreco == Enums.ETipoValorPreco.Percentual)
                {
                    valorFinal += (valorFinal * precoNovo / 100);
                }
                else
                {                   
                    valorFinal += precoNovo;
                }
            }
            else
            {
                if (eTipoValorPreco == Enums.ETipoValorPreco.Percentual)
                {
                    valorFinal -= (valorFinal * precoNovo / 100);
                }
                else
                {
                    valorFinal -= precoNovo;
                }
            }

            return (valorFinal >= 0) ;
        }

        private GrupoProdutoViewModel ObterGrupoProdutoPorId(long idGrupo)
        {
            return _produtoService.ObterGrupoPorId(idGrupo).Result;
        }

        private List<ClienteViewModel> ObterListaClientes()
        {
            return _clienteService.ObterTodas().Result.ToList();
        }

        #endregion

    }
}

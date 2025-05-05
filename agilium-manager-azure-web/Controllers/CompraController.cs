using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels.CompraViewModel;
using agilium.webapp.manager.mvc.ViewModels.CompraViewModel.ImportacaoXmlNFE;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using agilium.webapp.manager.mvc.ViewModels.Fornecedor;
using agilium.webapp.manager.mvc.ViewModels.ImpostoViewModel;
using agilium.webapp.manager.mvc.ViewModels.TurnoViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace agilium.webapp.manager.mvc.Controllers
{
    [Route("compra")]
    public class CompraController : MainController
    {
        private readonly ICompraService _compraService;
        private readonly IEmpresaService _empresaService;
        private readonly IFornecedorService _fornecedorService;
        private readonly ITabelaAuxiliarFiscalService _tabelaAuxiliarFiscalService;
        private readonly ITurnoService _turnoService;
        private readonly IProdutoService _produtoService;
        private readonly IEstoqueService _estoqueService;
        private readonly IUnidadeService _unidadeService;
        private readonly IImportarXMLNfe _importarXMLNfe;
        private IEnumerable<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();
        private IEnumerable<FornecedorViewModel> listaFornecedorViewModels { get; set; } = new List<FornecedorViewModel>();
        private List<CfopViewModel> Cfops { get; set; } = new List<CfopViewModel>();

        public CompraController(ICompraService compraService, IEmpresaService empresaService, IFornecedorService fornecedorService, 
            ITabelaAuxiliarFiscalService tabelaAuxiliarFiscalService, ITurnoService turnoService, IProdutoService produtoService,
            IEstoqueService estoqueService, IUnidadeService unidadeService, IImportarXMLNfe importarXMLNfe)
        {
            _compraService = compraService;
            _empresaService = empresaService;
            _fornecedorService = fornecedorService;
            _tabelaAuxiliarFiscalService = tabelaAuxiliarFiscalService;
            _turnoService = turnoService;
            _produtoService = produtoService;
            _estoqueService = estoqueService;
            _unidadeService = unidadeService;
            _importarXMLNfe = importarXMLNfe;

            if (listaEmpresaViewModels.Count() == 0)
                listaEmpresaViewModels = _empresaService.ObterTodas().Result;

            if (listaFornecedorViewModels.Count() == 0)
                listaFornecedorViewModels = _fornecedorService.ObterTodas().Result;


            if (Cfops.Count() == 0)
            {
                var tabelasAuxiliares = tabelaAuxiliarFiscalService.ObterTabelasAuxiliaresFiscal().Result;
                if(tabelaAuxiliarFiscalService != null)
                    Cfops = tabelasAuxiliares.Cfops;
            }                
        }

        #region Compra

        [Route("lista")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar a compra";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Compra";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Compra";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            var dataAtual = DateTime.Now;
            DateTime _dtini, _dtFim;
            if (DataInicial == null)
            {
                DateTime primeiroDiaDoMes = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                _dtini = primeiroDiaDoMes;
            }
            else _dtini = Convert.ToDateTime(DataInicial);

            if (DataFinal == null)
            {
                DateTime ultimoDiaDoMes = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month));
                _dtFim = ultimoDiaDoMes;
            }
            else _dtFim = Convert.ToDateTime(DataFinal);

            if (_dtini > _dtFim)
            {
                AdicionarErroValidacao("Data Final deve ser maior ou igual a data inicial");
            }

            if (ExibirErros())
            {
            }
            var lista = (await _compraService.ObterPaginacaoPorData(_idEmpresaSelec, _dtini.ToString(), _dtFim.ToString(), page, ps));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            lista.ReferenceAction = "Index";

            return View("Index", lista);
        }

        [Route("novo")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar compra";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "compra";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "compra";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";

            var model = new CompraViewModel();
            model.Situacao = Enums.ESituacaoCompra.Aberta;
            model.DataCadastro = DateTime.Now;
            model.IDEMPRESA = _idEmpresaSelec;
            model.DataCompra = DateTime.Now;
            model.NumeroCFOP = 1102;
            model.Importada = Enums.ESimNao.Nao;
            model.ValorBaseCalculoIcms = 0;
            model.ValorBaseCalculoSub = 0;
            model.ValorDesconto = 0;
            model.ValorFrete = 0;
            model.ValorIcms = 0;
            model.ValorIcmsRetido = 0;
            model.ValorIcmsSub = 0;
            model.ValorIpi = 0;
            model.ValorIsencao = 0;
            model.ValorOutros = 0;
            model.ValorSeguro = 0;
            model.ValorTotal = 0;
            model.ValorTotalProduto = 0;

            model.Id = 0;
            PopularListasAuxiliares(model);
            return View("CreateEdit", model);
        }

        private void PopularListasAuxiliares(CompraViewModel model)
        {
            if (model.Empresas.Count() == 0)
                model.Empresas = listaEmpresaViewModels.ToList();

            if (model.Fornecedores.Count() == 0)
                model.Fornecedores = listaFornecedorViewModels.ToList();

            if (model.Cfops.Count() == 0)
                model.Cfops  = Cfops.ToList();

            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (model.Turnos.Count() == 0)
            {
                if (_idEmpresaSelec > 0)
                    model.Turnos = _turnoService.ObterTodos(_idEmpresaSelec).Result;
            }
                
        }




        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> Create(CompraViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            PopularListasAuxiliares(model);
            if (!ModelState.IsValid) return View("CreateEdit", model);

            if (!model.ValorBaseCalculoIcms.HasValue) model.ValorBaseCalculoIcms = 0;
            if (!model.ValorBaseCalculoSub.HasValue) model.ValorBaseCalculoSub = 0;
            if (!model.ValorDesconto.HasValue) model.ValorDesconto = 0;
            if (!model.ValorFrete.HasValue) model.ValorFrete = 0;
            if (!model.ValorIcms.HasValue) model.ValorIcms = 0;
            if (!model.ValorIcmsRetido.HasValue) model.ValorIcmsRetido = 0;
            if (!model.ValorIcmsSub.HasValue) model.ValorIcmsSub = 0;
            if (!model.ValorIpi.HasValue) model.ValorIpi = 0;
            if (!model.ValorIsencao.HasValue) model.ValorIsencao = 0;
            if (!model.ValorOutros.HasValue) model.ValorOutros = 0;
            if (!model.ValorSeguro.HasValue) model.ValorSeguro = 0;
            if (!model.ValorTotal.HasValue) model.ValorTotal = 0;
            if (!model.ValorTotalProduto.HasValue) model.ValorTotalProduto = 0;

            model.ValorTotal = (model.ValorTotalProduto + model.ValorIcms + model.ValorIpi + model.ValorFrete + model.ValorSeguro + model.ValorOutros) 
                                - (model.ValorIsencao + model.ValorDesconto + model.ValorIcmsRetido);
                
            var resposta = await _compraService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova compra" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            var model = await _compraService.ObterCompraPorId(id);

            PopularListasAuxiliares(model);
            if (model == null)
            {
                var msgErro = $"Compra não localizada";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Compra";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View("CreateEdit", model);
        }

        [Route("editar")]
        [HttpPost]
        public async Task<IActionResult> Edit(CompraViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";
            PopularListasAuxiliares(model);

            if (!ModelState.IsValid) return View("CreateEdit", model);

            if (!model.ValorBaseCalculoIcms.HasValue) model.ValorBaseCalculoIcms = 0;
            if (!model.ValorBaseCalculoSub.HasValue) model.ValorBaseCalculoSub = 0;
            if (!model.ValorDesconto.HasValue) model.ValorDesconto = 0;
            if (!model.ValorFrete.HasValue) model.ValorFrete = 0;
            if (!model.ValorIcms.HasValue) model.ValorIcms = 0;
            if (!model.ValorIcmsRetido.HasValue) model.ValorIcmsRetido = 0;
            if (!model.ValorIcmsSub.HasValue) model.ValorIcmsSub = 0;
            if (!model.ValorIpi.HasValue) model.ValorIpi = 0;
            if (!model.ValorIsencao.HasValue) model.ValorIsencao = 0;
            if (!model.ValorOutros.HasValue) model.ValorOutros = 0;
            if (!model.ValorSeguro.HasValue) model.ValorSeguro = 0;
            if (!model.ValorTotal.HasValue) model.ValorTotal = 0;
            if (!model.ValorTotalProduto.HasValue) model.ValorTotalProduto = 0;

            var resposta = await _compraService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar compra" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("cancelar")]
        [HttpPost]
        public async Task<IActionResult> Cancelar(CompraViewModel viewModel)
        {
            var resposta = _compraService.Cancelar(viewModel.Id).Result;

            if (ResponsePossuiErros(resposta))
            {
                PopularListasAuxiliares(viewModel); 
                ObterDadosCompraParaViewBag(viewModel.Id);
              
                return View("Cancelar",viewModel);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("cancelar")]
        public async Task<IActionResult> Cancelar(long id)
        {
            var model = await _compraService.ObterCompraPorId(id);

            PopularListasAuxiliares(model);
            ObterDadosCompraParaViewBag(id);
            if (model == null)
            {
                var msgErro = $"Compra não localizada";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Compra";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View("Cancelar", model);
        }

        [Route("importar")]
        public async Task<ActionResult> Importar(long idCompra)
        {
            // var caminhoArquivo = "C:\\Agilium\\xml\\43230714239748000156550010000131081557406941-procNFe.xml";
            var model = new NFeProc(); //await _importarXMLNfe.LerXML(caminhoArquivo);
            model.idCompra = idCompra;
            ObterDadosCompraParaViewBag(idCompra);
            return View("RetornoXmlNfeImportada",model);
        }

        [Route("ImportarXML")]
        [HttpPost]
        public async Task<ActionResult> ImportarXML()
        {
            var _idCompra = HttpContext.Request.Form["idCompra"].ToString();
            var idCompra = Int64.Parse(_idCompra);
            var data = HttpContext.Request.Form.Files["arquivoNFe"];
            ObterDadosCompraParaViewBag(idCompra);

            NFeProc model;
            try
            {
                if (data == null)
                {
                    AdicionarErroValidacao("Erro ao selecionar arquivo XML para importação");
                    model = new NFeProc();
                }
                else
                {
                    var modelArquivo = new ImportacaoArquivo();
                    modelArquivo.idCompra = idCompra;
                    modelArquivo.XmlArquivo = data;

                  var viewResult =  await _compraService.ImportarArquivo(modelArquivo);

                    viewResult.Errors.Mensagens.ForEach(msg => {
                        AdicionarErroValidacao(msg);
                    });

                    if(viewResult == null || viewResult.Data == null)
                        model = new NFeProc();
                    else
                    {
                        model = (NFeProc)viewResult.Data;
                        model.CaminhoArquivo = modelArquivo.XmlArquivo.FileName;
                    }

                    model.sucesso = !viewResult.Errors.Mensagens.Any();
                    
                    model.idCompra = idCompra;                
                }
                if (!OperacaoValida())
                {
                }
            }
            catch
            {
                model = new NFeProc();
                AdicionarErroValidacao("Formato do arquivo XML invalido");
            }
            
            return PartialView("RetornoXmlNfeImportada", model);
        }

        [Route("importar")]
        [HttpPost]
        public async Task<IActionResult> Importar(NFeProc viewModel)
        {
            var resposta = _compraService.ImportarArquivo(viewModel).Result;
            if (TempData["arquivo"] != null)
            {
                var arquivo = TempData["arquivo"] as IFormFile;
            }

            if (ResponsePossuiErros(resposta))
            {

                return View("RetornoXmlNfeImportada", viewModel);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaItemCompra", new { idCompra = viewModel.idCompra});
        }

        [Route("efetivar")]
        public async Task<ActionResult> Efetivar(long id)
        {
            var model = await _compraService.ObterCompraPorId(id);

            PopularListasAuxiliares(model);
            ObterDadosCompraParaViewBag(id);
            if (model == null)
            {
                var msgErro = $"Compra não localizada";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Compra";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View(model);
        }


        [Route("efetivar")]
        [HttpPost]
        public async Task<IActionResult> Efetivar(CompraViewModel viewModel)
        {
            var resposta = _compraService.Efetivar(viewModel.Id).Result;

            if (ResponsePossuiErros(resposta))
            {
                PopularListasAuxiliares(viewModel);
                ObterDadosCompraParaViewBag(viewModel.Id);

                return View(viewModel);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }


        [Route("cadastro-produto-automatico")]
        public async Task<ActionResult> CadastroAutomaticoProduto(long id)
        {
            var model = await _compraService.ObterCompraPorId(id);

            PopularListasAuxiliares(model);
            ObterDadosCompraParaViewBag(id);
            if (model == null)
            {
                var msgErro = $"Compra não localizada";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Compra";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [Route("cadastro-produto-automatico")]
        [HttpPost]
        public async Task<IActionResult> CadastroAutomaticoProduto(CompraViewModel viewModel)
        {
            var resposta = _compraService.CadastrarProdutosAutomaticamente(viewModel.Id).Result;

            if (ResponsePossuiErros(resposta))
            {
                PopularListasAuxiliares(viewModel);
                ObterDadosCompraParaViewBag(viewModel.Id);

                return View(viewModel);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }
        #endregion

        #region Item Compra
        [Route("IndexItem")]
        public async Task<IActionResult> IndexItem(string id)
        {
            long _id = Convert.ToInt64(id);
            var lista = _compraService.ObterItemPorIdCompra(_id).Result;
                        
            return PartialView("_indexItem", lista);
        }

        [Route("ListaItemCompra")]
        public async Task<IActionResult> ListaItemCompra(long idCompra)
        {
            ObterDadosCompraParaViewBag(idCompra);

            var lista = _compraService.ObterItemPorIdCompra(idCompra).Result;

            return View(lista);
        }

        private void ObterDadosCompraParaViewBag(long idCompra)
        {
            var compra = _compraService.ObterCompraPorId(idCompra).Result;
            if (compra == null)
            {
                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Compra";
                TempData["Mensagem"] = "Erro ao tenta localizar item da compra";
                return;
            }
            ViewBag.NomeCompra = compra.Codigo;
            ViewBag.idCompra = compra.Id;
            ViewBag.importada = compra.Importada == Enums.ESimNao.Sim;
            ViewData["importada"] = ViewBag.importada;
        }

        [Route("item/novo")]
        public async Task<IActionResult> AdicionarItem(long idCompra)
        {
            ViewBag.acao = "AdicionarItem";
            ViewBag.operacao = "I";

            CompraItemViewModel model = new CompraItemViewModel();
            model.IDCOMPRA = idCompra;

            PopularListasAuxiliares(model);
            ObterDadosCompraParaViewBag(idCompra);

            return View("_createEditItemCompra", model);
        }

        [Route("item/novo")]
        [HttpPost]
        public async Task<IActionResult> AdicionarItem(CompraItemViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "AdicionarItem";

            PopularListasAuxiliares(model);
            ObterDadosCompraParaViewBag(model.IDCOMPRA.Value);

            if (!ModelState.IsValid) return View("_createEditItemCompra", model);

            var resposta = await _compraService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo item de compra" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("_createEditItemCompra", model); ;
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaItemCompra", new { idCompra = model.IDCOMPRA });
        }

        [Route("item/editar")]
        public async Task<IActionResult> EditarItem(long id)
        {
            ViewBag.acao = "EditarItem";
            ViewBag.operacao = "E";

            var model = _compraService.ObterItemPorId(id).Result;
            if (model == null)
            {
                var msgErro = $"Item da Compra não localizado";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Compra";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("ListaItemCompra", new { idCompra = model.IDCOMPRA });
            }

            PopularListasAuxiliares(model);
            ObterDadosCompraParaViewBag(model.IDCOMPRA.Value);

            return View("_createEditItemCompra", model);
        }

        [Route("item/editar")]
        [HttpPost]
        public async Task<IActionResult> EditarItem(CompraItemViewModel model)
        {
            ViewBag.acao = "EditarItem";
            ViewBag.operacao = "E";
            PopularListasAuxiliares(model);
            ObterDadosCompraParaViewBag(model.IDCOMPRA.Value);

            if (!ModelState.IsValid) return View("_createEditItemCompra", model);

            var resposta = await _compraService.Atualizar(model.Id,model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar item de compra" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("_createEditItemCompra", model); ;
            }

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaItemCompra", new { idCompra = model.IDCOMPRA });
        }

        
        [Route("EditarItemModal")]
        public async Task<IActionResult> EditarItemModal(long id)
        {            
            var model = _compraService.ObterItemPorId(id).Result;
            if (model == null)
            {
                var msgErro = $"Item da Compra não localizado";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Compra";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("ListaItemCompra", new { idCompra = model.IDCOMPRA });
            }
       
            var viewModel = new CompraItemEditViewModel();
            viewModel.IDPRODUTO = model.IDPRODUTO;
            viewModel.IDCOMPRA = model.IDCOMPRA;
            viewModel.IDESTOQUE = model.IDESTOQUE;
            viewModel.ValorNovoPrecoVenda = model.ValorNovoPrecoVenda;
            viewModel.ValorTotal = model.ValorTotal;
            viewModel.ValorUnitario = model.ValorUnitario;
            viewModel.CodigoProduto = model.CodigoProduto;
            viewModel.Id = model.Id;
            viewModel.Importada = model.Importada;
            viewModel.SGUN = model.SGUN;
            viewModel.Relacao = model.Relacao;
            viewModel.Quantidade = model.Quantidade;
            viewModel.certo = "N";

            PopularListasAuxiliares(viewModel);
            ObterDadosCompraParaViewBag(viewModel.IDCOMPRA.Value);

            return PartialView("_editarItemCompra", viewModel);
        }

        [Route("EditarItemModal")]
        [HttpPost]
        public async Task<IActionResult> EditarItemModal(CompraItemEditViewModel model) 
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();
            ObterDadosCompraParaViewBag(model.IDCOMPRA.Value);
            if (!ModelState.IsValid) 
            {
                model.certo = "S";
                model.Importada = true;
                return PartialView("_editarItemCompra", model); 
            }

            ResponsePossuiErros(_compraService.AtualizarProdutoItemCompra(model).Result);

            if (ExibirErros())
            {
                model.certo = "S";
                model.Importada = true;
                model.SGUN = "0101";
                return PartialView("_editarItemCompra", model);
            }

            var lista = _compraService.ObterItemPorIdCompra(model.IDCOMPRA.Value).Result;

           var url = Url.Action("ListaItemCompra", "Compra", new { idCompra = model.IDCOMPRA});
            
            return Json(new { success = true,url });
           // return PartialView("_listaItemCompra",lista);
        }

        [Route("ListaItem")]
        public async Task<ActionResult> ListaItem(long id)
        {
            ObterDadosCompraParaViewBag(id);

            var lista = _compraService.ObterItemPorIdCompra(id).Result;
            return PartialView("_listaItemCompra", lista);

        }

        private void PopularListasAuxiliares(CompraItemViewModel model)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (model.Produtos.Count() == 0)
                model.Produtos = _produtoService.ObterTodas(_idEmpresaSelec).Result.ToList();

            if (model.Estoques.Count() == 0)
                model.Estoques = _estoqueService.ObterTodas().Result.ToList();

            if (model.Unidades.Count() == 0)
                model.Unidades = _unidadeService.ObterTodas().Result.ToList();
        }

        private void PopularListasAuxiliares(CompraItemEditViewModel model)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (model.Produtos.Count() == 0)
                model.Produtos = _produtoService.ObterTodas(_idEmpresaSelec).Result.ToList();

            if (model.Estoques.Count() == 0)
                model.Estoques = _estoqueService.ObterTodas().Result.ToList();

            if (model.Unidades.Count() == 0)
                model.Unidades = _unidadeService.ObterTodas().Result.ToList();
        }
        #endregion

    }
}

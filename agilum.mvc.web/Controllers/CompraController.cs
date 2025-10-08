using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.ComprasNFEViewModel;
using agilium.api.business.Services;

using agilum.mvc.web.Interfaces;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Compra;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Estoque;
using agilum.mvc.web.ViewModels.Fornecedor;
using agilum.mvc.web.ViewModels.Impostos;
using agilum.mvc.web.ViewModels.Turno;
using agilum.mvc.web.ViewModels.UnidadeViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using NFeProc = agilum.mvc.web.ViewModels.Compra.NFeProc;
using agilum.mvc.web.Configuration;
using agilum.mvc.web.Extensions;
using agilium_manager_azure_business.Interfaces.IService;
using agilum.mvc.web.Data;
using Microsoft.AspNetCore.Identity;

namespace agilum.mvc.web.Controllers
{
    [Route("compra")]
    [Authorize]
    public class CompraController : MainController
    {
        #region constantes
        private readonly ICompraService _compraService;
        private readonly IEmpresaService _empresaService;
        private readonly IFornecedorService _fornecedorService;
        private readonly ITabelaAuxiliarFiscalService _tabelaAuxiliarFiscalService;
        private readonly ITurnoService _turnoService;
        private readonly IProdutoService _produtoService;
        private readonly IEstoqueService _estoqueService;
        private readonly IUnidadeService _unidadeService;

        private readonly IUsuarioService _usuarioService;

        private readonly string _nomeEntidadeMotivo = "Compra";
        #endregion

        #region Listas Auxiliares
        private IEnumerable<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();
        private IEnumerable<FornecedorViewModel> listaFornecedorViewModels { get; set; } = new List<FornecedorViewModel>();
        private List<CfopViewModel> Cfops { get; set; } = new List<CfopViewModel>();
        #endregion

        #region construtores
        public CompraController(ICompraService compraService, IEmpresaService empresaService, IFornecedorService fornecedorService,
            ITabelaAuxiliarFiscalService tabelaAuxiliarFiscalService, ITurnoService turnoService, IProdutoService produtoService,
            IEstoqueService estoqueService, IUnidadeService unidadeService,  IUsuarioService usuarioService,
            INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService, IMapper mapper,
            ILicencaService licencaService, SignInManager<AppUserAgiliumIdentity> signInManager) : base(notificador, configuration, appUser, 
                utilDapperRepository, logService, mapper, licencaService, signInManager)
        {
            _compraService = compraService;
            _empresaService = empresaService;
            _fornecedorService = fornecedorService;
            _tabelaAuxiliarFiscalService = tabelaAuxiliarFiscalService;
            _turnoService = turnoService;
            _produtoService = produtoService;
            _estoqueService = estoqueService;
            _unidadeService = unidadeService;

            _usuarioService = usuarioService;

            if (listaEmpresaViewModels.Count() == 0)
                listaEmpresaViewModels = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);

            if (listaFornecedorViewModels.Count() == 0)
                listaFornecedorViewModels = _mapper.Map<List<FornecedorViewModel>>( _fornecedorService.ObterTodos().Result);


            if (Cfops.Count() == 0)
            {
                var tabelasAuxiliares = ObterTabelasAuxiliaresFiscal().Result;
                if (tabelaAuxiliarFiscalService != null)
                    Cfops = tabelasAuxiliares.Cfops;
            }
        }
        #endregion

        #region compras

      //  [Route("lista")]
        //[ClaimsAuthorizeAttribute(2066)]
        //public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null)
        //{

        //    var empresaSelecionada = ObterObjetoEmpresaSelecionada();

        //    if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
        //    {
        //        var msgErro = $"Selecione uma empresa para acessar {_nomeEntidadeMotivo}";

        //        TempData["TipoMensagem"] = "danger";
        //        TempData["Titulo"] = _nomeEntidadeMotivo;
        //        TempData["Mensagem"] = msgErro;

        //        ViewBag.TipoMensagem = "danger";
        //        ViewBag.Titulo = _nomeEntidadeMotivo;
        //        ViewBag.Mensagem = msgErro;
        //        return RedirectToAction("Index", "Home");
        //    }

        //    var dataAtual = DateTime.Now;
        //    DateTime _dtini, _dtFim;
        //    if (DataInicial == null)
        //    {
        //        DateTime primeiroDiaDoMes = new DateTime(dataAtual.Year, dataAtual.Month, 1);
        //        _dtini = primeiroDiaDoMes;
        //    }
        //    else _dtini = Convert.ToDateTime(DataInicial);

        //    if (DataFinal == null)
        //    {
        //        DateTime ultimoDiaDoMes = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month));
        //        _dtFim = ultimoDiaDoMes;
        //    }
        //    else _dtFim = Convert.ToDateTime(DataFinal);

        //    if (_dtini > _dtFim)
        //    {
        //        AdicionarErroValidacao("Data Final deve ser maior ou igual a data inicial");
        //    }
        //    var lista = (await ObterListaCompraPaginado(Convert.ToInt64(empresaSelecionada.IDEMPRESA), _dtini, _dtFim, page, ps));

        //    ViewBag.DataInicial = _dtini;
        //    ViewBag.DataFinal = _dtFim;

        //    return View("Index", lista);
        //}

        [Route("lista")]
        [ClaimsAuthorizeAttribute(2066)]
        public async Task<IActionResult> IndexCompra([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null)
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
                return RedirectToAction("IndexCompra", "Home");
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
            var lista = (await ObterListaCompraIndexPaginado(Convert.ToInt64(empresaSelecionada.IDEMPRESA), _dtini, _dtFim, page, ps));
            
            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            return View(lista);
        }

        [Route("novo")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2067)]
        public async Task<IActionResult> Create()
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

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";

            var model = new CompraViewModel();
            model.Situacao = ESituacaoCompra.Aberta;
            model.DataCadastro = DateTime.Now;
            model.IDEMPRESA = Convert.ToInt64(empresaSelecionada.IDEMPRESA);
            model.DataCompra = DateTime.Now;
            model.NumeroCFOP = 1102;
            model.Importada = ESimNao.Nao;
            model.Codigo = _compraService.GerarCodigoCompra(model.IDEMPRESA).Result;

            //model.ValorBaseCalculoIcms = 0;
            //model.ValorBaseCalculoSub = 0;
            //model.ValorDesconto = 0;
            //model.ValorFrete = 0;
            //model.ValorIcms = 0;
            //model.ValorIcmsRetido = 0;
            //model.ValorIcmsSub = 0;
            //model.ValorIpi = 0;
            //model.ValorIsencao = 0;
            //model.ValorOutros = 0;
            //model.ValorSeguro = 0;
            //model.ValorTotal = 0;
            //model.ValorTotalProduto = 0;

            model.Id = 0;
            var turnoAberto = await _turnoService.ObterObjetoTurnoAbertoPorIdEmpresa(Convert.ToInt64(empresaSelecionada.IDEMPRESA));
           
            if(turnoAberto != null)
                model.IDTURNO = turnoAberto.Id;
            
            PopularListasAuxiliares(model);
            return View("CreateEdit", model);
        }

        private async Task<Compra> ConverterCompraViewModelParaModel(CompraViewModel model)
        {
            double ValorBaseCalculoIcms = 0, ValorBaseCalculoSub = 0, ValorDesconto = 0, ValorIcms = 0, ValorIcmsRetido = 0, ValorIpi = 0, ValorIsencao = 0,
                ValorSeguro = 0, ValorOutros = 0, ValorTotal = 0, ValorTotalProduto = 0, ValorIcmsSub = 0,ValorFrete = 0;

            if (!string.IsNullOrEmpty(model.ValorBaseCalculoIcms))
            {
                ValorBaseCalculoIcms = 0;
            }
            ValorBaseCalculoIcms = await ConverterStringParaDecimal(model.ValorBaseCalculoIcms, ValorBaseCalculoIcms);
            ValorBaseCalculoSub = await ConverterStringParaDecimal(model.ValorBaseCalculoSub, ValorBaseCalculoSub);
            ValorDesconto = await ConverterStringParaDecimal(model.ValorDesconto, ValorDesconto);
            ValorIcms = await ConverterStringParaDecimal(model.ValorIcms, ValorIcms);
            ValorIcmsRetido = await ConverterStringParaDecimal(model.ValorIcmsRetido, ValorIcmsRetido);
            ValorIpi = await ConverterStringParaDecimal(model.ValorIpi, ValorIpi);
            ValorIsencao = await ConverterStringParaDecimal(model.ValorIsencao, ValorIsencao);
            ValorSeguro = await ConverterStringParaDecimal(model.ValorSeguro, ValorSeguro);
            ValorOutros = await ConverterStringParaDecimal(model.ValorOutros, ValorOutros);
            ValorTotal = await ConverterStringParaDecimal(model.ValorTotal, ValorTotal);
            ValorTotalProduto = await ConverterStringParaDecimal(model.ValorTotalProduto, ValorTotalProduto);
            
            
            ValorTotal = (ValorTotalProduto + ValorIcms + ValorIpi + ValorSeguro + ValorOutros)
                                - (ValorIsencao + ValorDesconto + ValorIcmsRetido);

            int? importada = Convert.ToInt32(model.Importada);

            var compra = new Compra(model.IDEMPRESA,model.IDFORN,model.IDTURNO,model.DataCompra,model.DataCadastro,model.Codigo,model.Situacao,
                model.DataNF,model.NumeroNF,model.SerieNF,model.ChaveNFE,model.TipoComprovante,model.NumeroCFOP,ValorIcmsRetido,ValorBaseCalculoIcms,
                ValorIcms,ValorBaseCalculoSub,ValorIcmsSub,ValorIsencao,ValorTotalProduto,ValorFrete,ValorSeguro,ValorDesconto,ValorOutros,ValorIpi,ValorTotal,
                model.Observacao,importada);
            
            compra.AdicionarIdCompra(model.Id);

            return await Task.FromResult(compra);
        }

        private async Task<double> ConverterStringParaDecimal(string valor, double resultado)
        {
            resultado = 0;
            if (!string.IsNullOrEmpty(valor))
            {
                Double.TryParse(valor,out resultado);
            }

            return await Task.FromResult(resultado);
        }

        [Route("novo")]
        [HttpPost]
        [ClaimsAuthorizeAttribute(2067)]
        public async Task<IActionResult> Create(CompraViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
       
            PopularListasAuxiliares(model);
            if (!ModelState.IsValid) return View("CreateEdit", model);

            if (model.Id == 0) model.Id = await GerarId();

            var objeto = await ConverterCompraViewModelParaModel(model);

            await _compraService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova compra" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }
            await _compraService.Salvar();

            LogInformacao($"Objeto Criado com sucesso {Deserializar(objeto)}", "Cliente", "Adicionar", null);
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexCompra");
        }

        [Route("editar")]
        [HttpGet]
        [ClaimsAuthorizeAttribute(2070)]
        public async Task<IActionResult> Edit(long id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            var model = await Obter(id.ToString());

            PopularListasAuxiliares(model);
            if (model == null)
            {
                var msgErro = $"Compra não localizada";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Compra";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexCompra");
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

            var objeto = await ConverterCompraViewModelParaModel(model);

            await _compraService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao editar compra" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }

            await _compraService.Salvar();
            LogInformacao($"Objeto atualizar com sucesso {Deserializar(objeto)}", "Cliente", "ATUALIZAR", null);
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexCompra");
        }

        [HttpGet]
        [Route("cancelar")]
        [ClaimsAuthorizeAttribute(2068)]
        public async Task<IActionResult> Cancelar(long id)
        {
            var model = await Obter(id.ToString());

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
                return RedirectToAction("IndexCompra");
            }

            return View("Cancelar", model);
        }

        [Route("cancelar")]
        [HttpPost]
        public async Task<IActionResult> Cancelar(CompraViewModel viewModel)
        {
            string nomeUsuario = ObterNomeUsuarioLogado();

            await _compraService.CancelarCompra(viewModel.Id, nomeUsuario);

            if (!OperacaoValida())
            {
                PopularListasAuxiliares(viewModel);
                ObterDadosCompraParaViewBag(viewModel.Id);

                return View("Cancelar", viewModel);
            }
            LogInformacao($"Objeto cancelado com sucesso id:{viewModel.Id}", "Cliente", "Cancelar", null);
           
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexCompra");
        }

        [Route("importar")]
        [ClaimsAuthorizeAttribute(2070)]
        public async Task<ActionResult> Importar(long idCompra)
        {
            // var caminhoArquivo = "C:\\Agilium\\xml\\43230714239748000156550010000131081557406941-procNFe.xml";
            var model = new agilium.api.business.Models.CustomReturn.ComprasNFEViewModel.NFeProc(); //await _importarXMLNfe.LerXML(caminhoArquivo);
            model.idCompra = idCompra;
            ObterDadosCompraParaViewBag(idCompra);
            return View("RetornoXmlNfeImportada", model);
        }

        [Route("importar")]
        [HttpPost]
        public async Task<IActionResult> Importar(agilium.api.business.Models.CustomReturn.ComprasNFEViewModel.NFeProc viewModel)
        {
            if (!ModelState.IsValid) return View(ModelState);

            //var nfeXml = _mapper.Map<agilium.api.business.Models.CustomReturn.ComprasNFEViewModel.NFeProc>(viewModel);
            //var nfeXml = ConverterClasse.Mapear<agilium.api.business.Models.CustomReturn.ComprasNFEViewModel.NFeProc>(viewModel);

            await _compraService.SalvarArquivoXmlNFE(viewModel.idCompra,viewModel,viewModel.ArquivoXml);
            if (TempData["arquivo"] != null)
            {
                var arquivo = TempData["arquivo"] as IFormFile;
            }
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Compra", "importar", "Web"));
                return View("RetornoXmlNfeImportada", viewModel);
            }
            LogInformacao($"Objeto importado com sucesso id:{Deserializar(viewModel)}", "Compra", "Importar", null);
           
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaItemCompra", new { idCompra = viewModel.idCompra });
        }

        [Route("ImportarXML")]
        [HttpPost]
        [ClaimsAuthorizeAttribute(2070)]
        public async Task<ActionResult> ImportarXML()
        {
            var _idCompra = HttpContext.Request.Form["idCompra"].ToString();
            var idCompra = Int64.Parse(_idCompra);
            var data = HttpContext.Request.Form.Files["arquivoNFe"];
            ObterDadosCompraParaViewBag(idCompra);

            var model = new agilium.api.business.Models.CustomReturn.ComprasNFEViewModel.NFeProc();
            try
            {
                if (data == null)
                {
                    AdicionarErroValidacao("Erro ao selecionar arquivo XML para importação");                   
                }
                else
                {
                    var modelArquivo = new ImportacaoArquivo();
                    modelArquivo.idCompra = idCompra;
                    modelArquivo.XmlArquivo = data;

                    var arquivoConvertidoByte = await ConverterFormFileToByte(data);

                    var arquivoStringConvertidoDeByte = await ConverterByteToString(arquivoConvertidoByte);
                    model = await ImportarArquivoXmlNFESemGravar(idCompra, arquivoStringConvertidoDeByte);
                    model.CaminhoArquivo = modelArquivo.XmlArquivo.FileName;
                    model.ArquivoXml = arquivoStringConvertidoDeByte;
                    model.idCompra = idCompra;
                    
                    if (OperacaoValida())
                    {
                        LogInformacao($"Objeto importado com sucesso id:{Deserializar(model)}", "Compra", "ImportarXML", null);
                    }
                }
            }
            catch (Exception ex)
            {
                model = new agilium.api.business.Models.CustomReturn.ComprasNFEViewModel.NFeProc();
                AdicionarErroValidacao("Formato do arquivo XML invalido");
                LogErro(ex.Message, "Compra", "ImportarXML",null,"Web");
            }

            return PartialView("RetornoXmlNfeImportada", model);
        }


        [Route("cadastro-produto-automatico")]
        [ClaimsAuthorizeAttribute(2070)]
        public async Task<ActionResult> CadastroAutomaticoProduto(long id)
        {
            var model = _mapper.Map<CompraViewModel>( await _compraService.ObterPorId(id));

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
                return RedirectToAction("IndexCompra");
            }

            return View(model);
        }

        [Route("cadastro-produto-automatico")]
        [HttpPost]
        public async Task<IActionResult> CadastroAutomaticoProduto(CompraViewModel viewModel)
        {
            await _compraService.RealizarCadastroProdutoAutomatico(viewModel.Id);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Compra", "CadastroAutomaticoProduto", "Web"));

                PopularListasAuxiliares(viewModel);
                ObterDadosCompraParaViewBag(viewModel.Id);
                return View(viewModel);
            }
            LogInformacao($"Cadastro automatico de produtos:{Deserializar(viewModel)}", "Compra", "CadastroAutomaticoProduto", null);
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexCompra");
        }

        [Route("efetivar")]
        [ClaimsAuthorizeAttribute(2072)]
        public async Task<ActionResult> Efetivar(long id)
        {
            var model = _mapper.Map<CompraViewModel>(await _compraService.ObterPorId(id));

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
                return RedirectToAction("IndexCompra");
            }

            return View(model);
        }


        [Route("efetivar")]
        [HttpPost]
        public async Task<IActionResult> Efetivar(CompraViewModel viewModel)
        {
            string nomeUsuario = ObterNomeUsuarioLogado();

            await _compraService.EfetivarCompra(viewModel.Id, nomeUsuario);

            if (!OperacaoValida())
            {
                PopularListasAuxiliares(viewModel);
                ObterDadosCompraParaViewBag(viewModel.Id);

                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Compra", "Efetivar", "Web");
                return View(viewModel);
            }
            LogInformacao("Objeto efetivado com sucesso id:{viewModel.Id}", "Compra", "Efetivar", null);
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexCompra");
        }

        #endregion

        #region Item Compra

        [Route("IndexItem")]
        public async Task<IActionResult> IndexItem(string id)
        {
            long _id = Convert.ToInt64(id);
            var lista = _compraService.ObterItensPorCompra(_id).Result;
            var listaViewModel = new List<CompraItemViewModel>();
            lista.ForEach(async item => {
                var viewModel = await ConverterObjetoEmViewModel(item);
                listaViewModel.Add(viewModel);
            });

            return PartialView("_indexItem", lista);
        }

        [Route("ListaItemCompra")]
        public async Task<IActionResult> ListaItemCompra(long idCompra)
        {
            ObterDadosCompraParaViewBag(idCompra);

            long _id = Convert.ToInt64(idCompra);
            var lista = _compraService.ObterItensPorCompra(_id).Result;
            var listaViewModel = new List<CompraItemViewModel>();
            lista.ForEach(async item => {
                var viewModel = await ConverterObjetoEmViewModel(item);
                listaViewModel.Add(viewModel);
            });

            return View(listaViewModel);
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
        public async Task<IActionResult> AdicionarItem(CompraItemViewModel viewModel)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "AdicionarItem";

            PopularListasAuxiliares(viewModel);
            ObterDadosCompraParaViewBag(viewModel.IDCOMPRA.Value);

            if (!ModelState.IsValid) return View("_createEditItemCompra", viewModel);

            if (viewModel.Id == 0) viewModel.Id = await GerarId();

            if (!viewModel.ValorAliquotaCofins.HasValue) viewModel.ValorAliquotaCofins = 0;
            if (!viewModel.ValorPis.HasValue) viewModel.ValorPis = 0;
            if (!viewModel.ValorAliquotaPis.HasValue) viewModel.ValorAliquotaPis = 0;
            if (!viewModel.ValorAliquotaIcms.HasValue) viewModel.ValorAliquotaIcms = 0;
            if (!viewModel.ValorAliquotaIpi.HasValue) viewModel.ValorAliquotaIpi = 0;
            if (!viewModel.ValorBaseCalculoCofins.HasValue) viewModel.ValorBaseCalculoCofins = 0;
            if (!viewModel.ValorBaseCalculoIcms.HasValue) viewModel.ValorBaseCalculoIcms = 0;
            if (!viewModel.ValorBaseCalculoIpi.HasValue) viewModel.ValorBaseCalculoIpi = 0;
            if (!viewModel.ValorBaseCalculoPis.HasValue) viewModel.ValorBaseCalculoPis = 0;
            if (!viewModel.ValorBaseRetido.HasValue) viewModel.ValorBaseRetido = 0;
            if (!viewModel.ValorIpi.HasValue) viewModel.ValorIpi = 0;
            if (!viewModel.ValorOUTROS.HasValue) viewModel.ValorOUTROS = 0;
            if (!viewModel.ValorTotal.HasValue) viewModel.ValorTotal = 0;
            if (!viewModel.ValorUnitario.HasValue) viewModel.ValorUnitario = 0;
            if (!viewModel.ValorNovoPrecoVenda.HasValue) viewModel.ValorNovoPrecoVenda = 0;

            viewModel.AtualizarDoubles();
            var objeto = _mapper.Map<CompraItem>(viewModel);

            await _compraService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo item de compra" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("_createEditItemCompra", viewModel); ;
            }
            await _compraService.Salvar();
            LogInformacao($"Objeto efetivado com sucesso id:{Deserializar(objeto)}", "Compra", "AdicionarItem", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaItemCompra", new { idCompra = viewModel.IDCOMPRA });
        }

        [Route("item/editar")]
        public async Task<IActionResult> EditarItem(long id)
        {
            ViewBag.acao = "EditarItem";
            ViewBag.operacao = "E";

            ;
            var model =  _mapper.Map<CompraItemViewModel>(_compraService.ObterItemPorId(id).Result);
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

            model.AtualizarStrings();

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

            model.AtualizarDoubles();
            var objeto = _mapper.Map<CompraItem>(model);

            await _compraService.Atualizar(objeto);

            await _compraService.Salvar();

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao editar item de compra" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("_createEditItemCompra", model);
            }
            LogInformacao($"Objeto efetivado com sucesso id:{Deserializar(objeto)}", "Compra", "EditarItem", null);
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("ListaItemCompra", new { idCompra = model.IDCOMPRA });
        }

        [Route("EditarItemModal")]
        public async Task<IActionResult> EditarItemModal(long id)
        {
            var model = _mapper.Map<CompraItemViewModel>(_compraService.ObterItemPorId(id).Result);
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
            model.AtualizarStrings();

            return View("_editarItemCompra", viewModel);
        }

        [Route("EditarItemModal")]
        [HttpPost]
        public async Task<IActionResult> EditarItemModal(CompraItemEditViewModel model)
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            ObterDadosCompraParaViewBag(model.IDCOMPRA.Value);
            
            if (!ModelState.IsValid)
            {
                PopularListasAuxiliares(model);
                model.certo = "S";
                model.Importada = true;
                return PartialView("_editarItemCompra", model);
            }
            model.AtualizarDoubles();
           

            if(!await _compraService.AtualizarProdutoNoItemCompra(model.Id, model.IDCOMPRA.Value, model.IDPRODUTO, model.IDESTOQUE, model.SGUN, 
                model.Quantidade,model.Relacao, model.ValorUnitario, model.ValorTotal, model.ValorNovoPrecoVenda))
            {

            }

            if (!OperacaoValida())
            {
                PopularListasAuxiliares(model);
                model.certo = "S";
                model.Importada = true;
                model.SGUN = "0101";
                return PartialView("_editarItemCompra", model);
            }

            LogInformacao($"Objeto efetivado com sucesso id:{Deserializar(model)}", "Compra", "EditarItemModal", null);
            var lista = await _compraService.ObterItemPorId(model.IDCOMPRA.Value);

            var url = Url.Action("ListaItemCompra", "Compra", new { idCompra = model.IDCOMPRA });

            return RedirectToAction("ListaItemCompra", new { idCompra = model.IDCOMPRA });

        }

        #endregion

        #region metodos privados

        private async Task<agilium.api.business.Models.CustomReturn.ComprasNFEViewModel.NFeProc> ImportarArquivoXmlNFESemGravar(long idCompra, string ArquivoXml)
        {
            var nFeProc = new agilium.api.business.Models.CustomReturn.ComprasNFEViewModel.NFeProc();
            MemoryStream xmlStream = new MemoryStream();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(ArquivoXml);
            xmlDoc.Save(xmlStream);
            xmlStream.Flush();//Adjust this if you want read your data 
            xmlStream.Position = 0;

            var serializer = new XmlSerializer(typeof(agilium.api.business.Models.CustomReturn.ComprasNFEViewModel.NFeProc));
            nFeProc = (agilium.api.business.Models.CustomReturn.ComprasNFEViewModel.NFeProc)serializer.Deserialize(xmlStream);

            return nFeProc;
        }
        private async Task<TabelasAxuliaresFiscalViewModel> ObterTabelasAuxiliaresFiscal()
        {
            var objeto = new TabelasAxuliaresFiscalViewModel();
            objeto.Cests = _mapper.Map<List<CestViewModel>>(await _tabelaAuxiliarFiscalService.ObterTodosCestNcm());
            objeto.Csosn = _mapper.Map<List<CsosnViewModel>>(await _tabelaAuxiliarFiscalService.ObterTodosCsosn());
            objeto.Csts = _mapper.Map<List<CstViewModel>>(await _tabelaAuxiliarFiscalService.ObterTodosCst());
            objeto.Cfops = _mapper.Map<List<CfopViewModel>>(await _tabelaAuxiliarFiscalService.ObterTodosCfop());

            return objeto;
        }

        private async Task<PagedViewModel<CompraViewModel>> ObterListaCompraPaginado(long idempresa, DateTime dtIni, DateTime dtFinal, int page, int pageSize)
        {
            var lista = new List<CompraViewModel>();
            var retorno = await _compraService.ObterCompraPorPaginacaoDapper(idempresa, dtIni, dtFinal, page, pageSize);

            return new PagedViewModel<CompraViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "IndexCompra",
                ReferenceController ="compra",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<PagedViewModel<CompraIndexViewModel>> ObterListaCompraIndexPaginado(long idempresa, DateTime dtIni, DateTime dtFinal, int page, int pageSize)
        {
            //var lista = new List<CompraIndexViewModel>();
            var retorno = await _compraService.ObterCompraPorPaginacaoDapper(idempresa, dtIni, dtFinal, page, pageSize);
            var lista = _mapper.Map<List<CompraIndexViewModel>>(retorno.List);
            return new PagedViewModel<CompraIndexViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "lista",
                ReferenceController = "compra",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<CompraViewModel> ConverterObjetoEmViewModel(Compra dev)
        {
            var viewModel = _mapper.Map<CompraViewModel>(dev);

            if (dev.IDFORN.HasValue)
            {
                var fornecedor = _fornecedorService.ObterPorId(dev.IDFORN.Value).Result;
                viewModel.NomeFornecedor = fornecedor != null && !string.IsNullOrEmpty(fornecedor.NMRZSOCIAL) ? fornecedor.NMRZSOCIAL : string.Empty;
            }

            if (dev.IDTURNO.HasValue)
            {
                //var turno = _turnoService.Obterpo(dev.IDTURNO.Value).Result;
                //viewModel.NomeTurno = turno != null && turno.NUTURNO.HasValue ? $"{turno.DTTURNO?.ToString("dd/MM/yyyy")} - Nº {turno.NUTURNO.ToString()}" : string.Empty;
            }

            return viewModel;
        }

        private async Task<CompraItemViewModel> ConverterObjetoEmViewModel(CompraItem objeto)
        {
            var viewModel = _mapper.Map<CompraItemViewModel>(objeto);
            if (objeto.IDPRODUTO.HasValue)
            {
                var produto = _produtoService.ObterPorId(objeto.IDPRODUTO.Value).Result;
                viewModel.NomeProduto = produto != null && !string.IsNullOrEmpty(produto.NMPRODUTO) ? produto.NMPRODUTO : "";
                viewModel.CodigoProdutoFornecedor = produto != null && !string.IsNullOrEmpty(produto.CDPRODUTO) ? produto.CDPRODUTO : "";
            }

            if (objeto.IDESTOQUE.HasValue)
            {
                var estoque = _estoqueService.ObterPorId(objeto.IDESTOQUE.Value).Result;
                viewModel.NomeEstoque = estoque != null && !string.IsNullOrEmpty(estoque.Descricao) ? estoque.Descricao : "";
            }

            return viewModel;
        }

        private void PopularListasAuxiliares(CompraViewModel model)
        {
            if (model.Empresas.Count() == 0)
                model.Empresas = listaEmpresaViewModels.ToList();

            if (model.Fornecedores.Count() == 0)
                model.Fornecedores = listaFornecedorViewModels.ToList();

            if (model.Cfops.Count() == 0)
                model.Cfops = Cfops.ToList();

            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (model.Turnos.Count() == 0)
            {
                if (Convert.ToInt64(empresaSelecionada.IDEMPRESA) > 0)
                {
                    var lista =  _turnoService.ObterTodos(Convert.ToInt64(empresaSelecionada.IDEMPRESA)).Result;
                    model.Turnos = _mapper.Map<List<TurnoIndexViewModel>>(lista);
                }
            }

        }


        private void PopularListasAuxiliares(CompraItemViewModel model)
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (model.Produtos.Count() == 0)
                model.Produtos = _mapper.Map<List<ViewModels.Produtos.ProdutoViewModel>>(_produtoService.ObterTodas(Convert.ToInt64(empresaSelecionada.IDEMPRESA)).Result.ToList());

            if (model.Estoques.Count() == 0)
                model.Estoques = _mapper.Map<List<EstoqueViewModel>>(_estoqueService.ObterTodas().Result.ToList());

            if (model.Unidades.Count() == 0)
                model.Unidades = _mapper.Map<List<UnidadeIndexViewModel>>(_unidadeService.ObterTodas().Result.ToList());
        }

        private void PopularListasAuxiliares(CompraItemEditViewModel model)
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (model.Produtos.Count() == 0)
                model.Produtos = _mapper.Map<List<ViewModels.Produtos.ProdutoViewModel>>(_produtoService.ObterTodas(Convert.ToInt64(empresaSelecionada.IDEMPRESA)).Result.ToList());

            if (model.Estoques.Count() == 0)
                model.Estoques = _mapper.Map<List<EstoqueViewModel>>(_estoqueService.ObterTodas().Result.ToList());

            if (model.Unidades.Count() == 0)
                model.Unidades = _mapper.Map<List<UnidadeIndexViewModel>>(_unidadeService.ObterTodas().Result.ToList());
        }

        private async Task<CompraViewModel> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var objeto = await _compraService.ObterPorId(_id);
            return await ConverterObjetoEmViewModel(objeto);
                
        }

        private void ObterDadosCompraParaViewBag(long idCompra)
        {
            var compra = Obter(idCompra.ToString()).Result;
            if (compra == null)
            {
                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Compra";
                TempData["Mensagem"] = "Erro ao tenta localizar item da compra";
                return;
            }
            ViewBag.NomeCompra = compra.Codigo;
            ViewBag.idCompra = compra.Id;
            ViewBag.importada = compra.Importada == ESimNao.Sim;
            ViewData["importada"] = ViewBag.importada;
        }

        private string ObterNomeUsuarioLogado()
        {
            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
            var nomeUsuario = usuario != null ? usuario.nome : AppUser.GetUserEmail();
            return nomeUsuario;
        }

        private async Task<IFormFile> ConverterArquivoFormFile(ImportacaoArquivo model)
        {
            using var form = new MultipartFormDataContent();
            var itemContent = ObterConteudoImagem(model.XmlArquivo);

            form.Add(ObterConteudo(model.idCompra), "idCompra");
            byte[] data;
            if (model.XmlArquivo != null)
            {
                using (var br = new BinaryReader(model.XmlArquivo.OpenReadStream()))
                {
                    data = br.ReadBytes((int)model.XmlArquivo.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                form.Add(bytes, "XmlArquivo", model.XmlArquivo.FileName);
            }

            return form as IFormFile;
        }

        protected StringContent ObterConteudoImagem(object dado)
        {

            return new StringContent(
                JsonSerializer.Serialize(dado),
                Encoding.UTF8,
                "multipart/form-data");
        }

        protected StringContent ObterConteudo(object dado)
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true,

            };
            return new StringContent(
                JsonSerializer.Serialize(dado, options),
                Encoding.UTF8,
                "application/json");
        }


        protected async Task<IFormFile> ConverterToIFormFile(byte[] file, string fileName, string fileNameExtensao)
        {
            var stream = new MemoryStream(file);
            return new FormFile(stream, 0, stream.Length, fileName, fileNameExtensao);
        }

        protected async Task<byte[]> ConverterFormFileToByte(IFormFile formFile)
        {
            long length = formFile.Length;
            if (length < 0)
                return null;

            using var fileStream = formFile.OpenReadStream();
            byte[] bytes = new byte[length];
            fileStream.Read(bytes, 0, (int)formFile.Length);

            return bytes;
        }

        protected async Task<string> ConverterByteToString(byte[] byteArray)
        {
            return Encoding.Default.GetString(byteArray);
        }

        #endregion

    }
}

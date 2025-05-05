using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.manager.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using agilium.api.business.Models;
using agilium.api.manager.ViewModels.CompraViewModel;
using System.Linq;
using Microsoft.AspNetCore.Http;
using agilium.api.business.Models.CustomReturn.ComprasNFEViewModel;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Microsoft.OpenApi.Validations;
using SharpCompress.Writers;

namespace agilium.api.manager.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/compra")]
    [ApiVersion("1.0")]
    public class CompraController : MainController
    {
        private readonly ICompraService _compraService;
        private readonly ITurnoService _turnoService;
        private readonly IProdutoService _produtoService;
        private readonly IEstoqueService _estoqueService;
        private readonly IUsuarioService _usuarioService;

        private readonly IMapper _mapper;
        private readonly IFornecedorService _fornecedorService;
        public CompraController(INotificador notificador, IUser appUser, IConfiguration configuration, ICompraService compraService, IMapper mapper,
            ITurnoService turnoService, IFornecedorService fornecedorService, IProdutoService produtoService, IEstoqueService estoqueService, 
            IUtilDapperRepository utilDapperRepository, IUsuarioService usuarioService, ILogService logService) : base(notificador, appUser, configuration, 
                utilDapperRepository, logService)
        {
            _compraService = compraService;
            _mapper = mapper;
            _turnoService = turnoService;
            _fornecedorService = fornecedorService;
            _produtoService = produtoService;
            _estoqueService = estoqueService;
            _usuarioService = usuarioService;
        }

        #region Compra

        [HttpGet("obter-por-data")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CompraViewModel>>> ObterPorDataPaginacao([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string dtInicial = null, [FromQuery] string dtFinal = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();
            DateTime _dtInicial = Convert.ToDateTime(dtInicial);
            DateTime _dtFinal = Convert.ToDateTime(dtFinal);
            var lista = (await ObterListaCompraPaginado(idEmpresa, _dtInicial, _dtFinal, page, ps));

            return CustomResponse(lista);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] CompraViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Id == 0) viewModel.Id = await GerarId();
            if (!viewModel.DataCadastro.HasValue)
                viewModel.DataCadastro = DateTime.Now;
            if (!viewModel.DataCompra.HasValue)
                viewModel.DataCompra = DateTime.Now;
            if (!viewModel.ValorBaseCalculoIcms.HasValue) viewModel.ValorBaseCalculoIcms = 0;
            if (!viewModel.ValorBaseCalculoSub.HasValue) viewModel.ValorBaseCalculoSub = 0;
            if (!viewModel.ValorDesconto.HasValue) viewModel.ValorDesconto = 0;
            if (!viewModel.ValorFrete.HasValue) viewModel.ValorFrete = 0;
            if (!viewModel.ValorIcms.HasValue) viewModel.ValorIcms = 0;
            if (!viewModel.ValorIcmsRetido.HasValue) viewModel.ValorIcmsRetido = 0;
            if (!viewModel.ValorIcmsSub.HasValue) viewModel.ValorIcmsSub = 0;
            if (!viewModel.ValorIpi.HasValue) viewModel.ValorIpi = 0;
            if (!viewModel.ValorIsencao.HasValue) viewModel.ValorIsencao = 0;
            if (!viewModel.ValorOutros.HasValue) viewModel.ValorOutros = 0;
            if (!viewModel.ValorSeguro.HasValue) viewModel.ValorSeguro = 0;
            if (!viewModel.ValorTotal.HasValue) viewModel.ValorTotal = 0;
            if (!viewModel.ValorTotalProduto.HasValue) viewModel.ValorTotalProduto = 0;

            var objeto = _mapper.Map<Compra>(viewModel);

            await _compraService.Adicionar(objeto);
                       
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Compra", "Adicionar", "Web");
                return CustomResponse(msgErro);
            }
            await _compraService.Salvar();
            LogInformacao($"Objeto Criado com sucesso {Deserializar(objeto)}", "Cliente", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] CompraViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                ObterNotificacoes("Compra", "Atualizar", "Web");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (!viewModel.ValorBaseCalculoIcms.HasValue) viewModel.ValorBaseCalculoIcms = 0;
            if (!viewModel.ValorBaseCalculoSub.HasValue) viewModel.ValorBaseCalculoSub = 0;
            if (!viewModel.ValorDesconto.HasValue) viewModel.ValorDesconto = 0;
            if (!viewModel.ValorFrete.HasValue) viewModel.ValorFrete = 0;
            if (!viewModel.ValorIcms.HasValue) viewModel.ValorIcms = 0;
            if (!viewModel.ValorIcmsRetido.HasValue) viewModel.ValorIcmsRetido = 0;
            if (!viewModel.ValorIcmsSub.HasValue) viewModel.ValorIcmsSub = 0;
            if (!viewModel.ValorIpi.HasValue) viewModel.ValorIpi = 0;
            if (!viewModel.ValorIsencao.HasValue) viewModel.ValorIsencao = 0;
            if (!viewModel.ValorOutros.HasValue) viewModel.ValorOutros = 0;
            if (!viewModel.ValorSeguro.HasValue) viewModel.ValorSeguro = 0;
            if (!viewModel.ValorTotal.HasValue) viewModel.ValorTotal = 0;
            if (!viewModel.ValorTotalProduto.HasValue) viewModel.ValorTotalProduto = 0;
            
            var objeto = _mapper.Map<Compra>(viewModel);

            await _compraService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Compra", "Atualizar", "Web");
                return CustomResponse(msgErro);
            }

            await _compraService.Salvar();
            LogInformacao($"Objeto atualizar com sucesso {Deserializar(objeto)}", "Cliente", "ATUALIZAR", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CompraViewModel>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var objeto = await _compraService.ObterPorId(_id);
            if (objeto != null)
            {
                var viewModel = await ConverterObjetoEmViewModel(objeto);
                return CustomResponse(viewModel);
            }

            return CustomResponse(BadRequest("Compra nao localizada"));

        }

        [HttpGet("cancelar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Cancelar(long id)
        {
            string nomeUsuario = ObterNomeUsuarioLogado();

            await _compraService.CancelarCompra(id, nomeUsuario);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                return CustomResponse(msgErro);
            }
            LogInformacao($"Objeto cancelado com sucesso id:{id}", "Cliente", "Cancelar", null);
            return CustomResponse();
        }

        private string ObterNomeUsuarioLogado()
        {
            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
            var nomeUsuario = usuario != null ? usuario.nome : AppUser.GetUserEmail();
            return nomeUsuario;
        }

        [HttpGet("efetivar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Efetivar(long id)
        {
            string nomeUsuario = ObterNomeUsuarioLogado();

            await _compraService.EfetivarCompra(id, nomeUsuario);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                ObterNotificacoes("Compra", "Efetivar", "Web");
                return CustomResponse(msgErro);
            }
            LogInformacao($"Objeto efetivado com sucesso id:{id}", "Cliente", "Efetivar", null);
            return CustomResponse();
        }

        [HttpGet("cadastrar-produto-automaticamente/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> CadastroAutomaticoProduto(long id)
        {
            string nomeUsuario = ObterNomeUsuarioLogado();

            await _compraService.RealizarCadastroProdutoAutomatico(id);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Compra", "CadastroAutomaticoProduto", "Web"));

                return CustomResponse(msgErro);
            }

            return CustomResponse();
        }
        #endregion

        #region Item

        [HttpGet("itens/{idCompra}")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CompraItemViewModel>>> ObterItemCompraViewModel(long idCompra)
        {
            var listaObjeto = _compraService.ObterItensPorCompra(idCompra).Result;
            var listaViewModel = new List<CompraItemViewModel>();
            listaObjeto.ForEach(async item => {
                var viewModel = await ConverterObjetoEmViewModel(item);
                listaViewModel.Add(viewModel);  
            });

            return CustomResponse(listaViewModel);
        }

        [HttpPost("item")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> AdicionarItem([FromBody] CompraItemViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            if (viewModel.Id == 0) viewModel.Id = await GerarId();

            if (!viewModel.ValorAliquotaCofins.HasValue) viewModel.ValorAliquotaCofins = 0;
            if (!viewModel.ValorPis.HasValue) viewModel.ValorPis  = 0;
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

            var objeto = _mapper.Map<CompraItem>(viewModel);

            await _compraService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Compra", "AdicionarItem", "Web"));
                return CustomResponse(msgErro);
            }
            await _compraService.Salvar();
            LogInformacao($"Objeto efetivado com sucesso id:{Deserializar(objeto)}", "Compra", "AdicionarItem", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("item/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> AtualizarItem(string id, [FromBody] CompraItemViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<CompraItem>(viewModel);

            await _compraService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Compra", "AtualizarItem", "Web"));
                return CustomResponse(msgErro);
            }

            await _compraService.Salvar();
            LogInformacao($"Objeto efetivado com sucesso id:{Deserializar(viewModel)}", "Compra", "AtualizarItem", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("item/produto/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> AtualizarProdutoItemCompra(string id, [FromBody] CompraItemEditViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                ObterNotificacoes("Compra", "AtualizarProdutoItemCompra", "Web");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _compraService.AtualizarProdutoNoItemCompra(viewModel.Id,viewModel.IDCOMPRA.Value,viewModel.IDPRODUTO,viewModel.IDESTOQUE,viewModel.SGUN,viewModel.Quantidade,
                viewModel.Relacao,viewModel.ValorUnitario,viewModel.ValorTotal,viewModel.ValorNovoPrecoVenda);
            
           // NotificarErro("Erro Teste");
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Compra", "AtualizarProdutoItemCompra", "Web"));
                return CustomResponse(msgErro);
            }

            LogInformacao($"Objeto atualizado com sucesso id:{Deserializar(viewModel)}", "Compra", "AtualizarProdutoItemCompra", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("item/obter-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CompraViewModel>> ObterItem(string id)
        {
            long _id = Convert.ToInt64(id);
            var objeto = await _compraService.ObterItemPorId(_id);
            
            if (objeto != null)
            {
                var viewModel = await ConverterObjetoEmViewModel(objeto);
                return CustomResponse(viewModel);
            }

            return CustomResponse(BadRequest("Item da Compra nao localizado"));
        }

        [HttpPost("item/importar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Importar([FromBody] NFeProc viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _compraService.ImportarCompraDeXmlNfe(viewModel, viewModel.idCompra);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Compra", "importar", "Web"));
                return CustomResponse(msgErro);
            }
            LogInformacao($"Objeto importado com sucesso id:{Deserializar(viewModel)}", "Compra", "Importar", null);
            return CustomResponse(viewModel);
        }

        [HttpPost("item/importar-arquivo")]
        [RequestSizeLimit(bytes: 40_000_000)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> ImportarArquivo([FromBody] ImportacaoArquivo viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Compra", "ImportarArquivo", "Web"));
                return CustomResponse(msgErro);
            }

            return CustomResponse(viewModel);
        }

        [HttpPost("item/importar-xml")]
        [RequestSizeLimit(bytes: 80_000_000)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> ImportarArquivoXml([FromForm]IFormFile XmlArquivo, [FromForm] long idCompra)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var arquivoConvertidoByte = await ConverterFormFileToByte(XmlArquivo);

            var arquivoStringConvertidoDeByte = await ConverterByteToString(arquivoConvertidoByte);
            var nfeProc = await _compraService.ImportarArquivoXmlNFE(idCompra, arquivoStringConvertidoDeByte);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Compra", "ImportarArquivoXml", "Web"));
                return CustomResponse(msgErro);
            }
            LogInformacao($"Objeto xml importado com sucesso id:{Deserializar(nfeProc)}", "Compra", "ImportarArquivoXml", null);
            return CustomResponse(nfeProc);
        }
        #endregion

        #region Fiscal
        #endregion

        #region private
        private async Task<PagedResult<CompraViewModel>> ObterListaCompraPaginado(long idempresa, DateTime dtIni, DateTime dtFinal, int page, int pageSize)
        {
            var lista = new List<CompraViewModel>();
            var retorno = await _compraService.ObterCompraPorPaginacao(idempresa, dtIni, dtFinal, page, pageSize);

            retorno.List.ToList().ForEach(async dev =>
            {
                CompraViewModel viewModel = await ConverterObjetoEmViewModel(dev);

                lista.Add(viewModel);
            });
            return new PagedResult<CompraViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
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
                var turno = _turnoService.ObterCompletoPorId(dev.IDTURNO.Value).Result;
                viewModel.NomeTurno = turno != null && turno.NUTURNO.HasValue ? $"{turno.DTTURNO?.ToString("dd/MM/yyyy")} - Nº {turno.NUTURNO.ToString()}"  : string.Empty;
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

        #endregion
    }
}

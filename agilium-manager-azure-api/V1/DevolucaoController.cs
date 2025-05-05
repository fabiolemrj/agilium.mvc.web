using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.ViewModels.DevolucaoViewModel;
using agilium.api.manager.ViewModels.EmpresaViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace agilium.api.manager.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/devolucao")]
    [ApiVersion("1.0")]
    public class DevolucaoController : MainController
    {
        private readonly IDevolucaoService _devolucaoService;
        private readonly IEmpresaService _empresaService;
        private readonly IDevolucaoDapperRepository _devolucaoDapperRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly IValeService _valeService;
        private readonly IClienteService _clienteService;
        private readonly IVendaService _vendaService;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;
        public DevolucaoController(INotificador notificador, IUser appUser, IDevolucaoService devolucaoService,
            IEmpresaService empresaService, IMapper mapper, IConfiguration configuration,
            IDevolucaoDapperRepository devolucaoDapperRepository, IUsuarioService usuarioService, IValeService valeService, 
            IClienteService clienteService, IVendaService vendaService, IProdutoService produtoService, 
            IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser, configuration, utilDapperRepository, logService)
        {
            _devolucaoService = devolucaoService;
            _empresaService = empresaService;
            _mapper = mapper;
            _devolucaoDapperRepository = devolucaoDapperRepository;
            _usuarioService = usuarioService;
            _valeService = valeService;
            _clienteService = clienteService;
            _vendaService = vendaService;
            _produtoService = produtoService;
        }

        #region Devolucao
        [HttpGet("obter-por-data")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<DevolucaoViewModel>>> ObterPorDataPaginacao([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string dtInicial = null, [FromQuery] string dtFinal = null)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();
            DateTime _dtInicial = Convert.ToDateTime(dtInicial);
            DateTime _dtFinal = Convert.ToDateTime(dtFinal);
            var lista = (await ObterListaDevolucaoPaginado(idEmpresa,_dtInicial, _dtFinal, page, ps));

            return CustomResponse(lista);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] DevolucaoViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Id == 0) viewModel.Id = await GerarId();
            if (!viewModel.DataHora.HasValue)
                viewModel.DataHora = DateTime.Now;

            var devolucao = _mapper.Map<Devolucao>(viewModel);

            await _devolucaoService.Adicionar(devolucao);

            if (viewModel.DevolucaoItens.Count > 0)
            {
                viewModel.DevolucaoItens.ForEach(item =>
                {
                    item.idDevolucao = devolucao.Id;

                });
            }

            if (!AdicionarItens(viewModel.DevolucaoItens).Result)
                NotificarErro("Erro ao tentar adicionar Item da devolução");

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Devolucao", "Adicionar", "Web"));
                return CustomResponse(msgErro);
            }
            await _devolucaoService.Salvar();
            LogInformacao($"Objeto Criado com sucesso {Deserializar(devolucao)}", "Devolucao", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(string id, [FromBody] DevolucaoViewModel viewModel)
        {
            if (id != viewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var devolucao = _mapper.Map<Devolucao>(viewModel);

            await _devolucaoService.Atualizar(devolucao);

            if (!AdicionarItens(viewModel.DevolucaoItens).Result)
                NotificarErro("Erro ao tentar atualizar Item da devolução");

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Devolucao", "Atualizar", "Web"));
                return CustomResponse(msgErro);
            }

            await _devolucaoService.Salvar();
            LogInformacao($"Objeto atualizado com sucesso {Deserializar(devolucao)}", "Devolucao", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("cancelar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> Cancelar(long id)
        {
            var devolucao = await _devolucaoService.ObterPorId(id);
                       
            if (devolucao == null) return NotFound();

            if (devolucao.STDEV != business.Enums.ESituacaoDevolucao.Aberta)
            {
                NotificarErro("As devoluções devem estar com  situação aberta.");
                var msgErro = string.Join("\n\r", ObterNotificacoes("Devolucao", "Cancelar", "Web"));
                return CustomResponse(msgErro);
            }

            devolucao.Cancelar();

            await _devolucaoService.Atualizar(devolucao);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Devolucao", "Cancelar", "Web"));
                return CustomResponse(msgErro);
            }
            await _devolucaoService.Salvar();
            LogInformacao($"Cancelamento {Deserializar(devolucao)}", "Devolucao", "Cancelar", null);
            return CustomResponse(devolucao);
        }

        [HttpGet("{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DevolucaoViewModel>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var devolucao = await ObterDevolucaoViewModelPorId(_id);
            if (devolucao != null)
            {
                var objeto = _mapper.Map<DevolucaoViewModel>(devolucao);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("Devolução nao localizada"));

        }


        [HttpGet("obter-todos-por-empresa/{idEmpresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<DevolucaoViewModel>>> ObterTodos(long idEmpresa)
        {
            long _id = Convert.ToInt64(idEmpresa);
            var vale = await _devolucaoService.ObterTodasDevolucoes(_id);

            return CustomResponse(vale);
        }

        [HttpGet("realizar/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Realizar(long id)
        {
            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
            var nomeUsuario = usuario != null ? usuario.nome : AppUser.GetUserEmail();
            if(!_devolucaoDapperRepository.RealizarDevolucao(id, nomeUsuario).Result)
            {
                NotificarErro("Erro: Nao foi possivel realizar devolução");
            }
            
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Devolucao", "Realizar", "Web"));
                return CustomResponse(msgErro);
            }
            LogInformacao($"realizar id:{id}", "Devolucao", "Realizar", null);
            return CustomResponse("Devolução realizada com sucesso");
        }


        [HttpGet("gerar-vale/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GerarVale(long id)
        {

            await _valeService.GerarVale(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Devolucao", "GerarVale", "Web"));
                return CustomResponse(msgErro);
            }
            LogInformacao($"realizar id:{id}", "Devolucao", "GerarVale", null);
            return CustomResponse();
        }
        #endregion

        #region Devolucao Item
        [HttpGet("itens/{idDevolucao}")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<DevolucaoItemViewModel>>> ObterItemPorVenda(long idDevolucao)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();
            var lista = (await ObterListaVendaItemPaginado(idDevolucao));

            return CustomResponse(lista);
        }

        [HttpGet("itens/venda/{idDevolucao}/{idVenda}")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<DevolucaoItemVendaViewModel>>> ObterItemDevolucaoVenda(long idDevolucao,long idVenda)
        {
            //ps = ObterQuantidadeLinhasPorPaginas();
            var viewModel = _mapper.Map<List<DevolucaoItemVendaViewModel>>(_devolucaoDapperRepository.ObterItensComVendaItens(idVenda, idDevolucao).Result);
            
            return CustomResponse(viewModel);
        }

        #endregion

        #region MotivoDevolucao

        //[ClaimsAuthorize("ADMINSTRADOR", "CONSULTAR")]
        [HttpGet("motivos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PerfilIndexMotivo([FromQuery] long idEmpresa, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            ps = ObterQuantidadeLinhasPorPaginas();

            var lista = (await ObterMotivos(idEmpresa, q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }



        [HttpPost("motivo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "INCLUIR")]
        public async Task<IActionResult> AdicionarMotivo([FromBody] MotivoDevolucaoViewModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<MotivoDevolucao>(model);

            if (objeto.Id == 0) objeto.Id = objeto.GerarId();
            await _devolucaoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Devolucao", "AdicionarMotivo", "Web"));
                return CustomResponse(msgErro);
            }

            await _devolucaoService.Salvar();
            LogInformacao($"incluir {Deserializar(objeto)}", "Devolucao", "AdicionarMotivo", null);
            return CustomResponse(model);
        }

        [HttpPut("motivo/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ClaimsAuthorize("ADMINSTRADOR", "ALTERAR")]
        public async Task<IActionResult> AtualizarMotivo(string id, [FromBody] MotivoDevolucaoViewModel objetoViewModel)
        {
            if (id != objetoViewModel.Id.ToString())
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(objetoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var objeto = _mapper.Map<MotivoDevolucao>(objetoViewModel);

            await _devolucaoService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Devolucao", "AtualizarMotivo", "Web"));
                return CustomResponse(msgErro);
            }

            await _devolucaoService.Salvar();
            LogInformacao($"incluir {Deserializar(objeto)}", "Devolucao", "AtualizarMotivo", null);
            return CustomResponse(objetoViewModel);
        }

        [HttpDelete("motivo/{id}")]
        //[ClaimsAuthorize("ADMINSTRADOR", "ALTERAR")]
        public async Task<ActionResult> ApagarMotivo(long id)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (!_devolucaoService.ApagarMotivo(id).Result)
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));

                NotificarErro(msgErro);
                return CustomResponse(msgErro);
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Devolucao", "ApagarMotivo", "Web"));
                return CustomResponse(msgErro);
            }
            await _devolucaoService.Salvar();
            LogInformacao($"excluir id:{id}", "Devolucao", "ApagarMotivo", null);
            return CustomResponse();
        }

        [HttpGet("motivo/obter-todos")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MotivoDevolucaoViewModel>>> ObterTodosMotivo()
        {
            var motivos = _mapper.Map<List<MotivoDevolucaoViewModel>>(_devolucaoService.ObterTodosMotivos().Result);

            return CustomResponse(motivos);
        }


        [HttpGet("obter-motivo-por-id/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MotivoDevolucaoViewModel>> ObterMarcaCompletoPorId(long id)
        {
            var departamentos = _devolucaoService.ObterPorIdMotivo(id).Result;

            var model = _mapper.Map<MotivoDevolucaoViewModel>(departamentos);
            model.Empresas = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);

            return CustomResponse(model);
        }
        #endregion

        #region private
        private async Task<PagedResult<MotivoDevolucaoViewModel>> ObterMotivos(long idempresa, string filtro, int page, int pageSize)
        {
            var retorno = await _devolucaoService.ObterMotivoPaginacaoPorDescricao(idempresa, filtro, page, pageSize);
            var listaTeste = retorno.List;
            var lista = _mapper.Map<IEnumerable<MotivoDevolucaoViewModel>>(listaTeste);

            return new PagedResult<MotivoDevolucaoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<PagedResult<DevolucaoViewModel>> ObterListaDevolucaoPaginado(long idempresa, DateTime dtIni, DateTime dtFinal, int page, int pageSize)
        {
            var lista = new List<DevolucaoViewModel>();
            //  var retorno = await _devolucaoService.ObterDevolucaoPorPaginacao(idempresa, dtIni, dtFinal, page, pageSize);
            var retorno = await _devolucaoService.ObterDevolucaoPorPaginacao(idempresa, dtIni, dtFinal, page, pageSize);

            retorno.List.ToList().ForEach(dev => {
                var cliente = dev.IDCLIENTE.HasValue ? _clienteService.ObterPorId(dev.IDCLIENTE.Value).Result : null;
                var vendaViewModel = _mapper.Map<DevolucaoViewModel>(dev);
                vendaViewModel.ClienteNome = cliente != null && !string.IsNullOrEmpty(cliente.NMCLIENTE) ? cliente.NMCLIENTE : string.Empty;

                vendaViewModel.MotivoDevolucaoNome = dev.MotivoDevolucao != null && !string.IsNullOrEmpty(dev.MotivoDevolucao.DSMOTDEV) ? dev.MotivoDevolucao.DSMOTDEV : string.Empty;
                vendaViewModel.VendaNome = dev.Venda != null && dev.Venda.SQVENDA.HasValue ? dev.Venda.SQVENDA.ToString() : string.Empty;
                vendaViewModel.EmpresaNome = dev.Empresa != null && !string.IsNullOrEmpty(dev.Empresa.NMRZSOCIAL) ? dev.Empresa.NMRZSOCIAL : string.Empty;
                vendaViewModel.VendaData = dev.Venda != null && dev.Venda.DTHRVENDA.HasValue ? dev.Venda.DTHRVENDA.Value.ToString("dd/MM/yyyy"): string.Empty;
                vendaViewModel.CaixaNome = dev.Venda != null && dev.Venda.Caixa != null && dev.Venda.Caixa.SQCAIXA.HasValue ? dev.Venda.Caixa.SQCAIXA.Value.ToString() : string.Empty;
                if (vendaViewModel.IDVALE.HasValue)
                {
                    var vale = _valeService.ObterPorId(vendaViewModel.IDVALE.Value).Result;
                    vendaViewModel.ValeNome = vale != null && !string.IsNullOrEmpty(vale.CDVALE) ? vale.CDVALE : string.Empty;
                }
                    
                lista.Add(vendaViewModel);
            });

            return new PagedResult<DevolucaoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<List<DevolucaoItemViewModel>> ObterListaVendaItemPaginado(long idDevolucao)
        {
            var lista = new List<DevolucaoItemViewModel>();
            var retorno = await _devolucaoService.ObterDevolucaoItens(idDevolucao);

            retorno.ToList().ForEach(devItem => {
                var devItemViewModel = _mapper.Map<DevolucaoItemViewModel>(devItem);

                var vendaItem = _vendaService.ObterItemPorVenda(devItem.IDVENDA_ITEM.Value).Result;
                var produto = _produtoService.ObterPorId(vendaItem.IDPRODUTO.Value).Result;
                
                devItemViewModel.ProdutoNome = produto != null && !string.IsNullOrEmpty(produto.NMPRODUTO) ? produto.NMPRODUTO : string.Empty;
                devItemViewModel.SequencialVenda = vendaItem != null && vendaItem.SQITEM.HasValue ? vendaItem.SQITEM : 0;
                devItemViewModel.ValorItemVenda = vendaItem != null && vendaItem.VLITEM.HasValue ? vendaItem.VLITEM : 0;
                devItemViewModel.DevolucaoNome = devItem.Devolucao != null && !string.IsNullOrEmpty(devItem.Devolucao.CDDEV) ? devItem.Devolucao.CDDEV : string.Empty;
                devItemViewModel.CodigoProduto = produto != null && !string.IsNullOrEmpty(produto.CDPRODUTO) ? produto.CDPRODUTO : string.Empty;
                
                lista.Add(devItemViewModel);
            });

            return lista;
        }

        private async Task<DevolucaoViewModel> ObterDevolucaoViewModelPorId(long id)
        {
            var devolucao = await _devolucaoService.ObterDevolucaoPorId(id);
            var viewModel = _mapper.Map<DevolucaoViewModel>(devolucao);

            viewModel.VendaData = devolucao != null && devolucao.Venda != null && devolucao.Venda.DTHRVENDA.HasValue ? devolucao.Venda.DTHRVENDA.Value.ToString("dd/MM/yyyy") : string.Empty;
            viewModel.DevolucaoItens =  _mapper.Map<List<DevolucaoItemVendaViewModel>>(_devolucaoDapperRepository.ObterItensComVendaItens(devolucao.IDVENDA.Value, devolucao.Id).Result);
            viewModel.Itens = ObterListaVendaItemPaginado(id).Result;


            return viewModel;
        }

        private async Task<bool> AdicionarItens(List<DevolucaoItemVendaViewModel> viewModel)
        {
            var resultado = false;
         
            viewModel.ForEach(async item => {
                if (item.selecionado)
                {
                    
                    var itemDevolucao = new DevolucaoItem(item.idDevolucao, item.idItemVenda, item.QuantidadeVendida, item.ValorTotal);

                    if (item.idDevolucaoItem > 0)
                        itemDevolucao.Id = item.idDevolucaoItem;
                    
                    await _devolucaoService.AdicionarAtualizar(itemDevolucao);
                }

            });
            
            resultado = true;
            return resultado;
        }

        #endregion
    }
}

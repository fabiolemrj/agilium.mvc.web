using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Services;
using agilum.mvc.web.Extensions;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Estoque;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Route("estoque")]
    [Authorize]
    public class EstoqueController : MainController
    {
        #region constantes
        private readonly IEstoqueService _estoqueService;
        private readonly IEmpresaService _empresaService;
        private readonly IProdutoService _produtoService;
        private readonly string _nomeEntidadeMotivo = "Estoque";
        #endregion

        #region listas auxiliares
        private IEnumerable<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();
        #endregion

        #region construtor
        public EstoqueController(IEstoqueService estoqueService, IEmpresaService empresaService, IProdutoService produtoService,
            INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService, IMapper mapper) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper)
        {
            _estoqueService = estoqueService;
            _empresaService = empresaService;
            _produtoService = produtoService;

            listaEmpresaViewModels = _mapper.Map<IEnumerable<EmpresaViewModel>>(_empresaService.ObterTodas().Result);
        }
        #endregion

        #region estoque
        [Route("lista")]
        [ClaimsAuthorizeAttribute(2050)]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
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

            var lista = (await ObterListaPaginado(Convert.ToInt64(empresaSelecionada.IDEMPRESA), q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return View(lista);
        }

        [Route("estoque/novo")]
        [ClaimsAuthorizeAttribute(2051)]
        public async Task<IActionResult> CreateEstoque()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateEstoque";

            var model = new EstoqueViewModel();
            model.situacao = EAtivo.Ativo;
            model.Empresas = listaEmpresaViewModels.ToList();
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada != null && !string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
                model.idEmpresa = Convert.ToInt64(empresaSelecionada.IDEMPRESA);
            return View("CreateEditEstoque", model);
        }

        [HttpPost]
        [Route("estoque/novo")]
        public async Task<IActionResult> CreateEstoque(EstoqueViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateEstoque";
            if (!ModelState.IsValid) return View("CreateEditEstoque", model);

            var estoque = _mapper.Map<Estoque>(model);

            if (estoque.Id == 0) estoque.Id = estoque.GerarId();
            await _estoqueService.Adicionar(estoque);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Estoque", "Adicionar", "Web", Deserializar(estoque)));
                return View("CreateEditEstoque", model);
            }
            await _estoqueService.Salvar();
            LogInformacao($"sucesso: {Deserializar(model)}", "Estoque", "Adicionar", null);
          
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("estoque/editar")]
        [ClaimsAuthorizeAttribute(2054)]
        public async Task<IActionResult> EditEstoque(long id)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditEstoque";
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
            return View("CreateEditEstoque", objeto);
        }

        [HttpPost]
        [Route("estoque/editar")]
        public async Task<IActionResult> EditEstoque(EstoqueViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditEstoque";

            if (!ModelState.IsValid) return View("CreateEditEstoque", model);

            await _estoqueService.Atualizar(_mapper.Map<Estoque>(model));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Estoque", "Atualizar", "Web", Deserializar(model)));
                return View("CreateEditEstoque", model);
            }
            await _estoqueService.Salvar();
            LogInformacao($"sucesso: {Deserializar(model)}", "Estoque", "Atualizar", null);
                       
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("estoque/apagar")]
        [ClaimsAuthorizeAttribute(2052)]
        public async Task<IActionResult> DeleteEstoque(long id)
        {
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


        [HttpPost]
        [Route("estoque/apagar")]
        public async Task<IActionResult> DeleteEstoque(EstoqueViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _estoqueService.Apagar(model.Id);
            
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Estoque", "Excluir", "Web", Deserializar(model)));
                return View(model);
            }
            await _estoqueService.Salvar();
            LogInformacao($"excluir {Deserializar(model)}", "Estoque", "Excluir", null);
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }
        #endregion

        #region Estoque Produto
        [Route("produtos")]
        [ClaimsAuthorizeAttribute(2050)]
        public async Task<ActionResult> ProdutoEstoque(long idEstoque)
        {

            var lista = await _estoqueService.ObterProdutoEstoquePorEstoque(idEstoque);

            var model = new List<ProdutoPorEstoqueViewModel>();
            var produto = new Produto();

            lista.ForEach(prod => {

                if (prod.IDPRODUTO != produto.Id)
                    produto = _produtoService.ObterPorId(prod.IDPRODUTO.Value).Result;

                var estoqueProduto = new ProdutoPorEstoqueViewModel()
                {
                    Id = prod.Id,
                    idProduto = prod.IDPRODUTO.Value,
                    QuantidadeAtual = prod.NUQTD.HasValue ? prod.NUQTD.Value : 0,
                    Produto = produto.NMPRODUTO,
                    ValorCustoMedio = produto.VLCUSTOMEDIO.HasValue ? produto.VLCUSTOMEDIO.Value : 0,
                    ValorUltimaCompra = produto.VLULTIMACOMPRA.HasValue ? produto.VLULTIMACOMPRA.Value : 0,
                    Codigo = produto.CDPRODUTO
                };

                model.Add(estoqueProduto);
            });

            var estoque = _estoqueService.ObterPorId(idEstoque).Result;
            if(estoque != null)
            {
                ViewBag.Estoque = estoque.Descricao;
                ViewBag.idEstoque = idEstoque;
            }

            return View(model);
        }

        #endregion

        #region metodos privados
        private async Task<PagedViewModel<EstoqueViewModel>> ObterListaPaginado(long idempresa, string filtro, int page, int pageSize)
        {
            var retorno = await _estoqueService.ObterPorDescricaoPaginacao(idempresa, filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<EstoqueViewModel>>(retorno.List);

            return new PagedViewModel<EstoqueViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "lista",
                ReferenceController = "estoque",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<EstoqueViewModel> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var objeto = _mapper.Map<EstoqueViewModel>(await _estoqueService.ObterCompletoPorId(_id));
            if(objeto != null) objeto.Empresas = listaEmpresaViewModels.ToList();
            return objeto;
        }

        #endregion
    }

}


using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Services;
using agilum.mvc.web.ViewModels;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Impostos;
using agilum.mvc.web.ViewModels.Produtos;
using agilum.mvc.web.ViewModels.UnidadeViewModel;
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
    [Route("produto")]
    [Authorize]
    public class ProdutoController : MainController
    {
        private readonly IProdutoService _produtoService;
        private readonly IEmpresaService _empresaService;
        private readonly IUnidadeService _unidadeService;
        private readonly IEstoqueService _estoqueService;
        private readonly IClienteService _clienteService;
        private readonly ITurnoService _turnoService;
        private readonly ITabelaAuxiliarFiscalService _tabelaAuxiliarFiscalService;

        private readonly string _nomeEntidadeDepart = "Departamento";
        private readonly string _nomeEntidadeMarca = "Marca";
        private readonly string _nomeEntidadeGrupo = "Grupo de Produtos";
        private readonly string _nomeEntidadeSubGrupo = "SubGrupo de Produtos";

        #region Listas Auxiliares
        private List<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();
        private List<GrupoProdutoViewModel> Grupos { get; set; } = new List<GrupoProdutoViewModel>();
        private List<CfopViewModel> Cfops { get; set; } = new List<CfopViewModel>();
        private List<CstViewModel> Csts { get; set; } = new List<CstViewModel>();
        private List<CestViewModel> Cests { get; set; } = new List<CestViewModel>();
        private List<SubGrupoViewModel> SubGrupos { get; set; } = new List<SubGrupoViewModel>();
        private List<ProdutoDepartamentoViewModel> Departamentos { get; set; } = new List<ProdutoDepartamentoViewModel>();
        private List<ProdutoMarcaViewModel> Marcas { get; set; } = new List<ProdutoMarcaViewModel>();
        private List<UnidadeIndexViewModel> Unidades { get; set; } = new List<UnidadeIndexViewModel>();
        private List<CsosnViewModel> Csosn { get; set; } = new List<CsosnViewModel>();

        #endregion
        public ProdutoController(IProdutoService produtoService, IEmpresaService empresaService, INotificador notificador, 
            IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService, IMapper mapper,
            ITabelaAuxiliarFiscalService tabelaAuxiliarFiscalService,IUnidadeService unidadeService, IEstoqueService estoqueService, 
            IClienteService clienteService, ITurnoService turnoService) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper)
        {
            _produtoService = produtoService;
            _empresaService = empresaService;
            _tabelaAuxiliarFiscalService = tabelaAuxiliarFiscalService;
            _unidadeService = unidadeService;
            _estoqueService = estoqueService;
            _clienteService = clienteService;
            _turnoService = turnoService;

            if (!listaEmpresaViewModels.Any())
                listaEmpresaViewModels = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result.ToList());
            var listasAuxiliaresProdutos = ObterListasAuxiliares().Result;
            Grupos = listasAuxiliaresProdutos.Grupos;
        }

        #region GrupoProduto

        [Route("grupo/lista")]
        public async Task<IActionResult> IndexGrupo([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
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

            var lista = (await ObterGrupo(Convert.ToInt64(empresaSelecionada.IDEMPRESA), q, page, ps));

            ViewBag.Pesquisa = q;

            return View(lista);
        }

        [Route("grupo/novo")]
        public async Task<IActionResult> CreateGrupo()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateGrupo";

            var empresaSelecionada = ObterObjetoEmpresaSelecionada();
            var model = new GrupoProdutoViewModel();
            model.Situacao = EAtivo.Ativo;
            model.Empresas = listaEmpresaViewModels.ToList();
            model.idEmpresa = Convert.ToInt64(empresaSelecionada.IDEMPRESA);
            return View("CreateEditGrupo", model);
        }


        [Route("grupo/novo")]
        [HttpPost]
        public async Task<IActionResult> CreateGrupo(GrupoProdutoViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateGrupo";
            if (!ModelState.IsValid) return View("CreateEditGrupo", model);

            var objeto = _mapper.Map<GrupoProduto>(model);
            
            if (objeto.Id == 0) objeto.Id = objeto.GerarId();
            await _produtoService.Adicionar(objeto);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AdicionarGrupo", "Web", Deserializar(objeto)));
                AdicionarErroValidacao(msgErro);
                return View("CreateEditGrupo", model);
            }

            await _produtoService.Salvar();
          
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexGrupo");
        }

        [Route("grupo/editar")]
        public async Task<IActionResult> EditGrupo(long id)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditGrupo";
            var model = _mapper.Map <GrupoProdutoViewModel>(await _produtoService.ObterPorIdGrupo(id));
            if (model == null)
            {
                var msgErro = $"{_nomeEntidadeGrupo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeGrupo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexGrupo");
            }
            model.Empresas = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);

            return View("CreateEditGrupo", model);
        }

        [Route("grupo/editar")]
        [HttpPost]
        public async Task<IActionResult> EditGrupo(GrupoProdutoViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditGrupo";

            if (!ModelState.IsValid) return View("CreateEditGrupo", model);

            var objeto = _mapper.Map<GrupoProduto>(model);

            await _produtoService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AtualizarGrupo", "Web", Deserializar(objeto)));
                AdicionarErroValidacao(msgErro);
                return View("CreateEditGrupo", model);
            }

            await _produtoService.Salvar();

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexGrupo");
        }

        [Route("grupo/apagar")]
        public async Task<IActionResult> DeleteGrupo(long id)
        {

            var model = _mapper.Map<GrupoProdutoViewModel>(await _produtoService.ObterPorIdGrupo(id));
            if (model == null)
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
            model.Empresas = _mapper.Map<List<EmpresaViewModel>>(_empresaService.ObterTodas().Result);
            return View(model);
        }

        [Route("grupo/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteGrupo(GrupoProdutoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!_produtoService.ApagarProdutoGrupo(model.Id).Result)
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));

                NotificarErro(msgErro);
                return View(model);
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "ApagarGrupo", "Web", $"id:{model.Id}"));
                return View(model);
            }
            await _produtoService.Salvar();

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexGrupo");
        }
        #endregion

        #region SubGrupo
        [Route("subgrupo/lista")]
        public async Task<IActionResult> IndexSubGrupo([FromQuery] long idGrupo, [FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var lista = (await ObterSubGrupo(idGrupo, q, page, ps)); ;
            var modelGrupo = _mapper.Map<GrupoProdutoViewModel>(await _produtoService.ObterPorIdGrupo(idGrupo));
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
            var modelGrupo = _mapper.Map<GrupoProdutoViewModel>(await _produtoService.ObterPorIdGrupo(idGrupo));

            var model = new SubGrupoViewModel();
            model.Situacao = EAtivo.Ativo;
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

            var objeto = _mapper.Map<SubGrupoProduto>(model);

            if (objeto.Id == 0) objeto.Id = objeto.GerarId();
            await _produtoService.Adicionar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AdicionarGrupo", "Web", Deserializar(objeto)));
                return View("CreateEditSubGrupo", model);
            }

            await _produtoService.Salvar();
                       
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";
            LogInformacao($"sucesso:{Deserializar(objeto)}", "Produto", "AdicionarGrupo", null);

            return RedirectToAction("IndexSubGrupo", new { idGrupo = model.IDGRUPO });
        }

        [Route("subgrupo/editar")]
        public async Task<IActionResult> EditSubGrupo(long id)
        {
            if (id ==0)
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AtualizarSubGrupo", "Web", id.ToString())); 
                AdicionarErroValidacao(msgErro);
                return RedirectToAction("IndexGrupo");
            }
            ViewBag.operacao = "E";
            ViewBag.acao = "EditSubGrupo";

            var model = _mapper.Map<SubGrupoViewModel>(await _produtoService.ObterPorIdSubGrupo(id));
            if(model == null)
            {
                var msgErro = $"{_nomeEntidadeSubGrupo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeSubGrupo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexSubGrupo");
            }    

            var modelGrupo = _mapper.Map<GrupoProdutoViewModel>(await _produtoService.ObterPorIdGrupo(model.IDGRUPO.Value));

            model.NomeGrupo = modelGrupo != null ? modelGrupo.Nome : "";
          

            return View("CreateEditSubGrupo", model);
        }

        [Route("subgrupo/editar")]
        [HttpPost]
        public async Task<IActionResult> EditSubGrupo(SubGrupoViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditSubGrupo";

            if (!ModelState.IsValid) return View("CreateEditSubGrupo", model);

            var objeto = _mapper.Map<SubGrupoProduto>(model);

            await _produtoService.Atualizar(objeto);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "AtualizarGrupo", "Web", Deserializar(objeto)));
                return View("CreateEditSubGrupo", model);
            }

            await _produtoService.Salvar();
            LogInformacao($"sucesso:{Deserializar(objeto)}", "Produto", "AtualizarGrupo", null);
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexSubGrupo", new { idGrupo = model.IDGRUPO });
        }

        [Route("subgrupo/apagar")]
        public async Task<IActionResult> DeleteSubGrupo(long id)
        {
            if (id == 0)
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "ApagarSubGrupo", "Web", id.ToString()));
                AdicionarErroValidacao(msgErro);
                return RedirectToAction("IndexGrupo");
            }

            var model = _mapper.Map<SubGrupoViewModel>(await _produtoService.ObterPorIdSubGrupo(id));
            var modelGrupo = _mapper.Map<GrupoProdutoViewModel>(await _produtoService.ObterPorIdGrupo(model.IDGRUPO.Value));
            model.NomeGrupo = modelGrupo != null ? modelGrupo.Nome : "";
            if (model == null)
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

            return View(model);
        }

        [Route("subgrupo/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteSubGrupo(SubGrupoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!_produtoService.ApagarProdutoSubGrupo(model.Id).Result)
            {
                var msgErro = string.Join("\n\r", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));

                NotificarErro(msgErro);
                return View(model);
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Produto", "ApagarGrupo", "Web", $"id:{model.Id}"));
                return View(model);
            }
            await _produtoService.Salvar();
            LogInformacao($"sucesso:id:{model.Id}", "Produto", "ApagarGrupo", null);
            
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexSubGrupo", new { idGrupo = model.IDGRUPO });
        }

        #endregion

        #region metodos privados
        public async Task<ListasAuxiliaresProdutoViewModel> ObterListasAuxiliares()
        {
            var produtos = new ListasAuxiliaresProdutoViewModel();
            produtos.Grupos = _mapper.Map<List<GrupoProdutoViewModel>>(_produtoService.ObterTodosGrupos().Result.ToList());
            produtos.SubGrupos = _mapper.Map<List<SubGrupoViewModel>>(_produtoService.ObterTodosSubGrupos().Result.ToList());
            produtos.Marcas = _mapper.Map<List<ProdutoMarcaViewModel>>(_produtoService.ObterTodosMarca().Result.ToList());
            produtos.Departamentos = _mapper.Map<List<ProdutoDepartamentoViewModel>>(_produtoService.ObterTodosDepartamento().Result.ToList());
            return produtos ;
        }

        private async Task<PagedViewModel<GrupoProdutoViewModel>> ObterGrupo(long idempresa, string filtro, int page, int pageSize)
        {
            var retorno = await _produtoService.ObterGrupoPaginacaoPorDescricao(idempresa, filtro, page, pageSize);
            var listaTeste = retorno.List;
            var lista = _mapper.Map<IEnumerable<GrupoProdutoViewModel>>(listaTeste);

            return new PagedViewModel<GrupoProdutoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "Index",
                TotalResults = retorno.TotalResults
            };
        }

        private async Task<PagedViewModel<SubGrupoViewModel>> ObterSubGrupo(long idGrupo, string filtro, int page, int pageSize)
        {
            var retorno = await _produtoService.ObterSubGrupoPaginacaoPorDescricao(idGrupo, filtro, page, pageSize);
            var listaTeste = retorno.List;
            var lista = _mapper.Map<IEnumerable<SubGrupoViewModel>>(listaTeste);

            return new PagedViewModel<SubGrupoViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "IndexGrupo",
                TotalResults = retorno.TotalResults
            };
        }
        #endregion

    }
}

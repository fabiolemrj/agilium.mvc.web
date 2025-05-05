using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels.Cliente;
using agilium.webapp.manager.mvc.ViewModels.DevolucaoViewModel;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using agilium.webapp.manager.mvc.ViewModels.VendaViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    [Route("devolucao")]
    public class DevolucaoController : MainController
    {
        private readonly IDevolucaoService _devolucaoService;
        private readonly IEmpresaService _empresaService; 
        private readonly IVendaService _vendaService;
        private readonly IClienteService _clienteService;

        private readonly string _nomeEntidadeMotivo = "Motivos de Devolução";
        private IEnumerable<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();
        private IEnumerable<VendaViewModel> listaVendasViewModel { get; set; }=new List<VendaViewModel>();
        private List<ClienteViewModel> listaClienteViewModel { get; set; } = new List<ClienteViewModel>();

        public DevolucaoController(IDevolucaoService devolucaoService, IEmpresaService empresaService, IVendaService vendaService, IClienteService clienteService)
        {
            _devolucaoService = devolucaoService;
            _empresaService = empresaService;
            _clienteService = clienteService;
            _vendaService = vendaService;
            if (listaEmpresaViewModels.Count() == 0)
                listaEmpresaViewModels = _empresaService.ObterTodas().Result;
            
            if (listaClienteViewModel.Count == 0)
                listaClienteViewModel = _clienteService.ObterTodas().Result.ToList();

        }

        #region endpoints

        #region Devolucao
        [Route("lista")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string? DataFinal = null, [FromQuery] string? DataInicial = null)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar Devolução";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "Devolução";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Devolução";
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
            var lista = (await _devolucaoService.ObterPaginacaoPorData(_idEmpresaSelec,_dtini.ToString(), _dtFim.ToString(), page, ps));

            ViewBag.DataInicial = _dtini;
            ViewBag.DataFinal = _dtFim;

            lista.ReferenceAction = "Index";

            return View( "Index",lista);
        }

        [Route("itens")]
        public async Task<ActionResult> ObterItemDevolucao(long id)
        {
            
            var model = await _devolucaoService.ObterDevolucaoItemPorId(id);
            
            if(model != null && model.Count >0)
                ViewBag.devolucao = model.FirstOrDefault().DevolucaoNome;

            return PartialView("_indexItemDevolucao", model);
        }


        [Route("ObterItemVendaPorId")]
        public async Task<ActionResult> ObterItemVendaPorId(string idvenda, string iddev)
        {
            long _id = Convert.ToInt64(idvenda);
            long _iddev = Convert.ToInt64(iddev);
            var model = await _devolucaoService.ObterDevolucaoItemVendaPorId(_iddev, _id);

          //  var model = await _vendaService.ObterItensPorVenda(_id);

            return new JsonResult(new { model });
        }

        [Route("ObterVendaPorData")]
        public async Task<ActionResult> ObterVendaPorData(string data)
        {

            var model = await _vendaService.ObterVendaPorData(data, data);

            var viewDevolucao = new DevolucaoEditarViewModel();
            model.ToList().ForEach(venda => {
                var valor = venda.ValorTotal.HasValue ? venda.ValorTotal.Value : 0;
                viewDevolucao.VendasItens.Add(new DevolucaoItemEditarViewModel()
                {
                    idVenda = venda.Id,
                    VendaNome = $@"Caixa: {venda.CaixaNome} - Venda: {venda.Sequencial} - Total: {valor.ToString("N")}"
                });
            });

            return new JsonResult(new { viewDevolucao });
        }

        [Route("novo")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar devolução";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = "devolução";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "devolução";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";

            var model = new DevolucaoViewModel();
            model.Situacao = Enums.ESituacaoDevolucao.Aberta;
            model.DataHora = DateTime.Now;
            model.IDEMPRESA = _idEmpresaSelec;
            model.DataConsulta = new DateTime(DateTime.Now.Year, 7, 27);

            model.Id = 0;
            PopularListaAuxiliares(model);
            return View("CreateEdit", model);
        }

        private void PopularListaAuxiliares(DevolucaoViewModel model)
        {
            if (model.Clientes.Count() == 0)
                model.Clientes = listaClienteViewModel;
            if (model.Empresas.Count() == 0)
                model.Empresas = listaEmpresaViewModels.ToList();
            if (model.MotivosDevolucao.Count() == 0)
                model.MotivosDevolucao = _devolucaoService.ObterTodosMotivos().Result;

        }

        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> Create(DevolucaoViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "Create";
            PopularListaAuxiliares(model);
            if (!ModelState.IsValid) return View("CreateEdit", model);

            PopularDadosItemDevolucao(model);
            
            model.DataHora = DateTime.Now;

            var resposta = await _devolucaoService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova devolução" };

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

            var model = await _devolucaoService.ObterDevolucaoPorId(id);
        
            PopularListaAuxiliares(model);
            if (model == null)
            {
                var msgErro = $"Devolução/perda não localizada";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "perda/sobra";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            model.DataConsulta = !string.IsNullOrEmpty(model.VendaData) ? Convert.ToDateTime(model.VendaData) : model.DataConsulta;
            model.DevolucaoItens.ForEach(item => {
                if (item.idDevolucao == 0)
                    item.idDevolucao = model.Id;
            });


            return View("CreateEdit", model);
        }

        [Route("editar")]
        [HttpPost]
        public async Task<IActionResult> Edit(DevolucaoViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";
            PopularListaAuxiliares(model);

            if (!ModelState.IsValid) return View("CreateEdit", model);

            PopularDadosItemDevolucao(model);

            model.DataHora = DateTime.Now;

            var resposta = await _devolucaoService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar devolução/perda" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEdit", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        private static void PopularDadosItemDevolucao(DevolucaoViewModel model)
        {
            model.DevolucaoItens.ForEach(item =>
            {
                if (item.selecionado)
                {
                    item.QuantidadeDevolucao = item.QuantidadeVendida;
                    item.ValorDevolucao = item.ValorVendido;
                }
            });
        }

        [Route("cancelar")]
        [HttpPost]
        public async Task<IActionResult> Cancel(DevolucaoViewModel viewModel)
        {
            var resposta = _devolucaoService.CancelarDevolucao(viewModel.Id).Result;
          
            if (ResponsePossuiErros(resposta))
            {
                PopularListaAuxiliares(viewModel);
                var retornoErro = new { mensagem = $"Erro ao tentar apagar Vale Presente" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(viewModel);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("cancelar")]
        public async Task<IActionResult> Cancel(long id)
        {
            var model = await _devolucaoService.ObterDevolucaoPorId(id);

            PopularListaAuxiliares(model);
            if (model == null)
            {
                var msgErro = $"Devolução não localizada";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Devolução";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }

            return View("Cancel", model);
        }


        [Route("gerar-vale/{id}")]
        [HttpGet]
        public async Task<IActionResult> GerarVale(long id)
        {
            var resposta = _devolucaoService.GerarVale(id).Result;
            if (!ResponsePossuiErros(resposta))
            {
                TempData["Mensagem"] = "Operação realizada com sucesso";
                TempData["TipoMensagem"] = "success";
            }

            return RedirectToAction("Index");
        }


        [Route("realizar/{id}")]
        [HttpGet]
        public async Task<IActionResult> Realizar(long id)
        {
            var resposta = _devolucaoService.Realizar(id).Result;
            if (!ResponsePossuiErros(resposta))
            {
                TempData["Mensagem"] = "Operação realizada com sucesso";
                TempData["TipoMensagem"] = "success";
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region MotivoDevolucao
        [Route("motivo/lista")]
        public async Task<IActionResult> IndexMotivos([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
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

            var lista = (await _devolucaoService.ObterMotivoPaginacaoPorDescricao(_idEmpresaSelec, q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return View(lista);
        }

        [Route("motivo/novo")]
        public async Task<IActionResult> CreateMotivo()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateMotivo";

            var model = new MotivoDevolucaoViewModel();
            model.situacao = Enums.EAtivo.Ativo;
            model.Empresas = listaEmpresaViewModels.ToList();
            return View("CreateEditMotivo", model);
        }

        [Route("motivo/novo")]
        [HttpPost]
        public async Task<IActionResult> CreateMotivo(MotivoDevolucaoViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateMotivo";
            if (!ModelState.IsValid) return View("CreateEditMotivo", model);

            var resposta = await _devolucaoService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar nova {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditMotivo", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMotivos");
        }

        [Route("motivo/editar")]
        public async Task<IActionResult> EditMotivo(long id)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditMotivo";
            var objeto = await _devolucaoService.ObterMotivoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeMotivo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeMotivo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexMotivos");
            }

            return View("CreateEditMotivo", objeto);
        }

        [Route("motivo/editar")]
        [HttpPost]
        public async Task<IActionResult> EditMotivo(MotivoDevolucaoViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "EditMotivo";

            if (!ModelState.IsValid) return View("CreateEditMotivo", model);

            var resposta = await _devolucaoService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditMotivo", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMotivos");
        }

        [Route("motivo/apagar")]
        public async Task<IActionResult> DeleteMotivo(long id)
        {
            var objeto = await _devolucaoService.ObterMotivoPorId(id);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidadeMotivo} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidadeMotivo;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("IndexMotivos");
            }

            return View(objeto);
        }

        [Route("motivo/apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteMotivo(MotivoDevolucaoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var resposta = await _devolucaoService.RemoverMotivo(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar nova {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("IndexMotivos");
        }
        #endregion

        #endregion
    }
}

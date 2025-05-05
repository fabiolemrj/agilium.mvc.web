using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.Services;
using agilium.webapp.manager.mvc.ViewModels.Funcionario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    [Route("funcionario")]
    public class FuncionarioController : MainController
    {
        private readonly IFuncionarioService _funcionarioService;
        private readonly IEmpresaService _empresaService;
        private readonly string _nomeEntidadeMotivo = "Funcionario";

        public FuncionarioController(IFuncionarioService funcionarioService, IEmpresaService empresaService)
        {
            _funcionarioService = funcionarioService;
            _empresaService = empresaService;
        }

        #region endpoints
        [Route("lista")]
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
            
            var lista = (await _funcionarioService.ObterPorNome(_idEmpresaSelec, q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return View(lista);
        }

        [Route("novo")]
        [HttpGet]
        public async Task<IActionResult> CreateFuncionario()
        {
            ObterEstados();
            var objeto = await _funcionarioService.ObterListasAuxiliares();
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateFuncionario";
          
            var model = new FuncionarioViewModel();
            model.Situacao = Enums.EAtivo.Ativo;
            model.Endereco.Id = 0;
            if (objeto != null)
            {
                model.Empresas = objeto.Empresas;
                model.Usuarios = objeto.Usuarios;
            }

            return View("CreateEditFuncionario", model);
        }

        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> CreateFuncionario(FuncionarioViewModel model)
        {
            ObterEstados();
            ViewBag.operacao = "I";
            ViewBag.acao = "CreateFuncionario";
            if (!ModelState.IsValid) return View("CreateEditFuncionario", model);

            
            if (model.Endereco != null && !string.IsNullOrEmpty(model.Endereco.Cep))
                model.Endereco.Cep = RetirarPontos(model.Endereco.Cep);
            else
                model.Endereco = null;

            var resposta = await _funcionarioService.Adicionar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditFuncionario", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("editar")]
        [HttpGet]
        public async Task<IActionResult> EditFuncionario(long id)
        {
            ObterEstados();
            ViewBag.operacao = "E";
            ViewBag.acao = "EditFuncionario";
            var objeto = await _funcionarioService.ObterPorId(id);
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

            return View("CreateEditFuncionario", objeto);
        }

        [Route("editar")]
        [HttpPost]
        public async Task<IActionResult> EditFuncionario(FuncionarioViewModel model)
        {
            ObterEstados();
            ViewBag.operacao = "E";
            ViewBag.acao = "EditFuncionario";

            if (!ModelState.IsValid) return View("CreateEditFornecedor", model);

            if (model.Endereco != null && model.Endereco.Id == null)
            {
                model.IDENDERECO = 0;
                model.Endereco.Id = 0;
            }

            if (model.Endereco != null && !string.IsNullOrEmpty(model.Endereco.Cep))
                model.Endereco.Cep = RetirarPontos(model.Endereco.Cep);
            else
                model.Endereco = null;

            var resposta = await _funcionarioService.Atualizar(model.Id, model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditFuncionario", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }

        [Route("apagar")]
        [HttpGet]
        public async Task<IActionResult> DeleteFuncionario(long id)
        {
            ObterEstados();
            var objeto = await _funcionarioService.ObterPorId(id);
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

        [Route("apagar")]
        [HttpPost]
        public async Task<IActionResult> DeleteFuncionario(FuncionarioViewModel model)
        {
            ObterEstados();
            if (!ModelState.IsValid) return View(model);

            var resposta = await _funcionarioService.Remover(model.Id);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao tentar apagar {_nomeEntidadeMotivo}" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("Index");
        }
        #endregion

        #region ViewBag
        private void ObterEstados()
        {
            var estados = ListasAuxilares.ObterEstados();
            ViewBag.estados = new SelectList(estados, "Sigla", "Nome", "");
        }
        #endregion
    }
}

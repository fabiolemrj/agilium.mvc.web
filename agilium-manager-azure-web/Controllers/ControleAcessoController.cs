using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels.ControleAcesso;
using agilium.webapp.manager.mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using agilium.webapp.manager.mvc.ViewModels.UnidadeViewModel;
using agilium.webapp.manager.mvc.ViewModels.CaManagerViewModel;
using System.Linq;
using System.Xml.Linq;
using System;


namespace agilium.webapp.manager.mvc.Controllers
{
    [Route("ca")]
    public class ControleAcessoController : MainController
    {
        private readonly IControleAcessoService _controleAcessoService;
        private readonly string _nomeEntidade = "Controle Acesso";

        public ControleAcessoController(IControleAcessoService controleAcessoService)
        {
            _controleAcessoService = controleAcessoService;
        }



        #region endpoints
        public async Task<IActionResult> PerfilIndex([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {
            var _idEmpresaSelec = ObterIdEmpresaSelecionada();

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar os perfis";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidade;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;

                return RedirectToAction("Index", "Home");
            }

            var lista = (await _controleAcessoService.ObterPerfilPorDescricao(_idEmpresaSelec,q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return View(lista);
        }

        public async Task<IActionResult> EditPerfil(long id)
        {

            var perfilIndex = await _controleAcessoService.ObterPerfilPorId(id);

            if (perfilIndex == null)
            {
                var msgErro = $"{_nomeEntidade} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("PerfilIndex");
            }

            var objeto = new CreateEditPerfilViewModel() {
                Id = perfilIndex.Id,
                Descricao = perfilIndex.Descricao,
                Situacao = perfilIndex.Situacao,
                idEmpresa = perfilIndex.idEmpresa
            };
            
            return View(objeto);
        }

        [HttpPost]
        public async Task<IActionResult> EditPerfil(CreateEditPerfilViewModel model)
        {

            if (!ModelState.IsValid) return View(model);

            var resposta = await _controleAcessoService.AtualizarPerfil(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar nova {_nomeEntidade}" };
                AdicionarErroValidacao(retornoErro.mensagem);
                return View( model);
            }
            return RedirectToAction("PerfilIndex");
        }

        public async Task<IActionResult> EditModelo(long id)
        {

            var modelo = await _controleAcessoService.ObterModelosPorPerfil(id);

            if (modelo == null)
            {
                var msgErro = $"Modelo do perfil não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("PerfilIndex");
            }

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> EditModelo(CreateModeloViewModel model)
        {

            if (!ModelState.IsValid) return View(model);

            var resposta = await _controleAcessoService.AdicionarModeloItem(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar nova modelo" };
                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            return RedirectToAction("PerfilIndex");
        }

        #endregion

        #region Ca Manager
        [Route("perfil-manager")]
        public async Task<IActionResult> PerfilManagerIndex([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null)
        {            
            var lista = (await _controleAcessoService.ObterPerfilManagerPorDescricao(q, page, ps)); ;

            ViewBag.Pesquisa = q;

            return View(lista);
        }

        [Route("perfil-manager/novo")]
        [HttpGet]
        public async Task<IActionResult> CreatePerfil()
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "CreatePerfil";
            var model = new CaPerfilManagerViewModel();
            
            return View("CreateEditPerfil", model);
        }

        [Route("perfil-manager/novo")]
        [HttpPost]
        public async Task<IActionResult> CreatePerfil(CaPerfilManagerViewModel model)
        {

            ViewBag.operacao = "I";
            ViewBag.acao = "CreatePerfil";
            if (!ModelState.IsValid) return View("CreateEditPerfil", model);

            var resposta = await _controleAcessoService.AdicionarPerfil(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo Perfil" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditPerfil", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("PerfilManagerIndex");
        }

        [Route("perfil-manager/editar")]
        [HttpGet]
        public async Task<IActionResult> EditPerfil(int id)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditPerfil";
            var objeto = await _controleAcessoService.ObterPerfilManagerPorId(id);
            if (objeto == null)
            {
                var msgErro = $"Perfil não localizado";

                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Perfil";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("PerfilManagerIndex");
            }

            return View("CreateEditPerfil", objeto);
        }

        [Route("perfil-manager/editar")]
        [HttpPost]
        public async Task<IActionResult> EditPerfil(CaPerfilManagerViewModel model)
        {

            ViewBag.operacao = "E";
            ViewBag.acao = "EditPerfil";

            if (!ModelState.IsValid) return View("CreateEditPerfil", model);

            var resposta = await _controleAcessoService.AtualizarPerfil(model);

            if (ResponsePossuiErros(resposta))
            {

                var retornoErro = new { mensagem = $"Erro ao editar Perfil" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return View("CreateEditPerfil", model);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            return RedirectToAction("PerfilManagerIndex");
        }

        
        [Route("perfil-manager/permissoes")]
        public async Task<IActionResult> ListaPermissoesPorPerfil(int id)
        {

            var objeto = await _controleAcessoService.ObterPermissaoPorPerfil(id);

            if (objeto == null)
            {
                var msgErro = $"Inventario não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Mensagem"] = msgErro;
                TempData["TipoMensagem"] = "danger";

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = "Inventario";
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index");
            }
            var model = objeto;

            var listaCorrigida = PegarNoFilho(objeto.Area,1, objeto.Area);
            model.Area = listaCorrigida;

            return View("ListaPermissoesPorPerfil", model);
        }

        [Route("perfil-manager/permissoes")]
        [HttpPost]
        public async Task<ActionResult> ListaPermissoesPorPerfil(CaPermissaoPerfilViewModel model)
        {
            if (!ModelState.IsValid) return View(model);


            return RedirectToAction("PerfilManagerIndex");
        }



        [Route("geter")]
        public async Task<ActionResult> Geter(string id)
        {
            if (string.IsNullOrEmpty(id)) id = "8";
            var objeto = _controleAcessoService.ObterPermissaoPorPerfil(Convert.ToInt32(id)).Result;

            if (objeto == null)
            {
                return null;
            }

            var listaCorrigida = PegarNoFilho(objeto.Area, 1, objeto.Area);
       
           
            return Json(listaCorrigida);
        }


        [HttpPost]
        [Route("SaveCheckedNodes")]
        public async Task<ActionResult> SaveCheckedNodes(List<int> checkedIds, int idperfil)
        {
            if (checkedIds == null)
            {
                checkedIds = new List<int>();
            }
            var areasSelecionadas = new List<CaAreaManagerSalvarViewModel>();

            checkedIds.ForEach(x => {
                areasSelecionadas.Add(new CaAreaManagerSalvarViewModel()
                {
                    IdArea = x,
                    IdPerfil = idperfil
                });
            });
            
            var resposta = await _controleAcessoService.AdicionarPermissoes(areasSelecionadas);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao criar novo Perfil" };

                AdicionarErroValidacao(retornoErro.mensagem);
                return this.Json(true);
            }
            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";

            var url = Url.Action("perfil-manager", "ca");

            return Json(new { success = true, url });
        }
        #endregion

        #region Teste Location
        [Route("get")]
        public async Task<ActionResult> Get(string query)
        {
            List<Location> locations;
            List<LocationBd> records;
                locations = Popular();

                if (!string.IsNullOrWhiteSpace(query))
                {
                    locations = locations.Where(q => q.Name.Contains(query)).ToList();
                }

                records = locations.Where(l => l.ParentID == null).OrderBy(l => l.OrderNumber)
                    .Select(l => new LocationBd
                    {
                        id = l.ID,
                        text = l.Name,
                        @checked = l.Checked,
                        population = l.Population,
                        flagUrl = l.FlagUrl,
                        children = GetChildren(locations, l.ID),
                    }).ToList();
            return Json(records);
        }

        private List<LocationBd> GetChildren(List<Location> locations, int parentId)
        {
            return locations.Where(l => l.ParentID == parentId).OrderBy(l => l.OrderNumber)
                .Select(l => new LocationBd
                {
                    id = l.ID,
                    text = l.Name,
                    population = l.Population,
                    flagUrl = l.FlagUrl,
                    @checked = l.Checked,
                    children = GetChildren(locations, l.ID)
                }).ToList();
        }

        private List<Location> Popular()
        {

            var linha1 = new Location(1,null,"Asia",false,1,null,null,null,null);
            var linha2 = new Location(2, 1, "China", false, 1, 543232, null, null, null);
            var linha3 = new Location(3, 1, "Japan", false, 2, 324332, null, null, null);
            var linha4 = new Location(4, 1, "Mongolia", false, 3, 122, null, null, null);

            var resultado = new List<Location>();

            resultado.Add(linha1);
            resultado.Add(linha2);
            resultado.Add(linha3);
            resultado.Add(linha4);

            return resultado;
        }

        #endregion

        #region private

        //private async Task<List<CaAreaManagerViewModel>> PegarNoPrincipal(List<CaAreaManagerViewModel> listaOrigem)
        //{
        //    var resultado = listaOrigem.Where(x => x.hierarquia == 1).OrderBy(x => x.ordem)
        //        .Select(x => new CaAreaManagerViewModel
        //        {
        //            IdArea = x.IdArea,
        //            idtag = x.idtag,
        //            ordem = x.ordem,
        //            hierarquia = x.hierarquia,
        //            Selecao = x.Selecao,
        //            subitem = x.subitem,
        //            titulo = x.titulo,
        //            children = PegarNoFilho(listaOrigem,x.subitem)
        //        }).ToList();

        //    return resultado;
        //}

        private List<CaAreaManagerViewModel> PegarNoFilho(List<CaAreaManagerViewModel> listaOrigem,int parentId, List<CaAreaManagerViewModel> listaOriginal)
        {
            return listaOrigem.Where(x => x.hierarquia == parentId).OrderBy(x => x.ordem)
                .Select(x =>new CaAreaManagerViewModel
                {
                    IdArea = x.IdArea,
                    id = x.IdArea,
                    idtag = x.idtag,
                    ordem = x.ordem,
                    hierarquia = x.hierarquia,
                    Selecao = x.Selecao,
                    subitem = x.subitem,
                    titulo = x.titulo,
                    text = x.titulo,
                    @checked = listaOriginal.Any(w=>w.IdArea == x.IdArea && w.Selecao && w.subitem == 0),
                    children = PegarNoFilho(listaOrigem, x.subitem,listaOriginal)
                }).ToList();

        }

        #endregion

    }
}

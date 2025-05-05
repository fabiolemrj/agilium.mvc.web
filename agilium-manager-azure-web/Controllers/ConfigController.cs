using agilium.webapp.manager.mvc.Enums;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    public class ConfigController : MainController
    {
        private readonly IConfigServices _configService;
        private readonly string _nomeEntidade = "Configuração";

        public ConfigController(IConfigServices configService)
        {
            _configService = configService;
        }

        #region endpoint
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
        {

            var _idEmpresaSelec = Convert.ToInt64(ObterIdEmpresaSelecionada());
            
            if (_idEmpresaSelec <= 0)
                {
                    var msgErro = $"Selecione uma empresa para acessar as configurações";

                    TempData["TipoMensagem"] = "danger";
                    TempData["Titulo"] = _nomeEntidade;
                    TempData["Mensagem"] = msgErro;
                    
                    ViewBag.TipoMensagem = "danger";
                    ViewBag.Titulo = _nomeEntidade;
                    ViewBag.Mensagem = msgErro;

                    return RedirectToAction("Index", "Home");
                }            
            
            var lista = await _configService.ObterConfigurcoesPorEmpresaEChave(_idEmpresaSelec, q, page, ps);
            ViewBag.Pesquisa = q;
            ViewBag.idEmpresa = _idEmpresaSelec;
            lista.ReferenceAction = "Index";

            var listaEditarChaveConfig = new List<EditarChaveValorViewModel>();
            lista.List.ToList().ForEach( config => {
                var configuraçãConvertida = ConverterCamposConfigViewModel(config, _idEmpresaSelec).Result;
                
                if(configuraçãConvertida.Classificacao != EClassificacaoConfiguracao.NaoExibir) listaEditarChaveConfig.Add(configuraçãConvertida);
            });
            var listaConvertida = new PagedViewModel<EditarChaveValorViewModel>() { 
                PageIndex = lista.PageIndex,
                PageSize = lista.PageSize,
                Query = lista.Query,
                ReferenceAction = lista.ReferenceAction,
                TotalResults = lista.TotalResults,
                List = listaEditarChaveConfig
            };
            return View(listaConvertida);
        }

        public async Task<ActionResult> IndexConfigImagem()
        {
            var _idEmpresaSelec = Convert.ToInt64(ObterIdEmpresaSelecionada());

            if (_idEmpresaSelec <= 0)
            {
                var msgErro = $"Selecione uma empresa para acessar as configurações";

                TempData["TipoMensagem"] = "danger";
                TempData["Titulo"] = _nomeEntidade;
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;

                return RedirectToAction("Index", "Home");
            }

            var lista = new List<ConfigImagemViewModel>();

            _configService.ObterCongiImagemPorEmpresa(_idEmpresaSelec).Result.ToList().ForEach(item => {
               
                lista.Add(new ConfigImagemViewModel()
                {
                    Descricao = ObterDescricao(item.CHAVE),
                    CHAVE = item.CHAVE,
                    IDEMPRESA = item.IDEMPRESA,
                    IMG = item.IMG,
                    ImagemConvertida = item.ImagemConvertida
                });
            });

            return View("ConfigImagem",lista);
        }

        public async Task<ActionResult> EditConfigImage(string chave, long idEmpresa)
        {
            var objeto = await _configService.ObterConfigImagemPorId(idEmpresa,chave);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidade} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Home");
            }

            objeto.Descricao = ObterDescricao(chave);

            return View(objeto);
        }

        [HttpPost]
        public async Task<ActionResult> EditConfigImage(ConfigImagemViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            model.ImagemConvertida = model.ImagemConvertida.Replace("data:image/png;base64,", "")
                                                                                    .Replace("data:image/jpeg;base64,", "")
                                                                                    .Replace("data:image/jpg;base64,", "")
                                                                                    .Replace("data:image/bmp;base64,", "");
            //model.ImagemUpLoad = ConverterToIFormFile(ConverterToIbyteArray(model.ImagemConvertida),model.Descricao,"png");
            var resposta = await _configService.Atualizar(model);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar configuração" };
                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            ViewBag.TipoMensagem = "success";
            ViewBag.Titulo = _nomeEntidade;
            ViewBag.Mensagem = "Operação realizada com sucesso";
            return View("IndexConfigImagem");
        }

        public async Task<IActionResult> Edit(long idEmpresa)
        {            
            var objeto = await _configService.ObterTodosPorEmpresa(idEmpresa);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidade} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index","Home");
            }
            var objetoConvertido = await ConverterCamposConfigViewModel(objeto,idEmpresa);

            return View(objetoConvertido);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ConfigCamposViewModel model)
        {
         
            if (!ModelState.IsValid) return View(model);

            
            var resposta = await _configService.Atualizar(model.IdEmpresa, await ConverterChaveValor(model));

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar configuração" };
                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            ViewBag.TipoMensagem = "success";
            ViewBag.Titulo = _nomeEntidade;
            ViewBag.Mensagem = "Operação realizada com sucesso";
            return View(model);
        }

        public async Task<IActionResult> EditItem(string chave, long idEmpresa)
        {
            var objeto = await _configService.ObterPorChave(chave, idEmpresa);
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidade} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Config",new {idEmpresa = idEmpresa });
            }
            var objetoConvertido = await ConverterCamposConfigViewModel(objeto, idEmpresa);

            return View(objetoConvertido);
        }

        [HttpPost]
        public async Task<IActionResult> EditItem(EditarChaveValorViewModel model)
        {

            if (!ModelState.IsValid) return View(model);

            var resposta = await _configService.Atualizar(model.IdEmpresa, await ConverterChaveValor(model));

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao editar configuração" };
                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            ViewBag.TipoMensagem = "success";
            ViewBag.Titulo = _nomeEntidade;
            ViewBag.Mensagem = "Operação realizada com sucesso";
            return RedirectToAction("Index", "Config", new { idEmpresa = model.IdEmpresa });
        }

        public async Task<IActionResult> EditCertificado(long idEmpresa)
        {
            var chaveCertificado = "CERTIFICADO_CAMINHO";
            var objeto = await _configService.ObterPorChave(chaveCertificado, idEmpresa);
            if (objeto == null)
            {
                var msgErro = $"{chaveCertificado} não foi localizada nas configurações";
                AdicionarErroValidacao(msgErro);
                TempData["TipoMensagem"] = "danger";
                TempData["Mensagem"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Config", new { idEmpresa = idEmpresa });
            }
            var objetoConvertido = new ChaveValorViewModel() { 
                CHAVE = objeto.CHAVE,
                IDEMPRESA = idEmpresa,
                VALOR = objeto.VALOR
            };

            return View(objetoConvertido);
        }

        [HttpPost]
        public async Task<IActionResult> EditCertificado(ChaveValorViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var conversao = new ConfigIndexViewModel()
            {
                CHAVE = model.CHAVE,
                IDEMPRESA = model.IDEMPRESA,
                Arquivo = model.Arquivo,
                VALOR = model.VALOR
            };
            var resposta = await _configService.AtualizarCertificado(model.IDEMPRESA, conversao);
            //var resposta = await _configService.AtualizarCertificado(model.IDEMPRESA,model.Arquivo);

            if (ResponsePossuiErros(resposta))
            {
                var retornoErro = new { mensagem = $"Erro ao tentar atualizar certificado" };
                AdicionarErroValidacao(retornoErro.mensagem);
                TempData["TipoMensagem"] = "danger";
                TempData["Mensagem"] = retornoErro;

                return View(model);
            }
            TempData["TipoMensagem"] = "success";
            TempData["Mensagem"] = "Certificado atualizado com sucesso";

            return RedirectToAction("Index", "Config", new { idEmpresa = model.IDEMPRESA });
        }

        #endregion

        #region Private ConverterViewModel

        private async Task<ChaveValorViewModel> ConverterChaveValor(EditarChaveValorViewModel model)
        {
            return new ChaveValorViewModel() { 
                CHAVE = model.Chave,
                IDEMPRESA = model.IdEmpresa,
                VALOR = model.Valor
            };
        }

        private async Task<List<ChaveValorViewModel>> ConverterChaveValor(ConfigCamposViewModel model)
        {
            var listaFinal = new List<ChaveValorViewModel>();

            listaFinal.Add(model.CAIXA_DSSUPRIMENTO);
            listaFinal.Add(model.CAIXA_VLSUPRIMENTO);
            listaFinal.Add(model.CAIXA_VLSUPRIMENTO);
            listaFinal.Add(model.CERTIFICADO_CAMINHO);
            listaFinal.Add(model.CERTIFICADO_SENHA);
            listaFinal.Add(model.CONTA_IDCONTACLIENTE);
            listaFinal.Add(model.CONTA_IDCONTAESTOQUE);
            listaFinal.Add(model.CONTA_IDCONTAFORNECEDOR);
            listaFinal.Add(model.CONTA_NMCONTACLIENTE);
            listaFinal.Add(model.CONTA_NMCONTAESTOQUE);
            listaFinal.Add(model.CONTA_NMCONTAFORNECEDOR);
            listaFinal.Add(model.CONTA_REALIZARCONTROLE);
            listaFinal.Add(model.COR_FINAL);
            listaFinal.Add(model.COR_FONTE_TIT_EXCEL);
            listaFinal.Add(model.COR_FONTE_ZEBRADA);
            listaFinal.Add(model.COR_FONTE_ZEBRADA_EXCEL);
            listaFinal.Add(model.COR_FUNDO_TIT_EXCEL);
            listaFinal.Add(model.COR_INICIAL);
            listaFinal.Add(model.COR_ZEBRADA);
            listaFinal.Add(model.ESTOQUE_PERMITENEGATIVO);
            listaFinal.Add(model.MAIL_AUTENTICA);
            listaFinal.Add(model.MAIL_EMAIL);
            listaFinal.Add(model.MAIL_POP);
            listaFinal.Add(model.MAIL_PORTA_POP);
            listaFinal.Add(model.MAIL_PORTA_SMTP);
            listaFinal.Add(model.MAIL_REMETENTE);
            listaFinal.Add(model.MAIL_SENHA);
            listaFinal.Add(model.MAIL_SMTP);
            listaFinal.Add(model.MAIL_USUARIO);
            listaFinal.Add(model.NFCE_AMBIENTE);
            listaFinal.Add(model.NFCE_MODELO);
            listaFinal.Add(model.NFCE_MODELO_HOMOL);
            listaFinal.Add(model.NFCE_NATOP);
            listaFinal.Add(model.NFCE_NATOP_HOMOL);
            listaFinal.Add(model.NFCE_SERIE);
            listaFinal.Add(model.NFCE_SERIE_HOMOL);
            listaFinal.Add(model.PASTA_FOTOS);
            listaFinal.Add(model.PDV_IMPRESSORA_COZINHA);
            listaFinal.Add(model.PDV_PORTA_IMPRESSORA_COZINHA);
            listaFinal.Add(model.PDV_PREVENDA);
            listaFinal.Add(model.PDV_TAMANHO_FONTE);
            listaFinal.Add(model.PORTA_IMPRESSORA);
            listaFinal.Add(model.TXENTREGA_COBRAR);
            listaFinal.Add(model.TXENTREGA_FORMA);
            listaFinal.Add(model.TXENTREGA_VALOR);
            listaFinal.Add(model.TXENTREGA_VLMINIMO);
            listaFinal.Add(model.VENDAS_DOC_FISCAL_PADRAO);
            listaFinal.Add(model.VENDAS_DOC_FISCAL_PADRAO_STR);
            listaFinal.Add(model.COR_TEMA);

            return listaFinal;
        }

        private async Task<ConfigCamposViewModel> ConverterCamposConfigViewModel(List<ConfigIndexViewModel> model, long idEmpresa)
        {
            var resuldado = new ConfigCamposViewModel();
            PropertyInfo[] properties = resuldado.GetType().GetProperties();
            var conferir = resuldado.GetType().GetProperties();
            resuldado.CAIXA_DSSUPRIMENTO = ConverterChaveValorViewModel(model, "CAIXA_DSSUPRIMENTO",idEmpresa).Result;
            resuldado.CAIXA_VLSUPRIMENTO = ConverterChaveValorViewModel(model, "CAIXA_VLSUPRIMENTO", idEmpresa).Result;
            resuldado.CERTIFICADO_CAMINHO = ConverterChaveValorViewModel(model, "CERTIFICADO_CAMINHO", idEmpresa).Result;
            resuldado.CERTIFICADO_SENHA = ConverterChaveValorViewModel(model, "CERTIFICADO_SENHA", idEmpresa).Result;
            resuldado.CONTA_IDCONTACLIENTE = ConverterChaveValorViewModel(model, "CONTA_IDCONTACLIENTE", idEmpresa).Result;
            resuldado.CONTA_IDCONTAESTOQUE = ConverterChaveValorViewModel(model, "CONTA_IDCONTAESTOQUE", idEmpresa).Result;
            resuldado.CONTA_IDCONTAFORNECEDOR = ConverterChaveValorViewModel(model, "CONTA_IDCONTAFORNECEDOR", idEmpresa).Result;
            resuldado.CONTA_NMCONTACLIENTE = ConverterChaveValorViewModel(model, "CONTA_NMCONTACLIENTE", idEmpresa).Result;
            resuldado.CONTA_NMCONTAESTOQUE = ConverterChaveValorViewModel(model, "CONTA_NMCONTAESTOQUE", idEmpresa).Result;
            resuldado.CONTA_NMCONTAFORNECEDOR = ConverterChaveValorViewModel(model, "CONTA_NMCONTAFORNECEDOR", idEmpresa).Result;
            resuldado.CONTA_REALIZARCONTROLE = ConverterChaveValorViewModel(model, "CONTA_REALIZARCONTROLE", idEmpresa).Result;
            resuldado.COR_FINAL = ConverterChaveValorViewModel(model, "COR_FINAL", idEmpresa).Result;
            resuldado.COR_FONTE_TIT_EXCEL = ConverterChaveValorViewModel(model, "COR_FONTE_TIT_EXCEL", idEmpresa).Result;
            resuldado.COR_FONTE_ZEBRADA = ConverterChaveValorViewModel(model, "COR_FONTE_ZEBRADA", idEmpresa).Result; 
            resuldado.COR_FONTE_ZEBRADA_EXCEL = ConverterChaveValorViewModel(model, "COR_FONTE_ZEBRADA_EXCEL", idEmpresa).Result;
            resuldado.COR_FUNDO_TIT_EXCEL = ConverterChaveValorViewModel(model, "COR_FUNDO_TIT_EXCEL", idEmpresa).Result;
            resuldado.COR_INICIAL = ConverterChaveValorViewModel(model, "COR_INICIAL", idEmpresa).Result;
            resuldado.COR_ZEBRADA = ConverterChaveValorViewModel(model, "COR_ZEBRADA", idEmpresa).Result;
            resuldado.COR_TEMA = ConverterChaveValorViewModel(model, "COR_TEMA", idEmpresa).Result;
            resuldado.ESTOQUE_PERMITENEGATIVO = ConverterChaveValorViewModel(model, "ESTOQUE_PERMITENEGATIVO", idEmpresa).Result;
            resuldado.MAIL_AUTENTICA = ConverterChaveValorViewModel(model, "MAIL_AUTENTICA", idEmpresa).Result;
            resuldado.MAIL_EMAIL = ConverterChaveValorViewModel(model, "MAIL_EMAIL", idEmpresa).Result;
            resuldado.MAIL_POP = ConverterChaveValorViewModel(model, "MAIL_POP", idEmpresa).Result;
            resuldado.MAIL_PORTA_POP = ConverterChaveValorViewModel(model, "MAIL_PORTA_POP", idEmpresa).Result;
            resuldado.MAIL_PORTA_SMTP = ConverterChaveValorViewModel(model, "MAIL_PORTA_SMTP", idEmpresa).Result;
            resuldado.MAIL_REMETENTE = ConverterChaveValorViewModel(model, "MAIL_REMETENTE", idEmpresa).Result;
            resuldado.MAIL_SENHA = ConverterChaveValorViewModel(model, "MAIL_SENHA", idEmpresa).Result;
            resuldado.MAIL_SMTP = ConverterChaveValorViewModel(model, "MAIL_SMTP", idEmpresa).Result; 
            resuldado.MAIL_USUARIO = ConverterChaveValorViewModel(model, "MAIL_USUARIO", idEmpresa).Result;
            resuldado.NFCE_AMBIENTE = ConverterChaveValorViewModel(model, "NFCE_AMBIENTE", idEmpresa).Result;
            resuldado.NFCE_MODELO = ConverterChaveValorViewModel(model, "NFCE_MODELO", idEmpresa).Result;
            resuldado.NFCE_MODELO_HOMOL = ConverterChaveValorViewModel(model, "NFCE_MODELO_HOMOL", idEmpresa).Result;
            resuldado.NFCE_NATOP = ConverterChaveValorViewModel(model, "NFCE_NATOP", idEmpresa).Result;
            resuldado.NFCE_NATOP_HOMOL = ConverterChaveValorViewModel(model, "NFCE_NATOP_HOMOL", idEmpresa).Result;
            resuldado.NFCE_SERIE = ConverterChaveValorViewModel(model, "NFCE_SERIE", idEmpresa).Result;
            resuldado.NFCE_SERIE_HOMOL = ConverterChaveValorViewModel(model, "NFCE_SERIE_HOMOL", idEmpresa).Result;
            resuldado.PASTA_FOTOS = ConverterChaveValorViewModel(model, "PASTA_FOTOS", idEmpresa).Result;
            resuldado.PDV_IMPRESSORA_COZINHA = ConverterChaveValorViewModel(model, "PDV_IMPRESSORA_COZINHA", idEmpresa).Result;
            resuldado.PDV_PORTA_IMPRESSORA_COZINHA = ConverterChaveValorViewModel(model, "PDV_PORTA_IMPRESSORA_COZINHA", idEmpresa).Result;
            resuldado.PDV_PREVENDA = ConverterChaveValorViewModel(model, "PDV_PREVENDA", idEmpresa).Result;
            resuldado.PDV_TAMANHO_FONTE = ConverterChaveValorViewModel(model, "PDV_TAMANHO_FONTE", idEmpresa).Result;
            resuldado.PORTA_IMPRESSORA = ConverterChaveValorViewModel(model, "PORTA_IMPRESSORA", idEmpresa).Result;
            resuldado.TXENTREGA_COBRAR = ConverterChaveValorViewModel(model, "TXENTREGA_COBRAR", idEmpresa).Result;
            resuldado.TXENTREGA_FORMA = ConverterChaveValorViewModel(model, "TXENTREGA_FORMA", idEmpresa).Result;
            resuldado.TXENTREGA_VALOR = ConverterChaveValorViewModel(model, "TXENTREGA_VALOR", idEmpresa).Result;
            resuldado.TXENTREGA_VLMINIMO = ConverterChaveValorViewModel(model, "TXENTREGA_VLMINIMO", idEmpresa).Result;
            resuldado.VENDAS_DOC_FISCAL_PADRAO = ConverterChaveValorViewModel(model, "VENDAS_DOC_FISCAL_PADRAO", idEmpresa).Result;
            resuldado.VENDAS_DOC_FISCAL_PADRAO_STR = ConverterChaveValorViewModel(model, "VENDAS_DOC_FISCAL_PADRAO_STR", idEmpresa).Result;
            
            return resuldado;
        }

        private async Task<ChaveValorViewModel> ConverterChaveValorViewModel(List<ConfigIndexViewModel> model, string chave, long idEmpresa)
        {
            var chaveValor = new ChaveValorViewModel() { 
                CHAVE = chave
            };
            var config = model.FirstOrDefault(x => x.CHAVE.ToUpper() == chave.ToUpper());
            if (config != null)
                chaveValor.VALOR = config.VALOR;

            chaveValor.IDEMPRESA = idEmpresa;

            return chaveValor;
        }

        private async Task<EditarChaveValorViewModel> ConverterEditarChaveValorViewModel(ConfigIndexViewModel model, long idEmpresa, ETipoCompnenteConfig tipo, string label, EClassificacaoConfiguracao classificacao)
        {
            return new EditarChaveValorViewModel() {
                Chave = model.CHAVE,
                Valor = model.VALOR,
                IdEmpresa = idEmpresa,
                Tipo = tipo,
                Label = label,
                Classificacao = classificacao
            };
        }

        private async Task<EditarChaveValorViewModel> ConverterCamposConfigViewModel(ConfigIndexViewModel model, long idEmpresa)
        {
            var configCamposViewModel = new ConfigCamposViewModel();

            if (model.CHAVE.Trim().ToUpper() == "CAIXA_DSSUPRIMENTO")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa,ETipoCompnenteConfig.Texto, "Descrição Padrão Suprimento", EClassificacaoConfiguracao.Caixa).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "CAIXA_VLSUPRIMENTO")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Numero, "Valor Padrão Suprimento (R$)", EClassificacaoConfiguracao.Caixa).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "CERTIFICADO_SENHA")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Senha Certificado", EClassificacaoConfiguracao.NFCe).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "CONTA_IDCONTACLIENTE")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Conta Registro Cliente", EClassificacaoConfiguracao.Contabil).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "CONTA_IDCONTAFORNECEDOR")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Conta Registro Fornecedor", EClassificacaoConfiguracao.Contabil).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "CONTA_REALIZARCONTROLE")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.SimNao, "Realizar controle lançamentos contabeis", EClassificacaoConfiguracao.Contabil).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "COR_FINAL")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Cores, "Cor Final", EClassificacaoConfiguracao.Cores).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "COR_FONTE_TIT_EXCEL")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Cores, "Cor Fonte Titulo Excel", EClassificacaoConfiguracao.Cores).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "COR_FONTE_ZEBRADA")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Cores, "Cor Fonte Zebrada", EClassificacaoConfiguracao.Cores).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "COR_FONTE_ZEBRADA_EXCEL")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Cores, "Cor Fonte Zebrada Excel", EClassificacaoConfiguracao.Cores).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "COR_FUNDO_TIT_EXCEL")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Cores, "Cor de Fundo do Titulo Excel", EClassificacaoConfiguracao.Cores).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "COR_INICIAL")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Cores, "Cor Inicial", EClassificacaoConfiguracao.Cores).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "COR_ZEBRADA")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Cores, "Cor Zebrada", EClassificacaoConfiguracao.Cores).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "COR_TEMA")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Cores, "Cor Tema", EClassificacaoConfiguracao.Cores).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "ESTOQUE_PERMITENEGATIVO")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.SimNao, "Permitir Estoque Negativo", EClassificacaoConfiguracao.Gerais).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "MAIL_AUTENTICA")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.SimNao, "Email - Autenticação", EClassificacaoConfiguracao.Email).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "MAIL_EMAIL")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Email - Usuario", EClassificacaoConfiguracao.Email).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "MAIL_POP")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Email - POP", EClassificacaoConfiguracao.Email).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "MAIL_PORTA_POP")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Email - Porta POP", EClassificacaoConfiguracao.Email).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "MAIL_PORTA_SMTP")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Email - Porta SMTP", EClassificacaoConfiguracao.Email).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "MAIL_REMETENTE")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Email - Remetente", EClassificacaoConfiguracao.Email).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "MAIL_SENHA")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Email - Senha", EClassificacaoConfiguracao.Email).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "MAIL_SMTP")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Email - SMTP", EClassificacaoConfiguracao.Email).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "MAIL_USUARIO")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Email - Usuario", EClassificacaoConfiguracao.Email).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "MAIL_SSL")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.SimNao, "Autenticação SSL- Email", EClassificacaoConfiguracao.Email).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "NFCE_AMBIENTE")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.TipoAmbiente, "NFCe Ambiente", EClassificacaoConfiguracao.NFCe).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "NFCE_MODELO")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "NFCe Modelo", EClassificacaoConfiguracao.NFCe).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "NFCE_MODELO_HOMOL")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "NFCe Modelo - Homologação", EClassificacaoConfiguracao.NFCe).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "NFCE_NATOP")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "NFCe Natureza Operação", EClassificacaoConfiguracao.NFCe).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "NFCE_NATOP_HOMOL")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "NFCe Natureza Operação - Homologação", EClassificacaoConfiguracao.NFCe).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "NFCE_SERIE")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "NFCe Serie", EClassificacaoConfiguracao.NFCe).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "NFCE_SERIE_HOMOL")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "NFCe Serie - Homologação", EClassificacaoConfiguracao.NFCe).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "PASTA_FOTOS")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Pasta de Fotos", EClassificacaoConfiguracao.Empresa).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "PDV_IMPRESSORA_COZINHA")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Impressora PDV Cozinha", EClassificacaoConfiguracao.PDV).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "PDV_PORTA_IMPRESSORA_COZINHA")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Numero, "Porta Impressora PDV Cozinha", EClassificacaoConfiguracao.PDV).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "PDV_PREVENDA")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "PDV Pré-Venda", EClassificacaoConfiguracao.PDV).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "PDV_TAMANHO_FONTE")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "PDV Tamanho Fonta", EClassificacaoConfiguracao.PDV).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "PORTA_IMPRESSORA")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Porta Impressora", EClassificacaoConfiguracao.PDV).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "TXENTREGA_COBRAR")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Cobrar Taxa de entrega", EClassificacaoConfiguracao.Pedido).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "TXENTREGA_FORMA")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Forma Pagamento Taxa de entrega", EClassificacaoConfiguracao.Pedido).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "TXENTREGA_VALOR")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Valor Taxa de entrega", EClassificacaoConfiguracao.Pedido).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "TXENTREGA_VLMINIMO")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Valor Mínimo Taxa de entrega", EClassificacaoConfiguracao.Pedido).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "VENDAS_DOC_FISCAL_PADRAO")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Padrão Documento Valor Fiscal - Vendas", EClassificacaoConfiguracao.Pedido).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "VENDAS_DOC_FISCAL_PADRAO_STR")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Padrão Documento Valor Fiscal STR - Vendas", EClassificacaoConfiguracao.Pedido).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "CAIXA_TPABERTURA")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.TipoAberturaCaixa, "Tipo de Abertura de Caixa", EClassificacaoConfiguracao.Caixa).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "CERTIFICADO_CAMINHO")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Caminho Certificado Digital", EClassificacaoConfiguracao.Gerais).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "CONTA_IDCONTAESTOQUE")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Conta Registro Estoque", EClassificacaoConfiguracao.Contabil).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "CONTA_NMCONTAESTOQUE")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Nome Contabil Conta Estoque", EClassificacaoConfiguracao.NaoExibir).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "CONTA_NMCONTAFORNECEDOR")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Nome Contabil Conta Fornecedor", EClassificacaoConfiguracao.NaoExibir).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "CONTA_NMCONTACLIENTE")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Nome Contabil Conta Cliente", EClassificacaoConfiguracao.NaoExibir).Result;
            }
            else if (model.CHAVE.Trim().ToUpper() == "PREVENDA_ATIVO")
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Pre-Venda Ativo", EClassificacaoConfiguracao.NaoExibir).Result;
            }
            else
            {
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Vazio", EClassificacaoConfiguracao.Gerais).Result;
            }
        }

       

        private string ObterDescricao(string chave)
        {
            var resultado = "Logo Relatorio";
            if (chave.ToUpper().Contains("IMG_LOGONFCE"))
                resultado = "Logo NFCe";
            else if (chave.ToUpper().Contains("IMG_FUNDOPDV"))
                resultado = "Fundo PDV";

            return resultado;
        }

        #endregion
    }
}

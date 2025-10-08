using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Services;
using agilum.mvc.web.ViewModels.Config;
using agilum.mvc.web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using agilum.mvc.web.Enums;
using System.Reflection;
using agilium.api.business.Models;
using agilum.mvc.web.Services;
using agilum.mvc.web.Extensions;
using agilium_manager_azure_business.Interfaces.IService;
using agilum.mvc.web.Data;
using Microsoft.AspNetCore.Identity;

namespace agilum.mvc.web.Controllers
{
    [Route("config")]
    [Authorize]
    public class ConfigController : MainController
    {
        private readonly IConfigService _configService;
        private readonly string _nomeEntidade = "Configuração";

        #region construtor
        public ConfigController(INotificador notificador, IConfiguration configuration, IUser appUser, 
            IUtilDapperRepository utilDapperRepository, ILogService logService, IMapper mapper, IConfigService configService,
            ILicencaService licencaService, SignInManager<AppUserAgiliumIdentity> signInManager) : 
            base(notificador, configuration, appUser, utilDapperRepository, logService, mapper, licencaService, signInManager)
        {
            _configService = configService;
        }
        #endregion

        #region config
        [HttpGet]
        [Route("lista")]
        [ClaimsAuthorizeAttribute(1015)]
        public async Task<IActionResult> Index([FromQuery] int ps = 10, [FromQuery] int page = 1, [FromQuery] string q = null)
        {

            var empresaSelecionada = ObterObjetoEmpresaSelecionada();

            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
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

            var lista = await ObterListaPaginado(Convert.ToInt64(empresaSelecionada.IDEMPRESA), q, page, ps);
            ViewBag.Pesquisa = q;
            ViewBag.idEmpresa = Convert.ToInt64(empresaSelecionada.IDEMPRESA);
            lista.ReferenceAction = "lista";

            var listaEditarChaveConfig = new List<EditarChaveValorViewModel>();
            lista.List.ToList().ForEach(config => {
                var configuraçãConvertida = ConverterCamposConfigViewModel(config, Convert.ToInt64(empresaSelecionada.IDEMPRESA)).Result;

                if (configuraçãConvertida.Classificacao != EClassificacaoConfiguracao.NaoExibir) listaEditarChaveConfig.Add(configuraçãConvertida);
            });
            var listaConvertida = new PagedViewModel<EditarChaveValorViewModel>()
            {
                PageIndex = lista.PageIndex,
                PageSize = lista.PageSize,
                Query = lista.Query,
                ReferenceAction = lista.ReferenceAction,
                TotalResults = lista.TotalResults,
                ReferenceController = "config",
                List = listaEditarChaveConfig
            };
            return View(listaConvertida);
        }

        [Route("lista-imagem")]
        [ClaimsAuthorizeAttribute(1015)]
        public async Task<ActionResult> IndexConfigImagem()
        {
            var empresaSelecionada = ObterObjetoEmpresaSelecionada();


            if (empresaSelecionada == null || string.IsNullOrEmpty(empresaSelecionada.IDEMPRESA))
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

            _configService.ObterTodosConfigImagem(Convert.ToInt64(empresaSelecionada.IDEMPRESA)).Result.ToList().ForEach(item => {
                var model = new ConfigImagemViewModel();

                if (item.IMG != null)
                    model.ImagemConvertida = String.Format("data:image/png;base64,{0}", Utils.ConverterByteToBase64(item.IMG));
                model.IDEMPRESA = item.IDEMPRESA;
                model.CHAVE = item.CHAVE;
                model.IMG = item.IMG;
                model.Descricao = ObterDescricao(item.CHAVE);

                lista.Add(model);
            });

            var objetos = _mapper.Map<IEnumerable<ConfigImagemViewModel>>(lista);

            return View("ConfigImagem", lista);
        }

        [Route("editar-imagem")]
        public async Task<ActionResult> EditConfigImage(string chave, long idEmpresa)
        {
            var objeto = _mapper.Map<ConfigImagemViewModel>(await _configService.ObterConfigImagemPorChave(chave, idEmpresa));
            objeto.ImagemConvertida = String.Format("data:image/png;base64,{0}", Utils.ConverterByteToBase64(objeto.IMG));

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
        [Route("editar-imagem")]
        [ClaimsAuthorizeAttribute(1015)]
        public async Task<ActionResult> EditConfigImage(ConfigImagemViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            model.ImagemConvertida = model.ImagemConvertida.Replace("data:image/png;base64,", "")
                                                            .Replace("data:image/jpeg;base64,", "")
                                                            .Replace("data:image/jpg;base64,", "")
                                                            .Replace("data:image/bmp;base64,", "");
            var configImagem = _configService.ObterConfigImagemPorChave(model.CHAVE, Convert.ToInt64(model.IDEMPRESA)).Result;

            if (configImagem == null)
            {
                NotificarErro("erro ao tenta atualizar configuração de imagem");
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "AtualizaConfigImagem", "Web"));
                return View(model);
            }

            var memoryStream = new MemoryStream();
            await model.ImagemUpLoad.CopyToAsync(memoryStream);

            configImagem.IMG = memoryStream.ToArray();

            await _configService.Atualizar(configImagem);

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao editar configuração" };
                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            await _configService.Salvar();
            LogInformacao("Config", "AtualizaConfigImagem", "Web", $"Configuração de imagem {model.CHAVE} atualizada com sucesso");
            ViewBag.TipoMensagem = "success";
            ViewBag.Titulo = _nomeEntidade;
            ViewBag.Mensagem = "Operação realizada com sucesso";
            return RedirectToAction("IndexConfigImagem");
        }

        [Route("editar")]
        [ClaimsAuthorizeAttribute(1015)]
        public async Task<IActionResult> Edit(long idEmpresa)
        {
            var objeto = _mapper.Map<List<ConfigIndexViewModel>>(await _configService.ObterTodosPorEmpresa(idEmpresa));
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
            var objetoConvertido = await ConverterCamposConfigViewModel(objeto, idEmpresa);

            return View(objetoConvertido);
        }

        [HttpPost]
        [Route("editar")]
        [ClaimsAuthorizeAttribute(1015)]
        public async Task<IActionResult> Edit(ConfigCamposViewModel model)
        {

            if (!ModelState.IsValid) return View(model);

            var listaModels = await ConverterChaveValor(model);
            
            listaModels.ToList().ForEach(config => {
                if (!string.IsNullOrEmpty(config.CHAVE))
                {
                    _configService.Atualizar(_mapper.Map<Config>(config));
                    if (config.Arquivo != null)
                        if (!UploadArquivoAlternativo(config.Arquivo, model.IdEmpresa.ToString()).Result)
                        {
                            NotificarErro($"Erro ao tentar salvar arquivo: {config.CHAVE}");
                        }
                }
            });

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao editar configuração" };
                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }
            await _configService.Salvar();
          LogInformacao("Config", "AtualizaConfigCampos", "Web", $"Configurações atualizadas com sucesso");
            ViewBag.TipoMensagem = "success";
            ViewBag.Titulo = _nomeEntidade;
            ViewBag.Mensagem = "Operação realizada com sucesso";
            return View(model);
        }

        [Route("editar-item")]
        [ClaimsAuthorizeAttribute(1015)]
        public async Task<IActionResult> EditItem(string chave, long idEmpresa)
        {
            var objeto = _mapper.Map<ConfigIndexViewModel>(await _configService.ObterPorChave(chave, idEmpresa));
            if (objeto == null)
            {
                var msgErro = $"{_nomeEntidade} não localizado";
                AdicionarErroValidacao(msgErro);
                TempData["Erros"] = msgErro;

                ViewBag.TipoMensagem = "danger";
                ViewBag.Titulo = _nomeEntidade;
                ViewBag.Mensagem = msgErro;
                return RedirectToAction("Index", "Config", new { idEmpresa = idEmpresa });
            }
            var objetoConvertido = await ConverterCamposConfigViewModel(objeto, idEmpresa);

            return View(objetoConvertido);
        }

        [HttpPost]
        [Route("editar-item")]
        [ClaimsAuthorizeAttribute(1015)]
        public async Task<IActionResult> EditItem(EditarChaveValorViewModel model)
        {

            if (!ModelState.IsValid) return View(model);
           
            if (!string.IsNullOrEmpty(model.Chave))
            {
                await _configService.AtualizarManualmente(_mapper.Map<Config>(model));
            };

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao editar configuração" };
                AdicionarErroValidacao(retornoErro.mensagem);
                return View(model);
            }

            LogInformacao("Config", "AtualizaConfigItem", "Web", $"Configuração {model.Chave} atualizada com sucesso");
            ViewBag.TipoMensagem = "success";
            ViewBag.Titulo = _nomeEntidade;
            ViewBag.Mensagem = "Operação realizada com sucesso";
            return RedirectToAction("Index", "Config", new { idEmpresa = model.IdEmpresa });
        }

        [Route("editar-certificado")]
        [ClaimsAuthorizeAttribute(1015)]
        public async Task<IActionResult> EditCertificado(long idEmpresa)
        {
            var chaveCertificado = "CERTIFICADO_CAMINHO";
            var objeto = _mapper.Map<ConfigIndexViewModel>(await _configService.ObterPorChave(chaveCertificado, idEmpresa));
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
            var objetoConvertido = new ChaveValorViewModel()
            {
                CHAVE = objeto.CHAVE,
                IDEMPRESA = idEmpresa,
                VALOR = objeto.VALOR
            };

            return View(objetoConvertido);
        }


        [HttpPost]
        [Route("editar-certificado")]
        [ClaimsAuthorizeAttribute(1015)]
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
            //var resposta = await _configService.AtualizarCertificado(model.IDEMPRESA, conversao);
            
            if (!UploadArquivoAlternativo(model.Arquivo, model.IDEMPRESA.ToString()).Result)
            {
                NotificarErro($"Erro ao tentar fazer upload do certificado");
            }

            if (!OperacaoValida())
            {
                var retornoErro = new { mensagem = $"Erro ao tentar atualizar certificado" };
                AdicionarErroValidacao(retornoErro.mensagem);
                TempData["TipoMensagem"] = "danger";
                TempData["Mensagem"] = retornoErro;
            }
           LogInformacao("Config", "AtualizaCertificado", "Web", $"Certificado atualizado com sucesso");
            TempData["TipoMensagem"] = "success";
            TempData["Mensagem"] = "Certificado atualizado com sucesso";

            return RedirectToAction("Index", "Config", new { idEmpresa = model.IDEMPRESA });
        }
        #endregion

        #region private
        private async Task<bool> UploadArquivoAlternativo(IFormFile arquivo, string idEmpresa)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                NotificarErro("Forneça uma imagem!");
                return false;
            }

            var imgPrefixo = Guid.NewGuid() + "_";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/certificado", idEmpresa + arquivo.FileName);
            if (System.IO.File.Exists(path))
            {
                NotificarErro("Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }

        private async Task<PagedViewModel<ConfigIndexViewModel>> ObterListaPaginado(long idEmpresa, string filtro, int page, int pageSize)
        {
            var retorno = await _configService.ObterPorDescricaoPaginacao(idEmpresa, filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<ConfigIndexViewModel>>(retorno.List);

            return new PagedViewModel<ConfigIndexViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                ReferenceAction = "lista",
                TotalResults = retorno.TotalResults
            };
        }
        #endregion



        #region Private ConverterViewModel

        private async Task<ChaveValorViewModel> ConverterChaveValor(EditarChaveValorViewModel model)
        {
            return new ChaveValorViewModel()
            {
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
            resuldado.CAIXA_DSSUPRIMENTO = ConverterChaveValorViewModel(model, "CAIXA_DSSUPRIMENTO", idEmpresa).Result;
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
            var chaveValor = new ChaveValorViewModel()
            {
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
            return new EditarChaveValorViewModel()
            {
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
                return ConverterEditarChaveValorViewModel(model, idEmpresa, ETipoCompnenteConfig.Texto, "Descrição Padrão Suprimento", EClassificacaoConfiguracao.Caixa).Result;
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

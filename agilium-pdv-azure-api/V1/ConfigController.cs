using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Services;
using agilium.api.pdv.Controllers;
using agilium.api.pdv.ViewModels.ConfigViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.pdv.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/config")]
    [ApiVersion("1.0")]
    public class ConfigController : MainController
    {

        private readonly IConfigService _configService;
        private readonly IMapper _mapper;
        private readonly IPontoVendaService _pontoVendaService;
        private readonly IUsuarioService _usuarioService;
        private const string _nomeEntidade = "Configuração";

        public ConfigController(INotificador notificador, IUser appUser, IConfiguration configuration, IUtilDapperRepository utilDapperRepository, ILogService logService,
            IConfigService configService, IMapper mapper, IPontoVendaService pontoVendaService, IUsuarioService usuarioService) : base(notificador, appUser, configuration, utilDapperRepository, logService)
        {
            _configService = configService;
            _mapper = mapper;
            _pontoVendaService = pontoVendaService;
            _usuarioService = usuarioService;
        }

        #region configuracao Geral
        [HttpGet("obter-config-por-chave")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConfigIndexViewModel>> ObterPorId([FromQuery] string chave, [FromQuery] long idEmpresa)
        {
            var objeto = _mapper.Map<ConfigIndexViewModel>(await _configService.ObterPorChave(chave, idEmpresa));
            return CustomResponse(objeto);
        }

        [HttpGet("obter-por-empresa/{idEmpresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ConfigIndexViewModel>>> ObterTodosPorEmpresa(long idEmpresa)
        {
            var objetos = _mapper.Map<IEnumerable<ConfigIndexViewModel>>(await _configService.ObterTodosPorEmpresa(idEmpresa));
            return CustomResponse(objetos);
        }

        [HttpPut("{idEmpresa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequestSizeLimit(60000000)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(long idEmpresa, [FromBody] IEnumerable<ConfigIndexViewModel> viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            viewModel.ToList().ForEach(config => {
                if (config.IDEMPRESA.HasValue && !string.IsNullOrEmpty(config.CHAVE))
                {
                    _configService.Atualizar(_mapper.Map<Config>(config));                   
                }
            });

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "Atualizar", "WebPDV"));
                return CustomResponse(msgErro);
            }
            await _configService.Salvar();
            LogInformacao($"Objeto atualizar com sucesso {viewModel}", "Config", "AtualizarPDV", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("atualiza-config/{idEmpresa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequestSizeLimit(60000000)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(long idEmpresa, [FromBody] ConfigIndexViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.IDEMPRESA.HasValue && !string.IsNullOrEmpty(viewModel.CHAVE))
            {
                await _configService.Atualizar(_mapper.Map<Config>(viewModel));
            };

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "AtualizarPDV", "Web"));
                return CustomResponse(msgErro);
            }
            await _configService.Salvar();
            LogInformacao($"Objeto atualizar com sucesso {Deserializar(viewModel)}", "Config", "AdicionarPDV", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("obter-pdv/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PontoVendaViewModel>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var fornecedor = await _pontoVendaService.ObterCompletoPorId(_id);
            if (fornecedor != null)
            {
                var objeto = _mapper.Map<PontoVendaViewModel>(fornecedor);
                return CustomResponse(objeto);
            }

            return CustomResponse(BadRequest("ponto de venda nao localizado"));

        }

        [HttpPut("atualiza-config-geral/{idEmpresa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> AtualizarConfigGeral(string idEmpresa, [FromBody] ConfigGeralViewModel viewModel) 
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            long _idempresa = Convert.ToInt64(idEmpresa);

            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
            if (usuario == null)
            {
                NotificarErro("Erro ao tentar salvar configuração, usuario nao localizado");
            }

            var objeto = await ConverterConfigGeral(viewModel, _idempresa);

            await _configService.AtualizarConfig(objeto, usuario.Id);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "AtualizarConfigGeral", "Web"));
                return CustomResponse(msgErro);
            }
            await _configService.Salvar();
            LogInformacao($"Objeto atualizar com sucesso {Deserializar(viewModel)}", "Config", "AtualizarConfigGeral", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("obter-ponto-venda-disponveis-associacao/{idempresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConfigGeralViewModel>> ObterPdvDisponiveisParaAssociacao(string idempresa)
        {
            long _idempresa = Convert.ToInt64(idempresa);
            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
            if (usuario == null)
            {
                NotificarErro("Erro ao tentar salvar configuração, usuario nao localizado");
            }

            var model = _mapper.Map<IEnumerable<PontoVendaAssociacao>>(_configService.ObterPdvParaSelecao(_idempresa, usuario.Id).Result.ToList());

            return CustomResponse(model);
        }

        [HttpGet("obter-ponto-venda-por-usuario")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConfigGeralViewModel>> ObterPdvPorUsuario()
        {
            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
            if (usuario == null)
            {
                NotificarErro("Erro ao tentar salvar configuração, usuario nao localizado");
            }

            var model = await _configService.ObterPdvPorNomeMaquina(usuario.Id);

            return CustomResponse(model);
        }

        [HttpGet("obter-config-geral/{idempresa}/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConfigGeralViewModel>> ObterConfigGeral(string idempresa, string id)
        {
          //  long _id = Convert.ToInt64(id);
            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(id).Result;
            if (usuario == null)
            {
                NotificarErro("Erro ao tentar obter configuração");
            }

            long _idempresa = Convert.ToInt64(idempresa);
            var model = await ConverterConfigGeral(_configService.ObterConfiguracaoGeral(_idempresa, usuario.Id).Result.ToList());
            
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "DefinirPDV", "WebPDV", Deserializar(model)));
                return CustomResponse(msgErro);
            }
            return CustomResponse(model);
        }

        private async Task<List<Config>> ConverterConfigGeral(ConfigGeralViewModel model, long IDEMPRESA)
        {
            var resultado = new List<Config>();
            
            resultado.Add(new Config("IDPDV", IDEMPRESA, model.idPdv));
            resultado.Add(new Config("PDV_PREVENDA", IDEMPRESA,model.EmissaoCupomVenda?"S":"N"));
            resultado.Add(new Config("PDV_TAMANHO_FONTE",IDEMPRESA,model.TamanhoFonte.ToString()));
            resultado.Add(new Config("STGAVETA",IDEMPRESA,model.GavetaDinheiro));

            return resultado;
        }
        private async Task<ConfigGeralViewModel> ConverterConfigGeral(List<Config> lista)
        {
            var resultado = new ConfigGeralViewModel();

            var idpdv = lista.FirstOrDefault(x => x.CHAVE == "IDPDV");
            if (idpdv != null)
                resultado.idPdv = idpdv.VALOR;

            var nmImpresora = lista.FirstOrDefault(x => x.CHAVE == "NMIMPRESSORA");
            if (nmImpresora != null)
                resultado.ImpressoraCupom = nmImpresora.VALOR;

            var portaImpressora = lista.FirstOrDefault(x => x.CHAVE == "DSPORTAIMPRESSORA");
            if (portaImpressora != null)
                resultado.PortaImpressora= portaImpressora.VALOR;

            var gaveta = lista.FirstOrDefault(x => x.CHAVE == "STGAVETA");
            if (gaveta != null)
                resultado.GavetaDinheiro = gaveta.VALOR;

            var tamanhoFonte = lista.FirstOrDefault(x => x.CHAVE == "PDV_TAMANHO_FONTE");
            if (tamanhoFonte != null)
            {
                int fonte = 0;
                Int32.TryParse(tamanhoFonte.VALOR, out fonte);
                resultado.TamanhoFonte = fonte;
            }

            var pre_venda = lista.FirstOrDefault(x => x.CHAVE == "PDV_PREVENDA");
            if (pre_venda != null)
                resultado.EmissaoCupomVenda = pre_venda.VALOR == "S";

            return resultado;
        }

        [HttpPost("definir-pdv")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> DefinirPDV([FromBody] PontoVendaViewModel viewModel)
        {

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _configService.DefinirMaquinaPdv(viewModel.NomeMaquina, Convert.ToInt64(viewModel.Id));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "DefinirPDV", "WebPDV", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Config", "DefinirPDV", null);
            return CustomResponse(viewModel);
        }

        [HttpPost("definir-balanca")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> DefinirBalanca([FromBody] PontoVendaViewModel viewModel)
        {

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _configService.DefinirBalancaPDV(viewModel.CDMODELOBAL, viewModel.CDHANDSHAKEBAL, viewModel.CDPARITYBAL, viewModel.CDSERIALSTOPBITBAL,
                viewModel.NUDATABITBAL, viewModel.NUBAUDRATEBAL, viewModel.PortaImpressora, Convert.ToInt64(viewModel.Id));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "DefinirBalanca", "WebPDV", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Config", "DefinirBalanca", null);
            return CustomResponse(viewModel);
        }

        [HttpPost("definir-certificado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> DefinirCertificado([FromBody] PontoVendaViewModel viewModel)
        {

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _configService.DefinirCertificadoDigitalMaquina(Convert.ToInt64(viewModel.IDEMPRESA), viewModel.NomeMaquina, viewModel.CaminhoCertificadoDigital, viewModel.SenhaCertificadoDigital);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "DefinirCertificado", "WebPDV", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Config", "DefinirCertificado", null);
            return CustomResponse(viewModel);
        }

        [HttpPost("definir-porta")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> DefinirPorta([FromBody] PontoVendaViewModel viewModel)
        {

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _configService.DefinirPortaImpressoraPDV(viewModel.PortaImpressora, Convert.ToInt64(viewModel.Id));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "DefinirPorta", "WebPDV", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Config", "DefinirPorta", null);
            return CustomResponse(viewModel);
        }

        [HttpPost("desassociar-pdv")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> DesassociarPdv([FromBody] PontoVendaViewModel viewModel)
        {

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _configService.DesassociarMaquinaPDV(Convert.ToInt64(viewModel.Id));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "DesassociarPdv", "WebPDV", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Config", "DesassociarPdv", null);
            return CustomResponse(viewModel);
        }
        #endregion

        #region Config balanca
        [HttpGet("obter-config-balanca/{idempresa}/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConfigGeralViewModel>> ObterConfigBalanca(string idempresa, string id)
        {
            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(id).Result;
            if (usuario == null)
            {
                NotificarErro("Erro ao tentar obter configuração");
            }
            // long _id = Convert.ToInt64(id);
            long _idempresa = Convert.ToInt64(idempresa);
            var model = await ConverterConfigBalanca(_configService.ObterConfiguracaoBalanca(_idempresa, usuario.Id).Result.ToList());

            return CustomResponse(model);
        }

        [HttpPut("atualiza-config-balanca/{idempresa}/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> AtualizarConfigBalanca(string idempresa, string id, [FromBody] ConfigBalanca viewModel)
        {
            long _id = Convert.ToInt64(id);
            long _idempresa = Convert.ToInt64(idempresa);

            if (!ModelState.IsValid) return CustomResponse(ModelState);
            
            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
            if (usuario == null)
            {
                NotificarErro("Erro ao tentar salvar configuração, usuario nao localizado");
            }
            if(_id == 0)
            {
                _id = await _configService.ObterPdvPorNomeMaquina(usuario.Id);
            }

            await _configService.DefinirBalancaPDV(viewModel.cdModelo,viewModel.cdHandShake,viewModel.cdParity,viewModel.cdSerialStop,
                viewModel.nuDataBits,viewModel.nuBaudRate,viewModel.dsPorta,_id);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "AtualizarConfigBalamca", "Web"));
                return CustomResponse(msgErro);
            }
            await _configService.Salvar();
            LogInformacao($"Objeto atualizar com sucesso {Deserializar(viewModel)}", "Config", "AtualizarConfigBalanca", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("obter-lista-balanca")]
        public async Task<ActionResult<ListasViewModel>> ObterListasBalancas()
        {
            return CustomResponse(await MontarListaBalanca());
        }

        private async Task<ConfigBalanca> ConverterConfigBalanca(List<Config> lista)
        {
            var resultado = new ConfigBalanca();

            var CDMODELOBAL = lista.FirstOrDefault(x => x.CHAVE == "CDMODELOBAL");
            if (CDMODELOBAL != null)
                resultado.cdModelo = CDMODELOBAL.VALOR;

            var CDHANDSHAKEBAL = lista.FirstOrDefault(x => x.CHAVE == "CDHANDSHAKEBAL");
            if (CDHANDSHAKEBAL != null)
                resultado.cdHandShake = CDHANDSHAKEBAL.VALOR;

            var CDPARITYBAL = lista.FirstOrDefault(x => x.CHAVE == "CDPARITYBAL");
            if (CDPARITYBAL != null)
                resultado.cdParity = CDPARITYBAL.VALOR;

            var CDSERIALSTOPBITBAL = lista.FirstOrDefault(x => x.CHAVE == "CDSERIALSTOPBITBAL");
            if (CDSERIALSTOPBITBAL != null)
                resultado.cdSerialStop = CDSERIALSTOPBITBAL.VALOR;

            var NUDATABITBAL = lista.FirstOrDefault(x => x.CHAVE == "NUDATABITBAL");
            if (NUDATABITBAL != null)
                resultado.nuDataBits = NUDATABITBAL.VALOR;

            var NUBAUDRATEBAL = lista.FirstOrDefault(x => x.CHAVE == "NUBAUDRATEBAL");
            if (NUBAUDRATEBAL != null)
                resultado.nuBaudRate = NUBAUDRATEBAL.VALOR;

            var DSPORTABAL = lista.FirstOrDefault(x => x.CHAVE == "DSPORTABAL");
            if (DSPORTABAL != null)
                resultado.dsPorta = DSPORTABAL.VALOR;

            return resultado;
        }

        private async Task<List<Config>> ConverterConfigBalanca(ConfigBalanca model, long IDEMPRESA)
        {
            var resultado = new List<Config>();

            resultado.Add(new Config("CDMODELOBAL", IDEMPRESA, model.cdModelo));
            resultado.Add(new Config("CDHANDSHAKEBAL", IDEMPRESA, model.cdHandShake));
            resultado.Add(new Config("CDPARITYBAL", IDEMPRESA, model.cdParity));
            resultado.Add(new Config("CDSERIALSTOPBITBAL", IDEMPRESA, model.cdSerialStop));
            resultado.Add(new Config("NUDATABITBAL", IDEMPRESA, model.nuDataBits));
            resultado.Add(new Config("NUBAUDRATEBAL", IDEMPRESA, model.nuBaudRate));
            resultado.Add(new Config("DSPORTABAL", IDEMPRESA, model.dsPorta));
            
            return resultado;
        }

        private async Task<ListasViewModel> MontarListaBalanca()
        {
            var resultado = new ListasViewModel();
            
            //handshaking
            resultado.Handshaking.Add(new ListaBalanca(1,"Nenhum"));
            resultado.Handshaking.Add(new ListaBalanca(2, "XON/XOFF"));
            resultado.Handshaking.Add(new ListaBalanca(3, "RTS/CTS"));
            resultado.Handshaking.Add(new ListaBalanca(4, "DTR/DSR"));

            //modelo
            resultado.Modelos.Add(new ListaBalanca(1, "balNenhum"));
            resultado.Modelos.Add(new ListaBalanca(2, "balFilizola"));
            resultado.Modelos.Add(new ListaBalanca(3, "balToledo"));
            resultado.Modelos.Add(new ListaBalanca(4, "balUrano"));
            resultado.Modelos.Add(new ListaBalanca(1, "balMagna"));

            //Baud Rate
            resultado.BaudRate.Add(new ListaBalanca(1, "110"));
            resultado.BaudRate.Add(new ListaBalanca(2, "300"));
            resultado.BaudRate.Add(new ListaBalanca(3, "600"));
            resultado.BaudRate.Add(new ListaBalanca(4, "1200"));
            resultado.BaudRate.Add(new ListaBalanca(5, "2400"));
            resultado.BaudRate.Add(new ListaBalanca(6, "4800"));
            resultado.BaudRate.Add(new ListaBalanca(7, "9600"));
            resultado.BaudRate.Add(new ListaBalanca(8, "14400"));

            //data bits
            resultado.DataBits.Add(new ListaBalanca(1, "5"));
            resultado.DataBits.Add(new ListaBalanca(2, "6"));
            resultado.DataBits.Add(new ListaBalanca(3, "7"));
            resultado.DataBits.Add(new ListaBalanca(4, "8"));

            //parity
            resultado.Parity.Add(new ListaBalanca(1, "none"));
            resultado.Parity.Add(new ListaBalanca(2, "odd"));
            resultado.Parity.Add(new ListaBalanca(3, "even"));
            resultado.Parity.Add(new ListaBalanca(4, "mark"));
            resultado.Parity.Add(new ListaBalanca(5, "space"));

            //stop bits
            resultado.StopBits.Add(new ListaBalanca(1, "s1"));
            resultado.StopBits.Add(new ListaBalanca(2, "s1,5"));
            resultado.StopBits.Add(new ListaBalanca(3, "s2"));

            //porta serial
            resultado.PortaSerial.Add(new ListaBalanca(1, "COM1"));
            resultado.PortaSerial.Add(new ListaBalanca(2, "COM2"));
            resultado.PortaSerial.Add(new ListaBalanca(3, "COM3"));
            resultado.PortaSerial.Add(new ListaBalanca(4, "COM4"));
            resultado.PortaSerial.Add(new ListaBalanca(5, "COM5"));
            resultado.PortaSerial.Add(new ListaBalanca(6, "COM6"));
            resultado.PortaSerial.Add(new ListaBalanca(7, "COM7"));
            resultado.PortaSerial.Add(new ListaBalanca(8, "COM8"));

            return resultado;
        }
        #endregion

        #region Config PreVenda
        [HttpGet("obter-config-pre-venda/{idempresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConfigPreVendaViewModel>> ObterConfigPreVenda(string idempresa)
        {
            long _idempresa = Convert.ToInt64(idempresa);
            var config = _configService.ObterConfigPreVenda(_idempresa).Result.ToList().FirstOrDefault(x => x.CHAVE == "PDV_PREVENDA");
            var model = new ConfigPreVendaViewModel();
            model.PreVenda = !string.IsNullOrEmpty(config.VALOR) ? config.VALOR: "N";
            return CustomResponse(model);
        }

        [HttpPut("atualiza-config-pre-venda/{idEmpresa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public async Task<ActionResult> AtualizarConfigPreVenda(string idEmpresa, [FromBody] ConfigPreVendaViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            long _idempresa = Convert.ToInt64(idEmpresa);

            var usuario = _usuarioService.ObterPorUsuarioAspNetPorId(AppUser.GetUserId().ToString()).Result;
            if (usuario == null)
            {
                NotificarErro("Erro ao tentar salvar configuração, usuario nao localizado");
            }

            var objeto = new List<Config>();
            objeto.Add(new Config("PDV_PREVENDA", _idempresa,viewModel.PreVenda));

            await _configService.AtualizarConfigPreVenda(objeto, usuario.Id);

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "AtualizarConfigPreVenda", "Web"));
                return CustomResponse(msgErro);
            }
            await _configService.Salvar();
            LogInformacao($"Objeto atualizar com sucesso {Deserializar(viewModel)}", "Config", "AtualizarConfigPreVenda", null);
            return CustomResponse(viewModel);
        }
        #endregion
    }
}

using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Notificacoes;
using agilium_manager_azure_business.Interfaces.IService;
using agilium.api.business.Models;
using agilum.mvc.web.ViewModels.EmpresaUsuario;
using agilum.mvc.web.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    public abstract class MainController: Controller
    {
        private readonly INotificador _notificador;
        protected readonly IConfiguration _configuration;
        protected readonly IMapper _mapper;

        public readonly IUser AppUser;
        protected Guid UsuarioId { get; set; }
        protected bool UsuarioAutenticado { get; set; }
        protected readonly IUtilDapperRepository _utilDapperRepository;
        protected readonly ILogService _logService;
        protected readonly ILicencaService _licencaService;
        protected readonly IAuthService _authService;

        private INotificador notificador;
        private IConfiguration configuration;
        private IUtilDapperRepository utilDapperRepository;
        private ILogService logService;
        private IMapper mapper;

        protected MainController(INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, 
            ILogService logService, IMapper mapper, ILicencaService licencaService, IAuthService authService)
        {
            _notificador = notificador;
            _configuration = configuration;
            AppUser = appUser;
            _utilDapperRepository = utilDapperRepository;
            _logService = logService;
            _mapper = mapper;
            _authService = authService;
            if (appUser.IsAuthenticated())
            {
                UsuarioId = appUser.GetUserId();
                UsuarioAutenticado = true;
            }
            _licencaService = licencaService;
        }

        protected MainController(INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService, IMapper mapper)
        {
            this.notificador = notificador;
            this.configuration = configuration;
            AppUser = appUser;
            this.utilDapperRepository = utilDapperRepository;
            this.logService = logService;
            this.mapper = mapper;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        protected async Task<long> GerarId()
        {
            return await _utilDapperRepository.GerarUUID();
        }

        protected async Task<int> GerarIdInt(string generator)
        {
            return await _utilDapperRepository.GerarIdInt(generator);
        }

        protected void NotificarErro(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }

        protected string[] ObterNotificacoes()
        {
            return _notificador.ObterNotificacoes().Select(n => n.Mensagem).ToArray();
        }

        protected string[] ObterNotificacoes(string tela, string controle, string tipo, string msg)
        {
            if (!string.IsNullOrEmpty(msg))
                LogErro(msg, tela, controle, null, tipo);

            return ObterNotificacoes(tela, controle, tipo);
        }

        protected string[] ObterNotificacoes(string tela, string controle, string tipo)
        {
            var lista = _notificador.ObterNotificacoes().Select(n => n.Mensagem).ToArray();

            lista.ToList().ForEach(erro => {
                LogErro(erro, tela, controle, null, tipo);
            });

            return lista;
        }

        protected int ObterQuantidadeLinhasPorPaginas()
        {
            return 15;
        }

        protected void AdicionarErroValidacao(string mensagem)
        {
            ModelState.AddModelError(string.Empty, mensagem);
        }

        protected void LogInformacao(string msg, string tela, string controle, string sql)
        {
            _logService.Adicionar(AppUser.GetUserEmail(), msg, tela, controle, "Web", sql, null);
        }

        protected void LogErro(string msg, string tela, string controle, string sql, string tipo)
        {
            _logService.Erro(AppUser.GetUserEmail(), msg, tela, controle, "Web", sql, tipo);
        }

        protected void LogErro(string usuario, string msg, string tela, string controle, string sql, string tipo)
        {
            _logService.Erro(usuario, msg, tela, controle, "Web", sql, tipo);
        }
        protected string Deserializar(object objeto) => JsonSerializer.Serialize(objeto, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });

        protected string RetirarPontos(string valor)
        {
            return valor.Replace(".", "").Replace("-", "").Replace("/", "").Replace(",", "");
        }

        protected string ObterStringEmpresaSelecionada()
        {
            return HttpContext.Session.GetString("_empSelec");
        }

        protected EmpresaUsuarioViewModel ObterObjetoEmpresaSelecionada()
        {
            var empresa = ObterStringEmpresaSelecionada();
            if (empresa == null)
            {
                // Fallback: ler das Claims (cookie de autenticação)
                var idEmpresa = User.FindFirst("IDEMPRESA")?.Value;
                var nomeEmpresa = User.FindFirst("NomeEmpresa")?.Value;
                if (!string.IsNullOrEmpty(idEmpresa))
                    return new EmpresaUsuarioViewModel() { IDEMPRESA = idEmpresa, NomeEmpresa = nomeEmpresa ?? "Empresa" };
                
                return new EmpresaUsuarioViewModel() { NomeEmpresa = "Selecionar Empresa"};
            }
            var objeto = System.Text.Json.JsonSerializer.Deserialize<EmpresaUsuarioViewModel>(empresa);
            return objeto;
        }

        protected async Task VerificarValidadeLicenca()
        {
            var empresa = ObterObjetoEmpresaSelecionada();
            if(empresa != null)
            {
                if(! _licencaService.DataValida(Convert.ToInt64(empresa.IDEMPRESA)).Result)
                {
                    var retornoErro = new { mensagem = $"Verificação de validade da empresa falhou por divergência na Data de Validade" };
                    AdicionarErroValidacao(retornoErro.mensagem);

                    await Logout();
                }
            }
        }

        private async Task Logout()
        {
            HttpContext.Session.Remove("_empSelec");
            await _authService.SignOutAsync(HttpContext);
        }
    }
}


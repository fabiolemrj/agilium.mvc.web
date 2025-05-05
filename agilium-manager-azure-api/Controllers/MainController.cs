using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Notificacoes;
using agilium.api.manager.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace agilium.api.manager.Controllers
{
    [ApiController]
    public class MainController : Controller
    {
        protected readonly IConfiguration _configuration;

        private readonly INotificador _notificador;
        public readonly IUser AppUser;
        protected Guid UsuarioId { get; set; }
        protected bool UsuarioAutenticado { get; set; }
        protected readonly IUtilDapperRepository _utilDapperRepository;
        protected readonly ILogService _logService;

        protected MainController(INotificador notificador,
                                 IUser appUser,
                                  IConfiguration configuration,
                                  IUtilDapperRepository utilDapperRepository, 
                                  ILogService logService)
        {
            _notificador = notificador;
            AppUser = appUser;
            _configuration = configuration;

            if (appUser.IsAuthenticated())
            {
                UsuarioId = appUser.GetUserId();
                UsuarioAutenticado = true;
            }

            _utilDapperRepository = utilDapperRepository;
            _logService = logService;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        //protected long GerarId()
        //{
        //    Guid guid = Guid.NewGuid();

        //    byte[] _bytes = guid.ToByteArray();
        //    return BitConverter.ToInt64(_bytes, 0);
        //}

        protected async Task<long> GerarId()
        {
            return await _utilDapperRepository.GerarUUID();
        }

        protected async Task<int> GerarIdInt(string generator)
        {
            return await _utilDapperRepository.GerarIdInt(generator);
        }


        protected ActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(result);
            }

            //return BadRequest(new
            //{
            //    success = false,
            //    errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
            //});
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Mensagens", _notificador.ObterNotificacoes().Select(n => n.Mensagem).ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotificarErroModelInvalida(modelState);
            return CustomResponse();
        }

        protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotificarErro(errorMsg);
            }
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
            if(!string.IsNullOrEmpty(msg))
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

        protected void LogInformacao(string msg,string tela, string controle, string sql)
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

        protected async Task<byte[]> ConverterToIFormFile(string fileBase64)
        {
            return Convert.FromBase64String(fileBase64);
        }

        protected async Task<string> ConverterByteToString(byte[] byteArray)
        {
            return Encoding.Default.GetString(byteArray);
        }

        protected async Task<string> ConverterByteToBase64(byte[] byteArray)
        {
            return Convert.ToBase64String(byteArray);
        }

        protected async Task<IFormFile> ConverterToIFormFile(byte[] file, string fileName, string fileNameExtensao)
        {
            var stream = new MemoryStream(file);
            return new FormFile(stream, 0, stream.Length, fileName, fileNameExtensao);
        }

        protected async Task<byte[]> ConverterFormFileToByte(IFormFile formFile)
        {
            long length = formFile.Length;
            if (length < 0)
                return null;

            using var fileStream = formFile.OpenReadStream();
            byte[] bytes = new byte[length];
            fileStream.Read(bytes, 0, (int)formFile.Length);

            return bytes;
        }


        protected async Task<DateTime> FormatarDataConsulta(string data, bool Inicial)
        {
            DateTime _data = Convert.ToDateTime(data);

            return Inicial ? new DateTime(_data.Year, _data.Month, _data.Day, 0, 0, 0) : new DateTime(_data.Year, _data.Month, _data.Day, 23, 59, 59);
        }

        protected string Deserializar(object objeto) => JsonSerializer.Serialize(objeto);
        
    }
}
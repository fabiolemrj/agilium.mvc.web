using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using Microsoft.Extensions.Configuration;

namespace agilium.api.manager.Logger
{
    public class CustomLogger : ILogger
    {
        private readonly string loggerName;
        private readonly CustomerLoggerProviderConfiguration loggerConfig;
        UserResolverService _user = Singleton<UserResolverService>.Instance();
        List<string> lista;
       // private readonly ILogSistemaService _logSistemaService;

        private IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public CustomLogger(string name, CustomerLoggerProviderConfiguration config,
        IWebHostEnvironment hostingEnvironment,
        IConfiguration configuration)
        {
            loggerName = name;
            loggerConfig = config;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;

        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {

            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var user = "Usuario Não Identificado";

            if (!string.IsNullOrEmpty(_user.username))
            {
                user = _user.username;
            }

            if (!formatter(state, exception).ToString().ContainsAny(listaExclusaoStringEntrada(), StringComparison.InvariantCulture))
            {
                string mensagem = string.Format("{0}: {1} - {2} - {3} - {4}",
                 logLevel.ToString(), eventId.Id, user, formatter(state, exception), DateTime.Now.ToString());
                if (mensagem.Contains("refresh"))
                    Thread.Sleep(1000);
                GravarLog(mensagem);


            }


        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter, string user)
        {
            if (!formatter(state, exception).ToString().ContainsAny(listaExclusaoStringEntrada(), StringComparison.InvariantCulture))
            {
                string mensagem = string.Format("{0}: {1} - {2} - {3} - {4}",
                 logLevel.ToString(), eventId.Id, user, formatter(state, exception), DateTime.Now.ToString());
                if (mensagem.Contains("refresh"))
                    Thread.Sleep(1000);
                GravarLog(mensagem);


            }
        }

            private IEnumerable<string> listaExclusaoStringEntrada()
        {
            yield return "Executed";
            yield return "Executing";
            yield return "Request";
            yield return "Route";
            yield return "Successfully";
            yield return "Authorization";
            yield return "No type";
            yield return "Entity Framework";
            yield return "CORS";
            yield return "Now listening on";
            yield return "Application started";
            yield return "Hosting environment";
        }

        private void GravarLog(string mensagem)
        {

            var path = _hostingEnvironment.ContentRootPath;
            var pasta = _configuration["Log:pasta"];
            //path += "\\" + pasta;
            path = pasta;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // string path = "C:\\AppLog\\RiscosPessoais";
            //string nomeArquivo = "log-" + DateTime.Today.ToString("dd-MM-yyyy") + ".txt";

            string nomeArquivo = !string.IsNullOrEmpty(_configuration["Log:nomeArquivo"].ToString()) ? _configuration["Log:nomeArquivo"] : "log-";
            nomeArquivo += DateTime.Today.ToString("dd-MM-yyyy") + ".txt";

            string pathMain = Path.Combine(path, nomeArquivo);

            if (!File.Exists(pathMain))
                File.Create(pathMain).Close();

            //string caminho = @"c:\Users\lnormando\logteste.txt";
            using (var fs = new FileStream(pathMain, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(mensagem);
            }


        }

    }
}

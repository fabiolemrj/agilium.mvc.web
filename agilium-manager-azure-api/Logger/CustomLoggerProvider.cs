using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace agilium.api.manager.Logger
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        readonly CustomerLoggerProviderConfiguration loggerConfig;
        readonly ConcurrentDictionary<string, CustomLogger> loggers = new ConcurrentDictionary<string, CustomLogger>();
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, name => new CustomLogger(name, loggerConfig, _hostingEnvironment, _configuration));
        }

        public CustomLoggerProvider(CustomerLoggerProviderConfiguration config,
                            IWebHostEnvironment hostingEnvironment,
                            IConfiguration configuration)
        {
            loggerConfig = config;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public void Dispose()
        {

        }
    }
}

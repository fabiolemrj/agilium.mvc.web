using Microsoft.Extensions.Logging;

namespace agilium.api.manager.Logger
{
    public class CustomerLoggerProviderConfiguration
    {
        public string username { get; set; }
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;
        public int EventId { get; set; } = 0;
    }
}

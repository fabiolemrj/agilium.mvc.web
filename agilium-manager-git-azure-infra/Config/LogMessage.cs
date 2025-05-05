using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Config
{
    public class LogMessage
    {
        public string Category { get; set; }
        public DateTime DateTime { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
    }
}

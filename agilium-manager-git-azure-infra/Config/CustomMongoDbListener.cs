using KissLog;
using KissLog.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Config
{
    public class CustomMongoDbListener : ILogListener
    {
        public ILogListenerInterceptor Interceptor => throw new NotImplementedException();

        public void OnBeginRequest(HttpRequest httpRequest)
        {
            throw new NotImplementedException();
        }

        public void OnFlush(FlushLogArgs args)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(KissLog.LogMessage message)
        {
            throw new NotImplementedException();
        }
    }
}

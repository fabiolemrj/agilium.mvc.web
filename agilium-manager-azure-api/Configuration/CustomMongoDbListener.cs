using agilium.api.infra.Config;
using KissLog;
using KissLog.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.manager.Configuration
{
    public class CustomMongoDbListener : ILogListener
    {
        private readonly Lazy<IMongoDatabase> _mongoDatabase;
        public ILogListenerInterceptor Interceptor { get; set; }

        public CustomMongoDbListener(string connectionString, string databaseName)
        {
            _mongoDatabase = new Lazy<IMongoDatabase>(() =>
            {
                var mongoClient = new MongoClient(connectionString);
                var mongoDatabase = mongoClient.GetDatabase(databaseName);

                return mongoDatabase;
            });
        }

        public void OnBeginRequest(HttpRequest httpRequest)
        {
            // do nothing
        }

        public void OnFlush(FlushLogArgs args)
        {
            var logMessages = args.MessagesGroups.SelectMany(p => p.Messages).OrderBy(p => p.DateTime).ToList();

            RequestLog requestLog = CreateModel(args.HttpProperties);
            requestLog.Messages = logMessages.Select(p => CreateModel(p)).ToList();

            _mongoDatabase.Value.GetCollection<RequestLog>("RequestLog").InsertOne(requestLog);
        }

        public void OnMessage(KissLog.LogMessage message)
        {
            // do nothing
        }

        private api.infra.Config.LogMessage CreateModel(KissLog.LogMessage logMessage)
        {
            return new api.infra.Config.LogMessage
            {
                Category = logMessage.CategoryName,
                DateTime = logMessage.DateTime,
                LogLevel = logMessage.LogLevel.ToString(),
                Message = logMessage.Message
            };
        }

        private RequestLog CreateModel(HttpProperties httpProperties)
        {
            HttpRequest request = httpProperties.Request;
            HttpResponse response = httpProperties.Response;

            double durationInMs = (response.EndDateTime - request.StartDateTime).TotalMilliseconds;

            return new RequestLog
            {
                DateTime = request.StartDateTime,
                UserAgent = request.UserAgent,
                HttpMethod = request.HttpMethod,
                AbsoluteUri = request.Url.AbsoluteUri,
                RequestHeaders = request.Properties.Headers,
                DurationInMilliseconds = durationInMs,
                StatusCode = response.StatusCode,
                ResponseHeaders = response.Properties.Headers
            };
        }
    }
}

using agilium.api.infra.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Context
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;
        public ConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IMongoClient GetClient()
        {
             return new MongoClient(_connectionString);
        }

        public IMongoDatabase GetDatabase(IMongoClient mongoClient, string databaseName)
        {
            return mongoClient.GetDatabase(databaseName);
        }

        public IMongoDatabase GetDatabase(string databaseName)
        {
            var mongoDbClient = GetClient();

            return mongoDbClient.GetDatabase(databaseName);
        }
    }
}

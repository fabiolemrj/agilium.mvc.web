using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Interfaces
{
    public interface IConnectionFactory
    {
        IMongoClient GetClient();

        IMongoDatabase GetDatabase(IMongoClient mongoClient, string databaseName);

        IMongoDatabase GetDatabase(string databaseName);
    }
}

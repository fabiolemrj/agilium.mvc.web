using agilium.api.business.Models;
using agilium.api.infra.Context;
using agilium.api.infra.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Repository
{
    public class UsuarioFotoRepositoryMongo : RepositoryMongo<UsuarioFoto>
    {
        public UsuarioFotoRepositoryMongo(IMongoCollection<UsuarioFoto> collectionName) : base(collectionName)
        {
        }


        public UsuarioFotoRepositoryMongo(IConnectionFactory connectionFactory, string databaseName, string collectionName)
            : base(connectionFactory, databaseName, collectionName)
        {
        }
    }
}

using agilium.api.business.Interfaces;
using agilium.api.infra.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.infra.Repository
{
    public abstract class RepositoryMongo<T> : IRepositoryMongo<T> where T : class
    {
        private readonly IMongoCollection<T> _collectionName;

        protected RepositoryMongo(IMongoCollection<T> collectionName)
        {
            _collectionName = collectionName;
        }


        protected RepositoryMongo(IConnectionFactory connectionFactory, string databaseName, string collectionName)
        {
            _collectionName = connectionFactory.GetDatabase(databaseName).GetCollection<T>(collectionName);
        }

        public void Insert(T obj)
        {
            _collectionName.InsertOne(obj);
        }

        public void Update(Expression<Func<T, bool>> filter, T obj)
        {            
            _collectionName.ReplaceOne(filter, obj);

        }

        public async Task Replace(Expression<Func<T, string>> filter,string campo)
        {
            var filtro = Builders<T>.Filter.Eq(filter,campo);

            // create a Set operator update definition
            var updateNameDefinition = Builders<T>.Update
                        .Set(filter, campo);

            await _collectionName
            .UpdateOneAsync(filtro,updateNameDefinition);
        }

        public T Query(Expression<Func<T, bool>> filter)
        {
            return _collectionName.AsQueryable<T>().FirstOrDefault(filter);
        }

        public IQueryable<T> QueryAll()
        {
            return _collectionName.AsQueryable<T>();
        }

        public async Task RemoveAsync(Expression<Func<T, bool>> filter) => await _collectionName.DeleteOneAsync(filter);
    }
}

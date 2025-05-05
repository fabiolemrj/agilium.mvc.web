using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using MySqlConnector;
using MySql.Data.MySqlClient;

namespace agilium.api.infra.Repository.Dapper
{
    public sealed class DbSession : IDisposable
    {
        protected readonly IConfiguration _configuration;

        private Guid _id;
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }

        public void Dispose()
        {
            Connection?.Dispose();
        }

        public string GetConnection()
        {
           
            return _configuration.GetSection("ConnectionStrings").GetSection("ConnectionDb").Value;
        }

        public DbSession(IConfiguration configuration)
        {
            _configuration = configuration;

            _id = Guid.NewGuid();
            Connection = new MySqlConnection(GetConnection());
            Connection.Open();
        }
    }
}

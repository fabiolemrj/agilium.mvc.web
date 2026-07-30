using Microsoft.Extensions.Configuration;
using MySqlConnector;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace agilium.api.infra.Repository.Dapper
{
    public sealed class CardapioDigitalDbSession : IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly Guid _id;

        public IDbConnection Connection { get; }

        public CardapioDigitalDbSession(IConfiguration configuration)
        {
            _configuration = configuration;
            _id = Guid.NewGuid();

            var connectionString = _configuration
                .GetSection("CardapioDigital")
                .GetSection("ConnectionString").Value;

            Connection = new MySqlConnection(connectionString);
            Connection.Open();
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}

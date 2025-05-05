using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace agilium.api.infra.Repository.Dapper
{
    public class UtilDapperRepository : IUtilDapperRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly DbSession _dbSession;
        public string GetConnection()
        {
            var autenticacaoUrl = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return autenticacaoUrl;
        }

        public UtilDapperRepository(IConfiguration configuration, DbSession dbSession)
        {
            _configuration = configuration;
            _dbSession = dbSession;
        }

        public async Task<long> GerarUUID()
        {
            return await GerarUUIDPrivate();
            //long resultado = 0;
            //using (var con = new MySqlConnection(GetConnection()))
            //{
            //    try
            //    {
            //        con.Open();
            //        resultado = await GerarUUID(con);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //    finally { con.Close(); }

            //    return resultado;
            //};
        }

        private async Task<long> GerarUUID(MySqlConnection con)
        {
            var query = $@"SELECT uuid_short() AS ID";

            return con.Query<long>(query).FirstOrDefault();
        }

        public async Task<long> GerarUUIDPrivate()
        {
            var query = $@"SELECT uuid_short() AS ID";

            return _dbSession.Connection.Query<long>(query,null,_dbSession.Transaction).FirstOrDefault();
        }

        public async Task<int> GerarIdInt(string generator)
        {
            var query = $@"select GEN_ID(${generator}, 1) from RDB$DATABASE";

            return _dbSession.Connection.Query<int>(query, null, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<string> ConfigRetornaValor(string valor, long? idEmpresa)
        {
            var query = $@"Select valor from config where chave = @chave";
            if (idEmpresa.HasValue && idEmpresa.Value > 0)
                query += $" and IDEMPRESA = @IDEMPRESA";
            else
                query += $" and IDEMPRESA is null";

            var parametros = new DynamicParameters();
            parametros.Add("@chave", valor, DbType.String, ParameterDirection.Input);
            
            if (idEmpresa.HasValue && idEmpresa.Value > 0)
                parametros.Add("@IDEMPRESA", idEmpresa, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Query<string>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<string> GerarCodigo(string sql)
        {
            var resultado =  _dbSession.Connection.Query<string>(sql, null, _dbSession.Transaction).FirstOrDefault();
            int resultadoConvertido = 0;
            Int32.TryParse(resultado, out resultadoConvertido);
            resultadoConvertido += 1;
            return resultadoConvertido.ToString().PadLeft(6,'0');
        }

       
    }
}

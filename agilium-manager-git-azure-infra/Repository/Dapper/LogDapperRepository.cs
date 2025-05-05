using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.infra.Repository.Dapper
{
    public class LogDapperRepository : ILogDapper
    {
        protected readonly IConfiguration _configuration;
        private readonly DbSession _dbSession;
        private readonly IUtilDapperRepository _utilDapperRepository;

        public LogDapperRepository(IConfiguration configuration, DbSession dbSession, IUtilDapperRepository utilDapperRepository)
        {
            _configuration = configuration;
            _dbSession = dbSession;
            _utilDapperRepository = utilDapperRepository;
        }

        #region Log
        public async Task<bool> Adicionar(LogSistema logSistema)
        {
            return await Adicionar(logSistema.usuario, logSistema.descr, logSistema.tela, logSistema.controle, logSistema.maquina, logSistema.SQL_log, logSistema.so);
        }

        public async Task<bool> Adicionar(string usuario, string descr, string tela, string controle, string maquina, string sql_log, string so)
        {
           // var id = _utilDapperRepository.GerarUUID().Result;

            var query = $@"INSERT INTO log_sist (usuario, descr, tela, controle, maquina, data_log, hora_log, SQL_log, so)
                            VALUES(@usuario, @descr, @tela, @controle, @maquina, now(), @hora_log, @SQL_log, @so)";

            var parametros = new DynamicParameters();
            parametros.Add("@usuario", usuario, DbType.String, ParameterDirection.Input);
            //parametros.Add("@id_log", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@descr", descr, DbType.String, ParameterDirection.Input);
            parametros.Add("@tela", tela, DbType.String, ParameterDirection.Input);
            parametros.Add("@controle", controle, DbType.String, ParameterDirection.Input);
            parametros.Add("@maquina", maquina, DbType.String, ParameterDirection.Input);
            parametros.Add("@hora_log", DateTime.Now.ToString("HH:mm:ss"), DbType.String, ParameterDirection.Input);
            parametros.Add("@SQL_log", sql_log, DbType.String, ParameterDirection.Input);
            parametros.Add("@so", so, DbType.String, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        #endregion

        #region Erro
        public async Task<bool> Erro(string usuario, string descr, string tela, string controle, string maquina, string sql_log, string tipo)
        {
            //var id = _utilDapperRepository.GerarUUID().Result;

            var query = $@"INSERT INTO logerro (usuario,erro,Tipo,Tela,Controle,Maquina,Data_erro,Hora_erro,SQL_erro)
                        VALUES (@usuario, @erro, @Tipo, @Tela, @Controle, @Maquina, now(), @Hora_erro, @SQL_erro)";

            var parametros = new DynamicParameters();
            parametros.Add("@usuario", usuario, DbType.String, ParameterDirection.Input);
            parametros.Add("@Tipo", tipo, DbType.String, ParameterDirection.Input);
            parametros.Add("@erro", descr, DbType.String, ParameterDirection.Input);
            parametros.Add("@tela", tela, DbType.String, ParameterDirection.Input);
            parametros.Add("@controle", controle, DbType.String, ParameterDirection.Input);
            parametros.Add("@maquina", maquina, DbType.String, ParameterDirection.Input);
            parametros.Add("@Hora_erro", DateTime.Now.ToString("HH:mm:ss"), DbType.String, ParameterDirection.Input);
            parametros.Add("@SQL_erro", sql_log, DbType.String, ParameterDirection.Input);
            
            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }
        #endregion
    }
}

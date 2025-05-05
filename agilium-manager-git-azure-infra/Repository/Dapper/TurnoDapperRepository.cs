using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using System.Linq;
using System.Data;
using MySqlConnector;
using MySql.Data.MySqlClient;

namespace agilium.api.infra.Repository.Dapper
{
    public class TurnoDapperRepository : IPTurnoDapperRepository
    {
        protected readonly IConfiguration _configuration;

        public TurnoDapperRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnection()
        {
            var autenticacaoUrl = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return autenticacaoUrl;
        }

        #region Metods Publicos
        public async Task<bool> AbrirTurno(long idEmpresa, long idUsuario)
        {
            var resultado = false;
            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();

                        var idTuno = GerarUUID(con).Result;
                        
                        if(!TurnoAberto(idEmpresa,con).Result)
                        {
                            
                            var turno = new Turno(idEmpresa,idUsuario,null,DateTime.Now,GerarNumeroTurno(idEmpresa,con).Result,DateTime.Now,null,null);
                            IncluirTurno(idTuno, turno, con);
                        }
                        resultado = true;
                        scope.Complete();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally { con.Close(); }
                }

                return resultado;
            }
        }

        public async Task<bool> FecharTurno(long idEmpresa, long idUsuario, string obs)
        {
            var resultado = false;
            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();

                        var turno = ObterTurnoAberto(idEmpresa, con).Result;
                        if(turno != null)
                        {
                            turno.AdicionarObservacao(obs);
                            FecharTurno(turno,idUsuario, con);
                        }
                        
                        resultado = true;
                        scope.Complete();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally { con.Close(); }
                }

                return resultado;
            }
        }

        public async Task<bool> TurnoAberto(long id)
        {
            var resultado = false;
            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();                                         

                        resultado = TurnoAberto(id,con).Result;
                        scope.Complete();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally { con.Close(); }
                }

                return resultado;
            }
        }

        private async Task<bool> TurnoAberto(long id, MySqlConnection con)
        {
            var query = $@"SELECT IDTURNO as Id FROM turno WHERE IDEMPRESA = {id}  AND DTHRFIM IS NULL";

            var turno = con.Query<Turno>(query).FirstOrDefault();

            return turno != null;
        }


        public async Task<Turno> ObterTurnoAberto(long idEmpresa)
        {
            Turno turno;
            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();

                        turno = ObterTurnoAberto(idEmpresa, con).Result;
                        scope.Complete();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally { con.Close(); }
                }

                return turno;
            }
        }

        #endregion

        #region privates 
        private async Task<long> GerarUUID(MySqlConnection con)
        {
            var query = $@"SELECT uuid_short() AS ID";

            return con.Query<long>(query).FirstOrDefault();
        }

        private async Task<int> GerarNumeroTurno(long idEmpresa, MySqlConnection con)
        {
            var query = $@"SELECT(COALESCE(MAX(t.NUTURNO), 0) + 1) AS NOVOTURNO FROM turno t WHERE t.IDEMPRESA = {idEmpresa} AND t.DTTURNO = curdate()";


            return con.Query<int>(query).FirstOrDefault();
        }

        private async Task<Turno> ObterTurnoAberto(long idEmpresa, MySqlConnection con)
        {
            var query = $@"SELECT IDTURNO as Id, NUTURNO,DTTURNO,IDUSUARIOINI FROM turno WHERE IDEMPRESA = {idEmpresa} AND DTHRFIM IS NULL";


            return con.Query<Turno>(query).FirstOrDefault();
        }

        private void IncluirTurno(long novoId,Turno turno, MySqlConnection con)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDTURNO", novoId, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDEMPRESA", turno.IDEMPRESA, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDUSUARIOINI", turno.IDUSUARIOINI, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@DTTURNO", turno.DTTURNO, DbType.Date, ParameterDirection.Input);
            parametros.Add("@NUTURNO", turno.NUTURNO, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@DTHRINI", turno.DTHRINI, DbType.DateTime, ParameterDirection.Input);

            var query = $@"INSERT INTO turno(IDTURNO, IDEMPRESA, IDUSUARIOINI, DTTURNO, NUTURNO, DTHRINI)
                            values (@IDTURNO, @IDEMPRESA, @IDUSUARIOINI, @DTTURNO, @NUTURNO, @DTHRINI)";

            con.Execute(query, parametros);
        }

        private void FecharTurno(Turno turno, long idUsuarioFim,MySqlConnection con)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDTURNO", turno.Id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDUSUARIOFIM", idUsuarioFim, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@DSOBS", turno.DSOBS, DbType.String, ParameterDirection.Input);

            var query = $@"UPDATE turno SET IDUSUARIOFIM = @IDUSUARIOFIM, DTHRFIM = now(), DSOBS = @DSOBS WHERE IDTURNO = @IDTURNO ";
            con.Execute(query, parametros);
        }


        #endregion
    }




}

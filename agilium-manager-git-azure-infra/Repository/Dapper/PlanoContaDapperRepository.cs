using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
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
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace agilium.api.infra.Repository.Dapper
{
    public class PlanoContaDapperRepository : IPlanoContaDapperRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly DbSession _dbSession;
        private readonly IUtilDapperRepository _utilDapperRepository;

        public PlanoContaDapperRepository(IConfiguration configuration, IUtilDapperRepository utilDapperRepository, DbSession dbSession)
        {
            _configuration = configuration;
            _utilDapperRepository = utilDapperRepository;
            _dbSession = dbSession;
        }

        public string GetConnection()
        {
            var autenticacaoUrl = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return autenticacaoUrl;
        }

        #region metodos publicos
        public async Task AtualizarSaldoContaESubConta(long IdConta)
        {

            await AtualizarSaldoContaESubContaTransaction(IdConta);
            //using (var scope = new TransactionScope())
            //{
            //    using (var con = new MySqlConnection(GetConnection()))
            //    {
            //        try
            //        {
            //            con.Open();
            //            var queryCdConta = $@"SELECT CDCONTA FROM planoconta WHERE IDCONTA = {IdConta}";
            //            var planoConta = con.Query<PlanoConta>(queryCdConta).FirstOrDefault();

            //            if(planoConta != null && !string.IsNullOrEmpty(planoConta.CDCONTA.Trim()))
            //            {
            //                var idsConta = ObterIdsConta(planoConta.CDCONTA, con);

            //                var query = $@"SELECT coalesce(SUM(CASE WHEN PC.TPCONTA = PCL.TPLANC THEN PCL.VLLANC 
            //                                    ELSE (PCL.VLLANC * -1) END ), 0) AS SALDO 
            //                            FROM planoconta_lanc PCL 
            //                                INNER JOIN planoconta PC on PCL.IDCONTA = PC.IDCONTA 
            //                            WHERE PCL.IDCONTA IN ('{idsConta}')";

            //                var novoSaldo = con.Query<double>(query).FirstOrDefault();

            //                if (ExisteSaldoConta(IdConta, con))
            //                {
            //                    AtualizarPlanoSaldo(novoSaldo, IdConta, con);
            //                }
            //                else
            //                {
            //                    IncluirPlanoSaldo(novoSaldo, IdConta, con);
            //                }
            //            }
            //            scope.Complete();
            //        }
            //        catch (Exception ex)
            //        {
            //            throw;
            //        }
            //        finally { con.Close(); }

            //    };

            //}
        }

        public async Task<List<PlanoContaLancamento>> ObterLancamentosPorPlanoEData(long idPlano, DateTime dtInicial, DateTime dtFinal)
        {
            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {                 
                   
                    try
                    {
                        con.Open();
                        var queryCdConta = $@"SELECT IDLANC as Id,IDCONTA,DTCAD,DTREF,NUANOMESREF,DSLANC,VLLANC,TPLANC,STLANC FROM planoconta_lanc 
                                            WHERE IDCONTA = @IDCONTA and DTCAD between @DTINI and @DTFIM 
                                            order by DTCAD";
                        var parametros = new DynamicParameters();
                        parametros.Add("@DTINI", dtInicial, DbType.Date, ParameterDirection.Input);
                        parametros.Add("@DTFIM", dtFinal, DbType.Date, ParameterDirection.Input);
                        parametros.Add("@IDCONTA", idPlano, DbType.Int64, ParameterDirection.Input);

                        var lancamentos = con.Query<PlanoContaLancamento>(queryCdConta, parametros).ToList();
                        
                        return lancamentos;
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                    finally { con.Close(); }
                }
            }
        }

        public async Task<string> ObterDescricaoPlano(long idPlano)
        {
            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();
                        var queryCdConta = $@"SELECT IDCONTA as Id, DSCONTA FROM planoconta
                                            where IDCONTA = {idPlano}";
                        
                        var lancamento = con.Query<PlanoConta>(queryCdConta).FirstOrDefault();

                        return lancamento.DSCONTA;
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                    finally { con.Close(); }
                }
            }
        }

        public async Task RealizarLancamento(PlanoContaLancamento planoContaLancamento)
        {
            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally { con.Close(); }
                }
            }
        }

        #endregion

        #region metodos Privados
        private void AtualizarPlanoSaldo(double saldo, long idConta, MySqlConnection con) 
        {
            var query = $@"UPDATE planoconta_saldo 
                           SET VLSALDO = @VLSALDO, NUANOMESREF = @NUANOMESREF, DTHRATU = now() 
                           WHERE IDCONTA = @IDCONTA ";

            var anoMesReferencia = DateTime.Now.ToString("yyyyMM");
            var parametros = new DynamicParameters();
            parametros.Add("@VLSALDO", saldo, DbType.Double, ParameterDirection.Input);
            parametros.Add("@NUANOMESREF", anoMesReferencia, DbType.String, ParameterDirection.Input);
            parametros.Add("@IDCONTA", idConta, DbType.Int64, ParameterDirection.Input);

            con.Execute(query, parametros);
        }

        private void IncluirPlanoSaldo(double saldo, long idConta, MySqlConnection con)
        {
            var query = $@"INSERT INTO planoconta_saldo(IDSALDO, IDCONTA, DTHRATU, NUANOMESREF, VLSALDO)
                                    VALUES(uuid_short(),@IDCONTA,now(), @NUANOMESREF, @VLSALDO)";

            var anoMesReferencia = DateTime.Now.ToString("yyyyMM");
            var parametros = new DynamicParameters();
            parametros.Add("@VLSALDO", saldo, DbType.Double, ParameterDirection.Input);
            parametros.Add("@NUANOMESREF", anoMesReferencia, DbType.String, ParameterDirection.Input);
            parametros.Add("@IDCONTA", idConta, DbType.Int64, ParameterDirection.Input);

            con.Execute(query, parametros);
        }

        private bool ExisteSaldoConta(long IdConta, MySqlConnection con)
        {
            var query = $@"SELECT COUNT(*) AS TOTAL FROM planoconta_saldo WHERE IDCONTA = {IdConta}";
            var resultado =  con.Query<int>(query).FirstOrDefault();
            return resultado > 0;
        }

        private string ObterIdsConta(string cdConta, MySqlConnection con)
        {
            var queryIBPT = $@"SELECT IDCONTA as Id FROM planoconta WHERE CDCONTA LIKE '{cdConta}%'";
            var planosContas = con.Query<PlanoConta>(queryIBPT);
            var resultado = string.Empty;
            planosContas.ToList().ForEach(x => {
                if (string.IsNullOrEmpty(resultado.Trim())) resultado += x.Id.ToString();
                else resultado += "," + x.Id.ToString();
            });

            return resultado;
        }

        public async Task<long> RealizarLancamento(long idConta, DateTime DataRef, string Descricao, double valorLancamento, ETipoContaLancacmento tipoContaLancacmento)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
          
            var parametros = new DynamicParameters();
            parametros.Add("@IDLANC", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDCONTA", idConta, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@DTCAD", DateTime.Now, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@DTREF", DataRef, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NUANOMESREF", DataRef.ToString("yyyyMM"), DbType.String, ParameterDirection.Input);
            parametros.Add("@DSLANC", Descricao, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@VLLANC", valorLancamento, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@TPLANC", tipoContaLancacmento, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@STLANC", 1, DbType.Int32, ParameterDirection.Input);

            var query = $@"INSERT INTO planoconta_lanc (IDLANC, IDCONTA, DTCAD, DTREF, NUANOMESREF,DSLANC, VLLANC, TPLANC, STLANC) Values
                (@IDLANC, @IDCONTA, @DTCAD, @DTREF, @NUANOMESREF,@DSLANC, @VLLANC, @TPLANC, @STLANC)";
            var result = _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;

           return result ? id: -1;
        }

        public async Task<long> ObterContaPrimeiroNivel(long idConta)
        {
            long idContaTemp = idConta;
            while (idContaTemp > 0)
            {
                var queryIBPT = $@"SELECT IDCONTAPAI FROM planoconta WHERE IDCONTA = {idContaTemp}";
                idContaTemp = _dbSession.Connection.Query<long>(queryIBPT, null, _dbSession.Transaction).FirstOrDefault();

            }

            return idContaTemp;         
        }

        #endregion

        #region Transaction
        private void AtualizarPlanoSaldo(double saldo, long idConta)
        {
            var query = $@"UPDATE planoconta_saldo 
                           SET VLSALDO = @VLSALDO, NUANOMESREF = @NUANOMESREF, DTHRATU = now() 
                           WHERE IDCONTA = @IDCONTA ";

            var anoMesReferencia = DateTime.Now.ToString("yyyyMM");
            var parametros = new DynamicParameters();
            parametros.Add("@VLSALDO", saldo, DbType.Double, ParameterDirection.Input);
            parametros.Add("@NUANOMESREF", anoMesReferencia, DbType.String, ParameterDirection.Input);
            parametros.Add("@IDCONTA", idConta, DbType.Int64, ParameterDirection.Input);

            _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction);
        }

        private void IncluirPlanoSaldo(double saldo, long idConta)
        {
            var query = $@"INSERT INTO planoconta_saldo(IDSALDO, IDCONTA, DTHRATU, NUANOMESREF, VLSALDO)
                                    VALUES(uuid_short(),@IDCONTA,now(), @NUANOMESREF, @VLSALDO)";

            var anoMesReferencia = DateTime.Now.ToString("yyyyMM");
            var parametros = new DynamicParameters();
            parametros.Add("@VLSALDO", saldo, DbType.Double, ParameterDirection.Input);
            parametros.Add("@NUANOMESREF", anoMesReferencia, DbType.String, ParameterDirection.Input);
            parametros.Add("@IDCONTA", idConta, DbType.Int64, ParameterDirection.Input);

            _dbSession.Connection.Execute(query, parametros);
        }

        private bool ExisteSaldoConta(long IdConta)
        {
            var query = $@"SELECT COUNT(*) AS TOTAL FROM planoconta_saldo WHERE IDCONTA = {IdConta}";
            var resultado = _dbSession.Connection.Query<int>(query).FirstOrDefault();
            return resultado > 0;
        }

        private string ObterIdsConta(string cdConta)
        {
            var queryIBPT = $@"SELECT IDCONTA as Id FROM planoconta WHERE CDCONTA LIKE '{cdConta}%'";
            var planosContas = _dbSession.Connection.Query<PlanoConta>(queryIBPT);
            
            var resultado = string.Empty;
            planosContas.ToList().ForEach(x => {
                if (string.IsNullOrEmpty(resultado.Trim())) resultado += x.Id.ToString();
                else resultado += "," + x.Id.ToString();
            });

            return resultado;
        }

        private async Task AtualizarSaldoContaESubContaTransaction(long IdConta)
        {
            var queryCdConta = $@"SELECT CDCONTA FROM planoconta WHERE IDCONTA = {IdConta}";
            var planoConta = _dbSession.Connection.Query<PlanoConta>(queryCdConta).FirstOrDefault();

            if (planoConta != null && !string.IsNullOrEmpty(planoConta.CDCONTA.Trim()))
            {
                var idsConta = ObterIdsConta(planoConta.CDCONTA);

                var query = $@"SELECT coalesce(SUM(CASE WHEN PC.TPCONTA = PCL.TPLANC THEN PCL.VLLANC 
                                                ELSE (PCL.VLLANC * -1) END ), 0) AS SALDO 
                                        FROM planoconta_lanc PCL 
                                            INNER JOIN planoconta PC on PCL.IDCONTA = PC.IDCONTA 
                                        WHERE PCL.IDCONTA IN ('{idsConta}')";

                var novoSaldo = _dbSession.Connection.Query<double>(query).FirstOrDefault();

                if (ExisteSaldoConta(IdConta))
                {
                    AtualizarPlanoSaldo(novoSaldo, IdConta);
                }
                else
                {
                    IncluirPlanoSaldo(novoSaldo, IdConta);
                }
            }
        }

        public async Task<bool> ExcluirLancamento(long idLanc)
        {
            var query = $@"DELETE FROM planoconta_lanc WHERE IDLANC = {idLanc}";

            return _dbSession.Connection.Execute(query) > 0;
        }

        #endregion
    }
}

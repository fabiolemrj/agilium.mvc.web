using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using Microsoft.Extensions.Configuration;

using System;
using Dapper;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Linq;
using System.Data;
using MySqlConnector;
using MySql.Data.MySqlClient;
using agilium.api.business.Interfaces;

namespace agilium.api.infra.Repository.Dapper
{
    public class ContaReceberRepositoryDapper : IPContaReceberDapperRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IPlanoContaDapperRepository _planoContaDapperRepository;
        private readonly DbSession _dbSession;
        private readonly IUtilDapperRepository _utilDapperRepository;
        public ContaReceberRepositoryDapper(IConfiguration configuration, IPlanoContaDapperRepository planoContaDapperRepository, DbSession dbSession, IUtilDapperRepository utilDapperRepository)
        {
            _configuration = configuration;
            _planoContaDapperRepository = planoContaDapperRepository;
            _dbSession = dbSession;
            _utilDapperRepository = utilDapperRepository;
        }
        public string GetConnection()
        {
            var autenticacaoUrl = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return autenticacaoUrl;
        }


        #region Publicas
        public async Task<bool> ConsolidarConta(long id)
        {
            var resultado = false;
            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();
                        var queryCdConta = $@"select IDCONTAREC as Id, IDCONTA,IDLANC, DESCR, IDCONTAPAI,IDCATEG_FINANC, VLCONTA,VLDESC,VLACRES  from contas_receber where IDCONTAREC ={id}";
                        var contaPagar = con.Query<ContaReceber>(queryCdConta).FirstOrDefault();
                        if (contaPagar != null)
                        {
                            var ID = GerarUUID(con).Result;
                            var lancamento = new PlanoContaLancamento(contaPagar.IDCONTA, DateTime.Now, DateTime.Now, Int32.Parse(DateTime.Now.ToString("yyyyMM")),
                                contaPagar.DESCR, (contaPagar.VLCONTA.Value - contaPagar.VLDESC + contaPagar.VLACRES),
                                business.Enums.ETipoContaLancacmento.Credito, business.Enums.EAtivo.Ativo);

                            RealizarLancamento(ID, lancamento, con);

                            AtualizarConsolidacaoContaPagar(ID, id, con);

                            if (contaPagar.IDCONTA > 0)
                                await _planoContaDapperRepository.AtualizarSaldoContaESubConta(contaPagar.IDCONTA.Value);

                        }

                        scope.Complete();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally { con.Close(); }
                }
            }

            return resultado;
        }

        public async Task<bool> DesconsolidarConta(long id)
        {
            var resultado = false;
            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();
                        var queryCdConta = $@"select IDCONTAREC as Id, IDCONTA,IDLANC, DESCR, IDCONTAPAI,IDCATEG_FINANC, VLCONTA,VLDESC,VLACRES  from contas_receber where IDCONTAREC ={id}";
                        var contaPagar = con.Query<ContaReceber>(queryCdConta).FirstOrDefault();
                        if (contaPagar != null)
                        {
                            var lancamento = ObterPlanoContaLancamentoPorId(contaPagar.IDLANC.Value, con);
                            if (lancamento != null)
                            {
                                var anoMesLanc = Convert.ToInt32(lancamento.DTCAD.Value.ToString("yyyyMM"));
                                var anoMesAtual = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));

                                AtualizarDesconsolidacaoContaPagarPorId(contaPagar.Id, con);


                                if (anoMesLanc <= anoMesAtual)
                                {
                                    ApagarLancamentoPorId(contaPagar.IDLANC.Value, con);
                                }
                                else
                                {
                                    AtualizarLancamentoPorId(contaPagar.IDLANC.Value, con);
                                }

                                if (contaPagar.IDCONTA > 0)
                                {
                                    await _planoContaDapperRepository.AtualizarSaldoContaESubConta(contaPagar.IDCONTA.Value);
                                }
                            }
                        }

                        scope.Complete();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally { con.Close(); }
                }
            }

            return resultado;
        }

        public async Task<PlanoContaLancamento> CriarContaLancamentoDeContaReceber(long id)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDCONTAPAG", id, DbType.Int64, ParameterDirection.Input);

            var queryCdConta = $@"select IDCONTAREC as Id, IDCONTA,IDLANC, DESCR, IDCONTAPAI,IDCATEG_FINANC, VLCONTA,VLDESC,VLACRES  from contas_receber where IDCONTAREC =@IDCONTAPAG";
            var contaPagar = _dbSession.Connection.Query<ContaReceber>(queryCdConta, parametros, _dbSession.Transaction).FirstOrDefault();
            if (contaPagar != null)
            {
                var lancamento = new PlanoContaLancamento(contaPagar.IDCONTA, DateTime.Now, DateTime.Now, Int32.Parse(DateTime.Now.ToString("yyyyMM")),
                                contaPagar.DESCR, (contaPagar.VLCONTA.Value - contaPagar.VLDESC + contaPagar.VLACRES),
                                business.Enums.ETipoContaLancacmento.Credito, business.Enums.EAtivo.Ativo);

                return lancamento;
            }
            else
                return null;
        }

        public async Task<bool> RealizarLancamento(long idLanc, PlanoContaLancamento planoContaLancamento)
        {
            var query = $@"INSERT INTO planoconta_lanc (IDLANC, IDCONTA, DTCAD, DTREF, NUANOMESREF, DSLANC, VLLANC, TPLANC, STLANC)  
                            values(@IDLANC, @IDCONTA, now(), @DTREF, @NUANOMESREF, @DSLANC, @VLLANC, @TPLANC, @STLANC) ";

            var anoMesReferencia = DateTime.Now.ToString("yyyyMM");
            var parametros = new DynamicParameters();
            parametros.Add("@IDLANC", idLanc, DbType.Int64, ParameterDirection.Input);

            parametros.Add("@VLLANC", planoContaLancamento.VLLANC, DbType.Double, ParameterDirection.Input);
            parametros.Add("@IDCONTA", planoContaLancamento.IDCONTA, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NUANOMESREF", planoContaLancamento.NUANOMESREF, DbType.String, ParameterDirection.Input);
            parametros.Add("@DTREF", planoContaLancamento.DTREF, DbType.Date, ParameterDirection.Input);
            parametros.Add("@DSLANC", planoContaLancamento.DSLANC, DbType.String, ParameterDirection.Input);
            parametros.Add("@TPLANC", planoContaLancamento.TPLANC, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@STLANC", planoContaLancamento.STLANC, DbType.Int32, ParameterDirection.Input);

            return _dbSession.Connection.ExecuteAsync(query, parametros, _dbSession.Transaction).Result > 0;
        }

        public async Task<bool> AtualizarConsolidacaoContaReceber(long idLanc, long idConta)
        {
            var query = $@"update contas_receber set dtpag = now(), stconta = 2, IDLANC = {idLanc}  where IDCONTAREC ={idConta}";

            return _dbSession.Connection.ExecuteAsync(query, null, _dbSession.Transaction).Result > 0;
        }

        public async Task<PlanoContaLancamento> ObterPlanoContaLancamentoPorId(long idLanc)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDLANC", idLanc, DbType.Int64, ParameterDirection.Input);

            var query = $@"SELECT IDLANC as Id ,IDCONTA,DTCAD,DTREF,NUANOMESREF,DSLANC,VLLANC,TPLANC,STLANC FROM planoconta_lanc WHERE IDLANC =@IDLANC"; ;

            return _dbSession.Connection.Query<PlanoContaLancamento>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<bool> AtualizarDesconsolidacaoContaReceberPorId(long idLanc)
        {
            var query = $@"update contas_receber set dtpag = null, stconta = 1, IDLANC = null where IDCONTAREC ={idLanc}";
            
            return _dbSession.Connection.Execute(query, null, _dbSession.Transaction) > 0;
        }

        public async Task<bool> ApagarLancamentoPorId(long idLanc)
        {
            var query = $@"delete FROM planoconta_lanc WHERE IDLANC ={idLanc}";

            return _dbSession.Connection.ExecuteAsync(query, null, _dbSession.Transaction).Result > 0;
        }

        public async Task<bool> AtualizarLancamentoPorId(long idLanc)
        {
            var query = $@"update planoconta_lanc set stlanc = 0 WHERE IDLANC={idLanc}";

            return _dbSession.Connection.ExecuteAsync(query, null, _dbSession.Transaction).Result > 0;
        }
        #endregion

        #region Privates
        private async Task<long> GerarUUID(MySqlConnection con)
        {
            var query = $@"SELECT uuid_short() AS ID";

            return con.Query<long>(query).FirstOrDefault();
        }

        private PlanoContaLancamento ObterPlanoContaLancamentoPorId(long idLanc, MySqlConnection con)
        {
            var query = $@"SELECT IDLANC as Id ,IDCONTA,DTCAD,DTREF,NUANOMESREF,DSLANC,VLLANC,TPLANC,STLANC FROM planoconta_lanc WHERE IDLANC ={idLanc}"; ;

            return con.Query<PlanoContaLancamento>(query).FirstOrDefault();
        }

        private void ApagarLancamentoPorId(long idLanc, MySqlConnection con)
        {
            var query = $@"delete FROM planoconta_lanc WHERE IDLANC ={idLanc}";

            con.Execute(query);
        }

        private void AtualizarLancamentoPorId(long idLanc, MySqlConnection con)
        {
            var query = $@"update planoconta_lanc set stlanc = 0 WHERE IDLANC={idLanc}";

            con.Execute(query);
        }

        private void AtualizarDesconsolidacaoContaPagarPorId(long idLanc, MySqlConnection con)
        {
            var query = $@"update contas_receber set dtpag = null, stconta = 1, IDLANC = null where IDCONTAREC ={idLanc}";

            con.Execute(query);
        }


        private void AtualizarConsolidacaoContaPagar(long idLanc, long idConta, MySqlConnection con)
        {
            var query = $@"update contas_receber set dtpag = now(), stconta = 2, IDLANC = {idLanc}  where IDCONTAREC ={idConta}";

            con.Execute(query);
        }

        private void RealizarLancamento(long idLanc, PlanoContaLancamento planoContaLancamento, MySqlConnection con)
        {
            var query = $@"INSERT INTO planoconta_lanc (IDLANC, IDCONTA, DTCAD, DTREF, NUANOMESREF, DSLANC, VLLANC, TPLANC, STLANC)  
                            values(@IDLANC, @IDCONTA, now(), @DTREF, @NUANOMESREF, @DSLANC, @VLLANC, @TPLANC, @STLANC) ";

            var anoMesReferencia = DateTime.Now.ToString("yyyyMM");
            var parametros = new DynamicParameters();
            parametros.Add("@IDLANC", idLanc, DbType.Int64, ParameterDirection.Input);

            parametros.Add("@VLLANC", planoContaLancamento.VLLANC, DbType.Double, ParameterDirection.Input);
            parametros.Add("@IDCONTA", planoContaLancamento.IDCONTA, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NUANOMESREF", planoContaLancamento.NUANOMESREF, DbType.String, ParameterDirection.Input);
            parametros.Add("@DTREF", planoContaLancamento.DTREF, DbType.Date, ParameterDirection.Input);
            parametros.Add("@DSLANC", planoContaLancamento.DSLANC, DbType.String, ParameterDirection.Input);
            parametros.Add("@TPLANC", planoContaLancamento.TPLANC, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@STLANC", planoContaLancamento.STLANC, DbType.Int32, ParameterDirection.Input);

            con.Execute(query, parametros);
        }
        #endregion
    }
}

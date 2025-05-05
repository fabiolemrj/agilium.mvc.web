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

namespace agilium.api.infra.Repository.Dapper
{
    public class ContaPagarDapperRepository : IPContaPagarDapperRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IPlanoContaDapperRepository _planoContaDapperRepository;

        public ContaPagarDapperRepository(IConfiguration configuration, IPlanoContaDapperRepository planoContaDapperRepository)
        {
            _configuration = configuration;
            _planoContaDapperRepository = planoContaDapperRepository;
        }

        public string GetConnection()
        {
            var autenticacaoUrl = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return autenticacaoUrl;
        }

        #region metodos publicos
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
                        var queryCdConta = $@"select IDCONTAPAG as Id, IDCONTA,IDLANC, DESCR, IDCONTAPAG,VLCONTA,VLDESC,VLACRESC  from contas_pagar where IDCONTAPAG ={id}";
                        var contaPagar = con.Query<ContaPagar>(queryCdConta).FirstOrDefault();
                        if(contaPagar != null)
                        {
                            var ID = GerarUUID(con).Result;
                            var lancamento = new PlanoContaLancamento(contaPagar.IDCONTA,DateTime.Now, DateTime.Now, Int32.Parse(DateTime.Now.ToString("yyyyMM")),
                                contaPagar.DESCR,(contaPagar.VLCONTA.Value - contaPagar.VLDESC + contaPagar.VLACRESC),
                                business.Enums.ETipoContaLancacmento.Debito,business.Enums.EAtivo.Ativo);
                            
                            RealizarLancamento(ID,lancamento, con);
                            
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
                        var queryCdConta = $@"select IDCONTAPAG as Id, IDCONTA,IDLANC, DESCR, IDCONTAPAG  from contas_pagar where IDCONTAPAG ={id}";
                        var contaPagar = con.Query<ContaPagar>(queryCdConta).FirstOrDefault();
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
                            
                                if(contaPagar.IDCONTA > 0)
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
            var query = $@"update contas_pagar set dtpag = null, stconta = 1, IDLANC = null where IDCONTAPAG ={idLanc}";

            con.Execute(query);
        }

        private void AtualizarConsolidacaoContaPagar(long idLanc, long idConta, MySqlConnection con)
        {
            var query = $@"update contas_pagar set dtpag = now(), stconta = 2, IDLANC = {idLanc}  where IDCONTAPAG ={idConta}";

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

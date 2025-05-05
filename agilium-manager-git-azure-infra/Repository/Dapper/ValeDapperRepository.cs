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
    public class ValeDapperRepository : IValeDapperRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IDapperRepository _dapperRepository;
        private readonly IUtilDapperRepository _utilDapperRepository;
        private readonly DbSession _dbSession;

        public ValeDapperRepository(IConfiguration configuration,IDapperRepository dapperRepository, IUtilDapperRepository utilDapperRepository, DbSession dbSession)
        {
            _configuration = configuration;
            _dapperRepository = dapperRepository;
            _utilDapperRepository = utilDapperRepository;
            _dbSession = dbSession;
        }
        public string GetConnection()
        {
            var autenticacaoUrl = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return autenticacaoUrl;
        }

        public async Task<string> GerarCodigo(long idEmpresa)
        {
            var resultado = "";
            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();
                        resultado = await GerarCodigo(con, idEmpresa);
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

        public async Task<long> GerarValeAtualizaDevolucao(long idDevolucao)
        {
            long resultado = 0;
            
            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();
                        var devolucao = await ObterDevolucaoPorId(con, idDevolucao);
                        if(devolucao != null)
                        {
                            var codigoVale = await GerarCodigo(con, devolucao.IDEMPRESA.Value);
                            var codigoBarra = await GerarCodigoBarra(con);
                            var idVale = await GerarUUID(con);


                            var parametros = new DynamicParameters();
                            parametros.Add("@IDVALE", idVale, DbType.Int64, ParameterDirection.Input);
                            parametros.Add("@IDEMPRESA", devolucao.IDEMPRESA.HasValue ?devolucao.IDEMPRESA: null, DbType.Int64, ParameterDirection.Input);
                            parametros.Add("@IDCLIENTE", devolucao.IDCLIENTE.HasValue?devolucao.IDCLIENTE:null, DbType.Int64, ParameterDirection.Input);
                            parametros.Add("@CDVALE", codigoVale, DbType.Date, ParameterDirection.Input);
                            parametros.Add("@TPVALE", ETipoVale.Troca, DbType.Int32, ParameterDirection.Input);
                            parametros.Add("@DTHRVALE", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
                            parametros.Add("@VLVALE", devolucao.VLTOTALDEV, DbType.Double, ParameterDirection.Input);
                            parametros.Add("@STVALE", ESituacaoVale.Ativo, DbType.Int32, ParameterDirection.Input);
                            parametros.Add("@CDBARRA", codigoBarra, DbType.String, ParameterDirection.Input);

                            var query = $@"INSERT INTO vale (IDVALE,IDEMPRESA, IDCLIENTE, CDVALE, DTHRVALE, TPVALE, STVALE, VLVALE, CDBARRA)
                            values (@IDVALE,@IDEMPRESA, @IDCLIENTE, @CDVALE, @DTHRVALE, @TPVALE, @STVALE, @VLVALE, @CDBARRA)";

                            con.Execute(query, parametros);

                            await AtualizarDevolucaoComVale(con, idDevolucao, idVale);
                            resultado = idVale;
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

        public async Task<string> GerarCodigoBarra()
        {

            var resultado = "";
            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();
                        resultado = await GerarCodigoBarra(con);

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

        public async Task<bool> AtualizarDevolucaoComVale(long idDevolucao, long idvale)
        {
            var resultado = false;
            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();

                        var parametros = new DynamicParameters();
                        parametros.Add("@IDVALE", idvale, DbType.Int64, ParameterDirection.Input);
                        parametros.Add("@IDDEV", idDevolucao, DbType.Int64, ParameterDirection.Input);

                        var query = $@"UPDATE devolucao SET IDVALE = @IDVALE WHERE IDDEV = @IDDEV";

                        resultado = con.Execute(query, parametros) > 0 ;

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

        public async Task<bool> UtilizarValePorVenda(long idVenda)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDVENDA", idVenda, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@STVALE", ESituacaoVale.Utilizado, DbType.Int32, ParameterDirection.Input);

            var query = $@"UPDATE vale SET STVALE = @STVALE WHERE IDVALE in (SELECT COALESCE(IDVALE, 0) AS IDVALE FROM venda_moeda WHERE IDVENDA = @IDVENDA)";
            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        #region private

        private async Task<string> GerarCodigoBarra(MySqlConnection con)
        {
            var cdBarraUnico = false;
            var quant = 0;
            var numeroRandomico = 0;
            var resultado = "";

            Random valor = new Random();
            do
            {
                numeroRandomico = valor.Next(9999) + 1;
                quant++;

                resultado = DateTime.Now.ToString("yyMMdd") + numeroRandomico.ToString().PadLeft(4, '0');

                var parametros = new DynamicParameters();
                parametros.Add("@CDBARRA", resultado, DbType.String, ParameterDirection.Input);

                var query = $@"SELECT COUNT(*) AS TOTAL FROM vale WHERE CDBARRA =@CDBARRA";
                cdBarraUnico = con.Query<int>(query, parametros).FirstOrDefault() == 0;
            } while (!cdBarraUnico || quant > 100);

            if (!cdBarraUnico && quant > 100)
            {
                resultado = "";
            }

            return resultado;
        }

        private async Task<string> GerarCodigo(MySqlConnection con, long idEmpresa)
        {
            var query = $@"SELECT MAX(CAST(CDVALE AS UNSIGNED)) AS CD FROM vale where IDEMPRESA = {idEmpresa}";
            var codigo = con.Query<int>(query).FirstOrDefault();
            codigo++;
            return codigo.ToString().PadLeft(6, '0');
        }

        private async Task<long> GerarUUID(MySqlConnection con)
        {
            var query = $@"SELECT uuid_short() AS ID";

            return con.Query<long>(query).FirstOrDefault();
        }

        private async Task<bool> AtualizarDevolucaoComVale(MySqlConnection con,long idDevolucao, long idvale)
        {
            var resultado = false;
            var parametros = new DynamicParameters();
            parametros.Add("@IDVALE", idvale, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDDEV", idDevolucao, DbType.Int64, ParameterDirection.Input);

            var query = $@"UPDATE devolucao SET IDVALE = @IDVALE WHERE IDDEV = @IDDEV";

            resultado = con.Execute(query, parametros) > 0;

            return resultado;
        }

        private async Task<Devolucao> ObterDevolucaoPorId(MySqlConnection con, long id)
        {
            var query = $"SELECT IDDEV as Id, IDEMPRESA, IDVENDA, IDCLIENTE, IDMOTDEV, IDVALE,CDDEV, VLTOTALDEV, STDEV from devolucao where IDDEV =  {id}";
            var devolucao = con.Query<Devolucao>(query).FirstOrDefault();
            return devolucao;
        }

        #endregion
    }
}

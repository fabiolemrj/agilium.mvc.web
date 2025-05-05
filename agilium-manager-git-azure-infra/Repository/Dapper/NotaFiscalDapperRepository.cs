using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using Microsoft.Extensions.Configuration;

using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Linq;
using System.Data;
using agilium.api.business.Enums;
using MySqlConnector;
using MySql.Data.MySqlClient;

namespace agilium.api.infra.Repository.Dapper
{
    public class NotaFiscalDapperRepository : IPNotaFiscalDapperRepository
    {
        protected readonly IConfiguration _configuration;

        public string GetConnection()
        {
            var autenticacaoUrl = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return autenticacaoUrl;
        }

        public NotaFiscalDapperRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> EnviarNotaFiscalInutil(long id)
        {
            var resultado = false;
            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();
                        var queryCdConta = $@"select IDNFINUTIL as Id, IDEMPRESA,CDNFINUTIL,DSMOTIVO,NUANO,DSMODELO,DSSERIE,NUNFINI,DTHRINUTIL,STINUTIL,DSPROTOCOLO,DSXML 
                                                from nf_inutil where IDNFINUTIL ={id}";
                        var nFInutil = con.Query<NotaFiscalInutil>(queryCdConta).FirstOrDefault();
                       if(nFInutil != null)
                        {
                            AtualizarNotaFiscalInutil(nFInutil.CDNFINUTIL,nFInutil.Id.ToString()+nFInutil.DSMODELO,ESituacaoNFInutil.EnviadoSefaz,DateTime.Now,nFInutil.Id,con);
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

        private void AtualizarNotaFiscalInutil(string dsprotoclo, string dsXML,ESituacaoNFInutil stinutil, DateTime dt,long id, MySqlConnection con)
        {

            var parametros = new DynamicParameters();
            parametros.Add("@DTHRINUTIL", dt, DbType.Date, ParameterDirection.Input);
            parametros.Add("@DSPROTOCOLO", dsprotoclo, DbType.String, ParameterDirection.Input);
            parametros.Add("@DSXML", dsXML, DbType.String, ParameterDirection.Input);
            parametros.Add("@STINUTIL", stinutil, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@IDNFINUTIL", id, DbType.Int64, ParameterDirection.Input);

            var query = $@"UPDATE nf_inutil SET DSPROTOCOLO = @DSPROTOCOLO, DSXML = @DSXML, DTHRINUTIL = @DTHRINUTIL,STINUTIL = @STINUTIL WHERE IDNFINUTIL = @IDNFINUTIL";

            con.Execute(query,parametros);
        }

    }
}

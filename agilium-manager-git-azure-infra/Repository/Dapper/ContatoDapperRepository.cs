using agilium.api.business.Enums;
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
    public class ContatoDapperRepository : IContatoDapperRepository
    {
        private readonly IUtilDapperRepository _utilDapperRepository;
        protected readonly IConfiguration _configuration;
        private readonly DbSession _dbSession;

        public ContatoDapperRepository(IUtilDapperRepository utilDapperRepository, IConfiguration configuration, DbSession dbSession)
        {
            _utilDapperRepository = utilDapperRepository;
            _configuration = configuration;
            _dbSession = dbSession;
        }

        public string GetConnection()
        {
            var autenticacaoUrl = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return autenticacaoUrl;
        }

        public async Task<Contato> AdicionarContato(ETipoContato tipoContato, string descricao1, string descricao2, long idFornecedor)
        {
            return await AdicionarContatoTransacao(tipoContato, descricao1, descricao2, idFornecedor);
            //using (var con = new MySqlConnection(GetConnection()))
            //{
            //    try
            //    {
            //        var id = _utilDapperRepository.GerarUUID().Result;
            //        con.Open();

            //        var query = $@"INSERT INTO contato (IDCONTATO, TPCONTATO, DESCR1, DESCR2, DTHRATU)
            //                        VALUES (@IDCONTATO, @TPCONTATO, @DESCR1, @DESCR2, @DTHRATU) ";

            //        var contato = new Contato(tipoContato,descricao1,descricao2);
            //        contato.Id = id;

            //        var parametros = new DynamicParameters();
            //        parametros.Add("@IDCONTATO", contato.Id, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@TPCONTATO", contato.TPCONTATO, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@DESCR1", contato.DESCR1, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@DESCR2", contato.DESCR2, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@DTHRATU", DateTime.Now, DbType.DateTime, ParameterDirection.Input);

            //        if (con.Execute(query, parametros) > 0)
            //            return contato;
            //        else return null;

            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //    finally { con.Close(); }
            //}
        }

        public async Task<bool> AdicionarContatoFornecedor(long idContato, long idFornecedor)
        {
            return await AdicionarContatoFornecedorTransacao(idContato, idFornecedor);
            //using (var con = new MySqlConnection(GetConnection()))
            //{
            //    try
            //    {
            //        var id = _utilDapperRepository.GerarUUID().Result;
            //        con.Open();

            //        var query = $@"INSERT INTO forn_contato (IDCONTATO,IDFORN) VALUES (@IDCONTATO,@IDFORN) ";

            //        var parametros = new DynamicParameters();
            //        parametros.Add("@IDCONTATO", idContato, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@IDFORN", idFornecedor, DbType.Int64, ParameterDirection.Input);
                    
            //        return con.Execute(query, parametros) > 0;

            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //    finally { con.Close(); }
            //}
        }

        #region Transcao
        private async Task<Contato> AdicionarContatoTransacao(ETipoContato tipoContato, string descricao1, string descricao2, long idFornecedor)
        {
                    var id = _utilDapperRepository.GerarUUID().Result;
            
                    var query = $@"INSERT INTO contato (IDCONTATO, TPCONTATO, DESCR1, DESCR2, DTHRATU)
                                    VALUES (@IDCONTATO, @TPCONTATO, @DESCR1, @DESCR2, @DTHRATU) ";

                    var contato = new Contato(tipoContato, descricao1, descricao2);
                    contato.Id = id;

                    var parametros = new DynamicParameters();
                    parametros.Add("@IDCONTATO", contato.Id, DbType.Int64, ParameterDirection.Input);
                    parametros.Add("@TPCONTATO", contato.TPCONTATO, DbType.String, ParameterDirection.Input);
                    parametros.Add("@DESCR1", contato.DESCR1, DbType.String, ParameterDirection.Input);
                    parametros.Add("@DESCR2", contato.DESCR2, DbType.String, ParameterDirection.Input);
                    parametros.Add("@DTHRATU", DateTime.Now, DbType.DateTime, ParameterDirection.Input);

                    if (_dbSession.Connection.Execute(query, parametros,_dbSession.Transaction) > 0)
                        return contato;
                    else return null;

        }

        private async Task<bool> AdicionarContatoFornecedorTransacao(long idContato, long idFornecedor)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            
            var query = $@"INSERT INTO forn_contato (IDCONTATO,IDFORN) VALUES (@IDCONTATO,@IDFORN) ";

            var parametros = new DynamicParameters();
            parametros.Add("@IDCONTATO", idContato, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDFORN", idFornecedor, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros,_dbSession.Transaction) > 0;

                
        }
        #endregion
    }
}

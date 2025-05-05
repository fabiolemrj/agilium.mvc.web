using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using Dapper;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.infra.Repository.Dapper
{
    public class EnderecoDapperRepository : IEnderecoDapperRepository
    {
        private readonly IUtilDapperRepository _utilDapperRepository;
        protected readonly IConfiguration _configuration;
        private readonly DbSession _dbSession;


        public EnderecoDapperRepository(IUtilDapperRepository utilDapperRepository, IConfiguration configuration, DbSession dbSession)
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


        public Task<Endereco> AdicionarEndereco(string logradouro, string numero, string complemento, string bairro, string cep, string cidade, string uf, string pais)
        {
            throw new NotImplementedException();
        }

        public async Task<Endereco> AdicionarEndereco(Endereco endereco)
        {
            return await AdicionarEnderecoTransacao(endereco);
            //using (var con = new MySqlConnection(GetConnection()))
            //{
            //    try
            //    {
            //        var id = _utilDapperRepository.GerarUUID().Result;
            //        con.Open();

            //        var query = $@"INSERT INTO endereco (IDENDERECO, ENDER, NUM, COMPL, BAIRRO, CEP, CIDADE, UF, PAIS, IBGE, DTHRATU) 
            //                        VALUES (@IDENDERECO, @ENDER, @NUM, @COMPL, @BAIRRO, @CEP, @CIDADE, @UF, @PAIS, @IBGE, @DTHRATU) ";

            //        endereco.Id = id;

            //        var parametros = new DynamicParameters();
            //        parametros.Add("@IDENDERECO", endereco.Id, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@ENDER", endereco.Logradouro, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@NUM", endereco.Numero, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@BAIRRO", endereco.Bairro, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@CEP", endereco.Cep, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@CIDADE", endereco.Cidade, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@UF", endereco.Uf, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@PAIS", endereco.Pais, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@IBGE", endereco.Ibge, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@DTHRATU", DateTime.Now, DbType.DateTime, ParameterDirection.Input);

            //        con.Execute(query, parametros);

            //        return endereco;
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //    finally { con.Close(); }
            //}
        }

        public async Task<Cep> ObterEnderecoPorCep(string cep)
        {
            return await ObterEnderecoPorCepTranscacao(cep);
            //using (var con = new MySqlConnection(GetConnection()))
            //{
            //    try
            //    {
            //        con.Open();
            //        var query = $@"select cep, ender, bairro, cidade, uf, IBGE from ceps where cep = @cep";
            //        var parametros = new DynamicParameters();
            //        parametros.Add("@cep", cep, DbType.String, ParameterDirection.Input);
            //        return con.Query<Cep>(query,parametros).FirstOrDefault();
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //    finally { con.Close(); }
            //}
        }

        #region transacao
        private async Task<Endereco> AdicionarEnderecoTransacao(Endereco endereco)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            
            var query = $@"INSERT INTO endereco (IDENDERECO, ENDER, NUM,  BAIRRO, CEP, CIDADE, UF, PAIS, IBGE, DTHRATU) 
                            VALUES (@IDENDERECO, @ENDER, @NUM, @BAIRRO, @CEP, @CIDADE, @UF, @PAIS, @IBGE, @DTHRATU) ";

            endereco.Id = id;

            var parametros = new DynamicParameters();
            parametros.Add("@IDENDERECO", endereco.Id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@ENDER", endereco.Logradouro, DbType.String, ParameterDirection.Input);
            parametros.Add("@NUM", endereco.Numero, DbType.String, ParameterDirection.Input);
            parametros.Add("@BAIRRO", endereco.Bairro, DbType.String, ParameterDirection.Input);
            parametros.Add("@CEP", endereco.Cep, DbType.String, ParameterDirection.Input);
            parametros.Add("@CIDADE", endereco.Cidade, DbType.String, ParameterDirection.Input);
            parametros.Add("@UF", endereco.Uf, DbType.String, ParameterDirection.Input);
            parametros.Add("@PAIS", endereco.Pais, DbType.String, ParameterDirection.Input);
            parametros.Add("@IBGE", endereco.Ibge, DbType.String, ParameterDirection.Input);
            parametros.Add("@DTHRATU", DateTime.Now, DbType.DateTime, ParameterDirection.Input);

            _dbSession.Connection.Execute(query, parametros,_dbSession.Transaction);
            return endereco;
        }

        private async Task<Cep> ObterEnderecoPorCepTranscacao(string cep)
        {
            var query = $@"select cep as Numero, ender as Endereco, bairro, cidade, uf, IBGE from ceps where cep = @cep";
            var parametros = new DynamicParameters();
            parametros.Add("@cep", cep, DbType.String, ParameterDirection.Input);
            return _dbSession.Connection.Query<Cep>(query, parametros,_dbSession.Transaction).FirstOrDefault();
            }
        }
        #endregion
    
}

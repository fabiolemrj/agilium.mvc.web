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

namespace agilium.api.infra.Repository.Dapper
{
    public class FornecedorDapperRepository : IFornecedorDapperRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IUtilDapperRepository _utilDapperRepository;
        private readonly IContatoDapperRepository _contatoDapperRepository;
        private readonly IEnderecoDapperRepository _enderecoDapperRepository;
        private readonly DbSession _dbSession;

        public FornecedorDapperRepository(IConfiguration configuration, IUtilDapperRepository utilDapperRepository, 
            IContatoDapperRepository contatoDapperRepository, IEnderecoDapperRepository enderecoDapperRepository,DbSession dbSession)
        {
            _configuration = configuration;
            _utilDapperRepository = utilDapperRepository;
            _contatoDapperRepository = contatoDapperRepository;
            _enderecoDapperRepository = enderecoDapperRepository;
            _dbSession = dbSession;
        }

        public string GetConnection()
        {
            var autenticacaoUrl = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return autenticacaoUrl;
        }

        public async Task<Fornecedor> AdicionarFornecedor(string razaoSocial, string nomeFantasia, ETipoPessoa tipoPessoa, string cnpj, string inscricaoEstadual, ETipoFiscal tipoFiscal,Endereco endereco)
        {
            return await AdicionarFornecedorTransacao(razaoSocial, nomeFantasia, tipoPessoa, cnpj, inscricaoEstadual,tipoFiscal,endereco);
            //using (var con = new MySqlConnection(GetConnection()))
            //{
            //    try
            //    {
            //        var id = _utilDapperRepository.GerarUUID().Result;
            //        con.Open();
            //        await _enderecoDapperRepository.AdicionarEndereco(endereco);

            //        var codigo = GerarCodigo(con).Result;
            //        var fornecedor = new Fornecedor(endereco.Id,codigo,razaoSocial,nomeFantasia,tipoPessoa.ToString(),cnpj,inscricaoEstadual,tipoFiscal,(int)EAtivo.Ativo);
            //        var query = $@"INSERT INTO fornecedor (IDFORN,IDENDERECO,CDFORN,NMRZSOCIAL,NMFANTASIA,TPPESSOA,NUCPFCNPJ,DSINSCR,TPFISCAL,STFORNEC)
            //                        VALUES (@IDFORN,@IDENDERECO,@CDFORN,@NMRZSOCIAL,@NMFANTASIA,@TPPESSOA,@NUCPFCNPJ,@DSINSCR,@TPFISCAL,@STFORNEC)";

            //        fornecedor.Id = id;
                    
            //        var parametros = new DynamicParameters();
            //        parametros.Add("@IDFORN", fornecedor.Id, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@IDENDERECO", fornecedor.IDENDERECO, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@CDFORN", fornecedor.CDFORN, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@NMRZSOCIAL", fornecedor.NMRZSOCIAL, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@NMFANTASIA", fornecedor.NMFANTASIA, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@TPPESSOA", fornecedor.TPPESSOA, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@NUCPFCNPJ", fornecedor.NUCPFCNPJ, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@TPFISCAL", fornecedor.TPFISCAL, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@STFORNEC", fornecedor.STFORNEC, DbType.String, ParameterDirection.Input);

            //        con.Execute(query,parametros);

            //        return fornecedor;
            //    }
            //    catch (Exception)
            //    {
             
            //        throw;
            //    }
            //    finally { con.Close(); }
            //}
        }

        public async Task<Fornecedor> ObterFornecedorPorCNPJ(string cnpj)
        {
            return await ObterFornecedorPorCNPJTransacao(cnpj);
            //using (var con = new MySqlConnection(GetConnection()))
            //{
            //    try
            //    {
            //        con.Open();
            //        var parametros = new DynamicParameters();
            //        parametros.Add("@NUCPFCNPJ", cnpj, DbType.String, ParameterDirection.Input);

            //        var query = $@"SELECT fornecedor.IDFORN as Id, fornecedor.* FROM fornecedor WHERE NUCPFCNPJ = @NUCPFCNPJ";

            //        return con.Query<Fornecedor>(query, parametros).FirstOrDefault();
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //    finally { con.Close(); }
            //}               
          
        }

        private async Task<string> GerarCodigo(MySqlConnection con)
        {
            var query = $@"SELECT MAX(CAST(CDFORN AS UNSIGNED)) AS CD FROM fornecedor";

            return con.Query<string>(query).FirstOrDefault();
        }

        #region transacao
        private async Task<string> GerarCodigo()
        {
            var query = $@"SELECT MAX(CAST(CDFORN AS UNSIGNED)) AS CD FROM fornecedor";

            return _dbSession.Connection.Query<string>(query,null,_dbSession.Transaction).FirstOrDefault();
        }

        private async Task<Fornecedor> AdicionarFornecedorTransacao(string razaoSocial, string nomeFantasia, ETipoPessoa tipoPessoa, string cnpj, string inscricaoEstadual, ETipoFiscal tipoFiscal, Endereco endereco)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            await _enderecoDapperRepository.AdicionarEndereco(endereco);

            var codigo = GerarCodigo().Result;
            var fornecedor = new Fornecedor(endereco.Id, codigo, razaoSocial, nomeFantasia, tipoPessoa.ToString(), cnpj, inscricaoEstadual, tipoFiscal, (int)EAtivo.Ativo);
            var query = $@"INSERT INTO fornecedor (IDFORN,IDENDERECO,CDFORN,NMRZSOCIAL,NMFANTASIA,TPPESSOA,NUCPFCNPJ,DSINSCR,TPFISCAL,STFORNEC)
                            VALUES (@IDFORN,@IDENDERECO,@CDFORN,@NMRZSOCIAL,@NMFANTASIA,@TPPESSOA,@NUCPFCNPJ,@DSINSCR,@TPFISCAL,@STFORNEC)";

            fornecedor.Id = id;

            var parametros = new DynamicParameters();
            parametros.Add("@IDFORN", fornecedor.Id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDENDERECO", fornecedor.IDENDERECO, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@CDFORN", fornecedor.CDFORN, DbType.String, ParameterDirection.Input);
            parametros.Add("@NMRZSOCIAL", fornecedor.NMRZSOCIAL, DbType.String, ParameterDirection.Input);
            parametros.Add("@NMFANTASIA", fornecedor.NMFANTASIA, DbType.String, ParameterDirection.Input);
            parametros.Add("@TPPESSOA", fornecedor.TPPESSOA, DbType.String, ParameterDirection.Input);
            parametros.Add("@NUCPFCNPJ", fornecedor.NUCPFCNPJ, DbType.String, ParameterDirection.Input);
            parametros.Add("@TPFISCAL", fornecedor.TPFISCAL, DbType.String, ParameterDirection.Input);
            parametros.Add("@STFORNEC", fornecedor.STFORNEC, DbType.String, ParameterDirection.Input);
            parametros.Add("@DSINSCR", fornecedor.DSINSCR, DbType.String, ParameterDirection.Input);

            _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction);

            return fornecedor;
                     
        }

        private async Task<Fornecedor> ObterFornecedorPorCNPJTransacao(string cnpj)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@NUCPFCNPJ", cnpj, DbType.String, ParameterDirection.Input);

            var query = $@"SELECT fornecedor.IDFORN as Id, fornecedor.* FROM fornecedor WHERE NUCPFCNPJ = @NUCPFCNPJ";

            return _dbSession.Connection.Query<Fornecedor>(query, parametros, _dbSession.Transaction).FirstOrDefault();
            
            }

        

        #endregion
    }
}

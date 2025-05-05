using agilium.api.business.Enums;
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
    public class ClienteDapperRepository : IClienteDapperRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IDapperRepository _dapperRepository;
        private readonly IUtilDapperRepository _utilDapperRepository;
        private readonly DbSession _dbSession;

        public ClienteDapperRepository(IConfiguration configuration, IDapperRepository dapperRepository, IUtilDapperRepository utilDapperRepository, DbSession dbSession)
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


        public async Task<long> AdicionarClienteBasico(Cliente cliente)
        {
            var idEndereco = await AdicionarEndereco(cliente.Endereco);

            if(idEndereco > 0)
            {
                var id = _utilDapperRepository.GerarUUID().Result;
                var codigo = _utilDapperRepository.GerarCodigo("SELECT MAX(CAST(CDCLIENTE AS UNSIGNED)) AS CD FROM cliente").Result;
                var tipoPessoaCliente = cliente.TPCLIENTE == ETipoPessoa.F ? "F" : "J";
                var parametros = new DynamicParameters();
                parametros.Add("@IDCLIENTE", id, DbType.Int64, ParameterDirection.Input);
                parametros.Add("@CDCLIENTE", codigo, DbType.String, ParameterDirection.Input);
                parametros.Add("@NMCLIENTE", cliente.NMCLIENTE, DbType.String, ParameterDirection.Input);
                parametros.Add("@TPCLIENTE", tipoPessoaCliente, DbType.String, ParameterDirection.Input);
                parametros.Add("@DTCAD", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
                parametros.Add("@IDENDERECO", idEndereco, DbType.Int64, ParameterDirection.Input);
                parametros.Add("@STCLIENTE", EAtivo.Ativo, DbType.Int32, ParameterDirection.Input);
                parametros.Add("@STPUBEMAIL", ESimNao.Nao, DbType.Int32, ParameterDirection.Input);
                parametros.Add("@STPUBSMS", ESimNao.Nao, DbType.Int32, ParameterDirection.Input);

                var query = $@"INSERT INTO cliente (IDCLIENTE,CDCLIENTE,NMCLIENTE,TPCLIENTE,DTCAD,IDENDERECO,STCLIENTE,STPUBEMAIL,STPUBSMS)
                               values (@IDCLIENTE,@CDCLIENTE,@NMCLIENTE,@TPCLIENTE,@DTCAD,@IDENDERECO,@STCLIENTE,@STPUBEMAIL,@STPUBSMS)";

                if (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0)
                    return id;
                else
                    return 0;
            }

            return 0;           
        }

        private async Task<long> AdicionarEndereco(Endereco endereco)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            
            var parametros = new DynamicParameters();
            parametros.Add("@IDENDERECO", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@ENDER", endereco.Logradouro, DbType.String, ParameterDirection.Input);
            parametros.Add("@NUM", endereco.Numero, DbType.String, ParameterDirection.Input);
            parametros.Add("@COMPL",endereco.Complemento, DbType.String, ParameterDirection.Input);
            parametros.Add("@DTHRATU", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@BAIRRO", endereco.Bairro, DbType.String, ParameterDirection.Input);
            parametros.Add("@CEP", endereco.Cep, DbType.String, ParameterDirection.Input);
            parametros.Add("@CIDADE", endereco.Cidade, DbType.String, ParameterDirection.Input);
            parametros.Add("@UF", endereco.Uf, DbType.String, ParameterDirection.Input);
            parametros.Add("@PAIS", "Brasil", DbType.String, ParameterDirection.Input);
            parametros.Add("@DSPTREF", endereco.PontoReferencia, DbType.String, ParameterDirection.Input);

            var query = $@"INSERT INTO endereco (IDENDERECO,ENDER,NUM,COMPL,BAIRRO,CEP,CIDADE,UF,PAIS,DSPTREF,DTHRATU)
                        values (@IDENDERECO,@ENDER,@NUM,@COMPL,@BAIRRO,@CEP,@CIDADE,@UF,@PAIS,@DSPTREF,@DTHRATU)";

            if (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0)
                return id;
            else
                return 0;
        }

        public async Task<bool> AdicionarClientePF(ClientePF cliente)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDCLIENTE", cliente.Id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NUCPF", cliente.NUCPF, DbType.String, ParameterDirection.Input);

            var query = $@"INSERT INTO clientepf (IDCLIENTE,NUCPF) values (@IDCLIENTE,@NUCPF) ";

            return (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0);
        }

        public async Task<Cliente> ObterClientePorId(long id)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDCLIENTE", id, DbType.Int64, ParameterDirection.Input);

            var query = $@"SELECT c.IDCLIENTE as Id, c.* FROM cliente c where c.IDCLIENTE = @IDCLIENTE";

            return _dbSession.Connection.Query<Cliente>(query, parametros).FirstOrDefault();
        }

        public async Task<Endereco> ObterEnderecoPorId(long id)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDENDERECO", id, DbType.Int64, ParameterDirection.Input);

            var query = $@"SELECT e.IDENDERECO as Id, e.ender as logradouro, e.DSPTREF as PontoReferencia, e.compl as Complemento,
                            e.num as Numero, e.* FROM endereco e where e.IDENDERECO = @IDENDERECO";

            return _dbSession.Connection.Query<Endereco>(query, parametros).FirstOrDefault();
        }

        public async Task<Cliente> ObterClienteComEnderecoPorId(long id)
        {
            if(_dbSession.Connection == null)
                _dbSession.Connection.Open();
                    
                    var cliente = await ObterClientePorId(id);
            if(cliente != null)
            {
                var endereco = await ObterEnderecoPorId(cliente.IDENDERECO.HasValue ? cliente.IDENDERECO.Value : 0);
                if(endereco != null)
                {
                    cliente.AdicionarEndereco(endereco);                    
                }
                return cliente;
            }
            return null;

        }

        public async Task<Cliente> ObterClientePorCpf(string cpf)
        {
            var parametros = new DynamicParameters();
            var query = $@"SELECT c.IDCLIENTE as Id, c.* FROM cliente c inner join clientepf cf on c.IDCLIENTE = cf.IDCLIENTE where cf.NUCPF like @cpf";
            parametros.Add("@cpf", cpf, DbType.String, ParameterDirection.Input);
            var cliente = _dbSession.Connection.Query<Cliente>(query, parametros).FirstOrDefault();
            
            if (cliente != null)
            {
                var endereco = await ObterEnderecoPorId(cliente.IDENDERECO.HasValue ? cliente.IDENDERECO.Value : 0);
                if (endereco != null)
                {
                    cliente.AdicionarEndereco(endereco);
                }
                return cliente;
            }

            return null;
        }
    }
}

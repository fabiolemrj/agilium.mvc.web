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
        private readonly IEnderecoDapperRepository _enderecoDapperRepository;
        private readonly DbSession _dbSession;

        public ClienteDapperRepository(IConfiguration configuration, IDapperRepository dapperRepository, 
            IUtilDapperRepository utilDapperRepository, DbSession dbSession, IEnderecoDapperRepository enderecoDapperRepository)
        {
            _configuration = configuration;
            _dapperRepository = dapperRepository;
            _utilDapperRepository = utilDapperRepository;
            _dbSession = dbSession;
            _enderecoDapperRepository = enderecoDapperRepository;
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
            if (endereco.Id <= 0)
                endereco.Id = _utilDapperRepository.GerarUUID().Result;
           
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

            var query = $@"INSERT INTO endereco (IDENDERECO, ENDER, NUM,  BAIRRO, CEP, CIDADE, UF, PAIS, IBGE, DTHRATU) 
                            VALUES (@IDENDERECO, @ENDER, @NUM, @BAIRRO, @CEP, @CIDADE, @UF, @PAIS, @IBGE, @DTHRATU) ";

            if (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0)
            {
                return endereco.Id;
            }
                
            else
                return 0;
        }

        private async Task<long> AlterarEndereco(Endereco endereco)
        {

            var parametros = new DynamicParameters();
            parametros.Add("@IDENDERECO", endereco.Id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@ENDER", endereco.Logradouro, DbType.String, ParameterDirection.Input);
            parametros.Add("@NUM", endereco.Numero, DbType.String, ParameterDirection.Input);
            parametros.Add("@COMPL", endereco.Complemento, DbType.String, ParameterDirection.Input);
            parametros.Add("@DTHRATU", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@BAIRRO", endereco.Bairro, DbType.String, ParameterDirection.Input);
            parametros.Add("@CEP", endereco.Cep, DbType.String, ParameterDirection.Input);
            parametros.Add("@CIDADE", endereco.Cidade, DbType.String, ParameterDirection.Input);
            parametros.Add("@UF", endereco.Uf, DbType.String, ParameterDirection.Input);
            parametros.Add("@PAIS", "Brasil", DbType.String, ParameterDirection.Input);
            parametros.Add("@DSPTREF", endereco.PontoReferencia, DbType.String, ParameterDirection.Input);

            var query = $@"UPDATE endereco SET ENDER = @ENDER, NUM = @NUM, COMPL = @COMPL,BAIRRO = @BAIRRO, CEP = @CEP,
        CIDADE = @CIDADE,  UF = @UF,PAIS = @PAIS, DSPTREF = @DSPTREF, DTHRATU = @DTHRATU WHERE IDENDERECO = @IDENDERECO";

            if (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0)
                return endereco.Id;
            else
                return 0;
        }

        public async Task<bool> AdicionarClienteDapper(Cliente cliente)
        {
            if (cliente.IDENDERECO != null && cliente.Endereco != null)
            {
                var endereco = await ObterEnderecoPorId(cliente.IDENDERECO.HasValue ? cliente.IDENDERECO.Value : 0);
                if (endereco != null)
                {
                    await _enderecoDapperRepository.AtualizarEnderecoTransacao(cliente.Endereco);
                    // await AlterarEndereco(cliente.Endereco);
                }
                else
                {
                    var endereco_novo = await _enderecoDapperRepository.AdicionarEndereco(cliente.Endereco);
                    cliente.AdicionarEndereco(endereco_novo);
                    cliente.AdicionarEndereco(endereco_novo.Id);

                }

            }

            if (cliente.IDENDERECOCOB != null && cliente.EnderecoCobranca != null)
            {
                var endereco = await ObterEnderecoPorId(cliente.IDENDERECOCOB.HasValue ? cliente.IDENDERECOCOB.Value : 0);
                if (endereco != null)
                {
                    await _enderecoDapperRepository.AtualizarEnderecoTransacao(cliente.EnderecoCobranca);
                }
                else
                {
                    var endereco_novo = await _enderecoDapperRepository.AdicionarEndereco(cliente.EnderecoCobranca);
                    cliente.AdicionarEnderecoCobranca(endereco_novo);
                    cliente.AdicionarEnderecoCobranca(endereco_novo.Id);
            
                }
                    
            }

            if (cliente.IDENDERECOFAT != null && cliente.EnderecoFaturamento != null)
            {
                var endereco = await ObterEnderecoPorId(cliente.IDENDERECOFAT.HasValue ? cliente.IDENDERECOFAT.Value : 0);

                if (endereco != null)
                {
                    await _enderecoDapperRepository.AtualizarEnderecoTransacao(cliente.EnderecoFaturamento);
                }
                else
                {
                    var endereco_novo = await _enderecoDapperRepository.AdicionarEndereco(cliente.EnderecoFaturamento);
                    cliente.AdicionarEnderecoFaturamento(endereco_novo);
                    cliente.AdicionarEnderecoFaturamento(endereco_novo.Id);
                }
            }

            if (cliente.IDENDERECONTREGA != null && cliente.EnderecoEntrega != null)
            {
                var endereco = await ObterEnderecoPorId(cliente.IDENDERECONTREGA.HasValue ? cliente.IDENDERECONTREGA.Value : 0);
                if (endereco != null)
                {
                    await _enderecoDapperRepository.AtualizarEnderecoTransacao(cliente.EnderecoEntrega);
                }
                else
                {
                    var endereco_novo = await _enderecoDapperRepository.AdicionarEndereco(cliente.EnderecoEntrega);
                    cliente.AdicionarEnderecoEntrega(endereco_novo);
                    cliente.AdicionarEnderecoEntrega(endereco_novo.Id);
                }
            }

            var parametros = new DynamicParameters();

            parametros.Add("@IDCLIENTE", cliente.Id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@CDCLIENTE", cliente.CDCLIENTE, DbType.String, ParameterDirection.Input);
            parametros.Add("@NMCLIENTE", cliente.NMCLIENTE, DbType.String, ParameterDirection.Input);
            parametros.Add("@TPCLIENTE", cliente.TPCLIENTE, DbType.String, ParameterDirection.Input);
            parametros.Add("@DTCAD", DateTime.Now, DbType.DateTime, ParameterDirection.Input);

            // FKs para endereços (podem ser nulos se não tiver)
            parametros.Add("@IDENDERECO", cliente.IDENDERECO, DbType.Int64, ParameterDirection.Input);
           
            parametros.Add("@IDENDERECOCOB", cliente.IDENDERECOCOB, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDENDERECOFAT", cliente.IDENDERECOFAT, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDENDERECOENTREGA", cliente.IDENDERECONTREGA, DbType.Int64, ParameterDirection.Input);

            parametros.Add("@STCLIENTE", cliente.STCLIENTE, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@STPUBEMAIL", cliente.STPUBEMAIL, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@STPUBSMS", cliente.STPUBSMS, DbType.Int32, ParameterDirection.Input);

            var query = $@"UPDATE cliente SET CDCLIENTE = @CDCLIENTE, NMCLIENTE = @NMCLIENTE, TPCLIENTE = @TPCLIENTE, DTCAD = @DTCAD,
        IDENDERECO = @IDENDERECO, IDENDERECOCOB = @IDENDERECOCOB, IDENDERECOFAT = @IDENDERECOFAT, IDENDERECOENTREGA = @IDENDERECOENTREGA,
        STCLIENTE = @STCLIENTE, STPUBEMAIL = @STPUBEMAIL, STPUBSMS = @STPUBSMS  WHERE IDCLIENTE = @IDCLIENTE";

            try
            {
                if (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0)
                {
                  

                    if (cliente.ClientesPFs != null && cliente.TPCLIENTE == ETipoPessoa.F)
                    {
                        var clientePF = new ClientePF(cliente.ClientesPFs.NUCPF, cliente.ClientesPFs.NURG, cliente.ClientesPFs.DTNASC);
                        clientePF.AdicionarIdCliente(cliente.Id);
                        await AdicionarClientePFDapper(clientePF);
                    }
                    else if (cliente.ClientesPJs != null && cliente.TPCLIENTE == ETipoPessoa.J)
                    {
                        var clientePJ = new ClientePJ(cliente.ClientesPJs.NMRZSOCIAL, cliente.ClientesPJs.NUCNPJ, cliente.ClientesPJs.DSINSCREST);
                        clientePJ.AdicionarIdCliente(cliente.Id);
                        await AdicionarClientePJDapper(clientePJ);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }                
        }
        public async Task<bool> AdicionarClientePF(ClientePF cliente)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDCLIENTE", cliente.Id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NUCPF", cliente.NUCPF, DbType.String, ParameterDirection.Input);

            var query = $@"INSERT INTO clientepf (IDCLIENTE,NUCPF) values (@IDCLIENTE,@NUCPF) ";

            return (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0);
        }

        public async Task<bool> AdicionarClientePFDapper(ClientePF clientePf)
        {
            var query = $@"UPDATE clientepf SET NUCPF = @NUCPF, NURG = @NURG, DTNASC = @DTNASC WHERE IDCLIENTE = @IDCLIENTE";

            var parametros = new DynamicParameters();
            parametros.Add("@IDCLIENTE", clientePf.Id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NUCPF", clientePf.NUCPF, DbType.String, ParameterDirection.Input);
            parametros.Add("@NURG", clientePf.NURG, DbType.String, ParameterDirection.Input);
            parametros.Add("@DTNASC", clientePf.DTNASC, DbType.Date, ParameterDirection.Input);

            var resultado = (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0);
            return resultado;
        }

        public async Task<bool> AdicionarClientePJDapper(ClientePJ clientePJ)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDCLIENTE", clientePJ.Id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NMRZSOCIAL", clientePJ.NMRZSOCIAL, DbType.String, ParameterDirection.Input);
            parametros.Add("@NUCNPJ", clientePJ.NUCNPJ, DbType.String, ParameterDirection.Input);
            parametros.Add("@DSINSCREST", clientePJ.DSINSCREST, DbType.String, ParameterDirection.Input);

            var query = $@"UPDATE clientepj SET NMRZSOCIAL = @NMRZSOCIAL, NUCNPJ = @NUCNPJ, DSINSCREST = @DSINSCREST WHERE IDCLIENTE = @IDCLIENTE";

            var resultado = (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0);
            return resultado;
        }

        public async Task<Cliente> ObterClientePorId(long id)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDCLIENTE", id, DbType.Int64, ParameterDirection.Input);

            var query = $@"SELECT c.IDCLIENTE as Id, c.* FROM cliente c where c.IDCLIENTE = @IDCLIENTE";

            return _dbSession.Connection.Query<Cliente>(query, parametros,_dbSession.Transaction).FirstOrDefault();
        }

        public async Task<Endereco> ObterEnderecoPorId(long id)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDENDERECO", id, DbType.Int64, ParameterDirection.Input);

            var query = $@"SELECT e.IDENDERECO as Id, e.ender as logradouro, e.DSPTREF as PontoReferencia, e.compl as Complemento,
                            e.num as Numero, e.* FROM endereco e where e.IDENDERECO = @IDENDERECO";

            return _dbSession.Connection.Query<Endereco>(query, parametros,_dbSession.Transaction).FirstOrDefault();
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

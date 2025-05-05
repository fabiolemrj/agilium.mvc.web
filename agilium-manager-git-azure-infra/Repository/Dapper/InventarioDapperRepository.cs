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
    public class InventarioDapperRepository: IInventarioDapperRepository
    {
        private readonly IUtilDapperRepository _utilDapperRepository;
        protected readonly IConfiguration _configuration;
        private readonly DbSession _dbSession;

        public InventarioDapperRepository(IUtilDapperRepository utilDapperRepository, IConfiguration configuration, DbSession dbSession)
        {
            _utilDapperRepository = utilDapperRepository;
            _configuration = configuration;
            _dbSession = dbSession;
        }

        public async Task<List<EstoqueProduto>> ObterProdutosPorEstoque(long idEstoque)
        {
            var query = $@"select ep.IDESTOQUE_PROD as Id,ep.* from estoque_prod ep 
                            inner join produto p on p.IDPRODUTO = ep.IDPRODUTO 
                            where ep.IDESTOQUE = {idEstoque}";

            return _dbSession.Connection.Query<EstoqueProduto>(query, null, _dbSession.Transaction).ToList();
        }

        public async Task<bool> PodeIncluirProdutoEstoque(long idEstoque, long idProduto)
        {
            var query = $@"select count(*) from inventario_item where idproduto = {idProduto} and idinvent = {idEstoque}";

            return _dbSession.Connection.Query<int>(query, null, _dbSession.Transaction).FirstOrDefault() == 0;
        }

        public async Task<bool> IncluirProdutoItemEstoque(long idInventario, long idProduto)
        {

            var id = _utilDapperRepository.GerarUUID().Result;

            var query = $@"INSERT INTO inventario_item (IDINVENTITEM,IDINVENT,IDPRODUTO) values (@IDINVENTITEM,@IDINVENT,@IDPRODUTO)";

            var parametros = new DynamicParameters();
            parametros.Add("@IDINVENT", idInventario, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDINVENTITEM", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPRODUTO", idProduto, DbType.Int64, ParameterDirection.Input);
            
            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;

        }

        public async Task<List<Produto>> ObterProdutosDisponiveisParaInventarioPorIdInventario(long idInventario, long idEmpresa)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDINVENT", idInventario, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@idempresa", idEmpresa, DbType.Int64, ParameterDirection.Input);


            var query = $@"select distinct p.IDPRODUTO as Id,p.* from produto p where p.STPRODUTO = 1
                         and p.idempresa=@idempresa AND p.IDPRODUTO NOT IN  (SELECT I.IDPRODUTO FROM inventario_item I WHERE I.IDINVENT =@IDINVENT)";

            return _dbSession.Connection.Query<Produto>(query, parametros, _dbSession.Transaction).ToList();
        }

        public async Task<bool> EditarInventarioItem(long idInventario, double quantidadeApurada, long idUsuarioAnalise)
        {
            var query = $@"update inventario_item set DTHRANALISE = now(), NUQTDANALISE = @NUQTDANALISE, IDUSUARIOANALISE = @IDUSUARIOANALISE WHERE IDINVENTITEM = @IDINVENTITEM";

            var parametros = new DynamicParameters();
            parametros.Add("@IDINVENTITEM", idInventario, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDUSUARIOANALISE ", idUsuarioAnalise, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NUQTDANALISE", quantidadeApurada, DbType.Double, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        public async Task<bool> ApagarInventarioItem(long id)
        {
            var query = $@"delete from inventario_item where IDINVENTITEM = @IDINVENTITEM";

            var parametros = new DynamicParameters();
            parametros.Add("@IDINVENTITEM", id, DbType.Int64, ParameterDirection.Input);
            
            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        public async Task<bool> ExisteItemNaoInventariado(long id)
        {
            var query = $@"select distinct count(*) from inventario_item where IDINVENT = {id} and DTHRANALISE is null";

            return _dbSession.Connection.Query<int>(query, null, _dbSession.Transaction).FirstOrDefault() > 0;
        }

        public async Task<List<InventarioItem>> ObterInventarioItemPorIdInventario(long id)
        {
            var query = $@"select distinct it.IDINVENTITEM as Id, it.*,i.idestoque,i.cdinvent from inventario i inner join inventario_item it on it.IDINVENT = i.IDINVENT where it.IDINVENT ={id}";
            return _dbSession.Connection.Query<InventarioItem>(query, null, _dbSession.Transaction).ToList();
        }

        public async Task<Inventario> ObterInventarioPorIdInventario(long id) 
        {
            var query = $@"select distinct i.IDINVENT as Id, i.* from inventario i where IDINVENT ={id}";
            return _dbSession.Connection.Query<Inventario>(query, null, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<bool> AtualizarValorCustoMedio(long id, double novoValorCustoMedio)
        {
            var query = $@"update inventario_item set VLCUSTOMEDIO = @VLCUSTOMEDIO WHERE IDINVENTITEM = @IDINVENTITEM";

            var parametros = new DynamicParameters();
            parametros.Add("@IDINVENTITEM", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@VLCUSTOMEDIO", novoValorCustoMedio, DbType.Double, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        public async Task<bool> AlterarSituacao(long id, ESituacaoInventario eSituacaoInventario)
        {
            var query = $@"update inventario set stinvent = @stinvent where IDINVENT = @IDINVENT ";

            var parametros = new DynamicParameters();
            parametros.Add("@stinvent", eSituacaoInventario, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@IDINVENT", id, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }
    }
}

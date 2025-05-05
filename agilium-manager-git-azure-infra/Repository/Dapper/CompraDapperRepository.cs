using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.ComprasNFEViewModel;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace agilium.api.infra.Repository.Dapper
{
    public class CompraDapperRepository : ICompraDapperRepository
    {
        private readonly IUtilDapperRepository _utilDapperRepository;
        protected readonly IConfiguration _configuration;
        private readonly IFornecedorDapperRepository _fornecedorDapperRepository;
        private readonly DbSession _dbSession;

        public CompraDapperRepository(IUtilDapperRepository utilDapperRepository, IConfiguration configuration, IFornecedorDapperRepository fornecedorDapperRepository,DbSession dbSession)
        {
            _utilDapperRepository = utilDapperRepository;
            _configuration = configuration;
            _fornecedorDapperRepository = fornecedorDapperRepository;
            _dbSession = dbSession;
        }

        public string GetConnection()
        {
            var autenticacaoUrl = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return autenticacaoUrl;
        }

        public async Task<Compra> AtualizarCompra(Compra compra)
        {
            return await AtualizarCompraTransacao(compra);
            
            //using (var con = new MySqlConnection(GetConnection()))
            //{
            //    try
            //    {
            //        var id = _utilDapperRepository.GerarUUID().Result;
            //        con.Open();

            //        //incluir contato

            //        //var codigo = GerarCodigo(con,idEmpresa).Result;
            //        var query = $@"UPDATE compra SET
            //                            DTCOMPRA = @DTCOMPRA, STCOMPRA = @STCOMPRA,DTNF = @DTNF,NUNF = @NUNF,DSSERIENF = @DSSERIENF,
            //                            DSCHAVENFE = @DSCHAVENFE,TPCOMPROVANTE = @TPCOMPROVANTE,NUCFOP = @NUCFOP,VLICMSRETIDO = @VLICMSRETIDO,
            //                            VLBSCALCICMS = @VLBSCALCICMS,VLICMS = @VLICMS,VLBSCALCSUB = @VLBSCALCSUB,VLICMSSUB = @VLICMSSUB,
            //                            VLISENCAO = @VLISENCAO,VLTOTPROD = @VLTOTPROD,VLFRETE = @VLFRETE,VLSEGURO = @VLSEGURO,VLDESCONTO = @VLDESCONTO,
            //                            VLOUTROS = @VLOUTROS,VLIPI = @VLIPI,VLTOTAL = @VLTOTAL,DSOBS = @DSOBS,STIMPORTADA = @STIMPORTADA
            //                       WHERE IDCOMPRA = @IDCOMPRA";
            //        compra.Id = id;

            //        var parametros = new DynamicParameters();
            //        parametros.Add("@IDCOMPRA", compra.Id, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@DTCOMPRA", compra.DTCOMPRA, DbType.DateTime, ParameterDirection.Input);
            //        parametros.Add("@CDCOMPRA", compra.CDCOMPRA, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@STCOMPRA", compra.STCOMPRA, DbType.Int32, ParameterDirection.Input);
            //        parametros.Add("@DTNF", compra.DTNF, DbType.DateTime, ParameterDirection.Input);
            //        parametros.Add("@NUNF", compra.NUNF, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@DSSERIENF", compra.DSSERIENF, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@DSCHAVENFE", compra.DSCHAVENFE, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@TPCOMPROVANTE", compra.TPCOMPROVANTE, DbType.Int32, ParameterDirection.Input);
            //        parametros.Add("@NUCFOP", compra.NUCFOP, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@VLICMSRETIDO", compra.VLICMSRETIDO, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLBSCALCICMS", compra.VLBSCALCICMS, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLICMS", compra.VLICMS, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLBSCALCSUB", compra.VLICMSSUB, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLICMSSUB", compra.VLICMSSUB, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLISENCAO", compra.VLISENCAO, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLTOTPROD", compra.VLTOTPROD, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLFRETE", compra.VLFRETE, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLSEGURO", compra.VLSEGURO, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLDESCONTO", compra.VLDESCONTO, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLOUTROS", compra.VLOUTROS, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLIPI", compra.VLIPI, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLTOTAL", compra.VLTOTAL, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@DSOBS", compra.DSOBS, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@STIMPORTADA", compra.STIMPORTADA, DbType.Int32, ParameterDirection.Input);

            //        con.Execute(query, parametros);

            //        return compra;
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //    finally { con.Close(); }
            //}
        }

        #region Transacao
        private async Task<Compra> AtualizarCompraTransacao(Compra compra)
        {
            var query = $@"UPDATE compra SET
                                DTCOMPRA = @DTCOMPRA, STCOMPRA = @STCOMPRA,DTNF = @DTNF,NUNF = @NUNF,DSSERIENF = @DSSERIENF,
                                DSCHAVENFE = @DSCHAVENFE,TPCOMPROVANTE = @TPCOMPROVANTE,NUCFOP = @NUCFOP,VLICMSRETIDO = @VLICMSRETIDO,
                                VLBSCALCICMS = @VLBSCALCICMS,VLICMS = @VLICMS,VLBSCALCSUB = @VLBSCALCSUB,VLICMSSUB = @VLICMSSUB,
                                VLISENCAO = @VLISENCAO,VLTOTPROD = @VLTOTPROD,VLFRETE = @VLFRETE,VLSEGURO = @VLSEGURO,VLDESCONTO = @VLDESCONTO,
                                VLOUTROS = @VLOUTROS,VLIPI = @VLIPI,VLTOTAL = @VLTOTAL,DSOBS = @DSOBS,STIMPORTADA = @STIMPORTADA
                            WHERE IDCOMPRA = @IDCOMPRA";
      

            var parametros = new DynamicParameters();
            parametros.Add("@IDCOMPRA", compra.Id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@DTCOMPRA", compra.DTCOMPRA, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@STCOMPRA", compra.STCOMPRA, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@DTNF", compra.DTNF, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@NUNF", compra.NUNF, DbType.String, ParameterDirection.Input);
            parametros.Add("@DSSERIENF", compra.DSSERIENF, DbType.String, ParameterDirection.Input);
            parametros.Add("@DSCHAVENFE", compra.DSCHAVENFE, DbType.String, ParameterDirection.Input);
            parametros.Add("@TPCOMPROVANTE", compra.TPCOMPROVANTE, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@NUCFOP", compra.NUCFOP, DbType.String, ParameterDirection.Input);
            parametros.Add("@VLICMSRETIDO", compra.VLICMSRETIDO, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLBSCALCICMS", compra.VLBSCALCICMS, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLICMS", compra.VLICMS, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLBSCALCSUB", compra.VLICMSSUB, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLICMSSUB", compra.VLICMSSUB, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLISENCAO", compra.VLISENCAO, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTPROD", compra.VLTOTPROD, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLFRETE", compra.VLFRETE, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLSEGURO", compra.VLSEGURO, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLDESCONTO", compra.VLDESCONTO, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLOUTROS", compra.VLOUTROS, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLIPI", compra.VLIPI, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTAL", compra.VLTOTAL, DbType.Double, ParameterDirection.Input);
            parametros.Add("@DSOBS", compra.DSOBS, DbType.String, ParameterDirection.Input);
            parametros.Add("@STIMPORTADA", compra.STIMPORTADA, DbType.Int32, ParameterDirection.Input);

            
        _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction);
                
        return compra;
            
        }

        private async Task<Compra> AdicionarCompraTransacao(Compra compra)
        {
       
            var id = _utilDapperRepository.GerarUUID().Result;
                
            //var codigo = GerarCodigo(con,idEmpresa).Result;
            var query = $@"INSERT INTO compra (IDCOMPRA,IDEMPRESA,IDFORN,IDTURNO,DTCOMPRA,DTCAD,CDCOMPRA,STCOMPRA,DTNF,NUNF,DSSERIENF,DSCHAVENFE,TPCOMPROVANTE,
                                                        NUCFOP,VLICMSRETIDO,VLBSCALCICMS,VLICMS,VLBSCALCSUB,VLICMSSUB,VLISENCAO,VLTOTPROD,VLFRETE,VLSEGURO,VLDESCONTO,VLOUTROS,
                                                        VLIPI,VLTOTAL,DSOBS,STIMPORTADA)
                                    VALUES (@IDCOMPRA, @IDEMPRESA, @IDFORN, @IDTURNO,@DTCOMPRA,@DTCAD,@CDCOMPRA,@STCOMPRA,@DTNF,@NUNF,@DSSERIENF,@DSCHAVENFE,@TPCOMPROVANTE,
                                                        @NUCFOP,@VLICMSRETIDO,@VLBSCALCICMS,@VLICMS,@VLBSCALCSUB,@VLICMSSUB,@VLISENCAO,@VLTOTPROD,@VLFRETE,@VLSEGURO,@VLDESCONTO
                                                        ,@VLOUTROS,@VLIPI,@VLTOTAL,@DSOBS,@STIMPORTADA)";
            compra.Id = id;

            var parametros = new DynamicParameters();
            parametros.Add("@IDCOMPRA", compra.Id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDEMPRESA", compra.IDEMPRESA, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDTURNO", compra.IDTURNO, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@DTCOMPRA", compra.DTCOMPRA, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@DTCAD", compra.DTCAD, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@CDCOMPRA", compra.CDCOMPRA, DbType.String, ParameterDirection.Input);
            parametros.Add("@STCOMPRA", compra.STCOMPRA, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@DTNF", compra.DTNF, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@NUNF", compra.NUNF, DbType.String, ParameterDirection.Input);
            parametros.Add("@DSSERIENF", compra.DSSERIENF, DbType.String, ParameterDirection.Input);
            parametros.Add("@DSCHAVENFE", compra.DSCHAVENFE, DbType.String, ParameterDirection.Input);
            parametros.Add("@TPCOMPROVANTE", compra.TPCOMPROVANTE, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@NUCFOP", compra.NUCFOP, DbType.String, ParameterDirection.Input);
            parametros.Add("@VLICMSRETIDO", compra.VLICMSRETIDO, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLBSCALCICMS", compra.VLBSCALCICMS, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLICMS", compra.VLICMS, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLBSCALCSUB", compra.VLICMSSUB, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLICMSSUB", compra.VLICMSSUB, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLISENCAO", compra.VLISENCAO, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTPROD", compra.VLTOTPROD, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLFRETE", compra.VLFRETE, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLSEGURO", compra.VLSEGURO, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLDESCONTO", compra.VLDESCONTO, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLOUTROS", compra.VLOUTROS, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLIPI", compra.VLIPI, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTAL", compra.VLTOTAL, DbType.Double, ParameterDirection.Input);
            parametros.Add("@DSOBS", compra.DSOBS, DbType.String, ParameterDirection.Input);
            parametros.Add("@STIMPORTADA", compra.STIMPORTADA, DbType.Int32, ParameterDirection.Input);

            _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction);
            
            return compra;
        }

        private async Task<Compra> ExisteCompraPorChaveAcessoTransacao(string chaveAcesso)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@DSCHAVENFE", chaveAcesso, DbType.String, ParameterDirection.Input);
            parametros.Add("@STCOMPRA", ESituacaoCompra.Cancelada, DbType.Int32, ParameterDirection.Input);

            var query = $@"SELECT compra.IDCOMPRA as Id, compra.* FROM compra WHERE DSCHAVENFE = @DSCHAVENFE AND STCOMPRA <> @STCOMPRA";

            return _dbSession.Connection.Query<Compra>(query, parametros,_dbSession.Transaction).FirstOrDefault();
            
        }

        private async Task<Compra> ObterCompraPorIdTransacao(long id)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDCOMPRA", id, DbType.Int64, ParameterDirection.Input);

            var query = $@"SELECT compra.IDCOMPRA as Id, compra.* FROM compra WHERE IDCOMPRA = @IDCOMPRA";

            return _dbSession.Connection.Query<Compra>(query, parametros, _dbSession.Transaction).FirstOrDefault();
            
        }

        private async Task<bool> ApagarItensPorIdCompraTransacao(long id)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDCOMPRA", id, DbType.Int64, ParameterDirection.Input);

            var query = $@"delete FROM compra_item WHERE IDCOMPRA = @IDCOMPRA";

            return _dbSession.Connection.Query<bool>(query, parametros, _dbSession.Transaction).Any();
        }

        private async Task<CompraItem> AdicionarItemTransacao(CompraItem item)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
  
            var query = $@"INSERT INTO compra_item 
                                (IDITEM,IDCOMPRA,IDPRODUTO,IDESTOQUE,DSPRODUTO,CDEAN,CDNCM,CDCEST,SGUN,NUQTD,NURELACAO,
                                VLUNIT,VLTOTAL,DTVALIDADE,NUCFOP,VLOUTROS,VLBSRET,PCICMSRET,PCREDUCAO,CDCSTICMS,CDCSTPIS,CDCSTCOFINS,CDCSTIPI,VLALIQPIS,
                                VLALIQCOFINS,VLALIQICMS,VLALIQIPI,VLBSCALCPIS,VLBSCALCCOFINS,VLBSCALCICMS,VLBSCALCIPI,VLICMS,VLPIS,VLCOFINS,VLIPI,CDPRODFORN,VLNOVOPRECOVENDA)
                        VALUES (@IDITEM,@IDCOMPRA,@IDPRODUTO,@IDESTOQUE,@DSPRODUTO,@CDEAN,@CDNCM,@CDCEST,@SGUN,@NUQTD,@NURELACAO,
                                @VLUNIT,@VLTOTAL,@DTVALIDADE,@NUCFOP,@VLOUTROS,@VLBSRET,@PCICMSRET,@PCREDUCAO,@CDCSTICMS,@CDCSTPIS,@CDCSTCOFINS,@CDCSTIPI,@VLALIQPIS,
                                @VLALIQCOFINS,@VLALIQICMS,@VLALIQIPI,@VLBSCALCPIS,@VLBSCALCCOFINS,@VLBSCALCICMS,@VLBSCALCIPI,@VLICMS,@VLPIS,@VLCOFINS,
                                @VLIPI,@CDPRODFORN,@VLNOVOPRECOVENDA)";

            var parametros = new DynamicParameters();
            parametros.Add("@IDCOMPRA", item.IDCOMPRA, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDITEM", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPRODUTO", item.IDPRODUTO, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDESTOQUE", item.IDESTOQUE, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@DSPRODUTO", item.DSPRODUTO, DbType.String, ParameterDirection.Input);
            parametros.Add("@CDEAN", item.CDEAN, DbType.String, ParameterDirection.Input);
            parametros.Add("@CDNCM", item.CDNCM, DbType.String, ParameterDirection.Input);
            parametros.Add("@SGUN", item.SGUN, DbType.String, ParameterDirection.Input);
            parametros.Add("@CDCEST", item.CDCEST, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NUQTD", item.NUQTD, DbType.Double, ParameterDirection.Input);
            parametros.Add("@NURELACAO", item.NURELACAO, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLUNIT", item.VLUNIT, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTAL", item.VLTOTAL, DbType.Double, ParameterDirection.Input);
            parametros.Add("@DTVALIDADE", item.DTVALIDADE, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@VLOUTROS", item.VLOUTROS, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLBSRET", item.VLBSRET, DbType.Double, ParameterDirection.Input);
            parametros.Add("@PCICMSRET", item.PCICMSRET, DbType.Double, ParameterDirection.Input);
            parametros.Add("@PCREDUCAO", item.PCREDUCAO, DbType.Double, ParameterDirection.Input);
            parametros.Add("@CDCSTICMS", item.CDCSTICMS, DbType.String, ParameterDirection.Input);
            parametros.Add("@CDCSTPIS", item.CDCSTPIS, DbType.String, ParameterDirection.Input);
            parametros.Add("@CDCSTCOFINS", item.CDCSTCOFINS, DbType.String, ParameterDirection.Input);
            parametros.Add("@CDCSTIPI", item.CDCSTIPI, DbType.String, ParameterDirection.Input);
            parametros.Add("@VLALIQPIS", item.VLALIQCOFINS, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLALIQCOFINS", item.VLALIQCOFINS, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLALIQICMS", item.VLALIQICMS, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLALIQIPI", item.VLALIQIPI, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLBSCALCPIS", item.VLBSCALCPIS, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLBSCALCCOFINS", item.VLBSCALCCOFINS, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLBSCALCICMS", item.VLBSCALCICMS, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLBSCALCIPI", item.VLBSCALCIPI, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLICMS", item.VLICMS, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLPIS", item.VLPIS, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLCOFINS", item.VLCOFINS, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLIPI", item.VLIPI, DbType.Double, ParameterDirection.Input);
            parametros.Add("@CDPRODFORN", item.CDPRODFORN, DbType.String, ParameterDirection.Input);
            parametros.Add("@VLNOVOPRECOVENDA", item.VLNOVOPRECOVENDA, DbType.Double, ParameterDirection.Input);
            parametros.Add("@NUCFOP", item.NUCFOP, DbType.Int32, ParameterDirection.Input);

            _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction);

            return item;
            
        }

        private async Task<string> GerarCodigo(long idEmpresa)
        {
            var query = $@"SELECT MAX(CAST(CDCOMPRA AS UNSIGNED)) AS CD FROM compra WHERE IDEMPRESA = {idEmpresa}";

            return _dbSession.Connection.Query<string>(query, _dbSession.Transaction).FirstOrDefault();
        }

        private async Task<Cep> ObterEnderecoPorCep(string cep)
        {
            var query = $@"select cep, ender, bairro, cidade, uf, IBGE from ceps where cep = @cep";
            var parametros = new DynamicParameters();
            return _dbSession.Connection.Query<Cep>(query, _dbSession.Transaction).FirstOrDefault();
        }
        #endregion

        public async Task<Compra> AdicionarCompra(Compra compra)
        {
            return await AdicionarCompraTransacao(compra);
            //using (var con = new MySqlConnection(GetConnection()))
            //{
            //    try
            //    {
            //        var id = _utilDapperRepository.GerarUUID().Result;
            //        con.Open();

            //        //incluir contato

            //        //var codigo = GerarCodigo(con,idEmpresa).Result;
            //        var query = $@"INSERT INTO compra (IDCOMPRA,IDEMPRESA,IDFORN,IDTURNO,DTCOMPRA,DTCAD,CDCOMPRA,STCOMPRA,DTNF,NUNF,DSSERIENF,DSCHAVENFE,TPCOMPROVANTE,
            //                                                    NUCFOP,VLICMSRETIDO,VLBSCALCICMS,VLICMS,VLBSCALCSUB,VLICMSSUB,VLISENCAO,VLTOTPROD,VLFRETE,VLSEGURO,VLDESCONTO,VLOUTROS,
            //                                                    VLIPI,VLTOTAL,DSOBS,STIMPORTADA)
            //                                VALUES (@IDCOMPRA, @IDEMPRESA, @IDFORN, @IDTURNO,@DTCOMPRA,@DTCAD,@CDCOMPRA,@STCOMPRA,@DTNF,@NUNF,@DSSERIENF,@DSCHAVENFE,@TPCOMPROVANTE,
            //                                                    @NUCFOP,@VLICMSRETIDO,@VLBSCALCICMS,@VLICMS,@VLBSCALCSUB,@VLICMSSUB,@VLISENCAO,@VLTOTPROD,@VLFRETE,@VLSEGURO,@VLDESCONTO
            //                                                    ,@VLOUTROS,@VLIPI,@VLTOTAL,@DSOBS,@STIMPORTADA)";
            //        compra.Id = id;

            //        var parametros = new DynamicParameters();
            //        parametros.Add("@IDCOMPRA", compra.Id, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@IDEMPRESA", compra.IDEMPRESA, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@IDTURNO", compra.IDTURNO, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@DTCOMPRA", compra.DTCOMPRA, DbType.DateTime, ParameterDirection.Input);
            //        parametros.Add("@DTCAD", compra.DTCAD, DbType.DateTime, ParameterDirection.Input);
            //        parametros.Add("@CDCOMPRA", compra.CDCOMPRA, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@STCOMPRA", compra.STCOMPRA, DbType.Int32, ParameterDirection.Input);
            //        parametros.Add("@DTNF", compra.DTNF, DbType.DateTime, ParameterDirection.Input);
            //        parametros.Add("@NUNF", compra.NUNF, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@DSSERIENF", compra.DSSERIENF, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@DSCHAVENFE", compra.DSCHAVENFE, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@TPCOMPROVANTE", compra.TPCOMPROVANTE, DbType.Int32, ParameterDirection.Input);
            //        parametros.Add("@NUCFOP", compra.NUCFOP, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@VLICMSRETIDO", compra.VLICMSRETIDO, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLBSCALCICMS", compra.VLBSCALCICMS, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLICMS", compra.VLICMS, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLBSCALCSUB", compra.VLICMSSUB, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLICMSSUB", compra.VLICMSSUB, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLISENCAO", compra.VLISENCAO, DbType.Double, ParameterDirection.Input); 
            //        parametros.Add("@VLTOTPROD", compra.VLTOTPROD, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLFRETE", compra.VLFRETE, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLSEGURO", compra.VLSEGURO, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLDESCONTO", compra.VLDESCONTO, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLOUTROS", compra.VLOUTROS, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLIPI", compra.VLIPI, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@VLTOTAL", compra.VLTOTAL, DbType.Double, ParameterDirection.Input);
            //        parametros.Add("@DSOBS", compra.DSOBS, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@STIMPORTADA", compra.STIMPORTADA, DbType.Int32, ParameterDirection.Input);

            //        con.Execute(query, parametros);

            //        return compra;
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //    finally { con.Close(); }
            //}
        }

        public async Task<Compra> ExisteCompraPorChaveAcesso(string chaveAcesso)
        {
            return await ExisteCompraPorChaveAcessoTransacao(chaveAcesso);
            //using (var con = new MySqlConnection(GetConnection()))
            //{
            //    try
            //    {
            //        con.Open();
            //        var parametros = new DynamicParameters();
            //        parametros.Add("@DSCHAVENFE", chaveAcesso, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@STCOMPRA", ESituacaoCompra.Cancelada, DbType.Int32, ParameterDirection.Input);

            //        var query = $@"SELECT compra.IDCOMPRA as Id, compra.* FROM compra WHERE DSCHAVENFE = @DSCHAVENFE AND STCOMPRA <> @STCOMPRA";

            //        return con.Query<Compra>(query, parametros).FirstOrDefault();
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //    finally { con.Close(); }
            //}
        }

        public async Task<List<CompraItem>> ObterCompraItemPorIdCompra(long idCompra)
        {
            var query = $@"SELECT CI.* FROM compra_item CI INNER JOIN produto P ON CI.IDPRODUTO = P.IDPRODUTO WHERE IDCOMPRA = {idCompra}";
            var compraItens = _dbSession.Connection.Query<CompraItem>(query,null,_dbSession.Transaction).ToList();


            compraItens.ForEach(item => {
                if (item.IDPRODUTO.HasValue)
                {
                    var produto = _dbSession.Connection.Query<Produto>($"select produto.IDPRODUTO as Id, produto.* from produto where produto.IDPRODUTO = {item.IDPRODUTO}", null, _dbSession.Transaction).FirstOrDefault();
                    if(produto != null)
                        item.AdicionarProduto(produto);
                }

            });

            return compraItens;
        }

        private async Task<string> GerarCodigo(MySqlConnection con, long idEmpresa)
        {
            var query = $@"SELECT MAX(CAST(CDCOMPRA AS UNSIGNED)) AS CD FROM compra WHERE IDEMPRESA = {idEmpresa}";

            return con.Query<string>(query).FirstOrDefault();
        }

        private async Task<Cep> ObterEnderecoPorCep(MySqlConnection con, string cep)
        {
            var query = $@"select cep, ender, bairro, cidade, uf, IBGE from ceps where cep = @cep";
            var parametros = new DynamicParameters();
            return con.Query<Cep>(query).FirstOrDefault();
        }

        public async Task<Compra> ObterCompraPorId(long id)
        {
            return await ObterCompraPorIdTransacao(id);
            //using (var con = new MySqlConnection(GetConnection()))
            //{
            //    try
            //    {
            //        con.Open();
            //        var parametros = new DynamicParameters();
            //        parametros.Add("@IDCOMPRA", id, DbType.Int64, ParameterDirection.Input);

            //        var query = $@"SELECT compra.IDCOMPRA as Id, compra.* FROM compra WHERE IDCOMPRA = @IDCOMPRA";

            //        return con.Query<Compra>(query, parametros).FirstOrDefault();
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //    finally { con.Close(); }
            //}
        }

        public async Task<bool> ApagarItensPorIdCompra(long id)
        {
            return await ApagarItensPorIdCompraTransacao(id);
            //using (var con = new MySqlConnection(GetConnection()))
            //{
            //    try
            //    {
            //        con.Open();
            //        var parametros = new DynamicParameters();
            //        parametros.Add("@IDCOMPRA", id, DbType.Int64, ParameterDirection.Input);

            //        var query = $@"delete FROM compra_item WHERE IDCOMPRA = @IDCOMPRA";
                    
            //        return con.Query<bool>(query, parametros).Any();
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //    finally { con.Close(); }
            //}
        }

        public async Task<CompraItem> AdicionarItem(CompraItem item)
        {
            return await AdicionarItemTransacao(item);
            //using (var con = new MySqlConnection(GetConnection()))
            //{
            //    try
            //    {
            //        var id = _utilDapperRepository.GerarUUID().Result;
            //        con.Open();

            //        var query = $@"INSERT INTO compra_item 
            //                            (IDITEM,IDCOMPRA,IDPRODUTO,IDESTOQUE,DSPRODUTO,CDEAN,CDNCM,CDCEST,SGUN,NUQTD,NURELACAO,
            //                            VLUNIT,VLTOTAL,DTVALIDADE,NUCFOP,VLOUTROS,VLBSRET,PCICMSRET,PCREDUCAO,CDCSTICMS,CDCSTPIS,CDCSTCOFINS,CDCSTIPI,VLALIQPIS,
            //                            VLALIQCOFINS,VLALIQICMS,VLALIQIPI,VLBSCALCPIS,VLBSCALCCOFINS,VLBSCALCICMS,VLBSCALCIPI,VLICMS,VLPIS,VLCOFINS,VLIPI,CDPRODFORN,VLNOVOPRECOVENDA)
            //                    VALUES (@IDITEM,@IDCOMPRA,@IDPRODUTO,@IDESTOQUE,@DSPRODUTO,@CDEAN,@CDNCM,@CDCEST,@SGUN,@NUQTD,@NURELACAO,
            //                            @VLUNIT,@VLTOTAL,@DTVALIDADE,@NUCFOP,@VLOUTROS,@VLBSRET,@PCICMSRET,@PCREDUCAO,@CDCSTICMS,@CDCSTPIS,@CDCSTCOFINS,@CDCSTIPI,@VLALIQPIS,
            //                            @VLALIQCOFINS,@VLALIQICMS,@VLALIQIPI,@VLBSCALCPIS,@VLBSCALCCOFINS,@VLBSCALCICMS,@VLBSCALCIPI,@VLICMS,@VLPIS,@VLCOFINS,
            //                            @VLIPI,@CDPRODFORN,@VLNOVOPRECOVENDA)";

            //        var parametros = new DynamicParameters();
            //        parametros.Add("@IDCOMPRA", item.Id, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@IDITEM", id, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@IDPRODUTO", item.IDPRODUTO, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@IDESTOQUE", item.IDESTOQUE, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@DSPRODUTO", item.DSPRODUTO, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@CDEAN", item.CDEAN, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@CDNCM", item.CDNCM, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@SGUN", item.SGUN, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@CDCEST", item.CDCEST, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@NUQTD", item.NUQTD, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@NURELACAO", item.NURELACAO, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@VLUNIT", item.VLUNIT, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@VLTOTAL", item.VLTOTAL, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@DTVALIDADE", item.DTVALIDADE, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@VLOUTROS", item.VLOUTROS, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@VLBSRET", item.VLBSRET, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@PCICMSRET", item.PCICMSRET, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@PCREDUCAO", item.PCREDUCAO, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@CDCSTICMS", item.CDCSTICMS, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@CDCSTPIS", item.CDCSTPIS, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@CDCSTCOFINS", item.CDCSTCOFINS, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@CDCSTIPI", item.CDCSTIPI, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@VLALIQPIS", item.VLALIQCOFINS, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@VLALIQCOFINS", item.VLALIQCOFINS, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@VLALIQICMS", item.VLALIQICMS, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@VLALIQIPI", item.VLALIQIPI, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@VLBSCALCPIS", item.VLBSCALCPIS, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@VLBSCALCCOFINS", item.VLBSCALCCOFINS, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@VLBSCALCICMS", item.VLBSCALCICMS, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@VLBSCALCIPI", item.VLBSCALCIPI, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@VLICMS", item.VLICMS, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@VLPIS", item.VLPIS, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@VLCOFINS", item.VLCOFINS, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@VLIPI", item.VLIPI, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@CDPRODFORN", item.CDPRODFORN, DbType.Int64, ParameterDirection.Input);
            //        parametros.Add("@VLNOVOPRECOVENDA", item.VLNOVOPRECOVENDA, DbType.Int64, ParameterDirection.Input);

            //        con.Execute(query, parametros);

            //        return item;
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //    finally { con.Close(); }
            //}
        }

        public async Task<bool> AdicionarFiscal(long id, string xml)
        {
            var query = $@"INSERT INTO compra_fiscal (IDCOMPRAFISCAL,IDCOMPRA,STMANIFESTO,DSXML) 
                            VALUES (@IDCOMPRAFISCAL,@IDCOMPRA,@STMANIFESTO,@DSXML)";

            var idCompraFiscal = _utilDapperRepository.GerarUUID().Result;

            var parametros = new DynamicParameters();
            parametros.Add("@IDCOMPRA", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDCOMPRAFISCAL", idCompraFiscal, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@STMANIFESTO", ETipoManifestoCompra.NaoManifestada, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@DSXML", xml, DbType.String, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query,parametros, _dbSession.Transaction)>0;
        }

        public async Task<bool> AtualizarSituacaoCompra(long idCompra, ESituacaoCompra eSituacaoCompra)
        {
            var query = $@"UPDATE compra SET STCOMPRA = @STCOMPRA WHERE IDCOMPRA = @IDCOMPRA";

            var parametros = new DynamicParameters();
            parametros.Add("@IDCOMPRA", idCompra, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@STCOMPRA", eSituacaoCompra, DbType.Int32, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        public async Task<List<CompraItem>> ObterItemCompraEfetivada(long idCompra)
        {
            var query = $@"SELECT CI.IDITEM, CI.IDPRODUTO, CI.IDESTOQUE, CI.DSPRODUTO FROM compra_item CI 
                        INNER JOIN estoquehst HST ON CI.IDITEM = HST.IDITEM 
                        INNER JOIN planoconta_lanc PCL ON HST.IDLANC = PCL.IDLANC
                        WHERE CI.IDCOMPRA = {idCompra}";
            var compraItens = _dbSession.Connection.Query<CompraItem>(query, null, _dbSession.Transaction).ToList();

            compraItens.ForEach(item => {
                if (item.IDPRODUTO.HasValue)
                {
                    var queryEstoqueHist = $"select  HST.IDESTOQUEHST as Id, HST.IDLANC, HST.QTDHST, HST.IDESTOQUE from estoquehst HST where HST.IDITEM = {item.Id}";
                    var estoqueHistorico = _dbSession.Connection.Query<EstoqueHistorico>(queryEstoqueHist, null, _dbSession.Transaction).FirstOrDefault();
                    if (estoqueHistorico != null)
                    {
                        item.AdicionarEstoqueHistorico(estoqueHistorico);
                        if (estoqueHistorico.IDLANC.HasValue)
                        {
                            var queryPlanoConta = $"select IDLANC as Id, IDCONTA , DTREF, NUANOMESREF, DSLANC, VLLANC, TPLANC, STLANC from planoconta_lanc where IDLANC = {estoqueHistorico.IDLANC}";
                            var planoContaLancamento = _dbSession.Connection.Query<PlanoContaLancamento>(queryPlanoConta, null, _dbSession.Transaction).FirstOrDefault();
                            
                            if(planoContaLancamento != null)
                                estoqueHistorico.AdicionarPlanoConta(planoContaLancamento);
                        }

                        item.AdicionarEstoqueHistorico(estoqueHistorico);                       
                    }
                  
                }

            });

            return compraItens;
        }

        public async Task<List<CompraItem>> ObterItemCompraPorIdCompraParaCadastroAutomatico(long idCompra)
        {
            var query = $@"SELECT ci.IDITEM as Id, ci.* FROM compra_item ci WHERE ci.IDCOMPRA = {idCompra} AND ci.IDPRODUTO IS NULL";
            var compraItens = _dbSession.Connection.Query<CompraItem>(query, null, _dbSession.Transaction).ToList();

            return compraItens;
        }

        public async Task<bool> AtualizarCompraItemComIdProduto(long idProduto, long idItem)
        {
            var query = $@"UPDATE compra_item SET IDPRODUTO =@IDPRODUTO WHERE IDITEM = @IDITEM";

            var parametros = new DynamicParameters();
            parametros.Add("@IDPRODUTO", idProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDITEM", idItem, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0; ;
        }

        public async Task<int> ObterQtdItensNaoAssociados(long idCompra)
        {

            var query = $@"SELECT COUNT(*) AS TOTAL FROM compra_item WHERE IDCOMPRA = {idCompra} AND IDPRODUTO IS NULL";
            return _dbSession.Connection.Query<int>(query, null, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<bool> AtualizarProdutoNoItemCompra(long idItem, long idCompra, long? idProduto, long? idEstoque, string SGUN, double? Quantidade, double? Relacao, double? ValorUnitario, double? ValorTotal, double? NovoPrecoVenda)
        {
            var query = $@"UPDATE compra_item SET IDPRODUTO =@IDPRODUTO, IDCOMPRA =@IDCOMPRA, IDESTOQUE=@IDESTOQUE, NURELACAO=@NURELACAO, NUQTD=@NUQTD, VLUNIT=@VLUNIT, VLNOVOPRECOVENDA=@VLNOVOPRECOVENDA,SGUN=@SGUN WHERE IDITEM = @IDITEM";

            var parametros = new DynamicParameters();
            parametros.Add("@IDPRODUTO", idProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDITEM", idItem, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDCOMPRA", idCompra, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDESTOQUE", idEstoque, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NURELACAO", Relacao, DbType.Double, ParameterDirection.Input);
            parametros.Add("@NUQTD", Quantidade, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLUNIT", ValorUnitario, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLNOVOPRECOVENDA", NovoPrecoVenda, DbType.Double, ParameterDirection.Input); 
            parametros.Add("@SGUN", SGUN, DbType.String, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0; 
        }
    }
}

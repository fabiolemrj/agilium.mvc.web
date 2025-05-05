using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.ReportViewModel.EstoqueReportViewModel;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace agilium.api.infra.Repository.Dapper
{
    public class EstoqueDapperRepository : IEstoqueDapperRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly DbSession _dbSession;
        private readonly IUtilDapperRepository _utilDapperRepository;
        private readonly IDapperRepository _dapperRepository;

        public EstoqueDapperRepository(IConfiguration configuration, DbSession dbSession, IUtilDapperRepository utilDapperRepository, IDapperRepository dapperRepository)
        {
            _configuration = configuration;
            _dbSession = dbSession;
            _utilDapperRepository = utilDapperRepository;
            _dapperRepository = dapperRepository;
        }

        public string GetConnection()
        {
            var autenticacaoUrl = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return autenticacaoUrl;
        }

        public async Task<long> RealizaEntradaRetornaIdHistoricoGerado(long idEstoque, long idProduto, long idItem, string UsuarioHistorico, string DescricaoHistorico, double Quantidade)
        {
            return RealizaEntradaRetornaIdHistoricoGeradoTransacao(idEstoque, idProduto, idItem, UsuarioHistorico, DescricaoHistorico, Quantidade).Result;
            
            //long idEstoqueHistorico = 0;
            //var tpHistoricoEntrada = 1;

        
            //    using (var con = new MySqlConnection(GetConnection()))
            //    {
            //        try
            //        {
                        
            //            con.Open();
            //            var query = $@"SELECT IDPRODUTO as Id, IDEMPRESA, IDGRUPO, IDSUBGRUPO, IDDEP,IDMARCA, CDPRODUTO, NMPRODUTO, CTPRODUTO ,TPPRODUTO,UNCOMPRA, UNVENDA,
            //                            NURELACAO,NUPRECO,NUQTDMIN,CDSEFAZ,CDANP,CDNCM,CDCEST,CDSERV,STPRODUTO,VLULTIMACOMPRA,VLCUSTOMEDIO,PCIBPTFED,PCIBPTEST,PCIBPTMUN,
            //                            PCIBPTIMP,NUCFOP,STORIGEMPROD,DSICMS_CST,PCICMS_ALIQ,PCICMS_REDUCBC,PCICMSST_ALIQ,PCICMSST_MVA,PCICMSST_REDUCBC,DSIPI_CST,PCIPI_ALIQ,
            //                            DSPIS_CST,PCPIS_ALIQ,DSCOFINS_CST,PCCOFINS_ALIQ,STESTOQUE,STBALANCA,DSVOLUME
            //                        FROM produto WHERE IDPRODUTO ={idProduto}";
            //            var produto = con.Query<Produto>(query).FirstOrDefault();
            //            if(produto != null && (produto.CTPRODUTO == "1" || produto.CTPRODUTO == "4"))
            //            {
            //                var idEstoqueProduto = con.Query<long>($@"SELECT IDESTOQUE_PROD FROM estoque_prod WHERE IDESTOQUE = {idEstoque} AND IDPRODUTO = {idProduto}").FirstOrDefault();
            //                if (idEstoqueProduto > 0)
            //                {
            //                    AtualizarEstoqueProduto(1,idEstoqueProduto, Quantidade, con);
            //                }
            //                else
            //                {
            //                    idEstoqueProduto = GerarUUID(con).Result;
            //                    IncluirEstoqueProduto(idEstoqueProduto, idEstoque, idProduto, Quantidade, con);
            //                }

            //                idEstoqueHistorico = IncluirEstoqueHistorico(idEstoque, idProduto, idItem, UsuarioHistorico, tpHistoricoEntrada, DescricaoHistorico, Quantidade, con);
            //            }
            //            return idEstoqueHistorico;                
            //        }
            //        catch (Exception ex)
            //        {
            //            throw;
            //        }
            //        finally { con.Close(); }
            //}

        }
        public async Task<long> RealizaRetiradaRetornaIdHistoricoGerado(long idEstoque, long idProduto, long idItem, string UsuarioHistorico, string DescricaoHistorico, double Quantidade)
        {

            return RealizaRetiradaRetornaIdHistoricoGeradoTransacao(idEstoque,idProduto, idItem,UsuarioHistorico,DescricaoHistorico,Quantidade).Result;

            //long idEstoqueHistorico = 0;
            //var tpHistoricoEntrada = 1;


            //using (var con = new MySqlConnection(GetConnection()))
            //{
            //    try
            //    {

            //        con.Open();
            //        var query = $@"SELECT IDPRODUTO as Id, IDEMPRESA, IDGRUPO, IDSUBGRUPO, IDDEP,IDMARCA, CDPRODUTO, NMPRODUTO, CTPRODUTO ,TPPRODUTO,UNCOMPRA, UNVENDA,
            //                            NURELACAO,NUPRECO,NUQTDMIN,CDSEFAZ,CDANP,CDNCM,CDCEST,CDSERV,STPRODUTO,VLULTIMACOMPRA,VLCUSTOMEDIO,PCIBPTFED,PCIBPTEST,PCIBPTMUN,
            //                            PCIBPTIMP,NUCFOP,STORIGEMPROD,DSICMS_CST,PCICMS_ALIQ,PCICMS_REDUCBC,PCICMSST_ALIQ,PCICMSST_MVA,PCICMSST_REDUCBC,DSIPI_CST,PCIPI_ALIQ,
            //                            DSPIS_CST,PCPIS_ALIQ,DSCOFINS_CST,PCCOFINS_ALIQ,STESTOQUE,STBALANCA,DSVOLUME
            //                        FROM produto WHERE IDPRODUTO ={idProduto}";
            //        var produto = con.Query<Produto>(query).FirstOrDefault();
            //        if (produto != null && (produto.CTPRODUTO == "1" || produto.CTPRODUTO == "4"))
            //        {
            //            var idEstoqueProduto = con.Query<long>($@"SELECT IDESTOQUE_PROD FROM estoque_prod WHERE IDESTOQUE = {idEstoque} AND IDPRODUTO = {idProduto}").FirstOrDefault();
            //            if (idEstoqueProduto > 0)
            //            {
            //                AtualizarEstoqueProduto(2,idEstoqueProduto, Quantidade, con);
            //            }
            //            else
            //            {
            //                idEstoqueProduto = GerarUUID(con).Result;
            //                IncluirEstoqueProduto(idEstoqueProduto, idEstoque, idProduto, Quantidade * (-1), con);
            //            }

            //            idEstoqueHistorico = IncluirEstoqueHistorico(idEstoque, idProduto, idItem, UsuarioHistorico, tpHistoricoEntrada, DescricaoHistorico, Quantidade, con);
            //        }
            //        return idEstoqueHistorico;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //    finally { con.Close(); }
            //}
        }
       
        public async Task<bool> AtaulizarLancamentoEstoqueHistorico(long idLancamento, long idEstoqueHist)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDLANC", idLancamento, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDESTOQUEHST", idEstoqueHist, DbType.Int64, ParameterDirection.Input);

            var query = $@"UPDATE estoquehst SET IDLANC = @IDLANC WHERE IDESTOQUEHST = @IDESTOQUEHST";

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;

        }
        #region privates 
        private async Task<long> GerarUUID(MySqlConnection con)
        {
            var query = $@"SELECT uuid_short() AS ID";

            return con.Query<long>(query).FirstOrDefault();
        }

        private void IncluirEstoqueProduto(long idEstoqueProduto,long idEstoque, long idProduto,double quant, MySqlConnection con)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDESTOQUE_PROD", idEstoqueProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDESTOQUE", idEstoque, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPRODUTO", idProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NUQTD", quant, DbType.Double, ParameterDirection.Input);
            
            var query = $@"INSERT INTO estoque_prod (IDESTOQUE_PROD, IDESTOQUE, IDPRODUTO, NUQTD)
                            values (@IDESTOQUE_PROD, @IDESTOQUE, @IDPRODUTO, @NUQTD)";

            con.Execute(query, parametros);
        }

        private void AtualizarEstoqueProduto(int tpmov,long idEstoqueProduto, double quant, MySqlConnection con)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDESTOQUE_PROD", idEstoqueProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NUQTD", quant, DbType.Double, ParameterDirection.Input);

            var query = "";
            if (tpmov == 1)
                query= $@"UPDATE estoque_prod SET NUQTD = @NUQTD  WHERE IDESTOQUE_PROD = @IDESTOQUE_PROD";
            else 
                query = $@"UPDATE estoque_prod SET NUQTD = NUQTD - @NUQTD  WHERE IDESTOQUE_PROD = @IDESTOQUE_PROD";


            con.Execute(query, parametros);
        }

        private long IncluirEstoqueHistorico(long idEstoque, long idProduto, long idItem, string usuario,int tipoHistorico,
            string descricaoHistorico,double quant,MySqlConnection con)
        {
            var resultado = GerarUUID(con).Result;

            var parametros = new DynamicParameters();
            parametros.Add("@IDESTOQUEHST", resultado, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDESTOQUE", idEstoque, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPRODUTO", idProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@QTDHST", quant, DbType.Double, ParameterDirection.Input);
            parametros.Add("@IDITEM", idProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@DTHRHST", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@TPHST", tipoHistorico, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@DSHST", descricaoHistorico, DbType.String, ParameterDirection.Input);
            parametros.Add("@NMUSUARIO", usuario, DbType.String, ParameterDirection.Input);

            var query = $@"INSERT INTO estoquehst (IDESTOQUEHST, IDESTOQUE, IDPRODUTO,IDITEM, DTHRHST, NMUSUARIO, TPHST, DSHST, QTDHST)
                            VALUES (@IDESTOQUEHST, @IDESTOQUE, @IDPRODUTO, @IDITEM, @DTHRHST, @NMUSUARIO, @TPHST, @DSHST, @QTDHST)";

            con.Execute(query, parametros);
            
            return resultado;
        }


        #endregion

        #region Transacao
        public async Task<long> RealizaEntradaRetornaIdHistoricoGeradoTransacao(long idEstoque, long idProduto, long idItem, string UsuarioHistorico, string DescricaoHistorico, double Quantidade)
        {
            long idEstoqueHistorico = 0;
            var tpHistoricoEntrada = 1;


            
            var query = $@"SELECT IDPRODUTO as Id, IDEMPRESA, IDGRUPO, IDSUBGRUPO, IDDEP,IDMARCA, CDPRODUTO, NMPRODUTO, CTPRODUTO ,TPPRODUTO,UNCOMPRA, UNVENDA,
                                NURELACAO,NUPRECO,NUQTDMIN,CDSEFAZ,CDANP,CDNCM,CDCEST,CDSERV,STPRODUTO,VLULTIMACOMPRA,VLCUSTOMEDIO,PCIBPTFED,PCIBPTEST,PCIBPTMUN,
                                PCIBPTIMP,NUCFOP,STORIGEMPROD,DSICMS_CST,PCICMS_ALIQ,PCICMS_REDUCBC,PCICMSST_ALIQ,PCICMSST_MVA,PCICMSST_REDUCBC,DSIPI_CST,PCIPI_ALIQ,
                                DSPIS_CST,PCPIS_ALIQ,DSCOFINS_CST,PCCOFINS_ALIQ,STESTOQUE,STBALANCA,DSVOLUME
                            FROM produto WHERE IDPRODUTO ={idProduto}";
            var produto = _dbSession.Connection.Query<Produto>(query,null,_dbSession.Transaction).FirstOrDefault();
            if (produto != null && (produto.CTPRODUTO == "1" || produto.CTPRODUTO == "4"))
            {
                var idEstoqueProduto = _dbSession.Connection.Query<long>($@"SELECT IDESTOQUE_PROD FROM estoque_prod WHERE IDESTOQUE = {idEstoque} AND IDPRODUTO = {idProduto}",null,_dbSession.Transaction).FirstOrDefault();
                if (idEstoqueProduto > 0)
                {
                    AtualizarEstoqueProdutoTransacao(1, idEstoqueProduto, Quantidade);
                }
                else
                {
                    idEstoqueProduto = await _utilDapperRepository.GerarUUID();
                    IncluirEstoqueProduto(idEstoqueProduto, idEstoque, idProduto, Quantidade);
                }

                idEstoqueHistorico = IncluirEstoqueHistorico(idEstoque, idProduto, idItem, UsuarioHistorico, tpHistoricoEntrada, DescricaoHistorico, Quantidade);
            }
            return idEstoqueHistorico;
        }


        private void AtualizarEstoqueProdutoTransacao(int tpmov, long idEstoqueProduto, double quant)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDESTOQUE_PROD", idEstoqueProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NUQTD", quant, DbType.Double, ParameterDirection.Input);

            var query = "";
            if (tpmov == 1)
                query = $@"UPDATE estoque_prod SET NUQTD = @NUQTD  WHERE IDESTOQUE_PROD = @IDESTOQUE_PROD";
            else
                query = $@"UPDATE estoque_prod SET NUQTD = NUQTD - @NUQTD  WHERE IDESTOQUE_PROD = @IDESTOQUE_PROD";

            _dbSession.Connection.Execute(query, parametros,_dbSession.Transaction);
        }

        private long IncluirEstoqueHistorico(long idEstoque, long idProduto, long idItem, string usuario, int tipoHistorico,
         string descricaoHistorico, double quant)
        {
            var resultado = _utilDapperRepository.GerarUUID().Result;

            var parametros = new DynamicParameters();
            parametros.Add("@IDESTOQUEHST", resultado, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDESTOQUE", idEstoque, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPRODUTO", idProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@QTDHST", quant, DbType.Double, ParameterDirection.Input);
            parametros.Add("@IDITEM", idProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@DTHRHST", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@TPHST", tipoHistorico, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@DSHST", descricaoHistorico, DbType.String, ParameterDirection.Input);
            parametros.Add("@NMUSUARIO", usuario, DbType.String, ParameterDirection.Input);

            var query = $@"INSERT INTO estoquehst (IDESTOQUEHST, IDESTOQUE, IDPRODUTO,IDITEM, DTHRHST, NMUSUARIO, TPHST, DSHST, QTDHST)
                            VALUES (@IDESTOQUEHST, @IDESTOQUE, @IDPRODUTO, @IDITEM, @DTHRHST, @NMUSUARIO, @TPHST, @DSHST, @QTDHST)";

            _dbSession.Connection.Execute(query, parametros,_dbSession.Transaction);

            return resultado;
        }

        private void IncluirEstoqueProduto(long idEstoqueProduto, long idEstoque, long idProduto, double quant)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDESTOQUE_PROD", idEstoqueProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDESTOQUE", idEstoque, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPRODUTO", idProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NUQTD", quant, DbType.Double, ParameterDirection.Input);

            var query = $@"INSERT INTO estoque_prod (IDESTOQUE_PROD, IDESTOQUE, IDPRODUTO, NUQTD)
                            values (@IDESTOQUE_PROD, @IDESTOQUE, @IDPRODUTO, @NUQTD)";

            _dbSession.Connection.Execute(query, parametros,_dbSession.Transaction);
        }

        public async Task<long> RealizaRetiradaRetornaIdHistoricoGeradoTransacao(long idEstoque, long idProduto, long idItem, string UsuarioHistorico, string DescricaoHistorico, double Quantidade)
        {
            long idEstoqueHistorico = 0;
            var tpHistoricoEntrada = 1;


            var query = $@"SELECT IDPRODUTO as Id, IDEMPRESA, IDGRUPO, IDSUBGRUPO, IDDEP,IDMARCA, CDPRODUTO, NMPRODUTO, CTPRODUTO ,TPPRODUTO,UNCOMPRA, UNVENDA,
                                NURELACAO,NUPRECO,NUQTDMIN,CDSEFAZ,CDANP,CDNCM,CDCEST,CDSERV,STPRODUTO,VLULTIMACOMPRA,VLCUSTOMEDIO,PCIBPTFED,PCIBPTEST,PCIBPTMUN,
                                PCIBPTIMP,NUCFOP,STORIGEMPROD,DSICMS_CST,PCICMS_ALIQ,PCICMS_REDUCBC,PCICMSST_ALIQ,PCICMSST_MVA,PCICMSST_REDUCBC,DSIPI_CST,PCIPI_ALIQ,
                                DSPIS_CST,PCPIS_ALIQ,DSCOFINS_CST,PCCOFINS_ALIQ,STESTOQUE,STBALANCA,DSVOLUME
                            FROM produto WHERE IDPRODUTO ={idProduto}";
            var produto = _dbSession.Connection.Query<Produto>(query).FirstOrDefault();
            if (produto != null && (produto.CTPRODUTO == "1" || produto.CTPRODUTO == "4"))
            {
                var idEstoqueProduto = _dbSession.Connection.Query<long>($@"SELECT IDESTOQUE_PROD FROM estoque_prod WHERE IDESTOQUE = {idEstoque} AND IDPRODUTO = {idProduto}",
                                                                        null,_dbSession.Transaction).FirstOrDefault();
                if (idEstoqueProduto > 0)
                {
                    AtualizarEstoqueProdutoTransacao(2, idEstoqueProduto, Quantidade);
                }
                else
                {
                    idEstoqueProduto = _utilDapperRepository.GerarUUID().Result;
                    IncluirEstoqueProduto(idEstoqueProduto, idEstoque, idProduto, Quantidade * (-1));
                }

                idEstoqueHistorico = IncluirEstoqueHistorico(idEstoque, idProduto, idItem, UsuarioHistorico, tpHistoricoEntrada, DescricaoHistorico, Quantidade);
            }
            return idEstoqueHistorico;
         
        }

        public async Task<bool> DesvincularHistoricoDoLancamento(long idEstoqueHistorico)
        {
            var query = $@"UPDATE estoquehst SET IDLANC = NULL WHERE IDESTOQUEHST =@IDESTOQUEHST";

            var parametros = new DynamicParameters();
            parametros.Add("@IDESTOQUEHST", idEstoqueHistorico, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;

        }

        public async Task<List<EstoquePosicaoReport>> ObterPosicaoEstoque(long idEstoqueHistorico)
        {
            var query = $@"select e.IDESTOQUE as Id, e.idempresa,e.DSESTOQUE as descricao, e.TPESTOQUE as tipo, e.NUCAPACIDADE as capacidade, e.STESTOQUE
                        ,ep.NUQTD as quantidade,p.NMPRODUTO,p.CDPRODUTO,pg.NMGRUPO,coalesce(p.NUQTDMIN,0) NUQTDMIN,
                                  coalesce(p.VLCUSTOMEDIO,0) VLCUSTOMEDIO, coalesce((ep.NUQTD * p.VLCUSTOMEDIO),0) vlfinanc
                                   from estoque e
                                  inner join estoque_prod ep on e.IDESTOQUE = ep.IDESTOQUE
                                  inner join produto p on p.IDPRODUTO = ep.IDPRODUTO
                                  inner join prod_grupo pg on pg.IDGRUPO = p.IDGRUPO
                                    where e.IDESTOQUE={idEstoqueHistorico}
                                    order by p.NMPRODUTO";
            var resultado = _dbSession.Connection.Query<EstoquePosicaoReport>(query);
           
            return resultado.ToList();
        }

        #endregion

    }
}

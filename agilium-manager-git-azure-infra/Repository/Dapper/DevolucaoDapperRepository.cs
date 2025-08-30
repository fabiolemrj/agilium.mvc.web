using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn;
using agilium.api.business.Models.CustomReturn.ReportViewModel.VendaReportViewModel;
using agilium.api.infra.ViewModelDapper;
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
    public class DevolucaoDapperRepository : IDevolucaoDapperRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IDapperRepository _dapperRepository;
        private readonly IUtilDapperRepository _utilDapperRepository;
        private readonly DbSession _dbSession;

        public DevolucaoDapperRepository(IConfiguration configuration, IDapperRepository dapperRepository, IUtilDapperRepository utilDapperRepository,
            DbSession dbSession)
        {
            _configuration = configuration;
            _dapperRepository = dapperRepository;
            _dbSession = dbSession;
            _utilDapperRepository = utilDapperRepository;

        }

        public string GetConnection()
        {
            var autenticacaoUrl = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return autenticacaoUrl;
        }


        #region Realizar Devolucção
        public async Task<bool> RealizarDevolucao(long idDevolucao, string usuario)
        {
            var resultado = false;
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@IDDEV", idDevolucao, DbType.Int64, ParameterDirection.Input);

                var queryDevolucao = $@" SELECT IDDEV as Id, IDEMPRESA, IDVENDA, IDCLIENTE,IDMOTDEV,IDVALE,CDDEV,DTHRDEV,VLTOTALDEV,DSOBSDEV,STDEV FROM devolucao
                                                       WHERE IDDEV = @IDDEV";

                var devolucao = _dbSession.Connection.Query<Devolucao>(queryDevolucao, parametros, _dbSession.Transaction).FirstOrDefault();

                if (devolucao != null)
                {
                    var motivoDevolucaoParametros = new DynamicParameters();
                    motivoDevolucaoParametros.Add("@IDMOTDEV", devolucao.IDMOTDEV, DbType.Int64, ParameterDirection.Input);

                    var motivoDevolucao = "";
                    if (devolucao.IDMOTDEV.HasValue)
                        motivoDevolucao = _dbSession.Connection.Query<string>($"select dsmotdev from motivo_devolucao where IDMOTDEV = @IDMOTDEV", motivoDevolucaoParametros, _dbSession.Transaction).FirstOrDefault();

                    var motivoDevolucaoClienteParametros = new DynamicParameters();
                    motivoDevolucaoClienteParametros.Add("@idcliente", devolucao.IDCLIENTE, DbType.Int64, ParameterDirection.Input);

                    var clienteNome = "";
                    if (devolucao.IDCLIENTE.HasValue)
                        clienteNome = _dbSession.Connection.Query<string>($"select NMCLIENTE from cliente where idcliente= @idcliente", motivoDevolucaoClienteParametros, _dbSession.Transaction).FirstOrDefault();

                    var sqVenda = 0;
                    var sqCaixa = 0;
                    long idEstoque = 0;

                    var motivoVendaParametro = new DynamicParameters();
                    motivoVendaParametro.Add("@IDVENDA", devolucao.IDVENDA, DbType.Int64, ParameterDirection.Input);
                    var queryVenda = $@"select V.SQVENDA, C.SQCAIXA, P.IDESTOQUE from venda V 
                                                INNER JOIN caixa C ON V.IDCAIXA = C.IDCAIXA 
                                                INNER JOIN pdv P ON C.IDPDV = P.IDPDV
                                                WHERE V.IDVENDA = @IDVENDA";
                    var resultadoDinamico = _dbSession.Connection.Query<dynamic>(queryVenda, motivoVendaParametro, _dbSession.Transaction).FirstOrDefault();

                    if (resultadoDinamico != null)
                    {
                        sqVenda = Convert.ToInt32(resultadoDinamico.SQVENDA);
                        sqCaixa = Convert.ToInt32(resultadoDinamico.SQCAIXA);
                        idEstoque = Convert.ToInt64(resultadoDinamico.IDESTOQUE);
                    }

                    var queryDevolucaoItens = $@"SELECT IDDEV_ITEM as Id, IDDEV,IDVENDA_ITEM,NUQTD,VLITEM FROM devolucao_item WHERE IDDEV = {idDevolucao}";
                    var devolucaoItens = _dbSession.Connection.Query<DevolucaoItem>(queryDevolucaoItens);

                    devolucaoItens.ToList().ForEach(async item =>
                    {
                        var descricao = $@"'Entrada pela devolução nº {devolucao.CDDEV}, referente a venda nº {sqVenda} do caixa nº {sqCaixa}";

                        var produto = ObterProduto(item.IDVENDA_ITEM.Value).Result;

                        if (RealizaEntradaRetornaIdHistoricoGerado(idEstoque, produto.Id, -1, usuario, descricao, item.NUQTD.Value).Result)
                        {
                            await AtualizarItemVenda(item.IDVENDA_ITEM.Value, ESituacaoItemVenda.Devolvido);
                        }
                    });
                }
                await AtualizarDevolucao(idDevolucao, ESituacaoDevolucao.Realizada);
                resultado = true;

                return resultado;
            }
            catch (Exception)
            {
                return resultado;
            }

        }
        #endregion

        #region Obter Itens Devolucao com Venda Itens

        public async Task<List<DevolucaoItemVendaCustom>> ObterItensComVendaItens(long idVenda, long idDevolucao)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDVENDA", idVenda, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDDEV", idDevolucao, DbType.Int64, ParameterDirection.Input);

            var query = $@" SELECT VI.IDVENDA_ITEM as idItemVenda, VI.IDPRODUTO as idProduto, VI.SQITEM as SeqVenda, 
                                          VI.NUQTD as QuantidadeVendida, VI.VLTOTAL as ValorTotal, P.NMPRODUTO as ProdutoNome, 
                                          COALESCE(DI.IDDEV_ITEM, -1) AS idDevolucaoItem, 
                                          COALESCE(DI.NUQTD, 0) AS QuantidadeDevolucao, 
                                          COALESCE(DI.VLITEM, 0) AS ValorDevolucao,
                                          COALESCE(DI.IDDEV, 0) AS idDevolucao  
                                          FROM venda_item VI
                                          INNER JOIN produto P ON VI.IDPRODUTO = P.IDPRODUTO
                                          LEFT JOIN devolucao_item DI ON VI.IDVENDA_ITEM = DI.IDVENDA_ITEM
                                             AND DI.IDDEV = @IDDEV
                                          WHERE
                                          VI.IDVENDA = @IDVENDA
                                          AND VI.STITEM = 1
                                          ORDER BY VI.SQITEM";

            
            var resultado = _dbSession.Connection.Query<DevolucaoItemVendaCustom>(query, parametros, _dbSession.Transaction).ToList();
            resultado.ToList().ForEach(item => {
                item.selecionado = item.idDevolucaoItem > 0;
            });
            return resultado.ToList();
        }

        #endregion


        #region private
        private async Task<bool> RealizaEntradaRetornaIdHistoricoGerado(long idEstoque, long idProduto, long idItem, string UsuarioHistorico, string DescricaoHistorico, double Quantidade)
        {
            long idEstoqueHistorico = 0;
            var tpHistoricoEntrada = 1;
                var parametros = new DynamicParameters();
                parametros.Add("@idProduto", idProduto, DbType.Int64, ParameterDirection.Input);

                var query = $@"SELECT IDPRODUTO as Id, IDEMPRESA, IDGRUPO, IDSUBGRUPO, IDDEP,IDMARCA, CDPRODUTO, NMPRODUTO, CTPRODUTO ,TPPRODUTO,UNCOMPRA, UNVENDA,
                                NURELACAO,NUPRECO,NUQTDMIN,CDSEFAZ,CDANP,CDNCM,CDCEST,CDSERV,STPRODUTO,VLULTIMACOMPRA,VLCUSTOMEDIO,PCIBPTFED,PCIBPTEST,PCIBPTMUN,
                                PCIBPTIMP,NUCFOP,STORIGEMPROD,DSICMS_CST,PCICMS_ALIQ,PCICMS_REDUCBC,PCICMSST_ALIQ,PCICMSST_MVA,PCICMSST_REDUCBC,DSIPI_CST,PCIPI_ALIQ,
                                DSPIS_CST,PCPIS_ALIQ,DSCOFINS_CST,PCCOFINS_ALIQ,STESTOQUE,STBALANCA,DSVOLUME
                            FROM produto WHERE IDPRODUTO =@IDPRODUTO";
            var produto = _dbSession.Connection.Query<Produto>(query, parametros, _dbSession.Transaction).FirstOrDefault();

            if (produto != null && (produto.CTPRODUTO == "1" || produto.CTPRODUTO == "4"))
            {
                var idEstoqueProduto = _dbSession.Connection.Query<long>($@"SELECT IDESTOQUE_PROD FROM estoque_prod WHERE IDESTOQUE = {idEstoque} AND IDPRODUTO = {idProduto}").FirstOrDefault();
                if (idEstoqueProduto > 0)
                {
                    AtualizarEstoqueProduto(1, idEstoqueProduto, Quantidade);
                }
                else
                {
                    idEstoqueProduto = _utilDapperRepository.GerarUUID().Result;
                    IncluirEstoqueProduto(idEstoqueProduto, idEstoque, idProduto, Quantidade);
                }

                idEstoqueHistorico = IncluirEstoqueHistorico(idEstoque, idProduto, idItem, UsuarioHistorico, tpHistoricoEntrada, DescricaoHistorico, Quantidade);
            }
            return idEstoqueHistorico > 0;
        }

        private async Task<Produto> ObterProduto(long idVendaItem)
        {
                var parametros = new DynamicParameters();
                parametros.Add("@IDVENDA_ITEM", idVendaItem, DbType.Int64, ParameterDirection.Input);

                var query = @$"select p.IDPRODUTO as Id, p.NMPRODUTO from produto p
                            inner join venda_item vi on vi.IDPRODUTO = p.IDPRODUTO
                            inner join devolucao_item di on di.IDVENDA_ITEM = vi.IDVENDA_ITEM 
                            where vi.IDVENDA_ITEM = @IDVENDA_ITEM";
            return _dbSession.Connection.Query<Produto>(query,parametros,_dbSession.Transaction).FirstOrDefault();
        }

        //private async Task<long> GerarUUID(MySqlConnection con)
        //{
        //    var query = $@"SELECT uuid_short() AS ID";

        //    return con.Query<long>(query).FirstOrDefault();
        //}

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

        private void AtualizarEstoqueProduto(int tpmov, long idEstoqueProduto, double quant)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDESTOQUE_PROD", idEstoqueProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NUQTD", quant, DbType.Double, ParameterDirection.Input);

            var query = "";
            if (tpmov == 1)
                query = $@"UPDATE estoque_prod SET NUQTD = NUQTD + @NUQTD  WHERE IDESTOQUE_PROD = @IDESTOQUE_PROD";
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
            parametros.Add("@IDITEM", idItem, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@DTHRHST", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@TPHST", tipoHistorico, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@DSHST", descricaoHistorico, DbType.String, ParameterDirection.Input);
            parametros.Add("@NMUSUARIO", usuario, DbType.String, ParameterDirection.Input);

            var query = $@"INSERT INTO estoquehst (IDESTOQUEHST, IDESTOQUE, IDPRODUTO,IDITEM, DTHRHST, NMUSUARIO, TPHST, DSHST, QTDHST)
                            VALUES (@IDESTOQUEHST, @IDESTOQUE, @IDPRODUTO, @IDITEM, @DTHRHST, @NMUSUARIO, @TPHST, @DSHST, @QTDHST)";

                _dbSession.Connection.Execute(query, parametros,_dbSession.Transaction);

            return resultado;
        }

        private async Task<bool> AtualizarItemVenda(long idVendaItem, ESituacaoItemVenda situacaoVenda)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDVENDA_ITEM", idVendaItem, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@STITEM", situacaoVenda, DbType.Int32, ParameterDirection.Input);
            
            var query = $@"UPDATE venda_item SET STITEM = @STITEM WHERE IDVENDA_ITEM = @IDVENDA_ITEM";

            return _dbSession.Connection.Execute(query, parametros,_dbSession.Transaction) > 0;
        }

        private async Task<bool> AtualizarDevolucao(long idDevolucao, ESituacaoDevolucao situacaoVenda)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDDEV", idDevolucao, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@STDEV", situacaoVenda, DbType.Int32, ParameterDirection.Input);

            var query = $@"UPDATE devolucao SET STDEV = @STDEV WHERE IDDEV = @IDDEV";

            return _dbSession.Connection.Execute(query, parametros,_dbSession.Transaction) > 0;
        }

        #endregion
    }
}

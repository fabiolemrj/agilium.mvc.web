using agilium.api.business.Enums;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn;
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

        public DevolucaoDapperRepository(IConfiguration configuration)
        {
            _configuration = configuration;
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

            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();

                        var queryDevolucao = $@" SELECT IDDEV as Id, IDEMPRESA, IDVENDA, IDCLIENTE,IDMOTDEV,IDVALE,CDDEV,DTHRDEV,VLTOTALDEV,DSOBSDEV,STDEV FROM devolucao
                                                   WHERE IDDEV = {idDevolucao}";
                        var devolucao = con.Query<Devolucao>(queryDevolucao).FirstOrDefault();
                        if(devolucao != null)
                        {
                            var motivoDevolucao = "";
                            if (devolucao.IDMOTDEV.HasValue)
                                motivoDevolucao = con.Query<string>($"select dsmotdev from motivo_devolucao where IDMOTDEV = {devolucao.IDMOTDEV}").FirstOrDefault();
                            
                            var clienteNome = "";
                            if(devolucao.IDCLIENTE.HasValue)
                                clienteNome = con.Query<string>($"select NMCLIENTE from cliente where idcliente= {devolucao.IDCLIENTE}").FirstOrDefault();

                            var sqVenda = 0;
                            var sqCaixa = 0;
                            long idEstoque = 0;

                            var queryVenda = $@"select V.SQVENDA, C.SQCAIXA, P.IDESTOQUE from venda V 
                                                INNER JOIN caixa C ON V.IDCAIXA = C.IDCAIXA 
                                                INNER JOIN pdv P ON C.IDPDV = P.IDPDV
                                                WHERE V.IDVENDA = {devolucao.IDVENDA}";
                            var resultadoDinamico = con.Query<dynamic>(queryVenda).FirstOrDefault();
                            if(resultadoDinamico != null)
                            {
                                sqVenda = Convert.ToInt32(resultadoDinamico.SQVENDA);
                                sqCaixa = Convert.ToInt32(resultadoDinamico.SQCAIXA);
                                idEstoque = Convert.ToInt64(resultadoDinamico.IDESTOQUE);
                            }

                            var queryDevolucaoItens = $@"SELECT IDDEV_ITEM as Id, IDDEV,IDVENDA_ITEM,NUQTD,VLITEM FROM devolucao_item WHERE IDDEV = {idDevolucao}";
                            var devolucaoItens = con.Query<DevolucaoItem>(queryDevolucaoItens);
                            devolucaoItens.ToList().ForEach(async item => {
                                var descricao = $@"'Entrada pela devolução nº {devolucao.CDDEV}, referente a venda nº {sqVenda} do caixa nº {sqCaixa}";

                                var produto = ObterProduto(item.IDVENDA_ITEM.Value, con).Result;

                                if(RealizaEntradaRetornaIdHistoricoGerado(idEstoque, produto.Id, -1, usuario, descricao, item.NUQTD.Value, con).Result)
                                {
                                    await AtualizarItemVenda(item.IDVENDA_ITEM.Value, ESituacaoItemVenda.Devolvido, con);
                                }
                            });

                            await AtualizarDevolucao(idDevolucao,ESituacaoDevolucao.Realizada,con);
                        }

                        scope.Complete();
                        resultado = true;
                       
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        con.Dispose();
                    }
                    return resultado;
                }
            }
        }
        #endregion

        #region Obter Itens Devolucao com Venda Itens

        public async Task<List<DevolucaoItemVendaCustom>> ObterItensComVendaItens(long idVenda, long idDevolucao)
        {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();

                        var query = $@" SELECT VI.IDVENDA_ITEM as idItemVenda, VI.IDPRODUTO as idProduto, VI.SQITEM as SeqVenda, 
                                          VI.NUQTD as QuantidadeVendida, VI.VLTOTAL as ValorTotal, P.NMPRODUTO as ProdutoNome, 
                                          COALESCE(DI.IDDEV_ITEM, -1) AS idDevolucaoItem, 
                                          COALESCE(DI.NUQTD, 0) AS QuantidadeDevolucao, 
                                          COALESCE(DI.VLITEM, 0) AS ValorDevolucao,
                                          COALESCE(DI.IDDEV, 0) AS idDevolucao  
                                          FROM venda_item VI
                                          INNER JOIN produto P ON VI.IDPRODUTO = P.IDPRODUTO
                                          LEFT JOIN devolucao_item DI ON VI.IDVENDA_ITEM = DI.IDVENDA_ITEM
                                             AND DI.IDDEV = {idDevolucao}
                                          WHERE
                                          VI.IDVENDA = {idVenda}
                                          AND VI.STITEM = 1
                                          ORDER BY VI.SQITEM";

                        var resulta = con.Query<DevolucaoItemVendaCustom>(query);
                    resulta.ToList().ForEach(item => {
                        item.selecionado = item.idDevolucaoItem > 0;
                    });
                    return resulta.ToList();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        con.Dispose();
                    }
                }
        }

        #endregion


        #region private
        private async Task<bool> RealizaEntradaRetornaIdHistoricoGerado(long idEstoque, long idProduto, long idItem, string UsuarioHistorico, string DescricaoHistorico, double Quantidade, MySqlConnection con)
        {
            long idEstoqueHistorico = 0;
            var tpHistoricoEntrada = 1;

            var query = $@"SELECT IDPRODUTO as Id, IDEMPRESA, IDGRUPO, IDSUBGRUPO, IDDEP,IDMARCA, CDPRODUTO, NMPRODUTO, CTPRODUTO ,TPPRODUTO,UNCOMPRA, UNVENDA,
                                NURELACAO,NUPRECO,NUQTDMIN,CDSEFAZ,CDANP,CDNCM,CDCEST,CDSERV,STPRODUTO,VLULTIMACOMPRA,VLCUSTOMEDIO,PCIBPTFED,PCIBPTEST,PCIBPTMUN,
                                PCIBPTIMP,NUCFOP,STORIGEMPROD,DSICMS_CST,PCICMS_ALIQ,PCICMS_REDUCBC,PCICMSST_ALIQ,PCICMSST_MVA,PCICMSST_REDUCBC,DSIPI_CST,PCIPI_ALIQ,
                                DSPIS_CST,PCPIS_ALIQ,DSCOFINS_CST,PCCOFINS_ALIQ,STESTOQUE,STBALANCA,DSVOLUME
                            FROM produto WHERE IDPRODUTO ={idProduto}";
            var produto = con.Query<Produto>(query).FirstOrDefault();
            if (produto != null && (produto.CTPRODUTO == "1" || produto.CTPRODUTO == "4"))
            {
                var idEstoqueProduto = con.Query<long>($@"SELECT IDESTOQUE_PROD FROM estoque_prod WHERE IDESTOQUE = {idEstoque} AND IDPRODUTO = {idProduto}").FirstOrDefault();
                if (idEstoqueProduto > 0)
                {
                    AtualizarEstoqueProduto(1, idEstoqueProduto, Quantidade, con);
                }
                else
                {
                    idEstoqueProduto = GerarUUID(con).Result;
                    IncluirEstoqueProduto(idEstoqueProduto, idEstoque, idProduto, Quantidade, con);
                }

                idEstoqueHistorico = IncluirEstoqueHistorico(idEstoque, idProduto, idItem, UsuarioHistorico, tpHistoricoEntrada, DescricaoHistorico, Quantidade, con);
            }
            return idEstoqueHistorico > 0;
        }

        private async Task<Produto> ObterProduto(long idVendaItem, MySqlConnection con)
        {
            var query = @$"select p.IDPRODUTO as Id, p.NMPRODUTO from produto p
                            inner join venda_item vi on vi.IDPRODUTO = p.IDPRODUTO
                            inner join devolucao_item di on di.IDVENDA_ITEM = vi.IDVENDA_ITEM 
                            where vi.IDVENDA_ITEM = {idVendaItem}";
            return con.Query<Produto>(query).FirstOrDefault();
        }

        private async Task<long> GerarUUID(MySqlConnection con)
        {
            var query = $@"SELECT uuid_short() AS ID";

            return con.Query<long>(query).FirstOrDefault();
        }

        private void IncluirEstoqueProduto(long idEstoqueProduto, long idEstoque, long idProduto, double quant, MySqlConnection con)
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

        private void AtualizarEstoqueProduto(int tpmov, long idEstoqueProduto, double quant, MySqlConnection con)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDESTOQUE_PROD", idEstoqueProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NUQTD", quant, DbType.Double, ParameterDirection.Input);

            var query = "";
            if (tpmov == 1)
                query = $@"UPDATE estoque_prod SET NUQTD = NUQTD + @NUQTD  WHERE IDESTOQUE_PROD = @IDESTOQUE_PROD";
            else
                query = $@"UPDATE estoque_prod SET NUQTD = NUQTD - @NUQTD  WHERE IDESTOQUE_PROD = @IDESTOQUE_PROD";


            con.Execute(query, parametros);
        }

        private long IncluirEstoqueHistorico(long idEstoque, long idProduto, long idItem, string usuario, int tipoHistorico,
            string descricaoHistorico, double quant, MySqlConnection con)
        {
            var resultado = GerarUUID(con).Result;

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

            con.Execute(query, parametros);

            return resultado;
        }

        private async Task<bool> AtualizarItemVenda(long idVendaItem, ESituacaoItemVenda situacaoVenda, MySqlConnection con)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDVENDA_ITEM", idVendaItem, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@STITEM", situacaoVenda, DbType.Int32, ParameterDirection.Input);
            
            var query = $@"UPDATE venda_item SET STITEM = @STITEM WHERE IDVENDA_ITEM = @IDVENDA_ITEM";


            return con.Execute(query, parametros) > 0;
        }

        private async Task<bool> AtualizarDevolucao(long idDevolucao, ESituacaoDevolucao situacaoVenda, MySqlConnection con)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDDEV", idDevolucao, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@STDEV", situacaoVenda, DbType.Int32, ParameterDirection.Input);

            var query = $@"UPDATE devolucao SET STDEV = @STDEV WHERE IDDEV = @IDDEV";

            return con.Execute(query, parametros) > 0;
        }

        #endregion
    }
}

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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace agilium.api.infra.Repository.Dapper
{
    public class ProdutoDapper : IProdutoDapper
    {
        protected readonly IConfiguration _configuration;
        private readonly DbSession _dbSession;
        private readonly IUtilDapperRepository _utilDapperRepository;

        public ProdutoDapper(IConfiguration configuration, DbSession dbSession, IUtilDapperRepository utilDapperRepository)
        {
            _configuration = configuration;
            _dbSession = dbSession;
            _utilDapperRepository = utilDapperRepository;
        }

        public string GetConnection()
        {
            var autenticacaoUrl = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return autenticacaoUrl;
        }

        #region Atualizar IBPT
        public async Task AtualizarIBPTTodosProdutos()
        {
            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();
                        var query = $@"SELECT IDPRODUTO as Id,CDNCM,CDPRODUTO,NMPRODUTO  FROM produto WHERE STPRODUTO = 1";
                        var produtos = con.Query<Produto>(query).ToList();



                        produtos.ForEach(async prod =>
                        {
                            await AtualizarIBPTPorProduto(prod, con);
                        });

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally { con.Close(); }

                };

            }
        }

        private async Task AtualizarIBPTPorProduto(Produto produto, MySqlConnection con)
        {
            var resultado = false;

            if (!string.IsNullOrEmpty(produto.CDNCM))
            {
                //Se por acaso não encontrar o NCM com o código completo
                //deve-se ir reduzindo o código até 1 caracter para poder localizar
                var ncm = produto.CDNCM.Trim();

                while ((ncm.Length > 1) && (!resultado))
                {
                    var queryIBPT = $"SELECT * FROM ibpt WHERE NCM = '{ncm}' ORDER BY FIMVIG DESC";
                    var ibpt = con.Query<Ibpt>(queryIBPT).FirstOrDefault();
                    if (ibpt != null)
                    {
                        resultado = true;
                        var municipal = ibpt.MUNICIPAL.HasValue ? ibpt.MUNICIPAL.Value : 0;
                        var importadoFederal = ibpt.IMPORTADOSFEDERAL.HasValue ? ibpt.IMPORTADOSFEDERAL.Value : 0;
                        var nacionalFederal = ibpt.NACIONALFEDERAL.HasValue ? ibpt.NACIONALFEDERAL.Value : 0;
                        var estadual = ibpt.ESTADUAL.HasValue ? ibpt.ESTADUAL.Value : 0;

                        var parametros = new DynamicParameters();
                        parametros.Add("@PCIBPTFED", nacionalFederal, DbType.Double, ParameterDirection.Input);
                        parametros.Add("@PCIBPTEST", estadual, DbType.Double, ParameterDirection.Input);
                        parametros.Add("@PCIBPTMUN", municipal, DbType.Double, ParameterDirection.Input);
                        parametros.Add("@PCIBPTIMP", importadoFederal, DbType.Double, ParameterDirection.Input);
                        parametros.Add("@IDPRODUTO", produto.Id, DbType.Int64, ParameterDirection.Input);
                        //Só atualizo se estiver em vigência
                        if (ibpt.FIMVIG.HasValue && ibpt.FIMVIG.Value >= DateTime.Now)
                        {
                            var queryAtualizarProduto = @$"UPDATE produto SET PCIBPTFED = @PCIBPTFED, PCIBPTEST = @PCIBPTEST,
                                                            PCIBPTMUN = @PCIBPTMUN, PCIBPTIMP = @PCIBPTIMP
                                                            WHERE IDPRODUTO = @IDPRODUTO";
                            con.Execute(queryAtualizarProduto, parametros);
                        }
                    }
                    else
                    {
                        //Reduz 1 caracter no final do NCM
                        ncm = ncm.Substring(0, ncm.Length - 1);
                    }
                }
            }
        }

        private string ConverterValor(string valor) => double.Parse(valor, CultureInfo.InvariantCulture).ToString(CultureInfo.CurrentCulture);


        public async Task<Produto> ObterProdutoPorCodigoEan(string ean)
        {
            return await ObterProdutoPorCodigoEanTransacao(ean);
            //using (var con = new MySqlConnection(GetConnection()))
            //{
            //    try
            //    {
            //        con.Open();
            //        var parametros = new DynamicParameters();
            //        parametros.Add("@CDBARRA", ean, DbType.String, ParameterDirection.Input);

            //        var query = $@"SELECT p.IDPRODUTO as Id, p.* FROM prod_barra pb 
            //                    inner join produto p on p.IDPRODUTO = pb.IDPRODUTO
            //                    WHERE CDBARRA = @CDBARRA";

            //        return con.Query<Produto>(query, parametros).FirstOrDefault();
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //    finally { con.Close(); }
            //}
        }

        public async Task<Produto> ObterProdutoPorId(long id)
        {
            var query = $@"SELECT p.IDPRODUTO as Id, p.* FROM produto p where IDPRODUTO = {id}";
            return _dbSession.Connection.Query<Produto>(query, null, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<Produto> ObterProdutoPorCompraAnterior(long idFornecedor, string codigoFornecedor)
        {
            return await ObterProdutoPorCompraAnteriorTransacao(idFornecedor, codigoFornecedor);

            //using (var con = new MySqlConnection(GetConnection()))
            //{
            //    try
            //    {
            //        con.Open();
            //        var parametros = new DynamicParameters();
            //        parametros.Add("@CDPRODFORN", codigoFornecedor, DbType.String, ParameterDirection.Input);
            //        parametros.Add("@IDFORN", idFornecedor, DbType.Int64, ParameterDirection.Input);

            //        var query = $@"SELECT CI.IDPRODUTO,CI.CDPRODFORN,C.IDFORN   
            //                        FROM compra_item CI
            //                        INNER JOIN compra C ON CI.IDCOMPRA = C.IDCOMPRA 
            //                        WHERE CI.IDPRODUTO IS NOT NULL 
            //                        AND CI.CDPRODFORN = @CDPRODFORN
            //                        AND C.IDFORN = @IDFORN 
            //                        ORDER BY C.DTCOMPRA 
            //                        DESC LIMIT 1,1";

            //        return con.Query<Produto>(query, parametros).FirstOrDefault();
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //    finally { con.Close(); }
            //}
        }


        #endregion

        #region Transacao
        private async Task<Produto> ObterProdutoPorCompraAnteriorTransacao(long idFornecedor, string codigoFornecedor)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@CDPRODFORN", codigoFornecedor, DbType.String, ParameterDirection.Input);
            parametros.Add("@IDFORN", idFornecedor, DbType.Int64, ParameterDirection.Input);

            var query = $@"SELECT CI.IDPRODUTO,CI.CDPRODFORN,C.IDFORN   
                                    FROM compra_item CI
                                    INNER JOIN compra C ON CI.IDCOMPRA = C.IDCOMPRA 
                                    WHERE CI.IDPRODUTO IS NOT NULL 
                                    AND CI.CDPRODFORN = @CDPRODFORN
                                    AND C.IDFORN = @IDFORN 
                                    ORDER BY C.DTCOMPRA 
                                    DESC LIMIT 1,1";

            return _dbSession.Connection.Query<Produto>(query, parametros,_dbSession.Transaction).FirstOrDefault();
        }
        private async Task<Produto> ObterProdutoPorCodigoEanTransacao(string ean)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@CDBARRA", ean, DbType.String, ParameterDirection.Input);

            var query = $@"SELECT p.IDPRODUTO as Id, p.* FROM prod_barra pb 
                        inner join produto p on p.IDPRODUTO = pb.IDPRODUTO
                        WHERE CDBARRA = @CDBARRA";

            return _dbSession.Connection.Query<Produto>(query, parametros,_dbSession.Transaction).FirstOrDefault();
            
        }

        public async Task<double> AtualizarCustoMedio(long idProduto, double quantidadeEntrada, double valorCompra)
        {
         
            var queryQuantEstoque = $@"SELECT COALESCE(SUM(NUQTD), 0) QTDESTOQUE FROM estoque_prod WHERE IDPRODUTO = {idProduto}";

            var quantidadeEstoque = _dbSession.Connection.Query<double>(queryQuantEstoque, null, _dbSession.Transaction).FirstOrDefault();

            var queryValorCustoMedio = $@"SELECT COALESCE(VLCUSTOMEDIO, 0) VLCUSTOMEDIO FROM produto WHERE IDPRODUTO = {idProduto}";
            
            var ValorCustoMedioAtual = _dbSession.Connection.Query<double>(queryValorCustoMedio, null, _dbSession.Transaction).FirstOrDefault();

            var novoValorCustoMedio = ((quantidadeEstoque * ValorCustoMedioAtual) + (quantidadeEntrada* valorCompra)) / (quantidadeEntrada+ quantidadeEstoque);
            var queryAtualizaCustoMedio = $"UPDATE produto SET VLCUSTOMEDIO = @VLCUSTOMEDIO WHERE IDPRODUTO = @IDPRODUTO";

            var parametros = new DynamicParameters();
            parametros.Add("@VLCUSTOMEDIO", novoValorCustoMedio, DbType.Double, ParameterDirection.Input);
            parametros.Add("@IDPRODUTO", idProduto, DbType.Int64, ParameterDirection.Input);
            var resultado = _dbSession.Connection.Execute(queryValorCustoMedio, parametros, _dbSession.Transaction) > 0;
            
            if (resultado)
                return novoValorCustoMedio;
            else
                return 0;

        }

        public async Task<bool> AtualizarUltimoValorCompra(long idProduto, double valorCompra)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@VLULTIMACOMPRA", valorCompra, DbType.Double, ParameterDirection.Input);
            parametros.Add("@IDPRODUTO", idProduto, DbType.Int64, ParameterDirection.Input);
            var query = $"UPDATE produto SET VLULTIMACOMPRA = @VLULTIMACOMPRA WHERE IDPRODUTO = @IDPRODUTO";
            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;


        }

        public async Task<bool> AtualizarPrecoVenda(long idProduto, double novoValorVenda)
        {
            var precoAtualVenda = _dbSession.Connection.Query<double>($@"SELECT NUPRECO FROM produto WHERE IDPRODUTO = {idProduto}", null, _dbSession.Transaction).FirstOrDefault();
            var resultado = false;

            if(precoAtualVenda != novoValorVenda)
            {
                var idPrecoVenda = _utilDapperRepository.GerarUUID().Result;
                var queryAtualizaCustoMedio = $"INSERT INTO prod_preco(IDPROD_PRECO, IDPRODUTO, USUARIO, NUPRECO_NEW,NUPRECO_OLD, DTPROD_PRECO) values(@IDPROD_PRECO, @IDPRODUTO, @USUARIO, @NUPRECO_NEW,@NUPRECO_OLD, @DTPROD_PRECO)";

                var parametros = new DynamicParameters();
                parametros.Add("@IDPROD_PRECO", idPrecoVenda, DbType.Double, ParameterDirection.Input);
                parametros.Add("@IDPRODUTO", idProduto, DbType.Int64, ParameterDirection.Input);
                parametros.Add("@USUARIO", idProduto, DbType.Int64, ParameterDirection.Input);
                parametros.Add("@NUPRECO_NEW", novoValorVenda, DbType.Int64, ParameterDirection.Input);
                parametros.Add("@NUPRECO_OLD", precoAtualVenda, DbType.Int64, ParameterDirection.Input);
                parametros.Add("@DTPROD_PRECO", DateTime.Now, DbType.Int64, ParameterDirection.Input);

                resultado = _dbSession.Connection.Execute(queryAtualizaCustoMedio, parametros, _dbSession.Transaction) > 0;

                if (resultado)
                {
                    var parametrosAtualiza = new DynamicParameters();
                    parametrosAtualiza.Add("@NUPRECO ", novoValorVenda, DbType.Double, ParameterDirection.Input);
                    parametrosAtualiza.Add("@IDPRODUTO", idProduto, DbType.Int64, ParameterDirection.Input);

                    var queryAtualizaProduto = $"UPDATE produto SET NUPRECO = @NUPRECO WHERE IDPRODUTO = @IDPRODUTO";
                    resultado = _dbSession.Connection.Execute(queryAtualizaCustoMedio, parametrosAtualiza, _dbSession.Transaction) > 0;
                }
            }

            return resultado;
        }

        public async Task<bool> InsereProdutoCodigoBarra(long idProduto, string cdBarra)
        {
            var queryAtualizaCustoMedio = $"INSERT INTO prod_barra (IDPROD_BARRA, IDPRODUTO, CDBARRA) VALUES (@IDPROD_BARRA, @IDPRODUTO, @CDBARRA)";
            var parametros = new DynamicParameters();
            parametros.Add("@IDPROD_BARRA", idProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPRODUTO", idProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@CDBARRA", cdBarra, DbType.String, ParameterDirection.Input);

            return _dbSession.Connection.Execute(queryAtualizaCustoMedio, parametros, _dbSession.Transaction) > 0;

        }

        public async Task<long> InsereProdutoPendente(string NMPRODUTO, string UNCOMPRA, string CDNCM, string CDCEST, double NURELACAO, double NUPRECO, long idEmpresa)
        {
            var queryCampos = $"INSERT INTO produto (IDPRODUTO, IDEMPRESA, CDPRODUTO, NMPRODUTO, CTPRODUTO, TPPRODUTO, UNCOMPRA, NURELACAO, CDNCM, CDCEST, STPRODUTO, STESTOQUE";
                
            var queryValores = ") values (@IDPRODUTO, @IDEMPRESA, @CDPRODUTO, @NMPRODUTO, @CTPRODUTO, @TPPRODUTO, @UNCOMPRA, @NURELACAO, @CDNCM, @CDCEST, @STPRODUTO, @STESTOQUE";

            //const STESTOQUE_RETORNA = 1;
            //const STESTOQUE_NAORETORNA = 0;

            long idProduto = _utilDapperRepository.GerarUUID().Result;
            var codigo = _utilDapperRepository.GerarCodigo("SELECT MAX(CAST(CDPRODUTO AS UNSIGNED)) AS CD FROM produto").Result;

            var parametros = new DynamicParameters();
            parametros.Add("@IDPRODUTO", idProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDEMPRESA", idEmpresa, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@CDPRODUTO", codigo, DbType.String, ParameterDirection.Input);
            parametros.Add("@NMPRODUTO", NMPRODUTO, DbType.String, ParameterDirection.Input);
            parametros.Add("@CTPRODUTO", ECategoriaProduto.Simples, DbType.String, ParameterDirection.Input);
            parametros.Add("@TPPRODUTO", ETipoProduto.Mercadoria, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@UNCOMPRA", UNCOMPRA, DbType.String, ParameterDirection.Input);
            parametros.Add("@CDNCM", CDNCM, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@CDCEST", CDCEST, DbType.String, ParameterDirection.Input);
            parametros.Add("@STPRODUTO", EAtivo.Ativo, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@STESTOQUE", 1, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@NURELACAO", NURELACAO, DbType.Double, ParameterDirection.Input);

            if (NUPRECO > 0)
            {
                queryCampos += ", NUPRECO";
                queryValores += ",@NUPRECO";
                parametros.Add("@NUPRECO", NUPRECO, DbType.Double, ParameterDirection.Input);
            }

            var query = $"{queryCampos} {queryValores} )";

            if (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0)
                return idProduto;
            else 
                return 0;

        }

        #endregion

    }
}

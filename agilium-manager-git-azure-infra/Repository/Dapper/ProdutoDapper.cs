using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<List<Produto>> ObterProdutosParaAtualizarIbpt()
        {
            var query = $@"SELECT IDPRODUTO as Id,CDNCM,CDPRODUTO,NMPRODUTO  FROM produto WHERE STPRODUTO = 1";
            return _dbSession.Connection.QueryAsync<Produto>(query, null, _dbSession.Transaction).Result.ToList();
        }

        private async Task<Ibpt> ObterIbptPorCodCnm(string cdncm)
        {
            var query = $@"SELECT * FROM ibpt WHERE NCM = '{{ncm}}' ORDER BY FIMVIG DESC";
            return _dbSession.Connection.QueryAsync<Ibpt>(query, null, _dbSession.Transaction).Result.FirstOrDefault();
        }

        public async Task AtualizarIBPTPorProduto(Produto produto)
        {
            var resultado = false;

            if (!string.IsNullOrEmpty(produto.CDNCM))
            {
                //Se por acaso não encontrar o NCM com o código completo
                //deve-se ir reduzindo o código até 1 caracter para poder localizar
                var ncm = produto.CDNCM.Trim();

                while ((ncm.Length > 1) && (!resultado))
                {
                    var ibpt = await ObterIbptPorCodCnm(ncm);
                    
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
                            _dbSession.Connection.Execute(queryAtualizarProduto, parametros);
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

        }

        public async Task<List<Produto>> ObterTodosProdutos_IdDescricao(long idEmpresa)
        {
            var query = $@"SELECT p.IDPRODUTO as Id, p.NMPRODUTO FROM produto p where p.IDEMPRESA = @IDEMPRESA";
            var parametros = new DynamicParameters();
            parametros.Add("@IDEMPRESA", idEmpresa, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.QueryAsync<Produto>(query, parametros, _dbSession.Transaction).Result.ToList();
        }


        public async Task<List<Produto>> BuscarProdutosJson(long idEmpresa, string filtro)
        {
            var query = $@"SELECT p.IDPRODUTO as Id, p.NMPRODUTO FROM produto p where p.IDEMPRESA = @IDEMPRESA AND p.NMPRODUTO LIKE @NMPRODUTO";
            var parametros = new DynamicParameters();
            parametros.Add("@IDEMPRESA", idEmpresa, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NMPRODUTO", $"%{filtro}%", DbType.String, ParameterDirection.Input);

            var produtos = _dbSession.Connection.Query<Produto>(query, parametros, _dbSession.Transaction);
                
            return produtos.Where(p => p.NMPRODUTO.Contains(filtro)).Take(50).ToList();
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

        /// <summary>
        /// Atualiza produto via Dapper comparando com valores existentes no banco.
        /// Apenas colunas com valores diferentes são incluídas no UPDATE.
        /// </summary>
        public async Task<bool> AtualizarProduto(Produto produto)
        {
            // 1. Busca produto existente no banco para comparar
            var existente = await ObterProdutoPorId(produto.Id);
            if (existente == null)
                return false;

            // 2. Monta dicionário apenas com colunas que mudaram
            var alteracoes = new Dictionary<string, (object? Valor, DbType Tipo)>();
            CompararEAdicionar(alteracoes, "IDGRUPO",       existente.IDGRUPO,          produto.IDGRUPO,          DbType.Int64);
            CompararEAdicionar(alteracoes, "CDPRODUTO",     existente.CDPRODUTO,        produto.CDPRODUTO,        DbType.String);
            CompararEAdicionar(alteracoes, "NMPRODUTO",     existente.NMPRODUTO,        produto.NMPRODUTO,        DbType.String);
            CompararEAdicionar(alteracoes, "CTPRODUTO",     existente.CTPRODUTO,        produto.CTPRODUTO,        DbType.String);
            CompararEAdicionar(alteracoes, "TPPRODUTO",     existente.TPPRODUTO,        produto.TPPRODUTO,        DbType.Int32);
            CompararEAdicionar(alteracoes, "UNCOMPRA",      existente.UNCOMPRA,         produto.UNCOMPRA,         DbType.String);
            CompararEAdicionar(alteracoes, "UNVENDA",       existente.UNVENDA,          produto.UNVENDA,          DbType.String);
            CompararEAdicionar(alteracoes, "NURELACAO",     existente.NURELACAO,        produto.NURELACAO,        DbType.Int32);
            CompararEAdicionar(alteracoes, "NUPRECO",       existente.NUPRECO,          produto.NUPRECO,          DbType.Double);
            CompararEAdicionar(alteracoes, "NUQTDMIN",      existente.NUQTDMIN,         produto.NUQTDMIN,         DbType.Double);
            CompararEAdicionar(alteracoes, "CDSEFAZ",       existente.CDSEFAZ,          produto.CDSEFAZ,          DbType.String);
            CompararEAdicionar(alteracoes, "CDANP",         existente.CDANP,            produto.CDANP,            DbType.String);
            CompararEAdicionar(alteracoes, "CDNCM",         existente.CDNCM,            produto.CDNCM,            DbType.String);
            CompararEAdicionar(alteracoes, "CDCEST",        existente.CDCEST,           produto.CDCEST,           DbType.String);
            CompararEAdicionar(alteracoes, "CDSERV",        existente.CDSERV,           produto.CDSERV,           DbType.String);
            CompararEAdicionar(alteracoes, "STPRODUTO",     existente.STPRODUTO,        produto.STPRODUTO,        DbType.Int32);
            CompararEAdicionar(alteracoes, "VLULTIMACOMPRA",existente.VLULTIMACOMPRA,   produto.VLULTIMACOMPRA,   DbType.Double);
            CompararEAdicionar(alteracoes, "VLCUSTOMEDIO",  existente.VLCUSTOMEDIO,     produto.VLCUSTOMEDIO,     DbType.Double);
            CompararEAdicionar(alteracoes, "PCIBPTFED",     existente.PCIBPTFED,        produto.PCIBPTFED,        DbType.Double);
            CompararEAdicionar(alteracoes, "PCIBPTEST",     existente.PCIBPTEST,        produto.PCIBPTEST,        DbType.Double);
            CompararEAdicionar(alteracoes, "PCIBPTMUN",     existente.PCIBPTMUN,        produto.PCIBPTMUN,        DbType.Double);
            CompararEAdicionar(alteracoes, "PCIBPTIMP",     existente.PCIBPTIMP,        produto.PCIBPTIMP,        DbType.Double);
            CompararEAdicionar(alteracoes, "NUCFOP",        existente.NUCFOP,           produto.NUCFOP,           DbType.Int32);
            CompararEAdicionar(alteracoes, "STORIGEMPROD",  existente.STORIGEMPROD,     produto.STORIGEMPROD,     DbType.Int32);
            CompararEAdicionar(alteracoes, "DSICMS_CST",    existente.DSICMS_CST,       produto.DSICMS_CST,       DbType.String);
            CompararEAdicionar(alteracoes, "PCICMS_ALIQ",   existente.PCICMS_ALIQ,      produto.PCICMS_ALIQ,      DbType.Double);
            CompararEAdicionar(alteracoes, "PCICMS_REDUCBC",existente.PCICMS_REDUCBC,   produto.PCICMS_REDUCBC,   DbType.Double);
            CompararEAdicionar(alteracoes, "PCICMSST_ALIQ", existente.PCICMSST_ALIQ,    produto.PCICMSST_ALIQ,    DbType.Double);
            CompararEAdicionar(alteracoes, "PCICMSST_MVA",  existente.PCICMSST_MVA,     produto.PCICMSST_MVA,     DbType.Double);
            CompararEAdicionar(alteracoes, "PCICMSST_REDUCBC", existente.PCICMSST_REDUCBC, produto.PCICMSST_REDUCBC, DbType.Double);
            CompararEAdicionar(alteracoes, "DSIPI_CST",     existente.DSIPI_CST,        produto.DSIPI_CST,        DbType.String);
            CompararEAdicionar(alteracoes, "PCIPI_ALIQ",    existente.PCIPI_ALIQ,       produto.PCIPI_ALIQ,       DbType.Double);
            CompararEAdicionar(alteracoes, "DSPIS_CST",     existente.DSPIS_CST,        produto.DSPIS_CST,        DbType.String);
            CompararEAdicionar(alteracoes, "PCPIS_ALIQ",    existente.PCPIS_ALIQ,       produto.PCPIS_ALIQ,       DbType.Double);
            CompararEAdicionar(alteracoes, "DSCOFINS_CST",  existente.DSCOFINS_CST,     produto.DSCOFINS_CST,     DbType.String);
            CompararEAdicionar(alteracoes, "PCCOFINS_ALIQ", existente.PCCOFINS_ALIQ,    produto.PCCOFINS_ALIQ,    DbType.Double);
            CompararEAdicionar(alteracoes, "STESTOQUE",     existente.STESTOQUE,        produto.STESTOQUE,        DbType.Int32);
            CompararEAdicionar(alteracoes, "STBALANCA",     existente.STBALANCA,        produto.STBALANCA,        DbType.Int32);
            CompararEAdicionar(alteracoes, "STEXPORTARPEDIDO", existente.STEXPORTARPEDIDO, produto.STEXPORTARPEDIDO, DbType.Int32);
            CompararEAdicionar(alteracoes, "IDDEP",         existente.IDDEP,            produto.IDDEP,            DbType.Int64);
            CompararEAdicionar(alteracoes, "IDMARCA",       existente.IDMARCA,          produto.IDMARCA,          DbType.Int64);
            CompararEAdicionar(alteracoes, "IDSUBGRUPO",    existente.IDSUBGRUPO,       produto.IDSUBGRUPO,       DbType.Int64);
            CompararEAdicionar(alteracoes, "DSVOLUME",      existente.DSVOLUME,         produto.DSVOLUME,         DbType.String);

            // 3. Se nada mudou, considera sucesso (não precisa de UPDATE)
            if (alteracoes.Count == 0)
                return true;

            // 4. Monta SQL dinâmico apenas com colunas alteradas
            var sets = string.Join(", ", alteracoes.Select(a => $"{a.Key} = @{a.Key}"));
            var sql = $"UPDATE produto SET {sets} WHERE IDPRODUTO = @Id";

            var parametros = new DynamicParameters();
            parametros.Add("@Id", produto.Id, DbType.Int64);
            foreach (var alt in alteracoes)
                parametros.Add($"@{alt.Key}", alt.Value.Valor ?? DBNull.Value, alt.Value.Tipo);

            return _dbSession.Connection.Execute(sql, parametros) > 0;
        }

        /// <summary>
        /// Compara dois valores (nullable-aware) e adiciona ao dicionário se forem diferentes.
        /// </summary>
        private static void CompararEAdicionar(Dictionary<string, (object? Valor, DbType Tipo)> alteracoes,
            string coluna, object? valorExistente, object? valorNovo, DbType tipo)
        {
            if (!Equals(valorExistente, valorNovo))
                alteracoes[coluna] = (valorNovo, tipo);
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

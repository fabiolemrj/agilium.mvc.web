using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace agilium.api.infra.Repository.Dapper
{
    public class PerdaDapperReposiotry : IPerdaDapperRepository
    {
        private readonly IUtilDapperRepository _utilDapperRepository;
        protected readonly IConfiguration _configuration;
        private readonly DbSession _dbSession;

        public PerdaDapperReposiotry(IUtilDapperRepository utilDapperRepository, IConfiguration configuration, DbSession dbSession)
        {
            _utilDapperRepository = utilDapperRepository;
            _configuration = configuration;
            _dbSession = dbSession;
        }

        public async Task<bool> InserirPerda(long idEmpresa, long idEstoque, long idPerda, long idProduto, long idUsuario, ETipoPerda tipoPerda, ETipoMovimentoPerda tipoMovimentoPerda, double quantidadePerda, string Observacao)
        {
            var resultado = _utilDapperRepository.GerarUUID().Result;
            var codigo = _utilDapperRepository.GerarCodigo($"SELECT MAX(CAST(CDPERDA AS UNSIGNED)) AS CD FROM perda WHERE IDEMPRESA ={idEmpresa}").Result;

            var parametros = new DynamicParameters();
            parametros.Add("@IDPERDA", resultado, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDESTOQUE", idEstoque, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPRODUTO", idProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDEMPRESA", idEmpresa, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDUSUARIO", idUsuario, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@CDPERDA", codigo, DbType.String, ParameterDirection.Input);
            parametros.Add("@NUQTDPERDA", quantidadePerda, DbType.Double, ParameterDirection.Input);
            parametros.Add("@TPPERDA", tipoPerda, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@TPMOV", tipoMovimentoPerda, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@DSOBS", Observacao, DbType.String, ParameterDirection.Input);


            var query = $@"INSERT INTO perda (IDPERDA, IDEMPRESA, IDESTOQUE, IDPRODUTO, IDUSUARIO, CDPERDA, DTHRPERDA, TPPERDA, NUQTDPERDA, DSOBS, TPMOV)
                            values (@IDPERDA, @IDEMPRESA, @IDESTOQUE, @IDPRODUTO, @IDUSUARIO, @CDPERDA, now(), @TPPERDA, @NUQTDPERDA, @DSOBS, @TPMOV)";

            return _dbSession.Connection.Execute(query, parametros,_dbSession.Transaction) > 0;
        }

        public async Task<long> lancarPerdaRetornaIdHistoricoGerado(long idPerda, string usuarioPerda)
        {
           return  await lancarPerdaRetornaIdHistoricoGeradoTransacao(idPerda, usuarioPerda);
            //var descricaoHistorico = "";
            //long idEstoqueHistorico = 0;

            //using (var scope = new TransactionScope())
            //{
            //    using (var con = new MySqlConnection(GetConnection()))
            //    {
            //        try
            //        {
            //            con.Open();
            //            var query = $@"SELECT p.IDPERDA as Id ,p.IDEMPRESA,p.IDESTOQUE,p.IDESTOQUEHST,p.IDPRODUTO,p.IDUSUARIO,p.CDPERDA,p.DTHRPERDA,p.TPPERDA,p.TPMOV,p.NUQTDPERDA,p.VLCUSTOMEDIO,p.DSOBS                                    
            //                           from perda p WHERE IDPERDA = {idPerda}";
            //            var perda = con.Query<Perda>(query).FirstOrDefault(); 
                        
            //            if(perda != null && perda.IDPRODUTO.HasValue)
            //            {
            //                var queryProduto = @$"SELECT IDPRODUTO as Id,IDEMPRESA,IDGRUPO,IDSUBGRUPO,IDDEP,IDMARCA,CDPRODUTO,NMPRODUTO,CTPRODUTO,TPPRODUTO FROM produto where IDPRODUTO = {perda.IDPRODUTO}";
            //                var produto = con.Query<Produto>(queryProduto).FirstOrDefault();
            //                if(produto.CTPRODUTO == "1" || produto.CTPRODUTO == "4")
            //                {
            //                    descricaoHistorico = perda.TPMOV == business.Enums.ETipoMovimentoPerda.Perda  ? "Baixa" : "Entrada";
            //                    var tpmov = perda.TPMOV == business.Enums.ETipoMovimentoPerda.Perda ? 1 : 2;
            //                    var tpperda = (int)perda.TPPERDA;
            //                    descricaoHistorico += $@" pelo lançamento de {ConverterTipoMovPerda(tpmov)} nº {perda.CDPERDA} 
            //                                            | motivo: {ConverterTipoPerda(tpperda)}
            //                                            | Obs: {perda.DSOBS}";
            //                    //tipo perda 1
            //                    if(perda.TPMOV == business.Enums.ETipoMovimentoPerda.Perda)
            //                    {
            //                        idEstoqueHistorico = await RealizaRetiradaRetornaIdHistoricoGerado(perda.IDESTOQUE.Value, perda.IDPRODUTO.Value,
            //                                                                            -1, usuarioPerda, descricaoHistorico, perda.NUQTDPERDA.Value, con);
                                    
            //                    }
            //                    else
            //                    {
            //                        idEstoqueHistorico = await RealizaEntradaRetornaIdHistoricoGerado(perda.IDESTOQUE.Value, perda.IDPRODUTO.Value,
            //                                                                            -1, usuarioPerda, descricaoHistorico, perda.NUQTDPERDA.Value, con);
            //                    }
            //                    AtualizarPerdaComIdEstoqueHistorico(idEstoqueHistorico, perda.Id, con);
            //                }
            //            }
            //            scope.Complete();
            //            return idEstoqueHistorico;                       
            //        }
            //        catch (Exception ex)
            //        {
            //            throw;
            //        }
            //        finally { con.Close(); }
            //    };

            //}
        }

        private async Task<long> lancarPerdaRetornaIdHistoricoGeradoTransacao(long idPerda, string usuarioPerda)
        {
            var descricaoHistorico = "";
            long idEstoqueHistorico = 0;

                    try
                    {
                        var query = $@"SELECT p.IDPERDA as Id ,p.IDEMPRESA,p.IDESTOQUE,p.IDESTOQUEHST,p.IDPRODUTO,p.IDUSUARIO,p.CDPERDA,p.DTHRPERDA,p.TPPERDA,p.TPMOV,p.NUQTDPERDA,p.VLCUSTOMEDIO,p.DSOBS                                    
                                       from perda p WHERE IDPERDA = {idPerda}";
                        var perda = _dbSession.Connection.Query<Perda>(query, null, _dbSession.Transaction).FirstOrDefault();

                        if (perda != null && perda.IDPRODUTO.HasValue)
                        {
                            var queryProduto = @$"SELECT IDPRODUTO as Id,IDEMPRESA,IDGRUPO,IDSUBGRUPO,IDDEP,IDMARCA,CDPRODUTO,NMPRODUTO,CTPRODUTO,TPPRODUTO FROM produto where IDPRODUTO = {perda.IDPRODUTO}";
                            var produto = _dbSession.Connection.Query<Produto>(queryProduto,null, _dbSession.Transaction).FirstOrDefault();
                            if (produto.CTPRODUTO == "1" || produto.CTPRODUTO == "4")
                            {
                                descricaoHistorico = perda.TPMOV == business.Enums.ETipoMovimentoPerda.Perda ? "Baixa" : "Entrada";
                                var tpmov = perda.TPMOV == business.Enums.ETipoMovimentoPerda.Perda ? 1 : 2;
                                var tpperda = (int)perda.TPPERDA;
                                descricaoHistorico += $@" pelo lançamento de {ConverterTipoMovPerda(tpmov)} nº {perda.CDPERDA} 
                                                        | motivo: {ConverterTipoPerda(tpperda)}
                                                        | Obs: {perda.DSOBS}";
                                //tipo perda 1
                                if (perda.TPMOV == business.Enums.ETipoMovimentoPerda.Perda)
                                {
                                    idEstoqueHistorico = await RealizaRetiradaRetornaIdHistoricoGerado(perda.IDESTOQUE.Value, perda.IDPRODUTO.Value,
                                                                                        -1, usuarioPerda, descricaoHistorico, perda.NUQTDPERDA.Value);

                                }
                                else
                                {
                                    idEstoqueHistorico = await RealizaEntradaRetornaIdHistoricoGerado(perda.IDESTOQUE.Value, perda.IDPRODUTO.Value,
                                                                                        -1, usuarioPerda, descricaoHistorico, perda.NUQTDPERDA.Value);
                                }
                                AtualizarPerdaComIdEstoqueHistorico(idEstoqueHistorico, perda.Id);
                            }
                        }
                        return idEstoqueHistorico;
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }                    
              
        }

        #region privates 

        private async Task<long> RealizaRetiradaRetornaIdHistoricoGerado(long idEstoque, long idProduto, long idItem, string UsuarioHistorico, string DescricaoHistorico, double Quantidade)
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
                var idEstoqueProduto = _dbSession.Connection.Query<long>($@"SELECT IDESTOQUE_PROD FROM estoque_prod WHERE IDESTOQUE = {idEstoque} AND IDPRODUTO = {idProduto}").FirstOrDefault();
                if (idEstoqueProduto > 0)
                {
                    AtualizarEstoqueProduto(2, idEstoqueProduto, Quantidade);
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

        private async Task<long> RealizaEntradaRetornaIdHistoricoGerado(long idEstoque, long idProduto, long idItem, string UsuarioHistorico, string DescricaoHistorico, double Quantidade)
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
                var idEstoqueProduto = _dbSession.Connection.Query<long>($@"SELECT IDESTOQUE_PROD FROM estoque_prod WHERE IDESTOQUE = {idEstoque} AND IDPRODUTO = {idProduto}").FirstOrDefault();
                if (idEstoqueProduto > 0)
                {
                    AtualizarEstoqueProduto(1, idEstoqueProduto, Quantidade);
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

        private void IncluirEstoqueProduto(long idEstoqueProduto, long idEstoque, long idProduto, double quant)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDESTOQUE_PROD", idEstoqueProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDESTOQUE", idEstoque, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPRODUTO", idProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NUQTD", quant, DbType.Double, ParameterDirection.Input);

            var query = $@"INSERT INTO estoque_prod (IDESTOQUE_PROD, IDESTOQUE, IDPRODUTO, NUQTD)
                            values (@IDESTOQUE_PROD, @IDESTOQUE, @IDPRODUTO, @NUQTD)";

            _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction);
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


            _dbSession.Connection.Execute(query, parametros);
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

            _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction);

            return resultado;
        }
        private string ConverterTipoPerda(int tpPerda)
        {
            var result = "";
            switch (tpPerda)
            {
                case 1:
                    result = "Quebra ou Inutilização";
                    break;
                case 2:
                    result = "Devolução de Cliente";
                    break;
                case 3:
                    result = "Validade Vencida";
                    break;
                case 4:
                    result = "Acerto de Saldo";
                    break;
                case 5:
                    result = "Falha Operacional";
                    break;

                default:
                    result = "Outros";
                    break;
            }
            return result;
        }

        private string ConverterTipoMovPerda(int tpPerda)
        {
            var result = "Indefinido";
            switch (tpPerda)
            {
                case 1:
                    result = "Perda";
                    break;
                case 2:
                    result = "Sobra";
                    break;
            }
            return result;
        }

        private void AtualizarPerdaComIdEstoqueHistorico(long idEstoqueHistorico, long idPerda)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDESTOQUEHST", idEstoqueHistorico, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPERDA", idPerda, DbType.Int64, ParameterDirection.Input);

            var query = $@"UPDATE perda SET IDESTOQUEHST = {idEstoqueHistorico} WHERE IDPERDA = {idPerda}";

            _dbSession.Connection.Execute(query,null, _dbSession.Transaction);
        }



        #endregion
    }
}

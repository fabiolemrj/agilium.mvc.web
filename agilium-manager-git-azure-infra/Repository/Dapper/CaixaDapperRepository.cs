using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.PdvViewModel.CaixaReturnViewModel;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace agilium.api.infra.Repository.Dapper
{
    public class CaixaDapperRepository : ICaixaDapperRepository
    {
        private readonly IUtilDapperRepository _utilDapperRepository;
        protected readonly IConfiguration _configuration;
        private readonly DbSession _dbSession;

        public CaixaDapperRepository(IUtilDapperRepository utilDapperRepository, IConfiguration configuration, DbSession dbSession)
        {
            _utilDapperRepository = utilDapperRepository;
            _configuration = configuration;
            _dbSession = dbSession;
        }

        public string GetConnection()
        {
            var autenticacaoUrl = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return autenticacaoUrl;
        }

        public async Task<long> ObterIdFuncionarioPorUsuarioEmpresa(long idEmpresa, long idUsuario)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@idusuario", idUsuario, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@idempresa", idEmpresa, DbType.Int64, ParameterDirection.Input);

            var query = $@"SELECT  IDFUNC as Id, funcionario.* FROM funcionario where idusuario = @idusuario and idempresa = @idempresa";

            var funcionario = _dbSession.Connection.Query<Funcionario>(query, parametros, _dbSession.Transaction).FirstOrDefault();
            if (funcionario != null) return funcionario.Id;
            return 0;
        }

        public async Task<long> ObterIdCaixaAberto(long idEmpresa, long idUsuario)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@idusuario", idUsuario, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@idempresa", idEmpresa, DbType.Int64, ParameterDirection.Input);

            var query = $@"SELECT IDCAIXA as Id, caixa.* FROM caixa WHERE ((STCAIXA = 1) or (DTHRFECH is null))
                            AND IDEMPRESA = @idempresa AND IDFUNC=@idusuario";

            var caixa = _dbSession.Connection.Query<Caixa>(query, parametros, _dbSession.Transaction).FirstOrDefault();
            if (caixa != null) return caixa.Id;
            return 0;
        }

        public async Task<PontoVenda> ObterPdvPorNomeMaquina(string maquina)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@nmmaquina", maquina, DbType.String, ParameterDirection.Input);

            var query = $@"SELECT pdv.IDPDV as Id, pdv.* FROM pdv WHERE NMMAQUINA = @nmmaquina";

            return _dbSession.Connection.Query<PontoVenda>(query, parametros, _dbSession.Transaction).FirstOrDefault();
            
        }


        public async Task<business.Models.Config> ObterTipoAberturaCaixaPorEmpresa(long idEmpresa)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@chave", "CAIXA_TPABERTURA", DbType.String, ParameterDirection.Input);
            parametros.Add("@IDEMPRESA", idEmpresa, DbType.Int64, ParameterDirection.Input);

            var query = $@"select * from config where  IDEMPRESA = @IDEMPRESA and chave = @chave";

            return _dbSession.Connection.Query<business.Models.Config>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<double> ObterValorCaixaAnterior(long idFuncionario, long idEmpresa)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@idusuario", idFuncionario, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@idempresa", idEmpresa, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@stcaixa", ESituacaoCaixa.Fechado, DbType.Int32, ParameterDirection.Input);

            var query = $@"select IDCAIXA as Id, caixa.* from caixa where IDEMPRESA = @idempresa and IDFUNC=@idusuario and stcaixa = @stcaixa";

            var caixa = _dbSession.Connection.Query<Caixa>(query, parametros, _dbSession.Transaction).FirstOrDefault();
            if (caixa != null) return caixa.VLFECH.HasValue ? caixa.VLFECH.Value :0;
            return 0;
        }

        public async Task<double> ObterConfigRealizaSuprimentoAbertura(long idEmpresa)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@chave", "CAIXA_VLSUPRIMENTO", DbType.String, ParameterDirection.Input);
            parametros.Add("@IDEMPRESA", idEmpresa, DbType.Int64, ParameterDirection.Input);

            var query = $@"select * from config where  IDEMPRESA = @IDEMPRESA and chave = @chave";

            var config = _dbSession.Connection.Query<business.Models.Config>(query, parametros, _dbSession.Transaction).FirstOrDefault();

            double resultado = 0;
            if(string.IsNullOrEmpty(config.VALOR))
                double.TryParse(config.VALOR, out resultado);

            return resultado ;
        }

        public async Task<long> IncluirCaixa(long idEmpresa, long idTurno, long idPdv, long idFuncionario, int sqCaixa, double valorAbertura)
        {
            var id = _utilDapperRepository.GerarUUID().Result;

            var parametros = new DynamicParameters();
            parametros.Add("@IDCAIXA", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDEMPRESA", idEmpresa, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDTURNO", idTurno, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPDV", idPdv, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDFUNC", idFuncionario, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@SQCAIXA", sqCaixa, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@STCAIXA", ESituacaoCaixa.Aberto, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@VLABT", valorAbertura, DbType.Double, ParameterDirection.Input);

            var query = $@"INSERT INTO caixa (IDCAIXA, IDEMPRESA, IDTURNO, IDPDV, IDFUNC, SQCAIXA, STCAIXA, DTHRABT, VLABT)
                            values (@IDCAIXA, @IDEMPRESA, @IDTURNO, @IDPDV, @IDFUNC, @SQCAIXA, @STCAIXA, now(), @VLABT)";

            if (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) == 0)
                id = 0;

            return id;
        }

        public async Task<int> GerarSeqCaixa(long idEmpresa)
        {
            var parametros = new DynamicParameters();

            parametros.Add("@chave", "CAIXA", DbType.String, ParameterDirection.Input);
            parametros.Add("@IDEMPRESA", idEmpresa, DbType.Int64, ParameterDirection.Input);

            var query = $@"SELECT COALESCE(SEQUENCIAL, -1) as valor FROM seq WHERE IDEMPRESA = @IDEMPRESA AND CHAVE = @CHAVE FOR UPDATE";

            var resultado = _dbSession.Connection.Query<int>(query, parametros, _dbSession.Transaction).FirstOrDefault();
            if(resultado > 0)
            {
                resultado++;
                parametros.Add("@SEQUENCIAL", resultado, DbType.Int32, ParameterDirection.Input);
                var queryInsert = $@"UPDATE seq SET SEQUENCIAL = @SEQUENCIAL WHERE CHAVE = @CHAVE AND IDEMPRESA = @IDEMPRESA";
                _dbSession.Connection.Execute(queryInsert, parametros, _dbSession.Transaction);
            }
            else
            {
                resultado = 1;
                parametros.Add("@SEQUENCIAL", resultado, DbType.Int32, ParameterDirection.Input);
                var queryInsert = $@"INSERT INTO seq (CHAVE, IDEMPRESA, SEQUENCIAL) values (@CHAVE, @IDEMPRESA, @SEQUENCIAL)";
                _dbSession.Connection.Execute(queryInsert, parametros, _dbSession.Transaction);
            }
            return resultado;
        }

        public async Task<string> ObterConfigDescricaoSuprimentoAbertura(long idEmpresa)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@chave", "CAIXA_DSSUPRIMENTO", DbType.String, ParameterDirection.Input);
            parametros.Add("@IDEMPRESA", idEmpresa, DbType.Int64, ParameterDirection.Input);

            var query = $@"select * from config where  IDEMPRESA = @IDEMPRESA and chave = @chave";

            var config = _dbSession.Connection.Query<business.Models.Config>(query, parametros, _dbSession.Transaction).FirstOrDefault();

            var resultado = "";
            if (config != null)
                resultado = config.VALOR;

            return resultado;
        }

        public async Task<long> RealizarSuprimento(long idCaixa, long idFunc, double vlMovimento, string descricao)
        {
            var id = _utilDapperRepository.GerarUUID().Result;

            var parametros = new DynamicParameters();
            parametros.Add("@IDCAIXA", idCaixa, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDCAIXA_MOV", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@TPMOV", ETipoMovCaixa.Suprimento, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@DSMOV", descricao, DbType.String, ParameterDirection.Input);
            parametros.Add("@VLMOV", vlMovimento, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@STMOV", ESituacaoMovCaixa.Ativo, DbType.Int32, ParameterDirection.Input);

            var query = $@"INSERT INTO caixa_mov (IDCAIXA_MOV,IDCAIXA,TPMOV,DSMOV,VLMOV,STMOV)
                            values (@IDCAIXA_MOV,@IDCAIXA,@TPMOV,@DSMOV,@VLMOV,@STMOV)";

            if (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) == 0)
                id = 0;

            return id;
        }

        public async Task<Caixa> ObterCaixaPorId(long idCaixa)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDCAIXA", idCaixa, DbType.Int64, ParameterDirection.Input);

            var query = $@"select IDCAIXA as Id, caixa.* from caixa where IDCAIXA = @IDCAIXA";

            return _dbSession.Connection.Query<Caixa>(query, parametros, _dbSession.Transaction).FirstOrDefault();

        }

        public async Task<long> RealizarSangria(long idCaixa, long idFunc, double vlMovimento, string descricao)
        {
            var id = _utilDapperRepository.GerarUUID().Result;

            var parametros = new DynamicParameters();
            parametros.Add("@IDCAIXA", idCaixa, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDCAIXA_MOV", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@TPMOV", ETipoMovCaixa.Sangria, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@DSMOV", descricao, DbType.String, ParameterDirection.Input);
            parametros.Add("@VLMOV", vlMovimento, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@STMOV", ESituacaoMovCaixa.Ativo, DbType.Int32, ParameterDirection.Input);

            var query = $@"INSERT INTO caixa_mov (IDCAIXA_MOV,IDCAIXA,TPMOV,DSMOV,VLMOV,STMOV)
                            values (@IDCAIXA_MOV,@IDCAIXA,@TPMOV,@DSMOV,@VLMOV,@STMOV)";

            if (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) == 0)
                id = 0;

            return id;
        }

        public async Task<double> ObterSaldoCaixaPorMovimentacao(long idCAixa, ETipoMovCaixa eTipoMovCaixa)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDCAIXA", idCAixa, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@TPMOV", ETipoMovCaixa.Sangria, DbType.Int32, ParameterDirection.Input);
            var query = $@"select coalesce(sum(VLMOV),0) valor from caixa_mov where idcaixa = @IDCAIXA and TPMOV = @TPMOV";

            return _dbSession.Connection.Query<double>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<double> ObterSaldoVendaPorCaixa(long idCAixa)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDCAIXA", idCAixa, DbType.Int64, ParameterDirection.Input);
            var query = $@"select COALESCE(sum(VLTOTAL), 0) AS VLTOTAL from venda  WHERE IDCAIXA = @IDCAIXA";

            return _dbSession.Connection.Query<double>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<double> ObterSaldoAberturaCaixa(long idCAixa)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDCAIXA", idCAixa, DbType.Int64, ParameterDirection.Input);
            var query = $@"select coalesce(vlabt,0) valor from caixa where idcaixa = @IDCAIXA";

            return _dbSession.Connection.Query<double>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<double> ObterSaldoCaixa(long idCaixa)
        {           
            var valorSangria = await ObterSaldoCaixaPorMovimentacao(idCaixa, ETipoMovCaixa.Sangria);
            var valorSuprimento = await ObterSaldoCaixaPorMovimentacao(idCaixa, ETipoMovCaixa.Suprimento);
            var totalVendas = await ObterSaldoVendaPorCaixa(idCaixa);
            var valorAbertura = await ObterSaldoAberturaCaixa(idCaixa);

            return valorAbertura + totalVendas + valorSuprimento - valorSangria;
        }

        public async Task<bool> FecharCaixa(long idCaixa, double valorFechamento)
        {
            var resultado = false;
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@idcaixa", idCaixa, DbType.Int64, ParameterDirection.Input);
                parametros.Add("@stcaixa", ESituacaoCaixa.Fechado, DbType.Int32, ParameterDirection.Input);
                parametros.Add("@vlfech", valorFechamento, DbType.Double, ParameterDirection.Input);
                parametros.Add("@STVENDA", ESituacaoVenda.Ativo, DbType.Int32, ParameterDirection.Input);
                var query = $@"update caixa set stcaixa =@stcaixa, dthrfech=now(), vlfech=@vlfech
                        where idcaixa =@idcaixa";

                if (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0)
                {
                    var queryMoedas = $@"SELECT VM.IDMOEDA, SUM(VM.VLPAGO - VM.VLTROCO) AS TOTAL FROM venda_moeda VM 
                                    INNER JOIN venda V ON VM.IDVENDA = V.IDVENDA 
                                    WHERE V.IDCAIXA = @IDCAIXA
                                     AND V.STVENDA = @STVENDA
                                     GROUP BY VM.IDMOEDA";
                    var lista = _dbSession.Connection.Query<dynamic>(queryMoedas, parametros, _dbSession.Transaction);

                    var queryInsert = $@"INSERT INTO caixa_moeda(IDCAIXA_MOEDA, IDCAIXA, IDMOEDA, VLMOEDAORIGINAL)
                                values (@IDCAIXA_MOEDA, @IDCAIXA, @IDMOEDA, @VLMOEDAORIGINAL)";
                    lista.ToList().ForEach(item => {
                        var id = _utilDapperRepository.GerarUUID().Result;
                        var parametroInsert = new DynamicParameters();
                        parametroInsert.Add("@IDCAIXA_MOEDA", id, DbType.Int64, ParameterDirection.Input);
                        parametroInsert.Add("@IDCAIXA", idCaixa, DbType.Int64, ParameterDirection.Input);
                        parametroInsert.Add("@VLMOEDAORIGINAL", item.TOTAL, DbType.Double, ParameterDirection.Input);
                        parametroInsert.Add("@IDMOEDA", item.IDMOEDA, DbType.Int64, ParameterDirection.Input);

                        _dbSession.Connection.Execute(queryInsert, parametroInsert, _dbSession.Transaction);
                    });
                }
                resultado = true;
            }
            catch 
            {
                resultado = false;               
            }
            return resultado;
        }

        public async Task<FecharCaixaInicializarViewModel> ObterCaixaParaFechamento(long idCaixa)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDCAIXA", idCaixa, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@STCAIXA", ESituacaoCaixa.Aberto, DbType.Int64, ParameterDirection.Input);
            var query = $@"select c.idcaixa, c.idempresa, c.idturno, c.sqcaixa,t.NUTURNO,p.DSPDV as pdv, f.NMFUNC as usuario, t.dtturno as dataTurno, c.dthrabt as dataAbertura
                             from caixa c
	                            left join turno t on t.IDTURNO = c.IDTURNO
                             left join pdv p on p.IDPDV = c.IDPDV 
                             left join funcionario f on f.IDFUNC = c.IDFUNC
                             where (c.STCAIXA = @STCAIXA and c.IDCAIXA = @IDCAIXA) ";

            return _dbSession.Connection.Query<FecharCaixaInicializarViewModel>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<Caixa> ObterCaixaAberto(long idEmpresa, long idUsuario)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@idusuario", idUsuario, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@idempresa", idEmpresa, DbType.Int64, ParameterDirection.Input);

            var query = $@"SELECT IDCAIXA as Id, caixa.* FROM caixa WHERE ((STCAIXA = 1) or (DTHRFECH is null))
                            AND IDEMPRESA = @idempresa AND IDFUNC=@idusuario";

            return _dbSession.Connection.Query<Caixa>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<long> ObterEstoquePorIdCaixa(long idCaixa) 
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDCAIXA", idCaixa, DbType.Int64, ParameterDirection.Input);

            var query = $@"SELECT COALESCE(P.IDESTOQUE, 0) FROM pdv P INNER JOIN caixa C ON P.IDPDV = C.IDPDV WHERE C.IDCAIXA = @IDCAIXA";

            return _dbSession.Connection.Query<long>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }
    }
}

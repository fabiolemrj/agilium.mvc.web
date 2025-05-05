using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;

using System;
using Dapper;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Data;
using agilium.api.business.Models.CustomReturn.ReportViewModel.VendaReportViewModel;
using agilium.api.business.Interfaces;
using agilium.api.business.Enums;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using MySqlConnector;
using agilium.api.business.Models.CustomReturn.VendaCustomViewModel;
using MySql.Data.MySqlClient;

namespace agilium.api.infra.Repository.Dapper
{
    public class VendaDapperRepository : IVendaDapperRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IDapperRepository _dapperRepository;
        private readonly IUtilDapperRepository _utilDapperRepository;
        private readonly DbSession _dbSession;
        private readonly IEstoqueDapperRepository _estoqueDapperRepository;


        public VendaDapperRepository(IConfiguration configuration,IDapperRepository dapperRepository, IUtilDapperRepository utilDapperRepository,
            DbSession dbSession, IEstoqueDapperRepository estoqueDapperRepository)
        {
            _configuration = configuration;
            _dapperRepository = dapperRepository;
            _dbSession = dbSession;
            _utilDapperRepository = utilDapperRepository;
            _estoqueDapperRepository = estoqueDapperRepository;
           
        }

        public string GetConnection()
        {
            var autenticacaoUrl = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return autenticacaoUrl;
        }

        public async Task<List<VendaItemReportViewModel>> ObterItensVendaReportViewModelPorVenda(long idVenda)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@idVenda", idVenda, DbType.Int64, ParameterDirection.Input);

            var query = $@"select i.IDVENDA_ITEM as Id, i.IDVENDA, i.VLUNIT as ValorUnitario, i.NUQTD as Quantidade,
                             i.VLTOTAL as Total, i.STITEM as Situacao,
                            p.NMPRODUTO as Produto 
                            from venda_item i
                            inner join produto p on p.IDPRODUTO = i.IDPRODUTO
                            where i.idvenda = @idVenda
                            and i.STITEM = 1
                             order by i.sqitem";

            return _dbSession.Connection.Query<VendaItemReportViewModel>(query, parametros, _dbSession.Transaction).ToList();
        }

        public async Task<List<VendaMoedaItemReportViewModel>> ObterMoedaItensVendaReportViewModelPorVenda(long idVenda)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@idVenda", idVenda, DbType.Int64, ParameterDirection.Input);

            var query = $@" select (vm.VLPAGO - vm.VLTROCO) valor, m.DSMOEDA as Moeda
                             from venda_moeda vm
                             inner join moeda m on m.IDMOEDA = vm.IDMOEDA
                             where vm.idvenda  = @idVenda
                             group by valor,m.DSMOEDA order by m.DSMOEDA";

            return _dbSession.Connection.Query<VendaMoedaItemReportViewModel>(query, parametros, _dbSession.Transaction).ToList();
        }

        public async Task<List<VendaMoedaItemReportViewModel>> ObterMoedaTotalReportViewModelPorVenda(DateTime dataInicial, DateTime dataFinal)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@dtINicial", dataInicial, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@dtFinal", dataFinal, DbType.DateTime, ParameterDirection.Input);

            var query = $@"select m.DSMOEDA as Moeda, sum(vm.VLPAGO - vm.VLTROCO) as Valor 
                             from venda v 
                             inner join caixa c on c.IDCAIXA = v.IDCAIXA 
                             inner join turno t on t.IDTURNO = c.IDTURNO 
                             inner join venda_moeda vm on v.IDVENDA = vm.IDVENDA 
                             inner join moeda m on vm.IDMOEDA = m.IDMOEDA 
                             where date(v.dthrvenda)  between @dtINicial and @dtFinal
                             group by m.DSMOEDA";

            return _dbSession.Connection.Query<VendaMoedaItemReportViewModel>(query, parametros, _dbSession.Transaction).ToList();
        }

        public async Task<List<VendaReportViewModel>> ObterVendaReportViewModel(DateTime dataInicial, DateTime dataFinal)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@dtINicial", dataInicial, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@dtFinal", dataFinal, DbType.DateTime, ParameterDirection.Input);


            var query = $@"select v.IDVENDA as Id, v.SQVENDA as Sequencial, v.STVENDA as Situacao, v.VLVENDA as Valor, v.VLACRES as Acrescimo,
                            v.VLDESC as Desconto, v.VLTOTAL as Total, c.SQCAIXA as SeqCaixa,
                             l.NMCLIENTE, f.NMFUNC,t.NUTURNO,t.DTTURNO,p.DSPDV as Pdv, vc.IDUSUARIOCANCEL, vc.DTHRCANCEL, 
                            COALESCE((SELECT SUM(VI.VLTOTAL) FROM venda_item VI WHERE VI.IDVENDA = v.IDVENDA and VI.STITEM = 2), 0) as Devolucao
                             from venda v
                             inner join caixa c on c.IDCAIXA = v.IDCAIXA 
                             left join cliente l on l.IDCLIENTE = v.IDCLIENTE
                             inner join funcionario f on f.IDFUNC = c.IDFUNC
                             inner join turno t on t.IDTURNO = c.IDTURNO
                             inner join pdv p on p.IDPDV = c.IDPDV
                             left join venda_cancel vc on v.IDVENDA = vc.IDVENDA 
                             where date(v.dthrvenda) between @dtINicial and @dtFinal";

            return _dbSession.Connection.Query<VendaReportViewModel>(query, parametros, _dbSession.Transaction).ToList();
        }

        public async Task<List<VendaFornecedorReportViewModel>> ObterVendasPorFornecedor(DateTime dataInicial, DateTime dataFinal)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@dtINicial", dataInicial, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@dtFinal", dataFinal, DbType.DateTime, ParameterDirection.Input);

            var query = $@" SELECT P.IDPRODUTO, P.NMPRODUTO as PRODUTO, SUM(TRUNCATE(VI.VLTOTAL, 2)) AS TOTAL, 
                            SUM(TRUNCATE(VI.NUQTD, 3)) AS QUANTIDADE, c.IDFORN as IDFORNECEDOR,  F.NMFANTASIA as FORNECEDOR
                            FROM venda_item VI 
                            INNER JOIN produto P ON VI.IDPRODUTO = P.IDPRODUTO 
                            INNER JOIN venda V ON VI.IDVENDA = V.IDVENDA 
                            left join compra_item CI on ci.idproduto = vi.idproduto
                            left JOIN compra C ON CI.IDCOMPRA = C.IDCOMPRA
                            left JOIN fornecedor F ON C.IDFORN = F.IDFORN 
                            WHERE  DATE(V.DTHRVENDA) between @dtINicial and @dtFinal
                            GROUP BY P.IDPRODUTO, P.NMPRODUTO,c.IDFORN
                            ORDER BY c.IDFORN,F.NMFANTASIA,P.NMPRODUTO";
            var resul = _dbSession.Connection.Query<VendaFornecedorReportViewModel>(query, parametros, _dbSession.Transaction);
            return resul.ToList();
        }

        public async Task<dynamic> ObterVendasRankingPorProduto(DateTime dataInicial, DateTime dataFinal)
        {

            using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();
                        var parametros = new DynamicParameters();
                        parametros.Add("@dataInicial", dataInicial, DbType.Date, ParameterDirection.Input);
                        parametros.Add("@dataFinal", dataFinal, DbType.Date, ParameterDirection.Input);

                        var query = $@"select sum(vi.VLTOTAL) as valor, p.NMPRODUTO as produto 
                                        from venda v
                                        inner join venda_item vi on vi.IDVENDA = v.IDVENDA
                                        inner JOIN produto p ON p.IDPRODUTO = vi.IDPRODUTO
                                        inner join prod_grupo g on p.IDGRUPO = g.IDGRUPO
                                        inner join caixa c on c.idcaixa = v.idcaixa
                                            where date(v.dthrvenda) between @dataInicial and @dataFinal
                                        group by p.NMPRODUTO";
                        var resultado = con.Query<dynamic>(query, parametros).ToList();

                        return resultado;
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally { con.Close(); }            

            }
        }

        public async Task<List<VendaReportViewModel>> ObterVendasReportViewModel(DateTime dataInicial, DateTime dataFinal)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@dtINicial", dataInicial, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@dtFinal", dataFinal, DbType.DateTime, ParameterDirection.Input);

            var query = $@"select v.IDVENDA as Id, v.SQVENDA as Sequencial, v.STVENDA as Situacao, v.VLVENDA as Valor, v.VLACRES as Acrescimo,
                            v.VLDESC as Desconto, v.VLTOTAL as Total, c.SQCAIXA as SeqCaixa,p.DSPDV as Pdv, v.DTHRVENDA as DataVenda,
                            COALESCE((SELECT SUM(VI.VLTOTAL) FROM venda_item VI WHERE VI.IDVENDA = v.IDVENDA and VI.STITEM = 2), 0) as Devolucao
                             from venda v
                             inner join caixa c on c.IDCAIXA = v.IDCAIXA 
                             inner join funcionario f on f.IDFUNC = c.IDFUNC
                             inner join turno t on t.IDTURNO = c.IDTURNO
                             inner join pdv p on p.IDPDV = c.IDPDV
                             where date(v.dthrvenda) between @dtINicial and @dtFinal";
            var resul = _dbSession.Connection.Query<VendaReportViewModel>(query, parametros, _dbSession.Transaction);
            return resul.ToList();
        }

        public async Task<List<DateTime>> ObterListaVendaAgrupadasPorData(DateTime dataInicial, DateTime dataFinal)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@dtINicial", dataInicial, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@dtFinal", dataFinal, DbType.DateTime, ParameterDirection.Input);

            var query = $@"select Distinct DATE(V.DTHRVENDA) as DTVENDA FROM venda V 
                        inner join caixa cx on V.IDCAIXA = cx.IDCAIXA 
                         where date(v.dthrvenda) between @dtINicial and @dtFinal";
            var resul = _dbSession.Connection.Query<DateTime>(query, parametros, _dbSession.Transaction);
            return resul.ToList();
        }

        public async Task<List<TotalVendaMoedaPorDataReport>> ObterVendaMoedaTotalizadasPorData(DateTime dataInicial, DateTime dataFinal)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@dtINicial", dataInicial, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@dtFinal", dataFinal, DbType.DateTime, ParameterDirection.Input);


            var query = $@"select SUM(TRUNCATE(vm.VLPAGO,2) - TRUNCATE(vm.VLTROCO,2)) valor, m.DSMOEDA as Descricao
                            from venda_moeda vm
                            inner join moeda m on m.IDMOEDA = vm.IDMOEDA
                             inner join venda v on vm.IDVENDA = v.IDVENDA 
                             inner join caixa cx on v.IDCAIXA = cx.IDCAIXA 
                             where DATE(v.DTHRVENDA) between @dtINicial and @dtFinal
                             Group by m.DSMOEDA 
                             Order by m.DSMOEDA";
            var resul = _dbSession.Connection.Query<TotalVendaMoedaPorDataReport>(query, parametros, _dbSession.Transaction);
            return resul.ToList();
        }

        public async Task<List<TotalVendaMoedaPorDataReport>> ObterVendaMoedaTotalizadasPorData(DateTime data)
        {
            return await ObterVendaMoedaTotalizadasPorData(data, data);
        }

        public async Task<List<VendaDiferencaCaixaReport>> ObterVendaDiferencaCaixa(DateTime dataInicial, DateTime dataFinal)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@dtINicial", dataInicial, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@dtFinal", dataFinal, DbType.DateTime, ParameterDirection.Input);

            var query = $@"select coalesce(c.vlabt,0) vlabt, c.IDCAIXA, coalesce(c.VLFECH,0) VLFECH,
                          c.SQCAIXA, f.NMFUNC,c.DTHRABT,c.DTHRFECH
                          from caixa c
                          inner join funcionario f on f.IDFUNC = c.IDFUNC
                          where c.vlfech <> 0
                          and date(c.DTHRFECH) between @dtINicial and @dtFinal
                            order by DTHRFECH desc";

            var resul = _dbSession.Connection.Query<VendaDiferencaCaixaReport>(query, parametros, _dbSession.Transaction);
            return resul.ToList();
        }

        public async Task<List<VendaRankingReport>> ObterVendaRankingPorData(DateTime dataInicial, DateTime dataFinal, EResultadoFiltroRanking tipoResultado, EOrdenacaoFiltroRanking ordenacao)
        {
            var campoResultado = "";
            switch (tipoResultado)
            {
                case EResultadoFiltroRanking.Grupo:
                    campoResultado = "G.NMGRUPO";
                    break;
                case EResultadoFiltroRanking.Produto:
                    campoResultado = "P.NMPRODUTO";
                    break;
                case EResultadoFiltroRanking.Data:
                    campoResultado = "DATE_FORMAT(V.DTHRVENDA, '%d%m%Y')";
                    break;
                case EResultadoFiltroRanking.DiaSemana:
                    campoResultado = $@"CASE 
                            WHEN weekday(V.DTHRVENDA) = 0 THEN 'Segunda '
                            WHEN weekday(V.DTHRVENDA) = 1 THEN 'Terça '
                            WHEN weekday(V.DTHRVENDA) = 2 THEN 'Quarta '
                            WHEN weekday(V.DTHRVENDA) = 3 THEN 'Quinta '
                            WHEN weekday(V.DTHRVENDA) = 4 THEN 'Sexta '
                            WHEN weekday(V.DTHRVENDA) = 5 THEN 'Sábado '
                            WHEN weekday(V.DTHRVENDA) = 6 THEN 'Domingo '
                         END";
                    break;
                case EResultadoFiltroRanking.Mes:
                    campoResultado = $@"CASE 
                            WHEN DATE_FORMAT(V.DTHRVENDA, '%m') = '01' THEN 'Janeiro '
                            WHEN DATE_FORMAT(V.DTHRVENDA, '%m') = '02' THEN 'Fevereiro '
                            WHEN DATE_FORMAT(V.DTHRVENDA, '%m') = '03' THEN 'Março '
                            WHEN DATE_FORMAT(V.DTHRVENDA, '%m') = '04' THEN 'Abril '
                            WHEN DATE_FORMAT(V.DTHRVENDA, '%m') = '05' THEN 'Maio '
                            WHEN DATE_FORMAT(V.DTHRVENDA, '%m') = '06' THEN 'Junho '
                            WHEN DATE_FORMAT(V.DTHRVENDA, '%m') = '07' THEN 'Julho '
                            WHEN DATE_FORMAT(V.DTHRVENDA, '%m') = '08' THEN 'Agosto '
                            WHEN DATE_FORMAT(V.DTHRVENDA, '%m') = '09' THEN 'Setembro '
                            WHEN DATE_FORMAT(V.DTHRVENDA, '%m') = '10' THEN 'Outubro '
                            WHEN DATE_FORMAT(V.DTHRVENDA, '%m') = '11' THEN 'Novembro '
                            WHEN DATE_FORMAT(V.DTHRVENDA, '%m') = '12' THEN 'Dezembro '
                         END";
                    break;
                case EResultadoFiltroRanking.Ano:
                    campoResultado = "DATE_FORMAT(V.DTHRVENDA, '%Y')";
                    break;
                default:
                    break;
            }

            var campoOrdem = "";
            switch (ordenacao)
            {
                case EOrdenacaoFiltroRanking.Venda:
                    campoOrdem= "TotalVendida";
                    break;
                case EOrdenacaoFiltroRanking.Quantidade:
                    campoOrdem = "QuantidadeVendida";
                    break;
                case EOrdenacaoFiltroRanking.Lucro:
                    campoOrdem = "LucroTotal";
                    break;
                default:
                    break;
            }

            var parametros = new DynamicParameters();
            parametros.Add("@dtINicial", dataInicial, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@dtFinal", dataFinal, DbType.DateTime, ParameterDirection.Input);

            var query = $@"SELECT {campoResultado} AS RESULTADO, 
                                 SUM(TRUNCATE(VI.NUQTD, 3)) AS QuantidadeVendida, 
                                 SUM(TRUNCATE(VI.VLTOTAL, 2)) AS TotalVendida, 
                                 SUM(TRUNCATE(TRUNCATE(VI.NUQTD, 3) * TRUNCATE(VI.VLCUSTOMEDIO, 2), 2)) AS CustoTotal, 
                                 (SUM(TRUNCATE(VI.VLTOTAL, 2)) - SUM(TRUNCATE(TRUNCATE(VI.NUQTD, 3) * TRUNCATE(VI.VLCUSTOMEDIO, 2), 2))) AS LucroTotal
                                 FROM venda_item VI 
                                 INNER JOIN produto P ON VI.IDPRODUTO = P.IDPRODUTO 
                                 inner JOIN prod_grupo G ON P.IDGRUPO = G.IDGRUPO 
                                 INNER JOIN venda V ON VI.IDVENDA = V.IDVENDA 
                                 WHERE DATE(V.DTHRVENDA) between @dtINicial and @dtFinal
                        GROUP BY RESULTADO
                        ORDER BY {campoOrdem} DESC";

            var resul = _dbSession.Connection.Query<VendaRankingReport>(query, parametros, _dbSession.Transaction);
            return resul.ToList();
        }

        public async Task<long> AdicionarVenda(Venda venda, long idEstoque, int sqcaixa,string nomeUsuario, string cpf)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var parametros = new DynamicParameters();
            var query = $@"INSERT INTO venda (IDVENDA, IDCAIXA, IDCLIENTE, SQVENDA, DTHRVENDA, NUCPFCNPJ,
                                VLVENDA, VLACRES, VLDESC, VLTOTAL, STVENDA, DSINFCOMPL,VLTOTIBPTFED, VLTOTIBPTEST, 
                                VLTOTIBPTMUN, VLTOTIBPTIMP,TPDOC, STEMISSAO, TPORIGEM)
                           VALUES (@IDVENDA, @IDCAIXA, @IDCLIENTE, @SQVENDA, @DTHRVENDA, @NUCPFCNPJ,
                                @VLVENDA, @VLACRES, @VLDESC, @VLTOTAL, @STVENDA, @DSINFCOMPL,@VLTOTIBPTFED, @VLTOTIBPTEST, 
                                @VLTOTIBPTMUN, @VLTOTIBPTIMP,@TPDOC, @STEMISSAO, @TPORIGEM)";

            parametros.Add("@IDVENDA", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDCAIXA", venda.IDCAIXA, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDCLIENTE", venda.IDCLIENTE, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@SQVENDA", venda.SQVENDA, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@NUCPFCNPJ", cpf, DbType.String, ParameterDirection.Input);
            parametros.Add("@VLVENDA", venda.VLVENDA, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLACRES", venda.VLACRES, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLDESC", venda.VLDESC, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTAL", venda.VLTOTAL, DbType.Double, ParameterDirection.Input);
            parametros.Add("@STVENDA", venda.STVENDA, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@DTHRVENDA", venda.DTHRVENDA, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@DSINFCOMPL", venda.DSINFCOMPL, DbType.String, ParameterDirection.Input);
            parametros.Add("@VLTOTIBPTFED", venda.VLTOTIBPTFED, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTIBPTEST", venda.VLTOTIBPTEST, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTIBPTMUN", venda.VLTOTIBPTMUN, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTIBPTIMP", venda.VLTOTIBPTIMP, DbType.Double, ParameterDirection.Input);
            parametros.Add("@TPDOC", venda.TPDOC, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@STEMISSAO", venda.STEMISSAO, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@TPORIGEM", venda.TPORIGEM, DbType.Int32, ParameterDirection.Input);

            if(_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0)
            {
                venda.VendaItem.ForEach(async item => {
                    if(item.IDPRODUTO.HasValue)
                        await AdicionarItemVenda(item, id,venda.SQVENDA.Value,sqcaixa,nomeUsuario,idEstoque);
                });

                venda.VendaMoeda.ForEach(async item => {
                    if(item.IDMOEDA.HasValue)
                        await AdicionarVendaMoeda(item, id);
                });
                
                //espelho venda
            }

            return id;
        }

        public async Task<long> AdicionarVendaTemporaria(Venda venda, long idEstoque, int sqcaixa, string nomeUsuario, string cpf)
        {

            var id = _utilDapperRepository.GerarUUID().Result;
            var parametros = new DynamicParameters();
            var query = $@"INSERT INTO tmp_venda (IDVENDA, IDCAIXA, IDCLIENTE, SQVENDA, DTHRVENDA, NUCPFCNPJ,VLVENDA, VLACRES, VLDESC, 
                            VLTOTAL, STVENDA, DSINFCOMPL, VLTOTIBPTFED, VLTOTIBPTEST, VLTOTIBPTMUN, VLTOTIBPTIMP,TPDOC, STEMISSAO, TPORIGEM)
                           VALUES (@IDVENDA, @IDCAIXA, @IDCLIENTE, @SQVENDA, @DTHRVENDA, @NUCPFCNPJ,@VLVENDA, @VLACRES, @VLDESC, 
                            @VLTOTAL, @STVENDA, @DSINFCOMPL, @VLTOTIBPTFED, @VLTOTIBPTEST, @VLTOTIBPTMUN, @VLTOTIBPTIMP,@TPDOC, @STEMISSAO, @TPORIGEM)";

            parametros.Add("@IDVENDA", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDCAIXA", venda.IDCAIXA, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDCLIENTE", venda.IDCLIENTE, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@SQVENDA", venda.SQVENDA, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@NUCPFCNPJ", cpf, DbType.String, ParameterDirection.Input);
            parametros.Add("@VLVENDA", venda.VLVENDA, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLACRES", venda.VLACRES, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLDESC", venda.VLDESC, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTAL", venda.VLTOTAL, DbType.Double, ParameterDirection.Input);
            parametros.Add("@STVENDA", venda.STVENDA, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@DTHRVENDA", venda.DTHRVENDA, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@DSINFCOMPL", venda.DSINFCOMPL, DbType.String, ParameterDirection.Input);
            parametros.Add("@VLTOTIBPTFED", venda.VLTOTIBPTFED, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTIBPTEST", venda.VLTOTIBPTEST, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTIBPTMUN", venda.VLTOTIBPTMUN, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTIBPTIMP", venda.VLTOTIBPTIMP, DbType.Double, ParameterDirection.Input);
            parametros.Add("@TPDOC", venda.TPDOC, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@STEMISSAO", venda.STEMISSAO, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@TPORIGEM", venda.TPORIGEM, DbType.Int32, ParameterDirection.Input);

            if (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0)
            {
                venda.VendaItem.ForEach(async item => {
                    await AdicionarItemVendaTemporaria(item, id, venda.SQVENDA.Value, sqcaixa, nomeUsuario, idEstoque);
                });

                venda.VendaMoeda.ForEach(async item => {
                    await AdicionarVendaMoedaTemporaria(item, id);
                });

                //espelho venda
            }

            return id;
        }

        public async Task<bool> RetirarEstoqueComposicao(long idProduto, string nmUsuario,int sqvenda, int sqcaixa)
        {
            var listaProdutos = await ObterProdutosComposicao(idProduto);
            try
            {
                foreach (var item in listaProdutos)
                {
                    if ((ECategoriaProduto)Enum.Parse(typeof(ECategoriaProduto), item.CTPRODUTO.ToString()) == ECategoriaProduto.Composto
                        || (ECategoriaProduto)Enum.Parse(typeof(ECategoriaProduto), item.CTPRODUTO.ToString()) == ECategoriaProduto.Combo)
                    {
                        await RetirarEstoqueComposicao(item.IDPRODUTO_COMP, nmUsuario, sqvenda, sqcaixa);
                    }
                    else
                    {
                        var descricao = $"Retirada pela venda nº {sqvenda} do caixa nº {sqcaixa} produto {item.NMPRODUTO}";
                        await _estoqueDapperRepository.RealizaEntradaRetornaIdHistoricoGerado(item.IDESTOQUE, idProduto, -1, nmUsuario, descricao, item.NUQTD);
                    }
                }

                return true;
            }
            catch 
            {

                return false;
            }
           
        }

        public async Task<int> GerarNuNf(long idEmpresa, bool homologacao)
        {
            var result = -1;
            var modoEmissao = homologacao ? "NFCE_NUNF_HOMOL" : "NFCE_NUNF";
            var parametros = new DynamicParameters();
            parametros.Add("@IDEMPRESA", idEmpresa, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@CHAVE", modoEmissao, DbType.String, ParameterDirection.Input);

            var query = $@"SELECT COALESCE(SEQUENCIAL, -1) SEQUENCIAL FROM seq 
                            WHERE IDEMPRESA = @IDEMPRESA AND CHAVE = @CHAVE FOR UPDATE";

            result = _dbSession.Connection.Query<int>(query, parametros, _dbSession.Transaction).FirstOrDefault();

            if(result < 0)
            {
                result = 1;
                parametros.Add("@SEQUENCIAL", result, DbType.Int32, ParameterDirection.Input);
                query = $@"INSERT INTO seq (CHAVE, IDEMPRESA, SEQUENCIAL) values (@CHAVE, @IDEMPRESA, @SEQUENCIAL)";

                if (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) == 0)
                    result = -1;
            }
            else
            {
                result++;
                parametros.Add("@SEQUENCIAL", result, DbType.Int32, ParameterDirection.Input);
                query = $@"UPDATE seq SET SEQUENCIAL = @SEQUENCIAL WHERE CHAVE = @CHAVE AND IDEMPRESA = @IDEMPRESA";
                if (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) == 0)
                    result = -1;                
            }
            return result;
        }

        public async Task<Venda> ObterVendaPorId(long idVenda)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDVENDA", idVenda, DbType.DateTime, ParameterDirection.Input);

            var query = $@"SELECT v.IDVENDA as Id, v.* FROM venda v WHERE v.IDVENDA = @IDVENDA";

            return _dbSession.Connection.Query<Venda>(query, parametros, _dbSession.Transaction).FirstOrDefault();            
        }

        public async Task<IEnumerable<VendaItem>> ObterItensVendaPorIdVenda(long idVenda)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDVENDA", idVenda, DbType.DateTime, ParameterDirection.Input);

            var query = $@"SELECT I.IDVENDA_ITEM as Id, I.* FROM venda_item I WHERE I.IDVENDA = @IDVENDA ORDER BY I.SQITEM";

            var itens = _dbSession.Connection.Query<VendaItem>(query, parametros, _dbSession.Transaction);
            itens.ToList().ForEach(item => {
                var parametrosItem = new DynamicParameters();
                
                parametrosItem.Add("@IDPRODUTO", item.IDPRODUTO.HasValue ? item.IDPRODUTO.Value : 0, DbType.Int64, ParameterDirection.Input);
                var queryProduto = $@"select p.IDPRODUTO as Id, p.* from produto p where p.IDPRODUTO = @IDPRODUTO";
                var produto = _dbSession.Connection.Query<Produto>(queryProduto, parametrosItem, _dbSession.Transaction).FirstOrDefault();
                if(produto != null)
                    item.AdicionarProduto(produto);                
            });

            return itens;
        }

        public async Task<IEnumerable<VendaMoeda>> ObterMoedaVendaPorIdVenda(long idVenda)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDVENDA", idVenda, DbType.DateTime, ParameterDirection.Input);

            var query = $@"SELECT VM.IDVENDA_MOEDA as Id, VM.* FROM venda_moeda VM where VM.IDVENDA =@IDVENDA";

            var moedas = _dbSession.Connection.Query<VendaMoeda>(query, parametros, _dbSession.Transaction);
            moedas.ToList().ForEach(item => {
                var parametrosItem = new DynamicParameters();

                parametrosItem.Add("@IDMOEDA", item.IDMOEDA.HasValue ? item.IDMOEDA.Value : 0, DbType.Int64, ParameterDirection.Input);
                var queryProduto = $@" select M.IDMOEDA as Id,M.* from  moeda M where M.IDMOEDA = @IDMOEDA";
                var moeda = _dbSession.Connection.Query<Moeda>(queryProduto, parametrosItem, _dbSession.Transaction).FirstOrDefault();
                if (moeda != null)
                    item.AdicionarMoeda(moeda);
            });

            return moedas;
        }

        public async Task<Empresa> ObterEmpresaPorId(long id)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDEMPRESA", id, DbType.Int64, ParameterDirection.Input);

            var query = $@"SELECT e.idempresa as Id ,e.* FROM empresa e WHERE e.IDEMPRESA = @IDEMPRESA";

            return _dbSession.Connection.Query<Empresa>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<PontoVenda> ObterPdvPorId(long id)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDPDV", id, DbType.Int64, ParameterDirection.Input);

            var query = $@"select p.IDPDV as Id,p.* from pdv p where p.IDPDV = @IDPDV";

            return _dbSession.Connection.Query<PontoVenda>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }
        
        public async Task<Endereco> ObterEnderecoPorId(long id)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDENDERECO", id, DbType.Int64, ParameterDirection.Input);

            var query = $@"SELECT e.IDENDERECO as Id,e.* FROM endereco e WHERE e.IDENDERECO = @IDENDERECO";

            return _dbSession.Connection.Query<Endereco>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<Cliente> ObterClientePorId(long id)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDCLIENTE", id, DbType.Int64, ParameterDirection.Input);

            var query = $@" SELECT C.IDCLIENTE as Id, C.* FROM cliente C WHERE C.IDCLIENTE =@IDCLIENTE";

            var cliente = _dbSession.Connection.Query<Cliente>(query, parametros, _dbSession.Transaction).FirstOrDefault();
            if(cliente != null)
            {
                if (!cliente.IDENDERECO.HasValue)
                {
                    var endereco = await ObterEnderecoPorId(cliente.IDENDERECO.Value);
                    if(endereco != null)
                    {
                        cliente.AdicionarEndereco(endereco);
                    }
                }
            }

            return cliente;
        }

        public async Task<int> GerarSqVendaPorCaixa(long id)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDCAIXA", id, DbType.Int64, ParameterDirection.Input);

            var query = $@"SELECT (COALESCE(MAX(SQVENDA), 0) + 1) AS SQVENDA FROM venda WHERE IDCAIXA = @IDCAIXA";

            return _dbSession.Connection.Query<int>(query, parametros).FirstOrDefault();
        }

        public async Task<bool> ApagarVendaTemporaria(long id)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDVENDA", id, DbType.Int64, ParameterDirection.Input);
            var query = $@"DELETE FROM tmp_venda_moeda WHERE IDVENDA = @IDVENDA;
                            DELETE FROM tmp_venda_espelho WHERE IDVENDA = @IDVENDA;
                            DELETE FROM tmp_venda_item WHERE IDVENDA = @IDVENDA;
                            DELETE FROM tmp_venda WHERE IDVENDA = @IDVENDA;";
            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
            
        }

        #region privados

        private async Task<bool> AdicionarItemVenda(VendaItem item, long idVenda, int sqvenda, int sqcaixa, string nomeUsuario, long idEstoque)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var parametros = new DynamicParameters();
            var query = $@"INSERT INTO venda_item (IDVENDA_ITEM, IDVENDA, IDPRODUTO, SQITEM, VLUNIT, 
                                                    NUQTD, VLITEM, VLACRES, VLDESC, VLTOTAL, VLCUSTOMEDIO, 
                                                    STITEM, PCIBPTFED, PCIBPTEST, PCIBPTMUN, PCIBPTIMP)
                            values (@IDVENDA_ITEM, @IDVENDA, @IDPRODUTO, @SQITEM, @VLUNIT, 
                                    @NUQTD, @VLITEM, @VLACRES, @VLDESC, @VLTOTAL, @VLCUSTOMEDIO, 
                                    @STITEM, @PCIBPTFED, @PCIBPTEST, @PCIBPTMUN, @PCIBPTIMP)";

            parametros.Add("@IDVENDA_ITEM", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDVENDA", idVenda, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPRODUTO", item.IDPRODUTO, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@SQITEM", item.SQITEM, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@VLUNIT", item.VLUNIT, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLITEM", item.VLITEM, DbType.Double, ParameterDirection.Input);
            parametros.Add("@NUQTD", item.NUQTD, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLDESC", item.VLDESC, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLACRES", item.VLACRES, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLCUSTOMEDIO", item.VLCUSTOMEDIO, DbType.Double, ParameterDirection.Input);
            parametros.Add("@STITEM", item.STITEM, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@PCIBPTEST", item.PCIBPTEST, DbType.Double, ParameterDirection.Input);
            parametros.Add("@PCIBPTFED", item.PCIBPTFED, DbType.Double, ParameterDirection.Input);
            parametros.Add("@PCIBPTMUN", item.PCIBPTMUN, DbType.Double, ParameterDirection.Input);
            parametros.Add("@PCIBPTIMP", item.PCIBPTIMP, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTAL", item.VLTOTAL, DbType.Double, ParameterDirection.Input);

            if (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0)
            {
                var produto = await ObterProdutosPorId(item.IDPRODUTO.Value);
                if (produto != null)
                {
                    if ((ECategoriaProduto)Enum.Parse(typeof(ECategoriaProduto), produto.CTPRODUTO.ToString()) == ECategoriaProduto.Composto
                     || (ECategoriaProduto)Enum.Parse(typeof(ECategoriaProduto), produto.CTPRODUTO.ToString()) == ECategoriaProduto.Combo)
                    {
                        await RetirarEstoqueComposicao(produto.Id, nomeUsuario, sqvenda, sqcaixa);
                    }
                    else
                    {
                        var descricao = $"Retirada pela venda nº {sqvenda} do caixa nº {sqcaixa} produto {produto.NMPRODUTO}";
                        await _estoqueDapperRepository.RealizaEntradaRetornaIdHistoricoGerado(idEstoque, item.IDPRODUTO.Value, -1, nomeUsuario, descricao, item.NUQTD.Value);
                    }
                    return true;
                }
                else
                    return false;
            }
            return false;
        }

        private async Task<bool> AdicionarVendaMoeda(VendaMoeda item, long idVenda)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var parametros = new DynamicParameters();
            var query = $@"INSERT INTO venda_moeda (IDVENDA_MOEDA, IDVENDA, IDMOEDA, VLPAGO, VLTROCO, NSU, NUPARCELAS)
                        values (@IDVENDA_MOEDA, @IDVENDA, @IDMOEDA, @VLPAGO, @VLTROCO, @NSU, @NUPARCELAS)";
            parametros.Add("@IDVENDA_MOEDA", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDVENDA", idVenda, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDMOEDA", item.IDMOEDA, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@VLPAGO", item.VLPAGO, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTROCO", item.VLTROCO, DbType.Double, ParameterDirection.Input);
            parametros.Add("@NSU", item.NSU, DbType.String, ParameterDirection.Input);
            parametros.Add("@NUPARCELAS", item.NUPARCELAS, DbType.Int32, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        private async Task<bool> AdicionarVendaEspelho(string vendaEspelho, long idVenda)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var parametros = new DynamicParameters();
            var query = $@"INSERT INTO venda_espelho (IDESPELHO, IDVENDA, DSESPELHO) values (@IDESPELHO, @IDVENDA, @DSESPELHO)";
            parametros.Add("@IDESPELHO", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDVENDA", idVenda, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@DSESPELHO", vendaEspelho, DbType.String, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        private async Task<bool> AdicionarItemVendaTemporaria(VendaItem item, long idVenda, int sqvenda, int sqcaixa, string nomeUsuario, long idEstoque)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var parametros = new DynamicParameters();
            var query = $@"INSERT INTO tmp_venda_item (IDVENDA_ITEM, IDVENDA, IDPRODUTO, SQITEM, VLUNIT, 
                                                    NUQTD, VLITEM, VLACRES, VLDESC, VLTOTAL, VLCUSTOMEDIO, 
                                                    STITEM, PCIBPTFED, PCIBPTEST, PCIBPTMUN, PCIBPTIMP)
                            values (@IDVENDA_ITEM, @IDVENDA, @IDPRODUTO, @SQITEM, @VLUNIT, 
                                    @NUQTD, @VLITEM, @VLACRES, @VLDESC, @VLTOTAL, @VLCUSTOMEDIO, 
                                    @STITEM, @PCIBPTFED, @PCIBPTEST, @PCIBPTMUN, @PCIBPTIMP)";

            parametros.Add("@IDVENDA_ITEM", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDVENDA", idVenda, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPRODUTO", item.IDPRODUTO, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@SQITEM", item.SQITEM, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@VLUNIT", item.VLUNIT, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLITEM", item.VLITEM, DbType.Double, ParameterDirection.Input);
            parametros.Add("@NUQTD", item.NUQTD, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLDESC", item.VLDESC, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLACRES", item.VLACRES, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLCUSTOMEDIO", item.VLCUSTOMEDIO, DbType.Double, ParameterDirection.Input);
            parametros.Add("@STITEM", item.STITEM, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@PCIBPTEST", item.PCIBPTEST, DbType.Double, ParameterDirection.Input);
            parametros.Add("@PCIBPTFED", item.PCIBPTFED, DbType.Double, ParameterDirection.Input);
            parametros.Add("@PCIBPTMUN", item.PCIBPTMUN, DbType.Double, ParameterDirection.Input);
            parametros.Add("@PCIBPTIMP", item.PCIBPTIMP, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTAL", item.VLTOTAL, DbType.Double, ParameterDirection.Input);

            if (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0)
            {
                var produto = await ObterProdutosPorId(item.IDPRODUTO.Value);
                if (produto != null)
                {
                    if ((ECategoriaProduto)Enum.Parse(typeof(ECategoriaProduto), produto.CTPRODUTO.ToString()) == ECategoriaProduto.Composto
                     || (ECategoriaProduto)Enum.Parse(typeof(ECategoriaProduto), produto.CTPRODUTO.ToString()) == ECategoriaProduto.Combo)
                    {
                        await RetirarEstoqueComposicao(produto.Id, nomeUsuario, sqvenda, sqcaixa);
                    }
                    else
                    {
                        var descricao = $"Retirada pela venda nº {sqvenda} do caixa nº {sqcaixa} produto {produto.NMPRODUTO}";
                        await _estoqueDapperRepository.RealizaEntradaRetornaIdHistoricoGerado(idEstoque, item.IDPRODUTO.Value, -1, nomeUsuario, descricao, item.NUQTD.Value);
                    }
                    return true;
                }
                else
                    return false;
            }
            return false;
        }

        private async Task<bool> AdicionarVendaMoedaTemporaria(VendaMoeda item, long idVenda)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var parametros = new DynamicParameters();
            var query = $@"INSERT INTO tmp_venda_moeda (IDVENDA_MOEDA, IDVENDA, IDMOEDA, VLPAGO, VLTROCO, NSU, NUPARCELAS)
                        values (@IDVENDA_MOEDA, @IDVENDA, @IDMOEDA, @VLPAGO, @VLTROCO, @NSU, @NUPARCELAS)";
            parametros.Add("@IDVENDA_MOEDA", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDVENDA", idVenda, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDMOEDA", item.IDMOEDA, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@VLPAGO", item.VLPAGO, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTROCO", item.VLTROCO, DbType.Double, ParameterDirection.Input);
            parametros.Add("@NSU", item.NSU, DbType.String, ParameterDirection.Input);
            parametros.Add("@NUPARCELAS", item.NUPARCELAS, DbType.Int32, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        private async Task<bool> AdicionarVendaTemporariaEspelho(string vendaEspelho, long idVenda)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var parametros = new DynamicParameters();
            var query = $@"INSERT INTO venda_espelho (IDESPELHO, IDVENDA, DSESPELHO) values (@IDESPELHO, @IDVENDA, @DSESPELHO)";
            parametros.Add("@IDESPELHO", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDVENDA", idVenda, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@DSESPELHO", vendaEspelho, DbType.String, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }
        
        private async Task<IEnumerable<ProdutoComposicaoViewModel>> ObterProdutosComposicao(long idProduto)
        {
            var parametros = new DynamicParameters();

            var query = $@"SELECT PC.IDPRODUTO_COMP, PC.IDESTOQUE, PC.NUQTD, P.CTPRODUTO, P.NMPRODUTO FROM prod_composicao PC 
                            INNER JOIN produto P ON PC.IDPRODUTO_COMP = P.IDPRODUTO 
                            WHERE PC.IDPRODUTO = @IDPRODUTO";
            parametros.Add("@IDPRODUTO", idProduto, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Query<ProdutoComposicaoViewModel>(query, parametros, _dbSession.Transaction);
        }
        
        private async Task<Produto> ObterProdutosPorId(long idProduto)
        {
            var parametros = new DynamicParameters();

            var query = $@"SELECT P.IDPRODUTO as Id, P.* FROM produto P WHERE P.IDPRODUTO = @IDPRODUTO";
            parametros.Add("@IDPRODUTO", idProduto, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Query<Produto>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }


        #endregion
    }

    internal class ProdutoComposicaoViewModel
    {
        public long IDPRODUTO_COMP { get; set; }
        public long IDESTOQUE { get; set; }
        public long NUQTD { get; set; }
        public int CTPRODUTO { get; set; }
        public string NMPRODUTO { get; set; }
    }
}

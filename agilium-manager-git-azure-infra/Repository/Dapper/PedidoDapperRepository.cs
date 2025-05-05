using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.PedidoViewModel;
using agilium.api.business.Models.CustomReturn.ProdutoReturnViewModel;
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
    public class PedidoDapperRepository : IPedidoDapperRepository
    {
        private readonly IUtilDapperRepository _utilDapperRepository;
        protected readonly IConfiguration _configuration;
        private readonly DbSession _dbSession;

        public PedidoDapperRepository(IUtilDapperRepository utilDapperRepository, IConfiguration configuration, DbSession dbSession)
        {
            _utilDapperRepository = utilDapperRepository;
            _configuration = configuration;
            _dbSession = dbSession;
        }

        public async Task<IEnumerable<PedidoListaViewModel>> ObterListaPedido(DateTime dataInicial, DateTime dataFinal, 
            string numeroPedido = null, string nomeCliente = null, string nomeEntregador = null, string bairroEntrega = null)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@DtIni", dataInicial, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@DtFim", dataFinal, DbType.DateTime, ParameterDirection.Input);

            var query = $@"SELECT P.*,P.IDPEDIDO as Id, C.NMCLIENTE, P.IDFUNC as IDFuncionario,
                                group_concat(distinct CT.DESCR1, ' / ') AS CONTATOS, 
                                E.ENDER as Logradouro, E.NUM, E.COMPL, E.BAIRRO, E.CEP, E.CIDADE, E.UF, E.DSPTREF, 
                                CX.SQCAIXA, V.SQVENDA, V.IDVENDA, V.STEMISSAO,
                                F.NMFUNC 
                        FROM pedido P
                        LEFT JOIN pedido_venda PV ON P.IDPEDIDO = PV.IDPEDIDO
                        LEFT JOIN venda V ON PV.IDVENDA = V.IDVENDA
                        INNER JOIN caixa CX ON P.IDCAIXA = CX.IDCAIXA
                        INNER JOIN cliente C ON P.IDCLIENTE = C.IDCLIENTE
                        LEFT JOIN cli_contato CC ON C.IDCLIENTE = CC.IDCLIENTE
                        LEFT JOIN contato CT ON CC.IDCONTATO = CT.IDCONTATO
                        LEFT JOIN endereco E ON P.IDENDERECO = E.IDENDERECO
                        LEFT JOIN funcionario F ON P.IDFUNC = F.IDFUNC
                    where p.DTPEDIDO between @DtIni and @DtFim";

            if (!string.IsNullOrEmpty(nomeCliente))
            {
                query += " and C.NMCLIENTE like CONCAT('%',@nmCliente,'%') ";
                parametros.Add("@nmCliente", nomeCliente, DbType.String, ParameterDirection.Input);
            }

            if (!string.IsNullOrEmpty(nomeEntregador))
            {
                query += " and F.NMFUNC like CONCAT('%',@nmFunc,'%') ";
                parametros.Add("@nmFunc", nomeEntregador, DbType.String, ParameterDirection.Input);
            }

            if (!string.IsNullOrEmpty(bairroEntrega))
            {
                query += " and E.BAIRRO like CONCAT('%',@bairro,'%')  ";
                parametros.Add("@bairro", bairroEntrega, DbType.String, ParameterDirection.Input);
            }

            if (!string.IsNullOrEmpty(numeroPedido))
            {
                query += " and P.CDPEDIDO = @numero ";
                parametros.Add("@numero", numeroPedido, DbType.String, ParameterDirection.Input);
            }

            query += @" GROUP BY P.IDPEDIDO , P.IDFUNC,E.ENDER, E.NUM, E.COMPL, E.BAIRRO, E.CEP, E.CIDADE, E.UF, E.DSPTREF, 
                                CX.SQCAIXA, V.SQVENDA, V.IDVENDA, V.STEMISSAO, F.NMFUNC";
            query += " ORDER BY P.CDPEDIDO DESC";

            return _dbSession.Connection.Query<PedidoListaViewModel>(query, parametros, _dbSession.Transaction);
        }

        public async Task<IEnumerable<PedidoItemListaViewModel>> ObterListaItemPedido(long idpedido)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@idpedido", idpedido, DbType.Int64, ParameterDirection.Input);
            
            var query = $@"  SELECT PI.IDITEMPEDIDO as Id, PI.*, P.NMPRODUTO as DsProduto
                               FROM pedido_item PI
                              INNER JOIN produto P ON PI.IDPRODUTO = P.IDPRODUTO
                              WHERE PI.IDPEDIDO = @idpedido
                            ORDER BY PI.SQITEMPEDIDO;";
            return _dbSession.Connection.Query<PedidoItemListaViewModel>(query, parametros, _dbSession.Transaction);
        }

        public async Task<IEnumerable<PedidoFormaPagamentoListaViewModel>> ObterListaFormaPagamentoPedido(long idpedido)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@idpedido", idpedido, DbType.Int64, ParameterDirection.Input);

            var query = $@"SELECT PP.IDPEDIDOPAG as Id, PP.IDFORMAPAG AS IDFormaPagamento, 
                            PP.*, M.DSMOEDA ,PP.VLPAG as VLPagamento, PP.VLTROCO as VLTroco
                            FROM pedido_pagamento PP
                              INNER JOIN moeda M ON PP.IDMOEDA = M.IDMOEDA
                              WHERE PP.IDPEDIDO = @idpedido
                              ORDER BY M.DSMOEDA";
            return _dbSession.Connection.Query<PedidoFormaPagamentoListaViewModel>(query, parametros, _dbSession.Transaction);
        }

        public async Task<PedidosEstatisticasListaViewModel> ObterEstatistica()
        {
            var parametros = new DynamicParameters();
            parametros.Add("@stCaixa", ESituacaoCaixa.Aberto, DbType.Int32, ParameterDirection.Input);

            var query = $@" SELECT P.STPEDIDO, COUNT(*) AS TOTAL 
                              FROM pedido P 
                          INNER JOIN caixa CX ON P.IDCAIXA = CX.IDCAIXA 
                           WHERE CX.STCAIXA = @stCaixa
                           GROUP BY P.STPEDIDO";
            var lista = _dbSession.Connection.Query<PedidoListaSituacaoViewModel>(query, parametros, _dbSession.Transaction);
         
            var pedidosEstatisticasListaViewModel = new PedidosEstatisticasListaViewModel();
            foreach (var item in lista)
            {
                switch (item.StPedido)
                {   
                    case ESituacaoPedido.Aprovado:
                        pedidosEstatisticasListaViewModel.PedidosConcluido += item.Total;
                        break;
                    case ESituacaoPedido.Cancelado:
                        pedidosEstatisticasListaViewModel.PedidosCancelado += item.Total;
                        break;
                    default:
                        pedidosEstatisticasListaViewModel.PedidosGeral += item.Total;
                        break;
                }
            }

            return pedidosEstatisticasListaViewModel;
        }

        public async Task<IEnumerable<Cliente>> ObterTodosClientes(string nome)
        {
            var parametros = new DynamicParameters();

            var query = $@"SELECT IDCLIENTE as Id,  cliente.*
                              FROM cliente 
                              WHERE TPCLIENTE = 'F' AND STCLIENTE = 1";

            if (!string.IsNullOrEmpty(nome))
            {
                parametros.Add("@nome", nome, DbType.String, ParameterDirection.Input);
                query += " AND NMCLIENTE like CONCAT('%',@nome,'%')";
            }

            query += " ORDER BY NMCLIENTE";
            var lista = _dbSession.Connection.Query<Cliente>(query, parametros, _dbSession.Transaction);
            if(lista.Any())
            {
                lista.ToList().ForEach(async item => {
                    if (item.IDENDERECO.HasValue)
                        item.AdicionarEndereco(await ObterEnderecoPorCliente(item.IDENDERECO.Value));
                });
            }

            return lista;
        }

        private async Task<Endereco> ObterEnderecoPorCliente(long idCliente)
        {
            var parametros = new DynamicParameters();

            var query = $@"SELECT end.IDENDERECO as Id,end.ender as logradouro,end.* from endereco end
                            inner join cliente cl on (cl.IDENDERECO = end.IDENDERECO)
                            where IDCLIENTE = @idcliente";
            parametros.Add("@idcliente", idCliente, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Query<Endereco>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<IEnumerable<Endereco>> ObterEnderecosPorCliente(long idCliente)
        {
            var parametros = new DynamicParameters();

            var query = $@"SELECT end.IDENDERECO as Id,end.*, end.ender as logradouro from endereco end
                            inner join cliente cl on (cl.IDENDERECOENTREGA = end.IDENDERECO or cl.IDENDERECO = end.IDENDERECO)
                            where IDCLIENTE = @idcliente";
            parametros.Add("@idcliente", idCliente, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Query<Endereco>(query, parametros, _dbSession.Transaction);
        }
        public async Task<IEnumerable<Produto>> ObterTodosProdutos(string descricao)
        {
            var parametros = new DynamicParameters();

            var query = $@"SELECT P.IDPRODUTO as Id, P.*
                             FROM produto P 
                             INNER JOIN prod_grupo G ON P.IDGRUPO = G.IDGRUPO 
                             WHERE P.STPRODUTO = 1
                             AND P.CTPRODUTO IN (1,2,3)
                             AND (P.NMPRODUTO) LIKE CONCAT('%',@descricao,'%') 
                             OR (G.NMGRUPO) LIKE CONCAT('%',@descricao,'%')
                             OR (P.CDPRODUTO = @descricao) 
                             OR (P.IDPRODUTO IN (SELECT B.IDPRODUTO FROM prod_barra B WHERE B.CDBARRA LIKE CONCAT('%',@descricao,'%'))) 
                             ORDER BY P.NMPRODUTO";

            parametros.Add("@descricao", descricao, DbType.String, ParameterDirection.Input);

            return _dbSession.Connection.Query<Produto>(query, parametros);
        }


        public async Task<IEnumerable<ProdutoPesqReturnViewModel>> ObterProdutosPorDescricao(string descricao)
        {
            var parametros = new DynamicParameters();

            var query = $@"SELECT P.IDPRODUTO as Id, P.IDEMPRESA, P.IDGRUPO,P.CDPRODUTO as Codigo, P.NMPRODUTO as Nome, p.CTPRODUTO as Categoria,
                                P.TPPRODUTO as Tipo, P.UNVENDA as UnidadeVenda,P.NUPRECO, P.STPRODUTO as Situacao,P.VLCUSTOMEDIO as ValorCustoMedio
                                , B.CDBARRA as CodigoBarra,G.NMGRUPO as Grupo, P.NUPRECO as Preco
                             FROM produto P 
                             left JOIN prod_grupo G ON P.IDGRUPO = G.IDGRUPO 
                             left join prod_barra B on B.IDPRODUTO = P.IDPRODUTO
                             WHERE P.STPRODUTO = 1
                             AND P.CTPRODUTO IN (1,2,3)
                             AND (P.NMPRODUTO) LIKE CONCAT('%',@descricao,'%') 
                             OR (G.NMGRUPO) LIKE CONCAT('%',@descricao,'%')
                             OR (P.CDPRODUTO = @descricao) 
                             OR (B.CDBARRA LIKE CONCAT('%',@descricao,'%')) 
                             ORDER BY P.NMPRODUTO";

            if (descricao == null) descricao = string.Empty;

            parametros.Add("@descricao", descricao, DbType.String, ParameterDirection.Input);

            return _dbSession.Connection.Query<ProdutoPesqReturnViewModel>(query, parametros);
        }

        public async Task<ProdutoPesqReturnViewModel> ObterProdutosPorDescricaoCodigoCodBarra(string descricao)
        {
            var query = $@"SELECT P.IDPRODUTO as Id, P.IDEMPRESA, P.IDGRUPO,P.CDPRODUTO as Codigo, P.NMPRODUTO as Nome, p.CTPRODUTO as Categoria,
                                P.TPPRODUTO as Tipo, P.UNVENDA as UnidadeVenda,P.NUPRECO, P.STPRODUTO as Situacao,P.VLCUSTOMEDIO as ValorCustoMedio,P.NUPRECO as Preco
                            FROM produto P WHERE P.IDPRODUTO IN 
                            (SELECT IDPRODUTO FROM prod_barra WHERE CDBARRA = @descricao) AND  P.STPRODUTO = 1";

            var produto = await ObterProdutosPorDescricaoCodigoCodBarra(query,descricao);
            
            if(produto != null)
                return produto;

            query = $@"SELECT P.IDPRODUTO as Id, P.IDEMPRESA, P.IDGRUPO,P.CDPRODUTO as Codigo, P.NMPRODUTO as Nome, p.CTPRODUTO as Categoria,
                                P.TPPRODUTO as Tipo, P.UNVENDA as UnidadeVenda,P.NUPRECO, P.STPRODUTO as Situacao,P.VLCUSTOMEDIO as ValorCustoMedio,P.NUPRECO as Preco
FROM produto P WHERE P.CDPRODUTO = @descricao AND P.STPRODUTO = 1";
            produto = await ObterProdutosPorDescricaoCodigoCodBarra(query, descricao);

            if (produto != null)
                return produto;

            query = $@"SELECT P.IDPRODUTO as Id, P.IDEMPRESA, P.IDGRUPO,P.CDPRODUTO as Codigo, P.NMPRODUTO as Nome, p.CTPRODUTO as Categoria,
                                P.TPPRODUTO as Tipo, P.UNVENDA as UnidadeVenda,P.NUPRECO, P.STPRODUTO as Situacao,P.VLCUSTOMEDIO as ValorCustoMedio,P.NUPRECO as Preco
FROM produto P WHERE P.NMPRODUTO = @descricao AND P.STPRODUTO = 1";
            produto = await ObterProdutosPorDescricaoCodigoCodBarra(query, descricao);

            return produto;

        }

        private async Task<ProdutoPesqReturnViewModel> ObterProdutosPorDescricaoCodigoCodBarra(string query,string descricao)
        {
            var parametros = new DynamicParameters();

            parametros.Add("@descricao", descricao, DbType.String, ParameterDirection.Input);

            return _dbSession.Connection.Query<ProdutoPesqReturnViewModel>(query, parametros).FirstOrDefault();
        }


        public async Task<IEnumerable<Funcionario>> ObterEntregadoresPorEmpresa(long idempresa)
        {
            var parametros = new DynamicParameters();
            var query = $@"SELECT F.IDFUNC as Id, F.* FROM funcionario F
                            WHERE F.IDEMPRESA = @idempresa 
                            AND F.STFUNC = 1
                            AND F.TPFUNCAO = 2
                            ORDER BY F.NMFUNC";
            parametros.Add("@idempresa", idempresa, DbType.Int64, ParameterDirection.Input);
            
            return _dbSession.Connection.Query<Funcionario>(query, parametros, _dbSession.Transaction);
        }

        public async Task<IEnumerable<Moeda>> ObterMoedas(long idempresa)
        {
            var parametros = new DynamicParameters();

            var query = $@"SELECT IDMOEDA as Id, moeda.* FROM moeda 
                            WHERE IDEMPRESA = @idempresa
                            AND STMOEDA = 1
                            ORDER BY DSMOEDA";
            parametros.Add("@idempresa", idempresa, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Query<Moeda>(query, parametros, _dbSession.Transaction);
        }

        public async Task<bool> AdicionarEndereco(Endereco endereco)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var query = $@"INSERT INTO endereco (IDENDERECO,ENDER,NUM,COMPL,BAIRRO,CEP,CIDADE,UF,PAIS,IBGE,DSPTREF,DTHRATU)
                           VALUES (@IDENDERECO,@ENDER,@NUM,@COMPL,@BAIRRO,@CEP,@CIDADE,@UF,@PAIS,@IBGE,@DSPTREF,@DTHRATU)";

            var parametros = new DynamicParameters();
            parametros.Add("@IDENDERECO", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@ENDER", endereco.Logradouro, DbType.String, ParameterDirection.Input);
            parametros.Add("@NUM", endereco.Numero, DbType.String, ParameterDirection.Input);
            parametros.Add("@COMPL", endereco.Complemento, DbType.String, ParameterDirection.Input);
            parametros.Add("@BAIRRO", endereco.Bairro, DbType.String, ParameterDirection.Input);
            parametros.Add("@CEP", endereco.Cep, DbType.String, ParameterDirection.Input);
            parametros.Add("@CIDADE", endereco.Cidade, DbType.String, ParameterDirection.Input);
            parametros.Add("@UF", endereco.Uf, DbType.String, ParameterDirection.Input);
            parametros.Add("@PAIS", "Brasil", DbType.String, ParameterDirection.Input);
            parametros.Add("@IBGE", null, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@DSPTREF", endereco.PontoReferencia, DbType.String, ParameterDirection.Input);
            parametros.Add("@DTHRATU", DateTime.Now, DbType.DateTime, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;

        }
        public async Task<bool> AdicionarCliente(Cliente cliente)
        {

            var id = _utilDapperRepository.GerarUUID().Result;
            var codigo = _utilDapperRepository.GerarCodigo($"SELECT MAX(CAST(CDCLIENTE AS UNSIGNED)) AS CD FROM cliente").Result;

            var query = $@"INSERT INTO cliente
                            (IDCLIENTE,CDCLIENTE,NMCLIENTE,TPCLIENTE,DTCAD,IDENDERECO,IDENDERECOCOB,IDENDERECOFAT,
                            IDENDERECOENTREGA,STCLIENTE,STPUBEMAIL,STPUBSMS)
                            VALUES
                            (@IDCLIENTE,@CDCLIENTE,@NMCLIENTE,@TPCLIENTE,@DTCAD,@IDENDERECO,@IDENDERECOCOB,@IDENDERECOFAT,
                            @IDENDERECOENTREGA,@STCLIENTE,@STPUBEMAIL,@STPUBSMS)";

            var parametros = new DynamicParameters();
            parametros.Add("@IDCLIENTE", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@CDCLIENTE", codigo, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@NMCLIENTE", cliente.NMCLIENTE, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@TPCLIENTE", ETipoPessoa.F, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@DTCAD", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@IDENDERECO", cliente.IDENDERECO, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDENDERECOCOB", cliente.IDENDERECOCOB, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDENDERECOFAT", cliente.IDENDERECOFAT, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDENDERECOENTREGA", cliente.IDENDERECONTREGA, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@STCLIENTE", EAtivo.Ativo, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@STPUBEMAIL", ESimNao.Sim, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@STPUBSMS", ESimNao.Nao, DbType.Int32, ParameterDirection.Input);

            if(_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0)
            {
                var queryClienteFP = $@"INSERT INTO clientepf (IDCLIENTE) VALUES (@IDCLIENTE)";
                var parametrosFP = new DynamicParameters();

                parametrosFP.Add("@IDCLIENTE", id, DbType.Int64, ParameterDirection.Input);
                return _dbSession.Connection.Execute(queryClienteFP, parametrosFP, _dbSession.Transaction) > 0;
            }
            return false;

        }

        public async Task<bool> AdicionarPedido(Pedido pedido)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var codigo = _utilDapperRepository.GerarCodigo($"SELECT MAX(CAST(CDPEDIDO AS UNSIGNED)) AS CD FROM pedido").Result;
            var parametros = new DynamicParameters();

            var query = $@"INSERT INTO pedido (IDPEDIDO,IDEMPRESA,IDFUNC,IDCLIENTE,IDENDERECO,IDCAIXA,IDPDV,CDPEDIDO,DTPEDIDO,STPEDIDO,VLPEDIDO,
                                   VLACRES,VLDESC,VLOUTROS,VLTOTAL,DSOBS,NUDISTANCIA,DTHRCONCLUSAO)
                           VALUES (@IDPEDIDO,@IDEMPRESA,@IDFUNC,@IDCLIENTE,@IDENDERECO,@IDCAIXA,@IDPDV,@CDPEDIDO,@DTPEDIDO,@STPEDIDO,@VLPEDIDO,
                                    @VLACRES,@VLDESC,@VLOUTROS,@VLTOTAL,@DSOBS,@NUDISTANCIA,@DTHRCONCLUSAO)";
            parametros.Add("@IDPEDIDO", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDEMPRESA", pedido.IDEmpresa, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDFUNC", pedido.IDFuncionario, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDCLIENTE", pedido.IDCliente, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDENDERECO", pedido.IDEndereco, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDCAIXA", pedido.IDCaixa, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPDV", pedido.IDPDV, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@CDPEDIDO", codigo, DbType.String, ParameterDirection.Input);
            parametros.Add("@DTPEDIDO", pedido.DTPedido, DbType.DateTime, ParameterDirection.Input); 
            parametros.Add("@STPEDIDO", pedido.STPedido, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@VLPEDIDO", pedido.VLPedido, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLACRES", pedido.VLAcres, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLDESC", pedido.VLDesc, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTAL", pedido.VLTotal, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLOUTROS", pedido.VLOutros, DbType.Double, ParameterDirection.Input);
            parametros.Add("@DSOBS", pedido.DSObs, DbType.String, ParameterDirection.Input);
            parametros.Add("@NUDISTANCIA", pedido.NUDistancia, DbType.Double, ParameterDirection.Input);
            parametros.Add("@DTHRCONCLUSAO", pedido.DTHRConclusao, DbType.DateTime, ParameterDirection.Input);

            if(_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0)
            {
                foreach (var item in pedido.itens)
                {
                    if(!AdicionarItemPedido(id, item).Result)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private async Task<bool> AdicionarItemPedido(long idPedido, PedidoItem item)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var sqitem = await ObterSequencialItemPedido(id);
            var parametros = new DynamicParameters();

            var query = $@"INSERT INTO pedido_item (IDITEMPEDIDO,IDPEDIDO,IDPRODUTO,IDESTOQUE,IDFORN,SQITEMPEDIDO,VLUNIT,NUQTD,
                            VLITEM,VLACRES,VLDESC,VLOUTROS,VLTOTAL,VLCUSTOMEDIO,STITEMPEDIDO,DTPREV_ENTREGA,DSOBSITEM)
                            VALUES (@IDITEMPEDIDO,@IDPEDIDO,@IDPRODUTO,@IDESTOQUE,@IDFORN,@SQITEMPEDIDO,@VLUNIT,@NUQTD,
                            @VLITEM,@VLACRES,@VLDESC,@VLOUTROS,@VLTOTAL,@VLCUSTOMEDIO,@STITEMPEDIDO,@DTPREV_ENTREGA,@DSOBSITEM)";
            parametros.Add("@IDITEMPEDIDO", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPEDIDO", idPedido, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPRODUTO", item.IDProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDESTOQUE", null, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDFORN", null, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@VLUNIT", item.VLUnit.HasValue ? item.VLUnit : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@NUQTD", item.NUQtd, DbType.Double, ParameterDirection.Input); ;
            parametros.Add("@VLITEM", item.VLItem.HasValue ? item.VLItem : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLACRES", item.VLAcres.HasValue ? item.VLAcres : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLDESC", item.VLDesc.HasValue ? item.VLDesc : 0, DbType.Double, ParameterDirection.Input); 
            parametros.Add("@VLOUTROS", item.VLOutros.HasValue ? item.VLOutros : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTAL", item.VLTotal.HasValue ? item.VLTotal : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLCUSTOMEDIO", item.VLCustoMedio.HasValue ? item.VLCustoMedio : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@STITEMPEDIDO", EAtivo.Ativo, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@VLTOTAL", item.VLTotal.HasValue ? item.VLTotal : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@DTPREV_ENTREGA", item.DTPrevEntrega.HasValue ? item.DTPrevEntrega : DateTime.Now, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@DSOBSITEM", item.DSObsItem, DbType.String, ParameterDirection.Input);
            parametros.Add("@SQITEMPEDIDO", sqitem, DbType.Int32, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;

        }

        private async Task<int> ObterSequencialItemPedido(long idPedido)
        {
            var parametros = new DynamicParameters();

            var query = $@"select (COALESCE(MAX(SQITEMPEDIDO),0)) AS SQITEMPEDIDO FROM pedido_item where IDPEDIDO= @idpedido";
            parametros.Add("@idpedido", idPedido, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Query<int>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }


        public async Task<bool> AdicionarPedido(PedidoSalvarCustomViewModel pedido)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var codigo = _utilDapperRepository.GerarCodigo($"SELECT MAX(CAST(CDPEDIDO AS UNSIGNED)) AS CD FROM pedido").Result;
            var parametros = new DynamicParameters();
            pedido.Id = id.ToString();

            var query = $@"INSERT INTO pedido (IDPEDIDO,IDEMPRESA,IDFUNC,IDCLIENTE,IDENDERECO,IDCAIXA,IDPDV,CDPEDIDO,DTPEDIDO,STPEDIDO,VLPEDIDO,
                                   VLACRES,VLDESC,VLOUTROS,VLTOTAL,DSOBS,NUDISTANCIA)
                           VALUES (@IDPEDIDO,@IDEMPRESA,@IDFUNC,@IDCLIENTE,@IDENDERECO,@IDCAIXA,@IDPDV,@CDPEDIDO,@DTPEDIDO,@STPEDIDO,@VLPEDIDO,
                                    @VLACRES,@VLDESC,@VLOUTROS,@VLTOTAL,@DSOBS,@NUDISTANCIA)";
            parametros.Add("@IDPEDIDO", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDEMPRESA", pedido.IDEmpresa, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDFUNC", pedido.IDFuncionario, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDCLIENTE", pedido.IDCliente, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDENDERECO", pedido.IDEndereco, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDCAIXA", pedido.IDCaixa, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPDV", pedido.IDPDV, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@CDPEDIDO", codigo, DbType.String, ParameterDirection.Input);
            parametros.Add("@DTPEDIDO", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@STPEDIDO", pedido.STPedido, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@VLPEDIDO", pedido.VLPedido, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLACRES", pedido.VLAcres, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLDESC", pedido.VLDesc, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTAL", pedido.VLTotal, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLOUTROS", pedido.VLOutros, DbType.Double, ParameterDirection.Input);
            parametros.Add("@DSOBS", pedido.DSObs, DbType.String, ParameterDirection.Input);
            parametros.Add("@NUDISTANCIA", pedido.NUDistancia, DbType.Double, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;            
        }

        public async Task<bool> AtulizarPedido(PedidoSalvarCustomViewModel pedido)
        {
            var parametros = new DynamicParameters();
            var query = @$"UPDATE pedido SET IDCLIENTE = @IDCLIENTE,IDENDERECO = @IDENDERECO,VLPEDIDO=@VLPEDIDO,VLACRES =@VLACRES,
                        VLDESC=@VLDESC,VLOUTROS=@VLOUTROS,VLTOTAL=@VLTOTAL where IDPEDIDO = @IDPEDIDO";
            parametros.Add("@IDPEDIDO", pedido.Id, DbType.Int64, ParameterDirection.Input) ;
            parametros.Add("@IDENDERECO", pedido.IDEndereco, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@VLPEDIDO", pedido.VLPedido, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLACRES", pedido.VLAcres, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLDESC", pedido.VLDesc, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTAL", pedido.VLTotal, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLOUTROS", pedido.VLOutros, DbType.Double, ParameterDirection.Input);
            parametros.Add("@IDCLIENTE", pedido.IDCliente, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        public async Task<bool> AdicionarItemPedido(long idPedido, PedidoItemListaViewModel item)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var sqitem = await ObterSequencialItemPedido(id) + 1;
            var parametros = new DynamicParameters();

            var query = $@"INSERT INTO pedido_item (IDITEMPEDIDO,IDPEDIDO,IDPRODUTO,IDESTOQUE,IDFORN,SQITEMPEDIDO,VLUNIT,NUQTD,
                            VLITEM,VLACRES,VLDESC,VLOUTROS,VLTOTAL,VLCUSTOMEDIO,STITEMPEDIDO,DTPREV_ENTREGA,DSOBSITEM)
                            VALUES (@IDITEMPEDIDO,@IDPEDIDO,@IDPRODUTO,@IDESTOQUE,@IDFORN,@SQITEMPEDIDO,@VLUNIT,@NUQTD,
                            @VLITEM,@VLACRES,@VLDESC,@VLOUTROS,@VLTOTAL,@VLCUSTOMEDIO,@STITEMPEDIDO,@DTPREV_ENTREGA,@DSOBSITEM)";
            parametros.Add("@IDITEMPEDIDO", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPEDIDO", idPedido, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPRODUTO", item.IDProduto, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDESTOQUE", null, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDFORN", null, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@VLUNIT", item.VLUnit.HasValue ? item.VLUnit : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@NUQTD", item.NUQtd, DbType.Double, ParameterDirection.Input); ;
            parametros.Add("@VLITEM", item.VLItem.HasValue ? item.VLItem : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLACRES", item.VLAcres.HasValue ? item.VLAcres : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLDESC", item.VLDesc.HasValue ? item.VLDesc : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLOUTROS", item.VLOutros.HasValue ? item.VLOutros : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTAL", item.VLTotal.HasValue ? item.VLTotal : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLCUSTOMEDIO", item.VLCustoMedio.HasValue ? item.VLCustoMedio : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@STITEMPEDIDO", EAtivo.Ativo, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@VLTOTAL", item.VLTotal.HasValue ? item.VLTotal : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@DTPREV_ENTREGA", item.DTPrevEntrega.HasValue ? item.DTPrevEntrega : DateTime.Now, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@DSOBSITEM", item.DSObsItem, DbType.String, ParameterDirection.Input);
            parametros.Add("@SQITEMPEDIDO", sqitem, DbType.Int32, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        public async Task<bool> AtualizarItemPedido(PedidoItemListaViewModel item)
        {
            var parametros = new DynamicParameters();

            var query = $@"UPDATE pedido_item SET VLUNIT = @VLUNIT,NUQTD = @NUQTD,VLITEM = @VLITEM,VLACRES = @VLACRES,VLDESC = @VLDESC,
                                VLOUTROS = @VLOUTROS,VLTOTAL = @VLTOTAL,VLCUSTOMEDIO = @VLCUSTOMEDIO 
                           WHERE IDITEMPEDIDO = @IDITEMPEDIDO";
            parametros.Add("@IDITEMPEDIDO", item.Id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@VLUNIT", item.VLUnit.HasValue ? item.VLUnit : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@NUQTD", item.NUQtd, DbType.Double, ParameterDirection.Input); ;
            parametros.Add("@VLITEM", item.VLItem.HasValue ? item.VLItem : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLACRES", item.VLAcres.HasValue ? item.VLAcres : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLDESC", item.VLDesc.HasValue ? item.VLDesc : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLOUTROS", item.VLOutros.HasValue ? item.VLOutros : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTAL", item.VLTotal.HasValue ? item.VLTotal : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLCUSTOMEDIO", item.VLCustoMedio.HasValue ? item.VLCustoMedio : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTOTAL", item.VLTotal.HasValue ? item.VLTotal : 0, DbType.Double, ParameterDirection.Input);
            
            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        public async Task<bool> ObterItemPedidoPorid(PedidoItemListaViewModel item)
        {
            var parametros = new DynamicParameters();

            var query = $@"select IDITEMPEDIDO as ID,pedido_item.* from pedido_item where IDITEMPEDIDO = @IDITEMPEDIDO";
            parametros.Add("@IDITEMPEDIDO", item.Id, DbType.Int64, ParameterDirection.Input);
            return _dbSession.Connection.Query<PedidoItem>(query, parametros, _dbSession.Transaction).Any();
        }

        public async Task<bool> AdicionarFormaPagamento(long idPedido, PedidoFormaPagamentoListaViewModel item)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var parametros = new DynamicParameters();
            var query = $@"INSERT INTO pedido_pagamento (IDPEDIDOPAG,IDPEDIDO,IDFORMAPAG,IDMOEDA,VLPAG,VLTROCO,DSOBSPAG)
                            VALUES (@IDPEDIDOPAG,@IDPEDIDO,@IDFORMAPAG,@IDMOEDA,@VLPAG,@VLTROCO,@DSOBSPAG)";

            parametros.Add("@IDPEDIDOPAG", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPEDIDO", idPedido, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDFORMAPAG", item.IDFormaPagamento, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDMOEDA", item.IDMoeda, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@VLPAG", item.VLPagamento.HasValue? item.VLPagamento :0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTROCO", item.VLTroco.HasValue?item.VLTroco:0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@DSOBSPAG", item.DSObsPagamento, DbType.String, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        public async Task<bool> AtualizarFormaPagamento(PedidoFormaPagamentoListaViewModel item)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var parametros = new DynamicParameters();
            var query = $@"UPDATE pedido_pagamento SET VLPAG = @VLPAG, VLTROCO = @VLTROCO WHERE IDPEDIDOPAG = @IDPEDIDOPAG";

            parametros.Add("@IDPEDIDOPAG", item.Id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@VLPAG", item.VLPagamento.HasValue ? item.VLPagamento : 0, DbType.Double, ParameterDirection.Input);
            parametros.Add("@VLTROCO", item.VLTroco.HasValue ? item.VLTroco : 0, DbType.Double, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }


        public async Task<bool> ObterFormaPAgamentoPorId(PedidoFormaPagamentoListaViewModel item)
        {
            var parametros = new DynamicParameters();

            var query = $@"select IDFORMAPAG,IDMOEDA,IDPEDIDO,VLPAG,VLTROCO,DSOBSPAG from pedido_pagamento where IDPEDIDO = @IDPEDIDO and IDMOEDA =@IDMOEDA";
            parametros.Add("@IDPEDIDO", item.IDPedido, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDMOEDA", item.IDMoeda, DbType.Int64, ParameterDirection.Input);

            var formaPagmento = _dbSession.Connection.Query<FormaPagamento>(query, parametros, _dbSession.Transaction).FirstOrDefault();

            return formaPagmento != null;
        }


        public async Task<bool> DefinirEntregador(long idPedido, long idFuncionario)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var parametros = new DynamicParameters();
            var query = $@"UPDATE pedido SET IDFUNC = @IDFUNC WHERE IDPEDIDO = @IDPEDIDO";

            parametros.Add("@IDFUNC", idFuncionario, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPEDIDO", idPedido, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        public async Task<bool> CancelarPedido(long idPedido)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var parametros = new DynamicParameters();
            var query = $@"UPDATE pedido SET STPEDIDO = @STPEDIDO, DTHRCONCLUSAO = Now() 
                            WHERE IDPEDIDO = @IDPEDIDO";

            parametros.Add("@STPEDIDO", ESituacaoPedido.Cancelado, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@IDPEDIDO", idPedido, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        public async Task<PedidoFuncionarioCustomViewModel> ObterPedidosPorFuncionario(long idFuncionario)
        {
            var parametros = new DynamicParameters();

            var query = $@"SELECT COUNT(*) AS TotalPedido, COALESCE(SUM(NUDISTANCIA), 0) AS DistanciaPercorrida FROM pedido P 
                            INNER JOIN caixa CX ON P.IDCAIXA = CX.IDCAIXA 
                            WHERE P.IDFUNC = @IDFUNC
                            AND CX.STCAIXA = @STCAIXA";
            parametros.Add("@IDFUNC", idFuncionario, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@STCAIXA", ESituacaoCaixa.Aberto, DbType.Int32, ParameterDirection.Input);

            return _dbSession.Connection.Query<PedidoFuncionarioCustomViewModel>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<Pedido> ObterPedidoPorId(long id)
        {
            var parametros = new DynamicParameters();

            var query = $@"SELECT P.*,P.IDPEDIDO as Id, P.IDFUNC as IDFuncionario FROM pedido P where P.IDPEDIDO = @IDPEDIDO";
            parametros.Add("@IDPEDIDO", id, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Query<Pedido>(query, parametros, _dbSession.Transaction).FirstOrDefault();

        }

        public async Task<int> PedidoComQuantidadeZerada(long id)
        {
            var parametros = new DynamicParameters();

            var query = $@"SELECT COUNT(*) AS TOTAL FROM pedido_item WHERE IDPEDIDO = @IDPEDIDO AND NUQTD = 0";
            parametros.Add("@IDPEDIDO", id, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Query<int>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<int> PedidoComItensSemValor(long id)
        {
            var parametros = new DynamicParameters();

            var query = $@"SELECT COUNT(*) AS TOTAL FROM pedido_item WHERE IDPEDIDO = @IDPEDIDO AND VLTOTAL = 0";
            parametros.Add("@IDPEDIDO", id, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Query<int>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<int> GerarCodigoVenda(long idCaixa)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@IDCAIXA", idCaixa, DbType.Int64, ParameterDirection.Input);
            
            var query = $@"SELECT (COALESCE(MAX(SQVENDA), 0) + 1) AS SQVENDA FROM venda WHERE IDCAIXA =@IDCAIXA";

            return _dbSession.Connection.Query<int>(query, parametros,_dbSession.Transaction).FirstOrDefault();
        }

        public async Task<string> ObterCpfCnpjPorCliente(long id)
        {
            var cpf = "";
            var parametros = new DynamicParameters();
            parametros.Add("@IDCLIENTE", id, DbType.Int64, ParameterDirection.Input);

            var query = $@"SELECT NUCPF FROM clientepf WHERE IDCLIENTE = @IDCLIENTE";

            cpf = _dbSession.Connection.Query<string>(query, parametros,_dbSession.Transaction).FirstOrDefault();
            if (string.IsNullOrEmpty(cpf))
            {
                query = $@"SELECT NUCNPJ FROM clientepj WHERE IDCLIENTE = @IDCLIENTE";
                cpf = _dbSession.Connection.Query<string>(query,parametros, _dbSession.Transaction).FirstOrDefault();
            }
            return cpf;
        }

        public async Task<Produto> ObterProdutoPorIdPedido(long id)
        {
            var parametros = new DynamicParameters();

            var query = $@"SELECT P.IDPRODUTO  as Id, P.CDPRODUTO, P.NMPRODUTO, P.CTPRODUTO, 
                            P.UNVENDA, (COALESCE(MAX(P.PCIBPTFED),0)) as PCIBPTFED, (COALESCE(MAX(P.PCIBPTEST),0)) as PCIBPTEST, 
                            (COALESCE(MAX(P.PCIBPTMUN),0)) as PCIBPTMUN, (COALESCE(MAX(P.PCIBPTIMP),0)) as PCIBPTIMP
                            FROM pedido_item PI 
                            INNER JOIN produto P ON PI.IDPRODUTO = P.IDPRODUTO WHERE IDPEDIDO = @IDPEDIDO
                            group by  P.IDPRODUTO, P.CDPRODUTO, P.NMPRODUTO, P.CTPRODUTO";
            parametros.Add("@IDPEDIDO", id, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Query<Produto>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<Produto> ObterProdutoPorId(long id)
        {
            var query = $@"SELECT p.IDPRODUTO as Id, p.* FROM produto p where IDPRODUTO = {id}";
            return _dbSession.Connection.Query<Produto>(query, null, _dbSession.Transaction).FirstOrDefault();
        }

        public async Task<bool> AdicionarPedidoVenda(long idPedido, long idVenda)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var parametros = new DynamicParameters();
            var query = $@"INSERT INTO pedido_venda(IDPEDIDOVENDA, IDPEDIDO, IDVENDA) value (@IDPEDIDOVENDA, @IDPEDIDO, @IDVENDA)";

            parametros.Add("@IDPEDIDOVENDA", id, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPEDIDO", idPedido, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDVENDA", idVenda, DbType.Int64, ParameterDirection.Input);
            
            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        public async Task<bool> MudarSituacaoPedido(long idPedido, ESituacaoPedido eSituacaoPedido)
        {
            var id = _utilDapperRepository.GerarUUID().Result;
            var parametros = new DynamicParameters();
            var query = $@"UPDATE pedido SET STPEDIDO = @STPEDIDO, DTHRCONCLUSAO = Now() WHERE IDPEDIDO =  @IDPEDIDO";

            parametros.Add("@STPEDIDO", eSituacaoPedido, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@IDPEDIDO", idPedido, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        public async Task<Usuario> ObterUsuarioPorId(long id)
        {
            var parametros = new DynamicParameters();

            var query = $@"SELECT u.id_usuario as id,u.* FROM ca_usuarios u where u.id_usuario = @id_usuario";
            parametros.Add("@id_usuario", id, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Query<Usuario>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }
        public async Task<Pedido> ObterPorId(long id)
        {
            var parametros = new DynamicParameters();

            var query = $@"SELECT p.IDPEDIDO as Id,p.*, P.IDFUNC as IDFuncionario FROM pedido p where p.IDPEDIDO = @IDPEDIDO";
            parametros.Add("@IDPEDIDO", id, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Query<Pedido>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }
    }
}

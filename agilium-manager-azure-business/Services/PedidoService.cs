using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.PedidoViewModel;
using agilium.api.business.Models.CustomReturn.ProdutoReturnViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.business.Services
{
    public class PedidoService : BaseService, IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IPedidoDapperRepository _pedidoDapperRepository;
        private readonly IDapperRepository _dapperRepository;
        private readonly ICaixaDapperRepository _caixaDapperRepository;
        private readonly IConfigDapperRepository _configDapperRepository;
        private readonly IVendaDapperRepository _vendaDapperRepository;
        public PedidoService(INotificador notificador, IPedidoRepository pedidoRepository,
            IPedidoDapperRepository pedidoDapperRepository, IDapperRepository dapperRepository, 
            ICaixaDapperRepository caixaDapperRepository,IConfigDapperRepository configDapperRepository,
            IVendaDapperRepository vendaDapperRepository) : base(notificador)
        {
            _pedidoRepository = pedidoRepository;
            _pedidoDapperRepository = pedidoDapperRepository;
            _dapperRepository = dapperRepository;
            _caixaDapperRepository = caixaDapperRepository;
            _configDapperRepository = configDapperRepository;
            _vendaDapperRepository = vendaDapperRepository;
        }

        public void Dispose()
        {
            _pedidoRepository?.Dispose();
        }
     
        public async Task Salvar()
        {
            await _pedidoRepository.SaveChanges();
        }

     

        #region Dapper
        public async Task<IEnumerable<PedidoListaViewModel>> ObterListaPedido(DateTime dataInicial, DateTime dataFinal,
                string numeroPedido = null, string nomeCliente = null, string nomeEntregador = null, string bairroEntrega = null)
        {
            var resultado = await _pedidoDapperRepository.ObterListaPedido(dataInicial, dataFinal, numeroPedido, nomeCliente, nomeEntregador, bairroEntrega);
            return resultado;
        }

        public async Task<IEnumerable<PedidoItemListaViewModel>> ObterListaItemPedido(long idpedido)
        {
            return await _pedidoDapperRepository.ObterListaItemPedido(idpedido);
        }

        public async Task<IEnumerable<PedidoFormaPagamentoListaViewModel>> ObterListaFormaPagamentoPedido(long idpedido)
        {
            return await _pedidoDapperRepository.ObterListaFormaPagamentoPedido(idpedido);
        }

        public async Task<PedidosEstatisticasListaViewModel> ObterEstatistica()
        {
            return await _pedidoDapperRepository.ObterEstatistica();
        }

        public async Task<IEnumerable<Cliente>> ObterTodosClientes(string nome)
        {
            var lista = await _pedidoDapperRepository.ObterTodosClientes(nome);
            return lista;
        }

        public async Task<IEnumerable<Endereco>> ObterEnderecosPorCliente(long idCliente)
        {
            return await _pedidoDapperRepository.ObterEnderecosPorCliente(idCliente);
        }

        public async Task<IEnumerable<Produto>> ObterTodosProdutos(string descricao)
        {
            return await _pedidoDapperRepository.ObterTodosProdutos(descricao);
        }

        public async Task<IEnumerable<Moeda>> ObterMoedas(long idempresa)
        {
            return await _pedidoDapperRepository.ObterMoedas(idempresa);
        }

        public async Task<bool> AdcionarPedido(Pedido pedido)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                if (!await _pedidoDapperRepository.AdicionarPedido(pedido))
                {
                    Notificar("Erro ao adicionar o pedido");
                    return resultado;
                }
                
                if (!TemNotificacao())
                {
                    resultado = true;
                    await _dapperRepository.Commit();
                  
                }                  
                else
                {
                    resultado = false;
                    Notificar("Erro ao salvar novo pedido");                   
                    await _dapperRepository.Rollback();
                }              
            }
            catch (Exception ex)
            {
                await _dapperRepository.Rollback();
                Notificar(ex.Message);
                
            }

            return resultado;            
        }

        public async Task<bool> AdicionarCliente(ClientePedidoCustomViewModel model)
        {
            var cliente = new Cliente("0",model.Nome,Enums.ETipoPessoa.F,model.Endereco.Id,null,null,null,Enums.EAtivo.Ativo);
            if(await _pedidoDapperRepository.AdicionarCliente(cliente))
            {
                if(model.Id == "0" || string.IsNullOrEmpty(model.Id))
                {
                    if(!await _pedidoDapperRepository.AdicionarEndereco(model.Endereco))
                    {
                        Notificar("Falha ao atualizar o endereço de entrega para esse pedido");
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public async Task<bool> AdicionarPedido(PedidoSalvarCustomViewModel model)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                if (!await _pedidoDapperRepository.AdicionarPedido(model))
                {
                    Notificar("Erro ao adicionar o pedido");
                    return resultado;
                }

                foreach (var item in model.Itens)
                {
                    if(!await _pedidoDapperRepository.ObterItemPedidoPorid(item))
                    {

                        if (!await _pedidoDapperRepository.AdicionarItemPedido(Convert.ToInt64(model.Id), item))
                        {
                            Notificar("Erro ao adicionar o pedido");
                            return resultado;
                        }
                    }
                    else
                    {
                        if (!await _pedidoDapperRepository.AdicionarItemPedido(Convert.ToInt64(model.Id), item))
                        {
                            Notificar($"Erro ao tentar incluir item {item.DsProduto}");
                            break;
                        }
                    }
                  
                }

                foreach (var item in model.FormasPagamento)
                {
                    if (!await _pedidoDapperRepository.AdicionarFormaPagamento(Convert.ToInt64(model.Id), item))
                    {
                        Notificar($"Erro ao tentar incluir forma de pagamento {item.DsMoeda}");
                        break;
                    }
                }

                if (!TemNotificacao())
                {
                    resultado = true;
                    await _dapperRepository.Commit();

                }
                else
                {
                    resultado = false;
                    Notificar("Erro ao salvar novo pedido");
                    await _dapperRepository.Rollback();
                }
            }
            catch (Exception ex)
            {
                await _dapperRepository.Rollback();
                var contaErro = 0;
               Notificar("Erro não identificado");
            }

            return resultado;
        }

        public async Task<bool> CancelarPedido(long idPedido)
        {
            var resultado = false;
            var pedido = await _pedidoDapperRepository.ObterPedidoPorId(idPedido);

            if (pedido == null)
            {
                Notificar("pedido não localizado");
                return resultado;
            }

            if ((pedido.STPedido == ESituacaoPedido.Cancelado) || (pedido.STPedido == ESituacaoPedido.Aprovado) || (pedido.STPedido == ESituacaoPedido.Concluido))
            {
                Notificar("Somente pedidos não concluídos podem ser cancelado");
                return resultado;
            }

            try
            {
                await _dapperRepository.BeginTransaction();

                if (!await _pedidoDapperRepository.CancelarPedido(idPedido))
                {
                    Notificar("Erro ao cancelar o pedido");
                    return resultado;
                }

                if (!TemNotificacao())
                {
                    resultado = true;
                    await _dapperRepository.Commit();

                }
                else
                {
                    resultado = false;
                    Notificar("Erro ao cancelar  pedido");
                    await _dapperRepository.Rollback();
                }
            }
            catch (Exception ex)
            {
                await _dapperRepository.Rollback();
                var contaErro = 0;
                Notificar("Erro não identificado");
            }

            return resultado;
        }

        public async Task<bool> AtualizarPedido(PedidoSalvarCustomViewModel model)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                if (!await _pedidoDapperRepository.AtulizarPedido(model))
                {
                    Notificar("Erro ao atualizar o pedido");
                    return resultado;
                }

                foreach (var item in model.Itens)
                {
                    if (!await _pedidoDapperRepository.ObterItemPedidoPorid(item))
                    {

                        if (!await _pedidoDapperRepository.AdicionarItemPedido(Convert.ToInt64(model.Id), item))
                        {
                            Notificar("Erro ao adicionar o pedido");
                            return resultado;
                        }
                    }
                    else
                    {
                        if (!await _pedidoDapperRepository.AtualizarItemPedido(item))
                        {
                            Notificar($"Erro ao tentar atualizar item {item.DsProduto}");
                            break;
                        }
                    }
                  
                }

                foreach (var item in model.FormasPagamento)
                {
                    if(await _pedidoDapperRepository.ObterFormaPAgamentoPorId(item))
                    {
                        if (!await _pedidoDapperRepository.AtualizarFormaPagamento(item))
                        {
                            Notificar($"Erro ao tentar atualizar forma de pagamento {item.DsMoeda}");
                            break;
                        }
                    }
                    else
                    {
                        if (!await _pedidoDapperRepository.AdicionarFormaPagamento(Convert.ToInt64(model.Id), item))
                        {
                            Notificar($"Erro ao tentar incluir forma de pagamento {item.DsMoeda}");
                            break;
                        }
                    }
                  
                }

                if (!TemNotificacao())
                {
                    resultado = true;
                    await _dapperRepository.Commit();

                }
                else
                {
                    resultado = false;
                    Notificar("Erro ao atualizar pedido");
                    await _dapperRepository.Rollback();
                }
            }
            catch (Exception ex)
            {
                await _dapperRepository.Rollback();
                Notificar("Erro não identificado");
            }

            return resultado;
        }

        public async Task<IEnumerable<Funcionario>> ObterEntregadoresPorEmpresa(long idempresa)
        {
            return await _pedidoDapperRepository.ObterEntregadoresPorEmpresa(idempresa);
        }

        public async Task<bool> DefinirEntregador(long idPedido, long idFuncionario)
        {
            var resultado = false;
            var pedido = await _pedidoDapperRepository.ObterPedidoPorId(idPedido);

            if (pedido == null)
            {
                Notificar("pedido não localizado");
                return resultado;
            }

            if ((pedido.STPedido == ESituacaoPedido.Cancelado) || (pedido.STPedido == ESituacaoPedido.Aprovado) || (pedido.STPedido == ESituacaoPedido.Concluido))
            
            {
                Notificar("Somente pedidos não concluídos podem ter o entregador modificado");
                return resultado;
            }

            try
            {
                await _dapperRepository.BeginTransaction();

                if (!await _pedidoDapperRepository.DefinirEntregador(idPedido, idFuncionario))
                {
                    Notificar("Erro ao definir entregado para o pedido");
                    return resultado;
                }

                if (!TemNotificacao())
                {
                    resultado = true;
                    await _dapperRepository.Commit();
                }
                else
                {
                    resultado = false;
                    await _dapperRepository.Rollback();
                }
            }
            catch (Exception ex)
            {
                await _dapperRepository.Rollback();
                Notificar("Erro não identificado");
            }

            return resultado;
        }

        public async Task<PedidoFuncionarioCustomViewModel> ObterPedidosPorFuncionario(long idFuncionario)
        {
            return await _pedidoDapperRepository.ObterPedidosPorFuncionario(idFuncionario);
        }

        public async Task<bool> Concluir(long idPedido, long idUsusario)
        {
            var resultado = false;
            
            var pedido = await _pedidoDapperRepository.ObterPedidoPorId(idPedido);

            if (pedido == null)
            {
                Notificar("pedido não localizado");
                return resultado;
            }

            if (pedido.IDFuncionario == 0)
            {
                Notificar("Para concluir o pedido é obrigatorio a seleção de um entregador");
                return resultado;
            }

            if ((pedido.STPedido == ESituacaoPedido.Cancelado) || (pedido.STPedido == ESituacaoPedido.Aprovado) || (pedido.STPedido == ESituacaoPedido.Concluido))
            {
                Notificar("O pedido selecionado está cancelado ou concluído.");
                return resultado;
            }

            if (_pedidoDapperRepository.PedidoComQuantidadeZerada(idPedido).Result > 0)
            {
                Notificar("O pedido selecionado possui itens com a quantidade ZERADA. Corrija esses itens antes de concluir o pedido.");
                return resultado;
            }

            var usuario = await _pedidoDapperRepository.ObterUsuarioPorId(idUsusario);
            if (usuario == null)
            {
                Notificar("Usuario do sistema não localizadio.");
                return resultado;
            }

            var idFuncionario = await _caixaDapperRepository.ObterIdFuncionarioPorUsuarioEmpresa(pedido.IDEmpresa, idUsusario);
            if (idFuncionario == 0)
            {
                Notificar($"Não foi localizado cadastro de funcionário.");
            }

            var caixaAberto = await _caixaDapperRepository.ObterCaixaAberto(pedido.IDEmpresa, idFuncionario);
            if(caixaAberto == null)
            {
                Notificar("Não existe caixa aberto para o usuario.");
                return resultado;
            }


            try
            {
                await _dapperRepository.BeginTransaction();

                var itens = await _pedidoDapperRepository.ObterListaItemPedido(idPedido);
                       
                var seqVenda = await _pedidoDapperRepository.GerarCodigoVenda(caixaAberto.Id);
                var cpf = await _pedidoDapperRepository.ObterCpfCnpjPorCliente(pedido.IDCliente);
                var config = await _configDapperRepository.ObterConfig("VENDAS_DOC_FISCAL_PADRAO", pedido.IDEmpresa);
                var tpdoc = ETipoDocVenda.NFCE;

                var dsinfcompl = $@"Venda referente ao pedido No. {pedido.CDPedido}.;{pedido.DSObs}";
                if (config != null && !string.IsNullOrEmpty(config.VALOR))
                {
                    tpdoc = (ETipoDocVenda)Enum.Parse(typeof(ETipoDocVenda), config.VALOR);
                }
                //criar venda
                var venda = new Venda(caixaAberto.Id,pedido.IDCliente, seqVenda,DateTime.Now,cpf,pedido.VLPedido,pedido.VLDesc,pedido.VLTotal,
                    pedido.VLAcres,ESituacaoVenda.Ativo,dsinfcompl,0,0,0,0,null,null,tpdoc,ETipoEmissaoVenda.NaoEmitido,null,EOrigemVenda.PEDIDO);

                double? PCIBPTFED = 0d;
                double? PCIBPTEST = 0d;
                double? PCIBPTIMP = 0d;
                double? PCIBPTMUN = 0d;

                var i = 0;
                foreach (var item in itens)
                {
                    var _idProduto = Convert.ToInt64(item.IDProduto);
                    //var produto = await _pedidoDapperRepository.ObterProdutoPorIdPedido(_idProduto);
                    var produto = await _pedidoDapperRepository.ObterProdutoPorId(_idProduto);
                    if(produto != null)
                    {
                        i++;

                        PCIBPTFED = produto.PCIBPTFED + (item.VLTotal * (produto.PCIBPTFED / 100));
                        PCIBPTEST = produto.PCIBPTEST + (item.VLTotal * (produto.PCIBPTEST / 100));
                        PCIBPTIMP = produto.PCIBPTIMP + (item.VLTotal * (produto.PCIBPTIMP / 100));
                        PCIBPTMUN = produto.PCIBPTMUN + (item.VLTotal * (produto.PCIBPTMUN / 100));

                        var itemVenda = new VendaItem(0, _idProduto, i, item.VLUnit, item.NUQtd, item.VLItem, item.VLAcres, item.VLDesc, item.VLTotal,
                            item.VLCustoMedio, ESituacaoItemVenda.Ativo, PCIBPTFED, PCIBPTEST, PCIBPTMUN, PCIBPTIMP);

                        venda.VendaItem.Add(itemVenda);
                    }
                
                }

                venda.AdicionarIbpt(PCIBPTFED, PCIBPTEST, PCIBPTMUN, PCIBPTIMP);

                venda.AdicionarInformacaoComplementar("Val aprox Tributos");
                venda.AdicionarInformacaoComplementar($"Federal R$ {(venda.VLTOTIBPTFED + venda.VLTOTIBPTIMP)?.ToString("C")} ({(((venda.VLTOTIBPTFED + venda.VLTOTIBPTIMP) * 100) / venda.VLVENDA)?.ToString("C")} %)");
                venda.AdicionarInformacaoComplementar($"Estadual R$ {venda.VLTOTIBPTEST?.ToString("C")} ({((venda.VLTOTIBPTEST * 100) / venda.VLVENDA)?.ToString("C")} %)");
                venda.AdicionarInformacaoComplementar($"Municipal R$ {venda.VLTOTIBPTMUN?.ToString("C")} ({((venda.VLTOTIBPTMUN * 100) / venda.VLVENDA)?.ToString("C")} %)");
                venda.AdicionarInformacaoComplementar($"Fonte: IBPT {dsinfcompl}");

                var pedidosPagamento = await _pedidoDapperRepository.ObterListaFormaPagamentoPedido(idPedido);

                foreach (var item in pedidosPagamento)
                {
                    var idMoeda = Convert.ToInt64(item.IDMoeda);
                    var vendaMoeda = new VendaMoeda(0,idMoeda,null,item.VLPagamento,item.VLTroco,null,null);
                    venda.AdicionarMoeda(vendaMoeda);
                }

                var idestoque = await _caixaDapperRepository.ObterEstoquePorIdCaixa(caixaAberto.Id);
                if(idestoque == 0)
                {
                    Notificar("Erro ao tentar localziar o estoque do produto");
                }
                var idVenda = await _vendaDapperRepository.AdicionarVenda(venda,idestoque,caixaAberto.SQCAIXA.Value, usuario.nome, cpf);

                if (!_pedidoDapperRepository.AdicionarPedidoVenda(idPedido, idVenda).Result)
                {
                    Notificar("Erro ao tentar concluir pedido");
                }

                var configPreVenda = await _configDapperRepository.ObterConfig("PDV_PREVENDA", pedido.IDEmpresa);

                var mEISemCupom = false;
                if(configPreVenda != null && !string.IsNullOrEmpty(configPreVenda.VALOR))
                {
                    mEISemCupom = configPreVenda.VALOR == "S";
                }

                var configPreVendaAtivo = await _configDapperRepository.ObterConfig("PREVENDA_ATIVO", pedido.IDEmpresa);

                var preVendaAtivo = false;
                if (configPreVendaAtivo != null && !string.IsNullOrEmpty(configPreVenda.VALOR))
                {
                    preVendaAtivo = configPreVendaAtivo.VALOR == "S";
                }

                if(!preVendaAtivo && !mEISemCupom) 
                { 

                }

                if (!_pedidoDapperRepository.MudarSituacaoPedido(idPedido, ESituacaoPedido.Concluido).Result)
                {
                    Notificar("Erro ao tentar mudar siutação do pedido");
                }

                if (!TemNotificacao())
                {
                    resultado = true;
                    await _dapperRepository.Commit();

                }
                else
                {
                    resultado = false;
                    await _dapperRepository.Rollback();
                }
            }
            catch (Exception ex)
            {
                await _dapperRepository.Rollback();
                Notificar(ex.Message);
            }

            return resultado;
        }

        public async Task<Pedido> ObterPorId(long id)
        {
            return _pedidoDapperRepository.ObterPorId(id).Result;
        }

        public async Task<PedidoSalvarCustomViewModel> ObterPedidoEditar(long id)
        {
            var pedido = _pedidoDapperRepository.ObterPorId(id).Result;
            if(pedido == null)
            {
                Notificar("Pedido não localizado");
                return null;
            }

            var pedidoSalvar = new PedidoSalvarCustomViewModel
            {
                Id = pedido.Id.ToString(),
                IDEmpresa = pedido.IDEmpresa.ToString(),
                IDFuncionario = pedido.IDFuncionario.ToString(),
                IDCliente = pedido.IDCliente.ToString(),
                IDEndereco = pedido.IDEndereco.ToString(),
                IDCaixa = pedido.IDCaixa.ToString(),
                IDPDV = pedido.IDPDV.ToString(),
                CDPedido = pedido.CDPedido,
                DTPedido = pedido.DTPedido,
                STPedido = pedido.STPedido,
                VLPedido = pedido.VLPedido,
                VLAcres = pedido.VLAcres,
                VLDesc = pedido.VLDesc,
                VLOutros = pedido.VLOutros,
                VLTotal = pedido.VLTotal,
                DSObs = pedido.DSObs,
                NUDistancia = pedido.NUDistancia,
                DTHRConclusao = pedido.DTHRConclusao,
                Codigo = pedido.CDPedido
            };

            var itens = await _pedidoDapperRepository.ObterListaItemPedido(id);
            var itemResult = new List<PedidoItemListaViewModel>();
            itens.ToList().ForEach(item => {
                var conversao = new PedidoItemListaViewModel
                {
                    Id = item.Id.ToString(),
                    IDPedido = item.IDPedido?.ToString(),
                    IDProduto = item.IDProduto?.ToString(),
                    IDEstoque = item.IDEstoque?.ToString(),
                    IDFornecedor = item.IDFornecedor?.ToString(),
                    SQItemPedido = item.SQItemPedido,
                    VLUnit = item.VLUnit,
                    NUQtd = item.NUQtd,
                    VLItem = item.VLItem,
                    VLAcres = item.VLAcres,
                    VLDesc = item.VLDesc,
                    VLOutros = item.VLOutros,
                    VLTotal = item.VLTotal,
                    VLCustoMedio = item.VLCustoMedio,
                    STItemPedido = item.STItemPedido,
                    DTPrevEntrega = item.DTPrevEntrega,
                    DSObsItem = item.DSObsItem,
                    DsProduto = item.DsProduto,
                };

                itemResult.Add(conversao);
            });

            pedidoSalvar.Itens = itemResult;

            var formasPagamento = await _pedidoDapperRepository.ObterListaFormaPagamentoPedido(id);
            var formasPagamentoResult = new List<PedidoFormaPagamentoListaViewModel>();
            formasPagamento.ToList().ForEach(item => {
                formasPagamentoResult.Add(
                      new PedidoFormaPagamentoListaViewModel
                      {
                          Id = item.Id.ToString(),
                          IDPedido = item.IDPedido?.ToString(),
                          IDFormaPagamento = item.IDFormaPagamento?.ToString(),
                          IDMoeda = item.IDMoeda?.ToString(),
                          VLPagamento = item.VLPagamento,
                          VLTroco = item.VLTroco,
                          DSObsPagamento = item.DSObsPagamento,

                          // Additional property: DsMoeda
                          DsMoeda = item.DsMoeda
                      });
            });
            pedidoSalvar.FormasPagamento = formasPagamentoResult;

            return pedidoSalvar;
        }

        public async Task<IEnumerable<ProdutoPesqReturnViewModel>> ObterProdutosPorDescricao(string descricao)
        {
            return await _pedidoDapperRepository.ObterProdutosPorDescricao(descricao);
        }

        public async Task<ProdutoPesqReturnViewModel> ObterProdutosPorDescricaoCodigoCodBarra(string descricao)
        {
            return await _pedidoDapperRepository.ObterProdutosPorDescricaoCodigoCodBarra(descricao);
        }

        #endregion
    }
}

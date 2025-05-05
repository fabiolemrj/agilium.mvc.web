using agilium.api.business.Enums;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.PedidoViewModel;
using agilium.api.business.Models.CustomReturn.ProdutoReturnViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface IPedidoRepository : IRepository<Pedido> {}
    public interface IPedidoItemRepository : IRepository<PedidoItem> {}
    public interface IPedidoSiteMercadoRepository : IRepository<PedidoSitemercado> { }
    public interface IPedidoSiteMercadoItemRepository : IRepository<PedidoItemSitemercado> { }
    public interface IPedidoPagamentoRepository : IRepository<PedidoPagamento> { }
    public interface IPedidoPagamentoSiteMercadoRepository : IRepository<PedidoPagamentoSitemercado> { }
    public interface IPedidoVendaRepository : IRepository<PedidoVenda> { }
    public interface IPedidoVendaItemRepository : IRepository<PedidoVendaItem> { }

    public interface IPedidoDapperRepository 
    {
        Task<IEnumerable<PedidoListaViewModel>> ObterListaPedido(DateTime dataInicial, DateTime dataFinal, string numeroPedido = null,
                    string nomeCliente = null, string nomeEntregador = null, string bairroEntrega = null);
        Task<IEnumerable<PedidoItemListaViewModel>> ObterListaItemPedido(long idpedido);
        Task<IEnumerable<PedidoFormaPagamentoListaViewModel>> ObterListaFormaPagamentoPedido(long idpedido);
        Task<PedidosEstatisticasListaViewModel> ObterEstatistica();
        Task<IEnumerable<Cliente>> ObterTodosClientes(string nome);
        Task<IEnumerable<Endereco>> ObterEnderecosPorCliente(long idCliente);
        Task<IEnumerable<Produto>> ObterTodosProdutos(string descricao);
        Task<IEnumerable<Moeda>> ObterMoedas(long idempresa);
        Task<bool> AdicionarCliente(Cliente cliente);
        Task<bool> AdicionarPedido(Pedido pedido);
        Task<bool> AdicionarPedido(PedidoSalvarCustomViewModel pedido);
        Task<bool> AdicionarEndereco(Endereco endereco);
        Task<bool> AdicionarFormaPagamento(long idPedido, PedidoFormaPagamentoListaViewModel item);
        Task<bool> AdicionarItemPedido(long idPedido, PedidoItemListaViewModel item);
        Task<bool> CancelarPedido(long idPedido);
        Task<IEnumerable<Funcionario>> ObterEntregadoresPorEmpresa(long idempresa);
        Task<bool> DefinirEntregador(long idPedido, long idFuncionario);
        Task<PedidoFuncionarioCustomViewModel> ObterPedidosPorFuncionario(long idFuncionario);
        Task<Pedido> ObterPedidoPorId(long id);
        Task<int> PedidoComQuantidadeZerada(long id);
        Task<int> PedidoComItensSemValor(long id);
        Task<int> GerarCodigoVenda(long idCaixa);
        Task<string> ObterCpfCnpjPorCliente(long id);
        Task<Produto> ObterProdutoPorIdPedido(long id);
        Task<bool> AdicionarPedidoVenda(long idPedido, long idVenda);
        Task<bool> MudarSituacaoPedido(long idPedido, ESituacaoPedido eSituacaoPedido);
        Task<Usuario> ObterUsuarioPorId(long id);
        Task<Pedido> ObterPorId(long id);
        Task<bool> AtulizarPedido(PedidoSalvarCustomViewModel pedido);
        Task<bool> AtualizarItemPedido(PedidoItemListaViewModel item);
        Task<bool> AtualizarFormaPagamento(PedidoFormaPagamentoListaViewModel item);
        Task<IEnumerable<ProdutoPesqReturnViewModel>> ObterProdutosPorDescricao(string descricao);
        Task<ProdutoPesqReturnViewModel> ObterProdutosPorDescricaoCodigoCodBarra(string descricao);
        Task<bool> ObterFormaPAgamentoPorId(PedidoFormaPagamentoListaViewModel item);
        Task<bool> ObterItemPedidoPorid(PedidoItemListaViewModel item);
        Task<Produto> ObterProdutoPorId(long id);

    }
}

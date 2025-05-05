using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.PedidoViewModel;
using agilium.api.business.Models.CustomReturn.ProdutoReturnViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IPedidoService : IDisposable
    {
        Task Salvar();
        Task<Pedido> ObterPorId(long id);

        #region Dapper
        Task<IEnumerable<PedidoListaViewModel>> ObterListaPedido(DateTime dataInicial, DateTime dataFinal,string numeroPedido = null,
            string nomeCliente = null, string nomeEntregador = null, string bairroEntrega = null);

        Task<IEnumerable<PedidoItemListaViewModel>> ObterListaItemPedido(long idpedido);
        Task<IEnumerable<PedidoFormaPagamentoListaViewModel>> ObterListaFormaPagamentoPedido(long idpedido);
        Task<PedidosEstatisticasListaViewModel> ObterEstatistica();
        Task<IEnumerable<Cliente>> ObterTodosClientes(string nome);
        Task<IEnumerable<Endereco>> ObterEnderecosPorCliente(long idCliente);
        Task<IEnumerable<Produto>> ObterTodosProdutos(string descricao);
        Task<IEnumerable<Moeda>> ObterMoedas(long idempresa);
        Task<bool> AdcionarPedido(Pedido pedido);
        Task<bool> AdicionarCliente(ClientePedidoCustomViewModel cliente);
        Task<bool> AdicionarPedido(PedidoSalvarCustomViewModel model);
        Task<bool> CancelarPedido(long idPedido);
        Task<IEnumerable<Funcionario>> ObterEntregadoresPorEmpresa(long idempresa);
        Task<bool> DefinirEntregador(long idPedido, long idFuncionario);
        Task<PedidoFuncionarioCustomViewModel> ObterPedidosPorFuncionario(long idFuncionario);
        Task<bool> Concluir(long idPedido, long idUsusario);
        Task<PedidoSalvarCustomViewModel> ObterPedidoEditar(long id);
        Task<bool> AtualizarPedido(PedidoSalvarCustomViewModel model);
        Task<IEnumerable<ProdutoPesqReturnViewModel>> ObterProdutosPorDescricao(string descricao);
        Task<ProdutoPesqReturnViewModel> ObterProdutosPorDescricaoCodigoCodBarra(string descricao);
        #endregion
    }
}

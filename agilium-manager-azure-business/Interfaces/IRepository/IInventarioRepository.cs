using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface IInventarioRepository : IRepository<Inventario>
    {
    }

    public interface IInventarioItemRepository : IRepository<InventarioItem>
    {
    }

    public interface IInventarioDapperRepository
    {
        Task<List<EstoqueProduto>> ObterProdutosPorEstoque(long idEstoque);
        Task<bool> PodeIncluirProdutoEstoque(long idEstoque, long idProduto);
        Task<bool> IncluirProdutoItemEstoque(long idInventario, long idProduto);
        Task<List<Produto>> ObterProdutosDisponiveisParaInventarioPorIdInventario(long idInventario, long idEmpresa);
        Task<bool> EditarInventarioItem(long idInventario,  double quantidadeApurada, long idUsuarioAnalise);
        Task<bool> ApagarInventarioItem(long id);
        Task<bool> ExisteItemNaoInventariado(long id);
        Task<List<InventarioItem>> ObterInventarioItemPorIdInventario(long id);
        Task<Inventario> ObterInventarioPorIdInventario(long id);
        Task<bool> AtualizarValorCustoMedio(long id, double novoValorCustoMedio);
        Task<bool> AlterarSituacao(long id, ESituacaoInventario eSituacaoInventario);
    }
}

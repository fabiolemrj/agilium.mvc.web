using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IInventarioService : IDisposable
    {
        Task Salvar();

        #region Inventario
        Task Adicionar(Inventario inventario);
        Task Atualizar(Inventario inventario);
        Task Apagar(long id);
        Task<Inventario> ObterPorId(long id);
        Task<PagedResult<Inventario>> ObterPorPaginacao(long idEmpresa, string descricao, int page = 1, int pageSize = 15);
        Task<IEnumerable<Inventario>> ObterTodas(long idEmpresa);
        Task<List<Produto>> ObetrProdutoDisponvelInventario(long idEmpresa, long idInventario);

        #endregion

        #region Item
        Task Adicionar(InventarioItem inventarioItem);
        Task ApagarItem(long id);
        Task Atualizar(InventarioItem inventarioItem);
        Task<InventarioItem> ObterItemPorId(long id);
        Task<List<InventarioItem>> ObterItensPorInventario(long id);
        #endregion

        #region Dapper
        Task<bool> IncluirProdutosPorEstoque(long idEstoque, long idInvent);
        Task<bool> IncluirProdutoInventario(List<InventarioItem> itens);
        Task<bool> AlterarInventarioItem(List<InventarioItem> itens, long idUsuario);
        Task<bool> ApagarInventarioItem(List<InventarioItem> itens);
        Task<bool> ConcluirInventario(long idInventario, ESituacaoInventario situacaoInventario, long idUsuario);
        #endregion
    }
}

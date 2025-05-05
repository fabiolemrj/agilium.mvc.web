using agilium.api.business.Enums;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.ComprasNFEViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface ICompraRepository : IRepository<Compra>
    {
    }

    public interface ICompraItemRepository : IRepository<CompraItem>
    {
    }

    public interface ICompraFiscalRepository : IRepository<CompraFiscal>
    {
    }

    public interface ICompraDapperRepository
    {
        Task<Compra> ExisteCompraPorChaveAcesso(string chaveAcesso);
        Task<Compra> AdicionarCompra(Compra compra);
        Task<Compra> AtualizarCompra(Compra compra);
        Task<Compra> ObterCompraPorId(long id);
        Task<bool> ApagarItensPorIdCompra(long id);
        Task<CompraItem> AdicionarItem(CompraItem item);
        Task<bool> AdicionarFiscal(long id, string xml);
        Task<List<CompraItem>> ObterCompraItemPorIdCompra(long idCompra);
        Task<bool> AtualizarSituacaoCompra(long idCompra, ESituacaoCompra eSituacaoCompra);
        Task<List<CompraItem>> ObterItemCompraEfetivada(long idCompra);
        Task<List<CompraItem>> ObterItemCompraPorIdCompraParaCadastroAutomatico(long idCompra);
        Task<bool> AtualizarCompraItemComIdProduto(long idProduto, long idItem);
        Task<int> ObterQtdItensNaoAssociados(long idCompra);
        Task<bool> AtualizarProdutoNoItemCompra(long idItem, long idCompra, long? idProduto, long? idEstoque, string SGUN, double? Quantidade, double? Relacao, double? ValorUnitario, double? ValorTotal, double? NovoPrecoVenda);
    }

  
}

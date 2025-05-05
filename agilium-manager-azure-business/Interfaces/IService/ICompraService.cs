using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.ComprasNFEViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface ICompraService : IDisposable
    {
        Task Salvar();

        #region Compra
        Task Adicionar(Compra compra);
        Task Atualizar(Compra compra);
        Task Apagar(long id);
        Task<Compra> ObterPorId(long id);
        Task<PagedResult<Compra>> ObterCompraPorPaginacao(long idEmpresa, DateTime dtIni, DateTime dtFim, int page = 1, int pageSize = 15);
        Task<IEnumerable<Compra>> ObterTodas(long idEmpresa);
        #endregion

        #region Item
        Task Adicionar(CompraItem compra);
        Task ApagarItem(long id);
        Task Atualizar(CompraItem compra);
        Task<CompraItem> ObterItemPorId(long id);
        Task<List<CompraItem>> ObterItensPorCompra(long id);
        #endregion

        #region Fiscal
        Task Adicionar(CompraFiscal compra);
        Task ApagarFiscal(long id);
        Task Atualizar(CompraFiscal compra);
        Task<CompraFiscal> ObterFiscalPorId(long id);
        Task<List<CompraFiscal>> ObterFiscaisPorCompra(long id);
        #endregion

        #region Dapper
        Task<bool> ImportarCompraDeXmlNfe(NFeProc nfe, long idCompra);
        Task<bool> ImportarArquivoNFE(long idCompra, string ArquivoXml);
        Task<NFeProc> ImportarArquivoXmlNFE(long idCompra, string ArquivoXml);
        Task<bool> EfetivarCompra(long idCompra, string usuarioNome);
        Task<bool> CancelarCompra(long idCompra, string usuarioNome);
        Task<bool> RealizarCadastroProdutoAutomatico(long idCompra);
        Task<bool> AtualizarProdutoNoItemCompra(long idItem, long idCompra, long? idProduto, long? idEstoque, string SGUN, double? Quantidade, double? Relacao, double? ValorUnitario, double? ValorTotal, double? NovoPrecoVenda);
        #endregion
    }
}

using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface IProdutoDepartamentoRepository : IRepository<ProdutoDepartamento> { }
    public interface IProdutoMarcaRepository : IRepository<ProdutoMarca> { }
    public interface IGrupoProdutoRepository : IRepository<GrupoProduto> { }
    public interface ISubGrupoProdutoRepository : IRepository<SubGrupoProduto> { }
    public interface IProdutoRepository : IRepository<Produto> { }
    public interface IProdutoComposicaoRepository : IRepository<ProdutoComposicao> { }
    public interface IProdutoCodigoBarraRepository : IRepository<ProdutoCodigoBarra> { }
    public interface IProdutoPrecoRepository : IRepository<ProdutoPreco> { }
    public interface IProdutoFotoRepository : IRepository<ProdutoFoto> { }
    public interface IProdutoDapper 
    {
        Task AtualizarIBPTTodosProdutos();
        Task<Produto> ObterProdutoPorCodigoEan(string ean);
        Task<Produto> ObterProdutoPorId(long id);
        Task<Produto> ObterProdutoPorCompraAnterior(long idFornecedor, string codigoFornecedor);
        Task<double> AtualizarCustoMedio(long idProduto, double quantidadeEntrada, double valorCompra);
        Task<bool> AtualizarUltimoValorCompra(long idProduto, double valorCompra);
        Task<bool> AtualizarPrecoVenda(long idProduto, double novoValorVenda);
        Task<bool> InsereProdutoCodigoBarra(long idProduto, string cdBarra);
        Task<long> InsereProdutoPendente(string NMPRODUTO, string UNCOMPRA, string CDNCM, string CDCEST, double NURELACAO, double NUPRECO, long idEmpresa);
        Task<List<Produto>> ObterProdutosParaAtualizarIbpt();
        Task AtualizarIBPTPorProduto(Produto produto);

    }

}

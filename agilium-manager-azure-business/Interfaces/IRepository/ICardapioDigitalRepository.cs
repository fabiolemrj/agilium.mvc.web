using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface ICardapioDigitalRepository
    {
        Task GarantirColunasIntegracaoAsync();
        Task SincronizarCategoriasAsync(List<Produto> produtosAgilium);
        Task<List<CardapioProdutoDto>> ObterTodosProdutosAsync();
        Task<Dictionary<string, int>> ObterCategoriasPorNomeAsync();
        Task InserirProdutoAsync(long idProdutoAgilium, string cdProduto, string nome, string descricao, decimal preco, int categoriaId, string? imagemUrl = null);
        Task AtualizarProdutoAsync(int idCardapio, string nome, string descricao, decimal preco, int categoriaId, string cdProduto, string? imagemUrl = null);
        Task DesativarProdutosNaoEncontradosAsync(List<long> idsAgiliumAtivos);
        /// <summary>Desativa um produto específico no CardapioDigital pelo ID do Agilium.</summary>
        Task<bool> DesativarProdutoPorIdAgiliumAsync(long idProdutoAgilium);
    }
}

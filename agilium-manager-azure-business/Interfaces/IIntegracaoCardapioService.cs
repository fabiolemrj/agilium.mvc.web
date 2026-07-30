using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces
{
    public interface IIntegracaoCardapioService
    {
        /// <summary>
        /// Exporta produtos do Agilium (com STEXPORTARPEDIDO = Sim) para o banco CardapioDigital.
        /// Retorna um resumo da operação: quantos foram inseridos, atualizados e erros.
        /// </summary>
        Task<ResultadoExportacao> ExportarProdutosAsync(long idEmpresa);

        /// <summary>
        /// Desativa um produto no CardapioDigital quando STEXPORTARPEDIDO = Não ou STPRODUTO = Inativo.
        /// </summary>
        Task<bool> DesativarProdutoNoCardapioAsync(long idProdutoAgilium);
    }

    public class ResultadoExportacao
    {
        public bool Sucesso { get; set; }
        public int TotalProdutosAgilium { get; set; }
        public int Inseridos { get; set; }
        public int Atualizados { get; set; }
        public int ComErro { get; set; }
        public List<string> Erros { get; set; } = new List<string>();
        public string Mensagem { get; set; }
    }

    /// <summary>
    /// Representa um produto do CardapioDigital para comparação
    /// </summary>
    public class CardapioProdutoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public string ImagemUrl { get; set; }
        public int CategoriaId { get; set; }
        public bool Ativo { get; set; }
        public bool Destaque { get; set; }
        /// <summary>PK do Agilium — chave de correspondência entre os sistemas</summary>
        public long? IdProdutoAgilium { get; set; }
        /// <summary>Código do produto (CDPRODUTO) — guia visual para o usuário</summary>
        public string CdProdutoPdv { get; set; }
    }
}

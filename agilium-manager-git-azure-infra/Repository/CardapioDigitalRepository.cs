using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Repository.Dapper;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.infra.Repository
{
    public class CardapioDigitalRepository : ICardapioDigitalRepository
    {
        private readonly CardapioDigitalDbSession _session;

        public CardapioDigitalRepository(CardapioDigitalDbSession session)
        {
            _session = session;
        }

        public async Task GarantirColunasIntegracaoAsync()
        {
            var colunasNecessarias = new Dictionary<string, string>
            {
                { "id_produto_agilium", "BIGINT NULL" },
                { "cd_produto_pdv", "VARCHAR(50) NULL" }
            };

            foreach (var coluna in colunasNecessarias)
            {
                try
                {
                    var colunaExiste = await _session.Connection.QueryFirstOrDefaultAsync<int>(
                        @"SELECT COUNT(*) FROM information_schema.COLUMNS
                          WHERE TABLE_SCHEMA = DATABASE()
                            AND TABLE_NAME = 'produto'
                            AND COLUMN_NAME = @NomeColuna",
                        new { NomeColuna = coluna.Key });

                    if (colunaExiste == 0)
                    {
                        await _session.Connection.ExecuteAsync(
                            $"ALTER TABLE produto ADD COLUMN {coluna.Key} {coluna.Value}");
                    }
                }
                catch
                {
                    // Coluna já existe — ignora
                }
            }
        }

        public async Task SincronizarCategoriasAsync(List<Produto> produtosAgilium)
        {
            var nomesGrupos = produtosAgilium
                .Where(p => p.GrupoProduto != null && !string.IsNullOrEmpty(p.GrupoProduto.Nome))
                .Select(p => p.GrupoProduto.Nome)
                .Distinct()
                .ToList();

            if (!nomesGrupos.Any()) return;

            var categoriasExistentes = (await _session.Connection.QueryAsync<string>(
                "SELECT nome FROM categoria")).ToList();

            var categoriasUpper = categoriasExistentes
                .Select(c => c?.ToUpper())
                .ToHashSet();

            foreach (var nomeGrupo in nomesGrupos)
            {
                if (!categoriasUpper.Contains(nomeGrupo?.ToUpper()))
                {
                    await _session.Connection.ExecuteAsync(
                        @"INSERT INTO categoria (nome, descricao, ativo, data_criacao, data_atualizacao)
                          VALUES (@Nome, @Descricao, 1, NOW(), NOW())",
                        new
                        {
                            Nome = nomeGrupo,
                            Descricao = $"Grupo importado do Agilium: {nomeGrupo}"
                        });
                }
            }
        }

        public async Task<List<CardapioProdutoDto>> ObterTodosProdutosAsync()
        {
            return (await _session.Connection.QueryAsync<CardapioProdutoDto>(
                @"SELECT p.id, p.nome, p.descricao, p.preco, p.imagem_url AS ImagemUrl,
                         p.categoria_id AS CategoriaId, p.ativo, p.destaque,
                         p.id_produto_agilium AS IdProdutoAgilium,
                         p.cd_produto_pdv AS CdProdutoPdv
                  FROM produto p"))
                .ToList();
        }

        public async Task<Dictionary<string, int>> ObterCategoriasPorNomeAsync()
        {
            var categorias = await _session.Connection.QueryAsync<CategoriaCardapioDto>(
                "SELECT id, nome FROM categoria");

            return categorias.ToDictionary(c => c.Nome?.ToUpper(), c => c.Id);
        }

        public async Task InserirProdutoAsync(
            long idProdutoAgilium, string cdProduto, string nome,
            string descricao, decimal preco, int categoriaId, string? imagemUrl = null)
        {
            await _session.Connection.ExecuteAsync(
                @"INSERT INTO produto
                  (nome, descricao, preco, categoria_id, ativo, destaque,
                   id_produto_agilium, cd_produto_pdv, imagem_url, data_criacao, data_atualizacao)
                  VALUES
                  (@Nome, @Descricao, @Preco, @CategoriaId, 1, 0,
                   @IdProdutoAgilium, @CdProdutoPdv, @ImagemUrl, NOW(), NOW())",
                new
                {
                    Nome = nome,
                    Descricao = descricao,
                    Preco = preco,
                    CategoriaId = categoriaId,
                    IdProdutoAgilium = idProdutoAgilium,
                    CdProdutoPdv = cdProduto,
                    ImagemUrl = imagemUrl
                });
        }

        public async Task AtualizarProdutoAsync(
            int idCardapio, string nome, string descricao,
            decimal preco, int categoriaId, string cdProduto, string? imagemUrl = null)
        {
            await _session.Connection.ExecuteAsync(
                @"UPDATE produto
                  SET nome = @Nome, descricao = @Descricao, preco = @Preco,
                      categoria_id = @CategoriaId, ativo = @Ativo,
                      cd_produto_pdv = @CdProdutoPdv, imagem_url = @ImagemUrl, data_atualizacao = NOW()
                  WHERE id = @Id",
                new
                {
                    Nome = nome,
                    Descricao = descricao,
                    Preco = preco,
                    CategoriaId = categoriaId,
                    Ativo = 1,
                    CdProdutoPdv = cdProduto,
                    ImagemUrl = imagemUrl,
                    Id = idCardapio
                });
        }

        public async Task DesativarProdutosNaoEncontradosAsync(List<long> idsAgiliumAtivos)
        {
            if (!idsAgiliumAtivos.Any()) return;

            await _session.Connection.ExecuteAsync(
                @"UPDATE produto
                  SET ativo = 0, data_atualizacao = NOW()
                  WHERE id_produto_agilium IS NOT NULL
                    AND ativo = 1
                    AND id_produto_agilium NOT IN @Ids",
                new { Ids = idsAgiliumAtivos });
        }

        public async Task<bool> DesativarProdutoPorIdAgiliumAsync(long idProdutoAgilium)
        {
            var rows = await _session.Connection.ExecuteAsync(
                @"UPDATE produto
                  SET ativo = 0, data_atualizacao = NOW()
                  WHERE id_produto_agilium = @IdProdutoAgilium
                    AND ativo = 1",
                new { IdProdutoAgilium = idProdutoAgilium });

            return rows > 0;
        }

        private class CategoriaCardapioDto
        {
            public int Id { get; set; }
            public string Nome { get; set; }
        }
    }
}

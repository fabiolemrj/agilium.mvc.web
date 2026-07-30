using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace agilium.api.business.Services
{
    public class IntegracaoCardapioService : IIntegracaoCardapioService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICardapioDigitalRepository _cardapioDigitalRepository;
        private readonly IProdutoFotoRepository _produtoFotoRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public IntegracaoCardapioService(
            IProdutoRepository produtoRepository,
            ICardapioDigitalRepository cardapioDigitalRepository,
            IProdutoFotoRepository produtoFotoRepository,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _produtoRepository = produtoRepository;
            _cardapioDigitalRepository = cardapioDigitalRepository;
            _produtoFotoRepository = produtoFotoRepository;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResultadoExportacao> ExportarProdutosAsync(long idEmpresa)
        {
            var resultado = new ResultadoExportacao();

            try
            {
                // 1. Buscar produtos Agilium marcados para exportação
                var produtosAgilium = (await _produtoRepository
                    .Buscar(x => x.idEmpresa == idEmpresa
                              && x.STEXPORTARPEDIDO == ESimNao.Sim
                              && x.STPRODUTO == EAtivo.Ativo))
                    .ToList();

                resultado.TotalProdutosAgilium = produtosAgilium.Count;

                if (!produtosAgilium.Any())
                {
                    resultado.Sucesso = true;
                    resultado.Mensagem = "Nenhum produto marcado para exportação (STEXPORTARPEDIDO = Sim) encontrado.";
                    return resultado;
                }

                // 2. Garantir colunas de integração no CardapioDigital
                await _cardapioDigitalRepository.GarantirColunasIntegracaoAsync();

                // 3. Sincronizar categorias (grupos)
                await _cardapioDigitalRepository.SincronizarCategoriasAsync(produtosAgilium);

                // 4. Obter produtos existentes e categorias do CardapioDigital
                var produtosCardapio = await _cardapioDigitalRepository.ObterTodosProdutosAsync();
                var categoriasCardapio = await _cardapioDigitalRepository.ObterCategoriasPorNomeAsync();

                var lookupCardapio = produtosCardapio
                    .Where(p => p.IdProdutoAgilium.HasValue)
                    .ToDictionary(p => p.IdProdutoAgilium.Value, p => p);

                // 5. Para cada produto Agilium — INSERT ou UPDATE
                foreach (var produtoAgilium in produtosAgilium)
                {
                    try
                    {
                        var idProdutoAgilium = produtoAgilium.Id;
                        var cdProduto = produtoAgilium.CDPRODUTO;
                        var nome = produtoAgilium.NMPRODUTO ?? "";
                        var descricao = nome;
                        var preco = Convert.ToDecimal(produtoAgilium.NUPRECO ?? 0);

                        var nomeGrupo = produtoAgilium.GrupoProduto?.Nome ?? "";
                        int categoriaId = 1;

                        if (!string.IsNullOrEmpty(nomeGrupo) &&
                            categoriasCardapio.ContainsKey(nomeGrupo.ToUpper()))
                        {
                            categoriaId = categoriasCardapio[nomeGrupo.ToUpper()];
                        }

                        // 5a. Obter foto do produto e gerar URL para o CardapioDigital
                        var imagemUrl = await ObterUrlFotoProdutoAsync(idProdutoAgilium);

                        if (lookupCardapio.ContainsKey(idProdutoAgilium))
                        {
                            await _cardapioDigitalRepository.AtualizarProdutoAsync(
                                lookupCardapio[idProdutoAgilium].Id, nome, descricao, preco, categoriaId, cdProduto, imagemUrl);
                            resultado.Atualizados++;
                        }
                        else
                        {
                            await _cardapioDigitalRepository.InserirProdutoAsync(
                                idProdutoAgilium, cdProduto, nome, descricao, preco, categoriaId, imagemUrl);
                            resultado.Inseridos++;
                        }
                    }
                    catch (Exception ex)
                    {
                        resultado.ComErro++;
                        resultado.Erros.Add(
                            $"Erro ao exportar produto '{produtoAgilium.CDPRODUTO} - {produtoAgilium.NMPRODUTO}': {ex.Message}");
                    }
                }

                // 6. Desativar no destino produtos que não estão mais na lista de exportação
                var idsAgiliumAtivos = produtosAgilium.Select(p => p.Id).ToList();
                await _cardapioDigitalRepository.DesativarProdutosNaoEncontradosAsync(idsAgiliumAtivos);

                resultado.Sucesso = resultado.ComErro == 0;
                resultado.Mensagem = resultado.ComErro == 0
                    ? $"Exportação concluída com sucesso! {resultado.Inseridos} inserido(s), {resultado.Atualizados} atualizado(s) de {resultado.TotalProdutosAgilium} produto(s)."
                    : $"Exportação parcial: {resultado.Inseridos} inserido(s), {resultado.Atualizados} atualizado(s), {resultado.ComErro} erro(s) de {resultado.TotalProdutosAgilium} produto(s).";
            }
            catch (Exception ex)
            {
                resultado.Sucesso = false;
                resultado.Mensagem = $"Erro na exportação: {ex.Message}";
                resultado.Erros.Add(ex.Message);
            }

            return resultado;
        }

        public async Task<bool> DesativarProdutoNoCardapioAsync(long idProdutoAgilium)
        {
            return await _cardapioDigitalRepository.DesativarProdutoPorIdAgiliumAsync(idProdutoAgilium);
        }

        /// <summary>
        /// Busca a primeira foto do produto e faz upload para o endpoint do CardapioDigital.
        /// Retorna a URL pública retornada pela API ou null se não houver foto.
        /// </summary>
        private async Task<string?> ObterUrlFotoProdutoAsync(long idProduto)
        {
            try
            {
                var fotos = await _produtoFotoRepository
                    .Buscar(f => f.idProduto == idProduto && f.Foto != null);

                var foto = fotos.FirstOrDefault(f => f.Foto != null && f.Foto.Length > 0);
                if (foto == null)
                    return null;

                var extensao = ObterExtensaoPorBytes(foto.Foto);
                var nomeArquivo = $"{idProduto}{extensao}";

                // Upload para o endpoint do CardapioDigital
                using var content = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(foto.Foto);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(
                    extensao switch
                    {
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        ".webp" => "image/webp",
                        _ => "image/jpeg"
                    });
                content.Add(fileContent, "file", nomeArquivo);

                var apiBaseUrl = _configuration.GetValue<string>("CardapioDigital:ApiBaseUrl") ?? "";
                var httpClient = _httpClientFactory.CreateClient("CardapioDigital");
                var response = await httpClient.PostAsync($"{apiBaseUrl}/api/upload/imagem", content);

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var url = doc.RootElement.GetProperty("url").GetString();

                return url;
            }
            catch (Exception)
            {
                // Falha ao processar foto não deve interromper a exportação
                return null;
            }
        }

        /// <summary>
        /// Detecta a extensão do arquivo a partir dos bytes (magic numbers).
        /// </summary>
        private static string ObterExtensaoPorBytes(byte[] bytes)
        {
            if (bytes.Length < 4) return ".jpg";

            // PNG: 89 50 4E 47
            if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
                return ".png";

            // GIF: 47 49 46
            if (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46)
                return ".gif";

            // BMP: 42 4D
            if (bytes[0] == 0x42 && bytes[1] == 0x4D)
                return ".bmp";

            // WebP: 52 49 46 46 ... 57 45 42 50
            if (bytes.Length >= 12 &&
                bytes[0] == 0x52 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x46 &&
                bytes[8] == 0x57 && bytes[9] == 0x45 && bytes[10] == 0x42 && bytes[11] == 0x50)
                return ".webp";

            return ".jpg"; // fallback
        }
    }
}

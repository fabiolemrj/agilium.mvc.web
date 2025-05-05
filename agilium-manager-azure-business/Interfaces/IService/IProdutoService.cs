using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IProdutoService : IDisposable
    {
        #region Produto
        Task Adicionar(Produto produto);
        Task Atualizar(Produto produto);
        Task Apagar(long id);
        Task<Produto> ObterPorId(long id);
        Task<List<Produto>> ObterPorDescricao(string descricao);
        Task<PagedResult<Produto>> ObterPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<Produto> ObterCompletoPorId(long id);
        Task<List<Produto>> ObterTodas(long idEmpresa);

        Task<double> ObterPrecoAtual(long idProduto);
        #endregion

        #region ProdutoDepartamento
        Task<bool> Adicionar(ProdutoDepartamento produtoDepartamento);
        Task<IEnumerable<ProdutoDepartamento>> ObterTodosDepartamento();
        Task<IEnumerable<ProdutoDepartamento>> ObterTodosDepartamento(params string[] includes);
        Task<ProdutoDepartamento> ObterPorIdDepartamento(long id);
        Task<bool> Existe(ProdutoDepartamento produtoDepartamento);
        Task<bool> Atualizar(ProdutoDepartamento produtoDepartamento);
        Task<bool> ApagarDepartamento(long id);
        Task<PagedResult<ProdutoDepartamento>> ObterPaginacaoPorDescricao(long idempresa, string descricao, int page = 1, int pageSize = 15);

        #endregion

        #region ProdutoMarca
        Task<bool> Adicionar(ProdutoMarca produtoMarca);
        Task<IEnumerable<ProdutoMarca>> ObterTodosMarca();
        Task<IEnumerable<ProdutoMarca>> ObterTodosProdutoMarca(params string[] includes);
        Task<ProdutoMarca> ObterPorIdMarca(long id);
        Task<bool> Existe(ProdutoMarca produtoMarca);
        Task<bool> Atualizar(ProdutoMarca produtoMarca);
        Task<bool> ApagarProdutoMarca(long id);
        Task<PagedResult<ProdutoMarca>> ObterMarcaPaginacaoPorDescricao(long idempresa, string descricao, int page = 1, int pageSize = 15);

        #endregion

        #region Grupo
        Task<bool> Adicionar(GrupoProduto grupoProduto);
        Task<IEnumerable<GrupoProduto>> ObterTodosGrupos();
        Task<bool> Existe(GrupoProduto grupoProduto);
        Task<bool> Atualizar(GrupoProduto grupoProduto);
        Task<bool> ApagarProdutoGrupo(long id);
        Task<PagedResult<GrupoProduto>> ObterGrupoPaginacaoPorDescricao(long idempresa, string descricao, int page = 1, int pageSize = 15);
        Task<GrupoProduto> ObterPorIdGrupo(long id);
        #endregion

        #region SubGrupo
        Task<bool> Adicionar(SubGrupoProduto subGrupoProduto);
        Task<IEnumerable<SubGrupoProduto>> ObterTodosSubGrupos();
        Task<bool> Existe(SubGrupoProduto subGrupoProduto);
        Task<bool> Atualizar(SubGrupoProduto subGrupoProduto);
        Task<bool> ApagarProdutoSubGrupo(long id);
        Task<PagedResult<SubGrupoProduto>> ObterSubGrupoPaginacaoPorDescricao(long idGrupo, string descricao, int page = 1, int pageSize = 15);
        Task<SubGrupoProduto> ObterPorIdSubGrupo(long id);

        #endregion

        #region Codigo de Barra
        Task Adicionar(ProdutoCodigoBarra produto);
        Task Atualizar(ProdutoCodigoBarra produto);
        Task ApagarCodigoBarra(long id);
        Task<ProdutoCodigoBarra> ObterCodigoBarraPorId(long id);
        Task<IEnumerable<ProdutoCodigoBarra>> ObterTodosCodigoBarraPorProduto(long idProduto);
        #endregion

        #region Preco
        Task Adicionar(ProdutoPreco produto);
        Task Atualizar(ProdutoPreco produto);
        Task ApagarPreco(long id);
        Task<ProdutoPreco> ObterPrecoPorId(long id);
        Task<IEnumerable<ProdutoPreco>> ObterPrecoPorProduto(long idProduto);
        #endregion

        #region Produto Foto
        Task Adicionar(ProdutoFoto produto);
        Task Atualizar(ProdutoFoto produto);
        Task ApagarFoto(long id);
        Task<ProdutoFoto> ObterFotoPorId(long id);
        Task<IEnumerable<ProdutoFoto>> ObterFotoPorProduto(long idProduto);
        #endregion


        Task Salvar();
    }

}

using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IDevolucaoService: IDisposable
    {
        Task Salvar();
     
        #region MotivoDevolucao
        Task<bool> Adicionar(MotivoDevolucao motivoDevolucao);
        Task<IEnumerable<MotivoDevolucao>> ObterTodosMotivos();
        Task<IEnumerable<MotivoDevolucao>> ObterTodosProdutoMotivos(params string[] includes);
        Task<MotivoDevolucao> ObterPorIdMotivo(long id);
        Task<bool> Existe(MotivoDevolucao motivoDevolucao);
        Task<bool> Atualizar(MotivoDevolucao motivoDevolucao);
        Task<bool> ApagarMotivo(long id);
        Task<PagedResult<MotivoDevolucao>> ObterMotivoPaginacaoPorDescricao(long idempresa, string descricao, int page = 1, int pageSize = 15);

        #endregion

        #region Devolucao
        Task Adicionar(Devolucao devolucao);
        Task Atualizar(Devolucao devolucao);
        Task Apagar(long id);
        Task<Devolucao> ObterDevolucaoPorId(long id);
        Task<Devolucao> ObterPorId(long id);
        Task<PagedResult<Devolucao>> ObterDevolucaoPorPaginacao(long idEmpresa, DateTime dtIni, DateTime dtFim, int page = 1, int pageSize = 15);
        Task<IEnumerable<Devolucao>> ObterTodasDevolucoes(long idEmpresa);
        #endregion

        #region Devolucao Item
        Task<IEnumerable<DevolucaoItem>> ObterDevolucaoItens(long idDevolucao);
        Task<DevolucaoItem> ObterDevolucaoItemPorId(long id);
        Task Adicionar(DevolucaoItem devolucaoItem);
        Task AdicionarAtualizar(DevolucaoItem devolucaoItem);
        #endregion


    }
}

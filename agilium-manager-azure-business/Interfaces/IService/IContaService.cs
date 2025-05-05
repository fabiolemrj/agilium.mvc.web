using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IContaService: IDisposable
    {
        Task Salvar();

        #region Conta Pagar
        Task Adicionar(ContaPagar contaPagar);
        Task Atualizar(ContaPagar contaPagar);
        Task Apagar(long id);
        Task<ContaPagar> ObterPorId(long id);
        Task<List<ContaPagar>> ObterPorDescricao(string descricao);
        Task<PagedResult<ContaPagar>> ObterPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ContaPagar> ObterCompletoPorId(long id);
        Task<List<ContaPagar>> ObterTodas(long idEmpresa);

        #endregion

        #region Conta Receber
        Task Adicionar(ContaReceber contaReceber);
        Task Atualizar(ContaReceber contaReceber);
        Task ApagarContaReceber(long id);
        Task<ContaReceber> ObterContaReceberPorId(long id);
        Task<List<ContaReceber>> ObterContaReceberPorDescricao(string descricao);
        Task<PagedResult<ContaReceber>> ObterContaReceberPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ContaReceber> ObterContaReceberCompletoPorId(long id);
        Task<List<ContaReceber>> ObterTodasContaReceber(long idEmpresa);
        #endregion
    }
}

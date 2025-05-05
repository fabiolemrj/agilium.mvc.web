using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IPlanoContaService : IDisposable
    {
        #region Plano Conta
        Task Adicionar(PlanoConta planoConta);
        Task Atualizar(PlanoConta planoConta);
        Task Apagar(long id);
        Task<PlanoConta> ObterPorId(long id);
        Task<List<PlanoConta>> ObterPorDescricao(string descricao);
        Task<PagedResult<PlanoConta>> ObterPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<PlanoConta> ObterCompletoPorId(long id);
        Task<List<PlanoConta>> ObterTodas(long idEmpresa);

        #endregion

        #region Plano Conta Saldo
        Task Adicionar(PlanoContaSaldo planoContaSaldo); 
        Task Atualizar(PlanoContaSaldo planoContaSaldo);
        Task ApagarSaldo(long id);
        Task<List<PlanoContaSaldo>> ObterSaldoPorPlano(long idPlano);
        Task<PlanoContaSaldo> ObterSaldoPorId(long id);
        Task<double> ObterSaldoPorIdPlano(long idPlano);
        #endregion


        #region Plano Conta Lancamento
        Task<PagedResult<PlanoContaLancamento>> ObterLancamentoPorPaginacao(long idPlano, DateTime dtIni, DateTime dtFim, int page = 1, int pageSize = 15);
        Task Adicionar(PlanoContaLancamento planoConta);
        Task Atualizar(PlanoContaLancamento planoConta);
        Task ApagarLancamento(long id);
        Task<PlanoContaLancamento> ObterLancamentoPorId(long id);
        #endregion

        Task Salvar();
    }
}

using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface IContaPagarRepository: IRepository<ContaPagar>
    {
    }

    public interface IContaReceberRepository : IRepository<ContaReceber>
    {
    }

    public interface IPContaPagarDapperRepository
    {
        Task<bool> ConsolidarConta(long id);
        Task<bool> DesconsolidarConta(long id);
        Task<PlanoContaLancamento> CriarContaLancamentoDeContaPagar(long id);
        Task<bool> RealizarLancamento(long idLanc, PlanoContaLancamento planoContaLancamento);
        Task<bool> AtualizarConsolidacaoContaPagar(long idLanc, long idConta);
        Task<PlanoContaLancamento> ObterPlanoContaLancamentoPorId(long idLanc);
        Task<bool> AtualizarDesconsolidacaoContaPagarPorId(long idLanc);
        Task<bool> ApagarLancamentoPorId(long idLanc);
        Task<bool> AtualizarLancamentoPorId(long idLanc);
    }

    public interface IPContaReceberDapperRepository
    {
        Task<bool> ConsolidarConta(long id);
        Task<bool> DesconsolidarConta(long id);
        Task<PlanoContaLancamento> CriarContaLancamentoDeContaReceber(long id);
        Task<bool> RealizarLancamento(long idLanc, PlanoContaLancamento planoContaLancamento);
        Task<bool> AtualizarConsolidacaoContaReceber(long idLanc, long idConta);
        Task<PlanoContaLancamento> ObterPlanoContaLancamentoPorId(long idLanc);
        Task<bool> AtualizarDesconsolidacaoContaReceberPorId(long idLanc);
        Task<bool> ApagarLancamentoPorId(long idLanc);
        Task<bool> AtualizarLancamentoPorId(long idLanc);
    }
}

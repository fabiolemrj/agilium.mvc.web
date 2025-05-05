using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface IPlanoContaRepository : IRepository<PlanoConta> { }

    public interface IPlanoContaSaldoRepository : IRepository<PlanoContaSaldo> { }

    public interface IPlanoContaLancamentoRepository: IRepository<PlanoContaLancamento> { }
    public interface IPlanoContaDapperRepository
    {
        Task AtualizarSaldoContaESubConta(long IdConta);
        Task<List<PlanoContaLancamento>> ObterLancamentosPorPlanoEData(long idPlano, DateTime dtInicial, DateTime dtFinal);
        Task<string> ObterDescricaoPlano(long idPlano);
        Task RealizarLancamento(PlanoContaLancamento planoContaLancamento);
        Task<long> RealizarLancamento(long idConta, DateTime DataRef, string Descricao, double valorLancamento, ETipoContaLancacmento tipoContaLancacmento);
        Task<long> ObterContaPrimeiroNivel(long idConta);
        Task<bool> ExcluirLancamento(long idLanc);
    }
}

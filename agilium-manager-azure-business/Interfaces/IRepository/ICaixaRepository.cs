using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.PdvViewModel.CaixaReturnViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface ICaixaRepository: IRepository<Caixa>
    {
    }
    public interface ICaixaMoedaRepository : IRepository<CaixaMoeda>
    {
    }
    public interface ICaixaMovimentoRepository : IRepository<CaixaMovimento>
    {
    }
     

    public interface ICaixaDapperRepository
    {
        Task<long> ObterIdFuncionarioPorUsuarioEmpresa(long idEmpresa, long idUsuario);
        Task<long> ObterIdCaixaAberto(long idEmpresa, long idUsuario);
        Task<PontoVenda> ObterPdvPorNomeMaquina(string maquina);
        Task<Config> ObterTipoAberturaCaixaPorEmpresa(long idEmpresa);
        Task<double> ObterValorCaixaAnterior(long idFuncionario, long idEmpresa);
        Task<double> ObterConfigRealizaSuprimentoAbertura(long idEmpresa);
        Task<string> ObterConfigDescricaoSuprimentoAbertura(long idEmpresa);
        Task<long> IncluirCaixa(long idEmpresa, long idTurno, long idPdv, long idFuncionario, int sqCaixa, double valorAbertura);
        Task<long> RealizarSuprimento(long idCaixa, long idFunc, double vlMovimento, string descricao);
        Task<Caixa> ObterCaixaPorId(long idCaixa);
        Task<int> GerarSeqCaixa(long idEmpresa);
        Task<long> RealizarSangria(long idCaixa, long idFunc, double vlMovimento, string descricao);
        Task<double> ObterSaldoCaixa(long idCaixa);
        Task<bool> FecharCaixa(long idCaixa, double valorFechamento);
        Task<FecharCaixaInicializarViewModel> ObterCaixaParaFechamento(long idCaixa);
        Task<Caixa> ObterCaixaAberto(long idEmpresa, long idUsuario);
        Task<long> ObterEstoquePorIdCaixa(long idCaixa);
    }
}

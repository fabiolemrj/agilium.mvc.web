using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.PdvViewModel.CaixaReturnViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface ICaixaService : IDisposable
    {
        Task Salvar();

        #region Caixa
        Task<PagedResult<Caixa>> ObterPorPaginacao(long idEmpresa, DateTime dtIni, DateTime dtFim, int page = 1, int pageSize = 15);
        Task<Caixa> ObterCompletoPorId(long id);
        #endregion

        #region Caixa Movimentacao
        Task<PagedResult<CaixaMovimento>> ObterMovimentacaoPorPaginacao(long idCaixa, int page = 1, int pageSize = 15);
        #endregion

        #region Caixa Moeda
        Task<PagedResult<CaixaMoeda>> ObterMoedaPorPaginacao(long idCaixa, int page = 1, int pageSize = 15);
        Task RealizarCorrecaoValor(CaixaMoeda caixaMoeda);
        Task<CaixaMoeda> ObterCaixaMoedaCompletoPorId(long id);
        #endregion

        #region Dapper
        Task<int> AbrirCaixa(long idEmpresa, long idUsuario, long idPdv);
        Task<bool> FecharCaixa(long idCaixa, long idUsuario, double valorFechamneto, string msgFechamento);
        Task<FecharCaixaInicializarViewModel> ObterCaixaParaFechamento(long idCaixa);
        Task<FecharCaixaInicializarViewModel> ObterCaixaParaFechamento(long idEmpresa, long idusuario);
        Task<bool> RealizarSangria(long idCaixa, long idUsuario, double valor, string msg);
        Task<bool> RealizarSuprimento(long idCaixa, long idUsuario, double valor, string msg);
        Task<Caixa> ObterCaixaAbertoPorEmpresa(long idEmpresa, long idUsuario);

        #endregion
    }
}

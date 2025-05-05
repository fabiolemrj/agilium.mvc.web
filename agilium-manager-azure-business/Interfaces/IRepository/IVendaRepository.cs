using agilium.api.business.Enums;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.ReportViewModel.VendaReportViewModel;
using agilium.api.business.Models.CustomReturn.VendaCustomViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface IVendaRepository: IRepository<Venda>
    {
    }

    public interface IVendaItemRepository : IRepository<VendaItem>
    {
    }

    public interface IVendaMoedaRepository : IRepository<VendaMoeda>
    {
    }

    public interface IVendaFiscalRepository : IRepository<VendaFiscal>
    {
    }

    public interface IVendaCanceladaRepository : IRepository<VendaCancelada>
    {
    }

    public interface IVendaEspelhoRepository : IRepository<VendaEspelho>
    {
    }
    public interface IVendaTemporariaRepository : IRepository<VendaTemporaria>
    {
    }

    public interface IVendaTemporariaEspelhoRepository : IRepository<VendaTemporariaEspelho>
    {
    }

    public interface IVendaTemporariaItemRepository : IRepository<VendaTemporariaItem>
    {
    }

    public interface IVendaTemporariaMoedaRepository : IRepository<VendaTemporariaMoeda>
    {
    }


    public interface IVendaDapperRepository
    {
        Task<dynamic> ObterVendasRankingPorProduto(DateTime dataInicial, DateTime dataFinal);
        Task<List<VendaReportViewModel>> ObterVendasReportViewModel(DateTime dataInicial, DateTime dataFinal);
        Task<List<VendaItemReportViewModel>> ObterItensVendaReportViewModelPorVenda(long idVenda);
        Task<List<VendaMoedaItemReportViewModel>> ObterMoedaItensVendaReportViewModelPorVenda(long idVenda);
        Task<List<VendaMoedaItemReportViewModel>> ObterMoedaTotalReportViewModelPorVenda(DateTime dataInicial, DateTime dataFinal);
        Task<List<VendaFornecedorReportViewModel>> ObterVendasPorFornecedor(DateTime dataInicial, DateTime dataFinal);
        Task<List<TotalVendaMoedaPorDataReport>> ObterVendaMoedaTotalizadasPorData(DateTime data);
        Task<List<DateTime>> ObterListaVendaAgrupadasPorData(DateTime dataInicial, DateTime dataFinal);
        Task<List<TotalVendaMoedaPorDataReport>> ObterVendaMoedaTotalizadasPorData(DateTime dataInicial, DateTime dataFinal);
        Task<List<VendaRankingReport>> ObterVendaRankingPorData(DateTime dataInicial, DateTime dataFinal, EResultadoFiltroRanking tipoResultado, EOrdenacaoFiltroRanking ordenacao);
        Task<List<VendaDiferencaCaixaReport>> ObterVendaDiferencaCaixa(DateTime dataInicial, DateTime dataFinal);
        Task<bool> RetirarEstoqueComposicao(long idProduto, string nmUsuario, int sqvenda, int sqcaixa);
        Task<long> AdicionarVenda(Venda venda, long idEstoque, int sqcaixa, string nomeUsuario, string cpf);
        Task<int> GerarNuNf(long idEmpresa, bool homologacao);
        Task<Venda> ObterVendaPorId(long idVenda);
        Task<Empresa> ObterEmpresaPorId(long id);
        Task<PontoVenda> ObterPdvPorId(long id);
        Task<Endereco> ObterEnderecoPorId(long id);
        Task<Cliente> ObterClientePorId(long id);
        Task<int> GerarSqVendaPorCaixa(long id);
        Task<bool> ApagarVendaTemporaria(long id);

        Task<long> AdicionarVendaTemporaria(Venda venda, long idEstoque, int sqcaixa, string nomeUsuario,string cpf);
    }


}

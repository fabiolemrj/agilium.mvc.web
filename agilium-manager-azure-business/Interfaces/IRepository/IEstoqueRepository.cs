using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.ReportViewModel.EstoqueReportViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface IEstoqueRepository : IRepository<Estoque>
    {
    }

    public interface IEstoqueProdutoRepository : IRepository<EstoqueProduto>
    {
    }

    public interface IEstoqueHistoricoRepository : IRepository<EstoqueHistorico>
    {
    }

    public interface IEstoqueDapperRepository
    {
        Task<long> RealizaEntradaRetornaIdHistoricoGerado(long idEstoque, long idProduto, long idItem, string UsuarioHistorico, string DescricaoHistorico,
            double Quantidade);
        Task<long> RealizaRetiradaRetornaIdHistoricoGerado(long idEstoque, long idProduto, long idItem, string UsuarioHistorico, string DescricaoHistorico,
            double Quantidade);

        Task<bool> AtaulizarLancamentoEstoqueHistorico(long idLancamento, long idEstoqueHist);
        Task<bool> DesvincularHistoricoDoLancamento(long idEstoqueHistorico);
        Task<List<EstoquePosicaoReport>> ObterPosicaoEstoque(long idEstoqueHistorico);
    }
}

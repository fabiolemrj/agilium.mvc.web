using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface IMotivoDevolucaoRepository : IRepository<MotivoDevolucao>
    {
    }

    public interface IDevolucaoRepository : IRepository<Devolucao>
    {
        Task<List<Devolucao>> ObterDevolucaoPorPaginacao(long idEmpresa, DateTime dtIni, DateTime dtFim, int page = 1, int pageSize = 15);
        Task<Devolucao> ObterDevolucaoPorId(long id);
    }

    public interface IDevolucaoItemRepository : IRepository<DevolucaoItem>
    {
    }

    public interface IDevolucaoDapperRepository
    {
        Task<bool> RealizarDevolucao(long idDevolucao, string usuario);
        Task<List<DevolucaoItemVendaCustom>> ObterItensComVendaItens(long idVenda, long idDevolucao);
        
    }
}

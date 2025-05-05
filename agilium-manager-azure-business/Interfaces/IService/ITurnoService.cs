using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface ITurnoService: IDisposable
    {
        Task Salvar();
     
        #region Turno
        Task<PagedResult<Turno>> ObterPorPaginacao(long idEmpresa, DateTime dtIni, DateTime dtFim, int page = 1, int pageSize = 15);
        Task<Turno> ObterCompletoPorId(long id);
        Task<Turno> ObterTurnoAbertoCompletoPorId(long id);
        Task<List<Turno>> ObterTodos(long idEmpresa);
        #endregion

        #region Turno Preco
        Task Adicionar(TurnoPreco turnoPreco);
        Task Remover(long id);
        Task<TurnoPreco> ObteClientePrecoPorId(long id);
        Task<IEnumerable<TurnoPreco>> ObterTurnoPrecoPorProduto(long idProduto);
        #endregion

    }
}


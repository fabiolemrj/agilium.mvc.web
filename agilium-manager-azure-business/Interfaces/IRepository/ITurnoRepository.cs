using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface ITurnoRepository : IRepository<Turno>
    {
    }

    public interface ITurnoPrecoRepository : IRepository<TurnoPreco>
    {

    }

    public interface IPTurnoDapperRepository
    {
        Task<bool> AbrirTurno(long idEmpresa, long idUsuario);
        Task<bool> FecharTurno(long idEmpresa, long idUsuario, string obs);
        Task<bool> TurnoAberto(long id);
        Task<Turno> ObterTurnoAberto(long idEmpresa);
        Task<bool> TurnoAbertoPorId(long id);
        Task<Turno> ObterTurnoAbertoPorIdEmpresa(long idEmpresa);
        Task<int> GerarNumeroTurnoPorIdEmpresa(long idEmpresa);
        Task IncluirTurno(long novoId, Turno turno);
        Task<Turno> ObterObjetoTurnoAbertoPorIdEmpresa(long idEmpresa);
        Task FecharTurno(Turno turno, long idUsuarioFim);
    }

}

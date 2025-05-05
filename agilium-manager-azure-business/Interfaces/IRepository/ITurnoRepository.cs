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
    }

}

using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface ILogRepository : IRepository<LogSistema>
    {
    }

    public interface ILogErroRepository : IRepository<LogErro>
    {
    }

    public interface ILogDapper
    {
        Task<bool> Adicionar(LogSistema logSistema);
        Task<bool> Adicionar(string usuario, string descr, string tela, string controle, string maquina, string sql_log, string so);
        Task<bool> Erro(string usuario, string descr, string tela, string controle, string maquina, string sql_log, string tipo);
    }
}

using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface ILogService
    {
        Task<PagedResult<LogSistema>> ObterPorData(DateTime dtIni, DateTime dtFim, int page = 1, int pageSize = 25);
        Task<bool> Adicionar(string usuario, string descr, string tela, string controle, string maquina, string sql_log, string so);
        Task<bool> Erro(string usuario, string descr, string tela, string controle, string maquina, string sql_log, string tipo);
    }
}

using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Services
{
    public class LogService : ILogService
    {
        private readonly ILogDapper _logDapper;
        private readonly ILogRepository _logRepository;

        public LogService(ILogDapper logDapper, ILogRepository logRepository)
        {
            _logDapper = logDapper;
            _logRepository = logRepository;
        }

        #region Log
        public async Task<bool> Adicionar(string usuario, string descr, string tela, string controle, string maquina, string sql_log, string so)
        {
            return await _logDapper.Adicionar(usuario,descr,tela,controle,maquina,sql_log,so);
        }

        public async Task<PagedResult<LogSistema>> ObterPorData(DateTime dtIni, DateTime dtFim, int page = 1, int pageSize = 25)
        {
            int pagina = page > 0 ? page : 1;

            var lista = _logRepository.Obter(x => x.data_log >= dtIni && x.data_log <= dtFim).Result;

            return new PagedResult<LogSistema>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }
        #endregion

        #region Erro

        public async Task<bool> Erro(string usuario, string descr, string tela, string controle, string maquina, string sql_log, string tipo)
        {
           return await _logDapper.Erro(usuario,descr, tela, controle, maquina,sql_log,tipo);
        }
        #endregion
    }
}

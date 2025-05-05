using agilium.api.business.Interfaces;
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
    public class TabelaAuxiliarFiscalService : BaseService, ITabelaAuxiliarFiscalService
    {
        private readonly INcmRepository _ncmRepository;
        private readonly ICfopRepository _cfopRepository;
        private readonly ICestNcmRepository _cestNcmRepository;
        private readonly ICsosnRepository _csosnRepository;
        private readonly IIbptRepository _ibptRepository;
        private readonly ICstRepository _cstRepository;

        public TabelaAuxiliarFiscalService(INotificador notificador, INcmRepository ncmRepository,
            ICfopRepository cfopRepository, ICestNcmRepository cestNcmRepository,
            ICsosnRepository csosnRepository, IIbptRepository ibptRepository,
            ICstRepository cstRepository) : base(notificador)
        {
            _ncmRepository = ncmRepository;
            _cfopRepository = cfopRepository;
            _cestNcmRepository = cestNcmRepository;
            _csosnRepository = csosnRepository;
            _ibptRepository = ibptRepository;
            _cstRepository = cstRepository;
        }

        public void Dispose()
        {
            _cestNcmRepository?.Dispose();
            _cfopRepository?.Dispose();
            _csosnRepository?.Dispose();
            _ibptRepository?.Dispose();
            _ncmRepository?.Dispose();
            _cstRepository?.Dispose();
        }

        public async Task<CestNcm> ObterCestNcmPorId(long id)
        {
            return await _cestNcmRepository.ObterPorId(id);
        }

        public async Task<Cfop> ObterCfopPorId(int id)
        {
            return _cfopRepository.Obter(x=>x.CDCFOP == id).Result.FirstOrDefault();
        }

        public async Task<Csosn> ObterCsosnPorId(string id)
        {
            return _csosnRepository.Obter(x => x.CST == id).Result.FirstOrDefault();
        }

        public async Task<Cst> ObterCstPorId(string id)
        {
            return _cstRepository.Obter(x => x.CST == id).Result.FirstOrDefault();
        }

        public async Task<Ibpt> ObterIbptPorId(long id)
        {
            return await _ibptRepository.ObterPorId(id);
        }

        public async Task<CestNcm> ObterNcmPorId(string id)
        {
            return _cestNcmRepository.Obter(x => x.CDNCM == id).Result.FirstOrDefault();
        }

        public async Task<IEnumerable<CestNcm>> ObterTodosCestNcm()
        {
            return await _cestNcmRepository.ObterTodos();
        }

        public async Task<IEnumerable<Cfop>> ObterTodosCfop()
        {
            return await _cfopRepository.ObterTodos();
        }

        public async Task<IEnumerable<Csosn>> ObterTodosCsosn()
        {
            return await _csosnRepository.ObterTodos();
        }

        public async Task<IEnumerable<Cst>> ObterTodosCst()
        {
            return await _cstRepository.ObterTodos();
        }

        public async Task<IEnumerable<Ibpt>> ObterTodosIbpt()
        {
            return await _ibptRepository.ObterTodos();
        }

        public async Task<IEnumerable<CestNcm>> ObterTodosNcm()
        {
            return await _cestNcmRepository.ObterTodos();
        }
    }
}

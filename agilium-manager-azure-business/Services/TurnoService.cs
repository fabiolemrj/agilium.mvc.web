using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace agilium.api.business.Services
{
    public class TurnoService : BaseService, ITurnoService
    {
        private readonly ITurnoPrecoRepository _turnoPrecoRepository;
        private readonly ITurnoRepository _turnoRepository;
        private readonly IPTurnoDapperRepository _turnoDapperRepository;
        private readonly IDapperRepository _dapperRepository;
        private readonly IUtilDapperRepository _utilDapperRepository;

        public TurnoService(INotificador notificador, ITurnoPrecoRepository turnoPrecoRepository, ITurnoRepository turnoRepository, IPTurnoDapperRepository turnoDapperRepository,
            IDapperRepository dapperRepository,IUtilDapperRepository utilDapperRepository) : base(notificador)
        {
            _turnoPrecoRepository = turnoPrecoRepository;
            _turnoRepository = turnoRepository;
            _turnoDapperRepository = turnoDapperRepository;
            _dapperRepository = dapperRepository;
            _utilDapperRepository = utilDapperRepository;
        }

        public void Dispose()
        {
            _turnoPrecoRepository?.Dispose();
            _turnoRepository?.Dispose();
        }

        public async Task Salvar()
        {
            await _turnoRepository.SaveChanges(); 
        }

        #region Turno

        public async Task<PagedResult<Turno>> ObterPorPaginacao(long idEmpresa, DateTime dtIni, DateTime dtFim, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;

            var lista = await _turnoRepository.Buscar(x => x.IDEMPRESA == idEmpresa && (x.DTTURNO.Value >= dtIni && x.DTTURNO <= dtFim));

            return new PagedResult<Turno>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList().OrderBy(x => x.DTTURNO),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<Turno> ObterCompletoPorId(long id)
        {
            return _turnoRepository.Obter(x => x.Id == id,"Empresa", "UsuarioInicial", "UsuarioFinal").Result.FirstOrDefault();
        }


        public async Task<Turno> ObterTurnoAbertoCompletoPorId(long id)
        {
            return _turnoRepository.Obter(x => x.IDEMPRESA == id && x.DTHRFIM == null, "Empresa", "UsuarioInicial", "UsuarioFinal").Result.FirstOrDefault();
        }


        public async Task<List<Turno>> ObterTodos(long idEmpresa)
        {
            return _turnoRepository.Obter(x => x.IDEMPRESA == idEmpresa).Result.ToList();
        }


        #endregion

        #region Turno Preco
        public async Task Adicionar(TurnoPreco turnoPreco)
        {
            if (!ExecutarValidacao(new TurnoValidation(), turnoPreco))
                return;

            await _turnoPrecoRepository.AdicionarSemSalvar(turnoPreco);
        }

        public async Task<TurnoPreco> ObteClientePrecoPorId(long id)
        {
            return _turnoPrecoRepository.ObterPorId(id).Result;
        }

        public async Task<IEnumerable<TurnoPreco>> ObterTurnoPrecoPorProduto(long idProduto)
        {
            return _turnoPrecoRepository.Obter(x => x.IDPRODUTO == idProduto).Result;
        }

        public async Task Remover(long id)
        {
            if (!PodeApagarTurnoPreco(id).Result)
            {
                Notificar("Não foi possível apagar o Preço do produto para o turno, pois este possui historico!");
                return;
            }
            await _turnoPrecoRepository.RemoverSemSalvar(id);
        }
        #endregion

        #region Dapper
        public async Task<bool> AbrirTurno(long idEmpresa, long idUsuario)
        {
            try
            {
                await _dapperRepository.BeginTransaction();

                var idTuno = _utilDapperRepository.GerarUUID().Result;

                if (!_turnoDapperRepository.TurnoAbertoPorId(idEmpresa).Result)
                {

                    var turno = new Turno(idEmpresa, idUsuario, null, DateTime.Now, _turnoDapperRepository.GerarNumeroTurnoPorIdEmpresa(idEmpresa).Result, DateTime.Now, null, null);
                    await _turnoDapperRepository. IncluirTurno(idTuno, turno);
                }

                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                    await _dapperRepository.Rollback();
            }
            catch (Exception)
            {
                await _dapperRepository.Rollback();
                throw;
            }

            return !TemNotificacao();
            
        }

        public async Task<bool> TurnoAbertoPorId(long id)
        {
            return await _turnoDapperRepository.TurnoAbertoPorId(id);
        }

        public async Task<bool> FecharTurno(long idEmpresa, long idUsuario, string obs)
        {
            try
            {
                await _dapperRepository.BeginTransaction();

                var turno = _turnoDapperRepository.ObterObjetoTurnoAbertoPorIdEmpresa(idEmpresa).Result;
                if (turno != null)
                {
                    turno.AdicionarObservacao(obs);
                    await _turnoDapperRepository.FecharTurno(turno, idUsuario);
                }
                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                    await _dapperRepository.Rollback();
            }
            catch (Exception ex)
            {
                await _dapperRepository.Rollback();
                throw;
            }

            return !TemNotificacao();
        }

        #endregion

        #region Metodos Privados
        private async Task<bool> PodeApagarTurnoPreco(long idTurnoPreco) => true;

        public Task<Turno> ObterObjetoTurnoAbertoPorIdEmpresa(long idEmpresa)
        {
            throw new NotImplementedException();
        }




        #endregion
    }
}

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

namespace agilium.api.business.Services
{
    public class TurnoService : BaseService, ITurnoService
    {
        private readonly ITurnoPrecoRepository _turnoPrecoRepository;
        private readonly ITurnoRepository _turnoRepository;
     
        public TurnoService(INotificador notificador, ITurnoPrecoRepository turnoPrecoRepository, ITurnoRepository turnoRepository) : base(notificador)
        {
            _turnoPrecoRepository = turnoPrecoRepository;
            _turnoRepository = turnoRepository;
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

        #region Metodos Privados
        private async Task<bool> PodeApagarTurnoPreco(long idTurnoPreco) => true;



        #endregion
    }
}

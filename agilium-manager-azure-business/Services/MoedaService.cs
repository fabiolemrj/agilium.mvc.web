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
    public class MoedaService : BaseService, IMoedaService
    {
        private readonly IMoedaRepository _moedaRepository;
        public MoedaService(INotificador notificador, IMoedaRepository moedaRepository) : base(notificador)
        {
            _moedaRepository = moedaRepository;
        }

        public async Task Adicionar(Moeda moeda)
        {
            if (!ExecutarValidacao(new MoedaValidation(), moeda))
                return;

            await _moedaRepository.AdicionarSemSalvar(moeda);
        }

        public async Task Apagar(long id)
        {
            if (!PodeApagar(id).Result)
            {
                Notificar("Não foi possível apagar a moeda, pois este possui historico!");
                return;
            }
            await _moedaRepository.RemoverSemSalvar(id);
        }

        public async Task Atualizar(Moeda moeda)
        {
            if (!ExecutarValidacao(new MoedaValidation(), moeda))
                return;

            await _moedaRepository.AtualizarSemSalvar(moeda);
        }

        public void Dispose()
        {
            _moedaRepository?.Dispose();
        }

        public async Task<Moeda> ObterCompletoPorId(long id)
        {
            var lista = _moedaRepository.Obter(x => x.Id == id).Result;

            return lista.FirstOrDefault();
        }

        public async Task<List<Moeda>> ObterPorDescricao(string descricao)
        {
            return _moedaRepository.Buscar(x => x.DSMOEDA.ToUpper().Contains(descricao.ToUpper())).Result.ToList();
        }

        public async Task<Moeda> ObterPorId(long id)
        {
            return _moedaRepository.ObterPorId(id).Result;
        }

        public async Task<PagedResult<Moeda>> ObterPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(nome) ? string.Empty : nome;

            var lista = await _moedaRepository.Obter(x => x.IDEMPRESA == idEmpresa && x.DSMOEDA.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<Moeda>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            }; ;
        }

        public async Task<List<Moeda>> ObterTodas()
        {
            return await _moedaRepository.ObterTodos();
        }

        public async Task Salvar()
        {
            await _moedaRepository.SaveChanges();
        }

        #region Metodos Privados
        private async Task<bool> PodeApagar(long id) => true;

        #endregion
    }
}

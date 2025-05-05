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
    public class PontoVendaService : BaseService, IPontoVendaService
    {
        private readonly IPontoVendaRepository _pontoVendaRepository;
        public PontoVendaService(INotificador notificador, IPontoVendaRepository pontoVendaRepository) : base(notificador)
        {
            _pontoVendaRepository = pontoVendaRepository;
        }

        public async Task Adicionar(PontoVenda pontoVenda)
        {
            if (!ExecutarValidacao(new PontoVendaValidation(), pontoVenda))
                return;

            await _pontoVendaRepository.AdicionarSemSalvar(pontoVenda);
        }

        public async Task Apagar(long id)
        {
            if (!PodeApagar(id).Result)
            {
                Notificar("Não foi possível apagar o ponto de venda, pois este possui historico!");
                return;
            }
            await _pontoVendaRepository.RemoverSemSalvar(id);
        }

        public async Task Atualizar(PontoVenda pontoVenda)
        {
            if (!ExecutarValidacao(new PontoVendaValidation(), pontoVenda))
                return;

            await _pontoVendaRepository.AtualizarSemSalvar(pontoVenda);
        }

        public void Dispose()
        {
           _pontoVendaRepository?.Dispose();
        }

        public async Task<PontoVenda> ObterCompletoPorId(long id)
        {
            var lista = _pontoVendaRepository.Obter(x => x.Id == id).Result;

            return lista.FirstOrDefault();
        }

        public async Task<List<PontoVenda>> ObterPorDescricao(string descricao)
        {
            return _pontoVendaRepository.Buscar(x => x.DSPDV.ToUpper().Contains(descricao.ToUpper())).Result.ToList();
        }

        public async Task<PontoVenda> ObterPorId(long id)
        {
            return _pontoVendaRepository.ObterPorId(id).Result;
        }

        public async Task<PagedResult<PontoVenda>> ObterPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(nome) ? string.Empty : nome;

            var lista = await _pontoVendaRepository.Obter(x => x.IDEMPRESA == idEmpresa && x.DSPDV.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<PontoVenda>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            }; ;
        }

        public async Task<List<PontoVenda>> ObterTodas()
        {
            return await _pontoVendaRepository.ObterTodos();
        }

        public async Task Salvar()
        {
            await _pontoVendaRepository.SaveChanges();
        }


        #region Metodos Privados
        private async Task<bool> PodeApagar(long id) => true;

        #endregion
    }
}

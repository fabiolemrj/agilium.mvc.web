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
    public class PerdaService : BaseService, IPerdaService
    {
        private readonly IPerdaRepository _perdaRepository;
        public PerdaService(INotificador notificador, IPerdaRepository perdaRepository) : base(notificador)
        {
            _perdaRepository = perdaRepository;
        }


        public async Task Salvar()
        {
            await _perdaRepository.SaveChanges();
        }

        public void Dispose()
        {
            _perdaRepository?.Dispose();
        }

        public async Task Adicionar(Perda perda)
        {
            if (!ExecutarValidacao(new PerdaValidation(), perda))
                return;

            await _perdaRepository.AdicionarSemSalvar(perda);
        }

        public async Task Apagar(long id)
        {
            if (await PerdaNaoPodeSerExcluida(id))
            {
                Notificar("Não é possivel apagar esta perda pois o mesmo está sendo utilizado");
                return;
            }
            await _perdaRepository.RemoverSemSalvar(id);
        }

        public async Task Atualizar(Perda perda)
        {
            if (!ExecutarValidacao(new PerdaValidation(), perda))
                return;

            await _perdaRepository.AtualizarSemSalvar(perda);
        }


        public async Task<Perda> ObterPorId(long id)
        {
            return await _perdaRepository.ObterPorId(id);
        }

        public async Task<IEnumerable<Perda>> ObterTodas(long idEmpresa)
        {
            return await _perdaRepository.Obter(x=>x.IDEMPRESA == idEmpresa);
        }

        public async Task<PagedResult<Perda>> ObterValePorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(nome) ? string.Empty : nome;

            var lista = await _perdaRepository.Obter(x => x.IDEMPRESA == idEmpresa && x.Produto.NMPRODUTO.ToUpper().Contains(_nomeParametro.ToUpper()), "Produto","Estoque","Usuario");

            return new PagedResult<Perda>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        #region private
        private async Task<bool> PerdaNaoPodeSerExcluida(long id) => false;
        #endregion
    }
}

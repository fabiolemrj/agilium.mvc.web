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
    public class NotaFiscalService : BaseService, INotaFiscalService
    {
        private readonly INotaFiscalInutilRepository _notaFiscalInutilRepository;

        public NotaFiscalService(INotificador notificador, INotaFiscalInutilRepository notaFiscalInutilRepository) : base(notificador)
        {
            _notaFiscalInutilRepository = notaFiscalInutilRepository;
        }

        public void Dispose()
        {
            _notaFiscalInutilRepository?.Dispose();
        }

        public async Task Salvar()
        {
            await _notaFiscalInutilRepository.SaveChanges();
        }


        #region Nota Fiscal Inutil
        public async Task Adicionar(NotaFiscalInutil nf)
        {
            if (!ExecutarValidacao(new NotaFiscalInutilaValidation(), nf))
                return;

            await _notaFiscalInutilRepository.AdicionarSemSalvar(nf);
        }

        public async Task ApagarNFInutil(long id)
        {
            if (await NFInutilUtilizado(id))
            {
                Notificar("Não é possivel apagar este NF inutil pois o mesmo está sendo utilizado");
                return;
            }
            await _notaFiscalInutilRepository.RemoverSemSalvar(id);
        }

        public async Task Atualizar(NotaFiscalInutil nf)
        {
            if (!ExecutarValidacao(new NotaFiscalInutilaValidation(), nf))
                return;

            await _notaFiscalInutilRepository.AtualizarSemSalvar(nf);
        }

        public async Task<NotaFiscalInutil> ObterNFInutilCompletoPorId(long id)
        {
            var lista = _notaFiscalInutilRepository.Obter(x => x.Id == id).Result;

            return lista.FirstOrDefault();
        }

        public async Task<NotaFiscalInutil> ObterNFInutilPorId(long id)
        {
            return _notaFiscalInutilRepository.ObterPorId(id).Result;
        }

        public async Task<PagedResult<NotaFiscalInutil>> ObterNFInutilPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(nome) ? string.Empty : nome;

            var lista = await _notaFiscalInutilRepository.Buscar(x => x.IDEMPRESA == idEmpresa && x.DSMOTIVO.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<NotaFiscalInutil>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList().OrderBy(x => x.DTHRINUTIL),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<NotaFiscalInutil>> ObterTodasNFInutil(long idEmpresa)
        {
            return await _notaFiscalInutilRepository.Obter( x=> x.IDEMPRESA == idEmpresa);
        }
        #endregion

        #region Private
        private async Task<bool> NFInutilUtilizado(long id) => false;
        #endregion
    }
}
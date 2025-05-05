using agilium.api.business.Enums;
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
    public class SiteMercadoService : BaseService, ISiteMercadoService
    {
        private readonly IProdutoSiteMercadoRepository _produtoSiteMercadoRepository;
        private readonly IMoedaSiteMercadoRepository _moedaSiteMercadoRepository;
        public SiteMercadoService(INotificador notificador, IProdutoSiteMercadoRepository produtoSiteMercado, 
            IMoedaSiteMercadoRepository moedaSiteMercadoRepository) : base(notificador)
        {
            _produtoSiteMercadoRepository = produtoSiteMercado;
            _moedaSiteMercadoRepository = moedaSiteMercadoRepository;
        }

        public void Dispose()
        {
            _produtoSiteMercadoRepository?.Dispose();
            _moedaSiteMercadoRepository?.Dispose();
        }
        public async Task Salvar()
        {
            await _produtoSiteMercadoRepository.SaveChanges() ;
        }

        #region ProdutoSM
        public async Task Adicionar(ProdutoSiteMercado produto)
        {
            if (!ExecutarValidacao(new ProdutoSiteMercadoValidation(), produto))
                return;

            await _produtoSiteMercadoRepository.AdicionarSemSalvar(produto);
        }
        public async Task Apagar(long id)
        {
            if (!PodeApagarProduto(id).Result)
            {
                Notificar("Não é possivel apagar este produto pois o mesmo está sendo utilizado");
                return;
            }
            await _produtoSiteMercadoRepository.RemoverSemSalvar(id);
        }
        public async Task Atualizar(ProdutoSiteMercado produto)
        {
            if (!ExecutarValidacao(new ProdutoSiteMercadoValidation(), produto))
                return;

            await _produtoSiteMercadoRepository.AtualizarSemSalvar(produto);
        }
        public async Task<PagedResult<ProdutoSiteMercado>> ObterPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(nome) ? string.Empty : nome;

            var lista = await _produtoSiteMercadoRepository.Buscar(x => x.IDEMPRESA == idEmpresa && x.DSPROD.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<ProdutoSiteMercado>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList().OrderBy(x => x.DSPROD),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            }; 
        }

        public async Task<ProdutoSiteMercado> ObterPorId(long id)
        {
            return await _produtoSiteMercadoRepository.ObterPorId(id);
        }
        public async Task<IEnumerable<ProdutoSiteMercado>> ObterTodas(long idEmpresa)
        {
            return await _produtoSiteMercadoRepository.Obter(x=>x.IDEMPRESA == idEmpresa);
        }

        #endregion

        #region MoedaSM
        public async Task Adicionar(MoedaSiteMercado moeda)
        {
            if (!ExecutarValidacao(new MoedaSiteMercadoValidation(), moeda))
                return;

            await _moedaSiteMercadoRepository.AdicionarSemSalvar(moeda);
        }

        public async Task ApagarMoeda(long id)
        {
            if (!PodeApagarMoeda(id).Result)
            {
                Notificar("Não é possivel apagar esta associacao pois a mesma está sendo utilizada");
                return;
            }
            await _moedaSiteMercadoRepository.RemoverSemSalvar(id);
        }

        public async Task Atualizar(MoedaSiteMercado moeda)
        {
            if (!ExecutarValidacao(new MoedaSiteMercadoValidation(), moeda))
                return;

            await _moedaSiteMercadoRepository.AtualizarSemSalvar(moeda);
        }

        public async Task<MoedaSiteMercado> ObterMoedaPorId(long id)
        {
            return await _moedaSiteMercadoRepository.ObterPorId(id);
        }

        public async Task<PagedResult<MoedaSiteMercado>> ObterMoedaPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(nome) ? string.Empty : nome;

            var lista = await _moedaSiteMercadoRepository.Buscar(x => x.IDEMPRESA == idEmpresa && x.Moeda.DSMOEDA.ToUpper().Contains(_nomeParametro.ToUpper()),"Moeda");

            return new PagedResult<MoedaSiteMercado>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList().OrderBy(x => x.Moeda.DSMOEDA),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<bool> MoedaSMJaAssociada(ETipoMoedaSiteMercado idMoedaSm, long idEmpresa, long id)
        {
            return _moedaSiteMercadoRepository.Obter(x=> (x.IDSM == idMoedaSm && x.Id != id) && x.IDEMPRESA == idEmpresa).Result.Any();
        }

        #endregion

        #region private
        private async Task<bool> PodeApagarProduto(long id) => true;
        private async Task<bool> PodeApagarMoeda(long id) => true;

        #endregion
    }
}

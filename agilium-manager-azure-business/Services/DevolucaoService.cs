using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Services
{
    public class DevolucaoService : BaseService, IDevolucaoService
    {
        private readonly IMotivoDevolucaoRepository _motivoDevolucaoRepository;
        private readonly IDevolucaoRepository _devolucaoRepository;
        private readonly IDevolucaoItemRepository _devolucaoItemRepository;
        public DevolucaoService(INotificador notificador, IMotivoDevolucaoRepository motivoDevolucaoRepository, IDevolucaoRepository devolucaoRepository, 
            IDevolucaoItemRepository devolucaoItemRepository) : base(notificador)
        {
            _motivoDevolucaoRepository = motivoDevolucaoRepository;
            _devolucaoRepository = devolucaoRepository;
            _devolucaoItemRepository = devolucaoItemRepository;
        }

        public async Task Salvar()
        {
            await _devolucaoRepository.SaveChanges();
        }

        public void Dispose()
        {
            _motivoDevolucaoRepository?.Dispose();
            _devolucaoItemRepository?.Dispose();
            _devolucaoRepository?.Dispose();
        }

        #region MotivoDevolucao
        public async Task<bool> Adicionar(MotivoDevolucao motivoDevolucao)
        {
            if (!ExecutarValidacao(new MotivoDevolucaoValidation(), motivoDevolucao))
                return false;

            await _motivoDevolucaoRepository.AdicionarSemSalvar(motivoDevolucao);
            return true;
        }

        public async Task<bool> ApagarMotivo(long id)
        {
            if (await ExisteMotivoUtilizado(id))
            {
                Notificar("Não é possivel apagar esta marca pois o mesmo está sendo utilizado");
                return false;
            }
            await _motivoDevolucaoRepository.RemoverSemSalvar(id);
            return true;
        }

        public async Task<bool> Atualizar(MotivoDevolucao motivoDevolucao)
        {
            if (!ExecutarValidacao(new MotivoDevolucaoValidation(), motivoDevolucao))
                return false;

            await _motivoDevolucaoRepository.AtualizarSemSalvar(motivoDevolucao);
            return true;
        }


        public async Task<bool> Existe(MotivoDevolucao motivoDevolucao)
        {
            Expression<Func<MotivoDevolucao, bool>> expression = x => x.IDEMPRESA == motivoDevolucao.IDEMPRESA
                                                 && x.DSMOTDEV.Trim().ToUpper() == motivoDevolucao.DSMOTDEV.Trim().ToUpper();

            var result = _motivoDevolucaoRepository.Obter(expression).Result.Any();
            return result;
        }

        public async Task<PagedResult<MotivoDevolucao>> ObterMotivoPaginacaoPorDescricao(long idempresa, string descricao, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(descricao) ? string.Empty : descricao;

            var lista = await _motivoDevolucaoRepository.Buscar(x => x.IDEMPRESA == idempresa && x.DSMOTDEV.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<MotivoDevolucao>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList().OrderBy(x => x.DSMOTDEV),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<MotivoDevolucao> ObterPorIdMotivo(long id)
        {
            return await _motivoDevolucaoRepository.ObterPorId(id);
        }

        public async Task<IEnumerable<MotivoDevolucao>> ObterTodosMotivos()
        {
            return await _motivoDevolucaoRepository.ObterTodos();
        }

        public async Task<IEnumerable<MotivoDevolucao>> ObterTodosProdutoMotivos(params string[] includes)
        {
            return await _motivoDevolucaoRepository.ObterTodos();
        }
        #endregion

        #region Devolucao

        public async Task Adicionar(Devolucao devolucao)
        {
            if (!ExecutarValidacao(new DevolucaoValidation(), devolucao))
                return;

            await _devolucaoRepository.AdicionarSemSalvar(devolucao);
        }

        public async Task Atualizar(Devolucao devolucao)
        {
            if (!ExecutarValidacao(new DevolucaoValidation(), devolucao))
                return;

            await _devolucaoRepository.AtualizarSemSalvar(devolucao);
        }

        public async Task Apagar(long id)
        {
            if (await PodeApagarDevolucao(id))
            {
                Notificar("Não é possivel apagar esta devolucao pois o mesmo está sendo utilizado");
                return;
            }
            await _devolucaoRepository.RemoverSemSalvar(id);
        }

        public async Task<Devolucao> ObterDevolucaoPorId(long id)
        {
            return _devolucaoRepository.ObterDevolucaoPorId(id).Result;
        }

        public async Task<PagedResult<Devolucao>> ObterDevolucaoPorPaginacao(long idEmpresa, DateTime dtIni, DateTime dtFim, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;

            //var lista = _devolucaoRepository.Obter(x => x.DTHRDEV.Value >= dtIni && x.DTHRDEV<= dtFim, "MotivoDevolucao","Venda").Result;
            var lista = _devolucaoRepository.ObterDevolucaoPorPaginacao(idEmpresa, dtIni, dtFim, pagina, pageSize).Result;
            return new PagedResult<Devolucao>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<Devolucao>> ObterTodasDevolucoes(long idEmpresa)
        {
            return await _devolucaoRepository.Obter(x=>x.IDEMPRESA == idEmpresa);
        }


        public async Task<Devolucao> ObterPorId(long id)
        {
            return _devolucaoRepository.ObterPorId(id).Result;
        }

        #endregion

        #region Devolucao Item
        public async Task<IEnumerable<DevolucaoItem>> ObterDevolucaoItens(long idDevolucao)
        {
            return await _devolucaoItemRepository.Obter(x => x.IDDEV == idDevolucao, "Devolucao");
        }

        public async Task<DevolucaoItem> ObterDevolucaoItemPorId(long id)
        {
            return await _devolucaoItemRepository.ObterPorId(id);
        }

        public async Task Adicionar(DevolucaoItem devolucaoItem)
        {
            if (!ExecutarValidacao(new DevolucaoItemValidation(), devolucaoItem))
                return;

            await _devolucaoItemRepository.AdicionarSemSalvar(devolucaoItem);
        }

        public async Task AdicionarAtualizar(DevolucaoItem devolucaoItem)
        {
            if (_devolucaoItemRepository.Existe(x=>x.Id == devolucaoItem.Id).Result)
                await _devolucaoItemRepository.AtualizarSemSalvar(devolucaoItem);
            else
                await _devolucaoItemRepository.AdicionarSemSalvar(devolucaoItem);            
        }

        #endregion

        #region private
        private async Task<bool> ExisteMotivoUtilizado(long idDevolucao)
        {
            var resultado = false;
            //resultado = _produtoRepository.Obter(x => x.Id == idContato).Result.Any();

            return resultado;
        }

        private async Task<bool> ExisteProdutoMarcaUtilizado(long idContato)
        {
            var resultado = false;
            //resultado = _produtoRepository.Obter(x => x.Id == idContato).Result.Any();

            return resultado;
        }

        private async Task<bool> PodeApagarDevolucao(long idDevolucao) => true;


        #endregion
    }
}

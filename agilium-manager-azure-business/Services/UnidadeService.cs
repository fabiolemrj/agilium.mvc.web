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
    public class UnidadeService : BaseService, IUnidadeService
    {
        private readonly IUnidadeRepository _unidadeRepository;

        public UnidadeService(INotificador notificador, IUnidadeRepository unidadeRepository) : base(notificador)
        {
            _unidadeRepository = unidadeRepository;
        }

        public async Task Adicionar(Unidade unidade)
        {
            if (!ExecutarValidacao(new UnidadeValidation(), unidade))
                return;

            await _unidadeRepository.AdicionarSemSalvar(unidade); 
        }

        public async Task Apagar(long id)
        {
            await _unidadeRepository.RemoverSemSalvar(id); 
        }

        public async Task Atualizar(Unidade unidade)
        {
            if (!ExecutarValidacao(new UnidadeValidation(), unidade))
                return;

            await _unidadeRepository.AtualizarSemSalvar(unidade);
        }

        public void Dispose()
        {
            _unidadeRepository?.Dispose();
        }

        public async Task<bool> MudarSituacao(long id)
        {
            var objeto = await ObterPorId(id);
            if (objeto == null)
                return false;
            if (objeto.Ativo == Enums.EAtivo.Ativo) 
                objeto.Desativar();
            else
                objeto.Ativar();
            
            await Atualizar(objeto);

            await _unidadeRepository.SaveChanges();
            
            return true;
        }

        public async Task<PagedResult<Unidade>> ObterPorDescricaoPaginacao(string descricao, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(descricao) ? string.Empty : descricao;

            var lista = await _unidadeRepository.Buscar(x => x.Descricao.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<Unidade>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<Unidade> ObterPorId(long id)
        {
            return _unidadeRepository.ObterPorId(id).Result;
        }

        public async Task<List<Unidade>> ObterTodas()
        {
            return await _unidadeRepository.ObterTodos();
        }

        public async  Task Salvar()
        {
            await _unidadeRepository.SaveChanges(); ;
        }
    }
}

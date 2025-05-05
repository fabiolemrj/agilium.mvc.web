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
    public class CategoriaFinanceiraService : BaseService, ICategoriaFinanceiraService
    {
        private readonly ICategoriaFinanceiroRepository _categoriaFinanceiroRepository;
        public CategoriaFinanceiraService(INotificador notificador, 
                                ICategoriaFinanceiroRepository categoriaFinanceiroRepository) : base(notificador)
        {
            _categoriaFinanceiroRepository = categoriaFinanceiroRepository;
        }

        public async Task<bool> Adicionar(CategoriaFinanceira categoriaFinanceira)
        {
            if (!ExecutarValidacao(new CategoriaFinanceiraValidation(), categoriaFinanceira)) return false;

            await _categoriaFinanceiroRepository.Adicionar(categoriaFinanceira);
            return true;
        }

        public async Task<bool> Atualizar(CategoriaFinanceira categoriaFinanceira)
        {
            if (!ExecutarValidacao(new CategoriaFinanceiraValidation(), categoriaFinanceira)) return false;

            await _categoriaFinanceiroRepository.Atualizar(categoriaFinanceira);
            return true;
        }

        public void Dispose()
        {
            _categoriaFinanceiroRepository?.Dispose();
        }

        public async Task<bool> Existe(CategoriaFinanceira categoriaFinanceira)
        {
             Expression<Func< CategoriaFinanceira, bool>> expression = x => x.NMCATEG.Trim().ToUpper() == categoriaFinanceira.NMCATEG.Trim().ToUpper()
                                                    && x.STCATEG == categoriaFinanceira.STCATEG;

            return _categoriaFinanceiroRepository.Obter(expression).Result.Any();

        }

        public async Task<PagedResult<CategoriaFinanceira>> ObterPorDescricaoPaginacao(string descricao, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(descricao) ? string.Empty : descricao;

            var lista = await _categoriaFinanceiroRepository.Buscar(x => x.NMCATEG.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<CategoriaFinanceira>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<CategoriaFinanceira> ObterPorId(long id)
        {
            return await _categoriaFinanceiroRepository.ObterPorId(id); 
        }

        public async Task<IEnumerable<CategoriaFinanceira>> ObterTodos()
        {
            return await _categoriaFinanceiroRepository.ObterTodos();
        }
               

        public async Task Remover(long id)
        {
            await _categoriaFinanceiroRepository.RemoverSemSalvar(id);        
        }

        public async Task Salvar()
        {
            await _categoriaFinanceiroRepository.SaveChanges();
        }
    }
}

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
    public class EmpresaService : BaseService, IEmpresaService
    {
        private readonly IEmpresaRepository _empresaRepository;
        public EmpresaService(INotificador notificador, IEmpresaRepository empresaRepository) : base(notificador)
        {
            _empresaRepository = empresaRepository;
        }

        public async Task Adicionar(Empresa empresa)
        {
            if (!ExecutarValidacao(new EmpresaValidation(), empresa))
                return;

            await _empresaRepository.AdicionarSemSalvar(empresa);
        }

        public async Task Apagar(long id)
        {
            await _empresaRepository.RemoverSemSalvar(id);
        }

        public async Task Atualizar(Empresa empresa)
        {
            if (!ExecutarValidacao(new EmpresaValidation(), empresa))
                return;

            await _empresaRepository.AtualizarSemSalvar(empresa);
        }

        public void Dispose()
        {
            _empresaRepository?.Dispose();
        }

        public async Task<Empresa> ObterCompletoPorId(long id)
        {
            var lista = _empresaRepository.Obter(x=> x.Id == id, "Endereco", "ContatoEmpresas", "ContatoEmpresas.Contato").Result;
            return lista.FirstOrDefault();
        }

        public async Task<List<Empresa>> ObterPorDescricao(string descricao)
        {
            return _empresaRepository.Buscar(x => x.NMRZSOCIAL.ToUpper().Contains(descricao.ToUpper())).Result.ToList();
        }

        public async Task<PagedResult<Empresa>> ObterPorDescricaoPaginacao(string descricao, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(descricao) ? string.Empty : descricao;

            var lista = await _empresaRepository.Buscar(x => x.NMRZSOCIAL.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<Empresa>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<Empresa> ObterPorId(long id)
        {
            return _empresaRepository.ObterPorId(id).Result;
        }

        public async Task<List<Empresa>> ObterTodas()
        {
            return _empresaRepository.ObterTodos().Result; 
        }

        public async Task Salvar()
        {
            await _empresaRepository.SaveChanges();
        }
    }
}

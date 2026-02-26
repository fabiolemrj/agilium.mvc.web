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
        private readonly IUtilDapperRepository _utilDapperRepository;
        private readonly IEmpresaDapperRepository _empresaDapperRepository;
        private readonly IEnderecoDapperRepository _enderecoDapperRepository;
        public EmpresaService(INotificador notificador, IEmpresaRepository empresaRepository,IUtilDapperRepository utilDapperRepository,
            IEmpresaDapperRepository empresaDapperRepository, IEnderecoDapperRepository enderecoDapperRepository) : base(notificador)
        {
            _empresaRepository = empresaRepository;
            _utilDapperRepository = utilDapperRepository;
            _empresaDapperRepository = empresaDapperRepository;
            _enderecoDapperRepository = enderecoDapperRepository;
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

        public async Task Atualizar(Empresa empresaDb, object model)
        {
            // aplica apenas valores escalares
            await _empresaRepository.AtualizarComSetValues(empresaDb, model);

        }

        public void Dispose()
        {
            _empresaRepository?.Dispose();
        }

        public async Task<string> GerarCodigo()
        {
            return await _utilDapperRepository.GerarCodigo("SELECT MAX(CAST(CDEMPRESA AS UNSIGNED)) AS CD FROM empresa");
        }

        public async Task<Empresa> ObterCompletoPorId(long id)
        {
            var lista = await _empresaRepository.Obter(x => x.Id == id, "Endereco", "ContatoEmpresas", "ContatoEmpresas.Contato");
            return lista.FirstOrDefault();
        }

        public async Task<List<Empresa>> ObterPorDescricao(string descricao)
        {
            var resultado = await _empresaRepository.Buscar(x => x.NMRZSOCIAL.ToUpper().Contains(descricao.ToUpper()));
            return resultado.ToList();
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
            return await _empresaRepository.ObterPorId(id);
        }

        public async Task<Empresa> ObterPorIdCompleto(long id)
        {
            var lista = await _empresaRepository.Obter(
                e => e.Id == id,
                "Endereco",
                "ContatoEmpresas",
                "ContatoEmpresas.Contato"
            );

            return lista.FirstOrDefault();
        }

        public async Task<Empresa> ObterPorIdCompletoTracking(long id)
        {
            return await _empresaRepository.ObterCompletoTracking(id);
        }


        public async Task<List<Empresa>> ObterTodas()
        {
            return await _empresaRepository.ObterTodos();
        }

        public async Task Salvar()
        {
            await _empresaRepository.SaveChanges();
        }

        public async Task<bool> EditarEmpresa(Empresa empresa)
        {
            if (empresa == null)
                return false;

            // Chama o método Dapper que atualiza os campos alterados
            var atualizado = await _empresaDapperRepository.EditarEmpresa(empresa);
            if (atualizado)
            {
                await _enderecoDapperRepository.SalvarEndereco(empresa.Endereco);
            }
            return atualizado;
        }

    }
}

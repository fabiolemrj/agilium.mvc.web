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
    public class FuncionarioService : BaseService, IFuncionarioService
    {
        private readonly IFuncionarioRepository _funcionarioRepository;

        public FuncionarioService(INotificador notificador, IFuncionarioRepository funcionarioRepository) : base(notificador)
        {
            _funcionarioRepository = funcionarioRepository;
        }

        #region endpoint

        public async Task Adicionar(Funcionario funcionario)
        {
            if (!ExecutarValidacao(new FuncionarioValidation(), funcionario))
                return;

            await _funcionarioRepository.AdicionarSemSalvar(funcionario);
        }

        public async Task Apagar(long id)
        {
            if (!PodeApagarFuncionario(id).Result)
            {
                Notificar("Não foi possível apagar o funcionario, pois este possui historico!");
                return;
            }
            await _funcionarioRepository.RemoverSemSalvar(id);
        }

        public async Task Atualizar(Funcionario funcionario)
        {
            if (!ExecutarValidacao(new FuncionarioValidation(), funcionario))
                return;

            await _funcionarioRepository.AtualizarSemSalvar(funcionario);
        }

        public void Dispose()
        {
            _funcionarioRepository?.Dispose();
        }

        public async Task<Funcionario> ObterCompletoPorId(long id)
        {
            var lista = _funcionarioRepository.Obter(x => x.Id == id, "Endereco", "Usuario").Result;

            return lista.FirstOrDefault();
        }

        public async Task<Funcionario> ObterPorId(long id)
        {
            return _funcionarioRepository.ObterPorId(id).Result;
        }

        public async Task<List<Funcionario>> ObterPorNome(string descricao)
        {
            return _funcionarioRepository.Buscar(x => x.NMFUNC.ToUpper().Contains(descricao.ToUpper())).Result.ToList();
        }

        public async Task<PagedResult<Funcionario>> ObterPorNomePaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(nome) ? string.Empty : nome;

            var lista = await _funcionarioRepository.Obter(x => x.IDEMPRESA == idEmpresa && x.NMFUNC.ToUpper().Contains(_nomeParametro.ToUpper()), "Usuario");

            return new PagedResult<Funcionario>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            }; ;
        }

        public async Task<List<Funcionario>> ObterTodas()
        {
            return _funcionarioRepository.ObterTodos().Result;
        }

        public async Task Salvar()
        {
            await _funcionarioRepository.SaveChanges();
        }

        #endregion

        #region Metodos Privados
        private async Task<bool> PodeApagarFuncionario(long idFunc) => true;

        #endregion
    }
}

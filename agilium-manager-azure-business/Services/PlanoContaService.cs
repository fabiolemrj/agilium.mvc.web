using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Services
{
    public class PlanoContaService : BaseService, IPlanoContaService
    {
        private readonly IPlanoContaRepository _planoContaRepository;
        private readonly IPlanoContaLancamentoRepository _planoContaLancamentoRepository;
        private readonly IPlanoContaSaldoRepository _planoContaSaldoRepository;
        public PlanoContaService(INotificador notificador, IPlanoContaRepository planoContaRepository, IPlanoContaSaldoRepository planoContaSaldoRepository, 
                                IPlanoContaLancamentoRepository lancamentoRepository) : base(notificador)
        {
            _planoContaRepository = planoContaRepository;
            _planoContaSaldoRepository = planoContaSaldoRepository;
            _planoContaLancamentoRepository = lancamentoRepository;
        }

        public void Dispose()
        {
            _planoContaLancamentoRepository?.Dispose();
            _planoContaSaldoRepository?.Dispose();
            _planoContaRepository?.Dispose();            
        }

        public async Task Salvar()
        {
            await _planoContaRepository.SaveChanges();
        }

        #region Plano Conta

        public async Task Adicionar(PlanoConta planoConta)
        {
            if (!ExecutarValidacao(new PlanoContaValidation(), planoConta))
                return;

            await _planoContaRepository.AdicionarSemSalvar(planoConta);
        }

        public async Task Apagar(long id)
        {
            if (await PlanoContaUtilizado(id))
            {
                Notificar("Não é possivel apagar este plano de conta pois o mesmo está sendo utilizado");
                return;
            }
            await _planoContaRepository.RemoverSemSalvar(id);
        }

        public async Task Atualizar(PlanoConta planoConta)
        {
            if (!ExecutarValidacao(new PlanoContaValidation(), planoConta))
                return;

            await _planoContaRepository.AtualizarSemSalvar(planoConta);
        }
           

        public async Task<PlanoConta> ObterCompletoPorId(long id)
        {
            var lista = _planoContaRepository.Obter(x => x.Id == id).Result;

            return lista.FirstOrDefault();
        }

        public async Task<List<PlanoConta>> ObterPorDescricao(string descricao)
        {
            return _planoContaRepository.Buscar(x => x.DSCONTA.ToUpper().Contains(descricao.ToUpper())).Result.ToList();
        }

        public async Task<PlanoConta> ObterPorId(long id)
        {
            return await _planoContaRepository.ObterPorId(id);
        }

        public async Task<PagedResult<PlanoConta>> ObterPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(nome) ? string.Empty : nome;

            var lista = await _planoContaRepository.Buscar(x => x.IDEMPRESA == idEmpresa && x.DSCONTA.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<PlanoConta>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList().OrderBy(x => x.CDCONTA),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<List<PlanoConta>> ObterTodas(long idEmpresa)
        {
            return _planoContaRepository.Obter(x=>x.IDEMPRESA == idEmpresa).Result.ToList();
        }
        #endregion

        #region Plano Conta Saldo

        public async Task Adicionar(PlanoContaSaldo planoContaSaldo)
        {
            if (!ExecutarValidacao(new PlanoContaSaldoValidation(), planoContaSaldo))
                return;

            await _planoContaSaldoRepository.AdicionarSemSalvar(planoContaSaldo);
        }

        public async Task Atualizar(PlanoContaSaldo planoContaSaldo)
        {
            if (!ExecutarValidacao(new PlanoContaSaldoValidation(), planoContaSaldo))
                return;

            await _planoContaSaldoRepository.AtualizarSemSalvar(planoContaSaldo);
        }

        public async Task ApagarSaldo(long id)
        {
            await _planoContaSaldoRepository.RemoverSemSalvar(id);
        }

        public async Task<List<PlanoContaSaldo>> ObterSaldoPorPlano(long idPlano)
        {
            return _planoContaSaldoRepository.Buscar(x => x.IDCONTA == idPlano).Result.ToList();
        }

        public async Task<PlanoContaSaldo> ObterSaldoPorId(long id)
        {
            return _planoContaSaldoRepository.ObterPorId(id).Result;
        }

        public async Task<double> ObterSaldoPorIdPlano(long idPlano)
        {
            var saldo = _planoContaSaldoRepository.Obter(x => x.IDCONTA == idPlano).Result.OrderByDescending(x=>x.DTHRATU).FirstOrDefault();
            return (saldo != null && saldo.VLSALDO.HasValue) ? saldo.VLSALDO.Value : 0;
        }

        #endregion

        #region Plano Conta Lancamento
        public async Task<PagedResult<PlanoContaLancamento>> ObterLancamentoPorPaginacao(long idPlano, DateTime dtIni, DateTime dtFim, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;

            var lista = await _planoContaLancamentoRepository.Buscar(x => x.IDCONTA == idPlano && (x.DTCAD.Value >= dtIni && x.DTCAD <= dtFim));

            return new PagedResult<PlanoContaLancamento>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList().OrderBy(x => x.DTCAD.Value),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task Adicionar(PlanoContaLancamento planoConta)
        {
            if (!ExecutarValidacao(new PlanoContaLancamentoValidation(), planoConta))
                return;

            await _planoContaLancamentoRepository.AdicionarSemSalvar(planoConta);
        }

        public async Task Atualizar(PlanoContaLancamento planoConta)
        {
            if (!ExecutarValidacao(new PlanoContaLancamentoValidation(), planoConta))
                return;

            await _planoContaLancamentoRepository.AtualizarSemSalvar(planoConta);
        }

        public async Task ApagarLancamento(long id)
        {
            if (await PlanoContaLancamentoUtilizado(id))
            {
                Notificar("Não é possivel apagar este plano de conta Lancamento pois o mesmo está sendo utilizado");
                return;
            }
            await _planoContaLancamentoRepository.RemoverSemSalvar(id);
        }

        public async Task<PlanoContaLancamento> ObterLancamentoPorId(long id)
        {
            return await _planoContaLancamentoRepository.ObterPorId(id);
        }


        #endregion

        #region private
        private async Task<bool> PlanoContaUtilizado(long idPlanpConta)
        {
            var resultado = false;
            //resultado = _produtoRepository.Obter(x => x.Id == idContato).Result.Any();

            return resultado;
        }

        private async Task<bool> PlanoContaLancamentoUtilizado(long idPlanpConta) => false;

   

        #endregion

    }
}

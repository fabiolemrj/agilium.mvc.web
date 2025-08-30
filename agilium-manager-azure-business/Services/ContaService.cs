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
using System.Transactions;
using System.Xml;

namespace agilium.api.business.Services
{
    public class ContaService : BaseService, IContaService
    {
        private IContaPagarRepository _contaPagarRepository;
        private IContaReceberRepository _contaReceberRepository;
        private readonly IDapperRepository _dapperRepository;
        private readonly IUtilDapperRepository _utilDapperRepository;
        private readonly IPContaPagarDapperRepository _contaPagarDapperRepository;
        private readonly IPlanoContaDapperRepository _planoContaDapperRepository;
        private readonly IPContaReceberDapperRepository _ContaReceberDapperRepository;

        public ContaService(INotificador notificador, IContaPagarRepository contaPagarRepository, IContaReceberRepository contaReceberRepository,
            IDapperRepository dapperRepository, IUtilDapperRepository utilDapperRepository, IPContaPagarDapperRepository contaPagarDapperRepository, 
            IPlanoContaDapperRepository planoContaDapperRepository, IPContaReceberDapperRepository contaReceberDapperRepository) : base(notificador)
        {
            _contaPagarRepository = contaPagarRepository;
            _contaReceberRepository = contaReceberRepository;
            _dapperRepository = dapperRepository;
            _utilDapperRepository = utilDapperRepository;
            _contaPagarDapperRepository = contaPagarDapperRepository;
            _planoContaDapperRepository = planoContaDapperRepository;
            _ContaReceberDapperRepository = contaReceberDapperRepository;
        }

        public async Task Salvar()
        {
            await _contaPagarRepository.SaveChanges();
        }

        public void Dispose()
        {
            _contaPagarRepository?.Dispose();
            _contaReceberRepository?.Dispose();
        }

        #region Conta Pagar
        public async Task Adicionar(ContaPagar contaPagar)
        {
            if (!ExecutarValidacao(new ContaValidation(), contaPagar))
                return;

            await _contaPagarRepository.AdicionarSemSalvar(contaPagar);
        }

        public async Task Apagar(long id)
        {
            if (await ContaPagarNaoPodeSerExcluido(id))
            {
                Notificar("Não é possivel apagar esta conta a pagar pois o mesmo está sendo utilizado");
                return;
            }
            await _contaPagarRepository.RemoverSemSalvar(id);
        }

        public async Task Atualizar(ContaPagar contaPagar)
        {
            if (!ExecutarValidacao(new ContaValidation(), contaPagar))
                return;

            await _contaPagarRepository.AtualizarSemSalvar(contaPagar);
        }


        public async Task<ContaPagar> ObterCompletoPorId(long id)
        {
            return await _contaPagarRepository.ObterPorId(id);
        }

        public async Task<List<ContaPagar>> ObterPorDescricao(string descricao)
        {
            return _contaPagarRepository.Buscar(x => x.DESCR.ToUpper().Contains(descricao.ToUpper())).Result.ToList();
        }

        public async Task<ContaPagar> ObterPorId(long id)
        {
            return await _contaPagarRepository.ObterPorId(id);
        }

        public async Task<PagedResult<ContaPagar>> ObterPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(nome) ? string.Empty : nome;

            var lista = await _contaPagarRepository.Obter(x => x.IDEMPRESA == idEmpresa && x.DESCR.ToUpper().Contains(_nomeParametro.ToUpper()), "CategFinanc", "Fornecedor", "PlanoConta");

            return new PagedResult<ContaPagar>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList().OrderBy(x => x.DTCAD),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            }; ;
        }

        public async Task<List<ContaPagar>> ObterTodas(long idEmpresa)
        {
            return _contaPagarRepository.Obter(x=>x.IDEMPRESA == idEmpresa).Result.ToList();
        }

        #region dapper
        public async Task<bool> ConsolidarContaPorId(long id)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                var lancamento = await _contaPagarDapperRepository.CriarContaLancamentoDeContaPagar(id); ;
                if (lancamento != null)
                {
                    var ID = _utilDapperRepository.GerarUUID().Result;

                    await _contaPagarDapperRepository.RealizarLancamento(ID, lancamento);

                    await _contaPagarDapperRepository.AtualizarConsolidacaoContaPagar(ID, id);

                    if (lancamento.IDCONTA > 0)
                        await _planoContaDapperRepository.AtualizarSaldoContaESubConta(lancamento.IDCONTA.Value);
                                      
                    resultado = true;
                }
                await _dapperRepository.Commit();
            }
            catch (Exception ex)
            {
                Notificar(ex.Message);
                await _dapperRepository.Rollback();
                resultado = false;
            }

            return resultado;
        }

        public async Task<bool> DesconsolidarContaPorId(long id)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                var contaPagar = await _contaPagarRepository.ObterPorId(id);
                if(contaPagar != null)
                {
                    var lancamento = await _contaPagarDapperRepository.ObterPlanoContaLancamentoPorId(contaPagar.IDLANC.Value);
                    if (lancamento != null)
                    {
                        var anoMesLanc = Convert.ToInt32(lancamento.DTCAD.Value.ToString("yyyyMM"));
                        var anoMesAtual = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));

                        await _contaPagarDapperRepository.AtualizarDesconsolidacaoContaPagarPorId(contaPagar.Id);
                        
                        if (anoMesLanc <= anoMesAtual)
                        {
                            await _contaPagarDapperRepository.ApagarLancamentoPorId(contaPagar.IDLANC.Value);
                        }
                        else
                        {
                            await _contaPagarDapperRepository.AtualizarLancamentoPorId(contaPagar.IDLANC.Value);
                        }

                        if (contaPagar.IDCONTA > 0)
                        {
                            await _planoContaDapperRepository.AtualizarSaldoContaESubConta(contaPagar.IDCONTA.Value);
                        }

                    }
                }

                await _dapperRepository.Commit();
                resultado = true;
            }
            catch (Exception ex)
            {
                Notificar(ex.Message);
                await _dapperRepository.Rollback();
                resultado = false;
            }

            return resultado;
        }
        #endregion

        #endregion

        #region Conta Receber
        public async Task Adicionar(ContaReceber contaReceber)
        {
            if (!ExecutarValidacao(new ContaReceberValidation(), contaReceber))
                return;

            await _contaReceberRepository.AdicionarSemSalvar(contaReceber);
        }

        public async Task Atualizar(ContaReceber contaReceber)
        {
            if (!ExecutarValidacao(new ContaReceberValidation(), contaReceber))
                return;

            await _contaReceberRepository.AtualizarSemSalvar(contaReceber);
        }

        public async Task ApagarContaReceber(long id)
        {
            if (await ContaReceberNaoPodeSerExcluido(id))
            {
                Notificar("Não é possivel apagar esta conta a receber pois o mesmo está sendo utilizado");
                return;
            }
            await _contaReceberRepository.RemoverSemSalvar(id);
        }

        public async Task<ContaReceber> ObterContaReceberPorId(long id)
        {
            return await _contaReceberRepository.ObterPorId(id);
        }

        public async Task<List<ContaReceber>> ObterContaReceberPorDescricao(string descricao)
        {
            return _contaReceberRepository.Buscar(x => x.DESCR.ToUpper().Contains(descricao.ToUpper())).Result.ToList();
        }

        public async Task<PagedResult<ContaReceber>> ObterContaReceberPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(nome) ? string.Empty : nome;

            var lista = await _contaReceberRepository.Obter(x => x.IDEMPRESA == idEmpresa && x.DESCR.ToUpper().Contains(_nomeParametro.ToUpper()), "CategFinanc", "Cliente", "PlanoConta");

            return new PagedResult<ContaReceber>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList().OrderBy(x => x.DTCAD),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            }; ;
        }

        public async Task<ContaReceber> ObterContaReceberCompletoPorId(long id)
        {
            return await _contaReceberRepository.ObterPorId(id);
        }

        public async Task<List<ContaReceber>> ObterTodasContaReceber(long idEmpresa)
        {
            return await _contaReceberRepository.ObterTodos();
        }

        #region dapper
        public async Task<bool> ConsolidarContaReceberPorId(long id)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                var lancamento = await _ContaReceberDapperRepository.CriarContaLancamentoDeContaReceber(id);
                if (lancamento != null)
                {
                    var ID = _utilDapperRepository.GerarUUID().Result;

                    await _ContaReceberDapperRepository.RealizarLancamento(ID, lancamento);
                    await _ContaReceberDapperRepository.AtualizarConsolidacaoContaReceber(ID, id);

                    if (lancamento.IDCONTA > 0)
                        await _planoContaDapperRepository.AtualizarSaldoContaESubConta(lancamento.IDCONTA.Value);
                }
                
                await _dapperRepository.Commit();
                resultado = true;
            }
            catch (Exception ex)
            {
                Notificar(ex.Message);
                await _dapperRepository.Rollback();
                resultado = false;
            }
            return resultado;        
        }

        public async Task<bool> DesconsolidarContaReceberPorId(long id)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();
                var contaReceber = await _contaReceberRepository.ObterPorId(id);
                if (contaReceber != null)
                {
                    var lancamento = await _ContaReceberDapperRepository.ObterPlanoContaLancamentoPorId(contaReceber.IDLANC.Value);
                    if (lancamento != null)
                    {
                        var anoMesLanc = Convert.ToInt32(lancamento.DTCAD.Value.ToString("yyyyMM"));
                        var anoMesAtual = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
                        await _ContaReceberDapperRepository.AtualizarDesconsolidacaoContaReceberPorId(contaReceber.Id);

                        if (anoMesLanc <= anoMesAtual)
                        {
                            await _ContaReceberDapperRepository.ApagarLancamentoPorId(contaReceber.IDLANC.Value);
                        }
                        else
                        {
                            await _ContaReceberDapperRepository.AtualizarLancamentoPorId(contaReceber.IDLANC.Value);
                        }

                        if (contaReceber.IDCONTA > 0)
                        {
                            await _planoContaDapperRepository.AtualizarSaldoContaESubConta(contaReceber.IDCONTA.Value);
                        }
                    }
                }
                
                await _dapperRepository.Commit();
                resultado = true;
            }
            catch (Exception ex)
            {
                Notificar(ex.Message);
                await _dapperRepository.Rollback();
                resultado = false;
            }
            return resultado;
          
        }


        #endregion

        #endregion

        #region private
        private async Task<bool> ContaPagarNaoPodeSerExcluido(long id) => false;

        private async Task<bool> ContaReceberNaoPodeSerExcluido(long id) => false;


        #endregion

    }
}

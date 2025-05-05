using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.PdvViewModel.CaixaReturnViewModel;
using agilium.api.business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Services
{
    public class CaixaService : BaseService, ICaixaService
    {
        private readonly ICaixaRepository _caixaRepository;
        private readonly ICaixaMovimentoRepository _caixaMovimentoRepository;
        private readonly ICaixaMoedaRepository _caixaMoedaRepository;
        private readonly ICaixaDapperRepository _caixaDapperRepsitory;
        private readonly ITurnoService _turnoService;
        private readonly IDapperRepository _dapperRepository;
        private readonly IUtilDapperRepository _utilDapperRepository;

        public CaixaService(INotificador notificador, ICaixaRepository caixaRepository, ICaixaMovimentoRepository caixaMovimentoRepository,
            ICaixaMoedaRepository caixaMoedaRepository, ICaixaDapperRepository caixaDapperRepsitory, ITurnoService turnoService, 
            IDapperRepository dapperRepository, IUtilDapperRepository utilDapperRepository) : base(notificador)
        {
            _caixaRepository = caixaRepository;
            _caixaMovimentoRepository = caixaMovimentoRepository;
            _caixaMoedaRepository = caixaMoedaRepository;
            _caixaDapperRepsitory = caixaDapperRepsitory;
            _turnoService = turnoService;
            _dapperRepository = dapperRepository;
            _utilDapperRepository = utilDapperRepository;
        }

        public void Dispose()
        {
            _caixaMovimentoRepository?.Dispose();
            _caixaRepository?.Dispose();
        }

        public async Task Salvar()
        {
            await _caixaRepository.SaveChanges();
        }

        #region Caixa
        public async Task<Caixa> ObterCompletoPorId(long id)
        {
            return _caixaRepository.Obter(x => x.Id == id, "Empresa", "PontoVenda", "Funcionario", "Turno").Result.FirstOrDefault();
        }

        public async Task<PagedResult<Caixa>> ObterPorPaginacao(long idEmpresa, DateTime dtIni, DateTime dtFim, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;

            var lista = _caixaRepository.Obter(x => x.IDEMPRESA == idEmpresa && (x.DTHRABT.Value >= dtIni && x.DTHRABT <= dtFim), "Empresa", "PontoVenda", "Funcionario", "Turno").Result.OrderByDescending(x=>x.SQCAIXA).OrderByDescending(x=>x.DTHRABT);

            return new PagedResult<Caixa>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList().OrderBy(x => x.DTHRABT),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }


        #endregion

        #region Caixa Movimentacao
        public async Task<PagedResult<CaixaMovimento>> ObterMovimentacaoPorPaginacao(long idCaixa, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;

            var lista = _caixaMovimentoRepository.Obter(x => x.IDCAIXA == idCaixa, "Caixa").Result;

            return new PagedResult<CaixaMovimento>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }


        #endregion

        #region Caixa Moeda

        public async Task<PagedResult<CaixaMoeda>> ObterMoedaPorPaginacao(long idCaixa, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;

            var lista = _caixaMoedaRepository.Obter(x => x.IDCAIXA == idCaixa, "Moeda","Caixa", "UsuarioCorrecao").Result;

            return new PagedResult<CaixaMoeda>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task RealizarCorrecaoValor(CaixaMoeda caixaMoeda)
        {
            if (!ExecutarValidacao(new CaixaMoedaValidation(), caixaMoeda))
                return;
            await _caixaMoedaRepository.AtualizarSemSalvar(caixaMoeda);
        }

        public async Task<CaixaMoeda> ObterCaixaMoedaCompletoPorId(long id)
        {
            return _caixaMoedaRepository.Obter(x => x.Id == id, "Moeda", "Caixa", "UsuarioCorrecao").Result.FirstOrDefault();
        }

        #endregion

        #region Abrir Caixa

        public async Task<int> AbrirCaixa(long idEmpresa, long idUsuario, long idPdv)  
        {
            var resultado = 0;
            var turnoAberto = await _turnoService.ObterTurnoAbertoCompletoPorId(idEmpresa);
            if(turnoAberto == null)
            {
                Notificar($"Não foi localizado turno aberto");
                return resultado;
            }

            if(idUsuario == 0)
            {
                Notificar($"Não foi localizado usuario para abrir caixa");
                return resultado; ;
            }

            if(idPdv == 0)
            {
                Notificar($"Não foi localizado Ponto de Venda para abrir caixa");
                return resultado;
            }

            try 
            {
                await _dapperRepository.BeginTransaction();

                
                var idFuncionario = await _caixaDapperRepsitory.ObterIdFuncionarioPorUsuarioEmpresa(idEmpresa, idUsuario);
                if(idFuncionario == 0)
                {
                    Notificar($"Para abrir o caixa o usuário precisar estar associado a um cadastro de funcionário.");
                    return resultado;
                }

                var idCaixaAberto = await _caixaDapperRepsitory.ObterIdCaixaAberto(idEmpresa,idFuncionario);
                if (idCaixaAberto > 0)
                {
                    Notificar($"O caixa não pode ser aberto, pois este funcionário possui outro Caixa Aberto!");
                    return resultado;
                }

                var tipoAberturaCaixa = await _caixaDapperRepsitory.ObterTipoAberturaCaixaPorEmpresa(idEmpresa);

                //   const CAIXA_TPABERTURA_SALDOANTERIOR = 1;
                //   const CAIXA_TPABERTURA_SALDOZERADO = 2;
                //

                double valorAberturaCaixa = 0;
                if (tipoAberturaCaixa.VALOR == "1")
                {
                    valorAberturaCaixa = await _caixaDapperRepsitory.ObterValorCaixaAnterior(idFuncionario, idEmpresa);
                
                    //Se o saldo for negativo, significa que a sangria foi maior que o saldo do caixa
                    if (valorAberturaCaixa < 0) valorAberturaCaixa = 0;
                }

                var valorSuprimento = await _caixaDapperRepsitory.ObterConfigRealizaSuprimentoAbertura(idEmpresa);

                resultado = await _caixaDapperRepsitory.GerarSeqCaixa(idEmpresa);
                var idCaixa = await _caixaDapperRepsitory.IncluirCaixa(idEmpresa,turnoAberto.Id,idPdv,idFuncionario,resultado,valorAberturaCaixa);
                
                if(valorSuprimento > 0 && idCaixa > 0)
                {
                    var descricaoSuprimento = await _caixaDapperRepsitory.ObterConfigDescricaoSuprimentoAbertura(idEmpresa);
                    await _caixaDapperRepsitory.RealizarSuprimento(idCaixa,idFuncionario,valorSuprimento,descricaoSuprimento);
                }

                if (idCaixa == 0) resultado = 0;

                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar abrir caixa");
                }
            }
            catch (Exception ex)
            {
                await _dapperRepository.Rollback();
                do
                {
                    Notificar(ex.Message);
                }
                while (ex != null);
            }
            return resultado;
        }


        #endregion

        #region Fechar Caixa
        public async Task<bool> FecharCaixa(long idCaixa, long idUsuario, double valorFechamneto, string msgFechamento)
        {
            var resultado = false;
            var caixa = await _caixaDapperRepsitory.ObterCaixaPorId(idCaixa);
            
            if(caixa == null || caixa.STCAIXA != Enums.ESituacaoCaixa.Aberto)
            {
                Notificar($"Não foi localizado caixa aberto");
                return resultado;
            }

            var turnoAberto = await _turnoService.ObterTurnoAbertoCompletoPorId(caixa.IDEMPRESA.Value);
            if (turnoAberto == null)
            {
                Notificar($"Não foi localizado turno aberto");
                return resultado;
            }

            if (idUsuario == 0)
            {
                Notificar($"Não foi localizado usuario para fechar caixa");
                return resultado;
            }

            try
            {
                await _dapperRepository.BeginTransaction();

                var idFuncionario = await _caixaDapperRepsitory.ObterIdFuncionarioPorUsuarioEmpresa(caixa.IDEMPRESA.Value, idUsuario);
                if (idFuncionario == 0)
                {
                    Notificar($"Para fechar o caixa o usuário precisar estar associado a um cadastro de funcionário.");
                    return resultado;
                }

                //realizar sangria
                await _caixaDapperRepsitory.RealizarSangria(idCaixa, idFuncionario, valorFechamneto, msgFechamento);

                //obter saldo do caixa
                var saldoCaixa = await _caixaDapperRepsitory.ObterSaldoCaixa(idCaixa);
                
                //fechar caixa
                await _caixaDapperRepsitory.FecharCaixa(idCaixa, saldoCaixa);
                
                if (!TemNotificacao())
                {
                    resultado = true;
                    await _dapperRepository.Commit();
                }
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar fechar caixa");
                }
            }
            catch (Exception ex)
            {
                await _dapperRepository.Rollback();
                do
                {
                    Notificar(ex.Message);
                }
                while (ex != null);
            }
            return resultado;
        }

        public async Task<FecharCaixaInicializarViewModel> ObterCaixaParaFechamento(long idEmpresa, long idusuario)
        {
            var idFuncionario = await _caixaDapperRepsitory.ObterIdFuncionarioPorUsuarioEmpresa(idEmpresa, idusuario);
            if (idFuncionario == 0)
            {
                Notificar($"Para fechar o caixa o usuário precisar estar associado a um cadastro de funcionário.");
                return null;
            }

            var idCaixaAberto = await _caixaDapperRepsitory.ObterIdCaixaAberto(idEmpresa, idFuncionario);
            var caixa = await _caixaDapperRepsitory.ObterCaixaParaFechamento(idCaixaAberto);
            if(caixa == null)
            {
                Notificar("Não foi localizado caixa aberto par aeste ususario");
            }

            return caixa;
        }
        public async Task<FecharCaixaInicializarViewModel> ObterCaixaParaFechamento(long idCaixa)
        {
            return await _caixaDapperRepsitory.ObterCaixaParaFechamento(idCaixa);
        }
        #endregion

        #region Movimento Caixa
     

        public async Task<bool> RealizarSangria(long idCaixa, long idUsuario, double valor, string msg)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();
                
                var caixa = await _caixaDapperRepsitory.ObterCaixaPorId(idCaixa);

                if (caixa == null || caixa.STCAIXA != Enums.ESituacaoCaixa.Aberto)
                {
                    Notificar($"Não foi localizado caixa aberto");
                    return resultado;
                }

                var turnoAberto = await _turnoService.ObterTurnoAbertoCompletoPorId(caixa.IDEMPRESA.Value);
                if (turnoAberto == null)
                {
                    Notificar($"Não foi localizado turno aberto");
                    return resultado;
                }

                var idFuncionario = await _caixaDapperRepsitory.ObterIdFuncionarioPorUsuarioEmpresa(caixa.IDEMPRESA.Value, idUsuario);
                if (idFuncionario == 0)
                {
                    Notificar($"Para realizar movimento de caixa o usuário precisar estar associado a um cadastro de funcionário.");
                    return resultado;
                }

                var idMov = await _caixaDapperRepsitory.RealizarSangria(idCaixa, idFuncionario, valor, msg);
                
                resultado = idMov > 0;
                
                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar realizar sangria");
                }
            }
            catch (Exception ex)
            {
                await _dapperRepository.Rollback();
                do
                {
                    Notificar(ex.Message);
                }
                while (ex != null);
            }
            return resultado;
        }

        public async Task<bool> RealizarSuprimento(long idCaixa, long idUsuario, double valor, string msg)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                var caixa = await _caixaDapperRepsitory.ObterCaixaPorId(idCaixa);

                if (caixa == null || caixa.STCAIXA != Enums.ESituacaoCaixa.Aberto)
                {
                    Notificar($"Não foi localizado caixa aberto");
                    return resultado;
                }

                var turnoAberto = await _turnoService.ObterTurnoAbertoCompletoPorId(caixa.IDEMPRESA.Value);
                if (turnoAberto == null)
                {
                    Notificar($"Não foi localizado turno aberto");
                    return resultado;
                }

                var idFuncionario = await _caixaDapperRepsitory.ObterIdFuncionarioPorUsuarioEmpresa(caixa.IDEMPRESA.Value, idUsuario);
                if (idFuncionario == 0)
                {
                    Notificar($"Para realizar movimento de caixa o usuário precisar estar associado a um cadastro de funcionário.");
                    return resultado;
                }

                var idMov = await _caixaDapperRepsitory.RealizarSuprimento(idCaixa, idFuncionario, valor, msg);

                resultado = idMov > 0;

                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar realizar suprimento");
                }
            }
            catch (Exception ex)
            {
                await _dapperRepository.Rollback();
                do
                {
                    Notificar(ex.Message);
                }
                while (ex != null);
            }
            return resultado;
        }


        #endregion

        #region Caixa Aberto

        public async Task<Caixa> ObterCaixaAbertoPorEmpresa(long idEmpresa, long idUsuario)
        {
            var turnoAberto = await _turnoService.ObterTurnoAbertoCompletoPorId(idEmpresa);
            if (turnoAberto == null)
            {
                Notificar($"Não foi localizado turno aberto");                
            }

            if (idUsuario == 0)
            {
                Notificar($"Não foi localizado usuario para abrir caixa");                
            }
            var idFuncionario = await _caixaDapperRepsitory.ObterIdFuncionarioPorUsuarioEmpresa(idEmpresa, idUsuario);
            if (idFuncionario == 0)
            {
                Notificar($"Não foi localizado cadastro de funcionário.");             
            }

            return await _caixaDapperRepsitory.ObterCaixaAberto(idEmpresa, idFuncionario);
        }


        #endregion
    }
}

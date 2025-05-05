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
    public class ConfigService : BaseService, IConfigService
    {
        private readonly IConfigRepository _configRepository;
        private readonly IConfigImagemRepository _configImagemRepository;
        private readonly IDapperRepository _dapperRepository;
        private readonly IUtilDapperRepository _utilDapperRepository;
        private readonly IConfigDapperRepository _configRepositoryDapperRepository;
        public ConfigService(INotificador notificador, IConfigRepository configRepository,
            IConfigImagemRepository configImagemRepository,IDapperRepository dapperRepository, 
            IUtilDapperRepository utilDapperRepository, IConfigDapperRepository configRepositoryDapperRepository) : base(notificador)
        {
            _configRepository = configRepository;
            _configImagemRepository = configImagemRepository;
            _dapperRepository = dapperRepository;
            _utilDapperRepository = utilDapperRepository;
            _configRepositoryDapperRepository = configRepositoryDapperRepository;
        }

        #region Config
        public async Task Adicionar(Config config)
        {
            if (!ExecutarValidacao(new ConfigValidation(), config))
                return;

            await _configRepository.AdicionarSemSalvar(config);
        }

        public async Task Adicionar(ConfigImagem config)
        {
            if (_configImagemRepository.Obter(x => x.CHAVE.ToUpper() == config.CHAVE.ToUpper() && x.IDEMPRESA == config.IDEMPRESA).Result.Any())
                return;

            await _configImagemRepository.AdicionarSemSalvar( config);
        }

        public async Task Apagar(string chave, long idEmpresa)
        {
            var objeto = _configRepository.Obter(x=>x.CHAVE.ToUpper() == chave.ToUpper() && x.IDEMPRESA == idEmpresa).Result.FirstOrDefault();
            
            if(objeto != null)
            {
                await _configRepository.RemoverSemSalvar(objeto);
            }
        }

        public async Task ApagarImagem(string chave, long idEmpresa)
        {
            var objeto = _configImagemRepository.Obter(x => x.CHAVE.ToUpper() == chave.ToUpper() && x.IDEMPRESA == idEmpresa).Result.FirstOrDefault();

            if (objeto != null)
                await _configImagemRepository.RemoverSemSalvar(objeto);
        }

        public async Task Atualizar(Config config)
        {
            if (!ExecutarValidacao(new ConfigValidation(), config))
                return;

            await _configRepository.AtualizarSemSalvar(config);
        }

        public async Task Atualizar(ConfigImagem config)
        {          
            await _configImagemRepository.AtualizarSemSalvar(config);
        }

   
        public void Dispose()
        {
            _configRepository?.Dispose();
            _configImagemRepository?.Dispose();
        }

        public async Task<ConfigImagem> ObterConfigImagemPorChave(string chave, long idEmpresa)
        {
            return _configImagemRepository.Obter(x => x.CHAVE.ToUpper() == chave.ToUpper() && x.IDEMPRESA == idEmpresa).Result.FirstOrDefault();
        }

        public async Task<Config> ObterPorChave(string chave, long idEmpresa)
        {
            return _configRepository.Obter(x => x.CHAVE.ToUpper() == chave.ToUpper() && x.IDEMPRESA == idEmpresa).Result.FirstOrDefault();
        }

        public async Task<PagedResult<Config>> ObterPorDescricaoPaginacao(long idEmpresa, string descricao, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(descricao) ? string.Empty : descricao;

            var lista = await _configRepository.Buscar(x => x.IDEMPRESA == idEmpresa && x.CHAVE.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<Config>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<ConfigImagem>> ObterTodosConfigImagem(long idEmpresa)
        {
            return _configImagemRepository.Obter(x => x.IDEMPRESA == idEmpresa).Result.ToList();
        }

        public async Task<List<Config>> ObterTodosPorEmpresa(long idEmpresa)
        {
            return _configRepository.Obter(x => x.IDEMPRESA == idEmpresa).Result.ToList();
        }

        public async Task Salvar()
        {
            await _configRepository.SaveChanges();
        }
        #endregion

        #region Dapper

        #region Config Geral
        public async Task<bool> DefinirBalancaPDV(string modelo, string cdHandShake, string CDPARITYBAL, string cdSerialStop, string nuDataBits, string nuBaudRate, string dsPorta, long IDPDV)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                await _configRepositoryDapperRepository.DefinirBalancaPDV(modelo,cdHandShake,CDPARITYBAL,cdSerialStop,nuDataBits,nuBaudRate,dsPorta,IDPDV);

                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar definir balança do PDV");
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

        public async Task<bool> DefinirCertificadoDigitalMaquina(long IDEMPRESA, string NMMAQUINA, string DSCAMINHO, string DSSENHA)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                await _configRepositoryDapperRepository.DefinirCertificadoDigitalMaquina(IDEMPRESA, NMMAQUINA, DSCAMINHO, DSSENHA);

                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar definir certificado digital na maquina ");
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

        public async Task<bool> DefinirMaquinaPdv(string nomeMaquina, long idPdv)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                await _configRepositoryDapperRepository.DefinirPdv(idPdv, nomeMaquina);

                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar definir maquina ");
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

        public async Task<bool> DefinirPortaImpressoraPDV(string portaImpressora, long idPdv)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                await _configRepositoryDapperRepository.DefinirPortaImpressoraPDV(portaImpressora,idPdv);

                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar definir porta Ip da impressora");
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

        public async Task<bool> DesassociarMaquinaPDV(long idPdv)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                await _configRepositoryDapperRepository.DesassociarMaquinaPDV(idPdv);

                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar desassociar PDV");
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

        public async Task<IEnumerable<Config>> ObterConfiguracaoGeral(long IDEMPRESA, long idPdv)
        {
            var resultado = new List<Config>();
            try
            {
                await _dapperRepository.BeginTransaction();

                resultado = _configRepositoryDapperRepository.ObterConfigGeral(IDEMPRESA,idPdv).Result.ToList();

                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar desassociar PDV");
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

        public async Task<IEnumerable<PontoVenda>> ObterPdvParaSelecao(long IDEMPRESA, long idUsuario)
        {
            var resultado = new List<PontoVenda>();
            try
            {
                await _dapperRepository.BeginTransaction();

                resultado = _configRepositoryDapperRepository.ObterPdvParaSelecao(IDEMPRESA, idUsuario).Result.ToList();

                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar obter PDV disponiveis para associação");
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

        public async Task<bool> AtualizarConfig(List<Config> lista, long idUsuario)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                lista.ForEach(async config => {
                    //salvar pdv
                    if (config.CHAVE == "IDPDV")
                    {
                        long idPdv = Convert.ToInt64(config.VALOR);
                        await _configRepositoryDapperRepository.DefinirPdv(idPdv,idUsuario.ToString());
                    }
                    else
                    {
                        await _configRepositoryDapperRepository.SalvarConfig(config);
                    }
                });
                resultado = true;
                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar salvar configuração");
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

        public async Task<long> ObterPdvPorNomeMaquina(long idUsuario)
        {
            long resultado = 0;
            try
            {
                await _dapperRepository.BeginTransaction();

                resultado = await _configRepositoryDapperRepository.ObterPdvPorNomeMaquina(idUsuario);

                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar obter PDV Por nome maquina");
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

        #region config balanca
        
        public async Task<IEnumerable<Config>> ObterConfigPreVenda(long idEmpresa)
        {
            var resultado = new List<Config>();
            try
            {
                await _dapperRepository.BeginTransaction();

                resultado.Add(await _configRepositoryDapperRepository.ObterConfigPreVenda(idEmpresa));

                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar obter config Pre-venda");
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

        public async Task<bool> AtualizarConfigBalanca(List<Config> lista, long idPdv)
        {
            var resultado = false;

            try
            {
                await _dapperRepository.BeginTransaction();

                string CDMODELOBAL = string.Empty;
                string CDHANDSHAKEBAL = string.Empty;
                string CDPARITYBAL = string.Empty;
                string CDSERIALSTOPBITBAL = string.Empty;
                string NUDATABITBAL = string.Empty;
                string NUBAUDRATEBAL = string.Empty;
                string DSPORTABAL = string.Empty;

                lista.ForEach(async config => {
                    if (config.CHAVE == "CDMODELOBAL")
                    {
                        CDMODELOBAL = config.CHAVE;
                    }
                    else if (config.CHAVE == "CDHANDSHAKEBAL")
                    {
                        CDHANDSHAKEBAL = config.CHAVE;
                    }
                    else if (config.CHAVE == "CDPARITYBAL")
                    {
                        CDPARITYBAL = config.CHAVE;
                    }
                    else if (config.CHAVE == "CDSERIALSTOPBITBAL")
                    {
                        CDSERIALSTOPBITBAL = config.CHAVE;
                    }
                    else if (config.CHAVE == "NUDATABITBAL")
                    {
                        NUDATABITBAL = config.CHAVE;
                    }
                    else if (config.CHAVE == "NUBAUDRATEBAL")
                    {
                        NUBAUDRATEBAL = config.CHAVE;
                    }
                    else if (config.CHAVE == "DSPORTABAL")
                    {
                        DSPORTABAL = config.CHAVE;
                    }

                    await _configRepositoryDapperRepository.SalvarConfigBalanca(CDMODELOBAL, CDPARITYBAL, CDSERIALSTOPBITBAL, CDSERIALSTOPBITBAL,
                        NUDATABITBAL, NUBAUDRATEBAL, DSPORTABAL, idPdv);

                });
                resultado = true;
                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar salvar configuração");
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

        public async Task<IEnumerable<Config>> ObterConfiguracaoBalanca(long IDEMPRESA, long idPdv)
        {
            var resultado = new List<Config>();
            try
            {
                await _dapperRepository.BeginTransaction();

                resultado = _configRepositoryDapperRepository.ObterConfigBalanca(IDEMPRESA, idPdv).Result.ToList();

                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar obter config balanca");
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

        public async Task<bool> AtualizarConfigPreVenda(List<Config> lista, long idEmpresa)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                lista.ForEach(async config => {
                    //salvar pdv
                    await _configRepositoryDapperRepository.SalvarConfig(config);
                });
                resultado = true;
                if (!TemNotificacao())
                    await _dapperRepository.Commit();
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar salvar configuração");
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


        #endregion
    }
}

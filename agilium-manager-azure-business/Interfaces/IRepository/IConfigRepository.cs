using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface IConfigRepository: IRepository<Config>
    {
    }

    public interface IConfigImagemRepository : IRepository<ConfigImagem>
    {
    }

    public interface IConfigDapperRepository
    {
        Task<business.Models.Config> ObterConfig(string chave, long idEmpresa);

        #region Configuracao geral
        Task<long> DefinirPdv(long idPdv, string nomeMaquina);
        Task<bool> DefinirPortaImpressoraPDV(string portaImpressora, long IDPDV);
        Task<bool> DesassociarMaquinaPDV(long idPdv);
        Task<bool> DefinirBalancaPDV(string modelo, string cdHandShake, string CDPARITYBAL, string cdSerialStop, string nuDataBits,
            string nuBaudRate, string dsPorta, long IDPDV);
        Task<bool> DefinirCertificadoDigitalMaquina(long IDEMPRESA, string NMMAQUINA, string DSCAMINHO, string DSSENHA);
        Task<IEnumerable<Config>> ObterConfigGeral(long IDEMPRESA, long idPdv);
        Task<IEnumerable<PontoVenda>> ObterPdvParaSelecao(long IDEMPRESA, long idUsuario);
        Task<bool> SalvarConfig(Config config);
        Task<long> ObterPdvPorNomeMaquina(long idUsuario);
        #endregion

        #region Configuracao Balanca
        Task<IEnumerable<Config>> ObterConfigBalanca(long IDEMPRESA, long idPdv);
        Task<bool> SalvarConfigBalanca(string CDMODELOBAL, string CDHANDSHAKEBAL, string CDPARITYBAL, string CDSERIALSTOPBITBAL,
            string NUDATABITBAL, string NUBAUDRATEBAL, string DSPORTABAL, long IDPDV);
        Task<business.Models.Config> ObterConfigPreVenda(long IDEMPRESA);
        #endregion

    }
}

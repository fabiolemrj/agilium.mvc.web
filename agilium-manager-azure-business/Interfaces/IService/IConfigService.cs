using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IConfigService: IDisposable
    {
        Task Adicionar(Config config);
        Task Atualizar(Config config);
        Task Apagar(string chave, long idEmpresa);
        Task<Config> ObterPorChave(string chave, long idEmpresa);
        Task<List<Config>> ObterTodosPorEmpresa(long idEmpresa);
        Task Salvar();
        Task<PagedResult<Config>> ObterPorDescricaoPaginacao(long idEmpresa, string descricao, int page = 1, int pageSize = 15);

        Task Adicionar(ConfigImagem config);
        Task Atualizar(ConfigImagem config);
        Task ApagarImagem(string chave, long idEmpresa);
        Task<ConfigImagem> ObterConfigImagemPorChave(string chave, long idEmpresa);
        Task<IEnumerable<ConfigImagem>> ObterTodosConfigImagem(long idEmpresa);


        #region Dapper

        #region Config geral
        Task<bool> DefinirMaquinaPdv(string nomeMaquina, long idPdv);
        Task<bool> DefinirPortaImpressoraPDV(string portaImpressora, long idPdv);
        Task<bool> DesassociarMaquinaPDV(long idPdv);
 
        Task<bool> DefinirCertificadoDigitalMaquina(long IDEMPRESA, string NMMAQUINA, string DSCAMINHO, string DSSENHA);
        Task<IEnumerable<Config>> ObterConfiguracaoGeral(long IDEMPRESA, long idPdv);
        Task<IEnumerable<PontoVenda>> ObterPdvParaSelecao(long IDEMPRESA, long idUsuario);
        Task<bool> AtualizarConfig(List<Config> lista, long idUsuario);
        Task<long> ObterPdvPorNomeMaquina(long idUsuario);
        #endregion

        #region config balanca
        Task<bool> DefinirBalancaPDV(string modelo, string cdHandShake, string CDPARITYBAL, string cdSerialStop, string nuDataBits,
                                        string nuBaudRate, string dsPorta, long IDPDV);
        Task<IEnumerable<Config>> ObterConfiguracaoBalanca(long IDEMPRESA, long idPdv);
        Task<IEnumerable<Config>> ObterConfigPreVenda(long idEmpresa);
        Task<bool> AtualizarConfigPreVenda(List<Config> lista, long idEmpresa);
        #endregion

        #endregion
    }
}

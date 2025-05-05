using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace agilium.api.infra.Repository.Dapper
{
    public class ConfigDapperRepository : IConfigDapperRepository
    {
        private readonly IUtilDapperRepository _utilDapperRepository;
        protected readonly IConfiguration _configuration;
        private readonly DbSession _dbSession;

        public ConfigDapperRepository(IUtilDapperRepository utilDapperRepository, IConfiguration configuration, DbSession dbSession)
        {
            _utilDapperRepository = utilDapperRepository;
            _configuration = configuration;
            _dbSession = dbSession;
        }

        public string GetConnection()
        {
            var autenticacaoUrl = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return autenticacaoUrl;
        }

        #region Configuracao Geral
        public async Task<bool> DefinirBalancaPDV(string modelo, string cdHandShake, string CDPARITYBAL, string cdSerialStop, string nuDataBits, string nuBaudRate, string dsPorta, long IDPDV)
        {
            var parametros = new DynamicParameters();

            parametros.Add("@CDMODELOBAL", modelo, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@IDPDV", IDPDV, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@CDHANDSHAKEBAL", cdHandShake, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@CDPARITYBAL", CDPARITYBAL, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@CDSERIALSTOPBITBAL", cdSerialStop, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@NUDATABITBAL", nuDataBits, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@NUBAUDRATEBAL", nuBaudRate, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@DSPORTABAL", dsPorta, DbType.String, ParameterDirection.Input);

            var query = $@"UPDATE pdv SET CDMODELOBAL = @CDMODELOBAL, CDHANDSHAKEBAL =@CDHANDSHAKEBAL,CDPARITYBAL = @CDPARITYBAL, CDSERIALSTOPBITBAL = @CDSERIALSTOPBITBAL,
         NUDATABITBAL = @NUDATABITBAL, NUBAUDRATEBAL = @NUBAUDRATEBAL, DSPORTABAL = @DSPORTABAL WHERE IDPDV = @IDPDV";

            return (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0);
        }

        public async Task<bool> DefinirCertificadoDigitalMaquina(long IDEMPRESA, string NMMAQUINA, string DSCAMINHO, string DSSENHA)
        {
            var parametros = new DynamicParameters();

            parametros.Add("@DSCAMINHO", DSCAMINHO, DbType.String, ParameterDirection.Input);
            parametros.Add("@DSSENHA", DSSENHA, DbType.String, ParameterDirection.Input);
            parametros.Add("@NMMAQUINA", NMMAQUINA, DbType.String, ParameterDirection.Input);
            parametros.Add("@IDEMPRESA", IDEMPRESA, DbType.Int64, ParameterDirection.Input);

            var query = $@"UPDATE config_certif SET DSCAMINHO = @DSCAMINHO, DSSENHA = @DSSENHA WHERE NMMAQUINA = @NMMAQUINA AND IDEMPRESA = @IDEMPRESA";
            if (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) == 0)
            {
                var id = _utilDapperRepository.GerarUUID().Result;

                parametros.Add("@IDCONFIGCERTIF", id, DbType.Int64, ParameterDirection.Input);


                query = string.Empty;
                query = $@"INSERT INTO config_certif (IDCONFIGCERTIF, IDEMPRESA, NMMAQUINA, DSCAMINHO, DSSENHA)
                            values (@IDCONFIGCERTIF, @IDEMPRESA, @NMMAQUINA, @DSCAMINHO, @DSSENHA))";
                return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
            }
            return true;
        }

        public async Task<long> DefinirPdv(long idPdv, string nomeMaquina)
        {
            var parametros = new DynamicParameters();

            parametros.Add("@NMMAQUINA", nomeMaquina, DbType.String, ParameterDirection.Input);
            parametros.Add("@IDPDV", idPdv, DbType.Int64, ParameterDirection.Input);
            
            var query = $@"UPDATE pdv SET NMMAQUINA = @NMMAQUINA WHERE IDPDV = @IDPDV AND (NMMAQUINA IS NULL OR NMMAQUINA = '')";

            if(_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0)
            {
                query = string.Empty;
                query = $@"UPDATE pdv SET NMMAQUINA = NULL  WHERE IDPDV <> @IDPDV AND NMMAQUINA = @NMMAQUINA";
                return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction);
            }

            return 0;
        }

        public async Task<bool> DefinirPortaImpressoraPDV(string portaImpressora, long IDPDV)
        {
            var parametros = new DynamicParameters();

            parametros.Add("@DSPORTAIMPRESSORA", portaImpressora, DbType.String, ParameterDirection.Input);
            parametros.Add("@IDPDV", IDPDV, DbType.Int64, ParameterDirection.Input);

            var query = $@"UPDATE pdv SET DSPORTAIMPRESSORA = @DSPORTAIMPRESSORA WHERE IDPDV = @IDPDV";

            return (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0);
        }

        public async Task<bool> DesassociarMaquinaPDV(long idPdv)
        {
            var parametros = new DynamicParameters();

            parametros.Add("@IDPDV", idPdv, DbType.Int64, ParameterDirection.Input);

            var query = $@"UPDATE pdv SET NMMAQUINA = NULL WHERE IDPDV = @IDPDV";

            return (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0);
        }

        public async Task<IEnumerable<business.Models.Config>> ObterConfigGeral(long IDEMPRESA, long idPdv)
        {
            var listaConfiguracao = new List<business.Models.Config>();

            var parametros = new DynamicParameters();
            parametros.Add("@IDPDV", idPdv, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDEMPRESA", IDEMPRESA, DbType.Int64, ParameterDirection.Input);

            //descricaoPDV
            var query = $@"SELECT IDPDV as Id, DSPDV,NMIMPRESSORA,DSPORTAIMPRESSORA,STGAVETA FROM pdv WHERE  NMMAQUINA = @IDPDV and IDEMPRESA = @IDEMPRESA";

            var pdv = _dbSession.Connection.Query<PontoVenda>(query, parametros, _dbSession.Transaction).FirstOrDefault();
            if (pdv != null)
            {
                listaConfiguracao.Add(new business.Models.Config("IDPDV", IDEMPRESA,pdv.Id.ToString()));
                if(!string.IsNullOrEmpty(pdv.NMIMPRESSORA)) listaConfiguracao.Add(new business.Models.Config("NMIMPRESSORA", IDEMPRESA, pdv.NMIMPRESSORA));
                if(!string.IsNullOrEmpty(pdv.DSPORTAIMPRESSORA)) listaConfiguracao.Add(new business.Models.Config("DSPORTAIMPRESSORA", IDEMPRESA, pdv.DSPORTAIMPRESSORA));
                if (pdv.STGAVETA == ESimNao.Sim)
                {
                    listaConfiguracao.Add(new business.Models.Config("STGAVETA", IDEMPRESA, "true"));
                }
                else
                {
                    listaConfiguracao.Add(new business.Models.Config("STGAVETA", IDEMPRESA, "false"));
                }
            }

            //tamanho fonte PDV
            var config = ObterConfig("PDV_TAMANHO_FONTE", IDEMPRESA).Result;
            if (config != null && !string.IsNullOrEmpty(config.VALOR))
                listaConfiguracao.Add(config);

            //PreVenda
            var configPrevenda = ObterConfig("PDV_PREVENDA", IDEMPRESA).Result;
            if (configPrevenda != null)
                listaConfiguracao.Add(configPrevenda);

            return listaConfiguracao;
        }

        public async Task<business.Models.Config> ObterConfig(string chave, long idEmpresa)
        {
            var parametros = new DynamicParameters();

            var query = $@"select VALOR, CHAVE, IDEMPRESA from config where IDEMPRESA = @IDEMPRESA AND CHAVE = @CHAVE_PREVENDA";
            parametros.Add("@CHAVE_PREVENDA", chave, DbType.String, ParameterDirection.Input);
            parametros.Add("@IDEMPRESA", idEmpresa, DbType.Int64, ParameterDirection.Input);
            return _dbSession.Connection.Query<business.Models.Config>(query, parametros, _dbSession.Transaction).FirstOrDefault();

        }

        public async Task<IEnumerable<PontoVenda>> ObterPdvParaSelecao(long IDEMPRESA, long idUsuario)
        {
            var parametros = new DynamicParameters();

            var query = $@"SELECT P.IDPDV as ID, P.* FROM pdv P
                            WHERE P.STPDV = 1 AND ((P.NMMAQUINA IS NULL OR P.NMMAQUINA = '') OR (P.NMMAQUINA = @IDPDV))
                            AND P.IDEMPRESA = @IDEMPRESA 
                            ORDER BY P.CDPDV, P.DSPDV";
            parametros.Add("@IDEMPRESA", IDEMPRESA, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDPDV", idUsuario, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Query<PontoVenda>(query, parametros, _dbSession.Transaction);
        }

        public async Task<bool> SalvarConfig(business.Models.Config config)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@VALOR", config.VALOR, DbType.String, ParameterDirection.Input);
            parametros.Add("@IDEMPRESA", config.IDEMPRESA, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@CHAVE", config.CHAVE, DbType.String, ParameterDirection.Input);

            var query = $@"UPDATE config SET VALOR = @VALOR WHERE IDEMPRESA = @IDEMPRESA and CHAVE = @CHAVE";

            return (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0);
        }

        public async Task<long> ObterPdvPorNomeMaquina(long idUsuario)
        {
            var parametros = new DynamicParameters();

            var query = $@" select IDPDV from pdv where NMMAQUINA =@NMMAQUINA";
            parametros.Add("@NMMAQUINA", idUsuario, DbType.Int64, ParameterDirection.Input);

            return _dbSession.Connection.Query<long>(query, parametros, _dbSession.Transaction).FirstOrDefault();
        }



        #endregion

        #region Configuracao Balanca
        public async Task<IEnumerable<business.Models.Config>> ObterConfigBalanca(long IDEMPRESA, long idPdv)
        {
            var listaConfiguracao = new List<business.Models.Config>();

            var parametros = new DynamicParameters();
            parametros.Add("@IDPDV", idPdv, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@IDEMPRESA", IDEMPRESA, DbType.String, ParameterDirection.Input);

            //descricaoPDV
            var query = $@"SELECT IDPDV as Id, pdv.* FROM pdv WHERE NMMAQUINA = @IDPDV and IDEMPRESA = @IDEMPRESA";

            var pdv = _dbSession.Connection.Query<PontoVenda>(query, parametros, _dbSession.Transaction).FirstOrDefault();
            if (pdv != null)
            {
                listaConfiguracao.Add(new business.Models.Config("CDMODELOBAL", IDEMPRESA, pdv.CDMODELOBAL.HasValue? pdv.CDMODELOBAL.ToString(): "-1"));
                listaConfiguracao.Add(new business.Models.Config("CDHANDSHAKEBAL", IDEMPRESA, pdv.CDHANDSHAKEBAL.HasValue? pdv.CDHANDSHAKEBAL.ToString():"-1"));
                listaConfiguracao.Add(new business.Models.Config("CDPARITYBAL", IDEMPRESA, pdv.CDPARITYBAL.HasValue? pdv.CDPARITYBAL.ToString():"-1"));
                listaConfiguracao.Add(new business.Models.Config("CDSERIALSTOPBITBAL", IDEMPRESA, pdv.CDSERIALSTOPBITBAL.HasValue? pdv.CDSERIALSTOPBITBAL.ToString():"-1"));
                listaConfiguracao.Add(new business.Models.Config("NUDATABITBAL", IDEMPRESA, pdv.NUDATABITBAL.HasValue?pdv.NUDATABITBAL.ToString():"-1"));
                listaConfiguracao.Add(new business.Models.Config("NUBAUDRATEBAL", IDEMPRESA, pdv.NUBAUDRATEBAL.HasValue? pdv.NUBAUDRATEBAL.ToString():"-1"));
                listaConfiguracao.Add(new business.Models.Config("DSPORTABAL", IDEMPRESA, !string.IsNullOrEmpty(pdv.DSPORTABAL)?pdv.DSPORTABAL.ToString():""));
            }
            
            return listaConfiguracao;
        }

        public async Task<bool> SalvarConfigBalanca(string CDMODELOBAL, string CDHANDSHAKEBAL, string CDPARITYBAL, string CDSERIALSTOPBITBAL,
            string NUDATABITBAL, string NUBAUDRATEBAL, string DSPORTABAL, long IDPDV)
        {
            var parametros = new DynamicParameters();

            parametros.Add("@CDMODELOBAL", CDMODELOBAL, DbType.String, ParameterDirection.Input);
            parametros.Add("@IDPDV", IDPDV, DbType.Int64, ParameterDirection.Input);
            parametros.Add("@CDHANDSHAKEBAL", CDHANDSHAKEBAL, DbType.String, ParameterDirection.Input);
            parametros.Add("@CDPARITYBAL", CDPARITYBAL, DbType.String, ParameterDirection.Input);
            parametros.Add("@CDSERIALSTOPBITBAL", CDSERIALSTOPBITBAL, DbType.String, ParameterDirection.Input);
            parametros.Add("@NUDATABITBAL", NUDATABITBAL, DbType.String, ParameterDirection.Input);
            parametros.Add("@NUBAUDRATEBAL", NUBAUDRATEBAL, DbType.String, ParameterDirection.Input);
            parametros.Add("@DSPORTABAL", DSPORTABAL, DbType.String, ParameterDirection.Input);

            var query = $@" update pdv set CDMODELOBAL = @CDMODELOBAL, CDHANDSHAKEBAL = @CDHANDSHAKEBAL, CDPARITYBAL = @CDPARITYBAL
                             CDSERIALSTOPBITBAL = @CDSERIALSTOPBITBAL, NUDATABITBAL = @NUDATABITBAL, NUBAUDRATEBAL = @NUBAUDRATEBAL,
                             DSPORTABAL = @DSPORTABAL
                             where  IDPDV = @IDPDV";

            return (_dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0);
            
        }
        #endregion

        #region PreVenda
        public async Task<business.Models.Config> ObterConfigPreVenda(long IDEMPRESA)
        {
            return await ObterConfig("PDV_PREVENDA",IDEMPRESA);
        }
        #endregion
    }
}

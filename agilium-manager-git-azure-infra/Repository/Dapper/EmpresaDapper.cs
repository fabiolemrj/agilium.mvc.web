using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Repository.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium_manager_git_azure_infra.Repository.Dapper
{
    public class EmpresaDapper : IEmpresaDapperRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IDapperRepository _dapperRepository;
        private readonly IUtilDapperRepository _utilDapperRepository;
        private readonly IEnderecoDapperRepository _enderecoDapperRepository;
        private readonly DbSession _dbSession;

        public EmpresaDapper(IConfiguration configuration, IDapperRepository dapperRepository, IUtilDapperRepository utilDapperRepository,
            IEnderecoDapperRepository enderecoDapperRepository, DbSession dbSession)
        {
            _configuration = configuration;
            _dapperRepository = dapperRepository;
            _utilDapperRepository = utilDapperRepository;
            _enderecoDapperRepository = enderecoDapperRepository;
            _dbSession = dbSession;
        }

        public async Task<bool> EditarEmpresa(Empresa empresa)
        {
            // 1️⃣ Buscar empresa atual no banco
            var sqlSelect = @"SELECT * FROM empresa WHERE IDEMPRESA = @ID";
            var atual = await _dbSession.Connection.QueryFirstOrDefaultAsync<Empresa>(sqlSelect,new { ID = empresa.Id },_dbSession.Transaction);

            if (atual == null)
                return false; // empresa não encontrada

            // 2️⃣ Construir UPDATE dinâmico
            var camposUpdate = new List<string>();
            var parametros = new DynamicParameters();

            void VerificarAlteracao(string campo, object valorNovo, object valorAntigo)
            {
                if (!Equals(valorNovo, valorAntigo))
                {
                    camposUpdate.Add($"{campo} = @{campo}");
                    parametros.Add($"@{campo}", valorNovo);
                }
            }

            // 3️⃣ Comparar campo por campo
            VerificarAlteracao("NUCNPJ", empresa.NUCNPJ, atual.NUCNPJ);
            VerificarAlteracao("IDENDERECO", empresa.IDENDERECO, atual.IDENDERECO);
            VerificarAlteracao("CDEMPRESA", empresa.CDEMPRESA, atual.CDEMPRESA);
            VerificarAlteracao("NMRZSOCIAL", empresa.NMRZSOCIAL, atual.NMRZSOCIAL);
            VerificarAlteracao("NMFANTASIA", empresa.NMFANTASIA, atual.NMFANTASIA);
            VerificarAlteracao("DSINSCREST", empresa.DSINSCREST, atual.DSINSCREST);
            VerificarAlteracao("DSINSCRESTVINC", empresa.DSINSCRESTVINC, atual.DSINSCRESTVINC);
            VerificarAlteracao("DSINSCRMUN", empresa.DSINSCRMUN, atual.DSINSCRMUN);
            VerificarAlteracao("NMDISTRIBUIDORA", empresa.NMDISTRIBUIDORA, atual.NMDISTRIBUIDORA);
            VerificarAlteracao("NUREGJUNTACOM", empresa.NUREGJUNTACOM, atual.NUREGJUNTACOM);
            VerificarAlteracao("NUCAPARM", empresa.NUCAPARM, atual.NUCAPARM);
            VerificarAlteracao("STMICROEMPRESA", empresa.STMICROEMPRESA, atual.STMICROEMPRESA);
            VerificarAlteracao("STLUCROPRESUMIDO", empresa.STLUCROPRESUMIDO, atual.STLUCROPRESUMIDO);
            VerificarAlteracao("TPEMPRESA", empresa.TPEMPRESA, atual.TPEMPRESA);
            VerificarAlteracao("CRT", empresa.CRT, atual.CRT);
            VerificarAlteracao("IDCSC", empresa.IDCSC, atual.IDCSC);
            VerificarAlteracao("CSC", empresa.CSC, atual.CSC);
            VerificarAlteracao("NUCNAE", empresa.NUCNAE, atual.NUCNAE);
            VerificarAlteracao("IDCSC_HOMOL", empresa.IDCSC_HOMOL, atual.IDCSC_HOMOL);
            VerificarAlteracao("CSC_HOMOL", empresa.CSC_HOMOL, atual.CSC_HOMOL);
            VerificarAlteracao("IDLOJA_SITEMARCADO", empresa.IDLOJA_SITEMARCADO, atual.IDLOJA_SITEMARCADO);
            VerificarAlteracao("CLIENTID_SITEMERCADO", empresa.CLIENTID_SITEMERCADO, atual.CLIENTID_SITEMERCADO);
            VerificarAlteracao("CLIENTSECRET_SITEMERCADO", empresa.CLIENTSECRET_SITEMERCADO, atual.CLIENTSECRET_SITEMERCADO);

            // 4️⃣ Se nenhum campo mudou → não atualiza
            if (!camposUpdate.Any())
                return false;

            // 5️⃣ Montar UPDATE final
            var sqlUpdate = $@"
                                UPDATE empresa
                                SET {string.Join(",", camposUpdate)}
                                WHERE IDEMPRESA = @IDEMPRESA";

            parametros.Add("@IDEMPRESA", empresa.Id);

            // 6️⃣ Executar
            var linhas = await _dbSession.Connection.ExecuteAsync(
            sqlUpdate,
                parametros,
                _dbSession.Transaction
            );

            return linhas > 0;
        }

    }
}

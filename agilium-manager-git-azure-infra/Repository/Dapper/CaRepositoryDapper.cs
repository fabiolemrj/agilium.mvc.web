using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using Dapper;
using KissLog.RestClient.Requests.CreateRequestLog;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlConnector;

//using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace agilium.api.infra.Repository.Dapper
{
    public class CaRepositoryDapper : ICaRepositoryDapper
    {
        protected readonly IConfiguration _configuration;
        private readonly IDapperRepository _dapperRepository;
        private readonly IUtilDapperRepository _utilDapperRepository;
        private readonly DbSession _dbSession;

        public CaRepositoryDapper(IConfiguration configuration, IDapperRepository dapperRepository, IUtilDapperRepository utilDapperRepository, DbSession dbSession)
        {
            _configuration = configuration;
            _dapperRepository = dapperRepository;
            _utilDapperRepository = utilDapperRepository;
            _dbSession = dbSession;
        }

        public string GetConnection()
        {
            var autenticacaoUrl = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
          //  var connection = "Server=localhost;Database=agilium_base;Uid=root;Pwd=123456";
            return autenticacaoUrl;
        }

        #region Perfil
        public async Task AdicionarModelo(CaModelo modelo)
        {
            using (var con = new MySqlConnection(GetConnection())) 
            {
                try
                {
                    con.Open();
                    var query = $@"INSERT INTO camodelo (`idModelo`,`idPerfil`,`idPermissao`,`inc`,`alt`,`exc`,`rel`,`con`)
                                    VALUES (@id,@idPerfil,@idPermissao,@Incluir ,@Alterar,@Excluir,@Relatorio,@Consulta)";
                    con.Execute(query,modelo);
                }
                catch (Exception ex)
                {

                    throw;
                }finally { con.Close(); }

            }
        }

        public async Task AdicionarModelos(IEnumerable<CaModelo> modelos)
        {
            throw new NotImplementedException();
        }

        public Task AdicionarPerfil(CaPerfil perfil)
        {
            throw new NotImplementedException();
        }

        public async Task ApagarModelo(CaModelo modelo)
        {
            using (var con = new MySqlConnection(GetConnection()))
            {
                try
                {
                    con.Open();
                    var query = $@"DELETE FROM camodelo WHERE idperfil = {modelo.idPerfil} and idpermissao ={modelo.idPermissao} ";
                    con.Execute(query, modelo);

                }
                catch (Exception ex)
                {

                    throw;
                }
                finally { con.Close(); }

            };
          
        }

        public async Task RemoverModelos(IEnumerable<CaModelo> modelos)
        {
            throw new NotImplementedException();
        }

        public async Task AdicionarModeloPorPerfil(IEnumerable<CaModelo> modelos)
        {

            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();
                        modelos.ToList().ForEach(mod => {
                            ApagarrModeloTransaction(mod, con);
                            AdicionarModeloTransaction(mod, con);
                        });
                        await AtualizarUsuariosPorPerfil(modelos.FirstOrDefault().idPerfil, modelos, con);

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                    }
                    finally { con.Close(); }

                };

            }

        }

        private async Task AdicionarModeloTransaction(CaModelo modelo, MySqlConnection con)
        {
                    var query = $@"INSERT INTO camodelo (`idModelo`,`idPerfil`,`idPermissao`,`inc`,`alt`,`exc`,`rel`,`con`)
                                    VALUES (@id,@idPerfil,@idPermissao,@Incluir ,@Alterar,@Excluir,@Relatorio,@Consulta)";
                    con.Execute(query, modelo);
        }
        private async Task ApagarrModeloTransaction(CaModelo modelo, MySqlConnection con)
        {
            var query = $@"DELETE FROM camodelo WHERE idperfil = {modelo.idPerfil} and idpermissao ={modelo.idPermissao} ";
            con.Execute(query, modelo);
        }

        private async Task AtualizarUsuariosPorPerfil(long idPerfil, IEnumerable<CaModelo> modelos,MySqlConnection con)
        {
            var query = $@"SELECT * FROM ca_usuarios where idPerfil = {idPerfil}";
            var usuarios = con.Query<Usuario>(query).ToList();


            usuarios.ForEach(usu =>
            {
                query = $@"DELETE FROM aspnetuserclaims where UserId`='{usu.idUserAspNet}'";
                con.Execute(query);

                modelos.ToList().ForEach(mod =>
                {
                    var claimValueIncluir = (mod.Incluir == "S") ? "S" : "N";
                    var claimValueAlterar = (mod.Alterar == "S") ? "S" : "N";
                    var claimValueExcluir = (mod.Excluir == "S") ? "S" : "N";
                    var claimValueRelatorio = (mod.Relatorio == "S") ? "S" : "N";
                    var claimValueConsulta = (mod.Consulta == "S") ? "S" : "N";

                    query = $@"INSERT INTO aspnetuserclaims (`UserId`,`ClaimType`,`ClaimValue`) VALUES 
                            ('{usu.idUserAspNet}','{mod.CaPermissaoItem.Descricao}','{claimValueIncluir}'),
                            ('{usu.idUserAspNet}','{mod.CaPermissaoItem.Descricao}','{claimValueAlterar}'),
                            ('{usu.idUserAspNet}','{mod.CaPermissaoItem.Descricao}','{claimValueExcluir}'),
                            ('{usu.idUserAspNet}','{mod.CaPermissaoItem.Descricao}','{claimValueRelatorio}'),
                            ('{usu.idUserAspNet}','{mod.CaPermissaoItem.Descricao}','{claimValueRelatorio}')";

                    con.Execute(query);
                });
            });

        }

        private async Task AtualizarUsuarioPorPerfil(long idPerfil, string idUserAspNet, MySqlConnection con)
        {
            var query = $@"update ca_usuarios set idPerfil = {idPerfil} where idUserAspNet = '{idUserAspNet}'";
            con.Execute(query); 

            query = $@"SELECT `camodelo`.`idModelo`,`camodelo`.`idPerfil`,`camodelo`.`idPermissao`,`camodelo`.`inc` as incluir,`camodelo`.`alt` as alterar,
                        `camodelo`.`exc` as excluir,`camodelo`.`rel` as relatorio,`camodelo`.`con` as consulta,
                        `capermissaoitem`.`idpermissao`,`capermissaoitem`.`descricao`,`capermissaoitem`.`situacao`
                    FROM `camodelo`
                    inner join `capermissaoitem` on `capermissaoitem`.`idpermissao` = `camodelo`.`idPermissao`
                    WHERE idperfil = {idPerfil}";
            var modelos = con.Query<CaModelo, CaPermissaoItem, CaModelo>(query, (caModelo,caPermissaoItem) =>{
                caModelo.CaPermissaoItem = caPermissaoItem;
                return caModelo;
                },splitOn: "idpermissao").ToList();

            query = $@"DELETE FROM aspnetuserclaims where UserId='{idUserAspNet}'";
            con.Execute(query);

            modelos.ToList().ForEach(mod =>
            {
                query = "";
                var insertValues = "";
                if (mod.Incluir == "S")
                    insertValues += (!string.IsNullOrEmpty(insertValues)?",":" ") + $"('{idUserAspNet}', '{mod.CaPermissaoItem.Descricao}', 'INCLUIR')";

                if (mod.Alterar == "S")
                    insertValues += (!string.IsNullOrEmpty(insertValues) ? "," : " ") + $"('{idUserAspNet}', '{mod.CaPermissaoItem.Descricao}', 'ALTERAR')";

                if (mod.Excluir== "S")
                    insertValues += (!string.IsNullOrEmpty(insertValues) ? "," : " ") + $"('{idUserAspNet}', '{mod.CaPermissaoItem.Descricao}', 'EXCLUIR')";

                if (mod.Relatorio == "S")
                    insertValues += (!string.IsNullOrEmpty(insertValues) ? "," : " ") + $"('{idUserAspNet}', '{mod.CaPermissaoItem.Descricao}', 'RELATORIO')";

                if (mod.Consulta == "S")
                    insertValues += (!string.IsNullOrEmpty(insertValues) ? "," : " ") + $"('{idUserAspNet}', '{mod.CaPermissaoItem.Descricao}', 'CONSULTA')";

                if(!string.IsNullOrEmpty(insertValues))
                {
                    query = $@"INSERT INTO aspnetuserclaims (`UserId`,`ClaimType`,`ClaimValue`) VALUES {insertValues}";

                    con.Execute(query);
                }
                   
            });
        }

        public async Task<IEnumerable<CaPerfil>> ObterPerfil(long? idPerfil)
        {
            var modelos = new List<CaPerfil>();
            using (var con = new MySqlConnection(GetConnection()))
            {
                try
                {
                    //if (!idPerfil.HasValue)
                        idPerfil = 0;

                    con.Open();
                    var query = $@"SELECT  idperfil as id,descricao,situacao,idempresa FROM caperfil WHERE idperfil <> {idPerfil}";
                    modelos = con.Query<CaPerfil>(query).ToList();
                    return modelos;
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally { 
                    con.Close();
                   
                }
                return modelos;
            };
        }

      

        public async Task AtualizarUsuarioPorPerfil(long idPerfil, string idUserAspNet)
        {
            using (var scope = new TransactionScope())
            {
                using (var con = new MySqlConnection(GetConnection()))
                {
                    try
                    {
                        con.Open();
                        await AtualizarUsuarioPorPerfil(idPerfil, idUserAspNet, con);

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                    }
                    finally { con.Close(); }

                };

            }
        }


        #endregion

        #region Ca Manager

        public async Task<bool> RemoverPermissoesPorPerfil(int idPerfil)
        {
            var query = $@"delete from ca_permissoes where id_perfil = @IDPERFIL";
            var parametros = new DynamicParameters();
            parametros.Add("@IDPERFIL", idPerfil, DbType.Int32, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) >= 0;
        }

        public async Task<bool> AdicionarPermissaoPorPerfil(CaPermissaoManager permissao)
        {
            var query = $@"Insert into ca_permissoes(id_perfil, id_area) values (@IDPERFIL, @IDAREA)";
            var parametros = new DynamicParameters();
            parametros.Add("@IDPERFIL", permissao.IdPerfil, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@IDAREA", permissao.IdArea, DbType.Int32, ParameterDirection.Input);

            return _dbSession.Connection.Execute(query, parametros, _dbSession.Transaction) > 0;
        }

        public async Task<bool> UsuarioTemPermissaoAcesso(string idUsuarioAspNet, int idTag)
        {
            var query = $@"Select a.*,a.id_area as Id from ca_areas a 
                                inner join ca_permissoes perm on a.id_area = perm.id_area 
                                inner join ca_usuarios u on u.id_perfil = perm.id_perfil
                                where idUserAspNet = @idUserAspNet
                                and idtag = @idTag";
            var parametros = new DynamicParameters();
            parametros.Add("@idUserAspNet", idUsuarioAspNet, DbType.String, ParameterDirection.Input);
            parametros.Add("@idTag", idTag, DbType.Int32, ParameterDirection.Input);

            return _dbSession.Connection.Query<CaAreaManager>(query, parametros, _dbSession.Transaction).Any();
            
        }
        public async Task<IEnumerable<Empresa>> ObterEmpresasAssociadasPorUsuario(string idUsuarioAspNet)
        {
            var query = $@"select e.IDEMPRESA as ID,e.* 
                            from agilium_base.emp_auth au
                            inner join empresa e on e.IDEMPRESA = au.IDEMPRESA
                            inner join ca_usuarios u on u.id_usuario = au.IDUSUARIO
                            where u.idUserAspNet = @idUserAspNet";
            var parametros = new DynamicParameters();
            parametros.Add("@idUserAspNet", idUsuarioAspNet, DbType.String, ParameterDirection.Input);

            return _dbSession.Connection.Query<Empresa>(query, parametros, _dbSession.Transaction);
        }


        #endregion
    }
}

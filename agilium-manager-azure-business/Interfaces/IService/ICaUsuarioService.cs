using agilium.api.business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    /// <summary>
    /// Servico para gerenciamento de CaUsuarioIdentity (IdentityUser para login).
    /// </summary>
    public interface ICaUsuarioService : System.IDisposable
    {
        #region CRUD Basico

        Task<bool> Adicionar(CaUsuarioIdentity usuario);
        Task<bool> Atualizar(CaUsuarioIdentity usuario);
        Task<bool> Remover(string id);
        Task<CaUsuarioIdentity> ObterPorId(string id);
        Task<List<CaUsuarioIdentity>> ObterTodos();
        Task<PagedResult<CaUsuarioIdentity>> ObterPaginado(string filtro, int page = 1, int pageSize = 15);

        #endregion

        #region Consultas Especificas

        Task<CaUsuarioIdentity> ObterPorUserName(string userName);
        Task<CaUsuarioIdentity> ObterPorEmail(string email);
        Task<CaUsuarioIdentity> ObterPorCpf(string cpf);
        Task<CaUsuarioIdentity> ObterPorUserAspNetId(string idUserAspNet);
        Task<List<CaUsuarioIdentity>> ObterPorNome(string nome);
        Task<PagedResult<CaUsuarioIdentity>> ObterPorNomePaginado(string nome, int page = 1, int pageSize = 15);

        #endregion

        #region Ativacao / Desativacao

        Task<bool> Ativar(string id);
        Task<bool> Desativar(string id);

        #endregion

        #region Empresas Associadas

        Task<List<EmpresaAuth>> ObterEmpresasPorUsuario(long id);
        Task<EmpresaAuth> ObterEmpresaPorId(long idUsuario, long idEmpresa);
        Task<List<Empresa>> ObterEmpresasDisponiveisAssociacao(long idUsuario);
        Task<bool> AssociarEmpresa(long idUsuario, long idEmpresa);
        Task<bool> DesassociarEmpresa(long idUsuario, long idEmpresa);
        Task<bool> DesassociarTodasEmpresas(long idUsuario);

        #endregion

        #region Transacional

        Task Salvar();

        #endregion
    }
}

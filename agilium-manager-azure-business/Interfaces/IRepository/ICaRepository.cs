using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface ICaModeloRepository : IRepository<CaModelo>
    {
    }
    public interface ICaPerfilRepository : IRepository<CaPerfil>
    {
    }

    public interface ICaPermissaoItemRepository : IRepository<CaPermissaoItem>
    {
    }

    #region CaManager
    public interface ICaPermissaoManagerRepository : IRepository<CaPermissaoManager>
    {
    }

    public interface ICaPerfilManagerRepository : IRepository<CaPerfiManager>
    {
    }

    public interface ICaAreaManagerRepository : IRepository<CaAreaManager>
    {
    }

    #endregion

    public interface ICaRepositoryDapper
    {
        Task AdicionarPerfil(CaPerfil perfil);
        Task AdicionarModelo(CaModelo modelo);
        Task ApagarModelo(CaModelo modelo);
        Task AdicionarModelos(IEnumerable<CaModelo> modelos);
        Task RemoverModelos(IEnumerable<CaModelo> modelos);
        Task AdicionarModeloPorPerfil(IEnumerable<CaModelo> modelos);
        Task<IEnumerable<CaPerfil>> ObterPerfil(long? idPerfil);
        Task AtualizarUsuarioPorPerfil(long idPerfil, string idUserAspNet);
        Task<bool> RemoverPermissoesPorPerfil(int idPerfil);
        Task<bool> AdicionarPermissaoPorPerfil(CaPermissaoManager permissao);
        Task<bool> UsuarioTemPermissaoAcesso(string idUsuarioAspNet, int idTag);
        Task<IEnumerable<Empresa>> ObterEmpresasAssociadasPorUsuario(string idUsuarioAspNet);
    }
}

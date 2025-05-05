

using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.CaManagerViewModel;
using agilium.webapp.manager.mvc.ViewModels.ControleAcesso;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IControleAcessoService
    {
        #region Perfil
        Task<ResponseResult> AdicionarPerfil(CreateEditPerfilViewModel perfil);
        Task<ResponseResult> AtualizarPerfil(CreateEditPerfilViewModel perfil);
        Task<ResponseResult> AtivarPerfil(long idPerfil);
        Task<ResponseResult> ApagarPerfil(long idPerfil);
        Task<PerfilIndexViewModel> ObterPerfilPorId(long idPerfil);
        Task<PerfilIndexViewModel> ObterPerfilPorDescricao(string descricao);
        Task<IEnumerable<PerfilIndexViewModel>> ObterTodosPerfis();
        Task<PagedViewModel<PerfilIndexViewModel>> ObterPerfilPorDescricao(long idEmpresa,string descricao, int page = 1, int pageSize = 15);

        #endregion

        #region PermissaoItem
        Task<ResponseResult> AdicionarPermissaoItem(CreateEditPermissaoItemViewModel caPermissaoItem);
        Task<ResponseResult> ApagarPermissaoItem(long id);
        Task<IEnumerable<PermissaoItemIndexViewModel>> ObterTodosListaPermissao();
        Task<PagedViewModel<PermissaoItemIndexViewModel>> ObterPermissaoItemPorDescricao(string descricao, int page = 1, int pageSize = 15);

        #endregion

        #region Modelo
        Task<ResponseResult> AdicionarModeloItem(CreateModeloViewModel caModelo);
        Task<ResponseResult> ApagarModeloItem(long idModelo);
        Task<ResponseResult> ApagarModelosPorPerfil(long idModelo);
        Task<CreateModeloViewModel> ObterModelosPorPerfil(long idPerfil);
        Task<IEnumerable<CreateModeloItemViewModel>> ObterUsuariosPorPerfil(long idPerfil);

        #endregion

        #region Ca Manager
        Task<PagedViewModel<CaPerfilManagerViewModel>> ObterPerfilManagerPorDescricao(string descricao, int page = 1, int pageSize = 15);
        Task<ResponseResult> AdicionarPerfil(CaPerfilManagerViewModel perfil);
        Task<ResponseResult> AtualizarPerfil(CaPerfilManagerViewModel perfil);
        Task<CaPerfilManagerViewModel> ObterPerfilManagerPorId(long idPerfil);
        Task<CaPermissaoPerfilViewModel> ObterPermissaoPorPerfil(long idPerfil);
        Task<ResponseResult> AdicionarPermissoes(List<CaAreaManagerSalvarViewModel> permissoes);
        Task<List<CaPerfilManagerViewModel>> ObterTodosPerfilManager();
        #endregion
    }

}

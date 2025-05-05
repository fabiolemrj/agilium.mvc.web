using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface ICaService: IDisposable
    {
        Task<int> Salvar();

        #region Perfil
        Task<bool> AdicionarPerfil(CaPerfil caPerfil);
        Task<bool> AtualizarPerfil(CaPerfil caPerfil);
        Task<bool> AtivarPerfil(long idPerfil);
        Task<bool> ApagarPerfil(long idPerfil);
        Task<CaPerfil> ObterPerfilPorId(long idPerfil);
        Task<CaPerfil> ObterCompletoPerfilPorId(long idPerfil);
        Task<CaPerfil> ObterPerfilPorDescricao(string descricao);
        Task<IEnumerable<CaPerfil>> ObterTodosPerfis();
        Task<PagedResult<CaPerfil>> ObterUsuariosPorDescricao(long idempresa, string descricao, int page = 1, int pageSize = 15);
        Task<bool> AtivarPermissao(long id);
        void AtualizarPerfilSincr(CaPerfil perfil);

        #endregion

        #region PermissaoItem
        Task<bool> AdicionarPermissaoItem(CaPermissaoItem caPermissaoItem);
        Task<bool> ApagarPermissaoItem(long id);
        Task<IEnumerable<CaPermissaoItem>> ObterTodosListaPermissao();
        Task<PagedResult<CaPermissaoItem>> ObterPermissaoItemPorDescricao(string descricao, int page = 1, int pageSize = 15);

        #endregion

        #region Modelo
        Task<bool> AdicionarModeloItem(CaModelo caModelo);
        Task<bool> ApagarModeloItem(long idModelo);
        Task<bool> ApagarModeloItemInd(long idPermissao, long idPerfil);
        Task<bool> ApagarModelosPorPerfil(long idModelo);
        Task<bool> ApagarModelosPorPerfil(CaModelo caModelo);
        Task<IEnumerable<CaModelo>> ObterModelosPorPerfil(long idPerfil);
        Task<IEnumerable<Usuario>> ObterUsuariosPorPerfil(long idPerfil);
        void ApagarModelos(IEnumerable<CaModelo> modelos);
        void AdicionarModeloSincronizaa(CaModelo caModelo);
        void AtualizarModeloSincronizaa(CaModelo caModelo);
        void ApagarModelos(CaModelo modelo);
        Task DisposeAppUser();
        #endregion

        #region Ca Manager
        Task<IEnumerable<CaAreaManager>> ObterTodasCaAreas();
        Task<IEnumerable<CaPerfiManager>> ObterTodosCaPerfilPorDescricao(string descricao);
        Task<CaPerfiManager> ObterPerfilManagerPorId(int id);
        Task<CaPerfiManager> ObterCompletoPerfilManagerPorId(int idPerfil);
        Task<PagedResult<CaPerfiManager>> ObterPerfilPorDescricaoPaginacao(string descricao, int page = 1, int pageSize = 15);
        Task<bool> AdicionarPerfil(CaPerfiManager perfil);
        Task<bool> AtualizarPerfil(CaPerfiManager perfil);
        Task<bool> AdicionarPermissoes(List<CaPermissaoManager> permissoes);
        Task<IEnumerable<CaPerfiManager>> ObterTodosCaPerfilManager();
        #endregion
    }
}

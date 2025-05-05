
using agilium.webapp.manager.mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IUsuarioService
    {
        Task<UserFull> ObterPorUsuarioPorUserId(string id);
        Task<ResponseResult> Atualizar(string id, UserFull usuario);
        Task<bool> Remover(string id);
        Task<List<UserFull>> ObterUsuarios();
        Task<PagedViewModel<UserFull>> ObterTodosUsuarios(int page = 1, int pageSize = 15);
        Task<List<UserFull>> ObterUsuarios(string nome);
        Task<PagedViewModel<UserFull>> ObterUsuariosPorNome(string nome, int page = 1, int pageSize = 15);
        Task<UserFull> ObterPorUsuarioPorId(string id);
        Task<ResponseResult> AtualizarClaimUsuario(string id, List<ClaimSelecionada> claimSelecionadas);
        Task<ResponseResult> ObterClaimsPorUsuario(string id);
        Task<ResponseResult> DuplicarClaimPorUsuario(DuplicarUsuarioRetornoViewModel viewModel);
        Task<ResponseResult> ObterUsuarioComClaimsPorId(string id);
        Task<ResponseResult> RemoverClaimInvidualPorUsuario(ClaimEditaAcaoIndividualPorUsuario viewModel);
        Task<ResponseResult> DesativarUsuario(string id);
        Task<ResponseResult> AtivarUsuario(string id);
        Task<ResponseResult> AtualizarFoto(UsuarioFotoViewModel usuarioFoto);
        Task<ResponseResult> ObterFotoUsuarioPorId(string id);

        Task<ResponseResult> ObterEmpresasPorUsuario(long id);
        Task<ResponseResult> ObterEmpresaPorId(long idUsuario, long idEmpresa);
        Task<ResponseResult> Adicionar(EmpresaUsuarioViewModel empresaAuth);
        Task<ResponseResult> Adicionar(long idUsuario, List<EmpresaUsuarioSelecaoViewModel> empresaAuth);
        Task<ResponseResult> ObterEmpresasDispiniveisPorUsuario(long idUsuario);
        Task<bool> Remover(long idUsuario, long idEmpresa);
        Task<SelecionarPerfilViewModel> ObterPorUsuarioPerfis(string id);
        Task<ResponseResult> SelecionarPerfil(SelecionarPerfilViewModel model);
    }
}

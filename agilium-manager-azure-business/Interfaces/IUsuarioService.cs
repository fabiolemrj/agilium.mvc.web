using agilium.api.business.Models;
using agilium.api.business.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces
{
    public interface IUsuarioService: IDisposable
    {
        Task<bool> Adicionar(Usuario usuario);
        Task<bool> Atualizar(Usuario usuario);
        Task<bool> AtualizarFoto(Usuario usuario);
        Task<bool> Remover(long id);

        Task<Usuario> ObterPorUsuarioPorCpf(string cpf);
        Task<bool> DesativarUsuario(long id);
        Task<bool> AtivarUsuario(long id);
        Task<Usuario> ObterPorUsuarioPorId(long id);
        Task<Usuario> ObterPorUsuarioAspNetPorId(string id);
        Task<List<Usuario>> ObterTodosUsuarios();
        Task<PagedResult<Usuario>> ObterTodosUsuarios(int page = 1, int pageSize = 15);
        Task<List<Usuario>> ObterUsuariosPorNome(string nome);
        Task<PagedResult<Usuario>> ObterUsuariosPorNome(string nome, int page = 1, int pageSize = 15);


        Task<List<EmpresaAuth>> ObterEmpresasPorUsuario(long id);
        Task<EmpresaAuth> ObterEmpresaPorId(long idUsuario, long idEmpresa);
        Task Adicionar(EmpresaAuth empresaAuth);
        Task Atualizar(EmpresaAuth empresaAuth);
        Task AdicionarLista(IEnumerable<EmpresaAuth> empresaAuth);
        Task AtualizarLista(IEnumerable<EmpresaAuth> empresaAuth);
        Task<List<Empresa>> ObterEmpresasDisponiveisAssociacao( long idUsuario);
        Task<bool> Remover(long idUsuario, long idEmpresa);
        Task<bool> RemoverEmpresaUsuarioPorUsuario(long idUsuario);
        Task RemoverLista(IEnumerable<EmpresaAuth> empresaAuth);

        Task Salvar();
    }
}

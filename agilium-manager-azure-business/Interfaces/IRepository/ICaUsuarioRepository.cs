using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    /// <summary>
    /// Interface de repositorio para CaUsuarioIdentity (IdentityUser).
    /// Nao herda de IRepository&lt;T&gt; pois este exige constraint Entity.
    /// Define os mesmos metodos de forma independente.
    /// </summary>
    public interface ICaUsuarioRepository : IDisposable
    {
        Task Adicionar(CaUsuarioIdentity entity);
        Task AdicionarSemSalvar(CaUsuarioIdentity entity);
        Task AdicionarLista(IEnumerable<CaUsuarioIdentity> entity);
        Task<CaUsuarioIdentity> ObterPorId(string id);
        Task<CaUsuarioIdentity> ObterCompletoPorId(string id, params string[] includes);
        Task<List<CaUsuarioIdentity>> ObterTodos();
        Task Atualizar(CaUsuarioIdentity entity);
        Task AtualizarSemSalvar(CaUsuarioIdentity entity);
        Task AtualizarLista(IEnumerable<CaUsuarioIdentity> entity);
        void AtualizarSincrona(CaUsuarioIdentity entity);
        Task AtualizarComSetValues(CaUsuarioIdentity entity, object model);
        void RemoverSincrona(CaUsuarioIdentity entity);
        void AdicionarSincrona(CaUsuarioIdentity entity);
        Task Remover(string id);
        Task RemoverSemSalvar(CaUsuarioIdentity entity);
        Task RemoverSemSalvar(string id);
        Task RemoverSemSalvar(IEnumerable<CaUsuarioIdentity> entity);
        Task<IEnumerable<CaUsuarioIdentity>> Buscar(Expression<Func<CaUsuarioIdentity, bool>> predicate);
        Task<IEnumerable<CaUsuarioIdentity>> Buscar(Expression<Func<CaUsuarioIdentity, bool>> predicated, params string[] includes);
        Task<IEnumerable<CaUsuarioIdentity>> Obter(Expression<Func<CaUsuarioIdentity, bool>> predicated, params string[] includes);
        Task<int> SaveChanges();
        Task<bool> Existe(Expression<Func<CaUsuarioIdentity, bool>> predicate);
        Task<string> GerarCodigo(string sql);
        Task<CaUsuarioIdentity> GerarCodigoPorSql(string sql);
        Task Atualizar2(CaUsuarioIdentity entity, object key);
    }
}

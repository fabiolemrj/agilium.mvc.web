using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task Adicionar(TEntity entity);
        Task AdicionarSemSalvar(TEntity entity);
        Task AdicionarLista(IEnumerable<TEntity> entity);
        Task<TEntity> ObterPorId(long id);
        Task<TEntity> ObterCompletoPorId(long id, params string[] includes);
        Task<List<TEntity>> ObterTodos();
        Task Atualizar(TEntity entity);
        Task AtualizarSemSalvar(TEntity entity);
        Task AtualizarLista(IEnumerable<TEntity> entity);
        void AtualizarSincrona(TEntity entity);
        void RemoverSincrona(TEntity entity);
        void AdicionarSincrona(TEntity entity);
        Task Remover(long id);
        Task RemoverSemSalvar(TEntity entity);
        Task RemoverSemSalvar(long id);
        Task RemoverSemSalvar(IEnumerable<TEntity> entity);
        Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicated, params string[] includes);
        Task<IEnumerable<TEntity>> Obter(Expression<Func<TEntity, bool>> predicated, params string[] includes);
        Task<int> SaveChanges();
        Task<bool> Existe(Expression<Func<TEntity, bool>> predicate);
        Task<string> GerarCodigo(string sql);
        Task<TEntity> GerarCodigoPorSql(string sql);
    }

    public interface IUtilDapperRepository
    {
        Task<long> GerarUUID();
        Task<string> ConfigRetornaValor(string valor, long? idEmpresa);
        Task<string> GerarCodigo(string sql);
        Task<int> GerarIdInt(string generator);

    }
}

using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using agilium.api.infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace agilium.api.infra.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly AgiliumContext Db;
        protected readonly DbSet<TEntity> DbSet;

        protected Repository(AgiliumContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        #region ===================== CONSULTAS =====================

        /// <summary>
        /// Consulta sem tracking: ideal para listagens e consultas somente leitura.
        /// </summary>
        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking()
                              .Where(predicate)
                              .ToListAsync();
        }

        /// <summary>
        /// Consulta com tracking: ideal para atualização.
        /// </summary>
        public async Task<TEntity> ObterPorId(long id)
        {
            return await DbSet.FindAsync(id);
        }

        /// <summary>
        /// Consulta completa (sem tracking + includes).
        /// </summary>
        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            IQueryable<TEntity> query = DbSet.AsNoTracking().Where(predicate);

            foreach (var inc in includes)
                query = query.Include(inc);

            return await query.ToListAsync();
        }

        public async Task<TEntity> ObterCompletoPorId(long id, params string[] includes)
        {
            IQueryable<TEntity> query = DbSet.AsNoTracking().Where(x => x.Id == id);

            foreach (var inc in includes)
                query = query.Include(inc);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> Obter(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            IQueryable<TEntity> query = DbSet.Where(predicate);

            foreach (var inc in includes)
                query = query.Include(inc);

            return await query.ToListAsync();
        }

        public async Task<bool> Existe(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().AnyAsync(predicate);
        }

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        }

        #endregion

        #region ===================== COMANDOS =====================

        public async Task Adicionar(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await SaveChanges();
        }

        public async Task AdicionarSemSalvar(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task AdicionarLista(IEnumerable<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// Atualização segura: evita problemas de entidades detachadas.
        /// </summary>
        public async Task Atualizar(TEntity entity)
        {
            await AtualizarSemSalvar(entity);
            await SaveChanges();
        }

        public async Task AtualizarSemSalvar(TEntity entity)
        {
            var entry = Db.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                var noBanco = await DbSet.FindAsync(entity.Id);

                if (noBanco != null)
                {
                    Db.Entry(noBanco).CurrentValues.SetValues(entity);
                }
                else
                {
                    DbSet.Attach(entity);
                    entry.State = EntityState.Modified;
                }
            }
        }

        public async Task AtualizarComSetValues(TEntity entity, object model)
        {
            var entry = Db.Entry(entity);
            entry.CurrentValues.SetValues(model);
        }


        public async Task AtualizarLista(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                await AtualizarSemSalvar(entity);
        }

        public async Task Remover(long id)
        {
            DbSet.Remove(new TEntity { Id = id });
            await SaveChanges();
        }

        public async Task RemoverSemSalvar(long id)
        {
            DbSet.Remove(new TEntity { Id = id });
        }

        public async Task RemoverSemSalvar(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public async Task RemoverSemSalvar(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        #endregion

        #region ===================== SAVE =====================

        public async Task<int> SaveChanges()
        {
            try
            {
                return await Db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException(
                    "Erro de concorrência ao salvar a entidade. Nenhuma linha foi afetada. Dados podem ter sido alterados por outro processo.",
                    ex
                );
            }
        }

        #endregion

        #region ===================== OUTROS =====================

        public void Dispose()
        {
            Db?.Dispose();
        }

        public virtual Task<string> GerarCodigo(string sql)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TEntity> GerarCodigoPorSql(string sql)
        {
            throw new NotImplementedException();
        }

        public void AtualizarSincrona(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public void RemoverSincrona(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void AdicionarSincrona(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public Task Atualizar2(TEntity entity, object key)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

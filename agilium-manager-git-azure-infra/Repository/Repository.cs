using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using agilium.api.infra.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using static FluentValidation.Validators.IPredicateValidator;

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

        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<TEntity> ObterPorId(long id)
        {
            var objeto = await DbSet.FindAsync(id);
            
            return objeto;
        }

        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicated, params string[] includes)
        {
            var result = Db.Set<TEntity>().AsNoTracking().Where(predicated);

            foreach (var item in includes)
            {
                result = result.AsNoTracking().Include(item);
            }

            return await result.AsNoTracking().ToListAsync(); ;
        }
        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task Adicionar(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChanges();
        }

        public virtual async Task Atualizar(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChanges();
        }

        public virtual async Task Remover(long id)
        {
            DbSet.Remove(new TEntity { Id = id });
            await SaveChanges();
        }

        public virtual async Task RemoverSemSalvar(TEntity entity)
        {
            DbSet.Remove(entity);            
        }

        public void Dispose()
        {
            Db?.Dispose();
        }

        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync();
        }

        public async Task AdicionarSemSalvar(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public async Task AtualizarSemSalvar(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public async Task RemoverSemSalvar(long id)
        {
            var entity = new TEntity { Id = id };
            Db.Remove(entity);
        }

        public async Task<TEntity> ObterCompletoPorId(long id, params string[] includes)
        {
            var result = Db.Set<TEntity>().AsNoTracking().Where(x=>x.Id == id);
            
            foreach (var item in includes)
            {
                result = result.AsNoTracking().Include(item);
            }

            return result.AsNoTracking().ToList().FirstOrDefault();
        }

        public async Task<IEnumerable<TEntity>> Obter(Expression<Func<TEntity, bool>> predicated, params string[] includes)
        {
            var result = Db.Set<TEntity>().AsNoTracking().Where(predicated);
      
            foreach (var item in includes)
            {
                result = result.AsNoTracking().Include(item);
            }

            return await result.AsNoTracking().ToListAsync();
        }

        public async Task AdicionarLista(IEnumerable<TEntity> entity)
        {
            await DbSet.AddRangeAsync(entity);
        }

        public async Task AtualizarLista(IEnumerable<TEntity> entity)
        {
            DbSet.UpdateRange(entity);
        }

        public async Task RemoverSemSalvar(IEnumerable<TEntity> entity)
        {
            DbSet.RemoveRange(entity);
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

        public async Task<bool> Existe(Expression<Func<TEntity, bool>> predicated)
        {
            return await Db.Set<TEntity>().AsNoTracking().AnyAsync(predicated);
        }

        public virtual async Task<string> GerarCodigo(string sql)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> GerarCodigoPorSql(string sql)
        {
            throw new NotImplementedException();
        }
    }
}

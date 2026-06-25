using agilium.api.business.Interfaces.IRepository;
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
    /// <summary>
    /// Repositorio especifico para CaUsuarioIdentity (IdentityUser).
    /// Nao herda de Repository&lt;T&gt; porque a constraint exige Entity.
    /// Usa o DbContext diretamente para operacoes no DbSet&lt;CaUsuarioIdentity&gt;.
    /// </summary>
    public class CaUsuarioRepository : ICaUsuarioRepository, IDisposable
    {
        private readonly AgiliumContext _context;
        private readonly DbSet<CaUsuarioIdentity> _dbSet;

        public CaUsuarioRepository(AgiliumContext context)
        {
            _context = context;
            _dbSet = context.Set<CaUsuarioIdentity>();
        }

        public async Task Adicionar(CaUsuarioIdentity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AdicionarSemSalvar(CaUsuarioIdentity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AdicionarLista(IEnumerable<CaUsuarioIdentity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<CaUsuarioIdentity> ObterPorId(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<CaUsuarioIdentity> ObterCompletoPorId(string id, params string[] includes)
        {
            IQueryable<CaUsuarioIdentity> query = _dbSet;
            foreach (var include in includes)
                query = query.Include(include);
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<CaUsuarioIdentity>> ObterTodos()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task Atualizar(CaUsuarioIdentity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarSemSalvar(CaUsuarioIdentity entity)
        {
            _dbSet.Update(entity);
        }

        public async Task AtualizarLista(IEnumerable<CaUsuarioIdentity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void AtualizarSincrona(CaUsuarioIdentity entity)
        {
            _dbSet.Update(entity);
        }

        public async Task AtualizarComSetValues(CaUsuarioIdentity entity, object model)
        {
            var entry = _context.Entry(entity);
            entry.CurrentValues.SetValues(model);
            await Task.CompletedTask;
        }

        public void RemoverSincrona(CaUsuarioIdentity entity)
        {
            _dbSet.Remove(entity);
        }

        public void AdicionarSincrona(CaUsuarioIdentity entity)
        {
            _dbSet.Add(entity);
        }

        public async Task Remover(string id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoverSemSalvar(CaUsuarioIdentity entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task RemoverSemSalvar(string id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public async Task RemoverSemSalvar(IEnumerable<CaUsuarioIdentity> entities)
        {
            _dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<CaUsuarioIdentity>> Buscar(Expression<Func<CaUsuarioIdentity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<CaUsuarioIdentity>> Buscar(Expression<Func<CaUsuarioIdentity, bool>> predicate, params string[] includes)
        {
            IQueryable<CaUsuarioIdentity> query = _dbSet.Where(predicate);
            foreach (var include in includes)
                query = query.Include(include);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<CaUsuarioIdentity>> Obter(Expression<Func<CaUsuarioIdentity, bool>> predicate, params string[] includes)
        {
            IQueryable<CaUsuarioIdentity> query = _dbSet.Where(predicate);
            foreach (var include in includes)
                query = query.Include(include);
            return await query.ToListAsync();
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> Existe(Expression<Func<CaUsuarioIdentity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<string> GerarCodigo(string sql)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<CaUsuarioIdentity> GerarCodigoPorSql(string sql)
        {
            return await Task.FromResult<CaUsuarioIdentity>(null);
        }

        public async Task Atualizar2(CaUsuarioIdentity entity, object key)
        {
            var existing = await _dbSet.FindAsync(key);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
            }
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

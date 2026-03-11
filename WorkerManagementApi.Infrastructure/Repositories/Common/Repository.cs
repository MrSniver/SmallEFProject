using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Domain.Common.Entities;
using WorkerManagementApi.Infrastructure.Context;

namespace WorkerManagementApi.Infrastructure.Repositories.Common
{
    public class Repository<T>: IRepository<T> where T : class
    {
        protected readonly WorkerManApiContext _context;
        
        public Repository(WorkerManApiContext context)
        {
           _context = context;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken) 
        {
            return await _context.Set<T>().CountAsync(where, cancellationToken);
        }

        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            Entity tmp = entity as Entity;
            tmp.IsDeleted = true;
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> RemoveRangeAsync(List<T> entities, CancellationToken cancellationToken)
        {
            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task<List<T>> RemoveRangeWhereAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken,
            params Expression<Func<T, object>>[] includes)
        {
            var entites = GetWhereAsync(where, includes).Result.ToList();
            _context.Set<T>().RemoveRange(entites);
            await _context.SaveChangesAsync();
            return entites;
        }

        public async Task<bool> ExistAsync(T entity, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().AnyAsync(cancellationToken);
        }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var keyValues = new object[] { id };
            return await _context.Set<T>().FindAsync(keyValues, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> where,
            params Expression<Func<T, object>>[] includes)
        {
            var entites = _context.Set<T>().AsQueryable();

            if (where != null)
            {
                entites = entites.Where(where);
            }

            if (includes != null)
            {
                entites = includes.Aggregate(entites, (query, path) => query.Include(path));
            }

            return await entites.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().Where(where).ToListAsync(cancellationToken);
        }

        public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}

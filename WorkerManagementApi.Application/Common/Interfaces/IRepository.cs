using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WorkerManagementApi.Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<int> CountAsync(Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default);
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task<List<T>> RemoveRangeAsync(List<T> entites, CancellationToken cancellationToken = default);
        Task<List<T>> RemoveRangeWhereAsync(Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes);
        Task<bool> ExistAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> where = null, params Expression<Func<T, object>>[] includes);
        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

    }
}

using System.Linq.Expressions;

namespace Domain.Abstractions
{
    public interface IRepository<T>
    {
        public Task<IReadOnlyList<T>> GetByFilterAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
        public Task<IReadOnlyList<T>> GetByFilterAsync(
            Expression<Func<T, bool>> filter,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        public Task CreateAsync(T entity, CancellationToken cancellationToken);
        public Task Update(T entity);
        public Task Delete(T entity);
        public Task<int> CountAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    }
}

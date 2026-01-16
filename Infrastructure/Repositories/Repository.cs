using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<T>> GetByFilterAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
        {
            var entities = await _context.Set<T>()
                .Where(filter)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return entities;
        }

        public async Task<IReadOnlyList<T>> GetByFilterAsync(
            Expression<Func<T, bool>> filter,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            IQueryable<T> query = _context.Set<T>().Where(filter).AsNoTracking();

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task CreateAsync(T entity, CancellationToken cancellationToken)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
        }

        public Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<int> CountAsync(
            Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken)
        {
            return await _context.Set<T>()
                .Where(filter)
                .CountAsync(cancellationToken);
        }
    }
}
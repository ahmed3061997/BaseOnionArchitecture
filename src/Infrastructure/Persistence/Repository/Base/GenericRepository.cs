using Domain.Exceptions;
using Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Linq.Expressions;

namespace Persistence.Repository.Base
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual async Task Remove(int id)
        {
            var entity = await _dbSet.FindAsync(id) ?? throw new NotFoundException();
            _dbSet.Remove(entity);
        }

        public virtual async Task<T?> Find(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(predicate)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<T?> Find<TProperty>(Expression<Func<T, bool>> predicate, Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(navigationPropertyPath)
                .Where(predicate)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(navigationPropertyPath)
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<TResult>> SelectAllAsync<TResult>(Expression<Func<T, TResult>> selector)
        {
            return await _dbSet
                .AsNoTracking()
                .Select(selector)
                .ToListAsync();
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
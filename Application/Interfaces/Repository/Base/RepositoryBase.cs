using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Application.Interfaces.Persistence;
using Application.Interfaces.Repository;

namespace Application.Repository.Base
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        private readonly IApplicationDbContext context;

        public RepositoryBase(IApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public async Task Delete(Expression<Func<T, bool>> expression)
        {
            var entity = await context.Set<T>().Where(expression).FirstOrDefaultAsync();
            Delete(entity);
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }

        public IQueryable<T> AsQueryable()
        {
            return context.Set<T>().AsQueryable();
        }

        public async Task<T> Find(object key)
        {
            return await context.Set<T>().FindAsync(key);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression)
        {
            return await context.Set<T>().Where(expression).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expression)
        {
            return await context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}

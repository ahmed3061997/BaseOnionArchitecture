using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task Delete(Expression<Func<T, bool>> expression);
        Task<T> Find(object key);
        Task<T> Get(Expression<Func<T, bool>> expression);
        IQueryable<T> AsQueryable();
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expression);
        Task<int> SaveChangesAsync();
    }
}

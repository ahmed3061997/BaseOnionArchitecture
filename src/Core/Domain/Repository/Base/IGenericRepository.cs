using System.Linq.Expressions;

namespace Domain.Repository.Base
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath);
        Task<IEnumerable<TResult>> SelectAllAsync<TResult>(Expression<Func<T, TResult>> selector);
        Task<T?> GetByIdAsync(int id);
        Task<T?> Find(Expression<Func<T, bool>> predicate);
        Task<T?> Find<TProperty>(Expression<Func<T, bool>> predicate, Expression<Func<T, TProperty>> navigationPropertyPath);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        Task Remove(int id);
    }
}

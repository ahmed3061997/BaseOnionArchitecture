using Domain.Entities.System;
using Domain.Exceptions;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Repository.Base;
using System.Linq.Expressions;

namespace Persistence.Repository
{
    public class OperationRepository : GenericRepository<Operation>, IOperationRepository
    {
        public OperationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async override Task<IEnumerable<Operation>> GetAllAsync<TProperty>(Expression<Func<Operation, TProperty>> navigationPropertyPath)
        {
            return await _dbSet
                .Include(navigationPropertyPath)
                .AsNoTracking()
                .OrderBy(x => x.Code)
                .ToListAsync();
        }

        public override void Add(Operation entity)
        {
            ThrowIfDuplicatedCode(entity);
            ThrowIfDuplicatedNames(entity);
            base.Add(entity);
        }

        public override void Update(Operation entity)
        {
            ThrowIfDuplicatedCode(entity, entity.Id);
            ThrowIfDuplicatedNames(entity, entity.Id);
            base.Update(entity);
        }

        public override void Remove(Operation entity)
        {
            ThrowDeleteExceptionIfUsed(entity);
            base.Remove(entity);
        }

        #region Constrains

        private void ThrowIfDuplicatedCode(Operation entity, int? exclude = null)
        {
            if (_dbSet.Any(m => m.Code == entity.Code && m.Id != exclude))
                throw new CodeUsedException();
        }

        private void ThrowIfDuplicatedNames(Operation entity, int? exclude = null)
        {
            var names = entity.Names.Select(x => x.Name).ToHashSet();
            if (_context.Set<OperationName>().Any(n => names.Contains(n.Name) && n.OperationId != exclude))
            {
                throw new NameUsedException(_context
                    .Set<OperationName>()
                    .Where(n => names.Contains(n.Name))
                    .Select(x => x.Name)
                    .AsEnumerable());
            }
        }

        private void ThrowDeleteExceptionIfUsed(Operation entity)
        {
            if (_context.PageOperations.Any(m => m.OperationId == entity.Id))
                throw new DeleteUsedEntityException();
        }

        #endregion
    }
}

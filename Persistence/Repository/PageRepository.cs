using Application.Common.Exceptions;
using Domain.Entities.System;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Repository.Base;
using System.Linq.Expressions;

namespace Persistence.Repository
{
    public class PageRepository : GenericRepository<Page>, IPageRepository
    {
        public PageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async override Task<IEnumerable<Page>> GetAllAsync<TProperty>(Expression<Func<Page, TProperty>> navigationPropertyPath)
        {
            return await _dbSet
                .Include(navigationPropertyPath)
                .AsNoTracking()
                .OrderBy(x => x.Code)
                .ToListAsync();
        }

        public override void Add(Page entity)
        {
            ThrowIfDuplicatedCode(entity);
            ThrowIfDuplicatedNames(entity);
            base.Add(entity);
        }

        public override void Update(Page entity)
        {
            ThrowIfDuplicatedCode(entity, entity.Id);
            ThrowIfDuplicatedNames(entity, entity.Id);
            base.Update(entity);
        }

        #region Constrains

        private void ThrowIfDuplicatedCode(Page entity, int? exclude = null)
        {
            if (_dbSet.Any(m => m.Code == entity.Code && m.Id != exclude))
                throw new CodeUsedException();
        }

        private void ThrowIfDuplicatedNames(Page entity, int? exclude = null)
        {
            var names = entity.Names.Select(x => x.Name).ToHashSet();
            if (_context.Set<PageName>().Any(n => names.Contains(n.Name) && n.PageId != exclude))
            {
                throw new NameUsedException(_context
                    .Set<PageName>()
                    .Where(n => names.Contains(n.Name))
                    .Select(x => x.Name)
                    .AsEnumerable());
            }
        }

        #endregion
    }
}

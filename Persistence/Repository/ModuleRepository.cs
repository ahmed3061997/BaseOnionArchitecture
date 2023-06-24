using Application.Common.Exceptions;
using Domain.Entities.System;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Repository.Base;
using System.Linq.Expressions;

namespace Persistence.Repository
{
    public class ModuleRepository : GenericRepository<Module>, IModuleRepository
    {
        public ModuleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async override Task<IEnumerable<Module>> GetAllAsync<TProperty>(Expression<Func<Module, TProperty>> navigationPropertyPath)
        {
            return await _dbSet
                .Include(navigationPropertyPath)
                .AsNoTracking()
                .OrderBy(x => x.Code)
                .ToListAsync();
        }

        public override void Add(Module entity)
        {
            ThrowIfDuplicatedCode(entity);
            ThrowIfDuplicatedNames(entity);
            base.Add(entity);
        }

        public override void Update(Module entity)
        {
            ThrowIfDuplicatedCode(entity, entity.Id);
            ThrowIfDuplicatedNames(entity, entity.Id);
            base.Update(entity);
        }

        public override void Remove(Module entity)
        {
            ThrowDeleteExceptionIfUsed(entity);
            base.Remove(entity);
        }

        #region Constrains

        private void ThrowIfDuplicatedCode(Module entity, int? exclude = null)
        {
            if (_dbSet.Any(m => m.Code == entity.Code && m.Id != exclude))
                throw new CodeUsedException();
        }

        private void ThrowIfDuplicatedNames(Module entity, int? exclude = null)
        {
            var names = entity.Names.Select(x => x.Name).ToHashSet();
            if (_context.Set<ModuleName>().Any(n => names.Contains(n.Name) && n.ModuleId != exclude))
            {
                throw new NameUsedException(_context
                    .Set<ModuleName>()
                    .Where(n => names.Contains(n.Name))
                    .Select(x => x.Name)
                    .AsEnumerable());
            }
        }

        private void ThrowDeleteExceptionIfUsed(Module entity)
        {
            if (_context.Pages.Any(m => m.ModuleId == entity.Id))
                throw new DeleteUsedEntityException();
        }

        #endregion
    }
}

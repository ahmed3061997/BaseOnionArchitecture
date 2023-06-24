using Domain.Common;
using Domain.Repository;
using Domain.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Context;

namespace Persistence.Repository.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private const string APP_USER = "APP_USER";

        #region Repositories

        public IModuleRepository Modules { get; private set; }
        public IPageRepository Pages { get; private set; }
        public IOperationRepository Operations { get; private set; }
        public IPageOperationRepository PageOperations { get; private set; }

        #endregion

        public UnitOfWork(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

            Modules = new ModuleRepository(_context);
            Pages = new PageRepository(_context);
            Operations = new OperationRepository(_context);
            PageOperations = new PageOperationRepository(_context);
        }

        public async Task<IDbTransaction> BeginTransaction()
        {
            return new DbTransaction(await _context.Database.BeginTransactionAsync());
        }

        public async Task CompleteAsync()
        {
            AddAuditInfo();
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        #region Auditing

        private void AddAuditInfo()
        {
            var entities = _context.ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            var utcNow = DateTime.UtcNow;
            var user = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? _configuration.GetValue("AppName", APP_USER);

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    entity.Entity.Created = utcNow;
                    entity.Entity.CreatedBy = user;
                }

                if (entity.State == EntityState.Modified)
                {
                    entity.Entity.LastModified = utcNow;
                    entity.Entity.LastModifiedBy = user;
                }

            }
        }

        #endregion
    }
}

using Application.Interfaces.Persistence;
using Domain.Entities.System;
using Domain.Entities.Users;
using Infrastructure.Common;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationUserRole, string>, IApplicationDbContext
    {
        private readonly IMediator mediator;

        public ApplicationDbContext(
            IMediator mediator,
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.mediator = mediator;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await mediator.DispatchDomainEvents(this);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> UpdateAsync<T>(T obj, CancellationToken cancellationToken = default) where T : class
        {
            Entry(obj).State = EntityState.Modified;
            return await SaveChangesAsync(cancellationToken);
        }

        public DbSet<Module> Modules { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageOperation> PageOperations { get; set; }
    }
}

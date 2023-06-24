using Domain.Entities.System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Identity;

namespace Persistence.Context
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, IdentityUserLogin<string>, RoleClaim, IdentityUserToken<string>>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
            builder.Seed();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Module> Modules { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageOperation> PageOperations { get; set; }
    }
}

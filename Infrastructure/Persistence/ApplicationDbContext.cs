using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Persistence;
using Domain.Entities.Users;
using Infrastructure.Common;

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
    }
}

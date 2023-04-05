using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Identity
{
    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(rc => rc.UserId)
                .IsRequired();

            builder.HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(rc => rc.UserId)
                .IsRequired();
        }
    }
}

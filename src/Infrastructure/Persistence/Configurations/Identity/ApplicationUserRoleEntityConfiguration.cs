using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Identity
{
    public class ApplicationUserRoleEntityConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(rc => rc.UserId)
                .IsRequired();
        }
    }
}

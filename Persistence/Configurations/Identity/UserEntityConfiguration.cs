using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Identity;

namespace Persistence.Configurations.Identity
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
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

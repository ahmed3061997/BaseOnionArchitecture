using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Users
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public virtual ApplicationRole Role { get; set; }
    }
}

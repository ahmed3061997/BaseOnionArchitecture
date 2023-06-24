using Microsoft.AspNetCore.Identity;

namespace Persistence.Identity
{
    public class RoleClaim : IdentityRoleClaim<string>
    {
        public virtual Role Role { get; set; }
    }
}

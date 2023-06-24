using Microsoft.AspNetCore.Identity;

namespace Persistence.Identity
{
    public class UserRole : IdentityUserRole<string>
    {
        public string RoleId { get; set; }
        public Role Role { get; set; }
    }
}

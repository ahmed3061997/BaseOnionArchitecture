using Microsoft.AspNetCore.Identity;

namespace Persistence.Identity
{
    public class Role : IdentityRole
    {
        public Role() : base()
        {
        }
        public Role(string roleName) : base(roleName)
        {
        }

        public bool IsActive { get; set; }
        public IList<RoleName> Names { get; set; }
        public virtual ICollection<RoleClaim> Claims { get; set; }
        public virtual ICollection<UserRole> Users { get; set; }
    }
}

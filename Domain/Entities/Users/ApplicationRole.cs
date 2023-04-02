using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Users
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {
        }
        public ApplicationRole(string roleName) : base(roleName)
        {
        }

        public bool IsActive { get; set; }
        public IList<ApplicationRoleName> Names { get; set; }
        public virtual ICollection<ApplicationRoleClaim> Claims { get; set; }
        public virtual ICollection<ApplicationUserRole> Users { get; set; }
    }
}

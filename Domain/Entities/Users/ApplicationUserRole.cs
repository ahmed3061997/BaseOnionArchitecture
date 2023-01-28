using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Users
{
    public class ApplicationUserRole : IdentityRole
    {
        public ApplicationUserRole()
        {
        }

        public ApplicationUserRole(string roleName) : base(roleName)
        {
        }

        public IList<IdentityRoleClaim<int>>? Claims { get; set; }
    }
}

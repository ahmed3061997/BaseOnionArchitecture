using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Users
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public string RoleId { get; set; }
        public ApplicationRole Role { get; set; }
    }
}

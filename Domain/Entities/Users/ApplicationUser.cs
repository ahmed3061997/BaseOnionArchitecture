using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Users
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }

        public ApplicationUser(string userName) : base(userName)
        {
        }

        public string FullName { get; set; }
        public string? ProfileImage { get; set; }
        public bool IsActive { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool IsOnline => IsLoggedIn && RefreshTokens.Any(t => t.IsActive);

        public virtual IList<ApplicationUserClaim> Claims { get; set; }
        public virtual List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}

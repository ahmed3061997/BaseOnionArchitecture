using Microsoft.AspNetCore.Identity;

namespace Persistence.Identity
{
    public class User : IdentityUser
    {
        public User()
        {
        }

        public User(string userName) : base(userName)
        {
        }

        public string FullName { get; set; }
        public string? ProfileImage { get; set; }
        public bool IsActive { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool IsOnline => IsLoggedIn && RefreshTokens.Any(t => t.IsActive);

        public virtual IList<UserClaim> Claims { get; set; }
        public virtual IList<UserRole> Roles { get; set; }
        public virtual List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}

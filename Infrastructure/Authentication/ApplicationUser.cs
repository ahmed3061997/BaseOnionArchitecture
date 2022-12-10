using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public virtual List<RefreshToken> RefreshTokens { get; set; }
    }
}

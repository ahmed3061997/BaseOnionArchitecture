using Application.Models.Authentication;

namespace Infrastructure.Authentication
{
    public interface ITokenService
    {
        Task<JwtToken> GenerateToken(ApplicationUser user);
        Task<JwtToken> RefreshToken(string token);
    }
}

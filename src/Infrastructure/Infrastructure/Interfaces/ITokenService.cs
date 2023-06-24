using Application.Models.Users;
using Persistence.Identity;

namespace Infrastructure.Interfaces
{
    public interface ITokenService
    {
        Task<JwtToken> GenerateToken(User user);
        Task<JwtToken> RefreshToken(string token);
        Task RevokeTokens(string userId);
    }
}

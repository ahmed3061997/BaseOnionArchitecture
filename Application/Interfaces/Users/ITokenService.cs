using Application.Models.Users;
using Domain.Entities.Users;

namespace Application.Interfaces.Users
{
    public interface ITokenService
    {
        Task<JwtToken> GenerateToken(ApplicationUser user);
        Task<JwtToken> RefreshToken(string token);
        Task RevokeTokens(string userId);
    }
}

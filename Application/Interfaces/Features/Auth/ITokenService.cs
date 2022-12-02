using Domain.Entities;
using Domain.Models.Auth;

namespace Application.Interfaces.Features.Auth
{
    public interface ITokenService
    {
        Task<JwtToken> GenerateToken(User user);
        Task<JwtToken> RefreshToken(string token);
    }
}

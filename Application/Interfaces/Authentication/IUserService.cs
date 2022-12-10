using Application.Models.Authentication;

namespace Application.Interfaces.Authentication
{
    public interface IUserService
    {
        Task<JwtToken> CreateUser(UserDto user, string password);
        Task AssignToRole(string userId, string role);
        Task<JwtToken> Login(string username, string password);
        Task<JwtToken> RefreshToken(string token);
        Task<UserDto> Get(string userId);
        Task Logout();
    }
}

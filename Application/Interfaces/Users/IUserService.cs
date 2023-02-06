using Application.Models.Users;

namespace Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserDto> Get(string userId);
        Task<AuthResult> Create(UserDto user, string password);
        Task AssignToRoles(string userId, IEnumerable<string> roles);
        Task<IEnumerable<string>> GetRoles(string userId);
    }
}

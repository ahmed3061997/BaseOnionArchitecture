using Application.Models.Users;

namespace Application.Interfaces.Users
{
    public interface ICurrentUserService
    {
        Task<UserDto> GetCurrentUser();
        string GetCurrentUserId();
    }
}

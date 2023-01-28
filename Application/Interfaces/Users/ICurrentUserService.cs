using Application.Models.Users;

namespace Application.Interfaces.Users
{
    public interface ICurrentUserService
    {
        Task<CurrentUserDto> GetCurrentUser();
        string GetCurrentUserId();
    }
}

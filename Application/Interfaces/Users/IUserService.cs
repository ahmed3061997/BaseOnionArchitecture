using Application.Models.Common;
using Application.Models.Users;

namespace Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserDto> Get(string userId);
        Task<IEnumerable<UserDto>> GetDrop();
        Task<PageResultDto<UserDto>> GetAll(PageQueryDto query);
        Task<AuthResult> Create(UserDto user, string password);
        Task AssignToRoles(string userId, IEnumerable<string> roles);
        Task<IEnumerable<string>> GetRoles(string userId);
    }
}

using Application.Models.Users;

namespace Application.Interfaces.Users
{
    public interface IRoleService
    {
        Task Create(RoleDto role);
        Task Update(RoleDto role);
        Task Delete(string roleId);
        Task<RoleDto> Get(string roleId);
        Task SetClaims(string roleId, IEnumerable<string> claims);
    }
}

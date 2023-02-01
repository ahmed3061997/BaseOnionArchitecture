using Application.Models.Common;
using Application.Models.Users;
using System.Security.Claims;

namespace Application.Interfaces.Users
{
    public interface IRoleService
    {
        Task Create(RoleDto role);
        Task Update(RoleDto role);
        Task Delete(string id);
        Task<RoleDto> Get(string id);
        Task<IEnumerable<RoleDto>> GetDrop();
        Task<IEnumerable<RoleDto>> GetAll( ); 
    }
}

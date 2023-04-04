using Application.Models.Common;
using Application.Models.Users;

namespace Application.Interfaces.Users
{
    public interface IRoleService
    {
        Task Create(RoleDto role);
        Task Edit(RoleDto role);
        Task Delete(string id);
        Task Activate(string id);
        Task Stop(string id);
        Task<RoleDto> Get(string id);
        Task<IEnumerable<RoleDto>> GetDrop();
        Task<PageResultDto<RoleDto>> GetAll(PageQueryDto query);
    }
}

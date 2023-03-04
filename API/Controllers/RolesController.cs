using API.Common;
using Application.Interfaces.Users;
using Application.Models.Common;
using Application.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService roleService;

        public RolesController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        [HttpPost(ApiRoutes.GetAll)]
        public async Task<PageResultDto<RoleDto>> GetAll(PageQueryDto query)
        {
            return await roleService.GetAll(query);
        }
    }
}

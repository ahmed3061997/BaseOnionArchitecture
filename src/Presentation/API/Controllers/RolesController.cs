using API.Common;
using Application.Contracts.Identity;
using Application.Contracts.Validation;
using Application.Models.Common;
using Application.Models.Users;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService roleService;
        private readonly IValidationService validationService;

        public RolesController(IRoleService roleService, IValidationService validationService)
        {
            this.roleService = roleService;
            this.validationService = validationService;
        }

        [HttpPost(ApiRoutes.GetAll)]
        [PermissionAuthorize(Modules.ManageUsers, Pages.Roles, Operations.View)]
        public async Task<PageResultDto<RoleDto>> GetAll(PageQueryDto query)
        {
            return await roleService.GetAll(query);
        }

        [HttpGet(ApiRoutes.GetDrop)]
        public async Task<IEnumerable<RoleDto>> GetDrop()
        {
            return await roleService.GetDrop();
        }

        [HttpGet(ApiRoutes.Get)]
        [PermissionAuthorize(Modules.ManageUsers, Pages.Roles, Operations.View)]
        public async Task<RoleDto> Get(string id)
        {
            return await roleService.Get(id);
        }

        [HttpPost(ApiRoutes.Create)]
        [PermissionAuthorize(Modules.ManageUsers, Pages.Roles, Operations.Create)]
        public async Task Create(RoleDto role)
        {
            await validationService.ThrowIfInvalid(role);
            await roleService.Create(role);
        }

        [HttpPost(ApiRoutes.Edit)]
        [PermissionAuthorize(Modules.ManageUsers, Pages.Roles, Operations.Update)]
        public async Task Edit(RoleDto role)
        {
            await validationService.ThrowIfInvalid(role);
            await roleService.Edit(role);
        }

        [HttpPost(ApiRoutes.Delete)]
        [PermissionAuthorize(Modules.ManageUsers, Pages.Roles, Operations.Delete)]
        public async Task Delete(string id)
        {
            await roleService.Delete(id);
        }

        [HttpPost(ApiRoutes.Activate)]
        [PermissionAuthorize(Modules.ManageUsers, Pages.Roles, Operations.ToggleActive)]
        public async Task Activate(string id)
        {
            await roleService.Activate(id);
        }

        [HttpPost(ApiRoutes.Stop)]
        [PermissionAuthorize(Modules.ManageUsers, Pages.Roles, Operations.ToggleActive)]
        public async Task Stop(string id)
        {
            await roleService.Stop(id);
        }
    }
}

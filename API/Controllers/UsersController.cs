using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Common;
using Application.Models.Users;
using Application.Interfaces.Users;
using Application.Interfaces.Validation;
using AutoMapper;
using Application.Models.Common;
using Domain.Enums;
using Infrastructure.Features.Users;

namespace API.Controllers
{
    [Route("api/users")]
    [ApiController, Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IValidationService validationService;
        private readonly IMapper mapper;

        public UsersController(IUserService userService, IValidationService validationService, IMapper mapper)
        {
            this.userService = userService;
            this.validationService = validationService;
            this.mapper = mapper;
        }

        [HttpGet(ApiRoutes.GetRoles)]
        public async Task<IEnumerable<string>> GetRoles(string id)
        {
            return await userService.GetRoles(id);
        }

        [HttpGet(ApiRoutes.GetClaims)]
        public async Task<IEnumerable<string>> GetClaims(string id)
        {
            return await userService.GetClaims(id);
        }

        [HttpPost(ApiRoutes.GetAll)]
        [PermissionAuthorize(Modules.ManageUsers, Pages.Users, Operations.View)]
        public async Task<PageResultDto<UserDto>> GetAll(PageQueryDto query)
        {
            return await userService.GetAll(query);
        }

        [HttpGet(ApiRoutes.GetDrop)]
        public async Task<IEnumerable<UserDto>> GetDrop()
        {
            return await userService.GetDrop();
        }

        [HttpGet(ApiRoutes.Get)]
        [PermissionAuthorize(Modules.ManageUsers, Pages.Users, Operations.View)]
        public async Task<UserDto> Get(string id)
        {
            return await userService.Get(id);
        }

        [HttpPost(ApiRoutes.Create)]
        [PermissionAuthorize(Modules.ManageUsers, Pages.Users, Operations.View)]
        public async Task Create(UserDto user)
        {
            await validationService.ThrowIfInvalid(user);
            await userService.Create(mapper.Map<UserDto>(user), user.Password);
        }

        [HttpPost(ApiRoutes.Edit)]
        [PermissionAuthorize(Modules.ManageUsers, Pages.Users, Operations.Update)]
        public async Task Edit(UserDto user)
        {
            await validationService.ThrowIfInvalid(user);
            await userService.Edit(mapper.Map<UserDto>(user));
        }

        [HttpPost(ApiRoutes.Delete)]
        [PermissionAuthorize(Modules.ManageUsers, Pages.Users, Operations.Delete)]
        public async Task Delete(string id)
        {
            await userService.Delete(id);
        }

        [HttpPost(ApiRoutes.Activate)]
        [PermissionAuthorize(Modules.ManageUsers, Pages.Users, Operations.ToggleActive)]
        public async Task Activate(string id)
        {
            await userService.Activate(id);
        }

        [HttpPost(ApiRoutes.Stop)]
        [PermissionAuthorize(Modules.ManageUsers, Pages.Users, Operations.ToggleActive)]
        public async Task Stop(string id)
        {
            await userService.Stop(id);
        }
    }
}

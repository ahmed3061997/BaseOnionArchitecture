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
    }
}

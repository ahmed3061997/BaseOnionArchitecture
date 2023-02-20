using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Common;
using Application.Models.Users;
using Application.Interfaces.Users;
using Application.Interfaces.Validation;
using AutoMapper;

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

        [HttpPost(ApiRoutes.Register)]
        public async Task<AuthResult> Register(CreateUserDto dto)
        {
            await validationService.ThrowIfInvalid(dto);
            return await userService.Create(mapper.Map<UserDto>(dto), dto.Password);
        }
    }
}

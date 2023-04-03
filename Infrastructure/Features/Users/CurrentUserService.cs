using Application.Common.Constants;
using Application.Interfaces.Users;
using Application.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Features.Users
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUserService userService, IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<UserDto> GetCurrentUser()
        {
            var dto = mapper.Map<UserDto>(await userService.Get(GetCurrentUserId()));
            dto.Roles = httpContextAccessor.HttpContext.User.FindAll(Claims.Role).Select(x => x.Value);
            return dto;
        }

        public bool IsAuthenticated()
        {
            return httpContextAccessor.HttpContext.User.Identity!.IsAuthenticated;
        }

        public string GetCurrentUserId()
        {
            if (!IsAuthenticated()) return null;
            return httpContextAccessor.HttpContext.User.FindFirst(Claims.UserId).Value;
        }
    }
}

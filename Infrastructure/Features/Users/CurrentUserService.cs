using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Users;
using Application.Models.Users;
using Application.Common.Constants;

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

        public async Task<CurrentUserDto> GetCurrentUser()
        {
            var dto = mapper.Map<CurrentUserDto>(await userService.Get(GetCurrentUserId()));
            dto.Roles = httpContextAccessor.HttpContext.User.FindAll(ClaimTypes.Role).Select(x => x.Value);
            return dto;
        }

        public string GetCurrentUserId()
        {
            return httpContextAccessor.HttpContext.User.FindFirst(Claims.UserId).Value;
        }
    }
}

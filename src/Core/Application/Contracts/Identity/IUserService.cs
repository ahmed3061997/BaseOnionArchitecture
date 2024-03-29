﻿using Application.Models.Common;
using Application.Models.Users;

namespace Application.Contracts.Identity
{
    public interface IUserService
    {
        Task<UserDto> Get(string userId);
        Task<IEnumerable<UserDto>> GetDrop();
        Task<PageResultDto<UserDto>> GetAll(PageQueryDto query);
        Task Create(UserDto user, string password);
        Task Edit(UserDto user);
        Task Delete(string id);
        Task Activate(string id);
        Task Stop(string id);
        Task AssignToRoles(string id, IEnumerable<string> roles);
        Task<IEnumerable<string>> GetClaims(string id);
        Task<IEnumerable<UserRoleDto>> GetRoles(string id);
    }
}

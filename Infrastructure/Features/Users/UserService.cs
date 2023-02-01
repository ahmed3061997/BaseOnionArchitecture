﻿using Application.Common.Extensions;
using Application.Interfaces.Users;
using Application.Models.Users;
using AutoMapper;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Features.Users
{
    public class UserService : IUserService
    {
        private UserManager<ApplicationUser> userManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public UserService(UserManager<ApplicationUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        public async Task<UserDto> Get(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            return mapper.Map<UserDto>(user);
        }

        public async Task<AuthResult> Create(UserDto user, string password)
        {
            var dbUser = mapper.Map<ApplicationUser>(user);
            var result = await userManager.CreateAsync(dbUser, password);
            result.ThrowIfFailed();
            return new AuthResult() { User = mapper.Map<UserDto>(dbUser), Jwt = await tokenService.GenerateToken(dbUser) };
        }

        public async Task AssignToRole(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId);
            await userManager.AddToRoleAsync(user, role);
        }

        public async Task<IEnumerable<string>> GetRoles(string userId)
        {
            return await userManager.GetRolesAsync(await userManager.FindByIdAsync(userId));
        }
    }
}

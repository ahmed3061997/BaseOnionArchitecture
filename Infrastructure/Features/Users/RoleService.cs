using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interfaces.Users;
using Application.Models.Users;
using Domain.Entities.Users;
using Application.Common.Constants;

namespace Infrastructure.Features.Users
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationUserRole> roleManager;
        private readonly IMapper mapper;

        public RoleService(RoleManager<ApplicationUserRole> roleManager, IMapper mapper)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

        public async Task Create(RoleDto role)
        {
            var result = await roleManager.CreateAsync(mapper.Map<ApplicationUserRole>(role));
            result.ThrowIfFailed();
        }

        public async Task Update(RoleDto role)
        {
            var result = await roleManager.UpdateAsync(mapper.Map<ApplicationUserRole>(role));
            result.ThrowIfFailed();
        }

        public async Task Delete(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            var result = await roleManager.DeleteAsync(role);
            result.ThrowIfFailed();
        }

        public async Task<RoleDto> Get(string roleId)
        {
            return mapper.Map<RoleDto>(await roleManager.FindByIdAsync(roleId));
        }

        public async Task SetClaims(string roleId, IEnumerable<string> claims)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            role.Claims = claims.Select(claim => new IdentityRoleClaim<int>()
            {
                ClaimType = Claims.Permission,
                ClaimValue = claim
            }).ToList();
            var result = await roleManager.UpdateAsync(role);
            result.ThrowIfFailed();
        }
    }
}

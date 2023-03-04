using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Interfaces.Users;
using Application.Models.Common;
using Application.Models.Users;
using AutoMapper;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Features.Users
{
    public class RoleService : IRoleService
    {
        private readonly IEnumerable<string> SeachColumns = new string[] { "Id", "IsActive", "Names.Name" };
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IMapper mapper;

        public RoleService(RoleManager<ApplicationRole> roleManager, IMapper mapper)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

        public async Task Create(RoleDto role)
        {
            var result = await roleManager.CreateAsync(mapper.Map<ApplicationRole>(role));
            result.ThrowIfFailed();
        }

        public async Task Update(RoleDto role)
        {
            var result = await roleManager.UpdateAsync(mapper.Map<ApplicationRole>(role));
            result.ThrowIfFailed();
        }

        public async Task Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            var result = await roleManager.DeleteAsync(role);
            result.ThrowIfFailed();
        }

        public async Task<RoleDto> Get(string id)
        {
            return mapper.Map<RoleDto>(await roleManager.FindByIdAsync(id));
        }

        public async Task<PageResultDto<RoleDto>> GetAll(PageQueryDto queryDto)
        {
            var query = roleManager.Roles
                .Include(x => x.Names)
                .Where(SeachColumns, queryDto.SeachTerm)
                .OrderBy(queryDto.SortColumn, queryDto.SortDirection);

            return new PageResultDto<RoleDto>()
            {
                TotalCount = await query.CountAsync(),
                Items = await query.AsNoTracking()
                                   .Skip(queryDto.PageIndex * queryDto.PageSize)
                                   .Take(queryDto.PageSize)
                                   .Select(x => mapper.Map<RoleDto>(x))
                                   .ToListAsync()
            };
        }

        public async Task<IEnumerable<RoleDto>> GetDrop()
        {
            return await roleManager.Roles
                .Include(x => x.Names)
                .AsNoTracking()
                .Select(x => mapper.Map<RoleDto>(x)).ToListAsync();
        }
    }
}

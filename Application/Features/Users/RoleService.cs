using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Interfaces.Culture;
using Application.Interfaces.Notifications;
using Application.Interfaces.Persistence;
using Application.Interfaces.Users;
using Application.Models.Common;
using Application.Models.Notifications.Roles;
using Application.Models.Users;
using AutoMapper;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users
{
    public class RoleService : IRoleService
    {
        private readonly IEnumerable<string> SeachColumns = new string[] { "Id", "IsActive", "Names.Name" };
        private readonly INotificationService notificationService;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IApplicationDbContext context;
        private readonly ICurrentCultureService currentCultureService;
        private readonly IMapper mapper;

        public RoleService(
            INotificationService notificationService,
            RoleManager<ApplicationRole> roleManager,
            IApplicationDbContext context,
            ICurrentCultureService currentCultureService,
            IMapper mapper)
        {
            this.notificationService = notificationService;
            this.roleManager = roleManager;
            this.context = context;
            this.currentCultureService = currentCultureService;
            this.mapper = mapper;
        }

        public async Task Create(RoleDto role)
        {
            var result = await roleManager.CreateAsync(mapper.Map<ApplicationRole>(role));
            result.ThrowIfFailed();
        }

        public async Task Edit(RoleDto role)
        {
            var dbRole = await roleManager.Roles
                .Include(x => x.Names)
                .Include(x => x.Claims)
                .FirstOrDefaultAsync(x => x.Id == role.Id) ?? throw new NotFoundException();
            var result = await roleManager.UpdateAsync(mapper.Map(role, dbRole));
            result.ThrowIfFailed();
            await notificationService.Push(new RoleUpdatedNotification(await GetUsers(role.Id), role.Id));
        }

        public async Task Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            var result = await roleManager.DeleteAsync(role);
            result.ThrowIfFailed();
        }

        public async Task Activate(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            role.IsActive = true;
            var result = await roleManager.UpdateAsync(role);
            result.ThrowIfFailed();
        }

        public async Task Stop(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            role.IsActive = false;
            var result = await roleManager.UpdateAsync(role);
            result.ThrowIfFailed();
            await notificationService.Push(new RoleBlockedNotification(await GetUsers(role.Id), id));
        }

        public async Task<RoleDto> Get(string id)
        {
            return mapper.Map<RoleDto>(await roleManager.Roles
                 .Include(x => x.Names)
                 .Include(x => x.Claims)
                 .FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task<PageResultDto<RoleDto>> GetAll(PageQueryDto queryDto)
        {
            var culture = currentCultureService.GetCurrentUICulture();

            var query = roleManager.Roles
                .Include(x => x.Names)
                .Where(SeachColumns, queryDto.SearchTerm);

            if (queryDto.SortColumn?.ToLower() == "Name".ToLower())
                query = query.OrderByPredicated("Names.Name", "Culture", culture, queryDto.SortDirection);
            else
                query = query.OrderBy(queryDto.SortColumn, queryDto.SortDirection);

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

        private async Task<IEnumerable<string>> GetUsers(string id)
        {
            return await context.Set<ApplicationUserRole>()
                .Where(x => x.RoleId == id)
                .Select(x => x.UserId)
                .ToListAsync();
        }
    }
}

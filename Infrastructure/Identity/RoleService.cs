using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Contracts.Culture;
using Application.Contracts.Identity;
using Application.Contracts.Notifications;
using Application.Models.Common;
using Application.Models.Notifications.Roles;
using Application.Models.Users;
using AutoMapper;
using Infrastructure.Common.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Identity;

namespace Infrastructure.Identity
{
    public class RoleService : IRoleService
    {
        private readonly IEnumerable<string> SearchColumns = new string[] { "Id", "IsActive", "Names.Name" };
        private readonly INotificationService _notificationService;
        private readonly RoleManager<Role> _roleManager;
        private readonly ICurrentCultureService _currentCultureService;
        private readonly IMapper _mapper;

        public RoleService(
            INotificationService notificationService,
            RoleManager<Role> roleManager,
            ICurrentCultureService currentCultureService,
            IMapper mapper)
        {
            _notificationService = notificationService;
            _roleManager = roleManager;
            _currentCultureService = currentCultureService;
            _mapper = mapper;
        }

        public async Task Create(RoleDto role)
        {
            var result = await _roleManager.CreateAsync(_mapper.Map<Role>(role));
            result.ThrowIfFailed();
        }

        public async Task Edit(RoleDto role)
        {
            var dbRole = await _roleManager.Roles
                .Include(x => x.Names)
                .Include(x => x.Claims)
                .FirstOrDefaultAsync(x => x.Id == role.Id) ?? throw new NotFoundException();
            var result = await _roleManager.UpdateAsync(_mapper.Map(role, dbRole));
            result.ThrowIfFailed();
            await _notificationService.Push(new RoleUpdatedNotification(await GetUsers(role.Id!), role.Id));
        }

        public async Task Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id) ?? throw new NotFoundException();
            var result = await _roleManager.DeleteAsync(role);
            result.ThrowIfFailed();
        }

        public async Task Activate(string id)
        {
            var role = await _roleManager.FindByIdAsync(id) ?? throw new NotFoundException();
            role.IsActive = true;
            var result = await _roleManager.UpdateAsync(role);
            result.ThrowIfFailed();
        }

        public async Task Stop(string id)
        {
            var role = await _roleManager.FindByIdAsync(id) ?? throw new NotFoundException();
            role.IsActive = false;
            var result = await _roleManager.UpdateAsync(role);
            result.ThrowIfFailed();
            await _notificationService.Push(new RoleBlockedNotification(await GetUsers(role.Id), id));
        }

        public async Task<RoleDto> Get(string id)
        {
            return _mapper.Map<RoleDto>(await _roleManager.Roles
                 .Include(x => x.Names)
                 .Include(x => x.Claims)
                 .FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task<PageResultDto<RoleDto>> GetAll(PageQueryDto queryDto)
        {
            var culture = _currentCultureService.GetCurrentUICulture();

            var query = _roleManager.Roles
                .Include(x => x.Names)
                .Where(SearchColumns, queryDto.SearchTerm);

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
                                   .Select(x => _mapper.Map<RoleDto>(x))
                                   .ToListAsync()
            };
        }

        public async Task<IEnumerable<RoleDto>> GetDrop()
        {
            return await _roleManager.Roles
                .Include(x => x.Names)
                .AsNoTracking()
                .Select(x => _mapper.Map<RoleDto>(x)).ToListAsync();
        }

        private async Task<IEnumerable<string>> GetUsers(string id)
        {
            return await _roleManager.Roles
                .Where(x => x.Id == id)
                .SelectMany(x => x.Users)
                .Select(x => x.UserId)
                .ToListAsync();
        }
    }
}

using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Contracts.Identity;
using Application.Contracts.Notifications;
using Application.Models.Common;
using Application.Models.Notifications.Users;
using Application.Models.Users;
using AutoMapper;
using Infrastructure.Common.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Identity;

namespace Infrastructure.Identity
{
    public class UserService : IUserService
    {
        private readonly IEnumerable<string> SearchColumns = new string[] { "Id", "FirstName", "LastName", "UserName", "Email", "PhoneNumber" };
        private UserManager<User> _userManager;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<User> userManager,
            INotificationService notificationService,
            IMapper mapper)
        {
            _userManager = userManager;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<UserDto> Get(string userId)
        {
            var user = await _userManager.Users
                .Include(x => x.Roles)
                .ThenInclude(x => x.Role)
                .ThenInclude(x => x.Names)
                .FirstOrDefaultAsync(x => x.Id == userId);
            return _mapper.Map<UserDto>(user);
        }

        public async Task Create(UserDto user, string password)
        {
            //if (user.ProfileImageFile != null) user.ProfileImage = await fileManager.SaveFile(user.ProfileImageFile, "users");
            var dbUser = _mapper.Map<User>(user);
            var result = await _userManager.CreateAsync(dbUser, password);
            result.ThrowIfFailed();
        }

        public async Task Edit(UserDto user)
        {
            //if (user.ProfileImageFile != null) user.ProfileImage = await fileManager.SaveFile(user.ProfileImageFile, "users");
            var dbUser = await _userManager
                .Users
                .Include(x => x.Roles)
                .Include(x => x.Claims)
                .FirstOrDefaultAsync(x => x.Id == user.Id) ?? throw new NotFoundException();
            //if (dbUser.ProfileImage != user.ProfileImage) fileManager.DeleteFile(dbUser.ProfileImage);
            _mapper.Map(user, dbUser);
            var result = await _userManager.UpdateAsync(dbUser);
            result.ThrowIfFailed();
            await _notificationService.Push(new UserUpdatedNotification(new[] { user.Id }));
        }

        public async Task Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException();
            var result = await _userManager.DeleteAsync(user);
            result.ThrowIfFailed();
        }

        public async Task Activate(string id)
        {
            var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException();
            user.IsActive = true;
            var result = await _userManager.UpdateAsync(user);
            result.ThrowIfFailed();
        }

        public async Task Stop(string id)
        {
            var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException();
            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);
            result.ThrowIfFailed();
            await _notificationService.Push(new UserBlockedNotification(new[] { user.Id }));
        }

        public async Task AssignToRoles(string id, IEnumerable<string> roles)
        {
            var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException();
            await _userManager.AddToRolesAsync(user, roles);
        }

        public async Task<IEnumerable<UserRoleDto>> GetRoles(string id)
        {
            return await _userManager.Users
                .Where(x => x.Id == id)
                .SelectMany(x => x.Roles.Where(r => r.Role.IsActive))
                .Include(x => x.Role)
                .ThenInclude(x => x.Names)
                .Select(x => _mapper.Map<UserRoleDto>(x))
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetClaims(string id)
        {
            var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException();
            var userClaims = await _userManager.GetClaimsAsync(user);
            return userClaims.Select(x => x.Value);
        }

        public async Task<IEnumerable<UserDto>> GetDrop()
        {
            return await _userManager.Users
                  .Select(x => _mapper.Map<UserDto>(x))
                  .ToListAsync();
        }

        public async Task<PageResultDto<UserDto>> GetAll(PageQueryDto queryDto)
        {
            var query = _userManager.Users
                .Where(SearchColumns, queryDto.SearchTerm)
                .OrderBy(queryDto.SortColumn, queryDto.SortDirection);

            return new PageResultDto<UserDto>()
            {
                TotalCount = await query.CountAsync(),
                Items = await query.AsNoTracking()
                                   .Skip(queryDto.PageIndex * queryDto.PageSize)
                                   .Take(queryDto.PageSize)
                                   .Select(x => _mapper.Map<UserDto>(x))
                                   .ToListAsync()
            };
        }
    }
}

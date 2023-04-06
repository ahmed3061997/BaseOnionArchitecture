using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Interfaces.FileManager;
using Application.Interfaces.Notifications;
using Application.Interfaces.Persistence;
using Application.Interfaces.Users;
using Application.Models.Common;
using Application.Models.Notifications.Users;
using Application.Models.Users;
using AutoMapper;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;

namespace Application.Features.Users
{
    public class UserService : IUserService
    {
        private readonly IEnumerable<string> SeachColumns = new string[] { "Id", "FirstName", "LastName", "UserName", "Email", "PhoneNumber" };
        private readonly IApplicationDbContext context;
        private UserManager<ApplicationUser> userManager;
        private readonly IFileManager fileManager;
        private readonly INotificationService notificationService;
        private readonly IMapper mapper;

        public UserService(
            IApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IFileManager fileManager,
            INotificationService notificationService,
            IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.fileManager = fileManager;
            this.notificationService = notificationService;
            this.mapper = mapper;
        }

        public async Task<UserDto> Get(string userId)
        {
            var user = await userManager.Users
                .Include(x => x.Roles)
                .ThenInclude(x => x.Role)
                .ThenInclude(x => x.Names)
                .FirstOrDefaultAsync(x => x.Id == userId);
            return mapper.Map<UserDto>(user);
        }

        public async Task Create(UserDto user, string password)
        {
            if (user.ProfileImageFile != null) user.ProfileImage = await fileManager.SaveFile(user.ProfileImageFile, "users");
            var dbUser = mapper.Map<ApplicationUser>(user);
            var result = await userManager.CreateAsync(dbUser, password);
            result.ThrowIfFailed();
        }

        public async Task Edit(UserDto user)
        {
            if (user.ProfileImageFile != null) user.ProfileImage = await fileManager.SaveFile(user.ProfileImageFile, "users");
            var dbUser = await userManager
                .Users
                .Include(x => x.Roles)
                .Include(x => x.Claims)
                .FirstOrDefaultAsync(x => x.Id == user.Id) ?? throw new NotFoundException();
            if (dbUser.ProfileImage != user.ProfileImage) fileManager.DeleteFile(dbUser.ProfileImage);
            mapper.Map(user, dbUser);
            var result = await userManager.UpdateAsync(dbUser);
            result.ThrowIfFailed();
            await notificationService.Push(new UserUpdatedNotification(new[] { user.Id }));
        }

        public async Task Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var result = await userManager.DeleteAsync(user);
            result.ThrowIfFailed();
        }

        public async Task Activate(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            user.IsActive = true;
            var result = await userManager.UpdateAsync(user);
            result.ThrowIfFailed();
        }

        public async Task Stop(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            user.IsActive = false;
            var result = await userManager.UpdateAsync(user);
            result.ThrowIfFailed();
            await notificationService.Push(new UserBlockedNotification(new[] { user.Id }));
        }

        public async Task AssignToRoles(string userId, IEnumerable<string> roles)
        {
            var user = await userManager.FindByIdAsync(userId);
            await userManager.AddToRolesAsync(user, roles);
        }

        public async Task<IEnumerable<UserRoleDto>> GetRoles(string userId)
        {
            return await context.Set<ApplicationRole>()
                .Include(x => x.Names)
                .Where(x => x.Users.Any(u => u.UserId == userId) && x.IsActive)
                .Select(x => mapper.Map<UserRoleDto>(x))
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetClaims(string userId)
        {
            var userClaims = context.Set<ApplicationUserClaim>()
                                    .Where(x => x.UserId == userId)
                                    .Select(x => x.ClaimValue);
            var roleClaims = context.Set<ApplicationRoleClaim>()
                                    .Where(x => x.Role.IsActive && x.Role.Users.Any(y => y.UserId == userId))
                                    .Select(x => x.ClaimValue);
            return await userClaims.Union(roleClaims).ToListAsync();
        }

        public async Task<IEnumerable<UserDto>> GetDrop()
        {
            return await userManager.Users
                  .Select(x => mapper.Map<UserDto>(x))
                  .ToListAsync();
        }

        public async Task<PageResultDto<UserDto>> GetAll(PageQueryDto queryDto)
        {
            var query = userManager.Users
                .Where(SeachColumns, queryDto.SearchTerm)
                .OrderBy(queryDto.SortColumn, queryDto.SortDirection);

            return new PageResultDto<UserDto>()
            {
                TotalCount = await query.CountAsync(),
                Items = await query.AsNoTracking()
                                   .Skip(queryDto.PageIndex * queryDto.PageSize)
                                   .Take(queryDto.PageSize)
                                   .Select(x => mapper.Map<UserDto>(x))
                                   .ToListAsync()
            };
        }
    }
}

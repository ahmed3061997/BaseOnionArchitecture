using Application.Common.Extensions;
using Application.Interfaces.Culture;
using Application.Interfaces.FileManager;
using Application.Interfaces.Users;
using Application.Models.Common;
using Application.Models.Users;
using AutoMapper;
using Domain.Entities.Users;
using Infrastructure.Features.Culture;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Features.Users
{
    public class UserService : IUserService
    {
        private readonly IEnumerable<string> SeachColumns = new string[] { "Id", "FirstName", "LastName", "UserName", "Email", "PhoneNumber" };

        private UserManager<ApplicationUser> userManager;
        private readonly ITokenService tokenService;
        private readonly IFileManager fileManager;
        private readonly IMapper mapper;

        public UserService(
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            IFileManager fileManager,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.fileManager = fileManager;
            this.mapper = mapper;
        }

        public async Task<UserDto> Get(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            return mapper.Map<UserDto>(user);
        }

        public async Task<AuthResult> Create(UserDto user, string password)
        {
            if (user.ProfileImageFile != null) user.ProfileImage = await fileManager.SaveFile(user.ProfileImageFile, "users");
            var dbUser = mapper.Map<ApplicationUser>(user);
            var result = await userManager.CreateAsync(dbUser, password);
            result.ThrowIfFailed();
            return new AuthResult() { User = mapper.Map<UserDto>(dbUser), Jwt = await tokenService.GenerateToken(dbUser) };
        }

        public async Task Edit(UserDto user)
        {
            if (user.ProfileImageFile != null) user.ProfileImage = await fileManager.SaveFile(user.ProfileImageFile, "users");
            var dbUser = await userManager
                  .Users
                  .Include(x => x.Claims)
                  .FirstOrDefaultAsync(x => x.Id == user.Id);
            if (dbUser.ProfileImage != user.ProfileImage) fileManager.DeleteFile(dbUser.ProfileImage);
            var result = await userManager.UpdateAsync(mapper.Map(user, dbUser));
            result.ThrowIfFailed();
        }

        public async Task AssignToRoles(string userId, IEnumerable<string> roles)
        {
            var user = await userManager.FindByIdAsync(userId);
            await userManager.AddToRolesAsync(user, roles);
        }

        public async Task<IEnumerable<string>> GetRoles(string userId)
        {
            return await userManager.GetRolesAsync(await userManager.FindByIdAsync(userId));
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

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Application.Models.Authentication;
using Application.Interfaces.Authentication;
using Application.Exceptions;

namespace Infrastructure.Authentication
{
    public class UserService : IUserService
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public UserService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        public async Task<JwtToken> CreateUser(UserDto user, string password)
        {
            var dbUser = mapper.Map<ApplicationUser>(user);
            var result = await userManager.CreateAsync(dbUser, password);
            if (result.Succeeded) throw new CreateUserException(result.Errors.Select(x => x.Description));
            return await tokenService.GenerateToken(dbUser);
        }

        public async Task AssignToRole(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId);
            await userManager.AddToRoleAsync(user, role);
        }

        public async Task<UserDto> Get(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            return mapper.Map<UserDto>(user);
        }

        public async Task<JwtToken> Login(string username, string password)
        {
            var result = await signInManager.PasswordSignInAsync(username, password, false, false);
            if (!result.Succeeded) throw new AuthenticationException(result);
            var user = await userManager.FindByNameAsync(username);
            return await tokenService.GenerateToken(user);
        }

        public async Task<JwtToken> RefreshToken(string token)
        {
            return await tokenService.RefreshToken(token);
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }
    }
}

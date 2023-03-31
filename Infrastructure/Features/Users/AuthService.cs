using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Interfaces.Users;
using Application.Models.Users;
using AutoMapper;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Features.Users
{
    public class AuthService : IAuthService
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public AuthService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        public async Task<AuthResult> Login(string username, string password)
        {
            var user = await GetUser(username);
            var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
            result.ThrowIfFailed();
            await SetUserStatus(user, true);
            return new AuthResult() { User = mapper.Map<UserDto>(user), Jwt = await tokenService.GenerateToken(user) };
        }

        public async Task Logout()
        {
            var user = await userManager.FindByIdAsync(currentUserService.GetCurrentUserId());
            await SetUserStatus(user, false);
            await tokenService.RevokeTokens(currentUserService.GetCurrentUserId());
            await signInManager.SignOutAsync();
        }

        public async Task<IdentityTokenResult> GenerateResetPasswordToken(string username)
        {
            var user = await GetUser(username);
            return new IdentityTokenResult()
            {
                Name = user.FullName,
                Email = user.Email,
                Token = await userManager.GeneratePasswordResetTokenAsync(user)
            };
        }

        public async Task<AuthResult> ResetPassword(string username, string token, string newPassword)
        {
            var user = await GetUser(username);
            var result = await userManager.ResetPasswordAsync(user, token, newPassword);
            result.ThrowIfFailed();
            return new AuthResult() { User = mapper.Map<UserDto>(user), Jwt = await tokenService.GenerateToken(user) };
        }

        public async Task<IdentityTokenResult> GenerateEmailConfirmationToken(string username)
        {
            var user = await GetUser(username);
            return new IdentityTokenResult()
            {
                Name = user.FullName,
                Email = user.Email,
                Token = await userManager.GenerateEmailConfirmationTokenAsync(user)
            };
        }

        public async Task<AuthResult> ConfirmEmail(string username, string token)
        {
            var user = await GetUser(username);
            var result = await userManager.ConfirmEmailAsync(user, token);
            result.ThrowIfFailed();
            return new AuthResult() { User = mapper.Map<UserDto>(user), Jwt = await tokenService.GenerateToken(user) };
        }

        private async Task<ApplicationUser> GetUser(string username)
        {
            var user = await userManager.FindByEmailAsync(username) ?? await userManager.FindByNameAsync(username);
            if (user == null)
                throw new UserNotFoundException();
            return user;
        }

        private async Task SetUserStatus(ApplicationUser user, bool loggedIn)
        {
            user.IsLoggedIn = loggedIn;
            await userManager.UpdateAsync(user);
        }
    }
}

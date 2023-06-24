using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Contracts.Identity;
using Application.Models.Users;
using AutoMapper;
using Domain.Exceptions;
using Infrastructure.Common.Extensions;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Identity;

namespace Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public AuthService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<AuthResult> Login(string username, string password)
        {
            var user = await GetUser(username);
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            result.ThrowIfFailed();
            await SetUserStatus(user, true);
            return new AuthResult() { User = _mapper.Map<UserDto>(user), Jwt = await _tokenService.GenerateToken(user) };
        }

        public async Task Logout()
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.GetCurrentUserId());
            if (user == null) return;
            await SetUserStatus(user, false);
            await _tokenService.RevokeTokens(_currentUserService.GetCurrentUserId());
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityTokenResult> GenerateResetPasswordToken(string username)
        {
            var user = await GetUser(username);
            return new IdentityTokenResult()
            {
                Name = user.FullName,
                Email = user.Email,
                Token = await _userManager.GeneratePasswordResetTokenAsync(user)
            };
        }

        public async Task<AuthResult> ResetPassword(string username, string token, string newPassword)
        {
            var user = await GetUser(username);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            result.ThrowIfFailed();
            return new AuthResult() { User = _mapper.Map<UserDto>(user), Jwt = await _tokenService.GenerateToken(user) };
        }

        public async Task<IdentityTokenResult> GenerateEmailConfirmationToken(string username)
        {
            var user = await GetUser(username);
            return new IdentityTokenResult()
            {
                Name = user.FullName,
                Email = user.Email,
                Token = await _userManager.GenerateEmailConfirmationTokenAsync(user)
            };
        }

        public async Task<AuthResult> ConfirmEmail(string username, string token)
        {
            var user = await GetUser(username);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            result.ThrowIfFailed();
            return new AuthResult() { User = _mapper.Map<UserDto>(user), Jwt = await _tokenService.GenerateToken(user) };
        }

        private async Task<User> GetUser(string username)
        {
            var query = _userManager.Users
                .Include(x => x.Roles.Where(x => x.Role.IsActive))
                .Where(x => x.UserName!.ToLower() == username.ToLower() || x.Email!.ToLower() == username.ToLower());
            var user = await query.FirstOrDefaultAsync() ?? throw new NotFoundException();
            if (!user.IsActive || user.Roles.Count == 0)
                throw new UserBlockedException();
            return user;
        }

        private async Task SetUserStatus(User user, bool loggedIn)
        {
            user.IsLoggedIn = loggedIn;
            await _userManager.UpdateAsync(user);
        }
    }
}

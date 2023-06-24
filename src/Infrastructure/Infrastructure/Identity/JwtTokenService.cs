using Application.Common.Constants;
using Application.Common.Options;
using Application.Models.Users;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Persistence.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Identity
{
    public class JwtTokenService : ITokenService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly JwtOptions jwt;

        public JwtTokenService(UserManager<User> userManager, RoleManager<Role> roleManager, IOptions<JwtOptions> jwt)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.jwt = jwt.Value;
        }

        public async Task<JwtToken> GenerateToken(User user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.AddRange(await roleManager.GetClaimsAsync(await roleManager.FindByNameAsync(role)));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(Claims.UserId, user.Id),
            }
            .Union(userClaims)
            .Union(roleClaims)
            .Union(roles.Select(x => new Claim(Claims.Role, x)));

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwt.DurationInMinutes),
                signingCredentials: signingCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            RefreshToken refreshToken;
            if (user.RefreshTokens != null && user.RefreshTokens.Any(x => x.IsActive))
            {
                refreshToken = user.RefreshTokens.FirstOrDefault(x => x.IsActive);
            }
            else
            {
                refreshToken = GenerateRefreshToken();
                user.RefreshTokens.Add(refreshToken);
                user.RefreshTokens.RemoveAll(t => !t.IsActive);
                await userManager.UpdateAsync(user);
            }

            return new JwtToken()
            {
                Token = token,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiresOn = refreshToken.ExpiresOn,
            };
        }

        public async Task<JwtToken> RefreshToken(string token)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                throw new UnauthorizedAccessException("Refresh token is invalid");
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
            if (!refreshToken.IsActive)
            {
                throw new UnauthorizedAccessException("Refresh token is invalid");
            }

            refreshToken.RevokedOn = DateTime.UtcNow;
            await userManager.UpdateAsync(user);

            return await GenerateToken(user);
        }

        public async Task RevokeTokens(string userId)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            foreach (var token in user.RefreshTokens)
                token.RevokedOn = DateTime.UtcNow;
            await userManager.UpdateAsync(user);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var random = new byte[64];

            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(random);

            return new RefreshToken()
            {
                Token = Convert.ToBase64String(random),
                ExpiresOn = DateTime.UtcNow.AddHours(jwt.RefreshTokenDurationInHours),
                CreatedOn = DateTime.UtcNow,
            };
        }
    }
}

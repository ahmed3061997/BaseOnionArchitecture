using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Interfaces.Features.Auth;
using Domain.Configurations.Options;
using Domain.Constants;
using Domain.Entities;
using Domain.Models.Auth;

namespace Infrastructure.Services.Auth
{
    public class JwtTokenService : ITokenService
    {
        private readonly UserManager<User> userManager;
        private readonly JwtOptions jwt;

        public JwtTokenService(UserManager<User> userManager, IOptions<JwtOptions> jwt)
        {
            this.userManager = userManager;
            this.jwt = jwt.Value;
        }

        public async Task<JwtToken> GenerateToken(User user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim(Claims.Role, role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(Claims.UserId, user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

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

        private RefreshToken GenerateRefreshToken()
        {
            var random = new byte[64];

            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(random);

            return new RefreshToken()
            {
                Token = Convert.ToBase64String(random),
                ExpiresOn = DateTime.UtcNow.AddMinutes(jwt.RefreshTokenDurationInMinutes),
                CreatedOn = DateTime.UtcNow,
            };
        }
    }
}

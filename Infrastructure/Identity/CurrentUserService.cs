using Application.Common.Constants;
using Application.Contracts.Identity;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated()
        {
            return httpContextAccessor.HttpContext?.User.Identity!.IsAuthenticated ?? false;
        }

        public string? GetCurrentUserId()
        {
            if (!IsAuthenticated()) return null;
            return httpContextAccessor.HttpContext?.User.FindFirst(Claims.UserId)?.Value;
        }
    }
}

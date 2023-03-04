using Application.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API.Common
{
    public class PermissionHandler : AuthorizationHandler<ClaimsAuthorizationRequirement>
    {
        public PermissionHandler()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimsAuthorizationRequirement requirement)
        {
            var claims = requirement.AllowedValues ?? new List<string>();
            if (context.User.Claims.Any(x => claims.Contains(x.Value)))
                context.Succeed(requirement);
            else
                context.Fail();
            return Task.CompletedTask;
        }
    }
}

using Application.Common.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace API.Common
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(PermissionAuthorizeAttribute.POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                var claim = policyName.Split('_')[1];
                var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireClaim(Claims.Permission, claim);
                return Task.FromResult(policy.Build());
            }

            return Task.FromResult<AuthorizationPolicy>(null);
        }
    }
}

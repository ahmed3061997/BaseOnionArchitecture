using Application.Common.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Extensions
{
    public static class IdentityExtensions
    {
        public static void ThrowIfFailed(this SignInResult signInResult)
        {
            if (!signInResult.Succeeded)
                throw new AuthenticationException(signInResult);
        }

        public static void ThrowIfFailed(this IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
                throw new IdentityException(identityResult);
        }
    }
}

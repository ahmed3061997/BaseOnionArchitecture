using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Common.Exceptions
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(SignInResult result)
        {
            Result = result;
        }

        public SignInResult Result { get; }
    }
}

using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Common.Exceptions
{
    public class IdentityException : Exception
    {
        public IdentityException(IdentityResult result)
        {
            Result = result;
        }

        public IdentityResult Result { get; }
    }
}

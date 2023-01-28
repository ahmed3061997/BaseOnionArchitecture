using Microsoft.AspNetCore.Identity;

namespace Application.Common.Exceptions
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

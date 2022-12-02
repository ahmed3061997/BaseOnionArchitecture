using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
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

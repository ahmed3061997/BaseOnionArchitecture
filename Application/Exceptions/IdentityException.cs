using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
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

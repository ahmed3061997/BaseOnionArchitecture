using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;

namespace Application.Common.Extensions
{
    public static class IdentityExtensions
    {
        public static void ThrowIfFailed(this IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
                throw new IdentityException(identityResult);
        }
    }
}

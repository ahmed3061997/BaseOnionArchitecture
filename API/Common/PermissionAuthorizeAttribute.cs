using Application.Common.Extensions;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace API.Common
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public const string POLICY_PREFIX = "Permission";

        public PermissionAuthorizeAttribute(Modules module, Pages page, Operations operation)
        {
            Policy = $"{POLICY_PREFIX}_{module.EnumToString()}.{page.EnumToString()}.{operation.EnumToString()}";
        }
    }
}

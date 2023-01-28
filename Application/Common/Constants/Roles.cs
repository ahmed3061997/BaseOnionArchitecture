using AutoMapper.Internal;

namespace Application.Common.Constants
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Developer = "Developer";

        public static IEnumerable<string> GetRoles()
        {
            return typeof(Roles).GetMembers()
                .Where(m => m.MemberType == System.Reflection.MemberTypes.Field)
                .Select(m => m.GetMemberValue(null))
                .OfType<string>();
        }
    }
}

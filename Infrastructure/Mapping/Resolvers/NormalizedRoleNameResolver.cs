using Application.Models.Users;
using AutoMapper;
using Persistence.Identity;

namespace Infrastructure.Mapping.Resolvers
{
    public class NormalizedRoleNameResolver : IValueResolver<RoleDto, Role, string?>
    {
        public NormalizedRoleNameResolver()
        {
        }

        public string? Resolve(RoleDto source, Role destination, string destMember, ResolutionContext context)
        {
            var culture = "en";
            return source.Names?.Where(x => x.Culture == culture).Select(x => x.Value.Replace(" ", "_")).FirstOrDefault();
        }
    }
}

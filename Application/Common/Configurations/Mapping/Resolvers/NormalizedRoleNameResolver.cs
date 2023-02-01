using AutoMapper;
using Application.Models.Users;
using Domain.Entities.Users;

namespace Application.Common.Configurations.Mapping.Resolvers
{
    public class NormalizedRoleNameResolver : IValueResolver<RoleDto, ApplicationRole, string?>
    {
        public NormalizedRoleNameResolver()
        {
        }

        public string? Resolve(RoleDto source, ApplicationRole destination, string destMember, ResolutionContext context)
        {
            var culture = "en";
            return source.Names?.Where(x => x.Culture == culture).Select(x => x.Value.Replace(" ", "_")).FirstOrDefault();
        }
    }
}

using Application.Contracts.Culture;
using Application.Models.Users;
using AutoMapper;
using Persistence.Identity;

namespace Infrastructure.Mapping.Resolvers
{
    public class RoleNameResolver : IValueResolver<Role, RoleDto, string>, IValueResolver<Role, UserRoleDto, string>, IValueResolver<UserRole, UserRoleDto, string>
    {
        private readonly ICurrentCultureService currentCultureService;

        public RoleNameResolver(ICurrentCultureService currentCultureService)
        {
            this.currentCultureService = currentCultureService;
        }

        string IValueResolver<Role, RoleDto, string>.Resolve(Role source, RoleDto destination, string destMember, ResolutionContext context)
        {
            var culture = currentCultureService.GetCurrentUICulture();
            return source.Names?.Where(x => x.Culture == culture).Select(x => x.Name).FirstOrDefault();
        }

        string IValueResolver<Role, UserRoleDto, string>.Resolve(Role source, UserRoleDto destination, string destMember, ResolutionContext context)
        {
            var culture = currentCultureService.GetCurrentUICulture();
            return source.Names?.Where(x => x.Culture == culture).Select(x => x.Name).FirstOrDefault();
        }

        string IValueResolver<UserRole, UserRoleDto, string>.Resolve(UserRole source, UserRoleDto destination, string destMember, ResolutionContext context)
        {
            var culture = currentCultureService.GetCurrentUICulture();
            return source.Role?.Names?.Where(x => x.Culture == culture).Select(x => x.Name).FirstOrDefault();
        }
    }
}

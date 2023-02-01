using AutoMapper;
using Application.Interfaces.Culture;
using Application.Models.Users;
using Domain.Entities.Users;

namespace Application.Common.Configurations.Mapping.Resolvers
{
    public class RoleNameResolver : IValueResolver<ApplicationRole, RoleDto, string>
    {
        private readonly ICurrentCultureService currentCultureService;

        public RoleNameResolver(ICurrentCultureService currentCultureService)
        {
            this.currentCultureService = currentCultureService;
        }

        public string Resolve(ApplicationRole source, RoleDto destination, string destMember, ResolutionContext context)
        {
            var culture = currentCultureService.GetCurrentUICulture();
            return source.Names?.Where(x => x.Culture == culture).Select(x => x.Name).FirstOrDefault();
        }
    }
}

using AutoMapper;
using Application.Interfaces.Culture;
using Application.Models.System;
using Domain.Entities.System;

namespace Application.Common.Configurations.Mapping.Resolvers
{
    public class ModuleNameResolver : IValueResolver<Module, ModuleDto, string>
    {
        private readonly ICurrentCultureService currentCultureService;

        public ModuleNameResolver(ICurrentCultureService currentCultureService)
        {
            this.currentCultureService = currentCultureService;
        }

        public string Resolve(Module source, ModuleDto destination, string destMember, ResolutionContext context)
        {
            var culture = currentCultureService.GetCurrentUICulture();
            return source.Names?.Where(x => x.Culture == culture).Select(x => x.Name).FirstOrDefault();
        }
    }
}

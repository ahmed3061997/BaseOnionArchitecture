using Application.Contracts.Culture;
using Application.Models.System;
using AutoMapper;
using Domain.Entities.System;

namespace Application.Mapping.Resolvers
{
    public class OperationNameResolver : IValueResolver<Operation, OperationDto, string>
    {
        private readonly ICurrentCultureService currentCultureService;

        public OperationNameResolver(ICurrentCultureService currentCultureService)
        {
            this.currentCultureService = currentCultureService;
        }

        public string Resolve(Operation source, OperationDto destination, string destMember, ResolutionContext context)
        {
            var culture = currentCultureService.GetCurrentUICulture();
            return source.Names?.Where(x => x.Culture == culture).Select(x => x.Name).FirstOrDefault();
        }
    }
}

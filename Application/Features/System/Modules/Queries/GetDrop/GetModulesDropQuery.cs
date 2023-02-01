using Application.Interfaces.System;
using Application.Models.System;
using MediatR;

namespace Application.Features.System.Modules.Queries.GetDrop
{
    public class GetModulesDropQuery : IRequest<IEnumerable<ModuleDto>>
    {
    }

    public class GetModulesDropQueryHandler : IRequestHandler<GetModulesDropQuery, IEnumerable<ModuleDto>>
    {
        private readonly IModuleService service;

        public GetModulesDropQueryHandler(IModuleService service)
        {
            this.service = service;

        }

        public async Task<IEnumerable<ModuleDto>> Handle(GetModulesDropQuery request, CancellationToken cancellationToken)
        {
            return await service.GetDrop();
        }
    }
}

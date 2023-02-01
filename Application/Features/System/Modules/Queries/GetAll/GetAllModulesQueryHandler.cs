using Application.Interfaces.System;
using Application.Models.Common;
using Application.Models.System;
using MediatR;

namespace Application.Features.System.Modules.Queries.GetAll
{
    public class GetAllModulesQuery : IRequest<IEnumerable<ModuleDto>>
    {
        public DataTableParameters DataTableParameters { get; set; }
    }

    public class GetAllModulesQueryHandler : IRequestHandler<GetAllModulesQuery, IEnumerable<ModuleDto>>
    {
        private readonly IModuleService service;

        public GetAllModulesQueryHandler(IModuleService service)
        {
            this.service = service;
        }

        public async Task<IEnumerable<ModuleDto>> Handle(GetAllModulesQuery request, CancellationToken cancellationToken)
        {
            return await service.GetAll();
        }
    }
}

using Application.Interfaces.System;
using Application.Models.System;
using MediatR;

namespace Application.Features.System.Modules.Queries.GetById
{
    public class GetModuleByIdQuery : IRequest<ModuleDto>
    {
        public Guid Id { get; set; }
    }

    public class GetModuleByIdQueryHandler : IRequestHandler<GetModuleByIdQuery, ModuleDto>
    {
        private readonly IModuleService service;

        public GetModuleByIdQueryHandler(IModuleService service)
        {
            this.service = service;
        }

        public async Task<ModuleDto> Handle(GetModuleByIdQuery request, CancellationToken cancellationToken)
        {
            return await service.Get(request.Id);
        }
    }
}

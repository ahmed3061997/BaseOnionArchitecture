using Application.Interfaces.System;
using Application.Models.Common;
using Application.Models.System;
using MediatR;

namespace Application.Features.System.Operations.Queries.GetAll
{
    public class GetAllOperationsQuery : IRequest<IEnumerable<OperationDto>>
    {
        public DataTableParameters DataTableParameters { get; set; }
    }

    public class GetAllOperationsQueryHandler : IRequestHandler<GetAllOperationsQuery, IEnumerable<OperationDto>>
    {
        private readonly IOperationService service;

        public GetAllOperationsQueryHandler(IOperationService service)
        {
            this.service = service;
        }

        public async Task<IEnumerable<OperationDto>> Handle(GetAllOperationsQuery request, CancellationToken cancellationToken)
        {
            return await service.GetAll();
        }
    }
}

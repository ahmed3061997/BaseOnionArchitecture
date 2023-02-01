using Application.Interfaces.System;
using Application.Models.System;
using MediatR;

namespace Application.Features.System.Operations.Queries.GetDrop
{
    public class GetOperationsDropQuery : IRequest<IEnumerable<OperationDto>>
    {
    }

    public class GetOperationsDropQueryHandler : IRequestHandler<GetOperationsDropQuery, IEnumerable<OperationDto>>
    {
        private readonly IOperationService service;

        public GetOperationsDropQueryHandler(IOperationService service)
        {
            this.service = service;

        }

        public async Task<IEnumerable<OperationDto>> Handle(GetOperationsDropQuery request, CancellationToken cancellationToken)
        {
            return await service.GetDrop();
        }
    }
}

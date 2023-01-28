using Application.Interfaces.System;
using Application.Models.System;
using MediatR;

namespace Application.Features.System.Operations.Queries.GetById
{
    public class GetOperationByIdQuery : IRequest<OperationDto>
    {
        public Guid Id { get; set; }
    }

    public class GetOperationByIdQueryHandler : IRequestHandler<GetOperationByIdQuery, OperationDto>
    {
        private readonly IOperationService service;

        public GetOperationByIdQueryHandler(IOperationService service)
        {
            this.service = service;
        }

        public async Task<OperationDto> Handle(GetOperationByIdQuery request, CancellationToken cancellationToken)
        {
            return await service.Get(request.Id);
        }
    }
}

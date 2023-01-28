using Application.Interfaces.System;
using FluentValidation;
using Application.Common.Responses;
using MediatR;

namespace Application.Features.System.Operations.Commands.Delete
{
    public class DeleteOperationCommand : IRequest<IResponse>
    {
        public Guid Id { get; set; }

        public class DeleteOperationCommandValidator : AbstractValidator<DeleteOperationCommand>
        {
            public DeleteOperationCommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }
    }

    public class DeleteOperationCommandHandler : IRequestHandler<DeleteOperationCommand, IResponse>
    {
        private readonly IOperationService service;

        public DeleteOperationCommandHandler(IOperationService service)
        {
            this.service = service;
        }

        public async Task<IResponse> Handle(DeleteOperationCommand request, CancellationToken cancellationToken)
        {
            await service.Delete(request.Id);
            return new Response() { Result = true };
        }
    }

}

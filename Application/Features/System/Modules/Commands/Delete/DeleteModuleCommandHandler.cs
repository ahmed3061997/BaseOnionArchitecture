using Application.Interfaces.System;
using FluentValidation;
using Application.Common.Responses;
using MediatR;

namespace Application.Features.System.Modules.Commands.Delete
{
    public class DeleteModuleCommand : IRequest<IResponse>
    {
        public Guid Id { get; set; }

        public class DeleteModuleCommandValidator : AbstractValidator<DeleteModuleCommand>
        {
            public DeleteModuleCommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }
    }

    public class DeleteModuleCommandHandler : IRequestHandler<DeleteModuleCommand, IResponse>
    {
        private readonly IModuleService service;

        public DeleteModuleCommandHandler(IModuleService service)
        {
            this.service = service;
        }

        public async Task<IResponse> Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
        {
            await service.Delete(request.Id);
            return new Response() { Result = true };
        }
    }

}

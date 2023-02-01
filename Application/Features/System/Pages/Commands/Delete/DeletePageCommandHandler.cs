using FluentValidation;
using Application.Common.Responses;
using Application.Interfaces.System;
using MediatR;

namespace Application.Features.System.Pages.Commands.Delete
{
    public class DeletePageCommand : IRequest<IResponse>
    {
        public Guid Id { get; set; }

        public class DeletePageCommandValidator : AbstractValidator<DeletePageCommand>
        {
            public DeletePageCommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }
    }

    public class DeletePageCommandHandler : IRequestHandler<DeletePageCommand, IResponse>
    {
        private readonly IPageService service;

        public DeletePageCommandHandler(IPageService service)
        {
            this.service = service;
        }

        public async Task<IResponse> Handle(DeletePageCommand request, CancellationToken cancellationToken)
        {
            await service.Delete(request.Id);
            return new Response() { Result = true };
        }
    }

}

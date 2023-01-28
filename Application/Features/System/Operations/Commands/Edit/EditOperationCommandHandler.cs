using Application.Interfaces.System;
using Application.Models.Common;
using Application.Models.System;
using AutoMapper;
using FluentValidation;
using Application.Common.Responses;
using MediatR;

namespace Application.Features.System.Operations.Commands.Edit
{
    public class EditOperationCommand : IRequest<IResponse>
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public IEnumerable<CultureLookupDto> Names { get; set; }

        public class EditOperationCommandValidator : AbstractValidator<EditOperationCommand>
        {
            public EditOperationCommandValidator()
            {
                RuleFor(x => x.Code).NotEmpty();
                RuleFor(x => x.Names).NotEmpty();
                RuleForEach(x => x.Names).NotEmpty();
            }
        }
    }

    public class EditOperationCommandHandler : IRequestHandler<EditOperationCommand, IResponse>
    {
        private readonly IOperationService service;
        private readonly IMapper mapper;

        public EditOperationCommandHandler(IOperationService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResponse> Handle(EditOperationCommand request, CancellationToken cancellationToken)
        {
            await service.Edit(mapper.Map<OperationDto>(request));
            return new Response() { Result = true };
        }
    }
}

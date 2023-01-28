using Application.Interfaces.System;
using Application.Models.Common;
using Application.Models.System;
using AutoMapper;
using FluentValidation;
using Application.Common.Responses;
using MediatR;

namespace Application.Features.System.Operations.Commands.Create
{
    public class CreateOperationCommand : IRequest<IResponse>
    {
        public int Code { get; set; }
        public IEnumerable<CultureLookupDto> Names { get; set; }

        public class CreateOperationCommandValidator : AbstractValidator<CreateOperationCommand>
        {
            public CreateOperationCommandValidator()
            {
                RuleFor(x => x.Code).NotEmpty();
                RuleFor(x => x.Names).NotEmpty();
                RuleForEach(x => x.Names).NotEmpty();
            }
        }
    }

    public class CreateOperationCommandHandler : IRequestHandler<CreateOperationCommand, IResponse>
    {
        private readonly IOperationService service;
        private readonly IMapper mapper;

        public CreateOperationCommandHandler(IOperationService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResponse> Handle(CreateOperationCommand request, CancellationToken cancellationToken)
        {
            await service.Create(mapper.Map<OperationDto>(request));
            return new Response() { Result = true };
        }
    }
}

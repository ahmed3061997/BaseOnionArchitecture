using AutoMapper;
using FluentValidation;
using Application.Common.Responses;
using Application.Interfaces.System;
using Application.Models.Common;
using Application.Models.System;
using MediatR;

namespace Application.Features.System.Modules.Commands.Create
{
    public class CreateModuleCommand : IRequest<IResponse>
    {
        public int Code { get; set; }
        public IEnumerable<CultureLookupDto> Names { get; set; }

        public class CreateModuleCommandValidator : AbstractValidator<CreateModuleCommand>
        {
            public CreateModuleCommandValidator()
            {
                RuleFor(x => x.Code).NotEmpty();
                RuleFor(x => x.Names).NotEmpty();
                RuleForEach(x => x.Names).NotEmpty();
            }
        }
    }

    public class CreateModuleCommandHandler : IRequestHandler<CreateModuleCommand, IResponse>
    {
        private readonly IModuleService service;
        private readonly IMapper mapper;

        public CreateModuleCommandHandler(IModuleService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResponse> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
        {
            await service.Create(mapper.Map<ModuleDto>(request));
            return new Response() { Result = true };
        }
    }
}

using AutoMapper;
using FluentValidation;
using Application.Common.Responses;
using Application.Interfaces.System;
using Application.Models.Common;
using Application.Models.System;
using MediatR;

namespace Application.Features.System.Modules.Commands.Edit
{
    public class EditModuleCommand : IRequest<IResponse>
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public IEnumerable<CultureLookupDto> Names { get; set; }

        public class EditModuleCommandValidator : AbstractValidator<EditModuleCommand>
        {
            public EditModuleCommandValidator()
            {
                RuleFor(x => x.Code).NotEmpty();
                RuleFor(x => x.Names).NotEmpty();
                RuleForEach(x => x.Names).NotEmpty();
            }
        }
    }

    public class EditModuleCommandHandler : IRequestHandler<EditModuleCommand, IResponse>
    {
        private readonly IModuleService service;
        private readonly IMapper mapper;

        public EditModuleCommandHandler(IModuleService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResponse> Handle(EditModuleCommand request, CancellationToken cancellationToken)
        {
            await service.Edit(mapper.Map<ModuleDto>(request));
            return new Response() { Result = true };
        }
    }
}

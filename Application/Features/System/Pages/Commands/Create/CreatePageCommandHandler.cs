using AutoMapper;
using FluentValidation;
using Application.Common.Responses;
using Application.Interfaces.System;
using Application.Models.Common;
using Application.Models.System;
using MediatR;

namespace Application.Features.System.Pages.Commands.Create
{
    public class CreatePageCommand : IRequest<IResponse>
    {
        public Guid Id { get; set; }
        public Guid ModuleId { get; set; }
        public int Code { get; set; }
        public string Url { get; set; }
        public IEnumerable<CultureLookupDto> Names { get; set; }
        public IEnumerable<PageOperationDto> Operations { get; set; }

        public class CreatePageCommandValidator : AbstractValidator<CreatePageCommand>
        {
            public CreatePageCommandValidator()
            {
                RuleFor(x => x.Code).NotEmpty();
                RuleFor(x => x.Names).NotEmpty();
                RuleFor(x => x.Url).NotEmpty();
                RuleFor(x => x.Operations).NotEmpty();
                RuleForEach(x => x.Names).NotEmpty();
            }
        }
    }

    public class CreatePageCommandHandler : IRequestHandler<CreatePageCommand, IResponse>
    {
        private readonly IPageService service;
        private readonly IMapper mapper;

        public CreatePageCommandHandler(IPageService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResponse> Handle(CreatePageCommand request, CancellationToken cancellationToken)
        {
            await service.Create(mapper.Map<PageDto>(request));
            return new Response() { Result = true };
        }
    }
}

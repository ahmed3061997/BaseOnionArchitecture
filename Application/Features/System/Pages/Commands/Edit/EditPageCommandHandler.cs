using AutoMapper;
using FluentValidation;
using Application.Common.Responses;
using Application.Interfaces.System;
using Application.Models.Common;
using Application.Models.System;
using MediatR;

namespace Application.Features.System.Pages.Commands.Edit
{
    public class EditPageCommand : IRequest<IResponse>
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public IEnumerable<CultureLookupDto> Names { get; set; }

        public class EditPageCommandValidator : AbstractValidator<EditPageCommand>
        {
            public EditPageCommandValidator()
            {
                RuleFor(x => x.Code).NotEmpty();
                RuleFor(x => x.Names).NotEmpty();
                RuleForEach(x => x.Names).NotEmpty();
            }
        }
    }

    public class EditPageCommandHandler : IRequestHandler<EditPageCommand, IResponse>
    {
        private readonly IPageService service;
        private readonly IMapper mapper;

        public EditPageCommandHandler(IPageService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResponse> Handle(EditPageCommand request, CancellationToken cancellationToken)
        {
            await service.Edit(mapper.Map<PageDto>(request));
            return new Response() { Result = true };
        }
    }
}

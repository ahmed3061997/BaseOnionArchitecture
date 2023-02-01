using Application.Interfaces.System;
using Application.Models.System;
using MediatR;

namespace Application.Features.System.Pages.Queries.GetById
{
    public class GetPageByIdQuery : IRequest<PageDto>
    {
        public Guid Id { get; set; }
    }

    public class GetPageByIdQueryHandler : IRequestHandler<GetPageByIdQuery, PageDto>
    {
        private readonly IPageService service;

        public GetPageByIdQueryHandler(IPageService service)
        {
            this.service = service;
        }

        public async Task<PageDto> Handle(GetPageByIdQuery request, CancellationToken cancellationToken)
        {
            return await service.Get(request.Id);
        }
    }
}

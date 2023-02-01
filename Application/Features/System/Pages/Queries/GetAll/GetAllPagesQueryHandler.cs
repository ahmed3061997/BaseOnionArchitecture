using Application.Interfaces.System;
using Application.Models.Common;
using Application.Models.System;
using MediatR;

namespace Application.Features.System.Pages.Queries.GetAll
{
    public class GetAllPagesQuery : IRequest<IEnumerable<PageDto>>
    {
        public DataTableParameters DataTableParameters { get; set; }
    }

    public class GetAllPagesQueryHandler : IRequestHandler<GetAllPagesQuery, IEnumerable<PageDto>>
    {
        private readonly IPageService service;

        public GetAllPagesQueryHandler(IPageService service)
        {
            this.service = service;
        }

        public async Task<IEnumerable<PageDto>> Handle(GetAllPagesQuery request, CancellationToken cancellationToken)
        {
            return await service.GetAll();
        }
    }
}

using MediatR;
using Application.Common.Responses;
using Application.Interfaces.Users;
using Application.Models.Users;

namespace Application.Features.Users.Queries.GetCurrentUser
{
    public class GetCurrentUserQuery : IRequest<IResultResponse<CurrentUserDto>>
    {
    }

    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, IResultResponse<CurrentUserDto>>
    {
        private readonly ICurrentUserService currentUserService;

        public GetCurrentUserQueryHandler(ICurrentUserService currentUserService)
        {
            this.currentUserService = currentUserService;
        }

        public async Task<IResultResponse<CurrentUserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            return new ResultResponse<CurrentUserDto>() { Result = true, Value = await currentUserService.GetCurrentUser() };
        }
    }
}

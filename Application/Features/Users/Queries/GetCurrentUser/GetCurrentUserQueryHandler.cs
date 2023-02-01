using Application.Common.Responses;
using Application.Interfaces.Users;
using Application.Models.Users;
using MediatR;

namespace Application.Features.Users.Queries.GetCurrentUser
{
    public class GetCurrentUserQuery : IRequest<IResultResponse<UserDto>>
    {
    }

    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, IResultResponse<UserDto>>
    {
        private readonly ICurrentUserService currentUserService;

        public GetCurrentUserQueryHandler(ICurrentUserService currentUserService)
        {
            this.currentUserService = currentUserService;
        }

        public async Task<IResultResponse<UserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            return new ResultResponse<UserDto>() { Result = true, Value = await currentUserService.GetCurrentUser() };
        }
    }
}

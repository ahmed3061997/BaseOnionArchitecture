using MediatR;
using Application.Common.Responses;
using Application.Interfaces.Users;

namespace Application.Features.Users.Commands.AssignToRole
{
    public class AssignToRoleCommandHandler : IRequestHandler<AssignToRoleCommand, IResponse>
    {
        private readonly IUserService userService;

        public AssignToRoleCommandHandler(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IResponse> Handle(AssignToRoleCommand request, CancellationToken cancellationToken)
        {
            await userService.AssignToRole(request.UserId, request.Role);
            return new Response() { Result = true };
        }
    }
}

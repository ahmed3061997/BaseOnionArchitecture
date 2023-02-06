using Application.Common.Responses;
using Application.Interfaces.Users;
using MediatR;

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
            await userService.AssignToRoles(request.UserId, request.Roles);
            return new Response() { Result = true };
        }
    }
}

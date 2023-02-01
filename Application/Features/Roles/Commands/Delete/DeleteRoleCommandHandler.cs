using Application.Common.Responses;
using Application.Interfaces.Users;
using MediatR;

namespace Application.Features.Roles.Commands.Delete
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, IResponse>
    {
        private readonly IRoleService service;

        public DeleteRoleCommandHandler(IRoleService service)
        {
            this.service = service;
        }

        public async Task<IResponse> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            await service.Delete(request.Id);
            return new Response() { Result = true };
        }
    }
}

using Application.Interfaces.Users;
using Application.Models.Users;
using MediatR;

namespace Application.Features.Roles.Queries.GetDrop
{
    public class GetRolesDropQuery : IRequest<IEnumerable<RoleDto>>
    {
    }

    public class GetRolesDropQueryHandler : IRequestHandler<GetRolesDropQuery, IEnumerable<RoleDto>>
    {
        private readonly IRoleService service;

        public GetRolesDropQueryHandler(IRoleService service)
        {
            this.service = service;

        }

        public async Task<IEnumerable<RoleDto>> Handle(GetRolesDropQuery request, CancellationToken cancellationToken)
        {
            return await service.GetDrop();
        }
    }
}

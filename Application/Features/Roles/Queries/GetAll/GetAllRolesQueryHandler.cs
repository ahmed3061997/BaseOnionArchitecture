using Application.Interfaces.Users;
using Application.Models.Common;
using Application.Models.Users;
using MediatR;

namespace Application.Features.Roles.Queries.GetAll
{
    public class GetAllRolesQuery : IRequest<IEnumerable<RoleDto>>
    {
        public DataTableParameters DataTableParameters { get; set; }
    }

    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
    {
        private readonly IRoleService service;

        public GetAllRolesQueryHandler(IRoleService service)
        {
            this.service = service;
        }

        public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            return await service.GetAll();
        }
    }
}

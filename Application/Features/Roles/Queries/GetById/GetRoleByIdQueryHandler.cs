using Application.Interfaces.Users;
using Application.Models.Users;
using MediatR;

namespace Application.Features.Roles.Queries.GetById
{
    public class GetRoleByIdQuery : IRequest<RoleDto>
    {
        public string Id { get; set; }
    }

    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto>
    {
        private readonly IRoleService service;

        public GetRoleByIdQueryHandler(IRoleService service)
        {
            this.service = service;
        }

        public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            return await service.Get(request.Id);
        }
    }
}
